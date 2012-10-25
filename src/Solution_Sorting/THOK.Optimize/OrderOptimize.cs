using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace THOK.Optimize
{
    /// <summary>
    /// ˮƽʽ���Զ��ּ�ϵͳ�����Ż�
    /// </summary>
    public class OrderOptimize
    {
        public DataTable Optimize(DataRow masterRow, DataRow[] orderRows, DataTable channelTable)
        {
            DataTable scOrderTable = GetEmptyOrder();

            foreach (DataRow orderRow in orderRows)
            {
                string cigaretteCode = orderRow["CIGARETTECODE"].ToString();
                
                int quantity = Convert.ToInt32(orderRow["QUANTITY"]);   //��ǰƷ�ƶ�������
                
                int towerQuantity = quantity; //��ʽ���ּ�����

                int channelQuantity = 0;      //ͨ�����ּ�����

                DataRow[] channelRows = channelTable.Select(string.Format("CIGARETTECODE = '{0}' AND CHANNELTYPE ='3' AND QUANTITY > 0", cigaretteCode), "QUANTITY");
                
                if (channelRows.Length != 0) 
                {
                    string channelType = channelRows[0]["CHANNELTYPE"].ToString();
                    int tmpQuantity = 0;

                    if (channelRows.Length > 1)//��Ʒ���ж��ͨ�����ּ�
                    {
                        int splitQuantity = 0;

                        channelQuantity = quantity; //����ͨ�����ּ�����

                        splitQuantity = channelQuantity / channelRows.Length; //����ƽ��ÿ��ͨ���ּ�����

                        tmpQuantity = channelQuantity;

                        for (int i = 0; i < channelRows.Length; i++)
                        {
                            int remainQuantity = Convert.ToInt32(channelRows[i]["QUANTITY"]);

                            if (i == channelRows.Length - 1) //���һ��ͨ��
                            {
                                if (tmpQuantity < remainQuantity)
                                {
                                    splitQuantity = tmpQuantity;
                                }
                                else
                                {
                                    channelQuantity -= (tmpQuantity - remainQuantity);
                                    splitQuantity = remainQuantity;
                                }
                            }
                            else
                            {
                                splitQuantity = Math.Min(splitQuantity, remainQuantity);
                                tmpQuantity -= splitQuantity;
                            }
                            AddRow(masterRow, scOrderTable, orderRow["CIGARETTECODE"], orderRow["CIGARETTENAME"], channelRows[i]["CHANNELCODE"], splitQuantity);
                           
                            channelRows[i]["QUANTITY"] = remainQuantity - splitQuantity;
                        }
                    }
                    else//��Ʒ��ֻ��һ��ͨ�����ּ�
                    {
                        
                        int remainQuantity = Convert.ToInt32(channelRows[0]["QUANTITY"]);
                        
                        channelQuantity = quantity;

                        AddRow(masterRow, scOrderTable, orderRow["CIGARETTECODE"], orderRow["CIGARETTENAME"], channelRows[0]["CHANNELCODE"], channelQuantity);

                        channelRows[0]["QUANTITY"] = remainQuantity - channelQuantity;
                    }
                    //������ʽ���ּ�����
                    towerQuantity = quantity - channelQuantity;
                }

                //�������ʽ���ּ���
                if (towerQuantity != 0)
                {
                    channelRows = channelTable.Select(string.Format("CIGARETTECODE = '{0}' AND CHANNELTYPE='2' AND QUANTITY>0 ", cigaretteCode), "QUANTITY DESC");
                    if (channelRows.Length == 0)//���Ʒ����һ���̵��зּ�
                    {
                        AddRow(masterRow, scOrderTable, orderRow["CIGARETTECODE"], orderRow["CIGARETTENAME"], "-1", towerQuantity);
                    }
                    else if (channelRows.Length > 1)
                    {
                        int[] ChannelQuantity = new int[channelRows.Length];
                        int TotalQuantity = towerQuantity;

                        for (int i = 0; i < towerQuantity; i++)
                        {
                            if (TotalQuantity == 0)
                                break;
                            for (int j = 0; j < channelRows.Length; j++)
                            {
                                if (TotalQuantity == 0)
                                    break;
                                if (Convert.ToInt32(channelRows[j]["QUANTITY"]) > 0)
                                {
                                    ChannelQuantity[j]++;
                                    channelRows[j]["QUANTITY"] = Convert.ToInt32(channelRows[j]["QUANTITY"]) - 1;
                                    TotalQuantity--;
                                }                                                                                        
                            }                                                    
                        }
                        for (int i = 0; i < ChannelQuantity.Length; i++)
                        {
                            AddRow(masterRow, scOrderTable, orderRow["CIGARETTECODE"], orderRow["CIGARETTENAME"], channelRows[i]["CHANNELCODE"], ChannelQuantity[i]);
                        }                      
                    }
                    else
                    {
                        channelRows[0]["QUANTITY"] = Convert.ToInt32(channelRows[0]["QUANTITY"]) - towerQuantity;
                        AddRow(masterRow, scOrderTable, orderRow["CIGARETTECODE"], orderRow["CIGARETTENAME"], channelRows[0]["CHANNELCODE"], towerQuantity);
                    }
                }
            }
            return scOrderTable;
        }

        private void AddRow(DataRow masterRow, DataTable scOrderTable, object cigaretteCode, object cigaretteName, object channelCode, int quantity)
        {
            if (quantity != 0)
            {
                DataRow row = scOrderTable.NewRow();
                row["SORTNO"] = masterRow["SORTNO"];
                row["LINECODE"] = masterRow["LINECODE"];
                row["ORDERDATE"] = masterRow["ORDERDATE"];
                row["BATCHNO"] = masterRow["BATCHNO"];
                row["ORDERID"] = masterRow["ORDERID"];
                row["ORDERNO"] = masterRow["ORDERNO"];
                row["CIGARETTECODE"] = cigaretteCode;
                row["CIGARETTENAME"] = cigaretteName;
                row["CHANNELCODE"] = channelCode;
                row["QUANTITY"] = quantity;
                scOrderTable.Rows.Add(row);
            }
        }

        private DataTable GetEmptyOrder()
        {
            DataTable table = new DataTable();
            table.Columns.Add("ORDERDATE");
            table.Columns.Add("BATCHNO");
            table.Columns.Add("LINECODE");
            table.Columns.Add("SORTNO", typeof(Int32));            
            table.Columns.Add("ORDERID");
            table.Columns.Add("ORDERNO", typeof(Int32));            
            table.Columns.Add("CIGARETTECODE");
            table.Columns.Add("CIGARETTENAME");
            table.Columns.Add("CHANNELCODE");
            table.Columns.Add("QUANTITY", typeof(Int32));
            return table;
        }
    }
}
