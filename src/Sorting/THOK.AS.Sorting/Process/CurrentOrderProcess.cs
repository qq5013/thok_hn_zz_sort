using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.MCP;
using THOK.AS.Sorting.Util;
using THOK.AS.Sorting.Dao;
using THOK.AS.Sorting.View;
using THOK.Util;

namespace THOK.AS.Sorting.Process
{
    public class CurrentOrderProcess: AbstractProcess
    {
        private MessageUtil messageUtil = null;
        private List<string> routeMaxSortNoList = new List<string>();

        public override void Initialize(Context context)
        {
            try
            {
                base.Initialize(context);

                using (PersistentManager pm = new PersistentManager())
                {
                    OrderDao orderDao = new OrderDao();
                    routeMaxSortNoList = orderDao.FindRouteMaxSortNoList();
                }
              
                messageUtil = new MessageUtil(context.Attributes);
            }
            catch (Exception e)
            {
                Logger.Error("CurrentOrderProcess ��ʼ��ʧ�ܣ�ԭ��" + e.Message);
            }

        }

        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            try
            {
                object o = ObjectUtil.GetObject(stateItem.State);
                if (o != null)
                {
                    string sortNo = o.ToString();
                    if (sortNo == "0")
                    {
                        using (PersistentManager pm = new PersistentManager())
                        {
                            OrderDao orderDao = new OrderDao();
                            routeMaxSortNoList = orderDao.FindRouteMaxSortNoList();
                        }
                    }

                    if (routeMaxSortNoList.Contains(sortNo))
                    {
                        WriteToService("SortPLC", "RouteChannageTag", 1);
                    }
                    //ˢ�·ּ�״̬
                    Refresh(sortNo, dispatcher);
                    
                    //���Ͷ����Ÿ� �ּ�����ն�ϵͳ
                    if (Convert.ToInt32(sortNo) > 0)
                    {
                        sortNo = Convert.ToString(Convert.ToInt32(sortNo) + 1);
                        messageUtil.SendToExport(sortNo);
                    }
                }
            }
            catch (Exception e)
            {                
                Logger.Error("��ɶ�����Ϣ����ʧ�ܣ�ԭ��" + e.Message);
            }
        }

        private void Refresh(string sortNo, IProcessDispatcher dispatcher)
        {
            try
            {
                using (PersistentManager pm = new PersistentManager())
                {
                    OrderDao orderDao = new OrderDao();
                    if (sortNo == null)
                        sortNo = orderDao.FindLastSortNo();

                    //�������ʱ��
                    orderDao.UpdateFinisheTime(sortNo);

                    //ˢ��������ּ�״̬
                    DataTable infoTable = orderDao.FindOrderInfo(null);
                    RefreshData refreshData = new RefreshData();
                    refreshData.TotalCustomer = Convert.ToInt32(infoTable.Rows[0]["CUSTOMERNUM"]);
                    refreshData.TotalRoute = Convert.ToInt32(infoTable.Rows[0]["ROUTENUM"]);
                    refreshData.TotalQuantity = Convert.ToInt32(infoTable.Rows[0]["QUANTITY"]);

                    infoTable = orderDao.FindOrderInfo(sortNo);
                    refreshData.CompleteCustomer = Convert.ToInt32(infoTable.Rows[0]["CUSTOMERNUM"]);
                    refreshData.CompleteRoute = Convert.ToInt32(infoTable.Rows[0]["ROUTENUM"]);
                    refreshData.CompleteQuantity = Convert.ToInt32(infoTable.Rows[0]["QUANTITY"]);

                    refreshData.Average = orderDao.FindSortingAverage();

                    dispatcher.WriteToProcess("sortingStatus", "RefreshData", refreshData);
                    messageUtil.SendToSortLed(sortNo, refreshData);
                }
            }
            catch (Exception e)
            {
                Logger.Error("���·ּ���Ϣ����ʧ�ܣ�ԭ��" + e.Message);
            }

        }
    }
}
