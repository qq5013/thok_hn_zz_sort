using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using THOK.MCP;
namespace THOK.AS.OTS.View
{
    public partial class OrderForm : Form
    {
        private DataTable masterTable = null;
        private DataTable detailTable = null;

        public Context context = null;

        public OrderForm()
        {
            InitializeComponent();

            this.ORDERID.FilteringEnabled = true;
            this.Column4.FilteringEnabled = true;
            this.Column5.FilteringEnabled = true;
            this.Column6.FilteringEnabled = true;
            this.Column7.FilteringEnabled = true;
            this.PACKAGE.FilteringEnabled = true;
            this.Column12.FilteringEnabled = true;

            GetData();
            bsMaster.DataSource = masterTable;
            dgvDetail.DataSource = detailTable;
        }

        private void GetData()
        {
            using (THOK.Util.PersistentManager pm = new THOK.Util.PersistentManager())
            {
                Dao.OrderDao orderDao = new THOK.AS.OTS.Dao.OrderDao();
                masterTable = orderDao.FindOrderMaster();
                detailTable = orderDao.FindOrderDetail();
            }
        }

        private void dgvMaster_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detailTable.DefaultView.RowFilter = string.Format("ORDERID = '{0}'", dgvMaster.Rows[e.RowIndex].Cells[1].Value);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Printer printer = new Printer();
            using (THOK.Util.PersistentManager pm = new THOK.Util.PersistentManager())
            {
                Dao.OrderDao orderDao = new Dao.OrderDao();
                for (int i = dgvMaster.SelectedRows.Count - 1; i >= 0; i--)
                {
                    DataGridViewRow row = dgvMaster.SelectedRows[i];
                    string filter = string.Format("ORDERID='{0}'", row.Cells["ORDERID"].Value);
                    DataRow[] masterRows = masterTable.Select(filter, "ORDERID");
                    DataTable table = orderDao.FindOrderDetail(row.Cells["ORDERID"].Value.ToString());
                    int quantity = Convert.ToInt32(table.Compute("SUM(QUANTITY)",""));

                    printer.Print(masterRows[0], table.Select(), quantity);
                }
            }
        }

        //��ʶ��ӡ״̬
        private void btnModify_Click(object sender, EventArgs e)
        {
            using (THOK.Util.PersistentManager pm = new THOK.Util.PersistentManager())
            {
                if (dgvMaster.SelectedRows.Count != 0)
                {
                    if (DialogResult.OK == MessageBox.Show("��ʶ�������ܻᵼ�¶�����ӡ������ȷ��Ҫִ����", "ѯ��", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                    {
                        string PrintStatus = dgvMaster.SelectedRows[0].Cells[9].Value.ToString();
                        Dao.OrderDao orderDao = new THOK.AS.OTS.Dao.OrderDao();
                        orderDao.UpdatePrintStatusInOne(dgvMaster.SelectedRows[0].Cells["ORDERID"].Value.ToString());
                        DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMaster);
                        bsMaster.DataSource = orderDao.FindOrderMaster();
                        context.ProcessDispatcher.WriteToProcess("mainPanel", "SortNo", -1);
                    }
                }
            }
        }
        //�����ӡ״̬
        private void btnClean_Click(object sender, EventArgs e)
        {
            using (THOK.Util.PersistentManager pm = new THOK.Util.PersistentManager())
            {
                if (dgvMaster.SelectedRows.Count != 0)
                {
                    if (DialogResult.OK == MessageBox.Show("����������ܻᵼ�¶�����ӡ������ȷ��Ҫִ����", "ѯ��", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                    {
                        string PrintStatus = dgvMaster.SelectedRows[0].Cells[9].Value.ToString();
                        int sortNo = Convert.ToInt32(dgvMaster.SelectedRows[0].Cells[0].Value);
                        Dao.OrderDao orderDao = new THOK.AS.OTS.Dao.OrderDao();
                        orderDao.UpdatePrintStatusIsZero(dgvMaster.SelectedRows[0].Cells["ORDERID"].Value.ToString());
                        DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMaster);
                        bsMaster.DataSource = orderDao.FindOrderMaster();
                        context.ProcessDispatcher.WriteToProcess("mainPanel", "SortNo", -1);
                    }
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetData();
            bsMaster.DataSource = masterTable;
            dgvDetail.DataSource = detailTable;
        }
    }
}