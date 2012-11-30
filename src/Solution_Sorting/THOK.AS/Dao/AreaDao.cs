using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.AS.Dao
{
    public class AreaDao : BaseDao
    {
        /// <summary>
        /// �ֲ���ѯ�������������м�¼
        /// </summary>
        /// <param name="startRecord"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DataTable FindAll(int startRecord, int pageSize, string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);
            string sql = "SELECT * FROM AS_BI_AREA " + where;
            return ExecuteQuery(sql, "AS_BI_AREA", startRecord, pageSize).Tables[0];
        }


        /// <summary>
        /// ��ѯ����������¼����
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public int FindCount(string filter)
        {
            string where = " ";
            if (filter != null)
                where += (" WHERE " + filter);

            string sql = "SELECT COUNT(*) FROM AS_BI_AREA " + where;
            return (int)ExecuteScalar(sql);
        }

        

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="dataSet"></param>
        public void UpdateEntity(string sortID, string areaCode)
        {
            SqlCreate sqlCreate = new SqlCreate("AS_BI_AREA", SqlType.UPDATE);
            sqlCreate.Append("SORTID", sortID);
            sqlCreate.AppendWhereQuote("AREACODE", areaCode);
            ExecuteNonQuery(sqlCreate.GetSQL());
        }

        /// <summary>
        /// ���β���
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="tableName"></param>
        public void BatchInsertArea(DataTable dtData)
        {
            BatchInsert(dtData, "AS_BI_AREA");
        }

        public void Clear()
        {
            string sql = "TRUNCATE TABLE AS_BI_AREA";
            ExecuteNonQuery(sql);
        }

        public void SynchronizeArea(DataTable areaTable)
        {
            foreach (DataRow row in areaTable.Rows)
            {
                string sql = "IF '{0}' IN (SELECT AREACODE FROM AS_BI_AREA) " +
                                "BEGIN " +
                                    "UPDATE AS_BI_AREA SET AREANAME = '{1}' WHERE AREACODE = '{0}' " +
                                "END " +
                             "ELSE " +
                                "BEGIN " +
                                    "INSERT AS_BI_AREA VALUES ('{0}','{1}','{2}') " +
                                "END";
                sql = string.Format(sql, row["AREACODE"], row["AREANAME"], row["SORTID"]);
                ExecuteNonQuery(sql);
            }
        }
    }
}
