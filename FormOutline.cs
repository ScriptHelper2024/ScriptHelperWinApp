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
    public partial class FormOutline : Form
    {
        public FormOutline(string text, FormApp1 formApp1)
        {
            InitializeComponent();
            outlineTextBox.Text = text;
            _formApp1 = formApp1;
        }

        private FormApp1 _formApp1;

        private void copyOutlineButton_Click(object sender, EventArgs e)
        {
            // copy text to clipboard
            Clipboard.SetText(outlineTextBox.Text);
        }

        private void replaceMovieSeedButton_Click(object sender, EventArgs e)
        {
            // replace the movie seed text in formapp1
            _formApp1.SetMovieHintText(outlineTextBox.Text);
        }
    }
}
