using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.AS.Dao
{
    public class SalesSystemDao : BaseDao
    {
        public string dbTypeName = "test";
        public SalesSystemDao()
        {
            THOK.AS.Dao.SysParameterDao parameterDao = new SysParameterDao();
            Dictionary<string, string> parameter = parameterDao.FindParameters();

            //�ּ𶩵�ҵ�����ݽӿڷ��������ݿ�����
            if (parameter["SalesSystemDBType"] != "")
                dbTypeName = parameter["SalesSystemDBType"];
        }
        /// <summary>
        /// Ӫ��ϵͳ�����
        /// </summary>
        /// <returns></returns>
        public DataTable FindArea()
        {
            string sql = @"SELECT SALE_REG_CODE AS AREACODE,SALE_REG_NAME AS AREANAME,0 AS SORTID FROM DWV_ORG_SALE_REGION";
            //if (dbTypeName == "DB2")
            //{
            //    sql = @"SELECT AREACODE, AREANAME,0 AS SORTID FROM OUKANG.OUKANG_REGION";
            //}
            return ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// Ӫ��ϵͳ��·��
        /// </summary>
        /// <returns></returns>
        public DataTable FindRoute()
        {
            string sql = "SELECT DELIVER_LINE_CODE AS ROUTECODE, DELIVER_LINE_NAME AS ROUTENAME,DIST_STA_CODE AS AREACODE, DELIVER_LINE_ORDER AS SORTID, '' FROM DWV_CAR_DELIVER_LINE";
            //if (dbTypeName == "DB2")
            //{
            //    sql = @"SELECT ROUTECODE,ROUTENAME, AREACODE, SORTID FROM OUKANG.OUKANG_RUT";
            //}
            return ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// Ӫ��ϵͳ�ͻ���
        /// </summary>
        /// <returns></returns>
        public DataTable FindCustomer()
        {
            string sql = "SELECT CUST_N AS CUSTOMERCODE,CUST_NAME AS CUSTOMERNAME,DELIVER_LINE_CODE AS ROUTECODE, " +
                "SALE_REG_CODE AS AREACODE,LICENSE_CODE AS LICENSENO,DELIVER_ORDER AS SORTID, " +
                "PRINCIPAL_TEL AS TELNO,PRINCIPAL_ADDRESS AS ADDRESS FROM DWV_ORG_CUSTOMER";
            //if (dbTypeName == "DB2")
            //{
            //    sql = @"SELECT A.CUSTOMERCODE, A.CUSTOMERNAME, A.ROUTECODE, B.AREACODE AS AREACODE,A.LICENSENO,A.SORTID, A.TELNO,A.ADDRESS FROM OUKANG.OUKANG_CUST A LEFT JOIN OUKANG.OUKANG_RUT B ON A.ROUTECODE = B.ROUTECODE";

            //}
            return ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        ///Ӫ��ϵͳ���α�
        /// </summary>
        /// <returns></returns>
        public DataTable FindBatch()
        {
            string sql = "SELECT * FROM AS_BI_BATCH WHERE ISFINISH=0";
            return ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// Ӫ��ϵͳ���̱�
        /// </summary>
        /// <returns></returns>
        public DataTable FindCigarette()
        {
            string sql = @"SELECT BRAND_CODE AS CIGARETTECODE,BRAND_NAME AS CIGARETTENAME,
                                    IS_ABNORMITY_BRAND AS ISABNORMITY,RIGHT(BARCODE_PIECE,6) AS BARCODE
                                    FROM DWV_INF_BRAND";
            //if (dbTypeName == "DB2")
            //{
            //    sql = @"SELECT CIGARETTECODE, CIGARETTENAME,ISABNORMITY,RIGHT(ltrim(rtrim(BARCODE)),6)  BARCODE FROM OUKANG.OUKANG_ITEM";
            //}
            return ExecuteQuery(sql).Tables[0];
        }


        /// <summary>
        /// Ӫ��ϵͳ��������
        /// </summary>
        /// <param name="orderDate"></param>
        /// <param name="batchNo"></param>
        /// <param name="ExistAreaRoutes"></param>
        /// <returns></returns>
        public DataTable FindOrderMaster(DateTime orderDate, int batchNo,string sortLineCode, string existRoutes)
        {
            string sql = string.Format(@"SELECT (values date(substr(ORDER_DATE,1,4) concat '-' concat substr(ORDER_DATE,5,2) concat '-' concat substr(ORDER_DATE,7,2))) AS 
                                ORDERDATE,{0} AS BATCHNO,ORDER_ID AS ORDERID, DWV_ORG_CUSTOMER.CUST_N AS CUSTOMERCODE,
                            DWV_ORD_ORDER.DELIVER_LINE_CODE 
                            AS ROUTECODE,DWV_ORD_ORDER.SALE_REG_CODE AS AREACODE,DWV_ORD_ORDER.DELIVER_ORDER AS SORTID 
                            FROM DWV_ORD_ORDER
                                    LEFT JOIN DWV_ORG_CUSTOMER ON DWV_ORD_ORDER.CUST_CODE=DWV_ORG_CUSTOMER.CUST_CODE
                                    WHERE Dist_BILL_ID IN(SELECT Dist_BILL_ID FROM DWV_ORD_DIST_BILL
                                    WHERE SORT_BILL_ID IN(SELECT SORT_BILL_ID FROM DWV_ORD_SORT_BILL 
                                    WHERE SORT_DIST_DATE LIKE '{1}%' AND SORTING_CODE='{2}')) AND DWV_ORD_ORDER.DELIVER_LINE_CODE NOT IN ({3})"
              , batchNo, orderDate.ToString("yyyyMMdd"), sortLineCode, existRoutes);
//            string sql = string.Format(@"SELECT DISTINCT '{0}', {1}, A.ORDER_ID AS ORDERID, B.CUST_N AS CUSTOMERCODE, A.DELIVER_LINE_CODE AS ROUTECODE,
//                        A.SALE_REG_CODE AS AREACODE, A.DELIVER_ORDER AS SORTID FROM DWV_ORD_ORDER A LEFT JOIN DWV_ORG_CUSTOMER B ON A.CUST_CODE=B.CUST_CODE 
//                 WHERE A.ORDER_DATE LIKE '{2}%' AND A.SORTING_CODE={3} AND A.DELIVER_LINE_CODE NOT IN ({4})",
//                orderDate, batchNo, orderDate.ToString("yyyyMMdd"), sortLineCode, existRoutes);
            //if (dbTypeName == "DB2")
            //{
            //    sql = string.Format("SELECT '{0}', {1}, A.ORDERID AS ORDERID, A.CUSTOMERCODE AS CUSTOMERCODE, A.RUTCODE AS ROUTECODE,A.AREACODE AS AREACODE, B.SORTID AS SORTID FROM OUKANG.OUKANG_CO A LEFT JOIN OUKANG_CUST B on A.CUSTOMERCODE = B.CUSTOMERCODE WHERE A.ORDERDATE = '{2}' AND B.ROUTECODE NOT IN ({3}) GROUP BY ORDERID,A.CUSTOMERCODE, A.RUTCODE ,AREACODE,B.SORTID",
            //        orderDate, batchNo, orderDate.ToString("yyyy-MM-dd"),sortLineCode, existRoutes);
            //}
            return ExecuteQuery(sql).Tables[0];
        }

        /// <summary> 
        /// Ӫ��ϵͳ������ϸ��
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
            //if (dbTypeName == "DB2")
            //{
            //    sql = string.Format("SELECT A.ORDERID AS ORDERID, A.CIGARETTECODE AS CIGARETTECODE, B.CIGARETTENAME AS CIGARETTENAME,A.QUANTITY,0,0,'{0}' FROM OUKANG.OUKANG_CO A LEFT JOIN OUKANG.OUKANG_ITEM B ON A.CIGARETTECODE = B.CIGARETTECODE LEFT JOIN OUKANG_CUST C on A.CUSTOMERCODE = C.CUSTOMERCODE WHERE ORDERDATE = '{1}' AND C.ROUTECODE NOT IN ({2})", orderDate, orderDate.ToString("yyyy-MM-dd"), existRoutes);
            //}
            return ExecuteQuery(sql).Tables[0];
        }

        //public DataTable FindLineSchedule(DateTime orderDate, int batchNo, string existRoutes)
        //{
        //    string sql = string.Format("SELECT '{0}'AS ORDERDATE,{1} AS BATCHNO,SORTING_CODE,B.DELIVER_LINE_CODE,0 FROM DWV_ORD_SORT_BILL A LEFT JOIN " +
        //        "DWV_ORD_DIST_BILL B ON A.SORT_BILL_ID = B.SORT_BILL_ID AND LEFT(SORT_DIST_DATE,8) = B.DIST_DATE " +
        //        "WHERE SORT_DIST_DATE LIKE '{2}%' AND b.DELIVER_LINE_CODE NOT IN ({3})", orderDate, batchNo, orderDate.ToString("yyyyMMdd"), existRoutes);
        //    return ExecuteQuery(sql).Tables[0];
        //}
    }
}
