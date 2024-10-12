using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ScriptHelper
{
    public partial class FormRTB : Form
    {
        string myText = "";
        public FormRTB(string label,string text)
        {
            InitializeComponent();

            // enable cut and paste in Richtextboxes

            Utils.EnableFormRightClickForCutPaste(this);

            label1.Text = label;
            myText = text;
        }

        private void FormRTB_Load(object sender, EventArgs e)
        {
            RTBDisplay.Text = myText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
