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
                Logger.Error("OrderRequestProcess 初始化失败！原因：" + e.Message);
            }

        }

        protected override void StateChanged(StateItem stateItem, IProcessDispatcher dispatcher)
        {
            try
            {
                //读取订单明细并写给PLC
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
                                int[] orderData = new int[90];
                                for (int i = 0; i < detailTable.Rows.Count; i++)
                                {
                                    orderData[Convert.ToInt32(detailTable.Rows[i]["CHANNELADDRESS"]) - 1] = Convert.ToInt32(detailTable.Rows[i]["QUANTITY"]);
                                }

                                orderData[86] = Convert.ToInt32(sortNo);
                                orderData[87] = quantity;
                                orderData[88] = maxSortNo == sortNo ? 1 : 0;
                                orderData[89] = 1;

                                if (dispatcher.WriteToService("SortPLC", "OrderData", orderData))
                                {
                                    orderDao.UpdateOrderStatus(sortNo, "1");
                                    Logger.Info(string.Format("写订单数据成功,分拣订单号[{0}]。", sortNo));

                                    //发送订单号给分拣出口终端系统
                                    if (sortNo == "1")
                                    {
                                        messageUtil.SendToExport(sortNo);     
                                    }
                                                                 
                                }

                            }
                        }
                        catch (Exception e)
                        {
                            Logger.Error("写订单数据失败，原因：" + e.Message);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Logger.Error("分拣订单请求操作失败！原因：" + ee.Message);
            }

        }
    }
}
