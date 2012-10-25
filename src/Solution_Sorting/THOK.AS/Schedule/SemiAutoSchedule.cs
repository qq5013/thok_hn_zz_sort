using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.AS.Schedule;
using THOK.AS.Dao;
using THOK.Util;

using System.Threading;

namespace THOK.AS.Schedule
{
    public class SemiAutoSchedule
    {
        public event ScheduleEventHandler OnSchedule = null;

        public void DownloadData(string orderDate, int batchNo)
        {
            try
            {
                DateTime dtOrder = DateTime.Parse(orderDate);
                string historyDate = dtOrder.AddDays(-7).ToShortDateString();
                using (PersistentManager pm = new PersistentManager())
                {
                    BatchDao batchDao = new BatchDao();
                    using (PersistentManager ssPM = new PersistentManager("OuterConnection"))
                    {
                        SalesSystemDao ssDao = new SalesSystemDao();
                        ssDao.SetPersistentManager(ssPM);
                        try
                        {
                            pm.BeginTransaction();

                            //AS_BI_BATCH
                            batchDao.DeleteHistory(historyDate);
                            if (OnSchedule != null)
                                OnSchedule(this, new ScheduleEventArgs(1, "�������������", 1, 14));

                            //AS_SC_CHANNELUSED
                            ChannelScheduleDao csDao = new ChannelScheduleDao();
                            csDao.DeleteHistory(historyDate);
                            if (OnSchedule != null)
                                OnSchedule(this, new ScheduleEventArgs(1, "�������������", 2, 14));

                            //AS_SC_LINE
                            LineScheduleDao lsDao = new LineScheduleDao();
                            lsDao.DeleteHistory(historyDate);
                            if (OnSchedule != null)
                                OnSchedule(this, new ScheduleEventArgs(1, "�������������", 3, 14));

                            //AS_SC_PALLETMASTER ,AS_SC_PALLETDETAIL,AS_SC_ORDER
                            OrderScheduleDao osDao = new OrderScheduleDao();
                            osDao.DeleteHistory(historyDate);
                            if (OnSchedule != null)
                                OnSchedule(this, new ScheduleEventArgs(1, "�������������", 4, 14));

                            //AS_I_ORDERMASTER,AS_I_ORDERDETAIL,
                            OrderDao orderDao = new OrderDao();
                            orderDao.DeleteHistory(historyDate);
                            if (OnSchedule != null)
                                OnSchedule(this, new ScheduleEventArgs(1, "�������������", 5, 14));

                            //AS_SC_STOCKMIXCHANNEL
                            StockChannelDao scDao = new StockChannelDao();
                            scDao.DeleteHistory(historyDate);
                            if (OnSchedule != null)
                                OnSchedule(this, new ScheduleEventArgs(1, "�������������", 6, 14));

                            //AS_SC_SUPPLY
                            SupplyDao supplyDao = new SupplyDao();
                            supplyDao.DeleteHistory(historyDate);
                            if (OnSchedule != null)
                                OnSchedule(this, new ScheduleEventArgs(1, "�������������", 7, 14));

                            //AS_SC_HANDLESUPPLY
                            HandleSupplyDao handleSupplyDao = new HandleSupplyDao();
                            handleSupplyDao.DeleteHistory(historyDate);
                            if (OnSchedule != null)
                                OnSchedule(this, new ScheduleEventArgs(1, "�������������", 8, 14));

                            ClearSchedule(orderDate, batchNo);

                            //���������
                            AreaDao areaDao = new AreaDao();
                            areaDao.Clear();
                            DataTable areaTable = ssDao.FindArea();
                            areaDao.BatchInsertArea(areaTable);
                            if (OnSchedule != null)
                                OnSchedule(this, new ScheduleEventArgs(1, "�������������", 9, 14));

                            //����������·��
                            RouteDao routeDao = new RouteDao();
                            routeDao.Clear();
                            DataTable routeTable = ssDao.FindRoute();
                            routeDao.BatchInsertRoute(routeTable);
                            if (OnSchedule != null)
                                OnSchedule(this, new ScheduleEventArgs(1, "�������������", 10, 14));

                            //���ؿͻ���
                            CustomerDao customerDao = new CustomerDao();
                            customerDao.Clear();
                            DataTable customerTable = ssDao.FindCustomer();
                            customerDao.BatchInsertCustomer(customerTable);
                            if (OnSchedule != null)
                                OnSchedule(this, new ScheduleEventArgs(1, "�������������", 11, 14));

                            //���ؾ��̱� ����ͬ��
                            CigaretteDao cigaretteDao = new CigaretteDao();
                            DataTable cigaretteTable = ssDao.FindCigarette();
                            cigaretteDao.SynchronizeCigarette(cigaretteTable);
                            if (OnSchedule != null)
                                OnSchedule(this, new ScheduleEventArgs(1, "�������������", 12, 14));

                            //������������ּ��߶�Ӧ��ϵ
                            string routes = lsDao.FindRoutes(orderDate);
                            //DataTable lineTable = ssDao.FindLineSchedule(dtOrder, batchNo, routes);
                            //LineScheduleDao lcDao = new LineScheduleDao();
                            //lcDao.SaveLineSchedule(lineTable, orderDate, batchNo);

                            //���ض�������
                            DataTable masterTable = ssDao.FindOrderMaster(dtOrder, batchNo, routes);
                            orderDao.BatchInsertMaster(masterTable);
                            if (OnSchedule != null)
                                OnSchedule(this, new ScheduleEventArgs(1, "�������������", 13, 14));

                            //���ض�����ϸ
                            DataTable detailTable = ssDao.FindOrderDetail(dtOrder, routes);
                            orderDao.BatchInsertDetail(detailTable);
                            if (OnSchedule != null)
                                OnSchedule(this, new ScheduleEventArgs(1, "�������������", 14, 14));

                            pm.Commit();
                        }
                        catch (Exception e)
                        {
                            pm.Rollback();
                            if (OnSchedule != null)
                                OnSchedule(this, new ScheduleEventArgs(OptimizeStatus.ERROR, e.Message));
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                if (OnSchedule != null)
                    OnSchedule(this, new ScheduleEventArgs(OptimizeStatus.ERROR, ee.Message));
            }
        }

        public void GenSchedule(string orderDate, int batchNo)
        {
            try
            {
                //�������Ż�
                GenLineSchedule(orderDate, batchNo);

                //�����̵��Ż�
                GenStockChannelSchedule(orderDate, batchNo);

                //�������̵��Ż�
                GenChannelSchedule(orderDate, batchNo);

                //�����Ż�
                GenOrderScheduleV(orderDate, batchNo);

                //�ֹ������Ż�
                GenHandleSupplySchedule(orderDate, batchNo);

                //�����Ż�
                GenSupplySchedule(orderDate, batchNo);

                //����Ϊ���Ż�
                using (PersistentManager pm = new PersistentManager())
                {
                    BatchDao batchDao = new BatchDao();
                    batchDao.UpdateIsValid(orderDate, batchNo, "1");
                }

                if (OnSchedule != null)
                    OnSchedule(this, new ScheduleEventArgs(OptimizeStatus.COMPLETE));
                
            }
            catch (Exception e)
            {
                ClearSchedule(orderDate, batchNo);
                if (OnSchedule != null)
                    OnSchedule(this, new ScheduleEventArgs(OptimizeStatus.ERROR, e.Message));
            }
        }

        /// <summary>
        /// ���ָ����������
        /// </summary>
        /// <param name="orderDate"></param>
        /// <param name="batchNo"></param>
        public void ClearSchedule(string orderDate, int batchNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {         
                //AS_BI_BATCH
                BatchDao batchDao = new BatchDao();
                batchDao.UpdateExecuter("0", "0", orderDate, batchNo);
                batchDao.UpdateIsValid(orderDate, batchNo, "0");

                //AS_SC_CHANNELUSED
                ChannelScheduleDao csDao = new ChannelScheduleDao();
                csDao.DeleteSchedule(orderDate, batchNo);

                //AS_SC_LINE
                LineScheduleDao lsDao = new LineScheduleDao();
                lsDao.DeleteSchedule(orderDate, batchNo);

                //AS_SC_PALLETMASTER,AS_SC_PALLETDETAIL,AS_SC_ORDER
                OrderScheduleDao osDao = new OrderScheduleDao();
                osDao.DeleteSchedule(orderDate, batchNo);

                //AS_I_ORDERDETAIL��AS_I_ORDERMASTER
                OrderDao orderDao = new OrderDao();
                orderDao.DeleteOrder(orderDate, batchNo);

                //AS_SC_STOCKMIXCHANNEL
                StockChannelDao scDao = new StockChannelDao();
                scDao.DeleteSchedule(orderDate, batchNo);

                //AS_SC_SUPPLY
                SupplyDao supplyDao = new SupplyDao();
                supplyDao.DeleteSchedule(orderDate, batchNo);

                //AS_SC_HANDLESUPPLY
                HandleSupplyDao handleSupplyDao = new HandleSupplyDao();
                handleSupplyDao.DeleteHandleSupply(orderDate, batchNo);

            }
        }

        /// <summary>
        /// �������Ż�  2008-12-11�޸� 
        /// </summary>
        /// <param name="orderDate"></param>
        /// <param name="batchNo"></param>
        public void GenLineSchedule(string orderDate, int batchNo)
        {
            using (THOK.Util.PersistentManager pm = new THOK.Util.PersistentManager())
            {
                LineInfoDao lineDao = new LineInfoDao();
                OrderDao detailDao = new OrderDao();
                LineScheduleDao lineScDao = new LineScheduleDao();

                THOK.Optimize.LineOptimize lineSchedule = new THOK.Optimize.LineOptimize();

                DataTable routeTable = detailDao.FindRouteQuantity(orderDate, batchNo).Tables[0];
                DataTable lineTable = lineDao.GetAvailabeLine().Tables[0];

                DataTable scLineTable = lineSchedule.Optimize(routeTable, lineTable, orderDate, batchNo);
                lineScDao.SaveLineSchedule(scLineTable);
                if (OnSchedule != null)
                    OnSchedule(this, new ScheduleEventArgs(2, "�������Ż�", 1, 1));
            }     
        }

        /// <summary>
        /// �����̵��Ż�2010-04-19
        /// </summary>
        /// <param name="orderDate"></param>
        /// <param name="batchNo"></param>
        public void GenStockChannelSchedule(string orderDate, int batchNo)
        {
            using (THOK.Util.PersistentManager pm = new THOK.Util.PersistentManager())
            {
                THOK.AS.Dao.StockChannelDao schannelDao = new StockChannelDao();
                THOK.AS.Dao.OrderDao orderDao = new OrderDao();
                THOK.AS.Dao.SysParameterDao parameterDao = new SysParameterDao();
                Dictionary<string, string> parameter = parameterDao.FindParameters();
                
                //ÿ��ּ�����󱸻��̵��Ƿ�Ϊ��
                if (parameter["ClearStockChannel"] == "1")
                    schannelDao.ClearCigarette();

                DataTable channelTable = schannelDao.FindChannel();
                DataTable orderTable = orderDao.FindCigaretteQuantity(orderDate, batchNo);
                THOK.Optimize.StockOptimize stockOptimize = new THOK.Optimize.StockOptimize();

                DataTable mixTable = stockOptimize.Optimize(channelTable, orderTable, orderDate, batchNo);
                schannelDao.UpdateChannel(channelTable);
                schannelDao.InsertMixChannel(mixTable);
                if (OnSchedule != null)
                    OnSchedule(this, new ScheduleEventArgs(3, "�����̵��Ż�", 1, 1));
            }
        }

        /// <summary>
        /// �̵��Ż� 2008-12-11
        /// </summary>
        /// <param name="orderDate"></param>
        /// <param name="batchNo"></param>
        public void GenChannelSchedule(string orderDate, int batchNo)
        {
            using (THOK.Util.PersistentManager pm = new THOK.Util.PersistentManager())
            {
                THOK.AS.Dao.ChannelDao channelDao = new ChannelDao();
                THOK.AS.Dao.OrderDao detailDao = new OrderDao();
                THOK.AS.Dao.LineScheduleDao lineDao = new LineScheduleDao();
                THOK.AS.Dao.LineDeviceDao deviceDao = new LineDeviceDao();
                THOK.AS.Dao.SysParameterDao parameterDao = new SysParameterDao();
                Dictionary<string, string> parameter = parameterDao.FindParameters();

                THOK.Optimize.ChannelOptimize channelSchedule = new THOK.Optimize.ChannelOptimize();

                DataTable lineTable = lineDao.FindAllLine(orderDate, batchNo).Tables[0];

                int currentCount = 0;
                int totalCount = lineTable.Rows.Count;
                
                foreach (DataRow lineRow in lineTable.Rows)
                {
                    string lineCode = lineRow["LINECODE"].ToString();

                    DataTable channelTable = channelDao.FindAvailableChannel(lineCode).Tables[0];

                    DataTable orderTable = detailDao.FindCigaretteQuantity(orderDate, batchNo, lineCode).Tables[0];//���̼�������������5������

                    DataTable deviceTable = deviceDao.FindLineDevice(lineCode).Tables[0];

                    channelSchedule.Optimize(orderTable, channelTable, deviceTable);

                    channelDao.SaveChannelSchedule(channelTable, orderDate, batchNo);
                    
                    if (OnSchedule != null)
                        OnSchedule(this, new ScheduleEventArgs(4, "�����Ż�" + lineRow["LINECODE"].ToString() + "�ּ����̵�", ++currentCount, totalCount));
                }
            }
        }

        /// <summary>
        /// �����Ż� 2010-04-10
        /// </summary>
        /// <param name="orderDate"></param>
        /// <param name="batchNo"></param>
        public void GenOrderScheduleV(string orderDate, int batchNo)
        {
            using (THOK.Util.PersistentManager pm = new THOK.Util.PersistentManager())
            {

                THOK.AS.Dao.OrderDao orderDao = new OrderDao();
                THOK.AS.Dao.ChannelDao channelDao = new ChannelDao();
                THOK.AS.Dao.OrderScheduleDao orderScheduleDao = new OrderScheduleDao();
                THOK.AS.Dao.LineScheduleDao lineDao = new LineScheduleDao();
                THOK.AS.Dao.SupplyDao supplyDao = new SupplyDao();
                THOK.AS.Dao.SysParameterDao parameterDao = new SysParameterDao();

                THOK.Optimize.OrderOptimizeV orderSchedule = new THOK.Optimize.OrderOptimizeV();

                DataTable lineTable = lineDao.FindAllLine(orderDate, batchNo).Tables[0];

                //��������������
                int breakQuantity = 17;
                int margin = 0;
                Dictionary<string, string> parameter = parameterDao.FindParameters();                
                if (Convert.ToInt32(parameter["BreakQuantity"]) > 1)
                    breakQuantity = Convert.ToInt32(parameter["BreakQuantity"]);
                if (Convert.ToInt32(parameter["Margin"]) != 0)
                    margin = Convert.ToInt32(parameter["Margin"]);

                bool IsBreakOrderForLastThree = Convert.ToBoolean(parameter["IsBreakOrderForLastThree"]);
                bool IsAdvancedSupply = Convert.ToBoolean(parameter["IsAdvancedSupply"]);

                foreach (DataRow lineRow in lineTable.Rows)
                {
                    string lineCode = lineRow["LINECODE"].ToString();

                    DataTable masterTable = orderDao.FindOrderMaster(orderDate, batchNo, lineCode).Tables[0];
                    DataTable channelTable = channelDao.FindChannelSchedule(orderDate, batchNo, lineCode, IsAdvancedSupply).Tables[0];
                    DataTable orderTable = orderDao.FindOrderDetail(orderDate, batchNo, lineCode).Tables[0];

                    int sortNo = 0;
                    int currentCount = 0;
                    int totalCount = masterTable.Rows.Count;
                    
                    foreach (DataRow masterRow in masterTable.Rows)
                    {
                        DataRow[] orderRows = null;
                        orderRows = orderTable.Select(string.Format("ORDERID = '{0}'", masterRow["ORDERID"]), "CHANNELCODE");

                        DataSet ds = orderSchedule.Optimize(masterRow, orderRows, channelTable, ref sortNo, breakQuantity, margin, IsBreakOrderForLastThree);
                        orderScheduleDao.SaveOrder(ds);
                        supplyDao.InsertSupply(ds.Tables["SUPPLY"], lineCode, orderDate, batchNo);

                        if (OnSchedule != null)
                            OnSchedule(this, new ScheduleEventArgs(5, "�����Ż�" + lineRow["LINECODE"].ToString() + "�ּ��߶���", ++currentCount, totalCount));
                    }

                    channelDao.Update(channelTable);
                }
            }
        }

        /// <summary>
        /// �����Ż�2010-04-19
        /// </summary>
        /// <param name="orderDate"></param>
        /// <param name="batchNo"></param>
        public void GenSupplySchedule(string orderDate, int batchNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                THOK.AS.Dao.ChannelDao channelDao = new ChannelDao();
                THOK.AS.Dao.LineScheduleDao lineDao = new LineScheduleDao();
                THOK.AS.Dao.SupplyDao supplyDao = new SupplyDao();
                THOK.Optimize.SupplyOptimize supplyOptimize = new THOK.Optimize.SupplyOptimize();
                THOK.AS.Dao.SysParameterDao parameterDao = new SysParameterDao();

                Dictionary<string, string> parameter = parameterDao.FindParameters();
                int aheadCount = Convert.ToInt32(parameter["SupplyAheadCount"]);
                bool IsAdvancedSupply = Convert.ToBoolean(parameter["IsAdvancedSupply"]);

                DataTable lineTable = lineDao.FindAllLine(orderDate, batchNo).Tables[0];
                foreach (DataRow lineRow in lineTable.Rows)
                {
                    string lineCode = lineRow["LINECODE"].ToString();
                    supplyDao.AdjustSortNo(lineCode, aheadCount, orderDate, batchNo);
                    DataTable channelTable = channelDao.FindChannelSchedule(orderDate, batchNo, lineCode, IsAdvancedSupply).Tables[0];
                    DataTable supplyTable = supplyOptimize.Optimize(channelTable, IsAdvancedSupply);
                    supplyDao.InsertSupply(supplyTable);
                    if (OnSchedule != null)
                        OnSchedule(this, new ScheduleEventArgs(7, "�����Ż�" + lineRow["LINECODE"].ToString() + "�����ƻ�", 1, 1));
                }
            }
        }

        public void GenHandleSupplySchedule(string orderDate, int batchNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {

                THOK.Optimize.HandleSupplyOptimize handleSupplyOptimize = new THOK.Optimize.HandleSupplyOptimize();
                Dao.ScOrderDao scOrderDao = new ScOrderDao();
                Dao.ChannelDao channelDao = new ChannelDao();

                Dao.LineScheduleDao lineDao = new LineScheduleDao();
                DataTable lineTable = lineDao.FindAllLine(orderDate, batchNo).Tables[0];

                int currentCount = 0;
                int totalCount = lineTable.Rows.Count;

                foreach (DataRow lineRow in lineTable.Rows)
                {
                    string lineCode = lineRow["LINECODE"].ToString();

                    DataTable handSupplyOrders = scOrderDao.FindHandleSupplyOrder(orderDate, batchNo, lineCode);
                    DataTable multiBrandChannel = channelDao.FindMultiBrandChannel(lineCode);

                    AddColumnForChannelTable(multiBrandChannel, multiBrandChannel.Rows.Count);

                    //�����µ��ֹ�����������
                    DataTable newSupplyOrders = handleSupplyOptimize.Optimize(handSupplyOrders, multiBrandChannel);

                    //�����̵��ղ���ҵ��SortNo
                    channelDao.Update(multiBrandChannel,orderDate, batchNo);

                    //ɾ��sc_orderԭ�����ֹ���������
                    scOrderDao.DeleteOldSupplyOrders(orderDate, batchNo, lineCode);
                    //��sc_order�в����µ��ֹ���������
                    scOrderDao.InsertNewSupplyOrders(newSupplyOrders);


                    //��AS_SC_HANDLESUPPLY�в����µ��ֹ���������
                    scOrderDao.InsertHandSupplyOrders(newSupplyOrders);

                    if (OnSchedule != null)
                        OnSchedule(this, new ScheduleEventArgs(6, "�����Ż�" + lineRow["LINECODE"].ToString() + "�ּ����ֹ�������������", ++currentCount, totalCount));
                }
            }
        }

        private DataTable AddColumnForChannelTable(DataTable channel, int channelCount)
        {
            //channel.Columns.Add("QUANTITY", typeof(Int32));
            for (int i = 0; i < channelCount; i++)
            {
                channel.Rows[i]["QUANTITY"] = 0;
            }
            return channel;
        } 
    }
}