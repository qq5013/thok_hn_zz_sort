using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.AS.Dao;
using THOK.Util;

namespace THOK.AS.Dal
{
    public class OrderScheduleDal
    {
        public DataTable GetMasterAll(int pageIndex, int pageSize, string filter)
        {
            DataTable table = null;
            using (PersistentManager pm = new PersistentManager())
            {
                OrderScheduleDao orderDao = new OrderScheduleDao();
                table = orderDao.FindMasterAll((pageIndex - 1) * pageSize, pageSize, filter);
            }
            return table;
        }


        public int GetMasterCount(string filter)
        {
            int count = 0;
            using (PersistentManager pm = new PersistentManager())
            {
                OrderScheduleDao orderDao = new OrderScheduleDao();
                count = orderDao.FindMasterCount(filter);
            }
            return count;
        }

        public DataTable GetDetailAll(int pageIndex, int pageSize, string filter)
        {
            DataTable table = null;
            using (PersistentManager pm = new PersistentManager())
            {
                OrderScheduleDao orderDao = new OrderScheduleDao();
                table = orderDao.FindDetailAll((pageIndex - 1) * pageSize, pageSize, filter);
            }
            return table;
        }


        public int GetDetailCount(string filter)
        {
            int count = 0;
            using (PersistentManager pm = new PersistentManager())
            {
                OrderScheduleDao orderDao = new OrderScheduleDao();
                count = orderDao.FindDetailCount(filter);
            }
            return count;
        }

        public DataTable GetLineAll(int pageIndex, int pageSize, string filter)
        {
            DataTable table = null;
            using (PersistentManager pm = new PersistentManager())
            {
                OrderScheduleDao orderDao = new OrderScheduleDao();
                table = orderDao.FindLineAll((pageIndex - 1) * pageSize, pageSize, filter);
            }
            return table;
        }


        public int GetLineCount(string filter)
        {
            int count = 0;
            using (PersistentManager pm = new PersistentManager())
            {
                OrderScheduleDao orderDao = new OrderScheduleDao();
                count = orderDao.FindLineCount(filter);
            }
            return count;
        }

        public DataTable GetOrder(string orderDate, int batchNo, bool isAbnormity)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                OrderScheduleDao orderDao = new OrderScheduleDao();
                if (isAbnormity)
                    return orderDao.FindOrderForAbnormity(orderDate, batchNo);
                else 
                    return orderDao.FindOrder(orderDate, batchNo);
            }
        }
    }
}
