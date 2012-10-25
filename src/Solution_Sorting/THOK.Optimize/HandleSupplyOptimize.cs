using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
namespace THOK.Optimize
{
    //cz添
    /// <summary>
    /// 手工补获定单优化
    /// </summary>
    public class HandleSupplyOptimize
    {
        public DataTable Optimize(DataTable handSupplyOrders, DataTable multiBrandChannel)
        {
            int channelIndex = 0;
            int supplyBatch = 1;
            int channelCount = multiBrandChannel.Rows.Count;
            DataTable tempScOrder = GenerateTmpTable();
            foreach (DataRow order in handSupplyOrders.Rows)
            {
                while (Convert.ToInt32(order["QUANTITY"].ToString()) > 0)
                {
                    //烟道当前存储的卷烟数量
                    int ChannelCigQuantity = Convert.ToInt32(multiBrandChannel.Rows[channelIndex]["QUANTITY"]);
                    //当前定单卷烟数量
                    int CigQuantity = Convert.ToInt32(order["QUANTITY"]);
                    //烟道当前存储的卷烟数量=0
                    if (ChannelCigQuantity == 0)
                    {
                        //当前定单数量>25
                        if (CigQuantity > 25)
                        {
                            multiBrandChannel.Rows[channelIndex]["QUANTITY"] = 25;
                            order["QUANTITY"] = 25;
                            order["CHANNELCODE"] = multiBrandChannel.Rows[channelIndex]["CHANNELCODE"].ToString();
                            AddRow(tempScOrder, order,supplyBatch++);
                            order["QUANTITY"] = CigQuantity - 25;
                            //跳转到下一烟道
                            channelIndex++;
                            if (channelIndex >= channelCount)
                            {
                                channelIndex = 0;
                            }

                        }
                        //当前定单数量<25
                        else
                        {
                            multiBrandChannel.Rows[channelIndex]["QUANTITY"] = CigQuantity;
                            order["CHANNELCODE"] = multiBrandChannel.Rows[channelIndex]["CHANNELCODE"];
                            AddRow(tempScOrder, order, supplyBatch);
                            order["QUANTITY"] = 0;
                        }
                    }
                    //烟道当前存储的卷烟数量不=0
                    else
                    {
                        if (ChannelCigQuantity < 25)
                        {
                            //当前烟道存储的极限
                            int limitQuantity = 25 - ChannelCigQuantity;
                            //当前定单的数量大于当前烟道存储的极限
                            if (CigQuantity >= limitQuantity)
                            {
                                multiBrandChannel.Rows[channelIndex]["QUANTITY"] = ChannelCigQuantity + limitQuantity;
                                order["QUANTITY"] = limitQuantity;
                                order["CHANNELCODE"] = multiBrandChannel.Rows[channelIndex]["CHANNELCODE"];
                                AddRow(tempScOrder, order, supplyBatch++);

                                //拆分定单
                                order["QUANTITY"] = CigQuantity - limitQuantity;
                                //跳转到下一烟道
                                channelIndex++;
                                if (channelIndex >= channelCount)
                                {
                                    channelIndex = 0;
                                }
                            }
                            else
                            {
                                multiBrandChannel.Rows[channelIndex]["QUANTITY"] = ChannelCigQuantity + CigQuantity;
                                order["CHANNELCODE"] = multiBrandChannel.Rows[channelIndex]["CHANNELCODE"];
                                AddRow(tempScOrder, order, supplyBatch);
                                order["QUANTITY"] = 0;
                            }
                        }
                        //如果烟道当前存储大于25条,每次最多只能存储23条
                        else
                        {
                            //当前烟道-25后的值
                            int quantitySub25 = ChannelCigQuantity - 25;
                            //对23取余后的值
                            int quantity23 = quantitySub25 % 18;

                            //当前烟道还能存储的烟的最大条数
                            int limitQuantity = 18 - quantity23;
                            //当前定单的数量大于当前烟道存储的极限
                            if (CigQuantity >= limitQuantity)
                            {
                                multiBrandChannel.Rows[channelIndex]["QUANTITY"] = ChannelCigQuantity+limitQuantity;
                                order["CHANNELCODE"] = multiBrandChannel.Rows[channelIndex]["CHANNELCODE"];
                                order["QUANTITY"] = limitQuantity;
                                AddRow(tempScOrder, order, supplyBatch++);
                                order["QUANTITY"] = CigQuantity - limitQuantity;
                                //跳转到下一烟道
                                channelIndex++;
                                if (channelIndex >= channelCount)
                                {
                                    channelIndex = 0;
                                }
                            }
                            else
                            {
                                multiBrandChannel.Rows[channelIndex]["QUANTITY"] = ChannelCigQuantity+CigQuantity;
                                order["CHANNELCODE"] = multiBrandChannel.Rows[channelIndex]["CHANNELCODE"];
                                AddRow(tempScOrder, order, supplyBatch);
                                order["QUANTITY"] = 0;
                            }
                        }
                    }
                }
            }

            SetChannelSortNo(multiBrandChannel,tempScOrder);

            return tempScOrder;
        }


        private void SetChannelSortNo(DataTable multiBrandChannel, DataTable newSupplyOrders)
        {
            foreach (DataRow channelRow in multiBrandChannel.Rows)
            {
                int channelCount = 0;

                DataRow[] rows = null;

                rows = newSupplyOrders.Select(string.Format("CHANNELCODE='{0}'", channelRow["CHANNELCODE"]), "SORTNO DESC");

                channelCount = 0;
                foreach (DataRow scOrderRow in rows)
                {
                    //记录分拣倒数第7条的订单号
                    if (channelCount <= 7 && channelCount + Convert.ToInt32(scOrderRow["QUANTITY"]) >= 7)
                    {
                        channelRow["SORTNO"] = Convert.ToInt32(scOrderRow["SORTNO"]);
                        break;
                    }
                    channelCount += Convert.ToInt32(scOrderRow["QUANTITY"]);
                }
            }
        }

        /// <summary>
        /// 生成临时表
        /// </summary>
        /// <param name="groupNum"></param>
        /// <returns></returns>
        private DataTable GenerateTmpTable()
        {
            DataTable table = new DataTable();

            table.Columns.Add("SORTNO",typeof(Int32));
            table.Columns.Add("LINECODE");

            table.Columns.Add("ORDERDATE");
            table.Columns.Add("BATCHNO");

            //加入补货批次号
            table.Columns.Add("SUPPLYBATCH",typeof(Int32));
            table.Columns.Add("ORDERID");

            table.Columns.Add("CIGARETTECODE");
            table.Columns.Add("CIGARETTENAME");

            table.Columns.Add("CHANNELCODE");
            table.Columns.Add("QUANTITY", typeof(Int32));
            return table;
        }

        private void AddRow(DataTable scOrderTable,DataRow order,int supplyBatch)
        {
            if (order != null)
            {
                DataRow row = scOrderTable.NewRow();
                row["SORTNO"] = order["SORTNO"];
                row["LINECODE"] = order["LINECODE"];
                row["ORDERDATE"] = order["ORDERDATE"];
                row["BATCHNO"] = order["BATCHNO"];
                row["SUPPLYBATCH"] = supplyBatch;
                row["ORDERID"] = order["ORDERID"];

                row["CIGARETTECODE"] = order["CIGARETTECODE"];
                row["CIGARETTENAME"] = order["CIGARETTENAME"];
                row["CHANNELCODE"] = order["CHANNELCODE"];
                row["QUANTITY"] = order["QUANTITY"];
                scOrderTable.Rows.Add(row);
            }
        }
    }
}
