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
    public partial class FormEnlargeText : Form
    {
        
        public string finalText { get; set; } = "";

        string boxText = "";
        public FormEnlargeText(string text)
        {
            InitializeComponent();

            // enable cut and paste in Richtextboxes

            Utils.EnableFormRightClickForCutPaste(this);

            this.StartPosition = FormStartPosition.CenterScreen;
            boxText = text;
        }

        private void FormEnlargeText_Load(object sender, EventArgs e)
        {
            BigTextBox.Rtf = boxText;  
            // BigTextBox.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Utils.enlargeScript = false;
            
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Utils.enlargeScript = true;
            // finalText = BigTextBox.Rtf;

            finalText = BigTextBox.Text;
            this.Close ();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(BigTextBox.Text))
            {
                Clipboard.SetText(BigTextBox.Text);
            }
        }
    }
}
