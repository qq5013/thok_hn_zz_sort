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
    public partial class CustomerForm : THOK.AF.View.ToolbarForm
    {
        private OrderDal orderDal = new OrderDal();

        public CustomerForm()
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                bsMain.DataSource = orderDal.GetCustomerQuantity();
                Util.EnableFilter(dgvMaster);
            }
            catch (Exception exp)
            {
                MessageBox.Show("读入客户信息失败，原因：" + exp.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}

