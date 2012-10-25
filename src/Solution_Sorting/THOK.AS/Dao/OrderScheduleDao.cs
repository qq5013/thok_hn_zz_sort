using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.AS.Dao
{
    public class OrderScheduleDao : BaseDao
    {
        public DataTable FindMasterAll(int startRecord, int pageSize, string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);
            string sql = "SELECT * FROM AS_SC_PALLETMASTER " + where;
            sql += " ORDER BY ORDERDATE,BATCHNO,LINECODE,SORTNO";
            return ExecuteQuery(sql, "AS_SC_PALLETMASTER", startRecord, pageSize).Tables[0];
        }

        public int FindMasterCount(string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);

            string sql = "SELECT COUNT(*) FROM AS_SC_PALLETMASTER " + where;
            return (int)ExecuteScalar(sql);
        }

        public DataTable FindDetailAll(int startRecord, int pageSize, string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);
            string sql = "SELECT * FROM AS_SC_ORDER " + where;
            sql += " ORDER BY CHANNELCODE";
            return ExecuteQuery(sql, "AS_SC_ORDER", startRecord, pageSize).Tables[0];
        }

        public int FindDetailCount(string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);

            string sql = "SELECT COUNT(*) FROM AS_SC_ORDER " + where;
            return (int)ExecuteScalar(sql);
        }

        public DataTable FindLineAll(int startRecord, int pageSize, string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);
            string sql = "SELECT ORDERDATE,BATCHNO,LINECODE,CIGARETTECODE,CIGARETTENAME, SUM(QUANTITY) QUANTITY, SUM(QUANTITY)/50 JQUANTITY, SUM(QUANTITY)%50 TQUANTITY FROM AS_SC_ORDER " + where;
            sql += " GROUP BY ORDERDATE,BATCHNO,LINECODE,CIGARETTECODE,CIGARETTENAME ORDER BY ORDERDATE,BATCHNO,LINECODE";
            return ExecuteQuery(sql, "AS_SC_ORDER", startRecord, pageSize).Tables[0];
        }

        public int FindLineCount(string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);

            string sql = "SELECT COUNT(*) FROM "+
                "(SELECT ORDERDATE,BATCHNO,LINECODE,CIGARETTECODE,CIGARETTENAME, SUM(QUANTITY) QUANTITY, SUM(QUANTITY)/50 JQUANTITY, SUM(QUANTITY)%50 TQUANTITY FROM AS_SC_ORDER " +
                "GROUP BY ORDERDATE,BATCHNO,LINECODE,CIGARETTECODE,CIGARETTENAME) A" + where;
            return (int)ExecuteScalar(sql);
        }

        public void SaveOrder(DataSet ds)
        {
            foreach (DataRow masterRow in ds.Tables["MASTER"].Rows)
            {
                masterRow["QUANTITY"] = ds.Tables["DETAIL"].Compute("SUM(QUANTITY)", string.Format("SORTNO={0}", masterRow["SORTNO"]));
            }

            SaveOrderMaster(ds.Tables["MASTER"]);
            SaveOrderSchedule(ds.Tables["DETAIL"]);
        }

        //public void SaveOrder(DataTable orderTable)
        //{
        //    foreach (DataRow orderRow in orderTable.Rows)
        //    {
        //        SqlCreate sql = new SqlCreate("AS_SC_ORDER", SqlType.INSERT);
        //        sql.AppendQuote("ORDERDATE", orderRow["ORDERDATE"]);
        //        sql.Append("BATCHNO", orderRow["BATCHNO"]);
        //        sql.AppendQuote("LINECODE", orderRow["LINECODE"]);
        //        sql.Append("SORTNO", orderRow["SORTNO"]);
        //        sql.Append("PALLETNO", orderRow["PALLETNO"]);

        //        sql.AppendQuote("CHANNELCODE", orderRow["CHANNELCODE"]);
        //        sql.AppendQuote("CIGARETTECODE", orderRow["CIGARETTECODE"]);

        //        sql.AppendQuote("CIGARETTENAME", orderRow["CIGARETTENAME"]);
        //        sql.Append("QUANTITY", orderRow["QUANTITY"]);

        //        ExecuteNonQuery(sql.GetSQL());
        //    }
        //}

        public void SaveOrderMaster(DataTable masterTable)
        {
            foreach (DataRow orderRow in masterTable.Rows)
            {
                SqlCreate sql = new SqlCreate("AS_SC_PALLETMASTER", SqlType.INSERT);
                sql.AppendQuote("ORDERDATE", orderRow["ORDERDATE"]);
                sql.Append("BATCHNO", orderRow["BATCHNO"]);
                sql.AppendQuote("LINECODE", orderRow["LINECODE"]);
                sql.Append("SORTNO", orderRow["SORTNO"]);
                sql.AppendQuote("ORDERID", orderRow["ORDERID"]);

                sql.AppendQuote("AREACODE", orderRow["AREACODE"]);
                sql.AppendQuote("AREANAME", orderRow["AREANAME"]);

                sql.AppendQuote("ROUTECODE", orderRow["ROUTECODE"]);
                sql.AppendQuote("ROUTENAME", orderRow["ROUTENAME"]);

                sql.AppendQuote("CUSTOMERCODE", orderRow["CUSTOMERCODE"]);
                sql.AppendQuote("CUSTOMERNAME", orderRow["CUSTOMERNAME"]);

                sql.AppendQuote("ADDRESS", orderRow["ADDRESS"]);
                sql.AppendQuote("ORDERNO", orderRow["ORDERNO"]);

                sql.Append("QUANTITY", orderRow["QUANTITY"]);
                sql.Append("ABNORMITY_QUANTITY", orderRow["ABNORMITY_QUANTITY"]);

                ExecuteNonQuery(sql.GetSQL());
            }
        }

        public void SaveOrderSchedule(DataTable orderTable)
        {
            foreach (DataRow orderRow in orderTable.Rows)
            {
                SqlCreate sql = new SqlCreate("AS_SC_ORDER", SqlType.INSERT);
                sql.Append("SORTNO", orderRow["SORTNO"]);
                sql.AppendQuote("LINECODE", orderRow["LINECODE"]);
                sql.AppendQuote("BATCHNO", orderRow["BATCHNO"]);
                sql.AppendQuote("ORDERID", orderRow["ORDERID"]);
                sql.Append("ORDERNO", 1);
                sql.AppendQuote("ORDERDATE", orderRow["ORDERDATE"]);
                sql.AppendQuote("CIGARETTECODE", orderRow["CIGARETTECODE"]);
                sql.AppendQuote("CIGARETTENAME", orderRow["CIGARETTENAME"]);
                sql.AppendQuote("CHANNELCODE", orderRow["CHANNELCODE"]);
                sql.Append("QUANTITY", orderRow["QUANTITY"]);
                ExecuteNonQuery(sql.GetSQL());
            }
        }

        public DataSet FindOrder(string orderDate, int batchNo, string lineCode)
        {
            string sql = "SELECT * FROM AS_SC_ORDER WHERE ORDERDATE = '{0}' AND BATCHNO = '{1}' AND LINECODE = '{2}' ORDER BY CHANNELCODE";
            return ExecuteQuery(string.Format(sql, orderDate, batchNo, lineCode));
        }

        public DataSet FindOrder2(string orderDate, int batchNo, string lineCode)
        {
            string sql = "SELECT * FROM AS_SC_ORDER WHERE ORDERDATE = '{0}' AND BATCHNO = '{1}' AND LINECODE = '{2}' ORDER BY SORTNO";
            return ExecuteQuery(string.Format(sql, orderDate, batchNo, lineCode));
        }

        public void DeleteHistory(string orderDate)
        {

            string sql = string.Format("DELETE FROM AS_SC_ORDER WHERE ORDERDATE < '{0}'", orderDate);
            ExecuteNonQuery(sql);

            sql = string.Format("DELETE FROM AS_SC_PALLETDETAIL WHERE ORDERDATE < '{0}'", orderDate);
            ExecuteNonQuery(sql);

            sql = string.Format("DELETE FROM AS_SC_PALLETMASTER WHERE ORDERDATE < '{0}'", orderDate);
            ExecuteNonQuery(sql);

        }

        public void DeleteSchedule(string orderDate, int batchNo)
        {
            string sql = string.Format("DELETE FROM AS_SC_ORDER WHERE ORDERDATE = '{0}' AND BATCHNO = '{1}'", orderDate, batchNo);
            ExecuteNonQuery(sql);

            sql = string.Format("DELETE FROM AS_SC_PALLETDETAIL WHERE ORDERDATE = '{0}' AND BATCHNO = '{1}'", orderDate, batchNo);
            ExecuteNonQuery(sql);

            sql = string.Format("DELETE FROM AS_SC_PALLETMASTER WHERE ORDERDATE = '{0}' AND BATCHNO = '{1}'", orderDate, batchNo);
            ExecuteNonQuery(sql);
        }

        public DataTable FindOrder(string orderDate, int batchNo)
        {
            string sql = "SELECT A.SORTNO, A.ORDERID,A.CUSTOMERCODE,A.CUSTOMERNAME,B.CIGARETTECODE,B.CIGARETTENAME,B.QUANTITY, " +
                            " A.BATCHNO,A.SORTNO ORDERNO,A.ROUTECODE,A.ROUTENAME,CONVERT(NVARCHAR(10),A.ORDERDATE,120) ORDERDATE, " +
                            " CONVERT(NVARCHAR(10),Z.SCDATE,120) SCDATE,A.LINECODE,'1' AS ZZBS  "+
                            " FROM AS_SC_PALLETMASTER A " +
                            " LEFT JOIN AS_SC_ORDER B ON A.ORDERDATE = B.ORDERDATE AND A.BATCHNO = B.BATCHNO AND A.LINECODE=B.LINECODE AND A.SORTNO=B.SORTNO " +
                            " LEFT JOIN AS_BI_BATCH Z ON A.ORDERDATE = Z.ORDERDATE AND A.BATCHNO = Z.BATCHNO "+
                            " WHERE A.ORDERDATE='{0}' AND A.BATCHNO='{1}' "+
                            " ORDER BY A.LINECODE,A.SORTNO,B.CIGARETTECODE";
            return ExecuteQuery(string.Format(sql,orderDate, batchNo)).Tables[0];
        }

        public DataTable FindOrderForAbnormity(string orderDate, int batchNo)
        {
            string sql = "SELECT ROW_NUMBER() OVER (ORDER BY D.SORTID,C.SORTID,A.ORDERID ,B.CIGARETTECODE) AS SORTNO, "+
                            " A.ORDERID,C.CUSTOMERCODE,C.CUSTOMERNAME,"+
                            " B.CIGARETTECODE,B.CIGARETTENAME,B.QUANTITY, "+
                            " A.BATCHNO,"+
                            " ROW_NUMBER() OVER (ORDER BY D.SORTID,C.SORTID,A.ORDERID ,B.CIGARETTECODE) ORDERNO,"+
                            " D.ROUTECODE,D.ROUTENAME,"+
                            " CONVERT(NVARCHAR(10),A.ORDERDATE,120) ORDERDATE,"+
                            " CONVERT(NVARCHAR(10),Z.SCDATE,120) SCDATE,"+
                            " '03' AS LINECODE,'1' AS ZZBS  "+
                            " FROM AS_I_ORDERMASTER A "+
                            " LEFT JOIN AS_I_ORDERDETAIL B "+
                            " ON A.ORDERID = B.ORDERID "+
                            " LEFT JOIN AS_BI_BATCH Z "+
                            " ON A.ORDERDATE = Z.ORDERDATE AND A.BATCHNO = Z.BATCHNO "+
                            " LEFT JOIN AS_BI_CUSTOMER C "+
                            " ON A.CUSTOMERCODE = C.CUSTOMERCODE"+
                            " LEFT JOIN AS_BI_ROUTE D"+
                            " ON A.ROUTECODE = D.ROUTECODE "+
                            " LEFT JOIN AS_BI_CIGARETTE E"+
                            " ON B.CIGARETTECODE = E.CIGARETTECODE"+
                            " WHERE A.ORDERDATE='{0}' AND A.BATCHNO='{1}' AND B.QUANTITY IS NOT NULL AND E.ISABNORMITY = '1'"+
                            " ORDER BY LINECODE,SORTNO,CIGARETTECODE";
            return ExecuteQuery(string.Format(sql,orderDate, batchNo)).Tables[0];
        }
    }
}
