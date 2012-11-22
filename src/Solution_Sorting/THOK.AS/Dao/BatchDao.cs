using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;
namespace THOK.AS.Dao
{
    public class BatchDao : BaseDao
    {
        public DataTable FindAll(int startRecord, int pageSize, string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);
            String sql = "SELECT * FROM AS_BI_BATCH" + where + "ORDER BY ORDERDATE DESC,BATCHNO DESC";
            return ExecuteQuery(sql, "AS_BI_BATCH", startRecord, pageSize).Tables[0];
        }

        public int FindCount(string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);

            string sql = "SELECT COUNT(*) FROM AS_BI_BATCH " + where;
            return (int)ExecuteScalar(sql);
        }

        public DataTable FindBatch(string orderDate)
        {
            string sql = string.Format("SELECT * FROM AS_BI_BATCH WHERE ORDERDATE='{0}' ORDER BY BATCHNO", orderDate);
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindBatch(string orderDate, int batchNo)
        {
            string sql = string.Format("SELECT * FROM AS_BI_BATCH WHERE ORDERDATE='{0}' AND BATCHNO={1}", orderDate, batchNo);
            return ExecuteQuery(sql).Tables[0];
        }

        public bool BatchNoExists(string orderDate, int batchNo)
        {
            string sql = string.Format("SELECT COUNT(*) FROM AS_BI_BATCH WHERE ORDERDATE='{0}' AND BATCHNO={1}", orderDate, batchNo);
            return Convert.ToBoolean(ExecuteScalar(sql));
        }

        public void DeleteHistory(string orderDate)
        {
            string sql = string.Format("DELETE FROM AS_BI_BATCH WHERE ORDERDATE < '{0}'", orderDate);
            
            ExecuteNonQuery(sql);
        }

        public void InsertEntity(string orderDate, int batchNo)
        {
            DateTime SCDATE = DateTime.Parse(orderDate);

            SqlCreate sqlCreate = new SqlCreate("AS_BI_BATCH", SqlType.INSERT);
            sqlCreate.Append("BATCHNO", batchNo);
            sqlCreate.Append("BATCHNO_ONEPRO", batchNo);
            sqlCreate.AppendQuote("BATCHNAME", string.Format("{0}第{1}批次", orderDate, batchNo));
            sqlCreate.AppendQuote("ORDERDATE", orderDate);
            sqlCreate.AppendQuote("ISVALID", 0);
            sqlCreate.AppendQuote("EXECUTEUSER", 0);
            sqlCreate.AppendQuote("EXECUTEIP", 0);
            sqlCreate.AppendQuote("ISUPTONOONEPRO", 0);
            sqlCreate.AppendQuote("SCDATE", orderDate);
            //sqlCreate.AppendQuote("SCDATE", SCDATE.AddDays(+2).ToShortDateString());
            ExecuteNonQuery(sqlCreate.GetSQL());
        }

        public void UpdateExecuter(string user, string ip, string orderDate, int batchNo)
        {
            string sql = string.Format("UPDATE AS_BI_BATCH SET EXECUTEUSER='{0}',EXECUTEIP='{1}' " +
                "WHERE ORDERDATE='{2}' AND BATCHNO={3}", user, ip, orderDate, batchNo);
            ExecuteNonQuery(sql);
        }

        public void UpdateNoOnePro(string orderDate, int batchNo, string user)
        {
            string sql = string.Format("UPDATE AS_BI_BATCH SET ISUPTONOONEPRO='1',SENDNOONEUSER='{0}' " +
                "WHERE ORDERDATE='{1}' AND BATCHNO={2}", user, orderDate, batchNo);
            ExecuteNonQuery(sql);
        }

        public void UpdateIsValid(string orderDate, int batchNo, string status)
        {
            string sql = string.Format("UPDATE AS_BI_BATCH SET ISVALID='{0}' WHERE ORDERDATE='{1}' AND BATCHNO={2}", status, orderDate, batchNo);
            ExecuteNonQuery(sql);
        }


        public DataTable IsExistBatch()
        {
            string sql = "SELECT PARAMETERVALUE FROM AS_SYS_PARAMETER WHERE PARAMETERNAME ='OuterBatch'";
            return ExecuteQuery(sql).Tables[0];
        }

        /// <summary>
        /// 批次插入
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="tableName"></param>
        public void BatchInsertBatch(DataTable dtData)
        {
            BatchInsert(dtData, "AS_BI_BATCH");
        }

        public void UpdateEntity(string orderDate, string sortBatch, string no1Batch, string no1UpLoadState)
        {
            string sql = "UPDATE AS_BI_BATCH SET BATCHNO_ONEPRO = {0},ISUPTONOONEPRO='{3}' WHERE ORDERDATE='{1}' AND BATCHNO='{2}'";
            ExecuteNonQuery(string.Format(sql, no1Batch, orderDate, sortBatch, no1UpLoadState));
        }
    }
}
