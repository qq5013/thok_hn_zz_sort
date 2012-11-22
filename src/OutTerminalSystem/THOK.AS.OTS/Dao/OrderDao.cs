using System;
using System.Collections.Generic;
using System.Text;
using THOK.Util;
using System.Data;

namespace THOK.AS.OTS.Dao
{
    public class OrderDao: BaseDao
    {

        public DataTable FindOrder()
        {
            string sql = "SELECT LINECODE, BATCHNO, ORDERID, ORDERDATE, CIGARETTECODE, CIGARETTENAME, CHANNELCODE, SUM(QUANTITY) QUANTITY " +
                "FROM AS_SC_ORDER GROUP BY LINECODE, BATCHNO, ORDERID, ORDERDATE, CIGARETTECODE, CIGARETTENAME, CHANNELCODE ORDER BY ORDERID";
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindRoute()
        {
            string sql = "SELECT ORDERDATE,AREACODE,AREANAME,ROUTECODE,ROUTENAME ,MIN(SORTNO) SORTNO ,Count(DISTINCT ORDERID) ORDERCOUNT,SUM(QUANTITY) QUANTITY FROM AS_SC_PALLETMASTER " +
                "GROUP BY ORDERDATE,AREACODE,AREANAME,ROUTECODE,ROUTENAME ORDER BY SORTNO";
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindCustomer()
        {
            string sql = "SELECT ORDERID,ROUTECODE,ROUTENAME,CUSTOMERCODE,CUSTOMERNAME, QUANTITY FROM V_PALLETMASTER ORDER BY SORTNO";
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindOrderMaster()
        {
            string sql = "SELECT  ORDERDATE, ORDERID, ROUTECODE, ROUTENAME, CUSTOMERCODE, CUSTOMERNAME,ADDRESS,ORDERNO, QUANTITY ,CASE WHEN PACKQUANTITY=QUANTITY THEN '已发送' ELSE '未发送' END PACKAGE " +
                "FROM V_PALLETMASTER ORDER BY SORTNO";

            sql = "SELECT SORTNO, ORDERDATE, ORDERID, ROUTECODE, ROUTENAME, CUSTOMERCODE, CUSTOMERNAME, ADDRESS,ORDERNO,QUANTITY ,CASE WHEN PACKQUANTITY=QUANTITY THEN '已发送' ELSE '未发送' END PACKAGE ," +                    
            "CASE WHEN PRINTSTATUS=1 THEN '已打印' ELSE '未打印' END PRINTSTATUS FROM V_PALLETMASTER ORDER BY SORTNO";

            sql = "SELECT ROW_NUMBER() OVER(ORDER BY MIN(SORTNO)) AS CUSTOMERNO,MIN(SORTNO) AS SORTNO ,ORDERDATE,ORDERID,ROUTECODE,ROUTENAME,CUSTOMERCODE,CUSTOMERNAME,ADDRESS,ORDERNO,MAX(ORDERNO) AS SUMORDERNO,SUM(QUANTITY) AS QUANTITY, ABNORMITY_QUANTITY," +
                    "CASE WHEN SUM(PACKQUANTITY)=SUM(QUANTITY) THEN '已发送' ELSE '未发送' END [PACKAGE], "+
                    "CASE WHEN PRINTSTATUS=1 THEN '已打印' ELSE '未打印' END PRINTSTATUS "+
                    "FROM AS_SC_PALLETMASTER GROUP BY ORDERDATE,ROUTECODE,ROUTENAME,ORDERID,CUSTOMERCODE,CUSTOMERNAME,ADDRESS,ORDERNO,PRINTSTATUS,ABNORMITY_QUANTITY ORDER BY SORTNO";
            return ExecuteQuery(sql).Tables[0];
        }

        public string FindMaxOrderNo(string routeCode)
        {
            string sql = string.Format("SELECT MAX(ORDERNO) FROM AS_SC_PALLETMASTER  WHERE ROUTECODE ='{0}' ",routeCode);
            return ExecuteScalar(sql).ToString();
        }

        public DataTable FindOrderMaster(string routeCode)
        {
            string sql = string.Format("SELECT * FROM V_PALLETMASTER WHERE ROUTECODE='{0}' ORDER BY SORTNO", routeCode);
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindOrderDetail()
        {
            string sql = "SELECT A.ORDERID, A.CIGARETTECODE,A.CIGARETTENAME, SUM(A.QUANTITY) QUANTITY " +
                "FROM AS_SC_ORDER A  LEFT JOIN dbo.AS_SC_CHANNELUSED B ON A.CHANNELCODE = B.CHANNELCODE GROUP BY ORDERID, A.SORTNO ,B.CHANNELNAME,A.CIGARETTECODE,A.CIGARETTENAME ORDER BY A.SORTNO,B.CHANNELNAME";
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindOrderDetail(string orderID)
        {
            string sql = string.Format("SELECT A.CIGARETTENAME, SUM(A.QUANTITY) QUANTITY FROM AS_SC_ORDER A  LEFT JOIN dbo.AS_SC_CHANNELUSED B ON A.CHANNELCODE = B.CHANNELCODE WHERE ORDERID = '{0}' GROUP BY A.SORTNO ,B.CHANNELNAME,A.CIGARETTECODE,A.CIGARETTENAME ORDER BY A.SORTNO,B.CHANNELNAME", orderID);
            return ExecuteQuery(sql).Tables[0];
        }

        public void UpdateOrderPrintStatus(string orderID)
        {
            string sql = string.Format("UPDATE AS_SC_PALLETMASTER SET PRINTSTATUS = '1' WHERE ORDERID ='{0}' ",orderID);
            ExecuteNonQuery(sql);
        }

        public DataTable FindDetail(string orderID)
        {
            string sql = string.Format("SELECT * FROM AS_SC_ORDER WHERE ORDERID='{0}' ORDER BY SORTNO,CHANNELCODE", orderID);
            return ExecuteQuery(sql).Tables[0];
        }
        public string FindMinSortNo()
        {
            string sql = "SELECT MIN(SORTNO) FROM V_PALLETMASTER WHERE PRINTSTATUS = '0' ";
            return ExecuteScalar(sql).ToString();
        }

        public DataTable FindMasterTable(string sortNo)
        {
            string sql = string.Format("SELECT * FROM V_PALLETMASTER WHERE SORTNO>0 AND SORTNO <= {0} AND PRINTSTATUS = '0'  ORDER BY SORTNO ", sortNo);
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindNextMaster(string sortNo)
        {
            string sql = string.Format("SELECT TOP 1 * FROM V_PALLETMASTER WHERE SORTNO > {0} ORDER BY SORTNO ", sortNo);
            return ExecuteQuery(sql).Tables[0];
        }

        //修改标识
        public void UpdatePrintStatusInOne(string orderId)
        {
            string sql = string.Format("UPDATE AS_SC_PALLETMASTER SET PRINTSTATUS = 1 WHERE SORTNO <= (SELECT MAX(SORTNO) FROM AS_SC_PALLETMASTER WHERE ORDERID = '{0}')", orderId);
            ExecuteNonQuery(sql);
        }
        //修改清除
        public void UpdatePrintStatusIsZero(string orderId)
        {
            string sql = string.Format("UPDATE AS_SC_PALLETMASTER SET PRINTSTATUS = 0 WHERE SORTNO >=(SELECT MIN(SORTNO) FROM AS_SC_PALLETMASTER WHERE ORDERID = '{0}')", orderId);
            ExecuteNonQuery(sql);
        }
    }
}