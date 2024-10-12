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
    public partial class FormLargeView : Form
    {
        public FormLargeView(string originalText, string resultText, string analysisText, string title)
        {
            InitializeComponent();

            this.Text = title;

            this.originalRichTextBox.Text = originalText;
            this.resultRichTextBox.Text = resultText;
            this.analysisRichTextBox.Text = analysisText;
            this.originalRichTextBox.BackColor = Color.WhiteSmoke;
            this.originalRichTextBox.ForeColor = Color.Black;
            this.resultRichTextBox.BackColor = Color.WhiteSmoke;
            this.resultRichTextBox.ForeColor = Color.Black;
            this.analysisRichTextBox.BackColor = Color.WhiteSmoke;
            this.analysisRichTextBox.ForeColor = Color.Black;

            SetLayout();
        }

        private void SetLayout()
        {
            // the original box is anchor to the left, the result box is centered and the analysis box is anchored to the right
            // they all have the same width, which is 1/3rd of the window width minus the space between them
            this.SuspendLayout();

            this.originalRichTextBox.Anchor = AnchorStyles.None;
            this.resultRichTextBox.Anchor = AnchorStyles.None;
            this.analysisRichTextBox.Anchor = AnchorStyles.None;
            this.originalRichTextBox.Dock = DockStyle.None;
            this.resultRichTextBox.Dock = DockStyle.None;
            this.analysisRichTextBox.Dock = DockStyle.None;
            int spaceBetween = 10;
            int top = 30;
            int width = (this.Width - 5 * spaceBetween) / 3;
            int height = this.Height - 75 - top;

            // Set the size and position of each RichTextBox
            this.originalRichTextBox.Size = new Size(width, height);
            this.originalRichTextBox.Location = new Point(spaceBetween, top);
            this.label1.Location = new Point(spaceBetween, 10);

            this.resultRichTextBox.Size = new Size(width, height);
            this.resultRichTextBox.Location = new Point(2 * spaceBetween + width, top);
            this.label2.Location = new Point(2 * spaceBetween + width, 10);

            this.analysisRichTextBox.Size = new Size(width, height);
            this.analysisRichTextBox.Location = new Point(3 * spaceBetween + (2 * width), top);
            this.label3.Location = new Point(3 * spaceBetween + (2 * width), 10);

            this.ResumeLayout();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // swict the richtextbox background to dark grey and the text to white
            if (this.originalRichTextBox.BackColor == Color.Black)
            {
                this.originalRichTextBox.BackColor = Color.WhiteSmoke;
                this.originalRichTextBox.ForeColor = Color.Black;
                this.resultRichTextBox.BackColor = Color.WhiteSmoke;
                this.resultRichTextBox.ForeColor = Color.Black;
                this.analysisRichTextBox.BackColor = Color.WhiteSmoke;
                this.analysisRichTextBox.ForeColor = Color.Black;
                this.BackColor = Color.WhiteSmoke;
                this.label1.ForeColor = Color.Black;
                this.label2.ForeColor = Color.Black;
                this.label3.ForeColor = Color.Black;
            }
            else
            {
                this.originalRichTextBox.BackColor = Color.Black;
                this.originalRichTextBox.ForeColor = Color.WhiteSmoke;
                this.resultRichTextBox.BackColor = Color.Black;
                this.resultRichTextBox.ForeColor = Color.WhiteSmoke;
                this.analysisRichTextBox.BackColor = Color.Black;
                this.analysisRichTextBox.ForeColor = Color.WhiteSmoke;
                this.BackColor = Color.Black;
                this.label1.ForeColor = Color.WhiteSmoke;
                this.label2.ForeColor = Color.WhiteSmoke;
                this.label3.ForeColor = Color.WhiteSmoke;
            }
        }

        private void FormLargeView_Resize(object sender, EventArgs e)
        {
            SetLayout();
        }
    }
}
