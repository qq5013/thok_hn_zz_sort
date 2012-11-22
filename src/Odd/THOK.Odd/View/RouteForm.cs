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
    public partial class RouteForm : THOK.AF.View.ToolbarForm
    {
        private OrderDal orderDal = new OrderDal();

        public RouteForm()
        {
            InitializeComponent();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                bsMain.DataSource = orderDal.GetRouteQuantity();
                Util.EnableFilter(dgvMaster);
            }
            catch (Exception exp)
            {
                MessageBox.Show("读入线路信息失败，原因：" + exp.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void bsMain_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (bsMain.Current != null)
                {
                    DataRow masterRow = ((DataRowView)bsMain.Current).Row;
                    bsDetail.DataSource = orderDal.GetRouteCigarette(masterRow["ROUTECODE"].ToString());
                    Util.EnableFilter(dgvDetail);
                }

            }
            catch (Exception exp)
            {
                MessageBox.Show("读入线路卷烟信息失败，原因：" + exp.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }
    }
}

