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
    public partial class FormBigBox : Form
    {
        public FormBigBox(string text, bool canEdit, RichTextBox original)
        {
            InitializeComponent();
            richTextBox1.Text = text;
            richTextBox1.ReadOnly = !canEdit;
            this.original = original;
            this.canEdit = canEdit;
        }

        private RichTextBox original;
        private bool canEdit;

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (canEdit)
                original.Text = richTextBox1.Text;
        }
    }
}
