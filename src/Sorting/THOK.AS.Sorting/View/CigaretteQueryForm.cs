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
    public partial class CigaretteQueryForm : THOK.AF.View.ToolbarForm
    {
        private OrderDal orderDal = new OrderDal();

        public CigaretteQueryForm()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (dgvMaster.DataSource == null)
            {
                dgvMaster.DataSource = orderDal.GetCigarettes();
            }
            else
            {
                DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMaster);
            }
        }

        private void dgvMaster_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            dgvDetail.DataSource = orderDal.GetOrderWithCigarette(dgvMaster.Rows[e.RowIndex].Cells[1].Value.ToString());
        }
    }
}