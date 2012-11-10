using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using THOK.MCP;
using THOK.AS.Sorting.Dao;
using THOK.Util;
using THOK.AS.Sorting.Util;

namespace THOK.AS.Sorting.Process
{
    public class OrderRequestProcess: AbstractProcess
    {
        private MessageUtil messageUtil = null;

        public override void Initialize(Context context)
        {
            try
            {
                base.Initialize(context);
                messageUtil = new MessageUtil(context.Attributes);
            }
            catch (Exception e)
            {
                Logger.Error("OrderRequestProcess ��ʼ��ʧ�ܣ�ԭ��" + e.Message);
            }

        }

        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            try
            {
                //��ȡ������ϸ��д��PLC
                if (ObjectUtil.GetObject(stateItem.State).ToString() == "1")
                {
                    using (PersistentManager pm = new PersistentManager())
                    {
                        OrderDao orderDao = new OrderDao();
                        try
                        {
                            DataTable masterTable = orderDao.FindSortMaster();
                            
                            if (masterTable.Rows.Count != 0)
                            {
                                string maxSortNo = orderDao.FindMaxSortNoFromMasterByOrderID(masterTable.Rows[0]["ORDERID"].ToString());
                                string sortNo = masterTable.Rows[0]["SORTNO"].ToString();
                                int quantity = Convert.ToInt32(masterTable.Rows[0]["QUANTITY"]);

                                DataTable detailTable = orderDao.FindSortDetail(sortNo);
                                //��ѯ���ּ����������ˮ�ţ��ж��Ƿ����
                                string endSortNo = orderDao.FindEndSortNo();
                                int[] orderData = new int[90];
                                for (int i = 0; i < detailTable.Rows.Count; i++)
                                {
                                    orderData[Convert.ToInt32(detailTable.Rows[i]["CHANNELADDRESS"]) - 1] = Convert.ToInt32(detailTable.Rows[i]["QUANTITY"]);
                                }

                                orderData[86] = Convert.ToInt32(sortNo);
                                orderData[87] = quantity;
                                orderData[88] = maxSortNo == sortNo ? 1 : 0;
                                orderData[89] = 1;
                                
                                //�ּ���ˮ��
                                orderData[90] = Convert.ToInt32(sortNo);
                                //��������
                                orderData[91] = quantity;
                                //�Ƿ񻻻�
                                orderData[92] = maxSortNo == sortNo ? 1 : 0;
                                //�ͻ��ּ���ˮ��
                                orderData[93] = Convert.ToInt32(masterTable.Rows[0]["CUSTOMERSORTNO"].ToString());
                                //��װ����
                                orderData[94] = 0;
                                //���ּ���·�Ƿ����
                                orderData[95] = endSortNo == sortNo ? 1 : 0;
                                //��ɱ�־
                                orderData[96] = 1;

                                if (dispatcher.WriteToService("SortPLC", "OrderData", orderData))
                                {
                                    orderDao.UpdateOrderStatus(sortNo, "1");
                                    Logger.Info(string.Format("д�������ݳɹ�,�ּ𶩵���[{0}]��", sortNo));

                                    //���Ͷ����Ÿ��ּ�����ն�ϵͳ
                                    if (sortNo == "1")
                                    {
                                        messageUtil.SendToExport(sortNo);     
                                    }
                                                                 
                                }

                            }
                        }
                        catch (Exception e)
                        {
                            Logger.Error("д��������ʧ�ܣ�ԭ��" + e.Message);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Logger.Error("�ּ𶩵��������ʧ�ܣ�ԭ��" + ee.Message);
            }

        }
    }
}
