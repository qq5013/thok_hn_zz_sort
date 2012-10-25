using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.AS.Dao
{
    public class SupplyDao: BaseDao
    {
        string lineCode = "";
        int serialNo = 0;

        public void DeleteSupply()
        {
            string sql = "TRUNCATE TABLE AS_SC_SUPPLY";
            ExecuteNonQuery(sql);
        }

        public void InsertSupply(DataTable supplyTable)
        {
            foreach (DataRow row in supplyTable.Rows)
            {
                SqlCreate sqlCreate = new SqlCreate("AS_SC_SUPPLY", SqlType.INSERT);
                sqlCreate.AppendQuote("ORDERDATE", row["ORDERDATE"]);
                sqlCreate.Append("BATCHNO", row["BATCHNO"]);
                sqlCreate.Append("SERIALNO", row["SERIALNO"]);
                sqlCreate.AppendQuote("LINECODE", row["LINECODE"]);
                sqlCreate.Append("ORIGINALSORTNO", row["SORTNO"]);
                sqlCreate.Append("SORTNO", row["SORTNO"]);
                sqlCreate.Append("GROUPNO", row["GROUPNO"]);
                sqlCreate.AppendQuote("CHANNELCODE", row["CHANNELCODE"]);
                sqlCreate.AppendQuote("CIGARETTECODE", row["CIGARETTECODE"]);
                sqlCreate.AppendQuote("CIGARETTENAME", row["CIGARETTENAME"]);
                ExecuteNonQuery(sqlCreate.GetSQL());
            }
        }

        public void InsertSupply(DataTable supplyTable, string lineCode, string orderDate, int batchNo)
        {
            if (this.lineCode != lineCode)
            {
                this.lineCode = lineCode;
                string sql = string.Format("SELECT CASE WHEN MAX(SERIALNO) IS NULL THEN 1000 ELSE MAX(SERIALNO) END  FROM AS_SC_SUPPLY WHERE LINECODE='{0}' AND ORDERDATE = '{1}' AND BATCHNO = {2} ", lineCode, orderDate, batchNo);
                serialNo = Convert.ToInt32(ExecuteScalar(sql));
            }

            foreach (DataRow row in supplyTable.Rows)
            {
                SqlCreate sqlCreate = new SqlCreate("AS_SC_SUPPLY", SqlType.INSERT);
                sqlCreate.AppendQuote("ORDERDATE", row["ORDERDATE"]);
                sqlCreate.Append("BATCHNO", row["BATCHNO"]);
                sqlCreate.Append("SERIALNO", serialNo++);
                sqlCreate.AppendQuote("LINECODE", row["LINECODE"]);
                sqlCreate.Append("ORIGINALSORTNO", row["SORTNO"]);
                sqlCreate.Append("SORTNO", row["SORTNO"]);
                sqlCreate.Append("GROUPNO", row["GROUPNO"]);
                sqlCreate.AppendQuote("CHANNELCODE", row["CHANNELCODE"]);
                sqlCreate.AppendQuote("CIGARETTECODE", row["CIGARETTECODE"]);
                sqlCreate.AppendQuote("CIGARETTENAME", row["CIGARETTENAME"]);
                ExecuteNonQuery(sqlCreate.GetSQL());
            }
        }

        public void AdjustSortNo(string lineCode, int aheadCount,string orderDate, int batchNo)
        {
            aheadCount = aheadCount + 1000;
            string sql = string.Format("UPDATE AS_SC_SUPPLY SET SORTNO = CASE WHEN SERIALNO <= {0} THEN 1 ELSE ORIGINALSORTNO - (SELECT MAX(ORIGINALSORTNO) FROM AS_SC_SUPPLY WHERE SERIALNO <= {0} AND LINECODE='{1}' AND ORDERDATE = '{2}' AND BATCHNO = {3}) + 1 END WHERE LINECODE='{1}' AND ORDERDATE = '{2}' AND BATCHNO = {3}", aheadCount, lineCode, orderDate, batchNo);
            ExecuteNonQuery(sql);
        }

        public void DeleteSchedule(string orderDate, int batchNo)
        {
            string sql = string.Format("DELETE FROM AS_SC_SUPPLY WHERE ORDERDATE = '{0}' AND BATCHNO='{1}'", orderDate, batchNo);
            ExecuteNonQuery(sql);
        }

        public void DeleteHistory(string orderDate)
        {
            string sql = string.Format("DELETE FROM AS_SC_SUPPLY WHERE ORDERDATE < '{0}'", orderDate);
            ExecuteNonQuery(sql);
        }

    }
}
