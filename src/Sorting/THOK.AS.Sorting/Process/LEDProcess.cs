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
                Logger.Error("LEDProcess ��ʼ��ʧ�ܣ�ԭ��" + e.Message);
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
                Logger.Error("LEDProcess �ͷ���Դʧ�ܣ�ԭ��" + e.Message);
            }

        }

        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            try
            {
                switch (stateItem.ItemName)
                {
                    case "NewData"://�����ذ�ť�¼�����  
                        // д�ղֲ�����ˮ��
                        WriteChannelDataToPLC();
                        //д���¿�ʼ�ּ��־
                        WriteRestartDataToPLC();

                        //���������ݱ�־��
                        restartState.IsRestart = true;
                        Util.SerializableUtil.Serialize(true, @".\RestartState.sl", restartState);

                        //LED��ʾ�̵����ݺ;���Ʒ��
                        Show(false);

                        break;
                    case "Check"://���̵㰴ť�¼�����
                        if (!restartState.IsRestart)
                        {
                            object state = Context.Services["SortPLC"].Read("Check");
                            if (state is Array)
                            {
                                Array array = (Array)state;
                                if (array.Length == 86)
                                {
                                    //LED��ʾ�̵����ݺ;���Ʒ��
                                    int[] quantity = new int[86];
                                    array.CopyTo(quantity, 0);
                                    Show(true, quantity);
                                }
                            }
                        }
                        else
                            Show(true);
                            break;
                    case "UnCheck"://�ɿ�ʼ��ť�¼�����

                        // д�ղֲ�����ˮ��
                        WriteChannelDataToPLC();
                        
                        if (restartState.IsRestart)
                        {
                            restartState.IsRestart = false;
                            Util.SerializableUtil.Serialize(true, @".\RestartState.sl", restartState);                            
                        }

                        //��ʱ�����������ּ𶩵������̣߳�����װ���������̡߳�                        
                        if (Context.Processes["OrderRequestProcess"] != null)
                        {
                            Context.Processes["OrderRequestProcess"].Resume();
                        }

                        if (Context.Processes["PackRequestProcess"] != null)
                        {
                            Context.Processes["PackRequestProcess"].Resume();
                        }

                        //LED����ʾ�̵����ݣ�ֻ��ʾ����Ʒ��
                        Show(false);

                        break;
                    case "EmptyErr":
                        //ȱ�̱���
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
                Logger.Error("LED ���²���ʧ�ܣ�ԭ��" + e.Message);
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
                    DataRow[] channelRows = channelTable.Select("CHANNELTYPE='��ʽ��'", "CHANNELNAME");
                    if (!restartState.IsRestart && checkMode && quantity != null)
                    {
                        foreach (DataRow row in channelRows)
                        {
                            row["REMAINQUANTITY"] = Convert.ToInt32(row["REMAINQUANTITY"]) + quantity[Convert.ToInt32(row["CHANNELADDRESS"]) - 1];
                        }
                    }
                    ledUtil.Show(channelRows, checkMode);
                }
            }
            catch (Exception e)
            {
                Logger.Error("LED SHOW ����ʧ�ܣ�ԭ��" + e.Message);
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
                Logger.Error("д�ղֲ���ʧ�ܣ�ԭ��" + e.Message);
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
                Logger.Error("д���·ּ��־����ʧ�ܣ�ԭ��" + e.Message);
            }            
        }
    }
}
