using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScriptHelper
{
    public partial class FormCopyEdit : Form
    {
        public FormCopyEdit(FormApp1 formApp1)
        {
            InitializeComponent();
            _formApp1 = formApp1;
        }

        private FormApp1 _formApp1;

        private void enrichenRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (enrichenRadioButton.Checked)
                depurpleRadioButton.Checked = false;
        }

        private void depurpleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (depurpleRadioButton.Checked)
                enrichenRadioButton.Checked = false;
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            // close and run CopyEdit(type) in formapp1
            runButton.Enabled = false;
            if (enrichenRadioButton.Checked)
                _formApp1.CopyEdit("enrichen");
            else if (depurpleRadioButton.Checked)
                _formApp1.CopyEdit("depurple");
            this.Close();
        }
    }
}
