using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace THOK.Optimize
{
    public class ChannelOptimize
    {

        public void Optimize(DataTable orderTable, DataTable channelTable, DataTable deviceTable)
        {
            DataTable tmpTable = null;

            //�̶��̵�����
            foreach (DataRow deviceRow in deviceTable.Rows)
            {
                string channelType = deviceRow["CHANNELTYPE"].ToString();
                switch (channelType)
                {
                    case "2":
                        int groupCount = Convert.ToInt32(deviceRow["GROUPCOUNT"]);
                        tmpTable = GenerateTmpTable(groupCount);
                        SetGroup(channelTable, groupCount);
                        SetFixedTowerChannel(orderTable, channelTable, tmpTable, channelType);
                        break;
                    case "3":
                        SetFixedChannel(orderTable, channelTable, channelType);
                        break;
                }
            }

            //�ǹ̶��̵�����
            foreach (DataRow deviceRow in deviceTable.Rows)
            {
                string channelType = deviceRow["CHANNELTYPE"].ToString();
                
                switch (channelType)
                {
                    case "2": //��ʽ�� 
                        SetTowerChannel(orderTable, channelTable, tmpTable, channelType);
                        break;

                    case "3": //��5ͨ����
                        SetChannel(orderTable, channelTable, channelType);
                        break;
                }
            }
        }


        /// <summary>
        /// ����ʽ���̵����䵽��ͬ�Ĳ�����
        /// </summary>
        /// <param name="channelSet"></param>
        /// <param name="groupNum"></param>
        private void SetGroup(DataTable channelTable, int groupNum)
        {
            DataRow[] channelRows = channelTable.Select("CHANNELTYPE='2' AND STATUS='1'", "CHANNELCODE");
            int groupCount = channelRows.Length / groupNum;
            int i = 0;
            int maxGroupNum = groupNum - 1;

            foreach (DataRow channelRow in channelRows)
            {
                int j = i++ / groupCount;

                if (j < maxGroupNum)
                {
                    channelRow["GROUPNO"] = j;
                }
                else
                {
                    channelRow["GROUPNO"] = maxGroupNum;
                }
            }
        }

        private void SetFixedChannel(DataTable orderTable, DataTable channelTable, string channelType)
        {
            //�̶�ͨ����Ʒ��
            foreach (DataRow orderRow in orderTable.Rows)
            {
                //ȡ��ǰƷ�ƹ̶�ͨ�����̵�
                DataRow[] channelRows = channelTable.Select(string.Format("CHANNELTYPE = '{0}' AND CIGARETTECODE = '{1}'  AND STATUS='1'", channelType, orderRow["CIGARETTECODE"]), "CHANNELCODE");
                int quantity = Convert.ToInt32(orderRow["QUANTITY"]);  //��ǰƷ�ƶ�������

                if (channelRows.Length > 1)//ռ�ö���ͨ����
                {
                    int splitQuantity = quantity / channelRows.Length / 50 * 50;

                    for (int i = 0; i < channelRows.Length; i++)
                    {
                        channelRows[i]["CIGARETTECODE"] = orderRow["CIGARETTECODE"];
                        channelRows[i]["CIGARETTENAME"] = orderRow["CIGARETTENAME"];
                        if (i == channelRows.Length - 1)
                            channelRows[i]["QUANTITY"] = quantity - splitQuantity * (channelRows.Length - 1);
                        else
                            channelRows[i]["QUANTITY"] = splitQuantity;
                    }
                    //���㵱ǰƷ��ʣ����
                    orderRow["QUANTITY"] = 0;
                }
                else if (channelRows.Length == 1) //ֻռ��һ��ͨ����
                {
                    channelRows[0]["CIGARETTECODE"] = orderRow["CIGARETTECODE"];
                    channelRows[0]["CIGARETTENAME"] = orderRow["CIGARETTENAME"];
                    channelRows[0]["QUANTITY"] = quantity;
                    orderRow["QUANTITY"] = 0;
                }

            }
        }

        /// <summary>
        /// ����ͨ��������
        /// </summary>
        /// <param name="orderTable"></param>
        /// <param name="channelTable"></param>
        private void SetChannel(DataTable orderTable, DataTable channelTable, string channelType)
        {
            
            //�ǹ̶�ͨ����Ʒ��
            DataRow[] orderRows = orderTable.Select("QUANTITY > 0", "QUANTITY DESC");
            for (int j = 0; j < orderRows.Length; j++)
            {
                DataRow orderRow = orderRows[j];
                //ȡδ��ռ��ͨ�����̵�
                DataRow[] channelRows = channelTable.Select(string.Format("CHANNELTYPE = '{0}' AND CIGARETTECODE=''  AND STATUS='1'", channelType), "CHANNELINDEX");

                if (channelRows.Length != 0)
                {
                    int quantity = Convert.ToInt32(orderRow["QUANTITY"]);//��ǰƷ�ƶ�������                                      
                    channelRows[0]["CIGARETTECODE"] = orderRow["CIGARETTECODE"];
                    channelRows[0]["CIGARETTENAME"] = orderRow["CIGARETTENAME"];
                    channelRows[0]["QUANTITY"] = quantity;
                    orderRow["QUANTITY"] = 0;
                }
                else
                    break;
            }
        }

        private void SetFixedTowerChannel(DataTable orderTable, DataTable channelTable, DataTable tmpTable, string channelType)
        {
            DataRow[] orderRows = orderTable.Select("", "QUANTITY DESC");

            //�̶���ʽ������Ʒ��
            foreach (DataRow orderRow in orderRows)
            {
                //��ǰƷ���Ƿ��ǹ̶��̵�
                DataRow[] channelRows = channelTable.Select(string.Format("CHANNELTYPE = '{0}' AND CIGARETTECODE = '{1}'  AND STATUS='1'", channelType, orderRow["CIGARETTECODE"]), "CHANNELCODE");
                int quantity = Convert.ToInt32(orderRow["QUANTITY"]);
                //�����ǰƷ�ƴ����1�����Ϲ̶��̵�
                if (channelRows.Length > 1)
                {
                    //ÿ���̵�ƽ������
                    int avgQuantity = quantity / channelRows.Length;
                    int lastQuantity = quantity - avgQuantity * channelRows.Length;

                    for (int i = 0; i < channelRows.Length; i++)
                    {
                        int tmpQuantity = avgQuantity;
                        if (lastQuantity > 0)
                        {
                            tmpQuantity += 1;
                            lastQuantity--;
                        }
                        SetFixedTowerChannel(orderRow, channelRows[i], tmpTable, tmpQuantity);
                    }

                    orderRow["QUANTITY"] = 0;
                }
                else if (channelRows.Length == 1)
                {
                    //���̷��䵽�̶��̵�
                    SetFixedTowerChannel(orderRow, channelRows[0], tmpTable, quantity);
                    orderRow["QUANTITY"] = 0;
                }
            }
        }

        /// <summary>
        /// ���������������̵�
        /// </summary>
        /// <param name="cigaretteSet"></param>
        /// <param name="channelSet"></param>
        /// <param name="tmpTable"></param>
        private void SetTowerChannel(DataTable orderTable, DataTable channelTable, DataTable tmpTable, string channelType)
        {
            
            //�ǹ̶���ʽ���ľ���Ʒ��
            DataRow[] orderRows = orderTable.Select("QUANTITY > 0", "QUANTITY DESC");

            foreach (DataRow orderRow in orderRows)
            {
                int quantity = Convert.ToInt32(orderRow["QUANTITY"]);
                SetTowerChannel(orderRow, channelTable, tmpTable, quantity);
            }
        }

        private void SetFixedTowerChannel(DataRow orderRow, DataRow channelRow, DataTable tmpTable, int quantity)
        {
            if (quantity != 0)
            {
                channelRow["CIGARETTECODE"] = orderRow["CIGARETTECODE"];
                channelRow["CIGARETTENAME"] = orderRow["CIGARETTENAME"];
                channelRow["QUANTITY"] = quantity;

                DataRow[] tmpRows = tmpTable.Select(string.Format("GROUPNO = {0}", channelRow["GROUPNO"]), "QUANTITY");
                tmpRows[0]["QUANTITY"] = Convert.ToInt32(tmpRows[0]["QUANTITY"]) + quantity;
            }
        }

        /// <summary>
        /// ����ǹ̶���ʽ�̵�
        /// </summary>
        /// <param name="orderRow"></param>
        /// <param name="channelTable"></param>
        /// <param name="tmpTable"></param>
        /// <param name="quantity"></param>
        private void SetTowerChannel(DataRow orderRow, DataTable channelTable, DataTable tmpTable, int quantity)
        {
            if (quantity != 0)
            {
                //����ɷ���ķǹ̶��̵�Ϊ0
                if ((int)channelTable.Compute("COUNT(CHANNELCODE)", "CIGARETTECODE='' AND CHANNELTYPE='2' AND STATUS='1'") == 0)
                {
                    if ((int)channelTable.Compute("COUNT(CHANNELCODE)", "CIGARETTECODE='' AND CHANNELTYPE IN ('5') AND STATUS='1'") == 0)
                    {
                        throw new Exception("���о���Ʒ��δ�����̵�,�����ϵͳ������");
                    }
                    else
                    {
                        return;
                    }
                }

                DataRow[] tmpRows = tmpTable.Select("", "QUANTITY");
                foreach (DataRow tmpRow in tmpRows)
                {
                    DataRow[] channelRows = channelTable.Select("CIGARETTECODE='' AND CHANNELTYPE='2'  AND STATUS='1' AND GROUPNO = " + tmpRow["GROUPNO"], "CHANNELCODE");
                    if (channelRows.Length != 0)
                    {
                        channelRows[0]["CIGARETTECODE"] = orderRow["CIGARETTECODE"];
                        channelRows[0]["CIGARETTENAME"] = orderRow["CIGARETTENAME"];
                        channelRows[0]["QUANTITY"] = quantity;
                        tmpRow["QUANTITY"] = Convert.ToInt32(tmpRow["QUANTITY"]) + quantity;
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// ������ʱ��
        /// </summary>
        /// <param name="groupNum"></param>
        /// <returns></returns>
        private DataTable GenerateTmpTable(int groupNum)
        {
            DataTable table = new DataTable();
            table.Columns.Add("GROUPNO");
            table.Columns.Add("QUANTITY", typeof(Int32));
            for (int i = 0; i < groupNum; i++)
            {
                table.Rows.Add(i, 0);
            }
            return table;
        }
    }
}
