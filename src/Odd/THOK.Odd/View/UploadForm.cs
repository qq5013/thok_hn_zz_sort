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
    public partial class UploadForm : THOK.AF.View.ToolbarForm
    {
        public UploadForm()
        {
            InitializeComponent();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                DataDal dataDal = new DataDal();
                dataDal.OnProcessing += new ProcessEventHandler(dataDal_OnProcessing);
                pbGenFile.Value = 0;
                pbZipFile.Value = 0;
                pbSendFile.Value = 0;
                pbDeleteFile.Value = 0;
                SetState(true);
                dataDal.UploadData(dtpOrderDate.Value.ToShortDateString(), cbBatch.SelectedItem.ToString());
                SetState(false);
            }
            catch (Exception exp)
            {
                Util.ShowInfo("下载数据操作失败，原因：" + exp.Message);
                SetState(false);
            }
        }

        private void SetState(bool state)
        {
            InTask = state;
            btnUpload.Enabled = !state;
        }

        void dataDal_OnProcessing(object sender, ProcessEventArgs e)
        {
            if (e.ScheduleStep == 4)
            {
                pbGenFile.Value = pbGenFile.Maximum;
                pbZipFile.Value = pbZipFile.Maximum;
                pbSendFile.Value = pbSendFile.Maximum;
                pbDeleteFile.Value = pbDeleteFile.Maximum;
                Util.ShowInfo("数据上传完成");
            }
            else
            {
                switch (e.ScheduleStep)
                {
                    case 1:
                        pbGenFile.Value = e.CompleteCount / e.TotalCount * pbGenFile.Maximum;
                        break;
                    case 2:
                        pbGenFile.Value = pbGenFile.Maximum;
                        pbZipFile.Value = e.CompleteCount / e.TotalCount * pbZipFile.Maximum;
                        break;
                    case 3:
                        pbGenFile.Value = pbGenFile.Maximum;
                        pbZipFile.Value = pbZipFile.Maximum;
                        pbSendFile.Value = e.CompleteCount / e.TotalCount * pbSendFile.Maximum;
                        break;
                    case 4:
                        pbGenFile.Value = pbGenFile.Maximum;
                        pbZipFile.Value = pbZipFile.Maximum;
                        pbSendFile.Value = pbSendFile.Maximum;
                        pbDeleteFile.Value = e.CompleteCount / e.TotalCount * pbDeleteFile.Maximum;
                        break;
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void pnlMain_ParentChanged(object sender, EventArgs e)
        {
            try
            {
                RouteDal routeDal = new RouteDal();
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
                RouteDal routeDal = new RouteDal();
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
            if (cbBatch.Items.Count != 0)
                cbBatch.SelectedIndex = 0;
        }
    }
}

