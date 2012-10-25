using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.AS.Dao
{
    public class CustomerDao : BaseDao
    {
        public DataTable FindAll(int startRecord, int pageSize, string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);
            string sql = "SELECT * FROM V_AS_BI_CUSTOMER " + where + " ORDER BY ROUTECODE, SORTID";
            return ExecuteQuery(sql, "V_AS_BI_CUSTOMER", startRecord, pageSize).Tables[0];
        }


        public int FindCount(string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);

            string sql = "SELECT COUNT(*) FROM V_AS_BI_CUSTOMER " + where;
            return (int)ExecuteScalar(sql);
        }

        public void BatchInsertCustomer(DataTable dtData)
        {
            BatchInsert(dtData, "AS_BI_CUSTOMER");
        }

        public void Clear()
        {
            string sql = "TRUNCATE TABLE AS_BI_CUSTOMER";
            ExecuteNonQuery(sql);
        }
    }
}
