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
    public partial class FormMovieTextMagic : Form
    {
        public List<string> criticFactors = new List<string>();

        public FormMovieTextMagic()
        {
            InitializeComponent();
        }

        private void FormMovieTextMagic_Load(object sender, EventArgs e)
        {
            unCheckAll();

            foreach (Control control in this.Controls)
            {

                if (control is CheckBox)
                {
                    control.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
                }

                
            }
            
            
        }

        private void unCheckAll()
        {
            foreach (Control control in this.Controls)
            {
                if (control is CheckBox)
                {
                    ((CheckBox)control).Checked = false;
                }
            }
        }

        private void checkAll()
        {
            foreach (Control control in this.Controls)
            {
                if (control is CheckBox)
                {
                    ((CheckBox)control).Checked = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {


            foreach (Control control in this.Controls)
            {
                if (control is CheckBox)
                {
                    criticFactors.Add(control.Name);
                }

            }

            Utils.magicMovieNoteFlag = true;
            this.Close();

        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            unCheckAll();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            checkAll();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Utils.magicMovieNoteFlag = false;
            this.Close();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            CheckBox checkBox = new CheckBox();
            int checkBoxCount = 0;
            foreach (Control control in this.Controls)
            {
                if (control is CheckBox)
                {
                    checkBox = (CheckBox)control;
                    if (checkBox.Checked == true)
                    {
                        checkBoxCount++;
                        criticFactors.Add(control.Name);
                    }
                }
            }
            if (checkBoxCount > 5)
            {
                MessageBox.Show("Please select no more than five areas of focus.");
                return;
            }

            Utils.magicMovieNoteFlag = true;
            this.Close();
        }
    }
}
