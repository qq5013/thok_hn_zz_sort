using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.AS.Sorting.Dao;
using THOK.Util;

namespace THOK.AS.Sorting.Dal
{
    public class UploadDal
    {
        /// <summary>
        /// 上传国家局
        /// </summary>
        /// <param name="orderid">订单ID</param>
        /// <returns>true or false</returns>
        public bool DataUpload(string orderID)
        {
            try
            {
                OrderDao orderDao = new OrderDao();
                ParamDao paramDao = new ParamDao();
                DataTable dt = orderDao.SumFromOrderId(orderID);
                //string SORTNO = "";
                string strOrderID = "";
                string strSortType = "00";
                string strOuterLineCode = paramDao.FindState("OuterLineCode");
                string strCustomerCode = "";
                string strStartTime = "";
                string strFinishTime = "";
                string strQuantity = "";
                string strUpStatus = "1";
                string strUpTime = DateTime.Now.ToString("yyyyMMddHHmmss");

                string strInsert = "";
                using (PersistentManager persistentManager = new PersistentManager("OuterConnection"))
                {
                    UploadDao uploadDao = new UploadDao();
                    uploadDao.SetPersistentManager(persistentManager);
                    foreach (DataRow DR in dt.Rows)
                    {
                        //SORTNO = DR["SORTNO"].ToString().Trim();
                        strOrderID = orderID;
                        strCustomerCode = DR["CUSTOMERCODE"].ToString().Trim();
                        strStartTime = DateTime.Now.ToString("yyyyMMddHHmmss");
  
                        strFinishTime = DateTime.Now.ToString("yyyyMMddHHmmss"); 
                        strQuantity = DR["QUANTITY"].ToString().Trim();
                        strInsert = @"insert into DWV_DPS_SORT_STATUS 
                            select '{0}' as ORDER_ID,'{1}' as SORT_TYPE,'{2}' as SORTING_CODE,
                                    A.DIST_BILL_ID,b.SORT_BILL_ID,
                                    '{3}' as CUST_CODE,'{4}' as SORT_BEGIN_TIME,
                                    '{5}' as SORT_END_TIME,{6} as BRAND_QUANTITY,
                                    '{7}' as UP_STATUS,'{8}' as UPDATE_DATE 
                            from DWV_ORD_ORDER A left join 
                            DWV_ORD_DIST_BILL B on A.DIST_BILL_ID=B.DIST_BILL_ID
                            where A.ORDER_ID='{0}'";
                        strInsert = String.Format(strInsert, strOrderID, strSortType, strOuterLineCode, strCustomerCode, strStartTime, strFinishTime, strQuantity, strUpStatus, strUpTime);
                        uploadDao.UpData(strInsert);

                    }
                }
                return true;
            }
            catch (Exception ee)
            {
                THOK.MCP.Logger.Info("DataUpload:" + ee.Message);
                return false;
            }
        }

        public bool SaveUpload(string orderID)
        {
            try
            {
                OrderDao orderDao = new OrderDao();
                ParamDao paramDao = new ParamDao();
                DataTable dt = orderDao.SumFromOrderId(orderID);
                //string SORTNO = "";
                string strOrderID = "";
                string strSortType = "00";
                string strOuterLineCode = paramDao.FindState("OuterLineCode");
                string strCustomerCode = "";
                string strStartTime = "";
                string strFinishTime = "";
                string strQuantity = "";
                string strUpStatus = "1";
                string strUpTime = DateTime.Now.ToString("yyyyMMddHHmmss");

                string strInsert = "";
                using (PersistentManager persistentManager = new PersistentManager("OuterConnection"))
                {
                    UploadDao dao = new UploadDao();
                    dao.SetPersistentManager(persistentManager);
                    foreach (DataRow DR in dt.Rows)
                    {
                        //SORTNO = DR["SORTNO"].ToString().Trim();
                        strOrderID = orderID;
                        strCustomerCode = DR["CUSTOMERCODE"].ToString().Trim();
                        strStartTime  = DateTime.Now.ToString("yyyyMMddHHmmss");
                        strFinishTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        strQuantity = DR["QUANTITY"].ToString().Trim();
                        strInsert = @"select '{0}' as ORDER_ID,'{1}' as SORT_TYPE,'{2}' as SORTING_CODE,
                                    A.DIST_BILL_ID,b.SORT_BILL_ID,
                                    '{3}' as CUST_CODE,'{4}' as SORT_BEGIN_TIME,
                                    '{5}' as SORT_END_TIME,{6} as BRAND_QUANTITY,
                                    '{7}' as UP_STATUS,'{8}' as UPDATE_DATE 
                            from DWV_ORD_ORDER A left join 
                            DWV_ORD_DIST_BILL B on A.DIST_BILL_ID=B.DIST_BILL_ID
                            where A.ORDER_ID='{0}'";
                        strInsert = String.Format(strInsert, strOrderID, strSortType, strOuterLineCode, strCustomerCode, strStartTime, strFinishTime, strQuantity, strUpStatus, strUpTime);
                        DataTable table = dao.FindData(strInsert);
                        UploadDao uploadDao = new UploadDao();
                        uploadDao.Insert(table);
                    }
                }
                return true;
            }
            catch (Exception ee)
            {
                THOK.MCP.Logger.Info("SaveUpload:" + ee.Message);
                return false;
            }
        }
    }
}
