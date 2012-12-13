using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.AS.Sorting.Dao
{
    public class ParamDao : BaseDao
    {
        public string FindState(string key)
        {
            string sql = string.Format("SELECT MIN(PARAMVALUE) FROM AS_SYS_PARAM WHERE PARAMNAME='{0}'", key);
            return ExecuteScalar(sql).ToString();
        }

        public void Update(string key, string value)
        {
            string sql = string.Format("UPDATE AS_SYS_PARAM SET PARAMVALUE = '{0}' WHERE PARAMNAME = '{1}'", value, key);
            ExecuteNonQuery(sql);
        }
    }
}
