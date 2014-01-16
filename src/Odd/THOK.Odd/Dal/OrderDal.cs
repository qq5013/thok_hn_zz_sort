using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.Odd.Dao;

namespace THOK.Odd.Dal
{
    public class OrderDal
    {
        public DataTable GetOrderMaster()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindMaster();
            }
        }
        public DataTable GetHistoryOrderMaster()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindHistoryMaster();
            }
        }
        public DataTable GetRouteQuantity()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindRoute();
            }
        }

        public DataTable GetCustomerQuantity()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindCustomer();
            }
        }

        public DataTable GetRouteCigarette(string routeCode)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindRouteCigarette(routeCode);
            }
        }

        public DataTable GetCustomerCigarette(string customerCode)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindCustomerCigarette(customerCode);
            }
        }

        public DataTable HistoryData(string orderDate, string batchNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindHistoryData(orderDate, batchNo);
            }
        }
        public DataTable DetailTable(string orderDate, string batchNo)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindDetail(orderDate, batchNo);
            }
        }
        public DataTable batchTable()
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindBatchNo();
            }
        }

        internal DataTable DetailTablebyCustomer(string orderDate, string batchNo, string customerCode)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderDao orderDao = new OrderDao();
                return orderDao.FindDetailbyCustomer(orderDate, batchNo, customerCode);
            }
        }
    }
}
