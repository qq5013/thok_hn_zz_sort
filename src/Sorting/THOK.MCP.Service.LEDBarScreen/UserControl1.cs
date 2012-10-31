using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace THOK.MCP.Service.LEDBarScreen
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }
        LEDBarScreenService ledBar = new LEDBarScreenService();
        private void button1_Click(object sender, EventArgs e)
        {
            ledBar.Initialize("Led.xml");
            
            //ledForm.Visible = true;
        }
        int index = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new DataTable();
            dt.Columns.Add("CIGARETTENAME", typeof(string));
            dt.Columns.Add("QUANTITY", typeof(int));
            dt.Columns.Add("LEDGROUP", typeof(int));
            dt.Columns.Add("LEDNO", typeof(int));
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    System.Data.DataRow dr = dt.NewRow();
                    dr["CIGARETTENAME"] = (j % 3 == 0) ? "" : (index) + "天海欧康" + i + "-" + j;
                    //dr["CIGARETTENAME"] = "天海欧康";
                    dr["QUANTITY"] = 49;
                    dr["LEDGROUP"] = i;
                    dr["LEDNO"] = j;
                    dt.Rows.Add(dr);
                }
            }

            index++;
            ledBar.Write("sendtoled",dt);
        }

        private void UserControl1_Leave(object sender, EventArgs e)
        {
            ledBar.Release();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ledBar.Release();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = new DataTable();
            dt.Columns.Add("CIGARETTENAME", typeof(string));
            dt.Columns.Add("QUANTITY", typeof(int));
            dt.Columns.Add("LEDGROUP", typeof(int));
            dt.Columns.Add("LEDNO", typeof(int));
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    System.Data.DataRow dr = dt.NewRow();
                    dr["CIGARETTENAME"] = (j % 3 == 0) ? "中南海（8mg）" : "天(海欧)康天(海欧)康";
                    dr["CIGARETTENAME"] = (j % 5 == 0) ? "我的(名字)最长" : dr["CIGARETTENAME"];
                    dr["CIGARETTENAME"] = (j % 10 == 0) ? "我最短" : dr["CIGARETTENAME"];
                    dr["QUANTITY"] = 49;
                    dr["LEDGROUP"] = i;
                    dr["LEDNO"] = j;
                    dt.Rows.Add(dr);
                }
            }

            index++;
            ledBar.Write("Check", dt);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ledBar.Stop();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
}