using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using THOK.Util;

namespace THOK.AS.Sorting.Dao
{
    public class UploadDao : BaseDao
    {
        #region Old

//        public void Update(System.Data.DataSet dst, string tableName)
//        {
//        }
//        public void SetBackUpFlag(string dateTime, string batchNo, string lineCode)
//        {
//        }
//        public void SetDownloadFlag(string dateTime, string batchNo, string lineCode)
//        {
//            using (PersistentManager persistentManager = new PersistentManager())
//            {
//                //Execute exec = (Execute)persistentManager.BuildDao("THOK.DataBackup.Execute");
//                string sql2 = "select ISGETM from AS_BI_BATCH where convert(varchar(10),orderDate,120) like '%" + dateTime + "%' and BATCHNO='" + batchNo + "'";
//                DataSet dst = ExecuteQuery(sql2, "ISGETM");

//                string strlineCode = dst.Tables["ISGETM"].Rows[0]["ISGETM"].ToString();
//                if (strlineCode == "0")
//                {
//                    strlineCode = "";
//                }
//                strlineCode += lineCode;

//                string strSQL = "UPDATE AS_BI_BATCH SET ISGETM='" + strlineCode + "' WHERE ORDERDATE='" + dateTime + "' AND BATCHNO='" + batchNo + "'";
//                try
//                {
//                    ExecuteNonQuery(strSQL);
//                }
//                catch (Exception ex)
//                {
//                    throw ex;
//                }
//            }
//        }



//        /// <summary>
//        /// 上传国家局
//        /// </summary>
//        /// <param name="orderid">订单ID</param>
//        /// <returns>true or false</returns>
//        public bool DataUpload(string orderid)
//        {
//            try
//            {
//                PalletMasterDao masterDao = new PalletMasterDao();
//                DataTable dt = masterDao.SumFromOrderId(orderid);
//                //string SORTNO = "";
//                string strOrderID = "";
//                string strSortType = "00";
//                string strOuterLineCode = "15";
//                string strCustomerCode = "";
//                string strStartTime = "";
//                string strFinishTime = "";
//                string strQuantity = "";
//                string strUpStatus = "1";
//                string strUpTime = DateTime.Now.ToString("yyyyMMddHHmmss");

//                string strInsert = "";
//                using (PersistentManager persistentManager = new PersistentManager("OuterConnection"))
//                {

//                    foreach (DataRow DR in dt.Rows)
//                    {
//                        //SORTNO = DR["SORTNO"].ToString().Trim();
//                        strOrderID = orderid;
//                        strCustomerCode = DR["CUSTOMERCODE"].ToString().Trim();
//                        strStartTime = strFinishTime = Convert.ToDateTime(DR["FINISHEDTIME"].ToString().Trim()).ToString("yyyyMMddHHmmss");
//                        strFinishTime = Convert.ToDateTime(DR["FINISHEDTIME"].ToString().Trim()).ToString("yyyyMMddHHmmss");
//                        strQuantity = DR["QUANTITY"].ToString().Trim();
//                        strInsert = @"insert into DWV_DPS_SORT_STATUS 
//                            select '{0}' as ORDER_ID,'{1}' as SORT_TYPE,'{2}' as SORTING_CODE,
//                                    A.DIST_BILL_ID,b.SORT_BILL_ID,
//                                    '{3}' as CUST_CODE,'{4}' as SORT_BEGIN_TIME,
//                                    '{5}' as SORT_END_TIME,{6} as BRAND_QUANTITY,
//                                    '{7}' as UP_STATUS,'{8}' as UPDATE_DATE 
//                            from DWV_ORD_ORDER A left join 
//                            DWV_ORD_DIST_BILL B on A.DIST_BILL_ID=B.DIST_BILL_ID
//                            where A.ORDER_ID='{0}'";
//                        strInsert = String.Format(strInsert, strOrderID, strSortType, strOuterLineCode, strCustomerCode, strStartTime, strFinishTime, strQuantity, strUpStatus, strUpTime);
//                        dao.UpData(strInsert);

//                    }
//                }
//                return true;
//            }
//            catch (Exception ee)
//            {
//                return false;
//            }
//        }




//        public bool SaveUpload(string orderID)
//        {
//            try
//            {
//                PalletMasterDao masterDao = new PalletMasterDao();
//                DataTable dt = masterDao.SumFromOrderId(orderID);
//                //string SORTNO = "";
//                string strOrderID = "";
//                string strSortType = "00";
//                string strOuterLineCode = "15";
//                string strCustomerCode = "";
//                string strStartTime = "";
//                string strFinishTime = "";
//                string strQuantity = "";
//                string strUpStatus = "1";
//                string strUpTime = DateTime.Now.ToString("yyyyMMddHHmmss");

//                string strInsert = "";
//                using (PersistentManager persistentManager = new PersistentManager("OuterConnection"))
//                {
//                    foreach (DataRow DR in dt.Rows)
//                    {
//                        //SORTNO = DR["SORTNO"].ToString().Trim();
//                        strOrderID = orderID;
//                        strCustomerCode = DR["CUSTOMERCODE"].ToString().Trim();
//                        strStartTime = strFinishTime = Convert.ToDateTime(DR["FINISHEDTIME"].ToString().Trim()).ToString("yyyyMMddHHmmss");
//                        strFinishTime = Convert.ToDateTime(DR["FINISHEDTIME"].ToString().Trim()).ToString("yyyyMMddHHmmss");
//                        strQuantity = DR["QUANTITY"].ToString().Trim();
//                        strInsert = @"select '{0}' as ORDER_ID,'{1}' as SORT_TYPE,'{2}' as SORTING_CODE,
//                                    A.DIST_BILL_ID,b.SORT_BILL_ID,
//                                    '{3}' as CUST_CODE,'{4}' as SORT_BEGIN_TIME,
//                                    '{5}' as SORT_END_TIME,{6} as BRAND_QUANTITY,
//                                    '{7}' as UP_STATUS,'{8}' as UPDATE_DATE 
//                            from DWV_ORD_ORDER A left join 
//                            DWV_ORD_DIST_BILL B on A.DIST_BILL_ID=B.DIST_BILL_ID
//                            where A.ORDER_ID='{0}'";
//                        strInsert = String.Format(strInsert, strOrderID, strSortType, strOuterLineCode, strCustomerCode, strStartTime, strFinishTime, strQuantity, strUpStatus, strUpTime);
//                        DownloadDao dao = new DownloadDao();
//                        dao.SetPersistentManager(persistentManager);
//                        DataTable table = dao.FindData(strInsert);
//                        UploadDao2 uploadDao2 = new UploadDao2();
//                        uploadDao2.Insert(table);
//                    }
//                }
//                return true;
//            }
//            catch (Exception ee)
//            {
//                THOK.MCP.Logger.Info("SaveUpload:" + ee.Message);
//                return false;
//            }
//        }

//        public void Insert(DataRow row)
//        {

//            THOK.Util.SqlCreate sqlCreate = new THOK.Util.SqlCreate("DWV_DPS_SORT_STATUS", THOK.Util.SqlType.INSERT);
//            sqlCreate.AppendQuote("ORDER_ID", row["ORDER_ID"]);
//            sqlCreate.AppendQuote("SORT_TYPE", row["SORT_TYPE"]);
//            sqlCreate.AppendQuote("SORTING_CODE", row["SORTING_CODE"]);
//            sqlCreate.AppendQuote("CUST_CODE", row["CUST_CODE"]);
//            sqlCreate.AppendQuote("DIST_BILL_ID", row["DIST_BILL_ID"]);
//            sqlCreate.AppendQuote("SORT_BILL_ID", row["SORT_BILL_ID"]);
//            sqlCreate.AppendQuote("SORT_BEGIN_TIME", row["SORT_BEGIN_TIME"]);
//            sqlCreate.AppendQuote("SORT_END_TIME", row["SORT_END_TIME"]);
//            sqlCreate.Append("BRAND_QUANTITY", row["BRAND_QUANTITY"]);
//            sqlCreate.AppendQuote("UP_STATUS", row["UP_STATUS"]);
//            sqlCreate.AppendQuote("UPDATE_DATE", row["UPDATE_DATE"]);
//            System.Diagnostics.Debug.WriteLine(sqlCreate.GetSQL());
//            ExecuteNonQuery(sqlCreate.GetSQL());
        //        }
        #endregion

        internal void UpData(string strInsert)
        {
            ExecuteNonQuery(strInsert);
        }
    }
}
