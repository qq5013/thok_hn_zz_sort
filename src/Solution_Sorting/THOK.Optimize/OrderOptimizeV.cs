using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace THOK.Optimize
{
    /// <summary>
    /// ��ֱʽ���Զ��ּ�ϵͳ�����Ż�
    /// </summary>
    public class OrderOptimizeV
    {
        public DataSet Optimize(DataRow masterRow, DataRow[] orderRows, DataTable channelTable, ref int sortNo, int breakQuantity, int margin, bool IsBreakOrderForLastThree)
        {
            DataSet ds = new DataSet();
            DataTable masterTable = GetEmptyOrderMaster();
            DataTable detailTable  = GetEmptyOrderDetail();
            DataTable supplyTable = GetEmptySupply();

            AddMaster(masterRow, masterTable, ++sortNo);

            foreach (DataRow detailRow in orderRows)
            {
                DataRow[] channelRows = channelTable.Select(string.Format("CIGARETTECODE='{0}' AND QUANTITY>0", detailRow["CIGARETTECODE"]), "QUANTITY DESC");
                int quantity = Convert.ToInt32(detailRow["QUANTITY"]);

                if (channelRows.Length == 0)       //���Ʒ���ڻ���̵��зּ�
                {
                    AddRow(masterRow,sortNo, detailTable, detailRow["CIGARETTECODE"], detailRow["CIGARETTENAME"],"-1", quantity);
                }
                else if (channelRows.Length == 1)  //һ��Ʒ����һ���̵��зּ�
                {
                    Optimize(masterRow, channelRows[0], masterTable, detailTable, supplyTable, quantity, ref sortNo, breakQuantity, margin, IsBreakOrderForLastThree);
                }
                else if (channelRows.Length >= 2)  //һ��Ʒ���ڶ���̵��зּ�
                {
                    int[] quantitys = new int[channelRows.Length];

                    int[] channelQuantity = new int[channelRows.Length];
                    for (int i = 0; i < channelRows.Length; i++)
                        channelQuantity[i] = Convert.ToInt32(channelRows[i]["QUANTITY"]);

                    //��ֵ�����̵����зּ�
                    int totalQuantity = quantity;
                    while (totalQuantity != 0)
                    {
                        for (int j = 0; j < channelRows.Length && totalQuantity != 0; j++)
                        {
                            if (channelQuantity[j] > 0)
                            {
                                quantitys[j]++;
                                channelQuantity[j]--;
                                totalQuantity--;
                            }
                        }
                    }

                    //ȷ��ÿ���̵��ּ��Ƿ���Ҫ���зּ𶩵����
                    for (int i = 0; i < quantitys.Length; i++)
                    {
                        Optimize(masterRow, channelRows[i], masterTable, detailTable, supplyTable, quantitys[i], ref sortNo, breakQuantity, margin, IsBreakOrderForLastThree);
                    }        
                }
            }
            ds.Tables.Add(masterTable);
            ds.Tables.Add(detailTable);
            ds.Tables.Add(supplyTable);
            return ds;
        }

        private void Optimize(DataRow masterRow, DataRow channelRow, DataTable masterTable,
            DataTable detailTable, DataTable supplyTable, int quantity, ref int sortNo, int breakQuantity, int margin, bool IsBreakOrderForLastThree)
        {
            /**
             * 
             * ��Ҫ�𵥵����
             * 2����ʽ��ֻʣ���3��ʱ��Ҫ�𵥣���һ��Ϊһ��������5�����3,1,1��4����Ҫ���2,1,1��3�����1,1,1
             * 3��һ����ʽ�����γ��̴���22��ʱ��Ҫ�𵥡�23�����22,1��24�����22,2�� 
             * 
             * */
            int channelType = Convert.ToInt32(channelRow["CHANNELTYPE"]);
            int channelQuantity = Convert.ToInt32(channelRow["QUANTITY"]);
            int remainQuantity = Convert.ToInt32(channelRow["REMAINQUANTITY"]);
            int piece = Convert.ToInt32(channelRow["PIECE"]);
            int[] orderQuantity = null;

            #region ��

            //��Ҫ�۵����� 
            //breakQuantity = 17;
            //�Ƿ���Ҫ��
            bool bBreakOrder = false;
            bool bBreakOrderForLastThree = IsBreakOrderForLastThree;

            switch (channelType)
            {
                case 2:
                    bBreakOrder = channelType == 2;
                    break;
                case 3:
                    bBreakOrder = channelType == 3;
                    breakQuantity += margin;
                    break;
                default:
                    break;
            }


            if ( bBreakOrder && quantity > breakQuantity) //���Ϊ��ʽ���Ҷ�����������breakQuantity��ʱ��Ҫ��
            {
                int count = quantity / breakQuantity;
                if (bBreakOrderForLastThree && channelQuantity - quantity <= 3)//����˴γ��̰������3��
                {
                    int tmp = quantity % breakQuantity;
                    if (tmp == 0)
                    {
                        //���� breakQuantity * 2;
                        orderQuantity = new int[count + 2];
                        for (int i = 0; i < count; i++)
                            orderQuantity[i] = breakQuantity;
                        orderQuantity[count - 1] = breakQuantity - 2;
                        orderQuantity[count] = 1;
                        orderQuantity[count + 1] = 1;
                    }
                    else if (0 < tmp && tmp < 3)
                    {
                        //����quantity= breakQuantity + 1 ���quantity-2,1,1;breakQuantity + 2 ���quantity-2,1,1
                        orderQuantity = new int[count + 2];

                        for (int i = 0; i < count; i++)
                            orderQuantity[i] = breakQuantity;

                        if (tmp == 1)
                            orderQuantity[count - 1] = breakQuantity - 1;
                        orderQuantity[count] = 1;
                        orderQuantity[count + 1] = 1;

                    }
                    else if (tmp >= 3)//����quantity = breakQuantity + 6
                    {
                        orderQuantity = new int[count + 3];

                        for (int i = 0; i < count; i++)
                            orderQuantity[i] = breakQuantity;

                        orderQuantity[count] = tmp - 2;
                        orderQuantity[count + 1] = 1;
                        orderQuantity[count + 2] = 1;
                    }
                }
                else//�����Ҫ�𵥵��˴γ��̲��������3��
                {                    
                    int tmp = quantity % breakQuantity;
                    if (tmp != 0)
                    {
                        orderQuantity = new int[count + 1];
                        for (int i = 0; i < count; i++)
                            orderQuantity[i] = breakQuantity;
                        orderQuantity[orderQuantity.Length - 1] = tmp;
                    }
                    else
                    {
                        //quantity % breakQuantity ����Ϊ0ʱ����
                        orderQuantity = new int[count];
                        for (int i = 0; i < count; i++)
                            orderQuantity[i] = breakQuantity;
                    }
                }

            }
            else if (bBreakOrderForLastThree && quantity > 1 && channelQuantity - quantity <= 3)
            {
                //���ֻʣ���������Ҫ��

                if (quantity >= 3)
                {
                    orderQuantity = new int[3];
                    orderQuantity[0] = quantity - 2;
                }
                else
                {   //quantity=2
                    orderQuantity = new int[quantity];
                    orderQuantity[0] = 1;
                }
                for (int i = 1; i < orderQuantity.Length; i++)
                    orderQuantity[i] = 1;

            }
            else
            {
                //����Ҫ��
                orderQuantity = new int[1];
                orderQuantity[0] = quantity;
            }

            #endregion

            #region ����

            int tmpQuantity = channelQuantity;           
            for (int i = 0; i < orderQuantity.Length; i++)
            {
                //��Ϊ��20���������һ�������¼�����������20�в���Ҫ��������¼
                if (i != 0)
                    AddMaster(masterRow, masterTable, ++sortNo);

                AddDetail(masterRow, channelRow, detailTable, sortNo, orderQuantity[i]);

                //��¼�ּ�����7���Ķ�����
                if (tmpQuantity >= 7 && tmpQuantity - orderQuantity[i] <= 7)
                    channelRow["SORTNO"] = sortNo;

                tmpQuantity -= orderQuantity[i];

                //�������ķּ�ϵͳ��Ҫ���㲹����
                for (int j = 0; j < orderQuantity[i] / 50; j++)
                {
                    if (piece > 0)
                    {
                        AddSupply(supplyTable, sortNo, channelRow);
                        piece--;
                    }
                }

                remainQuantity -= orderQuantity[i] % 50;
                if (piece > 0 && remainQuantity <= 0)
                {
                    remainQuantity += 50;
                    AddSupply(supplyTable, sortNo, channelRow);
                    piece--;
                }
            }

            //��¼��ǰ�̵�δ�ּ�ʣ������
            channelRow["QUANTITY"] = channelQuantity - quantity;

            channelRow["REMAINQUANTITY"] = remainQuantity;
            channelRow["PIECE"] = piece;

            #endregion
        }

        private void AddMaster(DataRow masterRow, DataTable masterTable, int sortNo)
        {
            DataRow newRow = masterTable.NewRow();
            newRow["ORDERDATE"] = masterRow["ORDERDATE"];
            newRow["BATCHNO"] = masterRow["BATCHNO"];
            newRow["LINECODE"] = masterRow["LINECODE"];
            newRow["SORTNO"] = sortNo;
            newRow["ORDERID"] = masterRow["ORDERID"];
            newRow["AREACODE"] = masterRow["AREACODE"];
            newRow["AREANAME"] = masterRow["AREANAME"];
            newRow["ROUTECODE"] = masterRow["ROUTECODE"];
            newRow["ROUTENAME"] = masterRow["ROUTENAME"];
            newRow["CUSTOMERCODE"] = masterRow["CUSTOMERCODE"];
            newRow["CUSTOMERNAME"] = masterRow["CUSTOMERNAME"];

            newRow["ADDRESS"] = masterRow["ADDRESS"];
            newRow["ORDERNO"] = masterRow["ORDERNO"];            

            newRow["QUANTITY"] = 0;
            newRow["ABNORMITY_QUANTITY"] = masterRow["ABNORMITY_QUANTITY"];

            masterTable.Rows.Add(newRow);
        }

        private void AddDetail(DataRow masterRow, DataRow channelRow, DataTable detailTable, int sortNo, int quantity)
        {
            DataRow newRow = detailTable.NewRow();
            newRow["ORDERDATE"] = masterRow["ORDERDATE"];
            newRow["BATCHNO"] = masterRow["BATCHNO"];

            newRow["LINECODE"] = masterRow["LINECODE"];
            newRow["SORTNO"] = sortNo;
            newRow["ORDERID"] = masterRow["ORDERID"];

            newRow["CIGARETTECODE"] = channelRow["CIGARETTECODE"];
            newRow["CIGARETTENAME"] = channelRow["CIGARETTENAME"];

            newRow["CHANNELCODE"] = channelRow["CHANNELCODE"];
            newRow["QUANTITY"] = quantity;

            detailTable.Rows.Add(newRow);
        }

        private void AddRow(DataRow masterRow, int sortNo,DataTable scOrderTable, object cigaretteCode, object cigaretteName, object channelCode, int quantity)
        {
            if (quantity != 0)
            {
                DataRow row = scOrderTable.NewRow();
                row["ORDERDATE"] = masterRow["ORDERDATE"];
                row["BATCHNO"] = masterRow["BATCHNO"];

                row["LINECODE"] = masterRow["LINECODE"];
                row["SORTNO"] = sortNo;
                row["ORDERID"] = masterRow["ORDERID"];              
                                
                row["CIGARETTECODE"] = cigaretteCode;
                row["CIGARETTENAME"] = cigaretteName;

                row["CHANNELCODE"] = channelCode;
                row["QUANTITY"] = quantity;

                scOrderTable.Rows.Add(row);
            }
        }

        private void AddSupply(DataTable supplyTable, int sortNo, DataRow channelRow)
        {
            DataRow newRow = supplyTable.NewRow();
            newRow["ORDERDATE"] = channelRow["ORDERDATE"];
            newRow["BATCHNO"] = channelRow["BATCHNO"];
            newRow["SORTNO"] = sortNo;
            newRow["GROUPNO"] = channelRow["GROUPNO"];
            newRow["LINECODE"] = channelRow["LINECODE"];
            newRow["CHANNELCODE"] = channelRow["CHANNELCODE"];
            newRow["CIGARETTECODE"] = channelRow["CIGARETTECODE"];
            newRow["CIGARETTENAME"] = channelRow["CIGARETTENAME"];
            System.Diagnostics.Debug.WriteLine(string.Format("{0} {1}", channelRow["CHANNELCODE"], channelRow["GROUPNO"]));
            supplyTable.Rows.Add(newRow);
        }

        private DataTable GetEmptyOrderMaster()
        {
            DataTable table = new DataTable("MASTER");
            table.Columns.Add("ORDERDATE");
            table.Columns.Add("BATCHNO", typeof(Int32));
            table.Columns.Add("LINECODE");
            table.Columns.Add("SORTNO", typeof(Int32));            
            table.Columns.Add("ORDERID");
            table.Columns.Add("AREACODE");
            table.Columns.Add("AREANAME");
            table.Columns.Add("ROUTECODE");
            table.Columns.Add("ROUTENAME");
            table.Columns.Add("CUSTOMERCODE");
            table.Columns.Add("CUSTOMERNAME");

            table.Columns.Add("ADDRESS");
            table.Columns.Add("ORDERNO");
            
            table.Columns.Add("QUANTITY");
            table.Columns.Add("ABNORMITY_QUANTITY");

            return table;
        }

        private DataTable GetEmptyOrderDetail()
        {
            DataTable table = new DataTable("DETAIL");
            table.Columns.Add("ORDERDATE");
            table.Columns.Add("BATCHNO");
            table.Columns.Add("LINECODE");
            table.Columns.Add("SORTNO", typeof(Int32));            
            table.Columns.Add("ORDERID");
            table.Columns.Add("CIGARETTECODE");
            table.Columns.Add("CIGARETTENAME");
            table.Columns.Add("CHANNELCODE");
            table.Columns.Add("QUANTITY", typeof(Int32));
            return table;
        }

        private DataTable GetEmptySupply()
        {
            DataTable table = new DataTable("SUPPLY");
            table.Columns.Add("ORDERDATE");
            table.Columns.Add("BATCHNO");
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
