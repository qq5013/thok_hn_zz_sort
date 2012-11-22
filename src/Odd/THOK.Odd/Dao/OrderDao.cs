using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.Odd.Dao
{
    internal class OrderDao : BaseDao
    {
        public DataTable Find(string orderDate, string batchNo)
        {
            string sql = "SELECT  ORDERID,ORDERID,CUSTOMERCODE,CUSTOMERNAME,CIGARETTECODE,CIGARETTENAME,QUANTITY,BATCHNO, " +
                "1 CORDER,ROUTECODE,ROUTENAME,CONVERT(NVARCHAR(10),ORDERDATE, 120),CONVERT(NVARCHAR(10),GETDATE(), 120),'05','1' FROM SORDER " +
                "WHERE CIGARETTECODE IN (SELECT CIGARETTECODE FROM CIGARETTE WHERE ISABNORMITY = '1') " +
                "ORDER BY SORTID,ROUTECODE,CUSTOMERCODE,CIGARETTECODE";
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindMaster()
        {
            string sql = "SELECT ORDERDATE,ORDERID,ROUTECODE,ROUTENAME,CUSTOMERCODE,CUSTOMERNAME,ADDRESS,SORTID,SUM(QUANTITY) QUANTITY FROM SORDER " +
                "WHERE CIGARETTECODE IN (SELECT CIGARETTECODE FROM CIGARETTE WHERE ISABNORMITY='1') " +
                "GROUP BY ORDERDATE,ORDERID,ROUTECODE,ROUTENAME,CUSTOMERCODE,CUSTOMERNAME,ADDRESS,SORTID " +
                "ORDER BY SORTID,ROUTECODE,CUSTOMERCODE";
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindRoute()
        {
            string sql = "SELECT ORDERDATE,BATCHNO,ROUTECODE,ROUTENAME,SUM(QUANTITY) QUANTITY FROM SORDER " +
                "WHERE CIGARETTECODE IN (SELECT CIGARETTECODE FROM CIGARETTE WHERE ISABNORMITY='1') " +
                "GROUP BY ORDERDATE,BATCHNO,ROUTECODE,ROUTENAME ORDER BY ROUTECODE";
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindCustomer()
        {
            string sql = "SELECT ORDERDATE,BATCHNO,ROUTECODE,ROUTENAME,CUSTOMERCODE,CUSTOMERNAME,SUM(QUANTITY) QUANTITY FROM SORDER " +
                "WHERE CIGARETTECODE IN (SELECT CIGARETTECODE FROM CIGARETTE WHERE ISABNORMITY='1') " +
                "GROUP BY ORDERDATE,BATCHNO,ROUTECODE,ROUTENAME,CUSTOMERCODE,CUSTOMERNAME " +
                "ORDER BY ROUTECODE,CUSTOMERCODE";
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindRouteCigarette(string routeCode)
        {
            string sql = string.Format("SELECT CIGARETTECODE,CIGARETTENAME,SUM(QUANTITY) QUANTITY FROM SORDER " +
                "WHERE ROUTECODE='{0}' AND CIGARETTECODE IN (SELECT CIGARETTECODE FROM CIGARETTE WHERE ISABNORMITY='1') " +
                "GROUP BY CIGARETTECODE,CIGARETTENAME", routeCode);
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindCustomerCigarette(string customerCode)
        {
            string sql = string.Format("SELECT CIGARETTECODE,CIGARETTENAME,SUM(QUANTITY) QUANTITY FROM SORDER " +
                "WHERE CUSTOMERCODE='{0}' AND CIGARETTECODE IN (SELECT CIGARETTECODE FROM CIGARETTE WHERE ISABNORMITY='1') " +
                "GROUP BY CIGARETTECODE,CIGARETTENAME", customerCode);
            return ExecuteQuery(sql).Tables[0];
        }

        public void Clear()
        {
            ExecuteNonQuery("TRUNCATE TABLE SORDER");
        }

        public void Insert(DataRow row, string tableName)
        {
            SqlCreate sqlCreate = new SqlCreate(tableName, SqlType.INSERT);
            sqlCreate.AppendQuote("ORDERDATE", row["ORDERDATE"]);
            sqlCreate.Append("BATCHNO", row["BATCHNO"]);
            sqlCreate.AppendQuote("ORDERID", row["ORDERID"]);
            sqlCreate.AppendQuote("ROUTECODE", row["ROUTECODE"]);
            sqlCreate.AppendQuote("ROUTENAME", row["ROUTENAME"]);
            sqlCreate.AppendQuote("CUSTOMERCODE", row["CUSTOMERCODE"]);
            sqlCreate.AppendQuote("CUSTOMERNAME", row["CUSTOMERNAME"]);
            sqlCreate.AppendQuote("CIGARETTECODE", row["CIGARETTECODE"]);
            sqlCreate.AppendQuote("CIGARETTENAME", row["CIGARETTENAME"]);
            sqlCreate.Append("QUANTITY", row["QUANTITY"]);
            sqlCreate.Append("SORTID", row["SORTID"]);
            //sqlCreate.AppendQuote("ADDRESS", row["ADDRESS"]);
            string sql = sqlCreate.GetSQL();
            ExecuteNonQuery(sql);
            System.Diagnostics.Debug.WriteLine(sql);
        }

        public void DeleteExists(string orderDate)
        {
            string sql = string.Format("DELETE FROM SORDER WHERE ROUTECODE IN (SELECT ROUTECODE FROM ROUTE WHERE ORDERDATE='{0}')", orderDate);
            ExecuteNonQuery(sql);
        }

        public void Delete(string orderDate, string batchNo)
        {
            string sql = string.Format("DELETE FROM SORDER WHERE ORDERDATE='{0}' AND BATCHNO={1}", orderDate, batchNo);
            ExecuteNonQuery(sql);
        }

        public void GetSorder()
        {
            string sql = "SELECT * FROM SORDER";
            ExecuteQuery(sql);
        }
        
        public DataTable FindHistoryData(string orderDate, string batchNo)
        {
            string sql = "SELECT ORDERDATE,BATCHNO,ROUTECODE,ROUTENAME,CUSTOMERCODE,CUSTOMERNAME,SUM(QUANTITY) QUANTITY FROM SORDERHISTORY " +
                "WHERE CIGARETTECODE IN (SELECT CIGARETTECODE FROM CIGARETTE WHERE ISABNORMITY='1') AND BATCHNO='{0}' AND ORDERDATE='{1}'" +
                "GROUP BY ORDERDATE,BATCHNO,ROUTECODE,ROUTENAME,CUSTOMERCODE,CUSTOMERNAME " +
                "ORDER BY ROUTECODE,CUSTOMERCODE";
            return ExecuteQuery(sql).Tables[0];
        }
        public DataTable FindBatchNo()
        {
            string sql = "SELECT DISTINCT " +
                            " CONVERT(varchar(10),ORDERDATE,120) + '|' + ltrim(STR(BATCHNO)) AS BATCHINFO," +
                            " CONVERT(varchar(10),ORDERDATE,120) + ' µÚ ' + ltrim(STR(BATCHNO)) + ' Åú´Î '  AS BATCHNAME" +
                            " FROM SORDERHISTORY ";
            return ExecuteQuery(sql).Tables[0];
        }
    }
}
