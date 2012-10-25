using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.AS.Dao
{
    public class SalesSystemDao: BaseDao
    {
        public string dbTypeName = "test";
        public SalesSystemDao()
        {
            THOK.AS.Dao.SysParameterDao parameterDao = new SysParameterDao();
            Dictionary<string, string> parameter = parameterDao.FindParameters();

            //分拣订单业务数据接口服务器数据库类型
            if (parameter["SalesSystemDBType"] != "")
                dbTypeName = parameter["SalesSystemDBType"];
        }
        /// <summary>
        /// 营销系统区域表
        /// </summary>
        /// <returns></returns>
        public DataTable FindArea()
        {
            string sql = @"SELECT SALE_REG_CODE AS AREACODE,SALE_REG_NAME AS AREANAME,0 AS SORTID FROM DWV_ORG_SALE_REGION";
            if (dbTypeName == "DB2")
            {
                sql = @"SELECT AREACODE, AREANAME,0 AS SORTID FROM OUKANG.OUKANG_REGION";
            }            
            return ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 营销系统线路表
        /// </summary>
        /// <returns></returns>
        public DataTable FindRoute()
        {
            string sql = "SELECT DELIVER_LINE_CODE AS ROUTECODE, DELIVER_LINE_NAME AS ROUTENAME, '', DELIVER_LINE_ORDER AS SORTID, '' FROM DWV_CAR_DELIVER_LINE";
            if (dbTypeName == "DB2")
            {
                sql = @"SELECT ROUTECODE,ROUTENAME, AREACODE, SORTID FROM OUKANG.OUKANG_RUT";
            }
            return ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 营销系统客户表
        /// </summary>
        /// <returns></returns>
        public DataTable FindCustomer()
        {
            string sql = "SELECT CUST_CODE AS CUSTOMERCODE,CUST_NAME AS CUSTOMERNAME,DELIVER_LINE_CODE AS ROUTECODE, " +
                "SALE_REG_CODE AS AREACODE,LICENSE_CODE AS LICENSENO,DELIVER_ORDER AS SORTID, " +
                "PRINCIPAL_TEL AS TELNO,PRINCIPAL_ADDRESS AS ADDRESS FROM DWV_ORG_CUSTOMER";
            if (dbTypeName == "DB2")
            {
                sql = @"SELECT A.CUSTOMERCODE, A.CUSTOMERNAME, A.ROUTECODE, B.AREACODE AS AREACODE,A.LICENSENO,A.SORTID, A.TELNO,A.ADDRESS FROM OUKANG.OUKANG_CUST A LEFT JOIN OUKANG.OUKANG_RUT B ON A.ROUTECODE = B.ROUTECODE";

            }
            return ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 营销系统卷烟表
        /// </summary>
        /// <returns></returns>
        public DataTable FindCigarette()
        {
            string sql = @"SELECT BRAND_CODE AS CIGARETTECODE,BRAND_NAME AS CIGARETTENAME,
                                    IS_ABNORMITY_BRAND AS ISABNORMITY,RIGHT(BARCODE_PIECE,6) AS BARCODE
                                    FROM DWV_INF_BRAND";
            if (dbTypeName == "DB2")
            {
                sql = @"SELECT CIGARETTECODE, CIGARETTENAME,ISABNORMITY,RIGHT(ltrim(rtrim(BARCODE)),6)  BARCODE FROM OUKANG.OUKANG_ITEM";
            }
            return ExecuteQuery(sql).Tables[0];
        }


        /// <summary>
        /// 营销系统订单主表
        /// </summary>
        /// <param name="orderDate"></param>
        /// <param name="batchNo"></param>
        /// <param name="ExistAreaRoutes"></param>
        /// <returns></returns>
        public DataTable FindOrderMaster(DateTime orderDate, int batchNo, string existRoutes)
        {
            string sql = string.Format("SELECT '{0}', {1}, ORDER_ID AS ORDERID, CUST_CODE AS CUSTOMERCODE, DELIVER_LINE_CODE AS ROUTECODE, " +
                "SALE_REG_CODE AS AREACODE, DELIVER_ORDER AS SORTID FROM DWV_ORD_ORDER WHERE ORDER_DATE LIKE '{2}%' AND DELIVER_LINE_CODE NOT IN ({3})",
                orderDate, batchNo, orderDate.ToString("yyyyMMdd"), existRoutes);
            if (dbTypeName == "DB2")
            {
                sql = string.Format("SELECT '{0}', {1}, A.ORDERID AS ORDERID, A.CUSTOMERCODE AS CUSTOMERCODE, A.RUTCODE AS ROUTECODE,A.AREACODE AS AREACODE, B.SORTID AS SORTID FROM OUKANG.OUKANG_CO A LEFT JOIN OUKANG_CUST B on A.CUSTOMERCODE = B.CUSTOMERCODE WHERE A.ORDERDATE = '{2}' AND B.ROUTECODE NOT IN ({3}) GROUP BY ORDERID,A.CUSTOMERCODE, A.RUTCODE ,AREACODE,B.SORTID",
                    orderDate, batchNo, orderDate.ToString("yyyy-MM-dd"), existRoutes);
            }
            return ExecuteQuery(sql).Tables[0];
        }

        /// <summary> 
        /// 营销系统订单明细表
        /// </summary>
        /// <param name="orderDate"></param>
        /// <param name="batchNo"></param>
        /// <param name="ExistAreaRoutes"></param>
        /// <returns></returns>
        public DataTable FindOrderDetail(DateTime orderDate, string existRoutes)
        {
            string sql = string.Format("SELECT ORDER_ID AS ORDERID, BRAND_CODE AS CIGARETTECODE, BRAND_NAME AS CIGARETTENAME,QUANTITY,0,0,'{0}'  " +
                "FROM DWV_ORD_ORDER_DETAIL WHERE ORDER_ID IN (SELECT ORDER_ID FROM DWV_ORD_ORDER WHERE ORDER_DATE LIKE '{1}%' " +
                "AND DELIVER_LINE_CODE NOT IN ({2}))", orderDate, orderDate.ToString("yyyyMMdd"), existRoutes);
            if (dbTypeName == "DB2")
            {
                sql = string.Format("SELECT A.ORDERID AS ORDERID, A.CIGARETTECODE AS CIGARETTECODE, B.CIGARETTENAME AS CIGARETTENAME,A.QUANTITY,0,0,'{0}' FROM OUKANG.OUKANG_CO A LEFT JOIN OUKANG.OUKANG_ITEM B ON A.CIGARETTECODE = B.CIGARETTECODE LEFT JOIN OUKANG_CUST C on A.CUSTOMERCODE = C.CUSTOMERCODE WHERE ORDERDATE = '{1}' AND C.ROUTECODE NOT IN ({2})", orderDate, orderDate.ToString("yyyy-MM-dd"), existRoutes);
            }
            return ExecuteQuery(sql).Tables[0];
        }

        //public DataTable FindLineSchedule(DateTime orderDate, int batchNo, string existRoutes)
        //{
        //    string sql = string.Format("SELECT '{0}',{1},SORTING_CODE,B.DELIVER_LINE_CODE,0 FROM DWV_ORD_SORT_BILL A LEFT JOIN " +
        //        "DWV_ORD_DIST_BILL B ON A.SORT_BILL_ID = B.SORT_BILL_ID AND LEFT(SORT_DIST_DATE,8) = B.DIST_DATE " +
        //        "WHERE SORT_DIST_DATE LIKE '{2}%' AND b.DELIVER_LINE_CODE NOT IN ({3})" ,orderDate, batchNo, orderDate.ToString("yyyyMMdd"), existRoutes);
        //    return ExecuteQuery(sql).Tables[0];
        //}
    }
}
