using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Printing;
using System.Data;
using THOK.AS.OTS.Dao;

namespace THOK.AS.OTS
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
        private int abnormityQuantity = 0;
        private string routeCode;
        private string routeName;
        private int currentQuantity = 0;
        private int totalQuantity = 0;
        private int currentPackage = 0;
        private int totalPackage = 0;
        private string totalCustomer ="";
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

            //orderDate = Convert.ToDateTime(masterRow["ORDERDATE"]).ToShortDateString();
            orderDate = Convert.ToDateTime(masterRow["ORDERDATE"]).Month.ToString() + "-" + Convert.ToDateTime(masterRow["ORDERDATE"]).Day.ToString();
            orderID = masterRow["ORDERID"].ToString().Trim();
            customerName = masterRow["CUSTOMERNAME"].ToString().Trim();
            address = masterRow["ADDRESS"].ToString().Trim();
            sequence = masterRow["ORDERNO"].ToString().Trim();
            routeName = masterRow["ROUTENAME"].ToString().Trim();
            abnormityQuantity =Convert.ToInt32(masterRow["ABNORMITY_QUANTITY"].ToString().Trim());
            routeCode = masterRow["ROUTECODE"].ToString().Trim();
            using (THOK.Util.PersistentManager pm = new THOK.Util.PersistentManager())
            {
                Dao.OrderDao orderDao = new Dao.OrderDao();
                totalCustomer = orderDao.FindMaxOrderNo(routeCode);
            
            }
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
            //GetStyle();
            Font font = new Font("����", 9, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.Black);

            //���޴�Ʊ�¸�ʽ 2012��11��18��

            Font bigFont = new Font("����", 13, FontStyle.Bold);
            Font bFont = new Font("����", 11, FontStyle.Bold);
            Font bigbigFont = new Font("����", 15, FontStyle.Bold);
            if (customerName.Length > 13)
                customerName = customerName.Substring(0, 13);

            e.Graphics.DrawString(string.Format("{0}-{1}/{2} {3} ", routeName, sequence, totalCustomer, orderDate), bigFont, brush, startX, startY);
            e.Graphics.DrawString(customerName, bigbigFont, brush, startX+8, 23);
            if (abnormityQuantity > 0)  
            {
                e.Graphics.DrawString(string.Format("�죺{0}", abnormityQuantity), bigFont, brush, 170, 150);
            }
            e.Graphics.DrawString(string.Format(" {0}��/{1}��{2}ҳ/{3}ҳ-2", currentQuantity, totalQuantity, currentPackage, totalPackage), bigbigFont, brush, startX, 170);

            int length = 0;
            string cigaretteInfo ="";
            foreach (string detail in detailQueue)
            {
                length = length + detail.Length + 1;
                cigaretteInfo = cigaretteInfo+ " "+ detail;
            }
            string a = length.ToString();
            string b = cigaretteInfo.ToString();
            if (detailQueue.Count < 6)
            {
                int draw_y = 50;
                foreach (string detail in detailQueue)
                {
                    string cigaretteCount = detail.Substring(detail.IndexOf(" "));
                    string cigaretteName = detail.Substring(0, detail.IndexOf(" "));
                    e.Graphics.DrawString(cigaretteName + ":" + cigaretteCount + "", bFont, brush, startX + 8, draw_y);
                    draw_y = draw_y + 18;
                }
            }
            else if (detailQueue.Count < 11)
            {
                string str = "";
                int y = length / 15;

                for (int o = 0; o <= length / 15; o++)//15���ַ��ͻ���
                {
                    if (o < y)
                    {
                        str += cigaretteInfo.Substring(o * 15, 15) + Environment.NewLine;
                    }
                    else
                    {
                        str += cigaretteInfo.Substring(o * 15);
                    }
                }

                e.Graphics.DrawString(str, bFont, brush, 10, 50);
            }
            else
            {
                string str = "";
                int y = length / 18;

                for (int o = 0; o <= length / 18; o++)//16���ַ��ͻ���
                {
                    if (o < y)
                    {
                        str += cigaretteInfo.Substring(o * 18, 18) + Environment.NewLine;
                    }
                    else
                    {
                        str += cigaretteInfo.Substring(o * 18);
                    }
                }

                e.Graphics.DrawString(str + "|", font, brush, 10, 50);
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
