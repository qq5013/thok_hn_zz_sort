using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.MCP;
using THOK.AS.Sorting.Util;
using THOK.AS.Sorting.Dao;
using THOK.AS.Sorting.View;
using THOK.AS.Sorting.Dal;
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
                Logger.Error("CurrentOrderProcess 初始化失败！原因：" + e.Message);
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
                    //刷新分拣状态
                    Refresh(sortNo, dispatcher);
                    
                    //发送订单号给 分拣出口终端系统
                    if (Convert.ToInt32(sortNo) > 0)
                    {
                        sortNo = Convert.ToString(Convert.ToInt32(sortNo) + 1);
                        //messageUtil.SendToExport(sortNo);
                    }
                }
            }
            catch (Exception e)
            {                
                Logger.Error("完成订单信息处理失败！原因：" + e.Message);
            }
        }

        private void Refresh(string sortNo, IProcessDispatcher dispatcher)
        {
            try
            {
                using (PersistentManager pm = new PersistentManager())
                {
                    string sortNoTemp = sortNo == "" ? "0" : sortNo;
                    OrderDao orderDao = new OrderDao();
                    if (sortNo == null)
                        sortNo = orderDao.FindLastSortNo();

                    //更新完成时间
                    orderDao.UpdateFinisheTime(sortNo);

                    //刷新主界面分拣状态
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
                    //messageUtil.SendToSortLed(sortNo, refreshData);


                    if (sortNo != "0")
                    {
                        DataTable orderInfo = orderDao.GetOrderIdFromSortNo(sortNo);
                        if (orderInfo.Rows.Count > 0)
                        {
                            string orderid = orderInfo.Rows[0]["ORDERID"].ToString();
                            string sortNoMax = orderDao.GetMaxSortNoByOrderID(orderid);
                            if (sortNo == sortNoMax)
                            {
                                int uploadMode = 0;
                                ParamDao paramDao = new ParamDao();
                                uploadMode = Convert.ToInt32(paramDao.FindState("UPLOADMODE"));

                                UploadDal uploadDal = new UploadDal();
                                if (uploadMode == 0)//自动上传国家局
                                {
                                    //通过ORDERID汇总当前户的定单
                                    //UploadDao uploadDao = new UploadDao();
                                    if (uploadDal.DataUpload(orderid))
                                    {
                                        Logger.Info(string.Format("已经完成{0}号定单", sortNo));
                                        Logger.Info(string.Format("已经完成对定单号'{0}'的中烟上传", orderid));
                                    }
                                    else
                                    {
                                        Logger.Info(string.Format("定单号'{0}'的中烟上传失败", orderid));
                                    }
                                }
                                else
                                {
                                    if (uploadDal.SaveUpload(orderid))
                                    {
                                        Logger.Info(string.Format("已经完成对定单号'{0}'的保存", orderid));
                                    }
                                }
                            }
                            else
                            {
                                Logger.Info(string.Format("已经完成{0}号定单", sortNo));
                            }

                            ////通过ORDERID汇总当前户的定单
                            //UploadDao uploadDao = new UploadDao();
                            //if (uploadDao.DataUpload(orderid))
                            //{
                            //    Logger.Info(string.Format("已经完成对定单号'{0}'的中烟上传",orderid));
                            //}
                            ////DataTable dt= masterDao.SumFromOrderId(orderid);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error("更新分拣信息处理失败！原因：" + e.Message);
            }

        }
    }
}
