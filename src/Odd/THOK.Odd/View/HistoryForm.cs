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
        private DataTable masterTable = null;
        private DataTable detailTable = null;
        private string orderDate = "";
        private string batchNo = "";

        OrderDal orderDal = new OrderDal();
        public HistoryForm()
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {  
            DataTable batchTable = orderDal.batchTable();
            BatchSelectDialog BatchSelectDialog = new BatchSelectDialog(batchTable);

            if (BatchSelectDialog.ShowDialog() == DialogResult.OK)
            {
                orderDate = BatchSelectDialog.SelectedPrintBatch.Split("|"[0])[0];
                batchNo = BatchSelectDialog.SelectedPrintBatch.Split("|"[0])[1];
                masterTable = orderDal.HistoryData(orderDate, batchNo);
                detailTable = orderDal.DetailTable(orderDate, batchNo);
                dgvMaster.DataSource = masterTable;
                dgvDetail.DataSource = detailTable;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable masterTable = orderDal.GetHistoryOrderMaster();
            Printer printer = new Printer();
            for (int i = dgvMaster.SelectedRows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow row = dgvMaster.SelectedRows[i];
                string filter = string.Format("CUSTOMERCODE='{0}'", row.Cells["CUSTOMERCODE"].Value);
                DataRow[] masterRows = masterTable.Select(filter, "CUSTOMERCODE");
                DataTable table = orderDal.DetailTablebyCustomer(orderDate, batchNo, row.Cells["CUSTOMERCODE"].Value.ToString());
                int quantity = Convert.ToInt32(table.Compute("SUM(QUANTITY)", ""));
                printer.Print(masterRows[0], table.Select(), quantity);
            }
        }

        private void dgvMaster_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            detailTable.DefaultView.RowFilter = string.Format("CUSTOMERCODE = '{0}'", dgvMaster.Rows[e.RowIndex].Cells[4].Value);
        }
    }
}