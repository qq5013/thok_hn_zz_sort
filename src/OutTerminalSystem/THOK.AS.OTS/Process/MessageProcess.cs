using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.AS.OTS.Process
{
    public class MessageProcess: THOK.MCP.AbstractProcess
    {
        private Printer printer = new Printer();

        protected override void StateChanged(THOK.MCP.StateItem stateItem, THOK.MCP.IProcessDispatcher dispatcher)
        {
            Dictionary<string, string> parameter = (Dictionary<string, string>)stateItem.State;
            //ת����mainPanelˢ����ʾ����
            dispatcher.WriteToProcess("mainPanel", "SortNo", parameter["SORTNO"]);

            //��ӡ�ͻ���ǩ
            if (Context.Attributes["PrintLabel"].ToString().ToUpper().Equals("TRUE"))
            {
                using (PersistentManager pm = new PersistentManager())
                {
                    Dao.OrderDao orderDao = new Dao.OrderDao();

                    DataTable masterTable = orderDao.FindMasterTable(parameter["SORTNO"]);
                    if (masterTable.Rows.Count != 0)
                    {
                        foreach (DataRow  row in masterTable.Rows)
                        {
                            string orderID = row["ORDERID"].ToString();
                            THOK.MCP.Logger.Info("��ӡ�ͻ���ǩ.�����ţ�" + orderID);
                            DataTable detailTable = orderDao.FindOrderDetail(orderID);

                            int quantity = Convert.ToInt32(detailTable.Compute("SUM(QUANTITY)", ""));

                            printer.Print(row, detailTable.Select(), quantity);

                            orderDao.UpdateOrderPrintStatus(orderID);
                        }
                    }
                }
            }
        }
    }
}
