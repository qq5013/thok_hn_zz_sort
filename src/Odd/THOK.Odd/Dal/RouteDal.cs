using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
using THOK.Odd.Dao;

namespace THOK.Odd.Dal
{
    public class RouteDal
    {
        private RouteDao routeDao = new RouteDao();

        public DataTable GetBatch(string orderDate)
        {
            using (PersistentManager pm = new PersistentManager())
            {
                return routeDao.Find(orderDate);
            }
        }
    }
}
