using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
namespace THOK.AS.Dao
{
    public class OrderDao : BaseDao
    {
        public DataTable FindMasterAll(int startRecord, int pageSize, string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);
            string sql = "SELECT A.*,B.ROUTENAME,C.CUSTOMERNAME FROM AS_I_ORDERMASTER A " +
                "LEFT JOIN AS_BI_ROUTE B ON A.ROUTECODE=B.ROUTECODE " +
                "LEFT JOIN AS_BI_CUSTOMER C ON A.CUSTOMERCODE=C.CUSTOMERCODE " + where;
            sql += " ORDER BY ORDERDATE,BATCHNO, ORDERID";
            return ExecuteQuery(sql, "AS_I_ORDERMASTER", startRecord, pageSize).Tables[0];
        }

        public int FindMasterCount(string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);

            string sql = "SELECT COUNT(*) FROM AS_I_ORDERMASTER A ";
            sql += where;
            return (int)ExecuteScalar(sql);
        }

        public DataTable FindDetailAll(int startRecord, int pageSize, string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);
            string sql = "SELECT *,QUANTITY/50 JQUANTITY,QUANTITY%50 TQUANTITY FROM AS_I_ORDERDETAIL " + where;
            sql += " ORDER BY QUANTITY DESC";
            return ExecuteQuery(sql, "AS_I_ORDERDETAIL", startRecord, pageSize).Tables[0];
        }

        public int FindDetailCount(string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);

            string sql = "SELECT COUNT(*) FROM AS_I_ORDERDETAIL ";
            sql += where;
            return (int)ExecuteScalar(sql);
        }


        public DataTable FindRouteAll(int startRecord, int pageSize, string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);
            string sql = "SELECT A.*, C.AREANAME, D.ROUTENAME, CASE WHEN B.QUANTITY % 25 = 0 THEN B.QUANTITY / 25 ELSE B.QUANTITY / 25 + 1 END QUANTITY " +
                "FROM AS_I_ORDERMASTER A LEFT JOIN (SELECT ORDERID, SUM(QUANTITY) QUANTITY FROM AS_I_ORDERDETAIL WHERE CIGARETTECODE NOT IN " +
                "(SELECT CIGARETTECODE FROM AS_BI_CIGARETTE WHERE ISABNORMITY='1') GROUP BY ORDERID) B ON A.ORDERID=B.ORDERID " +
                "LEFT JOIN AS_BI_AREA C ON A.AREACODE=C.AREACODE LEFT JOIN AS_BI_ROUTE D ON A.ROUTECODE=D.ROUTECODE ";
            sql += where;
            sql += " ORDER BY ORDERDATE,BATCHNO,AREACODE,ROUTECODE,SORTID";
            return ExecuteQuery(sql, "AS_BI_ORDER", startRecord, pageSize).Tables[0];
        }

        public int FindRouteCount(string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);

            string sql = "SELECT COUNT(*) FROM (SELECT A.*, C.AREANAME, D.ROUTENAME, CASE WHEN B.QUANTITY % 25 = 0 THEN B.QUANTITY / 25 ELSE B.QUANTITY / 25 + 1 END QUANTITY "+
                "FROM AS_I_ORDERMASTER A LEFT JOIN (SELECT ORDERID, SUM(QUANTITY) QUANTITY FROM AS_I_ORDERDETAIL WHERE CIGARETTECODE NOT IN " +
                "(SELECT CIGARETTECODE FROM AS_BI_CIGARETTE WHERE ISABNORMITY='1') GROUP BY ORDERID) B ON A.ORDERID=B.ORDERID " +
                "LEFT JOIN AS_BI_AREA C ON A.AREACODE=C.AREACODE LEFT JOIN AS_BI_ROUTE D ON A.ROUTECODE=D.ROUTECODE) E "; 
            sql += where;
            return (int)ExecuteScalar(sql);
        }
        
        public DataSet FindRouteQuantity(string orderDate, int batchNo)
        {
            //排除异型烟
            string sql = "SELECT ROUTECODE, SORTID, SUM(QUANTITY) QUANTITY FROM (SELECT A.ROUTECODE, C.SORTID, B.QUANTITY FROM AS_I_ORDERMASTER A " +
                         "LEFT JOIN AS_I_ORDERDETAIL B ON A.ORDERID = B.ORDERID LEFT JOIN AS_BI_ROUTE C ON A.ROUTECODE = C.ROUTECODE AND A.AREACODE=C.AREACODE " +
                         "WHERE A.ORDERDATE = '{0}' AND A.BATCHNO = {1} AND CIGARETTECODE NOT IN (SELECT CIGARETTECODE FROM AS_BI_CIGARETTE WHERE ISABNORMITY = '1')) D " +
                         "GROUP BY ROUTECODE, SORTID ORDER BY SORTID";
            return ExecuteQuery(string.Format(sql, orderDate, batchNo));
        }

        /// <summary>
        /// 分拣烟道优化时，取卷烟名称及数量，进行优化。[ZENG]
        /// </summary>
        /// <param name="orderDate"></param>
        /// <param name="batchNo"></param>
        /// <param name="lineCode"></param>
        /// <returns></returns>
        public DataSet FindCigaretteQuantity(string orderDate, int batchNo, string lineCode)
        {
            //排除异型烟
            string sql = "SELECT A.CIGARETTECODE, B.CIGARETTENAME, " +
                            " SUM(A.QUANTITY) QUANTITY, SUM(A.QUANTITY -A.QUANTITY % 5) QUANTITY5" +
                            " FROM AS_I_ORDERDETAIL A" +
                            " LEFT JOIN AS_BI_CIGARETTE B ON A.CIGARETTECODE = B.CIGARETTECODE" +
                            " LEFT JOIN AS_I_ORDERMASTER C ON A.ORDERID = C.ORDERID" +
                            " LEFT JOIN AS_SC_LINE D ON A.ORDERDATE = D.ORDERDATE AND C.BATCHNO = D.BATCHNO AND C.ROUTECODE = D.ROUTECODE" +
                            " WHERE B.ISABNORMITY != '1' AND A.ORDERDATE='{0}' AND C.BATCHNO = '{1}' AND D.LINECODE = '{2}'" +
                            " GROUP BY A.CIGARETTECODE, B.CIGARETTENAME " +
                            " ORDER BY SUM(A.QUANTITY) DESC";

            return ExecuteQuery(string.Format(sql, orderDate, batchNo, lineCode));
        }

        /// <summary>
        /// 补货烟道优化时，取卷烟名称及数量，进行优化。[ZENG]
        /// </summary>
        /// <param name="orderDate"></param>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        public DataTable FindCigaretteQuantity(string orderDate, int batchNo)
        {
            //排除异型烟
            string sql = "SELECT A.CIGARETTECODE,B.CIGARETTENAME, SUM(QUANTITY) QUANTITY" +
                            " FROM AS_I_ORDERDETAIL A" +
                            " LEFT JOIN AS_BI_CIGARETTE B ON A.CIGARETTECODE =B.CIGARETTECODE" +
                            " LEFT JOIN AS_I_ORDERMASTER C ON A.ORDERID = C.ORDERID" +
                            " WHERE B.ISABNORMITY != '1' AND A.ORDERDATE='{0}' AND C.BATCHNO = '{1}' " +
                            " GROUP BY A.CIGARETTECODE, B.CIGARETTENAME HAVING SUM(QUANTITY) >= 50 " +
                            " ORDER BY SUM(QUANTITY) DESC";

            return ExecuteQuery(string.Format(sql, orderDate, batchNo)).Tables[0];
        }

        public DataSet FindOrderMaster(string orderDate, int batchNo, string lineCode)
        {
            string sql = "SELECT A.ORDERDATE,A.BATCHNO, B.LINECODE, ROW_NUMBER() over (ORDER BY D.SORTID,A.ROUTECODE, E.SORTID) SORTNO, " +
                            " A.ORDERID, A.AREACODE, C.AREANAME, A.ROUTECODE, D.ROUTENAME, A.CUSTOMERCODE, E.CUSTOMERNAME,E.ADDRESS, E.LICENSENO, " +
                            " 0 TQUANTITY, 0 QUANTITY, 0 PQUANTITY, 0 PACKQUANTITY,E.SORTID ORDERNO, 1 EXPORTNO, '0', NULL " +
                            " FROM AS_I_ORDERMASTER A " +
                            " LEFT JOIN AS_SC_LINE B ON A.ROUTECODE = B.ROUTECODE  AND A.ORDERDATE = B.ORDERDATE AND A.BATCHNO = B.BATCHNO " +
                            " LEFT JOIN AS_BI_AREA C ON A.AREACODE = C.AREACODE " +
                            " LEFT JOIN AS_BI_ROUTE D ON A.ROUTECODE = D.ROUTECODE " +
                            " LEFT JOIN AS_BI_CUSTOMER E ON A.CUSTOMERCODE = E.CUSTOMERCODE " +
                            " WHERE A.ORDERDATE = '{0}' AND A.BATCHNO = '{1}' AND B.LINECODE = '{2}' " + 
                            " ORDER BY SORTNO";

            sql = "SELECT A.ORDERDATE,A.BATCHNO, B.LINECODE,ROW_NUMBER() over (ORDER BY D.SORTID,A.ROUTECODE, E.SORTID) SORTNO, " +
                    " A.ORDERID, A.AREACODE, C.AREANAME, A.ROUTECODE, D.ROUTENAME, A.CUSTOMERCODE, E.CUSTOMERNAME,E.ADDRESS, E.LICENSENO, " +
                    " 0 TQUANTITY, 0 QUANTITY, 0 PQUANTITY, 0 PACKQUANTITY,ISNULL(SUM(H.QUANTITY),0) ABNORMITY_QUANTITY," +
                    //配送序号（业务配送序号）
                    //" E.SORTID ORDERNO, " + 
                    //分拣序号（根据业务配送序号分拣生成的连续顺序号）
                    " (SELECT ORDERNO FROM (SELECT *,ROW_NUMBER() over (ORDER BY SORTID) ORDERNO FROM AS_I_ORDERMASTER " +
		                " WHERE ROUTECODE = A.ROUTECODE AND ORDERDATE = '{0}' AND BATCHNO = '{1}') TEMP WHERE TEMP.ORDERID = A.ORDERID) ORDERNO," + 
                    " 1 EXPORTNO, '0', NULL" +            
                    " FROM AS_I_ORDERMASTER A " +
                    " LEFT JOIN AS_SC_LINE B ON A.ROUTECODE = B.ROUTECODE  AND A.ORDERDATE = B.ORDERDATE AND A.BATCHNO = B.BATCHNO " +
                    " LEFT JOIN AS_BI_AREA C ON A.AREACODE = C.AREACODE " +
                    " LEFT JOIN AS_BI_ROUTE D ON A.ROUTECODE = D.ROUTECODE " +
                    " LEFT JOIN AS_BI_CUSTOMER E ON A.CUSTOMERCODE = E.CUSTOMERCODE " +
	                " LEFT JOIN (SELECT F.* FROM AS_I_ORDERDETAIL F " +
				                " LEFT JOIN AS_BI_CIGARETTE G ON F.CIGARETTECODE = G.CIGARETTECODE WHERE G.ISABNORMITY ='1') H" +
                            " ON A.ORDERID = H.ORDERID" +
                    //
                    " LEFT JOIN (SELECT I.* FROM AS_I_ORDERDETAIL I " +
                                " LEFT JOIN AS_BI_CIGARETTE J ON I.CIGARETTECODE = J.CIGARETTECODE WHERE J.ISABNORMITY NOT IN ('1')) K" +
                            " ON A.ORDERID = K.ORDERID" +
                    //
                    " WHERE A.ORDERDATE = '{0}' AND A.BATCHNO = '{1}' AND B.LINECODE = '{2}' " +
                    " GROUP BY A.ORDERDATE,  A.BATCHNO, B.LINECODE,A.ORDERID, A.AREACODE, C.AREANAME, A.ROUTECODE," +
                        " D.ROUTENAME, A.CUSTOMERCODE, E.CUSTOMERNAME,E.ADDRESS, E.LICENSENO,D.SORTID,A.ROUTECODE, E.SORTID " +
                    //
                    " HAVING ISNULL(SUM(K.QUANTITY),0) > 0 " +
                    //
                    " ORDER BY SORTNO";
            sql = string.Format(sql,orderDate, batchNo, lineCode);
            return ExecuteQuery(sql);
        }

        public DataSet FindOrderDetail(string orderDate, int batchNo, string lineCode)
        {
            string sql = "SELECT A.*, B.CHANNELCODE FROM ( " +
                            "SELECT * FROM AS_I_ORDERDETAIL WHERE CIGARETTECODE NOT IN (SELECT CIGARETTECODE FROM AS_BI_CIGARETTE WHERE ISABNORMITY = '1') " +
                            "AND ORDERID IN (SELECT ORDERID FROM AS_I_ORDERMASTER WHERE ORDERDATE = '{0}' AND BATCHNO = '{1}' AND ROUTECODE IN ( " +
                            "SELECT ROUTECODE FROM AS_SC_LINE WHERE ORDERDATE = '{2}' AND BATCHNO = '{3}' AND LINECODE = '{4}'))) A " +
                            "LEFT JOIN (SELECT CIGARETTECODE,MIN(CHANNELCODE) CHANNELCODE FROM AS_SC_CHANNELUSED WHERE LINECODE='{4}' AND ORDERDATE = '{0}' AND BATCHNO = {1} GROUP BY CIGARETTECODE) B " +
                            "ON A.CIGARETTECODE = B.CIGARETTECODE ORDER BY ORDERID,CHANNELCODE";
            sql = string.Format(sql, orderDate, batchNo, orderDate, batchNo, lineCode);
            return ExecuteQuery(sql);
        }

        public DataTable FindOrderRoute(string orderDate, int batchNo)
        {
            string sql = string.Format("SELECT B.*,C.AREANAME " + 
                " FROM (SELECT DISTINCT ROUTECODE FROM AS_I_ORDERMASTER WHERE ORDERDATE='{0}' AND BATCHNO='{1}') A " +
                " LEFT JOIN AS_BI_ROUTE B ON A.ROUTECODE=B.ROUTECODE " + 
                " LEFT JOIN AS_BI_AREA C ON B.AREACODE=C.AREACODE " +
                " ORDER BY B.ROUTECODE", orderDate, batchNo);
            return ExecuteQuery(sql).Tables[0];
        }

        public void BatchInsertMaster(DataTable dtData)
        {
            BatchInsert(dtData, "AS_I_ORDERMASTER");
        }

        public void BatchInsertDetail(DataTable dtData)
        {
            BatchInsert(dtData, "AS_I_ORDERDETAIL");
        }

        public void DeleteHistory(string orderDate)
        {
            string sql = string.Format("DELETE FROM AS_I_ORDERDETAIL WHERE ORDERDATE < '{0}'", orderDate);
            ExecuteNonQuery(sql);

            sql = string.Format("DELETE FROM AS_I_ORDERMASTER WHERE ORDERDATE < '{0}'", orderDate);
            ExecuteNonQuery(sql);
        }

        public void DeleteOrder(string orderDate, int batchNo)
        {
            string sql = string.Format("DELETE FROM AS_I_ORDERDETAIL WHERE ORDERID IN (SELECT ORDERID FROM AS_I_ORDERMASTER WHERE ORDERDATE = '{0}' AND BATCHNO={1})", orderDate, batchNo);
            ExecuteNonQuery(sql);

            sql = string.Format("DELETE FROM AS_I_ORDERMASTER WHERE ORDERDATE = '{0}' AND BATCHNO = {1}", orderDate, batchNo);
            ExecuteNonQuery(sql);
        }

        public void DeleteNoUseOrder(string orderDate, int batchNo, string routes)
        {
            string sql = string.Format("DELETE FROM AS_I_ORDERDETAIL WHERE ORDERDATE='{0}' AND ORDERID NOT IN " +
                "(SELECT ORDERID FROM AS_I_ORDERMASTER WHERE ORDERDATE='{0}' AND BATCHNO={1} AND ROUTECODE IN ({2}))", orderDate, batchNo, routes);
            ExecuteNonQuery(sql);
            sql = string.Format("DELETE FROM AS_I_ORDERMASTER WHERE ORDERDATE='{0}' AND BATCHNO={1} AND ROUTECODE NOT IN ({2})", orderDate, batchNo, routes);
            ExecuteNonQuery(sql);
        }

        public void SetRouteNewSort(string route, int sortId)
        {
            string sql = String.Format("UPDATE AS_BI_ROUTE SET SORTID={0} WHERE ROUTECODE='{1}'", sortId, route);    
            ExecuteNonQuery(sql);
        }
    }
}
