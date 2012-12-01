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
                MessageBox.Show("先停止分拣作业才能退出系统。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (DialogResult.Yes == MessageBox.Show("您确定要退出分拣监控系统吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                Util.LogFile.DeleteFile();

                try
                {
                    Application.Exit();
                }
                catch (Exception ex)
                {

                    MessageBox.Show("先关闭所有对话框才能退出系统。" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

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
                Logger.Error("操作作业失败！原因：" + ee.Message);
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
                        MessageBox.Show("上传数据成功。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ee)
            {
                Logger.Error("上传数据失败！原因：" + ee.Message);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Context.ProcessDispatcher.WriteToProcess("SynchroLedProcess", "UnCheck", null);
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
            Context.Processes["SynchroLedProcess"].Resume();
            Context.ProcessDispatcher.WriteToProcess("SynchroLedProcess", "Check", null);
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "help.chm");
        }

        private void SwitchStatus(bool isStart)
        {
            btnDownload.Enabled = !isStart;
            //btnUpload.Enabled = !isStart;
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
                        if (DialogResult.Cancel == MessageBox.Show("还有未分拣的数据，您确定要重新下载数据吗？", "询问", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
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

                            Context.ProcessDispatcher.WriteToProcess("monitorView", "ProgressState", new ProgressState("下载烟道表", 4, 1));
                            table = serverDao.FindChannel(orderDate, batchNo, lineCode);
                            channelDao.InsertChannel(table);
                            System.Threading.Thread.Sleep(100);

                            Context.ProcessDispatcher.WriteToProcess("monitorView", "ProgressState", new ProgressState("下载订单主表", 4, 2));
                            table = serverDao.FindOrderMaster(orderDate, batchNo, lineCode);
                            orderDao.InsertMaster(table);
                            System.Threading.Thread.Sleep(100);

                            Context.ProcessDispatcher.WriteToProcess("monitorView", "ProgressState", new ProgressState("下载订单明细表", 4, 3));
                            table = serverDao.FindOrder(orderDate, batchNo, lineCode);
                            orderDao.InsertOrder(table);

                            Context.ProcessDispatcher.WriteToProcess("monitorView", "ProgressState", new ProgressState("下载手工补货订单明细表", 4, 4));
                            table = serverDao.FindHandleSupply(orderDate, batchNo, lineCode);
                            orderDao.InsertHandleSupply(table);
                            //更新混合烟道数量
                            orderDao.UpdateMixChannelQuantity();

                            serverDao.UpdateBatchStatus(batchID, lineCode);

                            Logger.Info("数据下载完成");
                            Context.ProcessDispatcher.WriteToProcess("SynchroLedProcess", "NewData", null);
                            Context.ProcessDispatcher.WriteToProcess("CurrentOrderProcess", "CurrentOrder", new int[] { 0 });
                            Context.ProcessDispatcher.WriteToProcess("monitorView", "ProgressState", new ProgressState());
                        }
                        else
                            MessageBox.Show("没有需要分拣的订单数据。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("下载数据失败！原因：" + e.Message);
            }
        }
    }
}
