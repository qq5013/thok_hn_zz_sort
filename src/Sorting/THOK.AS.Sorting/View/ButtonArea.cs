using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using THOK.MCP;
using THOK.MCP.View;
using THOK.Util;
using THOK.AS.Sorting.Dao;

namespace THOK.AS.Sorting.View
{
    public partial class ButtonArea : ProcessControl
    {
        public ButtonArea()
        {
            InitializeComponent();           
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (btnStop.Enabled)
            {
                MessageBox.Show("��ֹͣ�ּ���ҵ�����˳�ϵͳ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (DialogResult.Yes == MessageBox.Show("��ȷ��Ҫ�˳��ּ���ϵͳ��", "ѯ��", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                Util.LogFile.DeleteFile();
                Application.Exit();
            }
        }

        private void btnOperate_Click(object sender, EventArgs e)
        {
            try
            {
                THOK.AF.Config config = new THOK.AF.Config();
                THOK.AF.MainFrame mainFrame = new THOK.AF.MainFrame(config);
                mainFrame.ShowInTaskbar = false;
                mainFrame.ShowIcon = false;
                mainFrame.StartPosition = FormStartPosition.CenterScreen;
                mainFrame.WindowState = FormWindowState.Maximized;
                mainFrame.Context = Context;
                mainFrame.ShowDialog();
            }
            catch (Exception ee)
            {
                Logger.Error("������ҵʧ�ܣ�ԭ��" + ee.Message);
            }

        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            DownloadData();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                using (PersistentManager pm = new PersistentManager())
                {
                    OrderDao orderDao = new OrderDao();
                    string sortNo = orderDao.FindMaxSortedMaster();
                    using (PersistentManager pmServer = new PersistentManager("ServerConnection"))
                    {
                        ServerDao serverDao = new ServerDao();
                        serverDao.SetPersistentManager(pmServer);
                        serverDao.UpdateOrderStatus(sortNo);
                        MessageBox.Show("�ϴ����ݳɹ���", "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ee)
            {
                Logger.Error("�ϴ�����ʧ�ܣ�ԭ��" + ee.Message);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Context.ProcessDispatcher.WriteToProcess("LEDProcess", "UnCheck", null);
            SwitchStatus(true);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (Context.Processes["OrderRequestProcess"] != null)
            {
                Context.Processes["OrderRequestProcess"].Suspend();
            }

            if (Context.Processes["PackRequestProcess"] != null)
            {
                Context.Processes["PackRequestProcess"].Suspend();
            }

            SwitchStatus(false);
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            Context.Processes["LEDProcess"].Resume();
            Context.ProcessDispatcher.WriteToProcess("LEDProcess", "Check", null);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "help.chm");
        }

        private void SwitchStatus(bool isStart)
        {
            btnDownload.Enabled = !isStart;
            btnUpload.Enabled = !isStart;
            btnStart.Enabled = !isStart;
            btnStop.Enabled = isStart;
        }

        private void DownloadData()
        {
            try
            {
                using (PersistentManager pm = new PersistentManager())
                {
                    OrderDao orderDao = new OrderDao();
                    if (orderDao.FindUnsortCount() != 0)
                        if (DialogResult.Cancel == MessageBox.Show("����δ�ּ�����ݣ���ȷ��Ҫ��������������", "ѯ��", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                            return;

                    ChannelDao channelDao = new ChannelDao();

                    using (PersistentManager pmServer = new PersistentManager("ServerConnection"))
                    {
                        ServerDao serverDao = new ServerDao();
                        serverDao.SetPersistentManager(pmServer);

                        string lineCode = Context.Attributes["LineCode"].ToString();

                        DataTable table = serverDao.FindBatch(lineCode);
                        if (table.Rows.Count != 0)
                        {

                            string batchID = table.Rows[0]["BATCHID"].ToString();
                            string orderDate = table.Rows[0]["ORDERDATE"].ToString();
                            string batchNo = table.Rows[0]["BATCHNO"].ToString();

                            Context.ProcessDispatcher.WriteToProcess("monitorView", "ProgressState", new ProgressState("�����̵���", 4, 1));
                            table = serverDao.FindChannel(orderDate, batchNo, lineCode);
                            channelDao.InsertChannel(table);
                            System.Threading.Thread.Sleep(100);

                            Context.ProcessDispatcher.WriteToProcess("monitorView", "ProgressState", new ProgressState("���ض�������", 4, 2));
                            table = serverDao.FindOrderMaster(orderDate, batchNo, lineCode);
                            orderDao.InsertMaster(table);
                            System.Threading.Thread.Sleep(100);

                            Context.ProcessDispatcher.WriteToProcess("monitorView", "ProgressState", new ProgressState("���ض�����ϸ��", 4, 3));
                            table = serverDao.FindOrder(orderDate, batchNo, lineCode);
                            orderDao.InsertOrder(table);

                            Context.ProcessDispatcher.WriteToProcess("monitorView", "ProgressState", new ProgressState("�����ֹ�����������ϸ��", 4, 4));
                            table = serverDao.FindHandleSupply(orderDate, batchNo, lineCode);
                            orderDao.InsertHandleSupply(table);

                            serverDao.UpdateBatchStatus(batchID, lineCode);

                            Logger.Info("�����������");
                            Context.ProcessDispatcher.WriteToProcess("LEDProcess", "NewData", null);
                            Context.ProcessDispatcher.WriteToProcess("CurrentOrderProcess", "CurrentOrder", new int[] { 0 });
                            Context.ProcessDispatcher.WriteToProcess("monitorView", "ProgressState", new ProgressState());
                        }
                        else
                            MessageBox.Show("û����Ҫ�ּ�Ķ������ݡ�", "��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("��������ʧ�ܣ�ԭ��" + e.Message);
            }
        }
    }
}
