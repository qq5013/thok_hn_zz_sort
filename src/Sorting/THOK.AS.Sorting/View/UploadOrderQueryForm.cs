using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using THOK.AS.Sorting.Dal;
using THOK.AS.Sorting.Dao;
using THOK.Util;

namespace THOK.AS.Sorting.View
{
    public partial class UploadOrderQueryForm : THOK.AF.View.ToolbarForm
    {
        public UploadOrderQueryForm()
        {
            InitializeComponent();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            string uploadDate = dtpUpload.Value.ToString("yyyyMMdd");
            if (MessageBox.Show("��ȷ�������ϴ�����Ϊ:" + uploadDate, "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                btnUpload.Enabled = false;
                using (PersistentManager pm = new PersistentManager())
                {
                    UploadDao uploadDao = new UploadDao();
                    DataTable table = uploadDao.FindOrder(uploadDate);
                    gvMain.DataSource = table;
                    using (PersistentManager outPM = new PersistentManager("OuterConnection"))
                    {
                        UploadDao outDao = new UploadDao();
                        outDao.SetPersistentManager(outPM);
                        lblProcess.Visible = true;
                        int total = table.Rows.Count;
                        int current = 1;
                        foreach (DataRow row in table.Rows)
                        {
                            try
                            {
                                lblProcess.Text = string.Format("{0}/{1}", current++, total);
                                Application.DoEvents();

                                outDao.Insert(row);
                                uploadDao.Delete(row["ORDER_ID"].ToString());
                            }
                            catch (Exception exp)
                            {
                                MessageBox.Show("�����ϴ���ʽ����ԭ��:" + exp.Message);
                            }
                        }
                        lblProcess.Visible = false;
                    }
                }
                MessageBox.Show("�����ϴ��ɹ�");
                btnUpload.Enabled = true;
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                try
                {
                    ParamDao paramDao = new ParamDao();
                    int uploadMode = Convert.ToInt32(paramDao.FindState("UPLOADMODE"));

                    ParamDialog paramDialog = new ParamDialog(uploadMode);
                    if (paramDialog.ShowDialog() == DialogResult.OK)
                    {
                        paramDao.Update("UPLOADMODE", paramDialog.UploadMode.ToString());
                        MessageBox.Show(string.Format("�����ϴ���ʽΪ'{0}'�ɹ�", paramDialog.UploadMode == 0 ? "�Զ�" : "�ֶ�"));
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show("�����ϴ���ʽ����ԭ��:" + exp.Message);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Exit(); 
        }
    }
}