using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using THOK.AS.Sorting.Dal;

namespace THOK.AS.Sorting.View
{
    public partial class CacheChannelCheckForm : Form
    {
        public CacheChannelCheckForm(int deviceNo)
        {
            InitializeComponent();
            string LedGroup = deviceNo.ToString();
            string SortNo="1";
            ChannelDal channelDal = new ChannelDal();
            dgvMain.DataSource = channelDal.GetChannel(LedGroup, SortNo);
            this.Text = this.Text + string.Format("-��{0}��LED��", LedGroup);
        }
        public CacheChannelCheckForm(string deviceClass)
        {
            InitializeComponent();
            string SortNo = "1";
            ChannelDal channelDal = new ChannelDal();
            dgvMain.DataSource = channelDal.GetChannel(deviceClass, SortNo, "����������������");
            this.Text = this.Text + string.Format("-ȫ��{0}", deviceClass);
        }
    }
}