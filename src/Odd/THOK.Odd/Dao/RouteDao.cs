using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.Odd.Dao
{
    internal class RouteDao: BaseDao
    {
        public void Insert(string orderDate, string batchNo)
        {
            string sql = string.Format("INSERT INTO ROUTE SELECT '{0}', {1}, ROUTECODE FROM (SELECT  DISTINCT ROUTECODE FROM SORDER WHERE ORDERDATE='{0}' AND BATCHNO ={1}) A", orderDate, batchNo);
            ExecuteNonQuery(sql);
        }

        public void Delete(string orderDate, string batchNo)
        {
            string sql = string.Format("DELETE FROM ROUTE WHERE ORDERDATE='{0}' AND BATCHNO={1}", orderDate, batchNo);
            ExecuteNonQuery(sql);
        }

        public DataTable Find(string orderDate)
        {
            string sql = string.Format("SELECT DISTINCT BATCHNO FROM ROUTE WHERE ORDERDATE='{0}' ORDER BY BATCHNO DESC", orderDate);
            return ExecuteQuery(sql).Tables[0];
        }

        public int Find(string orderDate, string batchNo)
        {
            string sql = string.Format("SELECT COUNT(*) FROM ROUTE WHERE ORDERDATE='{0}' AND BATCHNO={1}", orderDate, batchNo);
            return Convert.ToInt32(ExecuteScalar(sql));
        }

        public int FindGreatBatch(string orderDate, string batchNo)
        {
            string sql = string.Format("SELECT COUNT(*) FROM ROUTE WHERE ORDERDATE='{0}' AND BATCHNO>{1}", orderDate, batchNo);
            return Convert.ToInt32(ExecuteScalar(sql));
        }

        public void Update(string orderDate, string batchNo)
        {
            string sql = string.Format("UPDATE ROUTE SET SORTID='1' WHERE ORDERDATE='{0}' AND BATCHNO={1}", orderDate, batchNo);
            ExecuteNonQuery(sql);
        }
    }
}
