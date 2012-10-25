using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.AS.Dao
{
    public class RouteDao : BaseDao
    {
        public DataTable FindAll(int startRecord, int pageSize, string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);
            string sql = "SELECT * FROM V_AS_BI_ROUTE " + where + " ORDER BY SORTID";
            return ExecuteQuery(sql, "V_AS_BI_ROUTE", startRecord, pageSize).Tables[0];
        }


        public int FindCount(string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);

            string sql = "SELECT COUNT(*) FROM V_AS_BI_ROUTE " + where;
            return (int)ExecuteScalar(sql);
        }

        public void BatchInsertRoute(DataTable dtData)
        {
            BatchInsert(dtData, "AS_BI_ROUTE");
        }

        public void Clear()
        {
            string sql = "TRUNCATE TABLE AS_BI_ROUTE";
            ExecuteNonQuery(sql);
        }
    }
}
