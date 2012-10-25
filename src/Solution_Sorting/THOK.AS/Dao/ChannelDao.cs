using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
namespace THOK.AS.Dao
{
    public class ChannelDao : BaseDao
    {
        public DataTable FindAll(int startRecord, int pageSize, string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);
            string sql = "SELECT * FROM AS_BI_CHANNEL " + where;
            return ExecuteQuery(sql, "AS_BI_CHANNEL", startRecord, pageSize).Tables[0];
        }

        public int FindCount(string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);

            string sql = "SELECT COUNT(*) FROM AS_BI_CHANNEL " + where;
            return (int)ExecuteScalar(sql);
        }

        public void UpdateEntity(string channelID, string cigaretteCode, string cigaretteName, string ledGroup, string ledNo, string status)
        {
            SqlCreate sqlCreate = new SqlCreate("AS_BI_CHANNEL", SqlType.UPDATE);
            sqlCreate.AppendQuote("LEDGROUP", ledGroup);
            sqlCreate.AppendQuote("CIGARETTECODE", cigaretteCode.Trim());
            sqlCreate.AppendQuote("CIGARETTENAME", cigaretteName.Trim());
            sqlCreate.Append("LEDNO", ledNo);
            sqlCreate.AppendQuote("STATUS", status);
            sqlCreate.AppendWhereQuote("CHANNELID", channelID);
            ExecuteNonQuery(sqlCreate.GetSQL());
        }

        public DataSet FindAvailableChannel(string lineCode)
        {
            string sql = string.Format("SELECT * ,ROW_NUMBER() OVER (ORDER BY CASE WHEN GROUPNO > 8 THEN ABS(GROUPNO - 17) ELSE GROUPNO END,GROUPNO) AS CHANNELINDEX FROM AS_BI_CHANNEL WHERE LINECODE = '{0}' ", lineCode);
            return ExecuteQuery(sql);
        }

        public DataSet FindChannelSchedule(string orderDate, int batchNo, string lineCode, bool IsAdvancedSupply)
        {
            string sql = "SELECT *,CASE WHEN CHANNELTYPE='3' THEN 50 ELSE 40 END REMAINQUANTITY,CASE WHEN CHANNELTYPE='3' THEN QUANTITY / 50 - 3 ELSE QUANTITY / 50 -1 END PIECE " +
                "FROM AS_SC_CHANNELUSED WHERE LINECODE = '{0}' AND BATCHNO = '{1}' AND ORDERDATE = '{2}'";
            if (IsAdvancedSupply)
            {
                sql = "SELECT *, CASE WHEN CHANNELTYPE='3' THEN 50 ELSE 50 END REMAINQUANTITY,CASE WHEN CHANNELTYPE='3' THEN QUANTITY / 50 ELSE QUANTITY / 50 -1 END PIECE " +
                "FROM AS_SC_CHANNELUSED WHERE LINECODE = '{0}' AND BATCHNO = '{1}' AND ORDERDATE = '{2}'";
            }
            return ExecuteQuery(string.Format(sql, lineCode, batchNo, orderDate));
        }

        public void SaveChannelSchedule(DataTable channelTable, string orderDate, int batchNo)
        {
            foreach (DataRow channelRow in channelTable.Rows)
            {
                SqlCreate sql = new SqlCreate("AS_SC_CHANNELUSED", SqlType.INSERT);
                sql.AppendQuote("CHANNELID", channelRow["CHANNELID"]);
                sql.AppendQuote("CHANNELCODE", channelRow["CHANNELCODE"]);
                sql.AppendQuote("CHANNELNAME", channelRow["CHANNELNAME"]);
                sql.AppendQuote("CHANNELTYPE", channelRow["CHANNELTYPE"]);
                sql.AppendQuote("LINECODE", channelRow["LINECODE"]);
                sql.AppendQuote("CIGARETTECODE", channelRow["CIGARETTECODE"]);
                sql.AppendQuote("CIGARETTENAME", channelRow["CIGARETTENAME"]);
                sql.Append("QUANTITY", channelRow["QUANTITY"]);
                sql.AppendQuote("STATUS", channelRow["STATUS"]);
                sql.AppendQuote("LEDNO", channelRow["LEDNO"]);
                sql.AppendQuote("LEDGROUP", channelRow["LEDGROUP"]);
                sql.AppendQuote("GROUPNO", channelRow["GROUPNO"]);
                sql.AppendQuote("ORDERDATE", orderDate);
                sql.AppendQuote("BATCHNO", batchNo);

                ExecuteNonQuery(sql.GetSQL());
            }
        }

        public void Update(DataTable channelTable)
        {
            foreach (DataRow channelRow in channelTable.Rows)
            {
                string sql = string.Format("UPDATE AS_SC_CHANNELUSED SET SORTNO={0} WHERE ORDERDATE='{1}' AND BATCHNO='{2}' AND LINECODE='{3}' AND CHANNELCODE='{4}'",
                    channelRow["SORTNO"], channelRow["ORDERDATE"], channelRow["BATCHNO"].ToString().Trim(), channelRow["LINECODE"], channelRow["CHANNELCODE"]);
                ExecuteNonQuery(sql);
            }
        }

        public void Update(DataTable channelTable, string orderDate, int batchNo)
        {
            foreach (DataRow channelRow in channelTable.Rows)
            {
                string sql = string.Format("UPDATE AS_SC_CHANNELUSED SET SORTNO={0} WHERE ORDERDATE='{1}' AND BATCHNO='{2}' AND LINECODE='{3}' AND CHANNELCODE='{4}'",
                    channelRow["SORTNO"], orderDate, batchNo, channelRow["LINECODE"], channelRow["CHANNELCODE"]);
                ExecuteNonQuery(sql);
            }
        }

        public string GetEnableChannel()
        {
            string strSql = "SELECT COUNT(CHANNELID) FROM AS_BI_CHANNEL WHERE CHANNELTYPE ='2' AND STATUS='1'";
            return ExecuteQuery(strSql).Tables[0].Rows[0][0].ToString();
        }

        public DataTable FindMultiBrandChannel(string lineCode)
        {
            string strSql = string.Format("SELECT * ,0 SORTNO FROM AS_BI_CHANNEL WHERE CHANNELTYPE ='5' AND STATUS='1' AND LINECODE='{0}' ", lineCode);
            return ExecuteQuery(strSql).Tables[0];
        }
    }
}
