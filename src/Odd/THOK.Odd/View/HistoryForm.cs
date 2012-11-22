using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using THOK.Odd.Dal;


namespace THOK.Odd.View
{
    public partial class HistoryForm : THOK.AF.View.ToolbarForm
    {
        public HistoryForm()
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            OrderDal orderDal=new OrderDal();
            DataTable batchTable = orderDal.batchTable();
            BatchSelectDialog BatchSelectDialog = new BatchSelectDialog(batchTable);

            if (BatchSelectDialog.ShowDialog() == DialogResult.OK)
            {
                string orderDate = "";
                string batchNo = "";
                orderDate = BatchSelectDialog.SelectedPrintBatch.Split("|"[0])[0];
                batchNo = BatchSelectDialog.SelectedPrintBatch.Split("|"[0])[1];
                DataTable customerTable = orderDal.HistoryData(orderDate,batchNo);
                dgvMaster.DataSource = customerTable;
            }
        }
        private void bsMain_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (bsMain.Current != null)
                {
                    DataRow masterRow = ((DataRowView)bsMain.Current).Row;
                    bsDetail.DataSource = orderDal.GetCustomerCigarette(masterRow["CUSTOMERCODE"].ToString());
                    Util.EnableFilter(dgvDetail);
                }

            }
            catch (Exception exp)
            {
                MessageBox.Show("读入客户卷烟信息失败，原因：" + exp.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable masterTable = orderDal.GetOrderMaster();
            Printer printer = new Printer();
            foreach (DataRow masterRow in masterTable.Rows)
            {
                DataTable detailTable = orderDal.GetCustomerCigarette(masterRow["CUSTOMERCODE"].ToString());
                printer.Print(masterRow, detailTable, Convert.ToInt32(masterRow["QUANTITY"]));
                Application.DoEvents();
            }
        }

        private void dgvMaster_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detailTable.DefaultView.RowFilter = string.Format("ORDERID = '{0}'", dgvMaster.Rows[e.RowIndex].Cells[3].Value);
        }
    }
}