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
    public class LEDProcess: AbstractProcess
    {
        [Serializable]
        public class RestartState
        {
            public bool IsRestart = false;
        }

        private RestartState restartState = new RestartState();

        private LEDUtil ledUtil = null;

        public override void Initialize(Context context)
        {
            try
            {
                restartState = Util.SerializableUtil.Deserialize<RestartState>(true, @".\RestartState.sl");
                ledUtil = new LEDUtil();
                base.Initialize(context);
            }
            catch (Exception e)
            {
                Logger.Error("LEDProcess 初始化失败！原因：" + e.Message);
            }
        }

        public override void Release()
        {
            try
            {
                ledUtil.Release();
                base.Release();
            }
            catch (Exception e)
            {
                Logger.Error("LEDProcess 释放资源失败！原因：" + e.Message);
            }

        }

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

                        //处理新数据标志。
                        restartState.IsRestart = true;
                        Util.SerializableUtil.Serialize(true, @".\RestartState.sl", restartState);

                        //LED显示盘点数据和卷烟品牌
                        Show(false);

                        break;
                    case "Check"://由盘点按钮事件发出
                        if (!restartState.IsRestart)
                        {
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
                        }
                        else
                            Show(true);
                            break;
                    case "UnCheck"://由开始按钮事件发出

                        // 写空仓操作流水号
                        WriteChannelDataToPLC();
                        
                        if (restartState.IsRestart)
                        {
                            restartState.IsRestart = false;
                            Util.SerializableUtil.Serialize(true, @".\RestartState.sl", restartState);                            
                        }

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
                            ledUtil.errChannelAddress.Clear();
                            Show(false);
                        }
                        else
                        {
                            ledUtil.errChannelAddress.Clear();
                            if (!ledUtil.errChannelAddress.ContainsKey(channelAddress))
                                ledUtil.errChannelAddress.Add(channelAddress, channelAddress);
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

        private void Show(bool checkMode, params int [] quantity)
        {
            try
            {
                using (PersistentManager pm = new PersistentManager())
                {
                    OrderDao orderDao = new OrderDao();
                    string sortNo = orderDao.FindMaxSortedMaster();
                    ChannelDao channelDao = new ChannelDao();
                    DataTable channelTable = channelDao.FindChannelQuantity(sortNo);
                    DataRow[] channelRows = channelTable.Select("CHANNELTYPE='立式机'", "CHANNELNAME");
                    if (!restartState.IsRestart && checkMode && quantity != null)
                    {
                        foreach (DataRow  row in channelRows)
                        {
                            row["REMAINQUANTITY"] = Convert.ToInt32(row["REMAINQUANTITY"]) + quantity[Convert.ToInt32(row["CHANNELADDRESS"]) - 1];
                        }
                    }
                    ledUtil.Show(channelRows, checkMode);
                }
            }
            catch (Exception e)
            {
                Logger.Error("LED SHOW 操作失败！原因：" + e.Message);
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
    }
}
