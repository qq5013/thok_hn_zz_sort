using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.AS.Dao
{
    public class StockChannelDao: BaseDao
    {
        public DataTable FindChannel()
        {
            string sql = "SELECT * FROM AS_BI_STOCKCHANNEL WHERE STATUS='1' ORDER BY ORDERNO";
            return ExecuteQuery(sql).Tables[0];
        }

        public int FindCount(string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);

            string sql = "SELECT COUNT(*) FROM AS_BI_STOCKCHANNEL " + where;
            return (int)ExecuteScalar(sql);
        }

        public DataTable FindAll(int startRecord, int pageSize, string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);
            string sql = "SELECT * FROM AS_BI_STOCKCHANNEL " + where;
            return ExecuteQuery(sql, "AS_BI_CHANNEL", startRecord, pageSize).Tables[0];
        }

        public void UpdateEntity(string channelCode, string cigaretteCode, string cigaretteName, string ledNo, string status)
        {
            SqlCreate sqlCreate = new SqlCreate("AS_BI_STOCKCHANNEL", SqlType.UPDATE);
            sqlCreate.AppendQuote("CIGARETTECODE", cigaretteCode.Trim());
            sqlCreate.AppendQuote("CIGARETTENAME", cigaretteName.Trim());
            sqlCreate.Append("LEDNO", ledNo);
            sqlCreate.AppendQuote("STATUS", status);
            sqlCreate.AppendWhereQuote("CHANNELCODE", channelCode);
            ExecuteNonQuery(sqlCreate.GetSQL());
        }

        public void UpdateChannel(DataTable channelTable)
        {
            foreach (DataRow row in channelTable.Rows)
            {
                SqlCreate sqlCreate = new SqlCreate("AS_BI_STOCKCHANNEL", SqlType.UPDATE);
                sqlCreate.AppendQuote("CIGARETTECODE", row["CIGARETTECODE"]);
                sqlCreate.AppendQuote("CIGARETTENAME", row["CIGARETTENAME"]);
                sqlCreate.AppendWhereQuote("CHANNELCODE", row["CHANNELCODE"]);
                ExecuteNonQuery(sqlCreate.GetSQL());
            }
        }

        public void ClearCigarette()
        {
            string sql = "UPDATE AS_BI_STOCKCHANNEL SET CIGARETTECODE='',CIGARETTENAME=''";
            ExecuteNonQuery(sql);
        }

        public void DeleteHistory(string orderDate)
        {
            string sql = string.Format("DELETE FROM AS_SC_STOCKMIXCHANNEL WHERE ORDERDATE < '{0}'", orderDate);
            ExecuteNonQuery(sql);
        }

        public void InsertMixChannel(DataTable mixTable)
        {
            foreach (DataRow row in mixTable.Rows)
            {
                SqlCreate sqlCreate = new SqlCreate("AS_SC_STOCKMIXCHANNEL", SqlType.INSERT);
                sqlCreate.AppendQuote("ORDERDATE", row["ORDERDATE"]);
                sqlCreate.Append("BATCHNO", row["BATCHNO"]);
                sqlCreate.AppendQuote("CHANNELCODE", row["CHANNELCODE"]);
                sqlCreate.AppendQuote("CIGARETTECODE", row["CIGARETTECODE"]);
                sqlCreate.AppendQuote("CIGARETTENAME", row["CIGARETTENAME"]);
                ExecuteNonQuery(sqlCreate.GetSQL());
            }
        }

        public void DeleteSchedule(string orderDate, int batchNo)
        {
            string sql = string.Format("DELETE FROM AS_SC_STOCKMIXCHANNEL WHERE ORDERDATE = '{0}' AND BATCHNO='{1}'", orderDate, batchNo);
            ExecuteNonQuery(sql);
        }


    }
}
