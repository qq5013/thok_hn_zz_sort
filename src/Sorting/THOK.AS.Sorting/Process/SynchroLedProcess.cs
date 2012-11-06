using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.MCP;
using THOK.Util;
using THOK.AS.Sorting.Dao;
using THOK.AS.Sorting.Util;

namespace THOK.AS.Sorting.Process
{
    public class SynchroLedProcess : AbstractProcess
    {
        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            try
            {
                switch (stateItem.ItemName)
                {
                    case "NewData"://由下载按钮事件发出  
                        // 写空仓操作流水号
                        WriteChannelDataToPLC();
                        //写重新开始分拣标志
                        WriteRestartDataToPLC();           
                        //LED显示盘点数据和卷烟品牌
                        Show(false);
                        break;
                    case "Check"://由盘点按钮事件发出
                        object state = Context.Services["SortPLC"].Read("Check");
                        if (state is Array)
                        {
                            Array array = (Array)state;
                            if (array.Length == 86)
                            {
                                //LED显示盘点数据和卷烟品牌
                                int[] quantity = new int[86];
                                array.CopyTo(quantity, 0);
                                Show(true, quantity);
                            }
                        }
                        else
                            Show(true);
                        break;
                    case "UnCheck"://由开始按钮事件发出

                        // 写空仓操作流水号
                        WriteChannelDataToPLC();
                        //延时处理，再启动分拣订单处理线程，及包装订单处理线程。                        
                        if (Context.Processes["OrderRequestProcess"] != null)
                        {
                            Context.Processes["OrderRequestProcess"].Resume();
                        }

                        if (Context.Processes["PackRequestProcess"] != null)
                        {
                            Context.Processes["PackRequestProcess"].Resume();
                        }

                        //LED不显示盘点数据，只显示卷烟品牌
                        Show(false);

                        break;
                    case "EmptyErr":
                        //缺烟报警
                        object o = ObjectUtil.GetObject(stateItem.State);
                        int channelAddress = Convert.ToInt32(o);
                        if (channelAddress == 0)
                        {
                            Show(false);
                        }
                        else
                        {
                            Show(false);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Logger.Error("LED 更新操作失败！原因：" + e.Message);
            }

        }

        private void WriteChannelDataToPLC()
        {
            try
            {
                using (PersistentManager pm = new PersistentManager())
                {
                    ChannelDao channelDao = new ChannelDao();
                    DataTable channelTable = channelDao.FindLastSortNo();
                    int[] channelData = new int[86];
                    for (int i = 0; i < channelTable.Rows.Count; i++)
                    {
                        channelData[Convert.ToInt32(channelTable.Rows[i]["CHANNELADDRESS"]) - 1] = Convert.ToInt32(channelTable.Rows[i]["SORTNO"]);
                    }

                    WriteToService("SortPLC", "ChannelData", channelData);
                }
            }
            catch (Exception e)
            {
                Logger.Error("写空仓操作失败！原因：" + e.Message);
            }
        }

        private void WriteRestartDataToPLC()
        {
            try
            {
                WriteToService("SortPLC", "RestartData", 1);
            }
            catch (Exception e)
            {
                Logger.Error("写重新分拣标志操作失败！原因：" + e.Message);
            }
        }

        private void Show(bool checkMode, params int[] quantity)
        {
            try
            {
                using (PersistentManager pm = new PersistentManager())
                {
                    if (!checkMode)
                    {
                        ChannelDao channelDao = new ChannelDao();
                        WriteToService("LedBarScreen", "SendToLed", channelDao.FindChannelInfo());
                    }
                    else
                    {
                        //汇总未发送给plc的剩余的烟的烟量
                        ChannelDao channelDao = new ChannelDao();
                        DataTable channelInfo = channelDao.FindChannelInfos();
                        //真实数量
                        for (int i = 10, j = 0; i < 70; i++, j++)
                        {
                            channelInfo.Rows[j]["UQUANTITY"] = Convert.ToInt32(channelInfo.Rows[j]["UQUANTITY"]) + quantity[i];
                            channelInfo.Rows[j]["FQUANTITY"] = Convert.ToInt32(channelInfo.Rows[j]["QUANTITY"]) - Convert.ToInt32(channelInfo.Rows[j]["UQUANTITY"]);
                        }
                        WriteToService("LedBarScreen", "Check", channelInfo);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("LED SHOW 操作失败！原因：" + e.Message);
            }
        }
    }
}
