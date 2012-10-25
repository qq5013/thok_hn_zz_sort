using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.AS.Sorting.View
{
    public partial class ModifyOrderDialog : Form
    {
        int firstQuantity = 0;
        int modifyQuantity = 0;
        public ModifyOrderDialog(string sortNo,string orderID,string channelName,string cigaretteCode,string cigaretteName,int quantity)
        {
            InitializeComponent();
            label1.Text = label1.Text + sortNo;
            label2.Text = label2.Text + orderID;
            label3.Text = label3.Text + channelName;
            label4.Text = label4.Text + cigaretteCode;
            label5.Text = label5.Text + cigaretteName;
            label6.Text = label6.Text + quantity;
            textBox1.Text = quantity.ToString();
            firstQuantity = quantity;
            btnOk.Enabled = false;
        }
        public int GetModifyQuantity()
        {
            return modifyQuantity;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string strQuantity = textBox1.Text.ToString();
            int quantity = 0;
            if (!Int32.TryParse(strQuantity,out quantity))
            {
                btnOk.Enabled = false;
                label8.Text = "请输入数字！";
                label8.ForeColor = Color.Red;
                textBox1.Focus();
                return;
            }
            if (quantity < 0 )
            {
                btnOk.Enabled = false;
                label8.Text = "请输入大于等于 0 小于原始数量的数字！";
                label8.ForeColor = Color.Red;
                textBox1.Focus();
                return;
            }
            if (quantity >= firstQuantity)
            {
                btnOk.Enabled = false;
                label8.Text = "请输入小于原始数量的数字！";
                label8.ForeColor = Color.Red;
                textBox1.Focus();
                return;
            }
            modifyQuantity = quantity;
            label8.Text = "";
            btnOk.Enabled = true;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            string strQuantity = textBox1.Text.ToString();
            int quantity = 0;
            if (!Int32.TryParse(strQuantity, out quantity))
            {
                btnOk.Enabled = false;
                label8.Text = "请输入数字！";
                label8.ForeColor = Color.Red;
                textBox1.Focus();
                return;
            }
            if (quantity < 0)
            {
                btnOk.Enabled = false;
                label8.Text = "请输入大于等于 0 小于原始数量的数字！";
                label8.ForeColor = Color.Red;
                textBox1.Focus();
                return;
            }
            if (quantity >= firstQuantity)
            {
                btnOk.Enabled = false;
                label8.Text = "请输入小于原始数量的数字！";
                label8.ForeColor = Color.Red;
                textBox1.Focus();
                return;
            }
            modifyQuantity = quantity;
            label8.Text = "";
            btnOk.Enabled = true;
        }
    }
}