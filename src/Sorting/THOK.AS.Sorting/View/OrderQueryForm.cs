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
    public partial class OrderQueryForm : THOK.AF.View.ToolbarForm
    {
        private OrderDal orderDal = new OrderDal();

        public OrderQueryForm()
        {
            InitializeComponent();
            this.Column2.FilteringEnabled = true;
            this.Column5.FilteringEnabled = true;
            this.Column6.FilteringEnabled = true;
            this.Column7.FilteringEnabled = true;
            this.Column8.FilteringEnabled = true;
            this.Column9.FilteringEnabled = true;
            this.Column10.FilteringEnabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (bsMaster.DataSource == null)
            {
                bsMaster.DataSource = orderDal.GetOrderMaster();
            }
            else
            {
                DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMaster);
            }
        }

        private void dgvMaster_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvDetail.DataSource = orderDal.GetOrderDetail(dgvMaster.Rows[e.RowIndex].Cells[2].Value.ToString());
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (dgvMaster.SelectedRows.Count != 0)
            {
                if (DialogResult.OK == MessageBox.Show("此操作可能会导致订单包装错误，您确定要执行吗？", "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                    orderDal.SetPackage(dgvMaster.SelectedRows[0].Cells[3].Value.ToString());
                    DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMaster);
                    bsMaster.DataSource = orderDal.GetOrderMaster();
                }
            }
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            if (dgvMaster.SelectedRows.Count != 0)
            {
                if (DialogResult.OK == MessageBox.Show("此操作可能会导致订单包装错误，您确定要执行吗？", "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                    orderDal.ClearPackage(dgvMaster.SelectedRows[0].Cells[3].Value.ToString());
                    DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMaster);
                    bsMaster.DataSource = orderDal.GetOrderMaster();
                }
            }
        }

        private void dgvDetail_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                string sortNo = dgvDetail.Rows[e.RowIndex].Cells["SORTNO"].Value.ToString();
                string orderID = dgvDetail.Rows[e.RowIndex].Cells["ORDERID"].Value.ToString();
                string channelName = dgvDetail.Rows[e.RowIndex].Cells["CHANNELNAME"].Value.ToString();
                string cigaretteCode = dgvDetail.Rows[e.RowIndex].Cells["CIGARETTECODE"].Value.ToString();
                string cigaretteName = dgvDetail.Rows[e.RowIndex].Cells["CIGARETTENAME"].Value.ToString();
                int quantity = Convert.ToInt32(dgvDetail.Rows[e.RowIndex].Cells["QUANTITY"].Value);

                ModifyOrderDialog modifyOrderDialog = new ModifyOrderDialog(sortNo, orderID, channelName, cigaretteCode, cigaretteName, quantity);
                if (modifyOrderDialog.ShowDialog() == DialogResult.OK)
                {
                    orderDal.UpdateQuantity(sortNo, orderID, channelName, cigaretteCode, modifyOrderDialog.GetModifyQuantity());
                    THOK.MCP.Logger.Info(string.Format("修改订单数量成功：SORTNO = {0} ,ORDERID = {1} , CHANNELNAME = {2} ,CIGARETTECODE = {3} ,QUANTITY = {4} ,MODIFYQUANTITY = {5} !", sortNo, orderID, channelName, cigaretteCode, quantity, modifyOrderDialog.GetModifyQuantity()));
                }
            }
            catch (Exception ee)
            {
                THOK.MCP.Logger.Error("修改订单失败！原因：" + ee.Message);
            }
        }
    }
}

