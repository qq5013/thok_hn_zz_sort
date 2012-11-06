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
        //public CacheOrderQueryForm(string deviceClass, params int[] channelAndSortNoes)
        //{
        //    for (i = 0, j == 1; i < 40; i = i + 2, j = j + 2)
        //    {

        //    }

        //}
        //#region ����β���
        //private int sortNo = 0;
        //private int deviceNo = 0;
        //private int sortNoStart = 0;
        //private int frontQuantity = 0;
        //private int laterQuantity = 0;
        //private int sumQuantity = 0;
        //private int channelGroup = 0;
        //private OrderDal orderDal = new OrderDal();
        //#endregion

        //#region ����κͰڶ���������ʾ
        ///// <summary>
        ///// ����κͰڶ���������ʾ
        ///// </summary>
        ///// <param name="deviceNo">�豸��</param>
        ///// <param name="channelGroup">�̵����</param>
        ///// <param name="sortNo">�ּ���ˮ��</param>
        //public CacheOrderQueryForm(int deviceNo, int channelGroup, int sortNo)
        //{
        //    InitializeComponent();
        //    string cacheName = "";
        //    int sumQutity = 0;
        //    if (deviceNo == 5)
        //    {
        //        cacheName = "�����";
        //    }
        //    else if (deviceNo == 6)
        //    {
        //        cacheName = "�ڶ���";
        //    }
        //    else if (deviceNo == 7)
        //    {
        //        cacheName = "1�Ű�װ��";
        //    }
        //    else if (deviceNo == 8)
        //    {
        //        cacheName = "2�Ű�װ��";
        //    }
        //    this.sortNo = sortNo;
        //    this.channelGroup = channelGroup;
        //    DataTable table = orderDal.GetOrderDetailForCacheOrderQuery(channelGroup, sortNo);
        //    if (table.Rows.Count != 0)
        //    {
        //        dgvDetail.DataSource = table;
        //        sumQutity = Convert.ToInt32(table.Compute("SUM(QUANTITY)", ""));
        //    }
        //    this.Text = this.Text + string.Format("[{0}��{1}�����][��ˮ�ţ�{2}][��������{3}]", channelGroup == 1 ? "A" : "B", cacheName, sortNo, sumQutity);

        //}
        //#endregion

        //#region �๵�������������ʾ
        ///// <summary>
        ///// ����������ط�����ѯ���๵������ε����ж�������
        ///// </summary>
        ///// <param name="deviceNo">�豸��</param>
        ///// <param name="channelGroup">�̵����</param>
        ///// <param name="sortNoStart">��ʼ��ˮ��</param>
        ///// <param name="beforeQuantity">СƤ������</param>
        ///// <param name="afterQuantity">�������</param>
        //public CacheOrderQueryForm(int deviceNo, int channelGroup, int sortNoStart, int beforeQuantity, int afterQuantity)
        //{
        //    InitializeComponent();
        //    this.deviceNo = deviceNo;
        //    this.channelGroup = channelGroup;
        //    this.sortNoStart = sortNoStart;
        //    this.frontQuantity = beforeQuantity;
        //    this.laterQuantity = afterQuantity;
        //    this.sumQuantity = frontQuantity + laterQuantity;
        //    string strhead = "";

        //    DataTable orderTable = orderDal.GetAllOrderDetailForCacheOrderQuery(channelGroup, sortNoStart);

        //    DataTable Table = new DataTable();
        //    CreatTableForCacheOrder(Table);

        //    if (deviceNo == 2 || deviceNo == 4)
        //    {
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

        //    }
        //    else
        //    {
        //        if (orderTable.Rows.Count != 0)
        //        {
        //            int tempQuantity = 0;
        //            bool flag = false;
        //            foreach (DataRow orderDetailRow in orderTable.Rows)
        //            {
        //                int orderQuantity = Convert.ToInt32(orderDetailRow["QUANTITY"]);
        //                tempQuantity = tempQuantity + orderQuantity;

        //                if (flag == false)
        //                {
        //                    if (tempQuantity >= frontQuantity)
        //                    {
        //                        if (laterQuantity != 0)
        //                        {
        //                            orderDetailRow["QUANTITY"] = tempQuantity - frontQuantity;
        //                            AddCacheOrderTableRow(Table, orderDetailRow);
        //                            flag = true;
        //                        }
        //                        else
        //                        {
        //                            orderDetailRow["QUANTITY"] = orderQuantity + frontQuantity - tempQuantity;
        //                            AddCacheOrderTableRow(Table, orderDetailRow);

        //                            orderDetailRow["QUANTITY"] = tempQuantity - frontQuantity;
        //                            AddCacheOrderTableRow(Table, orderDetailRow);
        //                            break;
        //                        }

        //                    }
        //                }
        //                else
        //                {
        //                    if (tempQuantity >= sumQuantity)
        //                    {
        //                        orderDetailRow["QUANTITY"] = orderQuantity + sumQuantity - tempQuantity;
        //                        AddCacheOrderTableRow(Table, orderDetailRow);

        //                        orderDetailRow["QUANTITY"] = tempQuantity - sumQuantity;
        //                        AddCacheOrderTableRow(Table, orderDetailRow);
        //                        break;

        //                    }
        //                    else
        //                    {
        //                        AddCacheOrderTableRow(Table, orderDetailRow);
        //                    }
        //                }
        //            }
        //            strhead = string.Format("[{0}�߶๵����˻����][��ˮ�ţ�{1}][��������{2}]", channelGroup == 1 ? "A" : "B", sortNoStart, laterQuantity);
        //        }
        //    }
        //    dgvDetail.DataSource = Table;
        //    this.Text = this.Text + strhead;
        //}
        //#endregion

        //#region ��װ�������������ʾ(δ���)

        ///// <summary>
        ///// ��װ�������������ʾ
        ///// </summary>
        ///// <param name="exportNo">��װ����</param>
        ///// <param name="deviceNo">�豸�ţ�����Ϊ�������������ֶΣ�û̫�����壩</param>
        ///// <param name="sortNo">��ˮ��</param>
        ///// <param name="channelGroup">�̵����</param>
        //public CacheOrderQueryForm(int exportNo, int deviceNo, int sortNo, int channelGroup)
        //{
        //    InitializeComponent();
        //    this.sortNo = sortNo;
        //    this.channelGroup = channelGroup;
        //}
        //#endregion

        //#region ʵʱ��װǰ��װ��
        ///// <summary>
        ///// ʵʱ��װǰ����װ�λ���������ʾ
        ///// </summary>
        ///// <param name="packMode"></param>
        ///// <param name="exportNo"></param>
        ///// <param name="sortNo"></param>
        ///// <param name="channelGroup"></param>
        //public CacheOrderQueryForm(string packMode, int exportNo, int sortNo, int channelGroup)
        //{
        //    InitializeComponent();
        //    this.sortNo = sortNo;
        //    this.channelGroup = channelGroup;

        //    int sumQutity = 0;
        //    DataTable table = orderDal.GetOrderDetailForCacheOrderQuery(packMode, exportNo, sortNo);

        //    if (table.Rows.Count != 0)
        //    {
        //        dgvDetail.DataSource = table;
        //        sumQutity = Convert.ToInt32(table.Compute("SUM(QUANTITY)", ""));
        //    }

        //    this.Text = this.Text + string.Format("[{0}�Ű�װ�������-{1}����ˮ��][��������{2}]", exportNo, sortNo, sumQutity);
        //}
        //#endregion

        //#region ������ɫ����
        //public void LoadColor(int sortNo, int channelGroup)
        //{
        //    DataTable table = orderDal.GetOrderDetailForCacheOrderQuery(channelGroup, sortNo);

        //    foreach (DataGridViewRow row in dgvDetail.Rows)
        //    {
        //        string sChannelGroup = row.Cells["CHANNELLINE"].Value.ToString();
        //        int iSortNo = Convert.ToInt32(row.Cells["SORTNO"].Value);
        //        DataRow[] dataRow = table.Select(string.Format("CHANNELLINE = '{0}' AND SORTNO = {1}", sChannelGroup, iSortNo));

        //        if (dataRow.Length > 0)
        //        {
        //            row.DefaultCellStyle.BackColor = Color.Red;
        //        }
        //    }
        //}

        //public void LoadColor()
        //{
        //    if (dgvDetail.Rows.Count == 2)
        //    {
        //        string cigaretteCode1 = dgvDetail.Rows[dgvDetail.Rows.Count - 2].Cells["CIGARETTECODE"].Value.ToString();
        //        string cigaretteCode2 = dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["CIGARETTECODE"].Value.ToString();
        //        int quantity = Convert.ToInt32(dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["CIGARETTECODE"].Value);
        //        if (cigaretteCode1 == cigaretteCode2 && quantity != 0)
        //        {
        //            dgvDetail.Rows[dgvDetail.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
        //        }
        //    }
        //    if (dgvDetail.Rows.Count >= 3)
        //    {
        //        string cigaretteCode1 = dgvDetail.Rows[dgvDetail.Rows.Count - 2].Cells["CIGARETTECODE"].Value.ToString();
        //        string cigaretteCode2 = dgvDetail.Rows[dgvDetail.Rows.Count - 1].Cells["CIGARETTECODE"].Value.ToString();
        //        if (cigaretteCode1 == cigaretteCode2)
        //        {
        //            dgvDetail.Rows[dgvDetail.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
        //        }
        //        foreach (DataGridViewRow row in dgvDetail.Rows)
        //        {
        //            int quantity = Convert.ToInt32(row.Cells["QUANTITY"].Value);
        //            if (quantity == 0)
        //            {
        //                dgvDetail.Rows.Remove(row);
        //            }
        //        }
        //    }
        //}

        //public void CacheOrderQueryForm_Paint(object sender, PaintEventArgs e)
        //{
        //    LoadColor(this.sortNo, this.channelGroup);
        //}

        //public void CacheOrderQueryFormPaint(object send, PaintEventArgs e)
        //{
        //    LoadColor();
        //}
        //#endregion

        //#region ����α��д����������
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
            Table.Columns.Add("PACKNO");
        }

        ///// <summary>
        ///// ����������
        ///// </summary>
        ///// <param name="Table">������</param>
        ///// <param name="orderDetailRow">������</param>
        //public void AddCacheOrderTableRow(DataTable Table, DataRow orderDetailRow)
        //{
        //    Table.Rows.Add();
        //    Table.Rows[Table.Rows.Count - 1]["SORTNO"] = orderDetailRow["SORTNO"];
        //    Table.Rows[Table.Rows.Count - 1]["ORDERID"] = orderDetailRow["ORDERID"];
        //    Table.Rows[Table.Rows.Count - 1]["CIGARETTECODE"] = orderDetailRow["CIGARETTECODE"];
        //    Table.Rows[Table.Rows.Count - 1]["CIGARETTENAME"] = orderDetailRow["CIGARETTENAME"];
        //    Table.Rows[Table.Rows.Count - 1]["QUANTITY"] = orderDetailRow["QUANTITY"];
        //    Table.Rows[Table.Rows.Count - 1]["CUSTOMERNAME"] = orderDetailRow["CUSTOMERNAME"];
        //    Table.Rows[Table.Rows.Count - 1]["CHANNELNAME"] = orderDetailRow["CHANNELNAME"];
        //    Table.Rows[Table.Rows.Count - 1]["CHANNELTYPE"] = orderDetailRow["CHANNELTYPE"];
        //    Table.Rows[Table.Rows.Count - 1]["CHANNELLINE"] = orderDetailRow["CHANNELLINE"];
        //    Table.Rows[Table.Rows.Count - 1]["PACKNO0"] = orderDetailRow["PACKNO0"];
        //    Table.Rows[Table.Rows.Count - 1]["PACKNO1"] = orderDetailRow["PACKNO1"];
        //    Table.Rows[Table.Rows.Count - 1]["PACKNO2"] = orderDetailRow["PACKNO2"];
        //}
        //#endregion

    }
}