using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Microsoft.Win32;
using THOK.AS.Dal;

namespace THOK.AS.Schedule
{
    public class UploadData
    {
        private Dictionary<string, string> parameter = null;
        private string txtFile = "";
        private string zipFile = "";
        public UploadData()
        {
            parameter = new Dal.ParameterDal().FindParameter();
            txtFile = "RetailerOrder" + System.DateTime.Now.ToString("yyyyMMddHHmmss");
            zipFile = parameter["NoOneProFilePath"] + txtFile + ".zip";
            txtFile = txtFile + ".Order";

            try
            {
                DirectoryInfo dir = new DirectoryInfo(parameter["NoOneProFilePath"]);
                if (!dir.Exists)
                    dir.Create();                
            }
            catch (Exception e)
            {
                ProcessState.Status = "ERROR";
                ProcessState.Message = e.Message;
            }
        }

        public void Upload(string orderDate, int batchNo)
        {
            try
            {
                ProcessState.Status = "PROCESSING";
                ProcessState.TotalCount = 5;
                ProcessState.Step = 1;

                CreateDataFile(orderDate, batchNo);
                ProcessState.CompleteCount = 1;

                CreateZipFile();
                ProcessState.CompleteCount = 2;

                SendZipFile();
                ProcessState.CompleteCount = 3;

                SaveUploadStatus(orderDate, batchNo);
                ProcessState.CompleteCount = 4;

                DeleteFiles();
                ProcessState.CompleteCount = 5;
            }
            catch (Exception e)
            {
                ProcessState.Status = "ERROR";
                ProcessState.Message = e.Message;
            }
        }

        /// <summary>
        /// ���������ļ�
        /// </summary>
        /// <param name="orderDate"></param>
        /// <param name="batchNo"></param>
        private void CreateDataFile(string orderDate, int batchNo)
        {

            FileStream file = new FileStream(parameter["NoOneProFilePath"]  + txtFile, FileMode.Create);            
            StreamWriter writer = new StreamWriter(file,Encoding.UTF8);
            OrderScheduleDal orderDal = new OrderScheduleDal();

            DataTable table = orderDal.GetOrder(orderDate, batchNo,false);
            int columnCount = table.Columns.Count;

            foreach (DataRow row in table.Rows)
            {
                string s = row["SORTNO"].ToString();
                for (int i = 1; i < columnCount; i++)
                    s += ("," + row[i].ToString().Trim());
                s += ";";
                writer.WriteLine(s);
                writer.Flush();
            }

            table = orderDal.GetOrder(orderDate, batchNo, true);
            columnCount = table.Columns.Count;

            foreach (DataRow row in table.Rows)
            {
                string s = row["SORTNO"].ToString();
                for (int i = 1; i < columnCount; i++)
                    s += ("," + row[i].ToString().Trim());
                s += ";";
                writer.WriteLine(s);
                writer.Flush();
            }

            file.Close();
        }

        /// <summary>
        /// ����ѹ���ļ�
        /// </summary>
        private void CreateZipFile()
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
            the_StartInfo.WorkingDirectory = parameter["NoOneProFilePath"];
            the_StartInfo.FileName = the_rar;
            the_StartInfo.Arguments = the_Info;
            the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            zipProcess = new Process();
            zipProcess.StartInfo = the_StartInfo;
            zipProcess.Start();

            //�ȴ�ѹ���ļ������˳�
            while (!zipProcess.HasExited)
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        /// <summary>
        /// ����ѹ���ļ�������һ�Ź���
        /// </summary>
        private void SendZipFile()
        {
            TcpClient client = new TcpClient();

            client.Connect(parameter["NoOneProIP"], Convert.ToInt32(parameter["NoOneProPort"]));
            NetworkStream stream = client.GetStream();

            FileStream file = new FileStream(zipFile, FileMode.Open);
            
            int fileLength = (int)file.Length;
            byte[] fileBytes = BitConverter.GetBytes(fileLength);
            Array.Reverse(fileBytes);
            //�����ļ�����
            stream.Write(fileBytes, 0, 4);
            stream.Flush();

            byte[] data = new byte[1024];
            int len = 0;
            while ((len = file.Read(data, 0, 1024)) > 0)
            {
                stream.Write(data, 0, len);
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
        /// �����ϴ�����״̬
        /// </summary>
        /// <param name="orderDate"></param>
        /// <param name="batchNo"></param>
        private void SaveUploadStatus(string orderDate, int batchNo)
        {
            BatchDal batchDal = new BatchDal();
            batchDal.SaveUploadUser(ProcessState.UserName, orderDate, batchNo);
        }

        /// <summary>
        /// ɾ�������ļ���ѹ���ļ�
        /// </summary>
        private void DeleteFiles()
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(parameter["NoOneProFilePath"]);
                FileInfo[] files = dir.GetFiles("*.Order");

                if (files != null)
                {
                    foreach (FileInfo file in files)
                        file.Delete();
                }

                dir = new DirectoryInfo(parameter["NoOneProFilePath"]);
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
