using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace THOK.Optimize
{
    public class ChannelOptimize
    {
        private int splitQuantity = 0;
        private int splitCount = 2;
        public void Optimize(DataTable orderTable, DataTable channelTable, DataTable deviceTable)
        {
            DataTable tmpTable = null;

            //固定烟道分配
            foreach (DataRow deviceRow in deviceTable.Rows)
            {
                string channelType = deviceRow["CHANNELTYPE"].ToString();
                switch (channelType)
                {
                    case "2":
                        int groupCount = Convert.ToInt32(deviceRow["GROUPCOUNT"]);
                        splitQuantity = Convert.ToInt32(deviceRow["CHANNELCOUNT"]) - Convert.ToInt32(deviceRow["SORTCOUNT"]);
                        splitCount = Convert.ToInt32(deviceRow["SPLITCOUNT"]);
                        tmpTable = GenerateTmpTable(groupCount);
                        SetGroup(channelTable, groupCount);
                        SetFixedTowerChannel(orderTable, channelTable, tmpTable, channelType);
                        break;
                    case "3":
                        SetFixedChannel(orderTable, channelTable, channelType);
                        break;
                }
            }

            //非固定烟道分配
            foreach (DataRow deviceRow in deviceTable.Rows)
            {
                string channelType = deviceRow["CHANNELTYPE"].ToString();
                
                switch (channelType)
                {
                    case "2": //立式机 
                        SetTowerChannel(orderTable, channelTable, tmpTable, channelType);
                        break;

                    case "3": //零5通道机
                        SetChannel(orderTable, channelTable, channelType);
                        break;
                }
            }
        }


        /// <summary>
        /// 将塔式机烟道分配到不同的补货区
        /// </summary>
        /// <param name="channelSet"></param>
        /// <param name="groupNum"></param>
        private void SetGroup(DataTable channelTable, int groupNum)
        {
            DataRow[] channelRows = channelTable.Select("CHANNELTYPE='2' AND STATUS='1'", "CHANNELCODE");
            int groupCount = channelRows.Length / groupNum;
            int i = 0;
            int maxGroupNum = groupNum - 1;

            foreach (DataRow channelRow in channelRows)
            {
                int j = i++ / groupCount;

                if (j < maxGroupNum)
                {
                    channelRow["GROUPNO"] = j;
                }
                else
                {
                    channelRow["GROUPNO"] = maxGroupNum;
                }
            }
        }

        private void SetFixedChannel(DataTable orderTable, DataTable channelTable, string channelType)
        {
            //固定通道机品牌
            foreach (DataRow orderRow in orderTable.Rows)
            {
                //取当前品牌固定通道机烟道
                DataRow[] channelRows = channelTable.Select(string.Format("CHANNELTYPE = '{0}' AND CIGARETTECODE = '{1}'  AND STATUS='1'", channelType, orderRow["CIGARETTECODE"]), "CHANNELCODE");
                int quantity = Convert.ToInt32(orderRow["QUANTITY"]);  //当前品牌订单数量

                if (channelRows.Length > 1)//占用多少通道机
                {
                    int tmpQuantity = quantity / channelRows.Length / 50 * 50;

                    for (int i = 0; i < channelRows.Length; i++)
                    {
                        channelRows[i]["CIGARETTECODE"] = orderRow["CIGARETTECODE"];
                        channelRows[i]["CIGARETTENAME"] = orderRow["CIGARETTENAME"];
                        if (i == channelRows.Length - 1)
                            channelRows[i]["QUANTITY"] = quantity - tmpQuantity * (channelRows.Length - 1);
                        else
                            channelRows[i]["QUANTITY"] = tmpQuantity;
                    }
                    //计算当前品牌剩余量
                    orderRow["QUANTITY"] = 0;
                    
                }
                else if (channelRows.Length == 1) //只占用一个通道机
                {
                    channelRows[0]["CIGARETTECODE"] = orderRow["CIGARETTECODE"];
                    channelRows[0]["CIGARETTENAME"] = orderRow["CIGARETTENAME"];
                    channelRows[0]["QUANTITY"] = quantity;
                    orderRow["QUANTITY"] = 0;
                }

            }
        }

        /// <summary>
        /// 设置通道机卷烟
        /// </summary>
        /// <param name="orderTable"></param>
        /// <param name="channelTable"></param>
        private void SetChannel(DataTable orderTable, DataTable channelTable, string channelType)
        {
            
            //非固定通道机品牌
            DataRow[] orderRows = orderTable.Select("QUANTITY > 0", "QUANTITY DESC");
            for (int j = 0; j < orderRows.Length; j++)
            {
                DataRow orderRow = orderRows[j];
                //取未被占用通道机烟道
                DataRow[] channelRows = channelTable.Select(string.Format("CHANNELTYPE = '{0}' AND CIGARETTECODE=''  AND STATUS='1'", channelType), "CHANNELINDEX");

                if (channelRows.Length != 0)
                {
                    int quantity = Convert.ToInt32(orderRow["QUANTITY"]);//当前品牌订单数量
                                      
                    channelRows[0]["CIGARETTECODE"] = orderRow["CIGARETTECODE"];
                    channelRows[0]["CIGARETTENAME"] = orderRow["CIGARETTENAME"];
                    channelRows[0]["QUANTITY"] = quantity;
                    orderRow["QUANTITY"] = 0;
                }
                else
                    break;
            }
        }

        private void SetFixedTowerChannel(DataTable orderTable, DataTable channelTable, DataTable tmpTable, string channelType)
        {
            DataRow[] orderRows = orderTable.Select("", "QUANTITY DESC");

            //固定立式机卷烟品牌
            foreach (DataRow orderRow in orderRows)
            {
                //当前品牌是否是固定烟道
                DataRow[] channelRows = channelTable.Select(string.Format("CHANNELTYPE = '{0}' AND CIGARETTECODE = '{1}'  AND STATUS='1'", channelType, orderRow["CIGARETTECODE"]), "CHANNELCODE");
                int quantity = Convert.ToInt32(orderRow["QUANTITY"]);
                //如果当前品牌存放在1个以上固定烟道
                if (channelRows.Length > 1)
                {
                    //每个烟道平均数据
                    int avgQuantity = quantity / channelRows.Length;
                    int lastQuantity = quantity - avgQuantity * channelRows.Length;

                    for (int i = 0; i < channelRows.Length; i++)
                    {
                        int tmpQuantity = avgQuantity;
                        if (lastQuantity > 0)
                        {
                            tmpQuantity += 1;
                            lastQuantity--;
                        }
                        SetFixedTowerChannel(orderRow, channelRows[i], tmpTable, tmpQuantity);
                    }
                    
                    orderRow["QUANTITY"] = 0;

                    //减少占用多个品牌数
                    splitQuantity--;
                }
                else if (channelRows.Length == 1)
                {
                    //把烟分配到固定烟道
                    SetFixedTowerChannel(orderRow, channelRows[0], tmpTable, quantity);
                    orderRow["QUANTITY"] = 0;
                }
            }
        }

        /// <summary>
        /// 设置立机或塔机烟道
        /// </summary>
        /// <param name="cigaretteSet"></param>
        /// <param name="channelSet"></param>
        /// <param name="tmpTable"></param>
        private void SetTowerChannel(DataTable orderTable, DataTable channelTable, DataTable tmpTable, string channelType)
        {
            
            //非固定塔式机的卷烟品牌
            DataRow[] orderRows = orderTable.Select("QUANTITY > 0", "QUANTITY DESC");

            foreach (DataRow orderRow in orderRows)
            {
                int quantity = Convert.ToInt32(orderRow["QUANTITY"]);
                if (splitQuantity > 0)
                {                    

                    DataRow[] tmpRows = tmpTable.Select("", "QUANTITY");

                    DataRow[] channelRows = channelTable.Select("CIGARETTECODE='' AND CHANNELTYPE='2'  AND STATUS='1' AND GROUPNO = " + tmpRows[0]["GROUPNO"], "CHANNELCODE");
                    if (channelRows.Length < splitCount)//如果本组未使用的烟道不能满足拆分需要，则查找全部
                    {
                        channelRows = channelTable.Select("CIGARETTECODE='' AND CHANNELTYPE='2'  AND STATUS='1'", "CHANNELCODE");
                        if (channelRows.Length < splitCount)//如果所有剩余的未使用烟道不能满足拆分需要，则只占用一个烟道
                        {
                            SetTowerChannel(orderRow, channelTable, tmpTable, quantity);
                            break;
                        }
                    }
                    

                    //如果未使用烟道能满足拆分需要，则进行拆分
                    int avgQuantity = quantity / splitCount;
                    int lastQuantity = quantity - avgQuantity * splitCount;

                    for (int i = 0; i < splitCount; i++)
                    {
                        int tmpQuantity = avgQuantity;
                        if (lastQuantity > 0)
                        {
                            tmpQuantity += 1;
                            lastQuantity--;
                        }

                        channelRows[i]["CIGARETTECODE"] = orderRow["CIGARETTECODE"];
                        channelRows[i]["CIGARETTENAME"] = orderRow["CIGARETTENAME"];
                        channelRows[i]["QUANTITY"] = tmpQuantity;
                        tmpRows = tmpTable.Select(string.Format("GROUPNO = {0}", channelRows[i]["GROUPNO"]), "QUANTITY");
                        tmpRows[0]["QUANTITY"] = Convert.ToInt32(tmpRows[0]["QUANTITY"]) + tmpQuantity;
                    }
                    //减少占用多个烟道品牌数
                    splitQuantity--;
                    
                }
                else
                    SetTowerChannel(orderRow, channelTable, tmpTable, quantity);
            }
        }

        private void SetFixedTowerChannel(DataRow orderRow, DataRow channelRow, DataTable tmpTable, int quantity)
        {
            if (quantity != 0)
            {
                channelRow["CIGARETTECODE"] = orderRow["CIGARETTECODE"];
                channelRow["CIGARETTENAME"] = orderRow["CIGARETTENAME"];
                channelRow["QUANTITY"] = quantity;

                DataRow[] tmpRows = tmpTable.Select(string.Format("GROUPNO = {0}", channelRow["GROUPNO"]), "QUANTITY");
                tmpRows[0]["QUANTITY"] = Convert.ToInt32(tmpRows[0]["QUANTITY"]) + quantity;
            }
        }

        /// <summary>
        /// 分配非固定塔式烟道
        /// </summary>
        /// <param name="orderRow"></param>
        /// <param name="channelTable"></param>
        /// <param name="tmpTable"></param>
        /// <param name="quantity"></param>
        private void SetTowerChannel(DataRow orderRow, DataTable channelTable, DataTable tmpTable, int quantity)
        {
            if (quantity != 0)
            {
                //如果可分配的非固定烟道为0
                if ((int)channelTable.Compute("COUNT(CHANNELCODE)", "CIGARETTECODE='' AND CHANNELTYPE='2' AND STATUS='1'") == 0)
                {
                    if ((int)channelTable.Compute("COUNT(CHANNELCODE)", "CIGARETTECODE='' AND CHANNELTYPE IN ('5') AND STATUS='1'") == 0)
                    {
                        throw new Exception("还有卷烟品牌未分配烟道,请调整系统参数。");
                    }
                    else
                    {
                        return;
                    }
                }

                DataRow[] tmpRows = tmpTable.Select("", "QUANTITY");
                foreach (DataRow tmpRow in tmpRows)
                {
                    DataRow[] channelRows = channelTable.Select("CIGARETTECODE='' AND CHANNELTYPE='2'  AND STATUS='1' AND GROUPNO = " + tmpRow["GROUPNO"], "CHANNELCODE");
                    if (channelRows.Length != 0)
                    {
                        channelRows[0]["CIGARETTECODE"] = orderRow["CIGARETTECODE"];
                        channelRows[0]["CIGARETTENAME"] = orderRow["CIGARETTENAME"];
                        channelRows[0]["QUANTITY"] = quantity;
                        tmpRow["QUANTITY"] = Convert.ToInt32(tmpRow["QUANTITY"]) + quantity;
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// 生成临时表
        /// </summary>
        /// <param name="groupNum"></param>
        /// <returns></returns>
        private DataTable GenerateTmpTable(int groupNum)
        {
            DataTable table = new DataTable();
            table.Columns.Add("GROUPNO");
            table.Columns.Add("QUANTITY", typeof(Int32));
            for (int i = 0; i < groupNum; i++)
            {
                table.Rows.Add(i, 0);
            }
            return table;
        }
    }
}
