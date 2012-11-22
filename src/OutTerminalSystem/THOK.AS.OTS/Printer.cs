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

        //请根据客户选用的标签大小更改width和height的值
        //private int width = 280;
        //private int height = 240;
        //private float quantity = 25.0f;

        //private int xOffset = 100;
        //private int count = 10;

        //修改(何华东2010年9月1日)
        private float width = 230;        //标签的宽度
        private int startX = 2;         // 打印开始的X坐标
        private int startY = 3;         //打印开始的Y坐标
        private int columnCount = 0;    //几列
        private int contentCount = 0;   //每列的品牌数
        private bool isSubString = false; //是否截取卷烟名称
        private int subStringCount = 0; //截取卷烟名称的长度
        private int fontSize = 0;       //字体的大小
        private int columnWidth = 0;    //每列的宽度
        private int rowHeight = 0;      //每行的高度

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

            //最后一包如果小于５条，则需要将倒数第二包的最后５条烟放到最后一包中去包装
            //也就是倒数第二包只包２０条，最后一包为６、７、８、９条这四种情况
            bool needSplit = totalQuantity % 25 < 5 && totalQuantity % 25 > 0;

            currentPackage = 0;
            currentQuantity = 0;
            foreach (DataRow row in detailRows)
            {
                string productName = row["CIGARETTENAME"].ToString().Trim();
                int tQuantity = Convert.ToInt32(row["QUANTITY"]);

                while (tQuantity > 0)
                {
                    int jx = needSplit && (currentPackage == totalPackage - 2) ? 20 : 25;　//当前包可包装总条数

                    if (jx - currentQuantity >= tQuantity)//如果当前烟道分拣数量小于可包装数量
                    {
                        if (detail.Count != 0)//如果堆栈不为空则检查当前卷烟分拣品牌与上一品牌是否相同，如果相同则与上一品牌合并，否则将当前品牌及数量加入堆栈
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
                        else//如果堆栈为空则表示之前没有品牌，则直接将当前品牌及数量入堆栈
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

                    if (currentQuantity == jx)//如果当前包已包满，则包下一包
                    {
                        detail.Push(null);
                        currentQuantity = 0;
                        currentPackage++;
                    }
                }
            }

            //打印标签
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
                else//表示一个包已结束，打印标签
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

        //反转堆栈
        private Stack<OrderDetail> Reverse(Stack<OrderDetail> orderDetail)
        {
            Stack<OrderDetail> detail = new Stack<OrderDetail>();
            //如果栈顶不为null则添加，因为打印标签的条件是碰到一个null值
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

        //如果标签格式改变只要修改此函数即可
        void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //GetStyle();
            Font font = new Font("宋体", 9, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.Black);

            //株洲打票新格式 2012年11月18日

            Font bigFont = new Font("宋体", 13, FontStyle.Bold);
            Font bFont = new Font("宋体", 11, FontStyle.Bold);
            Font bigbigFont = new Font("黑体", 15, FontStyle.Bold);
            if (customerName.Length > 13)
                customerName = customerName.Substring(0, 13);

            e.Graphics.DrawString(string.Format("{0}-{1}/{2} {3} ", routeName, sequence, totalCustomer, orderDate), bigFont, brush, startX, startY);
            e.Graphics.DrawString(customerName, bigbigFont, brush, startX+8, 23);
            if (abnormityQuantity > 0)  
            {
                e.Graphics.DrawString(string.Format("异：{0}", abnormityQuantity), bigFont, brush, 170, 150);
            }
            e.Graphics.DrawString(string.Format(" {0}条/{1}条{2}页/{3}页-2", currentQuantity, totalQuantity, currentPackage, totalPackage), bigbigFont, brush, startX, 170);

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

                for (int o = 0; o <= length / 15; o++)//15个字符就换行
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

                for (int o = 0; o <= length / 18; o++)//16个字符就换行
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

        //修改 2010年9月2日
        private void GetStyle()
        {
            //一包少于11个品牌
            if (detailQueue.Count < 13)
            {
                fontSize = 9;
                columnCount = 2;
                contentCount = 6;
                //少于6品牌，不用截取卷烟名称
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
                else// 10 ~ 16 个品牌
                {
                    columnCount = 2;
                    subStringCount = 8;
                }
            }

            columnWidth = Convert.ToInt32(Math.Ceiling((width - startX - 3) / columnCount));
            rowHeight = fontSize == 8 ? 11 : 13; // 8号宋体字+间距大概=11，9号则13
        }
    }
}
