using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using System.Data;
namespace THOK.AS.Dao
{
    public class ChannelScheduleDao : BaseDao
    {
        public DataTable FindAll(int startRecord, int pageSize, string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);
            string sql = "SELECT *,QUANTITY / 50 JQUANTITY, QUANTITY % 50 LQUANTITY FROM AS_SC_CHANNELUSED " + where;
            sql += " ORDER BY ORDERDATE, BATCHNO, LINECODE, CHANNELCODE";
            return ExecuteQuery(sql, "AS_BI_AREA", startRecord, pageSize).Tables[0];
        }

        public int FindCount(string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);

            string sql = "SELECT COUNT(*) FROM AS_SC_CHANNELUSED " + where;
            return (int)ExecuteScalar(sql);
        }

        public void DeleteHistory(string orderDate)
        {
            string sql = string.Format("DELETE FROM AS_SC_CHANNELUSED WHERE ORDERDATE < '{0}'", orderDate); 
            ExecuteNonQuery(sql);
        }

        public void DeleteSchedule(string orderDate, int batchNo)
        {
            string sql = string.Format("DELETE FROM AS_SC_CHANNELUSED WHERE ORDERDATE = '{0}' AND BATCHNO = '{1}'", orderDate, batchNo);
            ExecuteNonQuery(sql);
        }
    }
}
