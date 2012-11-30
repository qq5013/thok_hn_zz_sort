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
    public partial class DownloadForm : THOK.AF.View.ToolbarForm
    {
        private RouteDal routeDal = new RouteDal();
        private DataDal dataDal = new DataDal();

        public DownloadForm()
        {
            InitializeComponent();
        }

        private void pnlMain_ParentChanged(object sender, EventArgs e)
        {
            try
            {
                FillBatch(routeDal.GetBatch(dtpOrderDate.Value.ToShortDateString()));
            }
            catch (Exception exp)
            {
                Util.ShowInfo("读入数据失败，原因：" + exp.Message);
            }
        }

        private void dtpOrderDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                FillBatch(routeDal.GetBatch(dtpOrderDate.Value.ToShortDateString()));
            }
            catch (Exception exp)
            {
                Util.ShowInfo("读入数据失败，原因：" + exp.Message);
            }
        }

        private void FillBatch(DataTable table)
        {
            cbBatch.Items.Clear();
            foreach (DataRow row in table.Rows)
            {
                cbBatch.Items.Add(row["BATCHNO"]);
            }
            cbBatch.Items.Insert(0, table.Rows.Count + 1);
            cbBatch.SelectedIndex = 0;
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                DataDal dataDal = new DataDal();
                dataDal.OnProcessing += new ProcessEventHandler(dataDal_OnProcessing);
                SetState(true);
                dataDal.DownloadData(dtpOrderDate.Value, cbBatch.SelectedItem.ToString());
                SetState(false);
            }
            catch (Exception exp)
            {
                Util.ShowInfo("下载数据操作失败，原因：" + exp.Message);
                SetState(true);
            }
        }

        private void SetState(bool state)
        {
            InTask = state;
            btnDownload.Enabled = !state;
            btnClear.Enabled = !state;
        }

        void dataDal_OnProcessing(object sender, ProcessEventArgs e)
        {
            if (e.ScheduleStep == 4)
            {
                pbCigarette.Value = pbCigarette.Maximum;
                pbClear.Value = pbClear.Maximum;
                pbOrder.Value = pbOrder.Maximum;
                pbRoute.Value = pbRoute.Maximum;
                Util.ShowInfo("数据下载完成");
            }
            else
            {
                double value = Convert.ToDouble(e.CompleteCount) / e.TotalCount;
                switch (e.ScheduleStep)
                {
                    case 1:
                        pbCigarette.Value = Convert.ToInt32(value * pbCigarette.Maximum);
                        break;
                    case 2:
                        pbCigarette.Value = pbCigarette.Maximum;
                        pbClear.Value = Convert.ToInt32(value * pbClear.Maximum);
                        break;
                    case 3:
                        pbCigarette.Value = pbCigarette.Maximum;
                        pbClear.Value = pbClear.Maximum;
                        pbOrder.Value = Convert.ToInt32(value * pbOrder.Maximum);
                        break;
                    case 4:
                        pbCigarette.Value = pbCigarette.Maximum;
                        pbClear.Value = pbClear.Maximum;
                        pbOrder.Value = pbOrder.Maximum;
                        pbRoute.Value = Convert.ToInt32(value * pbRoute.Maximum);
                        break;
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                dataDal.DeleteBatch(dtpOrderDate.Value.ToShortDateString(), cbBatch.SelectedItem.ToString());
            }
            catch (Exception exp)
            {
                Util.ShowInfo("清除数据操作失败，原因：" + exp.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }        
    }
}

