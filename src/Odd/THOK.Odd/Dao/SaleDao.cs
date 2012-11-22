using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.Odd.Dao
{
    internal class SaleDao: BaseDao
    {
        public DataTable FindCigarette()
        {
            string sql = "SELECT BRAND_CODE AS CIGARETTECODE,BRAND_NAME AS CIGARETTENAME,IS_ABNORMITY_BRAND AS ISABNORMITY FROM DWV_INF_BRAND";
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindOrder(DateTime orderDate, string batchNo)
        {
            string sql = string.Format("SELECT '{0}' ORDERDATE, {1} BATCHNO, A.ORDER_ID AS ORDERID, " +
               "A.DELIVER_LINE_CODE AS ROUTECODE, B.DELIVER_LINE_NAME AS ROUTENAME, " +
               "C.CUST_N AS CUSTOMERCODE, C.CUST_NAME AS CUSTOMERNAME, C.PRINCIPAL_ADDRESS AS ADDRESS, " +               //
               "D.BRAND_CODE AS CIGARETTECODE, D.BRAND_NAME AS CIGARETTENAME,QUANTITY, A.DELIVER_ORDER AS SORTID FROM DWV_ORD_ORDER A " +
               "LEFT JOIN DWV_CAR_DELIVER_LINE B ON A.DELIVER_LINE_CODE=B.DELIVER_LINE_CODE " +
               "LEFT JOIN DWV_ORG_CUSTOMER C ON A.CUST_CODE=C.CUST_CODE " +
               "LEFT JOIN DWV_ORD_ORDER_DETAIL D ON A.ORDER_ID=D.ORDER_ID WHERE A.ORDER_DATE LIKE '{2}%'", orderDate, batchNo, orderDate.ToString("yyyyMMdd"));
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FIND()
        {
            string sql = "select * from DWV_ORD_ORDER where ORDER_DATE LIKE '20101203%' ";
            return ExecuteQuery(sql).Tables[0];
        }
    }
}
