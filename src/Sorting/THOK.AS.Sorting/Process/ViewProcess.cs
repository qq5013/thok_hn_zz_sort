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
                case "通道机":
                    using (PersistentManager pm = new PersistentManager())
                    {
                        ChannelDao channelDao = new ChannelDao();
                        string channelcode = (1000 + e.DeviceNo).ToString();
                        string CigaretteName = channelDao.FindChanneCigarette(channelcode);
                        string message = string.Format(" {1} 号 {0} 的 卷 烟 为   {2} ", e.DeviceClass, e.DeviceNo, CigaretteName);
                        if (MessageBox.Show(message, "查询", MessageBoxButtons.OKCancel) != DialogResult.OK)
                        {
                            CacheChannelCheckForm CacheChannelCheckForm1 = new CacheChannelCheckForm(e.DeviceClass);
                            CacheChannelCheckForm1.ShowDialog();
                        }
                    }
                    break;
                case "立式机":
                    using (PersistentManager pm = new PersistentManager())
                    {
                        
                        ChannelDao channelDao = new ChannelDao();
                        string message = string.Format("即将查询：{1} 号 {0} 的 卷 烟 ?", e.DeviceClass, e.DeviceNo);
                        if (MessageBox.Show(message, "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
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
                case "链板段":
                    int[] cacheAfter = new int[40];
                    object stateCacheAfter = Context.Services["SortPLC"].Read("CacheSortNoes");
                    if (stateCacheAfter is Array)
                    {
                        Array arrayCacheAfter = (Array)stateCacheAfter;
                        if (arrayCacheAfter.Length == 40)
                        {
                            arrayCacheAfter.CopyTo(cacheAfter, 0);
                        }
                    }
                    CacheOrderQueryForm CacheOrderQueryForm1 = new CacheOrderQueryForm(e.DeviceClass, cacheAfter);
                    CacheOrderQueryForm1.ShowDialog();
                    break;
                case "打码段":
                    int[] cacheBefore = new int[40];
                    object stateCacheBefore = Context.Services["SortPLC"].Read("CacheSortNoes");
                    if (stateCacheBefore is Array)
                    {
                        Array arrayCacheBefore = (Array)stateCacheBefore;
                        if (arrayCacheBefore.Length == 40)
                        {
                            arrayCacheBefore.CopyTo(cacheBefore, 0);
                        }
                    }
                    CacheOrderQueryForm CacheOrderQueryForm2 = new CacheOrderQueryForm(e.DeviceClass, cacheBefore);
                    CacheOrderQueryForm2.ShowDialog();
                    break;
            }
        }
    }
}

