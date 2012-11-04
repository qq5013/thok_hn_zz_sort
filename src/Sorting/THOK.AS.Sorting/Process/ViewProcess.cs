using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using THOK.MCP;
using THOK.Util;
using THOK.AS.Sorting.Dao;
using THOK.AS.Sorting.View;

namespace THOK.AS.Sorting.Process
{
    public class ViewProcess: AbstractProcess
    {
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            THOK.MCP.View.ViewClickArgs e = (THOK.MCP.View.ViewClickArgs)stateItem.State;
            
            switch (e.DeviceClass)
            {   
                case "ͨ����":
                    using (PersistentManager pm = new PersistentManager())
                    {
                        ChannelDao channelDao = new ChannelDao();
                        string channelcode = (1000 + e.DeviceNo).ToString();
                        string CigaretteName = channelDao.FindChanneCigarette(channelcode);
                        string message = string.Format(" {1} �� {0} �� �� �� Ϊ   {2} ", e.DeviceClass, e.DeviceNo, CigaretteName);
                        if (MessageBox.Show(message, "��ѯ", MessageBoxButtons.OKCancel) != DialogResult.OK)
                        {
                            CacheChannelCheckForm CacheChannelCheckForm1 = new CacheChannelCheckForm(e.DeviceClass);
                            CacheChannelCheckForm1.ShowDialog();
                        }
                    }
                    break;
                case "��ʽ��":
                    using (PersistentManager pm = new PersistentManager())
                    {
                        
                        ChannelDao channelDao = new ChannelDao();
                        string message = string.Format("������ѯ��{1} �� {0} �� �� �� ?", e.DeviceClass, e.DeviceNo);
                        if (MessageBox.Show(message, "��ʾ", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            CacheChannelCheckForm CacheChannelCheckForm2 = new CacheChannelCheckForm(e.DeviceNo);
                            CacheChannelCheckForm2.ShowDialog();
                        }
                        else
                        {
                            CacheChannelCheckForm CacheChannelCheckForm3 = new CacheChannelCheckForm(e.DeviceClass);
                            CacheChannelCheckForm3.ShowDialog();
                        }
                    }
                break;
            }
        }
    }
}

