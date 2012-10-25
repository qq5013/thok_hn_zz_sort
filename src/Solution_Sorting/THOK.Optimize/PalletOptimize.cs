using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace THOK.Optimize
{
    public class PalletOptimize
    {
        private int palletNo = 0;
        private int sortNo = 0;
        private string lineCode;
        private DateTime orderDate;
        private string batchNo;

        private int channelBlankCount = 0;
        private int towerBlankCount = 0;

        private int channelPallet = 0;
        private int towerPallet = 0;

        /// <summary>
        /// 生成单主线托盘计划
        /// </summary>
        /// <param name="masterRow"></param>
        /// <param name="orderTable"></param>
        /// <param name="channelTable"></param>
        /// <returns></returns>
        public DataTable SingleLineOptimize(DataRow masterRow, DataRow[] orderRows, DataTable channelTable, int channelBlankCount, int towerBlankCount)
        {
            DataTable detailTable = GetEmptyPalletDetail();
            palletNo = 0;
            sortNo = Convert.ToInt32(masterRow["SORTNO"]);
            lineCode = masterRow["LINECODE"].ToString();
            orderDate = Convert.ToDateTime(masterRow["ORDERDATE"]);
            batchNo = masterRow["BATCHNO"].ToString();

            this.channelBlankCount = channelBlankCount;
            this.towerBlankCount = towerBlankCount;

            channelPallet = 0;
            towerPallet = 0;
            masterRow["QUANTITY"] = 0; 
            foreach (DataRow orderRow in orderRows)
            {
                DataRow channelRow = channelTable.Select(string.Format("CHANNELCODE = '{0}'", orderRow["CHANNELCODE"]))[0];
                string channelType = channelRow["CHANNELTYPE"].ToString();
                switch (channelType)
                {
                    case "1": //整5通道机
                        SetChannelPallet(orderRow, channelRow, detailTable);
                        break;
                    case "2": //立式机
                        SetVerticalChannelPallet(orderRow, channelRow, detailTable);
                        break;
                    case "3": //零5通道机
                        SetChannelPallet(orderRow, channelRow, detailTable);
                        break;
                    case "4": //卧式机
                        SetHorizontalChannelPallet(orderRow, channelRow, detailTable);
                        break;
                }
                masterRow["QUANTITY"] = (int)masterRow["QUANTITY"] + (int)orderRow["QUANTITY"]; 
            }

            masterRow["PQUANTITY"] = channelPallet * 2 + towerPallet;
            return detailTable;
        }

        /// <summary>
        /// 通道机托盘分配
        /// </summary>
        /// <param name="orderRow"></param>
        /// <param name="channelRow"></param>
        /// <param name="detailTable"></param>
        private void SetChannelPallet(DataRow orderRow, DataRow channelRow, DataTable detailTable)
        {
                      
            int orderQuantity = Convert.ToInt32(orderRow["QUANTITY"]);
            int remainQuantity = Convert.ToInt32(channelRow["REMAINQUANTITY"]);

            int currentQuantity = 0;

            while (orderQuantity != 0)
            {
                int quantity = remainQuantity % 5;
                if (quantity == 0)
                    quantity = 5;

                if (quantity >= orderQuantity)
                {
                    quantity -= orderQuantity;
                    currentQuantity = orderQuantity;
                    orderQuantity = 0;
                }
                else
                {
                    orderQuantity -= quantity;
                    currentQuantity = quantity;

                }

                //添加进托盘序列
                //如果之前有空托盘则添加到空托盘位置，如果之前没有空托盘则添加到最后
                //之前有空托盘的条件是：选取托盘烟道代码不等于当前代码且数量为０的托盘
                AddPallet(detailTable, channelRow, orderRow, currentQuantity);

                remainQuantity -= currentQuantity;

                if (remainQuantity % 25 == 0 && orderQuantity > 0)
                {
                    //添加空托盘
                    for (int i = 0; i < channelBlankCount; i++)
                    {
                        AddPallet(detailTable, channelRow, orderRow, 0);
                    }

                }

                if (remainQuantity == 0)
                {
                    //补货点
                    //channelRow["SUPPLYQUANTITY"] = 1;
                    remainQuantity = 50;
                }
            }

            channelRow["REMAINQUANTITY"] = remainQuantity;

        }

        /// <summary>
        /// 立式机托盘分配
        /// </summary>
        /// <param name="orderRow"></param>
        /// <param name="channelRow"></param>
        /// <param name="detailTable"></param>
        private void SetVerticalChannelPallet(DataRow orderRow, DataRow channelRow, DataTable detailTable)
        {
            int quantity = Convert.ToInt32(orderRow["QUANTITY"]);
            
            for (int i = 0; i < quantity; i++)
            {
                AddPallet(detailTable, channelRow, orderRow, 1);
            }
        }

        /// <summary>
        /// 卧式机托盘分配
        /// </summary>
        /// <param name="orderRow"></param>
        /// <param name="channelRow"></param>
        /// <param name="detailTable"></param>
        private void SetHorizontalChannelPallet(DataRow orderRow, DataRow channelRow, DataTable detailTable)
        {
            int orderQuantity = Convert.ToInt32(orderRow["QUANTITY"]);
            int remainQuantity = Convert.ToInt32(channelRow["REMAINQUANTITY"]);

            int currentQuantity = 0;

            while (orderQuantity != 0)
            {
                int quantity = remainQuantity % 5;
                if (quantity == 0)
                {
                    quantity = 5;
                    //添加空托盘,根据参数设置添加 N 个空托盘
                    for (int i = 0; i < towerBlankCount; i++)
                    {
                        AddPallet(detailTable, channelRow, orderRow, 0);
                    }
                }

                if (quantity >= orderQuantity)
                {
                    quantity -= orderQuantity;
                    currentQuantity = orderQuantity;
                    orderQuantity = 0;
                }
                else
                {
                    orderQuantity -= quantity;
                    currentQuantity = quantity;
                }


                for (int i = 0; i < currentQuantity; i++)
                {
                    AddPallet(detailTable, channelRow, orderRow, 1);
                }

                remainQuantity -= currentQuantity;

                if (remainQuantity == 0)
                {
                    //补货点
                    channelRow["SUPPLYQUANTITY"] = 1;
                    remainQuantity = 50;
                }
            }

        }

        private void AddPallet(DataTable palletDetail, DataRow channelRow, DataRow orderRow, int quantity)
        {
            DataRow[] rows = palletDetail.Select(string.Format("CIGARETTECODE <> '{0}' AND QUANTITY = 0", orderRow["CIGARETTECODE"]), "PALLETNO");
            if (rows.Length != 0)
            {
                rows[0]["CHANNELCODE"] = channelRow["CHANNELCODE"];
                rows[0]["CIGARETTECODE"] = orderRow["CIGARETTECODE"];
                rows[0]["CIGARETTENAME"] = orderRow["CIGARETTENAME"];
                rows[0]["QUANTITY"] = quantity;
            }
            else
            {
                string channelType = channelRow["CHANNELTYPE"].ToString();
                if (channelType.Equals("1") || channelType.Equals("3"))
                    channelPallet++;
                else
                    towerPallet++;

                DataRow detailRow = palletDetail.NewRow();
                detailRow["SORTNO"] = sortNo;
                detailRow["PALLETNO"] = ++palletNo;
                detailRow["ORDERDATE"] = orderDate;
                detailRow["BATCHNO"] = batchNo.Trim();
                detailRow["LINECODE"] = lineCode;
                if (quantity != 0)
                {
                    detailRow["CHANNELCODE"] = channelRow["CHANNELCODE"];
                    detailRow["CIGARETTENAME"] = orderRow["CIGARETTENAME"];
                }
                detailRow["CIGARETTECODE"] = orderRow["CIGARETTECODE"];
                detailRow["QUANTITY"] = quantity;
                palletDetail.Rows.Add(detailRow);

            }
        }

        private DataTable GetEmptyPalletDetail()
        {
            DataTable table = new DataTable();
            table.Columns.Add("SORTNO", typeof(int));
            table.Columns.Add("PALLETNO", typeof(int));
            table.Columns.Add("LINECODE");
            table.Columns.Add("BATCHNO");
            table.Columns.Add("ORDERDATE");
            table.Columns.Add("CHANNELCODE");
            table.Columns.Add("CIGARETTECODE");
            table.Columns.Add("CIGARETTENAME");
            table.Columns.Add("QUANTITY", typeof(int));

            return table;
        }
    }
}
