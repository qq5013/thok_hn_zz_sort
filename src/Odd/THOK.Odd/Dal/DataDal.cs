using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Microsoft.Win32;
using System.Diagnostics;
using THOK.Util;
using THOK.Odd.Dao;

namespace THOK.Odd.Dal
{
    public class DataDal
    {
        internal event ProcessEventHandler OnProcessing = null;

        public void DownloadData(DateTime orderDate, string batchNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                RouteDao routeDao = new RouteDao();
                OrderDao orderDao = new OrderDao();
                CigaretteDao cigaretteDao = new CigaretteDao();
                try
                {
                    pm.BeginTransaction();
                    if (routeDao.Find(orderDate.ToShortDateString(), batchNo) == 0)
                    {
                        using (PersistentManager outpm = new PersistentManager("OuterConnection"))
                        {
                            SaleDao saleDao = new SaleDao();
                            saleDao.SetPersistentManager(outpm);

                            DataTable cigaretteTable = saleDao.FindCigarette();
                            if (OnProcessing != null)
                                OnProcessing(this, new ProcessEventArgs(1, "下载卷烟信息", 1, 2));
                            Refresh();

                            cigaretteDao.Insert(cigaretteTable);
                            if (OnProcessing != null)
                                OnProcessing(this, new ProcessEventArgs(1, "下载卷烟信息", 2, 2));
                            Refresh();

                            orderDao.Clear();
                            if (OnProcessing != null)
                                OnProcessing(this, new ProcessEventArgs(2, "清空订单数据", 1, 1));
                            Refresh();

                            DataTable orderTable = saleDao.FindOrder(orderDate, batchNo);
                            for (int i = 0; i < orderTable.Rows.Count; i++)
                            {
                                orderDao.Insert(orderTable.Rows[i]);
                                if (OnProcessing != null)
                                    OnProcessing(this, new ProcessEventArgs(3, "下载订单数据", i, orderTable.Rows.Count));
                                System.Windows.Forms.Application.DoEvents();
                            }
                            Refresh();

                            orderDao.DeleteExists(orderDate.ToShortDateString());

                            routeDao.Insert(orderDate.ToShortDateString(), batchNo);
                            if (OnProcessing != null)
                                OnProcessing(this, new ProcessEventArgs(4, "下载配送线路", 1, 1));

                        }
                    }
                    else
                        throw new Exception("生产批次订单数据已下载");
                    pm.Commit();
                }
                catch (Exception e)
                {
                    pm.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }

        public void UploadData(string orderDate, string batchNo)
        {
            string txtFile = "RetailerOrder" + System.DateTime.Now.ToString("yyyyMMddHHmmss");
            string zipFile = txtFile + ".zip";
            txtFile = txtFile + ".Order";


            using (PersistentManager pm = new PersistentManager())
            {
                RouteDao routeDao = new RouteDao();
                OrderDao orderDao = new OrderDao();
                CigaretteDao cigaretteDao = new CigaretteDao();
                try
                {
                    pm.BeginTransaction();

                    DataTable orderTable = orderDao.Find(orderDate, batchNo);
                    if (orderTable.Rows.Count != 0)
                    {
                        CreateDataFile(orderTable, txtFile);
                        if (OnProcessing != null)
                            OnProcessing(this, new ProcessEventArgs(1, "生成数据文件", 1, 1));
                        Refresh();

                        CreateZipFile(txtFile, zipFile);
                        if (OnProcessing != null)
                            OnProcessing(this, new ProcessEventArgs(2, "创建压缩文件", 1, 1));
                        Refresh();

                        SendZipFile(zipFile);
                        if (OnProcessing != null)
                            OnProcessing(this, new ProcessEventArgs(3, "发送数据文件", 1, 1));
                        Refresh();

                        DeleteFiles();
                        if (OnProcessing != null)
                            OnProcessing(this, new ProcessEventArgs(4, "删除过程文件", 1, 1));
                    }
                    else
                        throw new Exception("您选择的生产批次已上传一号工程或无异形烟");

                    pm.Commit();

                }
                catch (Exception e)
                {
                    pm.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }

        public void DeleteBatch(string orderDate, string batchNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                RouteDao routeDao = new RouteDao();
                OrderDao orderDao = new OrderDao();

                try
                {
                    pm.BeginTransaction();
                    if (routeDao.FindGreatBatch(orderDate, batchNo) == 0)
                    {
                        routeDao.Delete(orderDate, batchNo);
                        orderDao.Delete(orderDate, batchNo);
                    }
                    else
                        throw new Exception("不是最后一个生产批次");
                }
                catch (Exception e)
                {
                    pm.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }

        private void Refresh()
        {
            System.Windows.Forms.Application.DoEvents();
            Thread.Sleep(500);
        }

        /// <summary>
        /// 创建数据文件
        /// </summary>
        /// <param name="orderDate"></param>
        /// <param name="batchNo"></param>
        private void CreateDataFile(DataTable table, string txtFile)
        {
            FileStream file = new FileStream(txtFile, FileMode.Create);
            
            StreamWriter writer = new StreamWriter(file,Encoding.UTF8);

            int columnCount = table.Columns.Count;
            int corder = 0;
            string customerCode = "";
            foreach (DataRow row in table.Rows)
            {
                if (row["CUSTOMERCODE"].ToString() != customerCode)
                {
                    customerCode = row["CUSTOMERCODE"].ToString();
                    corder++;
                }
                row["CORDER"] = corder;
                string s = row[0].ToString();
                for (int i = 1; i < columnCount; i++)
                    s += ("," + row[i].ToString().Trim());
                s += ";";
                writer.WriteLine(s);
                writer.Flush();
            }

            file.Close();
        }

        /// <summary>
        /// 创建压缩文件
        /// </summary>
        private void CreateZipFile(string txtFile, string zipFile)
        {
            String the_rar;
            RegistryKey the_Reg;
            Object the_Obj;
            String the_Info;
            ProcessStartInfo the_StartInfo;
            Process zipProcess;

            the_Reg = Registry.ClassesRoot.OpenSubKey("Applications\\WinRAR.exe\\Shell\\Open\\Command");
            the_Obj = the_Reg.GetValue("");
            the_rar = the_Obj.ToString();
            the_Reg.Close();
            the_rar = the_rar.Substring(1, the_rar.Length - 7);
            the_Info = " a    " + zipFile + "  " + txtFile;
            the_StartInfo = new ProcessStartInfo();
            the_StartInfo.WorkingDirectory = @".\";
            the_StartInfo.FileName = the_rar;
            the_StartInfo.Arguments = the_Info;
            the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            zipProcess = new Process();
            zipProcess.StartInfo = the_StartInfo;
            zipProcess.Start();

            //等待压缩文件进程退出
            while (!zipProcess.HasExited)
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 发送压缩文件给中软一号工程
        /// </summary>
        private void SendZipFile(string zipFile)
        {
            TcpClient client = new TcpClient();

            client.Connect("10.73.68.17", 9050);
            NetworkStream stream = client.GetStream();

            FileStream file = new FileStream(zipFile, FileMode.Open);
            
            int fileLength = (int)file.Length;
            byte[] fileBytes = BitConverter.GetBytes(fileLength);
            Array.Reverse(fileBytes);
            //发送文件长度
            stream.Write(fileBytes, 0, 4);
            stream.Flush();

            byte[] data = new byte[1024];
            int len = 0;
            while ((len = file.Read(data, 0, 1024)) > 0)
            {
                stream.Write(data, 0, len);
                if (OnProcessing != null)
                    OnProcessing(this, new ProcessEventArgs(3, "发送数据文件", len, fileLength));
            }

            file.Close();

            byte[] buffer = new byte[1024];
            string recvStr = "";
            while (true)
            {
                int bytes = stream.Read(buffer, 0, 1024);

                if (bytes <= 0)
                    continue;
                else
                {
                    recvStr = Encoding.ASCII.GetString(buffer, bytes - 3, 2);
                    if (recvStr == "##")
                    {
                        recvStr = Encoding.Default.GetString(buffer, 4, bytes - 5);
                        break;
                    }
                }
            }
            client.Close();

            if (recvStr.Split(';').Length > 2)
            {
                throw new Exception(recvStr);
            }        
        }

        /// <summary>
        /// 删除数据文件和压缩文件
        /// </summary>
        private void DeleteFiles()
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(@".\");
                FileInfo[] files = dir.GetFiles("*.Order");

                if (files != null)
                {
                    foreach (FileInfo file in files)
                        file.Delete();
                }

                dir = new DirectoryInfo(@".\");
                if (dir.Exists)
                {
                    files = dir.GetFiles("*.zip");
                    if (files != null)
                    {
                        foreach (FileInfo file in files)
                            file.Delete();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
