using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace THOK.AS.Sorting.View
{
    public partial class ParamDialog : Form
    {
        public int UploadMode
        {
            get { return cbParam.SelectedIndex; }
        }

        public ParamDialog(int uploadMode)
        {
            InitializeComponent();
            cbParam.SelectedIndex = uploadMode;
        }
    }
}