using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.AS.Sorting.Dao
{
    public class OrderDao: BaseDao
    {
        public List<string> FindRouteMaxSortNoList()
        {
            List<string> routeMaxSortNoList = new List<string>();
            string sql = "SELECT MAX(SORTNO) AS ROUTE_MAX_SORTNO FROM AS_SC_PALLETMASTER GROUP BY ROUTECODE";
            DataTable table = ExecuteQuery(sql).Tables[0];
            foreach (DataRow row in table.Rows)
            {
                routeMaxSortNoList.Add(row["ROUTE_MAX_SORTNO"].ToString());
            }
            return routeMaxSortNoList;
        }
        public DataTable FindMaster()
        {
            string sql = "SELECT ORDERDATE,BATCHNO,SORTNO, ORDERID,ROUTECODE,ROUTENAME,CUSTOMERCODE,CUSTOMERNAME, " +
                "CASE STATUS WHEN '0' THEN '未下单' ELSE '已下单' END STATUS, CASE WHEN PACKQUANTITY=QUANTITY THEN '已发送' ELSE '未发送' END PACKAGE FROM AS_SC_PALLETMASTER";
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindDetail(string sortNo)
        {
            string sql = string.Format("SELECT A.SORTNO, ORDERID, B.CHANNELNAME, CASE B.CHANNELTYPE WHEN '2' THEN '立式机' WHEN '5' THEN '立式机' ELSE '通道机' END CHANNELTYPE, " +
                "A.CIGARETTECODE, A.CIGARETTENAME, A.QUANTITY FROM AS_SC_ORDER A LEFT JOIN AS_SC_CHANNELUSED B ON A.CHANNELCODE=B.CHANNELCODE " +
                "WHERE A.SORTNO={0} ORDER BY A.CHANNELCODE", sortNo);
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindCigarettes()
        {
            string sql = "SELECT CIGARETTECODE,CIGARETTENAME,SUM(QUANTITY) FROM AS_SC_ORDER GROUP BY CIGARETTECODE,CIGARETTENAME ORDER BY CIGARETTECODE";
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindOrderWithCigarette(string cigaretteCode)
        {
            string sql = "SELECT MIN(A.SORTNO) SORTNO,A.ORDERDATE,A.BATCHNO,A.LINECODE,A.ORDERID,B.ROUTENAME,B.CUSTOMERNAME,A.CIGARETTECODE,A.CIGARETTENAME,SUM(A.QUANTITY) QUANTITY,CHANNELCODE" +
                            " FROM AS_SC_ORDER A" +
                            " LEFT JOIN AS_SC_PALLETMASTER B ON A.ORDERID = B.ORDERID"+
                            " WHERE CIGARETTECODE = '{0}'" +
                            " GROUP BY A.ORDERDATE,A.BATCHNO,A.LINECODE,A.ORDERID,B.ORDERID,A.CIGARETTECODE,A.CIGARETTENAME,A.CHANNELCODE,B.ROUTENAME,B.CUSTOMERNAME" +
                            " ORDER BY A.ORDERDATE,A.BATCHNO,A.LINECODE,MIN(A.SORTNO)";
            sql = string.Format(sql,cigaretteCode);
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindPackMaster()
        {
            string sql = "SELECT ROW_NUMBER() OVER(ORDER BY MIN(SORTNO)) AS PACKNO,MIN(SORTNO) AS SORTNO ,ORDERDATE,ORDERID,ROUTECODE,ROUTENAME,CUSTOMERCODE,CUSTOMERNAME,SUM(QUANTITY) AS QUANTITY, " +
                            "CASE WHEN SUM(PACKQUANTITY)=SUM(QUANTITY) THEN '已发送' ELSE '未发送' END [PACKAGE]  " +
                            "FROM AS_SC_PALLETMASTER GROUP BY ORDERDATE,ROUTECODE,ROUTENAME,ORDERID,CUSTOMERCODE,CUSTOMERNAME ORDER BY SORTNO";
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindPackMaster(string [] filter)
        {
            string sql = "SELECT B.* FROM " +
                            " (" +
	                            " SELECT ROW_NUMBER() OVER(ORDER BY MIN(A.SORTNO)) AS PACKNO," +
	                            " MIN(A.SORTNO) AS SORTNO ,A.ORDERDATE,A.ORDERID,A.ROUTECODE,A.ROUTENAME," +
	                            " A.CUSTOMERCODE,A.CUSTOMERNAME,SUM(A.QUANTITY) AS QUANTITY," +
	                            " CASE WHEN SUM(A.PACKQUANTITY)=SUM(A.QUANTITY) THEN '已发送' ELSE '未发送' END PACKAGE" +
	                            " FROM AS_SC_PALLETMASTER A" +
	                            " GROUP BY A.ORDERDATE,A.ROUTECODE,A.ROUTENAME,A.ORDERID,A.CUSTOMERCODE,A.CUSTOMERNAME " +
                            " ) B " +
                            " LEFT JOIN AS_SC_ORDER C ON B.ORDERID = C.ORDERID " +
                            " WHERE {0} " +
                            " GROUP BY B.PACKNO,B.SORTNO,B.ORDERDATE,B.ROUTECODE,B.ROUTENAME,B.ORDERID,B.CUSTOMERCODE,B.CUSTOMERNAME,B.QUANTITY,B.PACKAGE" +
                            " {1} " +
                            " ORDER BY SORTNO";
            return ExecuteQuery(string.Format(sql,filter)).Tables[0];
        }


        public DataTable FindPackDetail(string orderId)
        {
            string sql = string.Format("SELECT A.ORDERID, A.CIGARETTECODE,A.CIGARETTENAME, SUM(A.QUANTITY) QUANTITY " +
                "FROM AS_SC_ORDER A  LEFT JOIN dbo.AS_SC_CHANNELUSED B ON A.CHANNELCODE = B.CHANNELCODE where A.ORDERID='{0}'  " +
                "GROUP BY ORDERID, A.SORTNO ,B.CHANNELNAME,A.CIGARETTECODE,A.CIGARETTENAME ORDER BY A.SORTNO,B.CHANNELNAME", orderId);
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindSortMaster()
        {
            string sql = "SELECT TOP 1 * FROM AS_SC_PALLETMASTER WHERE STATUS=0 ORDER BY SORTNO";
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindSortSpeed()
        {
            string sql = "SELECT * FROM 效率报表";
            return ExecuteQuery(sql).Tables[0];
        }

        public string FindMaxSortNoFromMasterByOrderID(string orderID)
        {
            string sql = "SELECT CASE WHEN MAX(SORTNO) IS NULL THEN 0 ELSE MAX(SORTNO) END FROM AS_SC_PALLETMASTER WHERE ORDERID= '{0}'";
            sql = string.Format(sql, orderID);
            return ExecuteScalar(sql).ToString();
        }

        public string FindMaxSortedMaster()
        {
            string sql = "SELECT CASE WHEN MAX(SORTNO) IS NULL THEN 0 ELSE MAX(SORTNO) END FROM AS_SC_PALLETMASTER WHERE STATUS='1'";
            return ExecuteScalar(sql).ToString();
        }

        public DataTable FindSortDetail(string sortNo)
        {
            string strSql = "SELECT A.CHANNELADDRESS,A.CHANNELCODE, A.CHANNELTYPE, CASE WHEN B.QUANTITY IS NULL THEN 0 ELSE B.QUANTITY END QUANTITY " +
                "FROM AS_SC_CHANNELUSED A LEFT JOIN (SELECT SORTNO,CHANNELCODE,SUM(QUANTITY)  QUANTITY FROM AS_SC_ORDER GROUP BY SORTNO,CHANNELCODE) B ON A.CHANNELCODE = B.CHANNELCODE AND B.SORTNO = '{0}' " +
                "ORDER BY A.CHANNELTYPE , A.CHANNELCODE";
            strSql = string.Format(strSql, sortNo);
            return ExecuteQuery(strSql).Tables[0];
        }

        public DataTable FindMasterInfo(string sortNo)
        {
            string sql = string.Format("SELECT ROUTENAME, CUSTOMERNAME FROM AS_SC_PALLETMASTER WHERE SORTNO={0}", sortNo);
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindCigaretteDetail(string sortNo)
        {
            string sql = string.Format("SELECT CIGARETTENAME, SUM(QUANTITY) QUANTITY FROM AS_SC_ORDER WHERE SORTNO={0} " +
                "GROUP BY CIGARETTENAME ORDER BY SUM(QUANTITY) DESC", sortNo);
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindOrderInfo(string sortNo)
        {
            string sql = "SELECT COUNT(DISTINCT CUSTOMERCODE) CUSTOMERNUM, COUNT(DISTINCT ROUTECODE) ROUTENUM, ISNULL(SUM(QUANTITY),0) QUANTITY " +
                "FROM AS_SC_PALLETMASTER ";
            if (sortNo != null)
                sql += ("WHERE SORTNO <= " + sortNo);
            return ExecuteQuery(sql).Tables[0];
        }

        public string FindLastSortNo()
        {
            string sql = "SELECT CASE WHEN MAX(SORTNO) IS NULL THEN '0' ELSE MAX(SORTNO) END FROM AS_SC_PALLETMASTER WHERE STATUS='1'";
            return ExecuteScalar(sql).ToString();
        }

        public void UpdateFinisheTime(string sortNo)
        {
            try
            {
                string sql = "UPDATE AS_SC_PALLETMASTER SET FINISHEDTIME = GETDATE() WHERE STATUS='1' AND SORTNO = {0} ";
                ExecuteNonQuery(string.Format(sql,sortNo));
            }
            catch (Exception)
            {
            }
        }

        public DataTable FindPackInfo()
        {
            //string sql = "SELECT TOP 1 SORTNO, (CEILING(QUANTITY/25.0)-CEILING((QUANTITY-PACKQUANTITY)/25.0)+1)  " +
            //    "AS BAGSN,CASE WHEN QUANTITY-PACKQUANTITY>=30 THEN 25 WHEN QUANTITY-PACKQUANTITY>25 THEN 20 ELSE QUANTITY-PACKQUANTITY " +
            //    "END AS QUANTITY FROM AS_SC_PALLETMASTER WHERE QUANTITY-PACKQUANTITY>0 ORDER BY SORTNO";
            string sql = "SELECT TOP 400 MIN(SORTNO) SORTNO, ORDERID,SUM(QUANTITY) QUANTITY FROM AS_SC_PALLETMASTER WHERE QUANTITY-PACKQUANTITY>0 GROUP BY ORDERID ORDER BY SORTNO";
            return ExecuteQuery(sql).Tables[0];
        }

        public void UpdatePackQuantity(string sortNo, int quantity)
        {
            string sql = string.Format("UPDATE AS_SC_PALLETMASTER SET PACKQUANTITY = {0} WHERE SORTNO = {1}", quantity, sortNo);
            ExecuteNonQuery(sql);
        }

        public void UpdatePackQuantityByOrderID(string orderID)
        {
            string sql = string.Format("UPDATE AS_SC_PALLETMASTER SET PACKQUANTITY = QUANTITY WHERE ORDERID = '{0}'", orderID);
            ExecuteNonQuery(sql);
        }

        public void UpdateOrderStatus(string sortNo, string status)
        {
            string sql = string.Format("UPDATE AS_SC_PALLETMASTER SET STATUS = '{0}' WHERE SORTNO = {1}", status, sortNo);
            ExecuteNonQuery(sql);
        }

        public void UpdateChannel(string sourceChannel, string targetChannel)
        {
            string sql = string.Format("UPDATE AS_SC_ORDER SET CHANNELCODE='{0}' WHERE CHANNELCODE='{1}'", targetChannel, sourceChannel);
            ExecuteNonQuery(sql);
        }

        public void ClearPackQuantity(string orderID)
        {
            string sql = string.Format("UPDATE AS_SC_PALLETMASTER SET PACKQUANTITY=0 WHERE SORTNO>= (SELECT MIN(SORTNO) FROM AS_SC_PALLETMASTER WHERE ORDERID = '{0}' )", orderID);
            ExecuteNonQuery(sql);
        }

        public void UpdatePackQuantity(string orderID)
        {
            string sql = string.Format("UPDATE AS_SC_PALLETMASTER SET PACKQUANTITY = QUANTITY WHERE SORTNO <= (SELECT MAX(SORTNO) FROM AS_SC_PALLETMASTER WHERE ORDERID = '{0}' )", orderID);
            ExecuteNonQuery(sql);
        }

        public void InsertMaster(DataTable masterTable)
        {
            ExecuteQuery("TRUNCATE TABLE AS_SC_PALLETMASTER");
            BatchInsert(masterTable, "AS_SC_PALLETMASTER");
        }

        public void InsertDetail(DataTable detailTable)
        {
            ExecuteQuery("TRUNCATE TABLE AS_SC_PALLETDETAIL");
            BatchInsert(detailTable, "AS_SC_PALLETDETAIL");
        }

        public void InsertOrder(DataTable orderTable)
        {
            ExecuteQuery("TRUNCATE TABLE AS_SC_ORDER");
            BatchInsert(orderTable, "AS_SC_ORDER");
        }

        public void InsertHandleSupply(DataTable handleSupplyOrderTable)
        {
            ExecuteQuery("TRUNCATE TABLE AS_SC_HANDLESUPPLY");
            BatchInsert(handleSupplyOrderTable, "AS_SC_HANDLESUPPLY");
        }

        public int FindUnsortCount()
        {
            string sql = "SELECT COUNT(*) FROM AS_SC_PALLETMASTER WHERE STATUS='0'";
            return Convert.ToInt32(ExecuteScalar(sql));
        }

        public void UpdateMissOrderStatus(string sortNo, string status)
        {
            string sql = string.Format("UPDATE AS_SC_PALLETMASTER SET STATUS = '{0}' WHERE SORTNO >= {1}", status, sortNo);
            ExecuteNonQuery(sql);
            sql = string.Format("UPDATE AS_SC_PALLETMASTER SET STATUS = '{0}' WHERE SORTNO < {1}", "1", sortNo);            
            ExecuteNonQuery(sql);
        }

        public void UpdateQuantity(string sortNo, string orderId,string channelName,string cigaretteCode, int quantity)
        {
            string sql = string.Format("UPDATE AS_SC_ORDER SET QUANTITY = {0} WHERE SORTNO = {1} AND ORDERID = '{2}' AND CIGARETTECODE = '{3}' ", quantity,sortNo, orderId, cigaretteCode);
            ExecuteNonQuery(sql);
            sql = string.Format("UPDATE AS_SC_HANDLESUPPLY SET QUANTITY = {0} WHERE SORTNO = {1} AND ORDERID = '{2}' AND CIGARETTECODE = '{3}' ", quantity, sortNo, orderId, cigaretteCode);
            ExecuteNonQuery(sql);
            sql = string.Format("UPDATE AS_SC_CHANNELUSED SET QUANTITY = (SELECT SUM(QUANTITY) FROM AS_SC_ORDER WHERE AS_SC_ORDER.CHANNELCODE = AS_SC_CHANNELUSED.CHANNELCODE) WHERE CHANNELNAME = '{0}' ", channelName);
            ExecuteNonQuery(sql);
            sql = string.Format("UPDATE AS_SC_PALLETMASTER SET QUANTITY = (SELECT SUM(QUANTITY) FROM AS_SC_ORDER WHERE SORTNO = {0}) WHERE SORTNO = {0} ", sortNo);
            ExecuteNonQuery(sql);

            sql = string.Format("SELECT * FROM AS_SC_CHANNELUSED WHERE CHANNELNAME = '{0}' " , channelName);
            DataTable channelTable = ExecuteQuery(sql).Tables[0];
            sql = string.Format("SELECT A.* FROM AS_SC_ORDER A LEFT JOIN AS_SC_CHANNELUSED B ON A.CHANNELCODE = B.CHANNELCODE WHERE B.CHANNELNAME = '{0}' ", channelName);
            DataTable orderTable = ExecuteQuery(sql).Tables[0];
            Util.SetChannelSortNoUtil.SetChannelSortNo(channelTable,orderTable);
            sql = string.Format("UPDATE AS_SC_CHANNELUSED SET SORTNO = {0} WHERE CHANNELNAME = '{0}' ",channelTable.Rows[0]["SORTNO"] ,channelName);
            ExecuteNonQuery(sql);
        }

        //查询分拣效率平均值
        public int FindSortingAverage()
        {
            string sql = "SELECT (CASE WHEN COUNT(*)>=1 THEN (SUM(分拣效率)/COUNT(*)) ELSE 0 END) AS AVERAGE FROM 效率报表 ";
            return Convert.ToInt32(ExecuteQuery(sql).Tables[0].Rows[0][0]);
        }
    }
}
