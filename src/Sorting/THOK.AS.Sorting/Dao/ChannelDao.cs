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

        //获取单个立式机烟道信息
        public DataTable FindChannelQuantity(string ledGroup,string sortNo)
        {
            string sql = string.Format(@"SELECT *, REMAINQUANTITY / 50 BOXQUANTITY, REMAINQUANTITY % 50 ITEMQUANTITY FROM (
                                        SELECT A.CHANNELNAME, A.LEDGROUP, A.LEDNO, CASE CHANNELTYPE WHEN '2' THEN '立式机' WHEN '5' THEN '立式机' ELSE '通道机' END CHANNELTYPE,
                                        A.CIGARETTECODE, A.CIGARETTENAME, A.QUANTITY, CASE WHEN B.QUANTITY IS NULL THEN 0 ELSE B.QUANTITY END SORTQUANTITY,
                                        A.QUANTITY -CASE WHEN B.QUANTITY IS NULL THEN 0 ELSE B.QUANTITY END REMAINQUANTITY,A.LED_X,A.LED_Y,A.LED_WIDTH,A.LED_HEIGHT,A.CHANNELADDRESS FROM AS_SC_CHANNELUSED A
                                        LEFT JOIN(SELECT CHANNELCODE, SUM(QUANTITY) QUANTITY FROM AS_SC_ORDER WHERE SORTNO <= '{0}' GROUP BY CHANNELCODE) B
                                        ON A.CHANNELCODE = B.CHANNELCODE) C WHERE LEDGROUP='{1}' ORDER BY CHANNELNAME", sortNo,ledGroup);
            return ExecuteQuery(sql).Tables[0];
        }

        //获取全部通道机或者立式机烟道信息
        public DataTable FindChannelQuantity(string deviceClass, string sortNo, string nulltype)
        {
            string sql = string.Format(@"SELECT *, REMAINQUANTITY / 50 BOXQUANTITY, REMAINQUANTITY % 50 ITEMQUANTITY FROM (
                                        SELECT A.CHANNELNAME, A.LEDGROUP, A.LEDNO, CASE CHANNELTYPE WHEN '2' THEN '立式机' WHEN '5' THEN '立式机' ELSE '通道机' END CHANNELTYPE,
                                        A.CIGARETTECODE, A.CIGARETTENAME, A.QUANTITY, CASE WHEN B.QUANTITY IS NULL THEN 0 ELSE B.QUANTITY END SORTQUANTITY,
                                        A.QUANTITY -CASE WHEN B.QUANTITY IS NULL THEN 0 ELSE B.QUANTITY END REMAINQUANTITY,A.LED_X,A.LED_Y,A.LED_WIDTH,A.LED_HEIGHT,A.CHANNELADDRESS FROM AS_SC_CHANNELUSED A
                                        LEFT JOIN(SELECT CHANNELCODE, SUM(QUANTITY) QUANTITY FROM AS_SC_ORDER WHERE SORTNO <= '{0}' GROUP BY CHANNELCODE) B
                                        ON A.CHANNELCODE = B.CHANNELCODE) C WHERE CHANNELTYPE='{1}' ORDER BY CHANNELNAME", sortNo, deviceClass);
            return ExecuteQuery(sql).Tables[0];
        }

        //立式机烟道信息
        public DataTable FindChannelInfo()
        {
            string strSql = "SELECT DISTINCT A.CHANNELCODE,A.CHANNELTYPE, C.CIGARETTENAME,A.LEDGROUP,A.LEDNO,SUM(C.QUANTITY) as QUANTITY FROM "
                + "AS_SC_ORDER C left join AS_SC_PALLETMASTER B on B.SORTNO=C.SORTNO "
                + " left join AS_SC_CHANNELUSED A on C.CHANNELCODE=A.CHANNELCODE"
                + " where A.CHANNELTYPE in ('2','5')"
                + "GROUP BY A.CHANNELCODE,A.CHANNELTYPE,C.CIGARETTENAME,A.LEDGROUP,A.LEDNO ORDER BY A.LEDGROUP asc,A.LEDNO asc";
            return ExecuteQuery(strSql).Tables[0];
        }

        //汇总烟道信息，FQUANTITY=0,立机式
        public DataTable FindChannelInfos()
        {
            string strSql = "SELECT A.CHANNELCODE,A.CHANNELTYPE,A.QUANTITY,0 AS FQUANTITY,CASE WHEN B.QUANTITY IS NULL THEN 0 ELSE B.QUANTITY END UQUANTITY,A.CIGARETTENAME,A.LEDGROUP,A.LEDNO FROM ( "
                + "SELECT CU.CHANNELCODE,CU.CHANNELTYPE,CU.CIGARETTENAME,CU.QUANTITY,CU.LEDGROUP,CU.LEDNO FROM AS_SC_CHANNELUSED CU  "
                + "LEFT JOIN AS_SC_ORDER OD ON CU.CHANNELCODE=OD.CHANNELCODE "
                + "WHERE CU.CHANNELTYPE IN ('2','5') "
                + "GROUP BY CU.CHANNELCODE,CU.CIGARETTENAME,CU.CHANNELTYPE,CU.QUANTITY,CU.LEDGROUP,CU.LEDNO ) AS A "
                + "LEFT JOIN "
                + "(SELECT TOP 70 OD.CHANNELCODE,SUM(OD.QUANTITY) QUANTITY FROM AS_SC_PALLETMASTER PM LEFT JOIN AS_SC_ORDER OD "
                + "ON OD.SORTNO=PM.SORTNO "
                + "WHERE PM.STATUS='0' GROUP BY CHANNELCODE ORDER BY CHANNELCODE ) B "
                + "ON A.CHANNELCODE=B.CHANNELCODE ";
            return ExecuteQuery(strSql).Tables[0];
        }

        public DataTable FindEmptyChannel()
        {
            string sql = "SELECT CHANNELCODE, CHANNELNAME + ' ' + CASE CHANNELTYPE WHEN '2' THEN '立式机'   WHEN '5' THEN '立式机'  ELSE '通道机' END CHANNELNAME FROM AS_SC_CHANNELUSED WHERE CHANNELTYPE IN ('2','3') ORDER BY CHANNELNAME";
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
