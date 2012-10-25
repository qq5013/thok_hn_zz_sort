using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace THOK.Optimize
{
    public class StockOptimize
    {
        public DataTable Optimize(DataTable channelTable, DataTable orderTable, String orderDate, int batchNo)
        {
            DataTable mixTable = GetMixTable();
            foreach (DataRow orderRow in orderTable.Rows)
            {
                DataRow[] channelRows = channelTable.Select(string.Format("CHANNELTYPE <> '{0}' AND CIGARETTECODE = '{1}'  AND STATUS='1'", "3", orderRow["CIGARETTECODE"]), "CHANNELCODE");
                if (channelRows.Length == 0 )
                {
                    channelRows = channelTable.Select("CHANNELTYPE <> '3' AND LEN(TRIM(CIGARETTECODE)) = 0", "ORDERNO");
                    if (channelRows.Length != 0)//有未被占用的非混合烟道
                    {

                        channelRows[0]["CIGARETTECODE"] = orderRow["CIGARETTECODE"];
                        channelRows[0]["CIGARETTENAME"] = orderRow["CIGARETTENAME"];
                    }
                    else//占用混合烟道
                    {
                        channelRows = channelTable.Select("CHANNELTYPE = '3'", "QUANTITY ASC");
                        if (channelRows.Length != 0)
                        {
                            mixTable.Rows.Add(new object[] { orderDate, batchNo, channelRows[0]["CHANNELCODE"], orderRow["CIGARETTECODE"], orderRow["CIGARETTENAME"] });
                            channelRows[0]["QUANTITY"] = Convert.ToInt32(channelRows[0]["QUANTITY"]) + Convert.ToInt32(orderRow["QUANTITY"]);
                        }
                    }
                }
            }
            return mixTable;
        }

        //public DataTable GetStockTable()
        //{
        //    DataTable table = new DataTable("STOCK");
        //    table.Columns.Add("ORDERDATE");
        //    table.Columns.Add("BATCHNO");
        //    table.Columns.Add("CHANNELCODE");
        //    table.Columns.Add("CHANNELNAME");
        //    table.Columns.Add("CHANNELTYPE");
        //    table.Columns.Add("ORDERNO");
        //    table.Columns.Add("CIGARETTECODE");
        //    table.Columns.Add("CIGARETTENAME");
        //    table.Columns.Add("STATUS");
        //    table.Columns.Add("LEDNO");
        //    table.Columns.Add("QUANTITY");
        //    return table;
        //}

        public DataTable GetMixTable()
        {
            DataTable table = new DataTable("MIX");
            table.Columns.Add("ORDERDATE");
            table.Columns.Add("BATCHNO");
            table.Columns.Add("CHANNELCODE");
            table.Columns.Add("CIGARETTECODE");
            table.Columns.Add("CIGARETTENAME");

            return table;
        }
    }
}
