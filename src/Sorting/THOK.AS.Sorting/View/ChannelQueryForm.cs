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
    public partial class ChannelQueryForm : THOK.AF.View.ToolbarForm
    {
        private ChannelDal channelDal = new ChannelDal();

        public ChannelQueryForm()
        {
            InitializeComponent();
            this.Column1.FilteringEnabled = true;
            this.Column2.FilteringEnabled = true;
            this.Column5.FilteringEnabled = true;
            this.Column6.FilteringEnabled = true;
            this.Column7.FilteringEnabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (bsMain.DataSource == null)
                bsMain.DataSource = channelDal.GetChannel();
            else
                DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMain);
        }

        private void btnExchange_Click(object sender, EventArgs e)
        {
            if (dgvMain.SelectedRows.Count > 0 && dgvMain.SelectedRows[0].Cells[6].Value.ToString() != "0")
            {
                string channelCode = dgvMain.SelectedRows[0].Cells[0].Value.ToString();
                DataTable table = channelDal.GetEmptyChannel();
                if (table.Rows.Count != 0)
                {
                    ChannelDialog channelDailog = new ChannelDialog(table);
                    if (channelDailog.ShowDialog() == DialogResult.OK)
                    {
                        int sourceChannelAddress = 0;
                        int targetChannelAddress = 0;

                        if (channelDal.ExechangeChannel(channelCode, channelDailog.SelectedChannelCode,out sourceChannelAddress,out targetChannelAddress))
                        {
                            int [] data = new int [3];
                            data[0] = sourceChannelAddress;
                            data[1] = targetChannelAddress;
                            data[2] = 1;

                            this.mainFrame.Context.ProcessDispatcher.WriteToService("SortPLC", "ChannelChangeData", data);
                        }
                        DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn.RemoveFilter(dgvMain);
                        bsMain.DataSource = channelDal.GetChannel();
                    }
                }
                else
                    MessageBox.Show("无法调整烟道，原因：没有可用烟道。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

