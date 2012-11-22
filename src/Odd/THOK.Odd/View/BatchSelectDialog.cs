using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.Odd.View
{
    public partial class BatchSelectDialog : Form
    {
        public string SelectedPrintBatch
        {
            get { return cbPrintBatch.SelectedValue.ToString(); }
        }

        public BatchSelectDialog(DataTable table)
        {
            InitializeComponent();
            cbPrintBatch.DataSource = table;
            cbPrintBatch.ValueMember = "BATCHINFO";
            cbPrintBatch.DisplayMember = "BATCHNAME";
        }
    }
}