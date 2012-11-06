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
                    case "NewData"://�����ذ�ť�¼�����  
                        // д�ղֲ�����ˮ��
                        WriteChannelDataToPLC();
                        //д���¿�ʼ�ּ��־
                        WriteRestartDataToPLC();           
                        //LED��ʾ�̵����ݺ;���Ʒ��
                        Show(false);

                        break;
                    case "Check"://���̵㰴ť�¼�����
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
                        else
                            Show(true);
                        break;
                    case "UnCheck"://�ɿ�ʼ��ť�¼�����

                        // д�ղֲ�����ˮ��
                        WriteChannelDataToPLC();
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
                            //ledUtil.errChannelAddress.Clear();
                            Show(false);
                        }
                        else
                        {
                            //ledUtil.errChannelAddress.Clear();
                            //if (!ledUtil.errChannelAddress.ContainsKey(channelAddress))
                            //    ledUtil.errChannelAddress.Add(channelAddress, channelAddress);
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
                        //����δ���͸�plc��ʣ����̵�����
                        ChannelDao channelDao = new ChannelDao();
                        DataTable channelInfo = channelDao.FindChannelInfos();
                        //��ʵ����
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
                Logger.Error("LED SHOW ����ʧ�ܣ�ԭ��" + e.Message);
            }
        }
    }
}
