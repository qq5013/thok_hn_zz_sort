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
        /// ����ζ�����ѯ
        /// </summary>
        /// <param name="deviceClass">�豸����</param>
        /// <param name="SortNoes">��ˮ������</param>
        public CacheOrderQueryForm(string deviceClass, params int[] SortNoes)
        {
            InitializeComponent();
            int countAfter = 0;//�������εľ�������
            int countBefore = 0;//����Ƥ���εľ�������
            int sortNoFirst = 0;//������׸���ˮ��
            int countSortNoFirst = 0;//������׸���ˮ�ŵľ�������

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
                        sortNoQuantity = sortNoQuantity + rowQuantity;//ǰ����ˮ�����������ľ���������
                    }
                    if (deviceClass == "�����") 
                    {
                        frontQuantity = sortNoQuantity - countSortNoFirst;
                        laterQuantity = countBefore;
                    }
                    else if (deviceClass == "�����") 
                    {
                        frontQuantity = sortNoQuantity - countSortNoFirst + countBefore;
                        laterQuantity = countAfter;
                    }
                }

                DataTable Table = new DataTable();//���������ṹ
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
                this.dgvDetail.DataSource = Table;

                #region ���÷���
                //if (countBefore >= countSortNoFirst)//�������ζ�����ˮ����������1��
                //{
                //    if (sortNoTable.Rows.Count != 0)
                //    {
                //        int sortNoQuantity = 0;
                //        foreach (DataRow sortNoDetailRow in sortNoTable.Rows)
                //        {
                //            int rowQuantity = Convert.ToInt32(sortNoDetailRow["QUANTITY"]);
                //            sortNoQuantity = sortNoQuantity + rowQuantity;//ǰ����ˮ�����������ľ���������
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
                //            strhead = string.Format("[{0}�߶๵��ǰ��СƤ��][��ˮ�ţ�{1}][��������{2}]", channelGroup == 1 ? "A" : "B", sortNoStart, frontQuantity);
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
        /// �������������Ӧ�ֶ�
        /// </summary>
        /// <returns>������</returns>
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
        /// ����������
        /// </summary>
        /// <param name="Table">������</param>
        /// <param name="orderDetailRow">������</param>
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
        }

        public void CacheOrderQueryFormPaint(object send, PaintEventArgs e)
        {
            LoadColor();
        }

        public void LoadColor()
        {
            if (dgvDetail.Rows.Count == 2)
            {
                string cigaretteCode1 = dgvDetail.Rows[dgvDetail.Rows.Count - 2].Cells["CIGARETTECODE"].Value.ToString();
                string cigaretteCode2 = dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["CIGARETTECODE"].Value.ToString();
                int quantity = Convert.ToInt32(dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["CIGARETTECODE"].Value);
                if (cigaretteCode1 == cigaretteCode2 && quantity != 0)
                {
                    dgvDetail.Rows[dgvDetail.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                }
            }
            if (dgvDetail.Rows.Count >= 3)
            {
                string cigaretteCode1 = dgvDetail.Rows[dgvDetail.Rows.Count - 2].Cells["CIGARETTECODE"].Value.ToString();
                string cigaretteCode2 = dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["CIGARETTECODE"].Value.ToString();
                if (cigaretteCode1 == cigaretteCode2)
                {
                    dgvDetail.Rows[dgvDetail.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                }
                foreach (DataGridViewRow row in dgvDetail.Rows)
                {
                    int quantity = Convert.ToInt32(row.Cells["QUANTITY"].Value);
                    if (quantity == 0)
                    {
                        dgvDetail.Rows.Remove(row);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderTable">����ζ�����</param>
        /// <param name="frontQuantity">ǰ����</param>
        /// <param name="laterQuantity">������</param>
        /// <returns>���ǰ��������������Ϊ��������Table</returns>
        //public DataTable CacheorderDataTable(DataTable orderTable, int frontQuantity, int laterQuantity)
        //{
        //    DataTable Table = new DataTable();//���������ṹ
        //    CreatTableForCacheOrder(Table);

        //    int sumQuantity = frontQuantity + laterQuantity;
        //    if (orderTable.Rows.Count != 0)
        //    {
        //        int tempQuantity = 0;
        //        bool flag = false;
        //        foreach (DataRow orderDetailRow in orderTable.Rows)
        //        {
        //            int orderQuantity = Convert.ToInt32(orderDetailRow["QUANTITY"]);
        //            tempQuantity = tempQuantity + orderQuantity;

        //            if (flag == false)
        //            {
        //                if (tempQuantity >= frontQuantity)
        //                {
        //                    if (laterQuantity != 0)
        //                    {
        //                        orderDetailRow["QUANTITY"] = tempQuantity - frontQuantity;
        //                        AddCacheOrderTableRow(Table, orderDetailRow);
        //                        flag = true;
        //                    }
        //                    else
        //                    {
        //                        orderDetailRow["QUANTITY"] = orderQuantity + frontQuantity - tempQuantity;
        //                        AddCacheOrderTableRow(Table, orderDetailRow);

        //                        orderDetailRow["QUANTITY"] = tempQuantity - frontQuantity;
        //                        AddCacheOrderTableRow(Table, orderDetailRow);
        //                        break;
        //                    }

        //                }
        //            }
        //            else
        //            {
        //                if (tempQuantity >= sumQuantity)
        //                {
        //                    orderDetailRow["QUANTITY"] = orderQuantity + sumQuantity - tempQuantity;
        //                    AddCacheOrderTableRow(Table, orderDetailRow);

        //                    orderDetailRow["QUANTITY"] = tempQuantity - sumQuantity;
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
        //    return Table;
        //}

    }
}