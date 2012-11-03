using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using THOK.Util;

namespace THOK.AS.Sorting.Dao
{
    public class ChannelDao: BaseDao
    {
        public DataTable FindChannel()
        {
            string sql = "SELECT CHANNELCODE, CHANNELNAME, CASE CHANNELTYPE WHEN '2' THEN '立式机' WHEN '5' THEN '立式机' ELSE '通道机' END CHANNELTYPE, " +
                "LINECODE, CIGARETTECODE, CIGARETTENAME, QUANTITY FROM AS_SC_CHANNELUSED ORDER BY CHANNELNAME";
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindChannelQuantity(string sortNo)
        {
            string sql = string.Format("SELECT *, REMAINQUANTITY / 50 BOXQUANTITY, REMAINQUANTITY % 50 ITEMQUANTITY FROM ( " +
                "SELECT A.CHANNELNAME, A.LEDGROUP, A.LEDNO, CASE CHANNELTYPE WHEN '2' THEN '立式机' WHEN '5' THEN '立式机' ELSE '通道机' END CHANNELTYPE, " +
                "A.CIGARETTECODE, A.CIGARETTENAME, A.QUANTITY, CASE WHEN B.QUANTITY IS NULL THEN 0 ELSE B.QUANTITY END SORTQUANTITY, " +
                "A.QUANTITY -CASE WHEN B.QUANTITY IS NULL THEN 0 ELSE B.QUANTITY END REMAINQUANTITY,A.LED_X,A.LED_Y,A.LED_WIDTH,A.LED_HEIGHT,A.CHANNELADDRESS FROM AS_SC_CHANNELUSED A " +
                "LEFT JOIN(SELECT CHANNELCODE, SUM(QUANTITY) QUANTITY FROM AS_SC_ORDER WHERE SORTNO <= '{0}' GROUP BY CHANNELCODE) B " +
                "ON A.CHANNELCODE = B.CHANNELCODE) C ORDER BY CHANNELNAME", sortNo);
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindEmptyChannel()
        {
            string sql = "SELECT CHANNELCODE, CHANNELNAME + ' ' + CASE CHANNELTYPE WHEN '2' THEN '立式机'   WHEN '5' THEN '立式机'  ELSE '通道机' END CHANNELNAME FROM AS_SC_CHANNELUSED WHERE QUANTITY=0 AND CHANNELTYPE IN ('2','3') ORDER BY CHANNELNAME";
            return ExecuteQuery(sql).Tables[0];
        }

        public DataTable FindChannel(string channelCode)
        {
            string sql = string.Format("SELECT * FROM AS_SC_CHANNELUSED WHERE CHANNELCODE='{0}'", channelCode);
            return ExecuteQuery(sql).Tables[0];
        }

        public string FindChanneCigarette(string channelCode)
        {
            string sql = string.Format("SELECT CIGARETTENAME FROM AS_SC_CHANNELUSED WHERE CHANNELCODE='{0}'", channelCode);
            return ExecuteScalar(sql).ToString();
        }

        public DataTable FindLastSortNo()
        {
            string sql = string.Format("SELECT CHANNELADDRESS,CHANNELCODE,SORTNO FROM AS_SC_CHANNELUSED ORDER BY CHANNELTYPE,CHANNELCODE");
            return ExecuteQuery(sql).Tables[0];
        }

        public void UpdateChannel(string channelCode, string cigaretteCode, string cigaretteName, int quantity, string sortNo)
        {
            string sql = string.Format("UPDATE AS_SC_CHANNELUSED SET CIGARETTECODE='{0}', CIGARETTENAME='{1}', QUANTITY={2}, SORTNO={3} WHERE CHANNELCODE='{4}'",
                cigaretteCode, cigaretteName, quantity, sortNo, channelCode);
            ExecuteNonQuery(sql);
        }

        public void InsertChannel(DataTable channelTable)
        {
            ExecuteQuery("TRUNCATE TABLE AS_SORTSTATUS");
            ExecuteQuery("TRUNCATE TABLE AS_SC_CHANNELUSED");
            BatchInsert(channelTable, "AS_SC_CHANNELUSED");
        }
    }
}
