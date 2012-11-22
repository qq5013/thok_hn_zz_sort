using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.Odd.Dao
{
    internal class CigaretteDao: BaseDao
    {
        public DataTable Find()
        {
            string sql = "SELECT * FROM CIGARETTE ORDER BY CIGARETTENAME";
            return ExecuteQuery(sql).Tables[0];
        }

        public void Insert(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                string sql = "IF '{0}' IN (SELECT CIGARETTECODE FROM CIGARETTE) " +
                                "BEGIN " +
                                    "IF '{1}' =  '1' " +
                                    "BEGIN " +
                                        "UPDATE CIGARETTE SET ISABNORMITY = {1} WHERE CIGARETTECODE = '{0}' " +
                                    "END " +
                                "END " +
                             "ELSE " +
                                "BEGIN " +
                                    "INSERT CIGARETTE VALUES ('{0}','{2}',{1}) " +
                                "END";
                sql = string.Format(sql, row["CIGARETTECODE"], row["ISABNORMITY"].ToString() == "1" ? "1" : "0", row["CIGARETTENAME"]);
                ExecuteNonQuery(sql);
            }
        }

        public void Update(DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                SqlCreate sqlCreate = new SqlCreate("CIGARETTE", SqlType.UPDATE);
                sqlCreate.AppendQuote("ISABNORMITY", row["ISABNORMITY"]);
                sqlCreate.AppendWhereQuote("CIGARETTECODE", row["CIGARETTECODE"]);
                ExecuteNonQuery(sqlCreate.GetSQL());
            }
        }

        public void Update(string code, string isAbnormity)
        {
            SqlCreate sqlCreate = new SqlCreate("CIGARETTE", SqlType.UPDATE);
            sqlCreate.AppendQuote("ISABNORMITY", isAbnormity);
            sqlCreate.AppendWhereQuote("CIGARETTECODE", code);
            ExecuteNonQuery(sqlCreate.GetSQL()); 
        }
    }
}
