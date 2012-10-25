using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace THOK.Optimize
{
    public class SupplyOptimize
    {
        public DataTable Optimize(DataTable channelTable,bool IsAdvancedSupply)
        {
            DataTable supplyTable = GetEmptySupply();
            int serialNo = 1;
            foreach (DataRow row in channelTable.Rows)
            {
                int quantity = Convert.ToInt32(row["QUANTITY"]) / 50;
                if (quantity >= 1)
                {
                    int count = 1;
                    if (row["CHANNELTYPE"].ToString() == "3")
                        count = quantity >= 3 ? 3 : quantity;
                    if (row["CHANNELTYPE"].ToString() == "2" || !IsAdvancedSupply)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            DataRow supplyRow = supplyTable.NewRow();
                            supplyRow["ORDERDATE"] = row["ORDERDATE"];
                            supplyRow["BATCHNO"] = row["BATCHNO"];
                            supplyRow["SERIALNO"] = serialNo++;
                            supplyRow["LINECODE"] = row["LINECODE"];
                            supplyRow["SORTNO"] = 0;
                            supplyRow["GROUPNO"] = row["GROUPNO"];
                            supplyRow["CHANNELCODE"] = row["CHANNELCODE"];
                            supplyRow["CIGARETTECODE"] = row["CIGARETTECODE"];
                            supplyRow["CIGARETTENAME"] = row["CIGARETTENAME"];
                            supplyTable.Rows.Add(supplyRow);
                        }
                    }
                }
            }
            return supplyTable;
        }

        private DataTable GetEmptySupply()
        {
            DataTable table = new DataTable("SUPPLY");
            table.Columns.Add("ORDERDATE");
            table.Columns.Add("BATCHNO");
            table.Columns.Add("SERIALNO");
            table.Columns.Add("LINECODE");
            table.Columns.Add("SORTNO");
            table.Columns.Add("GROUPNO");
            table.Columns.Add("CHANNELCODE");
            table.Columns.Add("CIGARETTECODE");
            table.Columns.Add("CIGARETTENAME");

            return table;
        }
    }
}
