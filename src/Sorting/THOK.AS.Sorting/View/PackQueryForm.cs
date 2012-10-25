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
    public partial class PackQueryForm : THOK.AF.View.ToolbarForm
    {
        private OrderDal orderDal = new OrderDal();
        public PackQueryForm()
        {
            InitializeComponent();
            this.Column2.FilteringEnabled = true;
            this.Column3.FilteringEnabled = true;
            this.Column5.FilteringEnabled = true;
            this.Column6.FilteringEnabled = true;
            this.Column7.FilteringEnabled = true;
            this.Column8.FilteringEnabled = true;
            this.Column10.FilteringEnabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }
        
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            bsMaster.DataSource = orderDal.GetPackMaster();
            if (bsMaster.DataSource != null)
            {
                DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMaster);
            }
        }

        private void dgvMaster_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvDetail.DataSource = orderDal.GetPackDetail(dgvMaster.Rows[e.RowIndex].Cells[1].Value.ToString());
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (dgvMaster.SelectedRows.Count != 0)
            {
                if (DialogResult.OK == MessageBox.Show("此操作可能会导致订单包装错误，您确定要执行吗？", "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                    orderDal.SetPackage(dgvMaster.SelectedRows[0].Cells[1].Value.ToString());
                    DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMaster);
                    bsMaster.DataSource = orderDal.GetPackMaster();
                }
            }
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            if (dgvMaster.SelectedRows.Count != 0)
            {
                if (DialogResult.OK == MessageBox.Show("此操作可能会导致订单包装错误，您确定要执行吗？", "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                    orderDal.ClearPackage(dgvMaster.SelectedRows[0].Cells[1].Value.ToString());
                    DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMaster);
                    bsMaster.DataSource = orderDal.GetPackMaster();
                }
            }
        }

        private void btQuery_Click(object sender, EventArgs e)
        {
            DataTable table = orderDal.GetCigarettes();
            if (table.Rows.Count != 0)
            {
                CigaretteQueryDialog cigaretteQueryDialog = new CigaretteQueryDialog(table);
                if (cigaretteQueryDialog.ShowDialog() == DialogResult.OK)
                {
                    string [] filter = cigaretteQueryDialog.Filter;
                    bsMaster.DataSource = orderDal.GetPackMaster(filter);
                }
            }

            if (bsMaster.DataSource != null)
            {
                DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMaster);
            }
        }

        private void dgvMaster_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //MessageBox.Show(e.ToString());
        }
    }
}

