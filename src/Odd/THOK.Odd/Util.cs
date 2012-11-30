using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace THOK.Odd
{
    public class Util
    {
        public static void ShowInfo(string msg)
        {
            if (msg.IndexOf("DB.XML") == -1)
                System.Windows.Forms.MessageBox.Show(msg, "ÌבÊ¾", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        }
        
        public static void EnableFilter(DataGridView gridView)
        {
            if (gridView.DataSource is BindingSource)
                ((BindingSource)gridView.DataSource).Filter = "";

            foreach (DataGridViewColumn column in gridView.Columns)
            {
                if (column is DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn)
                    ((DataGridViewAutoFilter.DataGridViewAutoFilterTextBoxColumn)column).FilteringEnabled = true;
            }
        }
    }
}
