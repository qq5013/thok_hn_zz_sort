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
        /// ���ɵ��������̼ƻ�
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
                    case "1": //��5ͨ����
                        SetChannelPallet(orderRow, channelRow, detailTable);
                        break;
                    case "2": //��ʽ��
                        SetVerticalChannelPallet(orderRow, channelRow, detailTable);
                        break;
                    case "3": //��5ͨ����
                        SetChannelPallet(orderRow, channelRow, detailTable);
                        break;
                    case "4": //��ʽ��
                        SetHorizontalChannelPallet(orderRow, channelRow, detailTable);
                        break;
                }
                masterRow["QUANTITY"] = (int)masterRow["QUANTITY"] + (int)orderRow["QUANTITY"]; 
            }

            masterRow["PQUANTITY"] = channelPallet * 2 + towerPallet;
            return detailTable;
        }

        /// <summary>
        /// ͨ�������̷���
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

                //��ӽ���������
                //���֮ǰ�п���������ӵ�������λ�ã����֮ǰû�п���������ӵ����
                //֮ǰ�п����̵������ǣ�ѡȡ�����̵����벻���ڵ�ǰ����������Ϊ��������
                AddPallet(detailTable, channelRow, orderRow, currentQuantity);

                remainQuantity -= currentQuantity;

                if (remainQuantity % 25 == 0 && orderQuantity > 0)
                {
                    //��ӿ�����
                    for (int i = 0; i < channelBlankCount; i++)
                    {
                        AddPallet(detailTable, channelRow, orderRow, 0);
                    }

                }

                if (remainQuantity == 0)
                {
                    //������
                    //channelRow["SUPPLYQUANTITY"] = 1;
                    remainQuantity = 50;
                }
            }

            channelRow["REMAINQUANTITY"] = remainQuantity;

        }

        /// <summary>
        /// ��ʽ�����̷���
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
        /// ��ʽ�����̷���
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
                    //��ӿ�����,���ݲ���������� N ��������
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
                    //������
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
