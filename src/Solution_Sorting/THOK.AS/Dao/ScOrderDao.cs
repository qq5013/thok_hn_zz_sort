using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using THOK.Util;

namespace THOK.AS.Dao
{
    public class ScOrderDao : BaseDao
    {
        public DataTable FindHandleSupplyOrder(string orderDate, int batchNo, string lineCode)
        {
            string sql = string.Format("SELECT * FROM AS_SC_ORDER WHERE CHANNELCODE='-1' AND ORDERDATE='{0}' AND BATCHNO='{1}' AND LINECODE='{2}' ORDER BY SORTNO ASC,QUANTITY DESC", orderDate, batchNo, lineCode);
            return ExecuteQuery(sql).Tables[0];
        }
        
        public void DeleteOldSupplyOrders(string orderDate, int batchNo,string lineCode)
        {
            string sql = string.Format("DELETE FROM AS_SC_ORDER WHERE CHANNELCODE='-1' AND ORDERDATE='{0}' AND BATCHNO='{1}' AND LINECODE='{2}' ", orderDate, batchNo,lineCode);
            ExecuteNonQuery(sql);
        }
        public void InsertNewSupplyOrders(DataTable newSupplyOrders)
        {
            foreach (DataRow dataRow in newSupplyOrders.Rows)
            {
                SqlCreate sqlCreate = new SqlCreate("AS_SC_ORDER", SqlType.INSERT);
                sqlCreate.Append("SORTNO", dataRow["SORTNO"]);
                sqlCreate.AppendQuote("LINECODE", dataRow["LINECODE"]);

                sqlCreate.AppendQuote("ORDERDATE", dataRow["ORDERDATE"]);
                sqlCreate.AppendQuote("BATCHNO", dataRow["BATCHNO"]);
                sqlCreate.AppendQuote("ORDERID", dataRow["ORDERID"]);

                sqlCreate.AppendQuote("CIGARETTECODE", dataRow["CIGARETTECODE"]);
                sqlCreate.AppendQuote("CIGARETTENAME", dataRow["CIGARETTENAME"]);

                sqlCreate.AppendQuote("CHANNELCODE", dataRow["CHANNELCODE"]);
                sqlCreate.Append("QUANTITY", dataRow["QUANTITY"]);
                ExecuteNonQuery(sqlCreate.GetSQL());
            }
        }
        public void InsertHandSupplyOrders(DataTable newSupplyOrders)
        {
            foreach (DataRow dataRow in newSupplyOrders.Rows)
            {
                SqlCreate sqlCreate = new SqlCreate("AS_SC_HANDLESUPPLY", SqlType.INSERT);
                sqlCreate.Append("SORTNO", dataRow["SORTNO"]);
                sqlCreate.AppendQuote("LINECODE", dataRow["LINECODE"]);
                sqlCreate.AppendQuote("BATCHNO", dataRow["BATCHNO"]);
                sqlCreate.AppendQuote("SUPPLYBATCH", dataRow["SUPPLYBATCH"]);
                sqlCreate.AppendQuote("ORDERDATE", dataRow["ORDERDATE"]);
                sqlCreate.AppendQuote("ORDERID", dataRow["ORDERID"]);
                sqlCreate.AppendQuote("CIGARETTECODE", dataRow["CIGARETTECODE"]);
                sqlCreate.AppendQuote("CIGARETTENAME", dataRow["CIGARETTENAME"]);
                sqlCreate.AppendQuote("CHANNELCODE", dataRow["CHANNELCODE"]);
                sqlCreate.Append("QUANTITY", dataRow["QUANTITY"]);
                sqlCreate.AppendQuote("STATUS", "0");
                ExecuteNonQuery(sqlCreate.GetSQL());                
            }
        }

        public void UpdateMixChannelQuantity(string orderDate,int batchNo,string lineCode)
        {
            string sql = string.Format(@"UPDATE AS_SC_CHANNELUSED 
                                             SET QUANTITY=(SELECT ISNULL(SUM(QUANTITY),0) FROM AS_SC_HANDLESUPPLY WHERE CHANNELCODE=2058 AND ORDERDATE='{0}' AND BATCHNO={1} AND LINECODE='{2}')
                                             WHERE CHANNELCODE=2058 AND CHANNELTYPE=5 AND ORDERDATE='{0}' AND BATCHNO={1} AND LINECODE='{2}'
                                         UPDATE AS_SC_CHANNELUSED 
                                             SET QUANTITY=(SELECT ISNULL(SUM(QUANTITY),0) FROM AS_SC_HANDLESUPPLY WHERE CHANNELCODE=2059 AND ORDERDATE='{0}' AND BATCHNO={1} AND LINECODE='{2}')
                                             WHERE CHANNELCODE=2059 AND CHANNELTYPE=5 AND ORDERDATE='{0}' AND BATCHNO={1} AND LINECODE='{2}'
                                         UPDATE AS_SC_CHANNELUSED 
                                             SET QUANTITY=(SELECT ISNULL(SUM(QUANTITY),0) FROM AS_SC_HANDLESUPPLY WHERE CHANNELCODE=2060 AND ORDERDATE='{0}' AND BATCHNO={1} AND LINECODE='{2}')
                                             WHERE CHANNELCODE=2060 AND CHANNELTYPE=5 AND ORDERDATE='{0}' AND BATCHNO={1} AND LINECODE='{2}'", orderDate, batchNo, lineCode);
            ExecuteNonQuery(sql);
        }
    }
}
