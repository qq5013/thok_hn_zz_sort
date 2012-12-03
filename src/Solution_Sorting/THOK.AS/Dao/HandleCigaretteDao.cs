using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.AS.Dao
{
    public class HandleCigaretteDao : BaseDao
    {
        public DataTable FindLineAll(int startRecord, int pageSize, string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);
            string sql = "SELECT ORDERDATE, BATCHNO, LINECODE, CIGARETTECODE, CIGARETTENAME, SUM(QUANTITY) QUANTITY FROM AS_SC_HANDLESUPPLY " + where;
            sql += " GROUP BY ORDERDATE, BATCHNO, LINECODE, CIGARETTECODE, CIGARETTENAME ORDER BY ORDERDATE, BATCHNO, LINECODE";
            return ExecuteQuery(sql, "AS_SC_HANDLESUPPLY", startRecord, pageSize).Tables[0];
        }

        public DataTable FindLineAllDetail(int startRecord, int pageSize, string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);
            string sql = @"SELECT ORDERDATE, BATCHNO, 
                            LINECODE,CHANNELCODE,
                            CIGARETTECODE, CIGARETTENAME,QUANTITY 
                               FROM AS_SC_HANDLESUPPLY  " + where + "ORDER BY ORDERDATE DESC ,BATCHNO DESC,LINECODE,SORTNO";
            return ExecuteQuery(sql, "AS_SC_HANDLESUPPLY", startRecord, pageSize).Tables[0];
        }

        public DataTable FindRouteAll(int startRecord, int pageSize, string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);
            string sql = "SELECT ORDERDATE, BATCHNO, ROUTECODE, ROUTENAME, CUSTOMERCODE, CUSTOMERNAME, CIGARETTECODE, CIGARETTENAME, SUM(QUANTITY) QUANTITY FROM AS_SC_HANDLESUPPLY " + where;
            sql += " GROUP BY ORDERDATE, BATCHNO, ROUTECODE, ROUTENAME, CUSTOMERCODE, CUSTOMERNAME, CIGARETTECODE, CIGARETTENAME";
            return ExecuteQuery(sql, "AS_SC_HANDLESUPPLY", startRecord, pageSize).Tables[0];
        }

        public int FindLineCount(string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);

            string sql = "SELECT COUNT(*) FROM (SELECT ORDERDATE, BATCHNO, LINECODE, CIGARETTECODE, CIGARETTENAME, SUM(QUANTITY) QUANTITY FROM AS_SC_HANDLESUPPLY " + where;
            sql += " GROUP BY ORDERDATE, BATCHNO, LINECODE, CIGARETTECODE, CIGARETTENAME ) A";
            return (int)ExecuteScalar(sql);
        }

        public int FindLineDetailCount(string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);

            string sql = @"SELECT COUNT(*) FROM (SELECT ORDERDATE, BATCHNO, 
                            LINECODE,CHANNELCODE,
                            CIGARETTECODE, CIGARETTENAME,QUANTITY 
                            FROM AS_SC_HANDLESUPPLY" + where+ " ) AA";
            return (int)ExecuteScalar(sql);
        }

        public int FindRouteCount(string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);

            string sql = "SELECT COUNT(*) FROM (SELECT ORDERDATE, BATCHNO, ROUTECODE, ROUTENAME, CUSTOMERCODE, CUSTOMERNAME, CIGARETTECODE, CIGARETTENAME, SUM(QUANTITY) QUANTITY FROM AS_SC_HANDLESUPPLY " + where;
            sql += " GROUP BY ORDERDATE, BATCHNO, ROUTECODE, ROUTENAME, CUSTOMERCODE, CUSTOMERNAME, CIGARETTECODE, CIGARETTENAME ) A";
            return (int)ExecuteScalar(sql);
        }
    }
}
