using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.Odd.View
{
    public partial class CigaretteForm : THOK.AF.View.ToolbarForm
    {
        private DataTable cigaretteTable = null;
        private THOK.Odd.Dal.CigaretteDal cigaretteDal = new THOK.Odd.Dal.CigaretteDal();

        public CigaretteForm()
        {
            InitializeComponent();
            Util.EnableFilter(dgvMain);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                cigaretteTable = cigaretteDal.GetCigarette();
                bsMain.DataSource = cigaretteTable;

            }
            catch (Exception exp)
            {
                MessageBox.Show("���������Ϣʧ�ܣ�ԭ��" + exp.Message, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                cigaretteDal.SaveCigarette(cigaretteTable);
                Util.ShowInfo("���������Ϣ�ɹ�");

            }
            catch (Exception exp)
            {
                MessageBox.Show("���������Ϣʧ�ܣ�ԭ��" + exp.Message, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }
    }
}

