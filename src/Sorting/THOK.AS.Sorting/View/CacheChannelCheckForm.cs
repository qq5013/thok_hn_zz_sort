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
        private OrderDal orderDal = new OrderDal();

        public CacheChannelCheckForm(int deviceNo)
        {
            InitializeComponent();
            string LedGroup = (deviceNo - 1).ToString();
            string SortNo = orderDal.FindMaxSortedNo();
            ChannelDal channelDal = new ChannelDal();
            dgvMain.DataSource = channelDal.GetChannel(LedGroup, SortNo);
            this.Text = this.Text + string.Format("-第{0}组LED屏", deviceNo);
        }
        public CacheChannelCheckForm(string deviceClass)
        {
            InitializeComponent();
            string SortNo = orderDal.FindMaxSortedNo();
            ChannelDal channelDal = new ChannelDal();
            dgvMain.DataSource = channelDal.GetChannel(deviceClass, SortNo, "本参数无特殊意义");
            this.Text = this.Text + string.Format("-全部{0}", deviceClass);
        }
    }
}