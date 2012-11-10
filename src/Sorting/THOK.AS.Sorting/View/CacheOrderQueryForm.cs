using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using THOK.AS.Sorting.Dal;

namespace THOK.AS.Sorting.View
{
    public partial class CacheOrderQueryForm : Form
    {
        public CacheOrderQueryForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 缓存段订单查询
        /// </summary>
        /// <param name="deviceClass">设备类型</param>
        /// <param name="SortNoes">流水号数组</param>
        public CacheOrderQueryForm(string deviceClass, params int[] SortNoes)
        {
            int countAfter = 0;//缓存打码段的卷烟数量
            int countBefore = 0;//缓存皮带段的卷烟数量
            int sortNoFirst = 0;//缓存段首个流水号
            int countSortNoFirst = 0;//缓存段首个流水号的卷烟数量

            int frontQuantity = 0;
            int laterQuantity = 0;

            for (int i = 0; i < 20; i++)
            {
                if (SortNoes[i] != 0)
                {
                    countAfter++;
                }
            }

            for(int j = 20; j < 40;j++)
            {
                if (SortNoes[j] != 0)
                {
                    countBefore++;
                }
            }

            for (int k = 0; k < 40; k++) 
            {
                if (SortNoes[k] != 0)
                {
                    sortNoFirst = SortNoes[k];
                    break;
                }
            }

            for (int o = 0; o < 40; o++)
            {
                if (SortNoes[o] != sortNoFirst) 
                {
                    countSortNoFirst++;
                }
            }

            OrderDal orderDal = new OrderDal();
            DataTable orderTable = orderDal.GetAllOrderDetailForCacheOrderQuery(sortNoFirst);


            if (orderTable.Rows.Count != 0)
            {
                DataTable sortNoTable = orderDal.GetSortNoOrderDetail(sortNoFirst);
                if (sortNoTable.Rows.Count != 0)
                {
                    int sortNoQuantity = 0;
                    foreach (DataRow sortNoDetailRow in sortNoTable.Rows)
                    {
                        int rowQuantity = Convert.ToInt32(sortNoDetailRow["QUANTITY"]);
                        sortNoQuantity = sortNoQuantity + rowQuantity;//前端流水号所属订单的卷烟总数量
                    }
                    if (deviceClass == "打码段") 
                    {
                        frontQuantity = sortNoQuantity - countSortNoFirst;
                        laterQuantity = countBefore;
                    }
                    else if (deviceClass == "链板段") 
                    {
                        frontQuantity = sortNoQuantity - countSortNoFirst + countBefore;
                        laterQuantity = countAfter;
                    }
                    //DataTable table = CacheorderDataTable(orderTable, frontQuantity, laterQuantity);
                }
                dgvDetail.DataSource = CacheorderDataTable(orderTable, frontQuantity, laterQuantity);
                #region 备用方法
                //if (countBefore >= countSortNoFirst)//如果打码段订单流水号数量大于1个
                //{
                //    if (sortNoTable.Rows.Count != 0)
                //    {
                //        int sortNoQuantity = 0;
                //        foreach (DataRow sortNoDetailRow in sortNoTable.Rows)
                //        {
                //            int rowQuantity = Convert.ToInt32(sortNoDetailRow["QUANTITY"]);
                //            sortNoQuantity = sortNoQuantity + rowQuantity;//前端流水号所属订单的卷烟总数量
                //        }

                //        if (orderTable.Rows.Count != 0)
                //        {
                //            int tempQuantity = 0;
                //            foreach (DataRow orderDetailRow in orderTable.Rows)
                //            {
                //                int orderQuantity = Convert.ToInt32(orderDetailRow["QUANTITY"]);
                //                tempQuantity = tempQuantity + orderQuantity;

                //                if (tempQuantity >= frontQuantity)
                //                {
                //                    orderDetailRow["QUANTITY"] = orderQuantity + frontQuantity - tempQuantity;
                //                    AddCacheOrderTableRow(Table, orderDetailRow);

                //                    orderDetailRow["QUANTITY"] = tempQuantity - frontQuantity;
                //                    AddCacheOrderTableRow(Table, orderDetailRow);
                //                    break;
                //                }
                //                else
                //                {
                //                    AddCacheOrderTableRow(Table, orderDetailRow);
                //                }
                //            }
                //            strhead = string.Format("[{0}线多沟带前端小皮带][流水号：{1}][总数量：{2}]", channelGroup == 1 ? "A" : "B", sortNoStart, frontQuantity);
                //        }

                //        int goneQuantity = sortNoQuantity - countBefore;
                //        int tempQuantity = 0;
                //        int tempGoneQuantity = 0;
                //        int isFirstTime = 0;

                //        foreach (DataRow orderDetailRow in orderTable.Rows)
                //        {
                //            int orderQuantity = Convert.ToInt32(orderDetailRow["QUANTITY"]);
                //            tempGoneQuantity = tempGoneQuantity + orderQuantity;
                //            if (tempGoneQuantity < goneQuantity)
                //            {
                //                continue;
                //            }
                //            else
                //            {
                //                isFirstTime++;
                //                if (isFirstTime == 1)
                //                {
                //                    orderDetailRow["QUANTITY"] = tempQuantity - goneQuantity;
                //                }
                //                tempQuantity = tempQuantity - goneQuantity;
                //                if (tempQuantity >= countBefore)
                //                {
                //                    orderDetailRow["QUANTITY"] = orderQuantity + countBefore - tempQuantity;
                //                    AddCacheOrderTableRow(Table, orderDetailRow);

                //                    orderDetailRow["QUANTITY"] = tempQuantity - countBefore;
                //                    AddCacheOrderTableRow(Table, orderDetailRow);
                //                    break;
                //                }
                //                else
                //                {
                //                    AddCacheOrderTableRow(Table, orderDetailRow);
                //                }
                //            }
                //        }
                //    }
                //}

                #endregion
            }
        }

        /// <summary>
        /// 添加虚拟表并添加相应字段
        /// </summary>
        /// <returns>订单表</returns>
        public void CreatTableForCacheOrder(DataTable Table)
        {
            Table.Columns.Add("SORTNO");
            Table.Columns.Add("ORDERID");
            Table.Columns.Add("CIGARETTECODE");
            Table.Columns.Add("CIGARETTENAME");
            Table.Columns.Add("QUANTITY");
            Table.Columns.Add("CUSTOMERNAME");
            Table.Columns.Add("CHANNELNAME");
            Table.Columns.Add("CHANNELTYPE");
        }

        /// <summary>
        /// 增加行数据
        /// </summary>
        /// <param name="Table">订单表</param>
        /// <param name="orderDetailRow">订单行</param>
        public void AddCacheOrderTableRow(DataTable Table, DataRow orderDetailRow)
        {
            Table.Rows.Add();
            Table.Rows[Table.Rows.Count - 1]["SORTNO"] = orderDetailRow["SORTNO"];
            Table.Rows[Table.Rows.Count - 1]["ORDERID"] = orderDetailRow["ORDERID"];
            Table.Rows[Table.Rows.Count - 1]["CIGARETTECODE"] = orderDetailRow["CIGARETTECODE"];
            Table.Rows[Table.Rows.Count - 1]["CIGARETTENAME"] = orderDetailRow["CIGARETTENAME"];
            Table.Rows[Table.Rows.Count - 1]["QUANTITY"] = orderDetailRow["QUANTITY"];
            Table.Rows[Table.Rows.Count - 1]["CUSTOMERNAME"] = orderDetailRow["CUSTOMERNAME"];
            Table.Rows[Table.Rows.Count - 1]["CHANNELNAME"] = orderDetailRow["CHANNELNAME"];
            Table.Rows[Table.Rows.Count - 1]["CHANNELTYPE"] = orderDetailRow["CHANNELTYPE"];
            Table.Rows[Table.Rows.Count - 1]["CHANNELLINE"] = orderDetailRow["CHANNELLINE"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderTable">缓存段订单表</param>
        /// <param name="frontQuantity">前数量</param>
        /// <param name="laterQuantity">后数量</param>
        /// <returns>拆除前数量，卷烟数量为后数量的Table</returns>
        public DataTable CacheorderDataTable(DataTable orderTable, int frontQuantity, int laterQuantity)
        {
            DataTable Table = new DataTable();//创建虚拟表结构
            CreatTableForCacheOrder(Table);

            int sumQuantity = frontQuantity + laterQuantity;
            if (orderTable.Rows.Count != 0)
            {
                int tempQuantity = 0;
                bool flag = false;
                foreach (DataRow orderDetailRow in orderTable.Rows)
                {
                    int orderQuantity = Convert.ToInt32(orderDetailRow["QUANTITY"]);
                    tempQuantity = tempQuantity + orderQuantity;

                    if (flag == false)
                    {
                        if (tempQuantity >= frontQuantity)
                        {
                            if (laterQuantity != 0)
                            {
                                orderDetailRow["QUANTITY"] = tempQuantity - frontQuantity;
                                AddCacheOrderTableRow(Table, orderDetailRow);
                                flag = true;
                            }
                            else
                            {
                                orderDetailRow["QUANTITY"] = orderQuantity + frontQuantity - tempQuantity;
                                AddCacheOrderTableRow(Table, orderDetailRow);

                                orderDetailRow["QUANTITY"] = tempQuantity - frontQuantity;
                                AddCacheOrderTableRow(Table, orderDetailRow);
                                break;
                            }

                        }
                    }
                    else
                    {
                        if (tempQuantity >= sumQuantity)
                        {
                            orderDetailRow["QUANTITY"] = orderQuantity + sumQuantity - tempQuantity;
                            AddCacheOrderTableRow(Table, orderDetailRow);

                            orderDetailRow["QUANTITY"] = tempQuantity - sumQuantity;
                            AddCacheOrderTableRow(Table, orderDetailRow);
                            break;

                        }
                        else
                        {
                            AddCacheOrderTableRow(Table, orderDetailRow);
                        }
                    }
                }
            }
            return Table;
        }
    }
}