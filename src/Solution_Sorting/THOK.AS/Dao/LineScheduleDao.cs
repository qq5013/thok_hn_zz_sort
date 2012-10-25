using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.AS.Dao
{
   
    public class LineScheduleDao : BaseDao
    {
        public DataTable FindAll(int startRecord, int pageSize, string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);
            string sql = "SELECT A.*, B.ROUTENAME FROM AS_SC_LINE A LEFT JOIN AS_BI_ROUTE B ON A.ROUTECODE = B.ROUTECODE " + where;
            return ExecuteQuery(sql, "AS_SC_LINE", startRecord, pageSize).Tables[0];
        }

        public int FindCount(string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);

            string sql = "SELECT COUNT(*) FROM AS_SC_LINE " + where;
            return (int)ExecuteScalar(sql);
        }

        public void SaveLineSchedule(DataTable lineTable)
        {
            foreach (DataRow lineRow in lineTable.Rows)
            {
                SqlCreate sqlCreate = new SqlCreate("AS_SC_LINE", SqlType.INSERT);
                sqlCreate.AppendQuote("LINECODE", lineRow["LINECODE"]);
                sqlCreate.AppendQuote("ROUTECODE", lineRow["ROUTECODE"]);
                sqlCreate.Append("QUANTITY", lineRow["QUANTITY"]);
                sqlCreate.AppendQuote("BATCHNO", lineRow["BATCHNO"]);
                sqlCreate.AppendQuote("ORDERDATE", lineRow["ORDERDATE"]);
                string sql = sqlCreate.GetSQL();
                ExecuteNonQuery(sql);
            }
        }

        public DataSet FindAllLine(string orderDate, int batchNo)
        {
            string sql = string.Format("SELECT DISTINCT LINECODE FROM AS_SC_LINE WHERE ORDERDATE = '{0}' AND BATCHNO = '{1}'", orderDate, batchNo);
            return ExecuteQuery(sql);
        }

        public void DeleteHistory(string orderDate)
        {
            string sql = string.Format("DELETE FROM AS_SC_LINE WHERE ORDERDATE < '{0}'", orderDate);
            ExecuteNonQuery(sql);
        }

        public void DeleteSchedule(string orderDate, int batchNo)
        {
            string sql = string.Format("DELETE FROM AS_SC_LINE WHERE ORDERDATE = '{0}' AND BATCHNO='{1}'", orderDate, batchNo);
            ExecuteNonQuery(sql);
        }

        public string FindRoutes(string orderDate)
        {
            string result = null;
            string sql = string.Format("SELECT ROUTECODE FROM AS_SC_LINE WHERE ORDERDATE = '{0}'", orderDate);
            DataTable routeTable = ExecuteQuery(sql).Tables[0];
            if (routeTable.Rows.Count == 0)
                result = "''";
            else
                for (int i = 0; i < routeTable.Rows.Count; i++)
                {
                    if (i == routeTable.Rows.Count - 1)
                        result += string.Format("'{0}'", routeTable.Rows[i]["ROUTECODE"]);
                    else
                        result += string.Format("'{0}',", routeTable.Rows[i]["ROUTECODE"]);
                }
            return result;
        }
    }
}
