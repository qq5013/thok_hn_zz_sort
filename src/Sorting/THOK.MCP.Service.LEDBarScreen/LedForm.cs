using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.MCP.Service.LEDBarScreen
{
    public partial class LedForm : Form
    {
        private Config.Configuration config = null;
        private Panel[] plGroup;
        private Label[,] lbName;
        private Label[,] lbNum;
        private bool rolling = false;

        private int rollSpeed = 1;
        private int rollTime = 80;
        private System.Drawing.Size plSize = new Size(320,64);
        private System.Collections.Generic.Dictionary<string, string> haveData = null;
        private System.Threading.Thread thread = null;
        private int labelNumHeight = 16;//盘点时为16，分拣时为0
        private int charHeight = 13; //每个字高度
        private delegate void ControlAccessHandler(int groupNo, int ledNo,string name,int quantity);

        private int[,] delay;//当第一个字到顶时，记录rollTime的次数
        private int delayTime = 0;//当第一个字到顶时，停止滚动delayTime次rollTime时间

        private int nSpace = 1;//两组数据之间的间隔

        private int nGroupNo = 0;//最长名字的组号
        private int nLedNo = 0;//最长名字的编号
        private int maxLength = 0;//最长名字的字节数

        public LedForm()
        {
            InitializeComponent();
        }

        public LedForm(Config.Configuration config):this()
        {
            this.config = config;
            int height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            int width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            this.SetBounds(width, 0, width, height);
            //this.SetBounds(0, 0, width, height);
            plGroup = new Panel[config.Group.Count];
            lbName = new Label[config.Group.Count, 10];
            lbNum = new Label[config.Group.Count, 10];
            delay = new int[config.Group.Count, 10];
            delayTime = config.stopTime / config.rollTime;

            for (int i = 0; i < config.Group.Count; i++)
            {
                for (int j = 0; j < 10; j++)
                    delay[i, j] = 0;
            }

            rollSpeed = config.rollSpeed;
            rollTime = config.rollTime;

            CreateGroupPanel();
            CreateLed();

        }

        public void StopRoll()
        {
            rolling = false;
        }

        private void roll()
        {
            //当字数超过显示区域时，滚屏
            rolling = true;
            while (rolling)
            {

                System.Threading.Thread.Sleep(rollTime);
                if (delay[nGroupNo, nLedNo] < delayTime)//等待最长滚动
                {
                    delay[nGroupNo, nLedNo] = delay[nGroupNo, nLedNo] + 1;
                    if (delay[nGroupNo, nLedNo] == delayTime)//设置滚动开始
                    {
                        for (int groupNo = 0; groupNo < lbName.GetLength(0); groupNo++)
                        {
                            for (int ledNo = 0; ledNo < lbName.GetLength(1); ledNo++)
                            {
                                delay[groupNo, ledNo] = delayTime;
                            }
                        }
                    }
                    continue;
                }
                //进行滚动
                for (int groupNo = 0; groupNo < lbName.GetLength(0); groupNo++)
                {
                    for (int ledNo = 0; ledNo < lbName.GetLength(1); ledNo++)
                    {
                        if (delay[groupNo, ledNo] >= delayTime)
                            RollItem(groupNo, ledNo, "", 0);
                    }
                    
                }
                //Application.DoEvents();//不用线程
            }
            rolling = false;
        }

        private void CreateGroupPanel()
        {
            //每组增加一个panel容器
            foreach (int key in config.Group.Keys)
            {
                plGroup[key] = new Panel();
                plGroup[key].Location = config.Group[key];
                plGroup[key].BackColor = Color.Black;
                plGroup[key].Size = plSize;
                plGroup[key].BorderStyle = BorderStyle.FixedSingle;
                this.Controls.Add(plGroup[key]);
            }
        }
        private void CreateLed()
        {
            //添加led
            for (int groupNo = 0; groupNo < lbName.GetLength(0); groupNo++)
            {
                for (int ledNo = 0; ledNo < lbName.GetLength(1); ledNo++)
                {
                    lbNum[groupNo,ledNo] = new Label();
                    lbNum[groupNo, ledNo].Font = new System.Drawing.Font("幼圆", 6F, System.Drawing.FontStyle.Bold , System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    //lbNum[groupNo, ledNo].ForeColor = (ledNo%2==0)? Color.Red:Color.Green;
                    lbNum[groupNo, ledNo].ForeColor = Color.Red;
                    lbNum[groupNo, ledNo].Width = 16;
                    lbNum[groupNo, ledNo].Height = labelNumHeight;
                    lbNum[groupNo, ledNo].Top =  48;
                    lbNum[groupNo, ledNo].Left = ledNo * 32;
                    lbNum[groupNo, ledNo].Text = "";
                    lbNum[groupNo, ledNo].BackColor = Color.Transparent;
                    lbNum[groupNo, ledNo].Margin = new Padding(0);
                    plGroup[groupNo].Controls.Add(lbNum[groupNo, ledNo]);


                    lbName[groupNo, ledNo] = new Label();
                    lbName[groupNo, ledNo].Width = 16;
                    //lbName[groupNo, ledNo].Text = "天海欧康123";
                    lbName[groupNo, ledNo].Text = "无";
                    lbName[groupNo, ledNo].Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    //lbName[groupNo, ledNo].ForeColor = (ledNo % 2 == 0) ? Color.Red : Color.Green;
                    lbName[groupNo, ledNo].ForeColor = Color.Red;
                    lbName[groupNo, ledNo].Width = 16;
                    //lbName[groupNo, ledNo].Height = lbName[groupNo, ledNo].Font.Height * (lbName[groupNo, ledNo].Text.Length);
                    lbName[groupNo, ledNo].Height = charHeight * (lbName[groupNo, ledNo].Text.Length);
                    lbName[groupNo, ledNo].Top = 0;
                    lbName[groupNo, ledNo].Left = ledNo * 32-2;
                    lbName[groupNo, ledNo].BackColor = Color.Transparent;
                    lbName[groupNo, ledNo].Margin = new Padding(0);
                    plGroup[groupNo].Controls.Add(lbName[groupNo, ledNo]);
                }
 
            }

        }
       
        public void Refresh(System.Data.DataTable dt,bool showQuantity)
        {
            //刷新led显示的数据，并设置字体颜色

            if (showQuantity)
                labelNumHeight = 16;
            else
                labelNumHeight = 0;

            if (haveData == null)
                haveData = new Dictionary<string, string>();

            haveData.Clear();

            if (null != dt)
            {
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    int groupNo = Convert.ToInt32(dr["LEDGROUP"]);
                    int ledNo = Convert.ToInt32(dr["LEDNO"]);
                    int quantity = Convert.ToInt32(dr["QUANTITY"]);
                    string name = dr["CIGARETTENAME"].ToString();
                    if (dr["CHANNELTYPE"].ToString() == "5")
                    {
                        name = "多品牌";
                    }
                    //已经添加，针对一个烟道出多种烟的情况
                    if (haveData.ContainsKey(groupNo + "," + ledNo))
                    {
                        continue;
                    }
                    haveData.Add(groupNo + "," + ledNo, "");
                    name = name.Replace("(", "︵");
                    name = name.Replace(")", "︶");
                    name = name.Replace("（", "︵");
                    name = name.Replace("）", "︶");

                    if (name == "")
                    {
                        name = "无";
                    }
                    //所能显示的字数小于实际字数，滚屏
                    if ((plGroup[0].Height - labelNumHeight) / charHeight < name.Length )
                    {
                        string space = "";
                        for (int i = 0; i < nSpace; i++)
                            space = space + " ";
                        name = name + space + name;
                    }
                    if (name.Length >= maxLength)
                    {
                        maxLength = name.Length;
                        nGroupNo = groupNo;
                        nLedNo = ledNo;
                    }

                    SetName(groupNo, ledNo, name, quantity);
                    SetQuantity(groupNo, ledNo, name, quantity);
                    //if (name.Substring(0,3) == "芙蓉王")
                    //{
                    //    string aa = "";
                    //}
                    //if (haveData.ContainsKey(groupNo + "," + ledNo))
                    //{
                    //    if (name != "多品牌" || name != "无")
                    //    {
                    //        SetQuantity(groupNo, ledNo, name, quantity);
                    //    }
                    //}
                    //else
                    //{
                    //    SetQuantity(groupNo, ledNo, name, quantity);
                    //}
                }
            }

            //没数据的烟道显示"无"
            for (int groupNo = 0; groupNo < lbName.GetLength(0); groupNo++)
            {
                for (int ledNo = 0; ledNo < lbName.GetLength(1); ledNo++)
                {
                    if (haveData.ContainsKey(groupNo + "," + ledNo))
                        continue;
                    SetName(groupNo, ledNo, "无", 0);
                    SetQuantity(groupNo, ledNo, "无", 0);
                }
            }

            //if (!showQuantity)//分拣停止滚动
            //    rolling = false;
            //else 
            if (!rolling)//滚动未启动
            {
                //roll();
                //线程的方式
                thread = new System.Threading.Thread(roll);
                thread.Start();
            }
            
        }

        private void RollItem(int groupNo, int ledNo, string name, int quantity)
        {
            
            if (!lbName[groupNo, ledNo].InvokeRequired)
            {
                //字数在屏幕高度内不需滚
                if ((plGroup[0].Height - labelNumHeight) / charHeight >= lbName[groupNo, ledNo].Text.Length)
                    return;
                //int pos = lbName[groupNo, ledNo].Top + ((lbName[groupNo, ledNo].Text.Length - nSpace) / 2 + nSpace) * lbName[groupNo, ledNo].Font.Height;
                int pos = lbName[groupNo, ledNo].Top + ((lbName[groupNo, ledNo].Text.Length - nSpace) / 2 + nSpace) * charHeight;

                //第二组数据第一个字是否到顶，到顶重置计数为0,并且重新移动位置使第一组数据在顶 
                if (pos - rollSpeed < rollSpeed && pos >= rollSpeed)
                {
                    delay[groupNo, ledNo] = 0;
                    lbName[groupNo, ledNo].Top = pos - rollSpeed;
                }
                else
                    lbName[groupNo, ledNo].Top = lbName[groupNo, ledNo].Top - rollSpeed;
            }
            else
            {
                lbName[groupNo, ledNo].Invoke(new ControlAccessHandler(RollItem), groupNo, ledNo, name, quantity);
            }
                
        }
        private void SetName(int groupNo, int ledNo, string name,int quantity)
        {
            if (!lbName[groupNo, ledNo].InvokeRequired)
            {
                if (name == "无")
                    //lbName[groupNo, ledNo].ForeColor = Color.Blue;
                    lbName[groupNo, ledNo].ForeColor = Color.Red;
                else
                    //lbName[groupNo, ledNo].ForeColor = (ledNo % 2 == 0) ? Color.Red : Color.Green;
                    lbName[groupNo, ledNo].ForeColor =  Color.Red ;

                lbName[groupNo, ledNo].Text = name;
                //lbName[groupNo, ledNo].Height = lbName[groupNo, ledNo].Font.Height * name.Length;
                lbName[groupNo, ledNo].Height = charHeight * name.Length;
                
                //if (this.labelNumHeight == 0)//如果是分拣设置top为0不滚动
                //    lbName[groupNo, ledNo].Top = 0;
            }
            else
            {
                lbName[groupNo, ledNo].Invoke(new ControlAccessHandler(SetName),groupNo,ledNo,name,quantity);
            }
        }

        private void SetQuantity(int groupNo, int ledNo, string name, int quantity)
        {
            if (!lbNum[groupNo, ledNo].InvokeRequired)
            {
                if (name == "无")
                {
                    //lbNum[groupNo, ledNo].ForeColor = Color.Blue;
                    lbNum[groupNo, ledNo].ForeColor = Color.Red;
                    lbNum[groupNo, ledNo].Text = ""; 
                }
                else
                {
                    //lbNum[groupNo, ledNo].ForeColor = (ledNo % 2 == 0) ? Color.Red : Color.Green;
                    lbNum[groupNo, ledNo].ForeColor = Color.Red;
                    lbNum[groupNo, ledNo].Text = (quantity % 50).ToString();
                }
                lbNum[groupNo, ledNo].Height = labelNumHeight;
            }
            else
            {
                lbNum[groupNo, ledNo].Invoke(new ControlAccessHandler(SetQuantity), groupNo, ledNo, name, quantity);
            }
        }

        private void LedForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            rolling = false;
            thread.Abort();
        }

        private void LedForm_Load(object sender, EventArgs e)
        {

        }

    }
}