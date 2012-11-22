using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;

namespace THOK.Odd
{
    internal class OrderDetail
    {
        public OrderDetail(string name, int quantity)
        {
            this.Name = name;
            this.Quantity = quantity;
        }

        public override string ToString()
        {
            return Name + " " + Quantity.ToString();
        }
        public string Name;
        public int Quantity;
    }

    public class Printer
    {
        private string orderDate;
        private string orderID;
        private string customerName;
        private string address = "";
        private string sequence = "";
        private string routeName;
        private int currentQuantity = 0;
        private int totalQuantity = 0;
        private int currentPackage = 0;
        private int totalPackage = 0;
        private Queue<string> detailQueue = new Queue<string>();

        //����ݿͻ�ѡ�õı�ǩ��С����width��height��ֵ
        //private int width = 280;
        //private int height = 240;
        //private float quantity = 25.0f;

        //private int xOffset = 100;
        //private int count = 10;

        //�޸�(�λ���2010��9��1��)
        private float width = 230;        //��ǩ�Ŀ��
        private int startX = 2;         // ��ӡ��ʼ��X����
        private int startY = 3;         //��ӡ��ʼ��Y����
        private int columnCount = 0;    //����
        private int contentCount = 0;   //ÿ�е�Ʒ����
        private bool isSubString = false; //�Ƿ��ȡ��������
        private int subStringCount = 0; //��ȡ�������Ƶĳ���
        private int fontSize = 0;       //����Ĵ�С
        private int columnWidth = 0;    //ÿ�еĿ��
        private int rowHeight = 0;      //ÿ�еĸ߶�

        public Printer()
        {
            //count = (height - 68) / 12;
            //xOffset = width / Convert.ToInt32(Math.Ceiling(quantity / count));
        }

        public void Print(DataRow masterRow, DataTable detailTable, int quantity)
        {
            DataRow[] rows = new DataRow[detailTable.Rows.Count];
            detailTable.Rows.CopyTo(rows, 0);
            Print(masterRow, rows, quantity);
        }

        public void Print(DataRow masterRow, DataRow[] detailRows, int quantity)
        {
            Stack<OrderDetail> detail = new Stack<OrderDetail>();

            orderDate = Convert.ToDateTime(masterRow["ORDERDATE"]).ToShortDateString();
            orderID = masterRow["ORDERID"].ToString().Trim();
            customerName = masterRow["CUSTOMERNAME"].ToString().Trim();
            address = masterRow["ADDRESS"].ToString().Trim();
            sequence = masterRow["SORTID"].ToString().Trim();
            routeName = masterRow["ROUTENAME"].ToString().Trim();
            

            totalQuantity = quantity;
            totalPackage = totalQuantity % 25 == 0 ? totalQuantity / 25 : totalQuantity / 25 + 1;

            //���һ�����С�ڣ���������Ҫ�������ڶ�����������̷ŵ����һ����ȥ��װ
            //Ҳ���ǵ����ڶ���ֻ�������������һ��Ϊ�������������������������
            bool needSplit = totalQuantity % 25 < 5 && totalQuantity % 25 > 0;

            currentPackage = 0;
            currentQuantity = 0;
            foreach (DataRow row in detailRows)
            {
                string productName = row["CIGARETTENAME"].ToString().Trim();
                int tQuantity = Convert.ToInt32(row["QUANTITY"]);

                while (tQuantity > 0)
                {
                    int jx = needSplit && (currentPackage == totalPackage - 2) ? 20 : 25;��//��ǰ���ɰ�װ������

                    if (jx - currentQuantity >= tQuantity)//�����ǰ�̵��ּ�����С�ڿɰ�װ����
                    {
                        if (detail.Count != 0)//�����ջ��Ϊ�����鵱ǰ���̷ּ�Ʒ������һƷ���Ƿ���ͬ�������ͬ������һƷ�ƺϲ������򽫵�ǰƷ�Ƽ����������ջ
                        {
                            OrderDetail odTmp = detail.Peek();
                            if (odTmp != null && odTmp.Name.Equals(productName))
                            {
                                odTmp = detail.Pop();
                                odTmp.Quantity += tQuantity;
                                detail.Push(odTmp);
                            }
                            else
                                detail.Push(new OrderDetail(productName, tQuantity));
                        }
                        else//�����ջΪ�����ʾ֮ǰû��Ʒ�ƣ���ֱ�ӽ���ǰƷ�Ƽ��������ջ
                            detail.Push(new OrderDetail(productName, tQuantity));
                        currentQuantity += tQuantity;
                        tQuantity = 0;

                    }
                    else
                    {
                        int tmp = jx - currentQuantity;
                        if (detail.Count != 0)
                        {
                            OrderDetail odTmp = detail.Peek();
                            if (odTmp != null && odTmp.Name.Equals(productName))
                            {
                                odTmp = detail.Pop();
                                odTmp.Quantity += tmp;
                                detail.Push(odTmp);
                            }
                            else
                                detail.Push(new OrderDetail(productName, tmp));
                        }
                        else
                            detail.Push(new OrderDetail(productName, tmp));
                        currentQuantity += tmp;
                        tQuantity -= tmp;
                    }

                    if (currentQuantity == jx)//�����ǰ���Ѱ����������һ��
                    {
                        detail.Push(null);
                        currentQuantity = 0;
                        currentPackage++;
                    }
                }
            }

            //��ӡ��ǩ
            currentPackage = 1;
            currentQuantity = 0;
            detail = Reverse(detail);
            while (detail.Count > 0)
            {
                OrderDetail od = detail.Pop();
                if (od != null)
                {
                    currentQuantity += od.Quantity;
                    detailQueue.Enqueue(od.ToString());
                }
                else//��ʾһ�����ѽ�������ӡ��ǩ
                {
                    if (currentPackage <= totalPackage)
                    {
                        Print();
                        detailQueue = new Queue<string>();
                        currentPackage++;
                    }
                    currentQuantity = 0;
                }
            }
        }

        //��ת��ջ
        private Stack<OrderDetail> Reverse(Stack<OrderDetail> orderDetail)
        {
            Stack<OrderDetail> detail = new Stack<OrderDetail>();
            //���ջ����Ϊnull����ӣ���Ϊ��ӡ��ǩ������������һ��nullֵ
            if (orderDetail.Count != 0 && orderDetail.Peek() != null)
                detail.Push(null);
            while (orderDetail.Count != 0)
                detail.Push(orderDetail.Pop());
            return detail;
        }

        private void Print()
        {
            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pd_PrintPage);
            pd.Print();
        }

        //�����ǩ��ʽ�ı�ֻҪ�޸Ĵ˺�������
        void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            GetStyle();
            Font font = new Font("����", 9, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.Black);

            //�°汾 2010��9��15��
            /*
             * 1����·�������ͻ�������
             * 3����ţ���������������������
             * 4����ַ����������
             */

            Font bigFont = new Font("����", 13, FontStyle.Bold);
            if (customerName.Length > 13)
                customerName = customerName.Substring(0, 13);
            if (address.Length > 13)
                address = address.Substring(0, 13);

            //90          
            e.Graphics.DrawString("����:" + orderDate, font, brush, startX, startY);
            e.Graphics.DrawString("��·:" + routeName, font, brush, 100, startY);
            e.Graphics.DrawString(customerName, bigFont, brush, startX, 16);
            //e.Graphics.DrawString("���:" + sequence, font, brush, startX, 35);
            e.Graphics.DrawString(string.Format("���� {0}/{1}", currentQuantity, totalQuantity), font, brush, 85, 35);
            e.Graphics.DrawString(string.Format("����{0}/{1}", currentPackage, totalPackage), font, brush, 160, 35);
            e.Graphics.DrawString("��ַ:" + address, font, brush, startX, 49);

            font = new Font("����", fontSize, FontStyle.Bold);
            int i = 0;
            int j = 0;

            while (detailQueue.Count != 0)
            {
                if (i % contentCount == 0)
                    j = 0;
                int x = startX + i++ / contentCount * columnWidth;
                int y = 65 + rowHeight * j++;

                string cigaretteInfo = detailQueue.Dequeue().Trim();
                string cigaretteCount = cigaretteInfo.Substring(cigaretteInfo.IndexOf(" "));
                string cigaretteName = cigaretteInfo.Substring(0, cigaretteInfo.IndexOf(" "));
                if (isSubString)
                    if (cigaretteName.Length > subStringCount)
                        cigaretteName = cigaretteName.Substring(0, subStringCount);
                e.Graphics.DrawString(cigaretteName + ":" + cigaretteCount, font, brush, x, y);
            }
        }

        //�޸� 2010��9��2��
        private void GetStyle()
        {
            //һ������11��Ʒ��
            if (detailQueue.Count < 13)
            {
                fontSize = 9;
                columnCount = 2;
                contentCount = 6;
                //����6Ʒ�ƣ����ý�ȡ��������
                if (detailQueue.Count < 7)
                    isSubString = false;
                else
                {
                    isSubString = true;
                    subStringCount = 7;
                }
            }
            else
            {
                fontSize = 8;
                contentCount = 8;
                isSubString = true;
                if (detailQueue.Count >= 17)
                {
                    columnCount = 3;
                    subStringCount = 5;
                }
                else// 10 ~ 16 ��Ʒ��
                {
                    columnCount = 2;
                    subStringCount = 8;
                }
            }

            columnWidth = Convert.ToInt32(Math.Ceiling((width - startX - 3) / columnCount));
            rowHeight = fontSize == 8 ? 11 : 13; // 8��������+�����=11��9����13
        }
    }
}
