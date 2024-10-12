using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScriptHelper
{
    public partial class FormFork : Form
    {
        string oldMovieTitle = "";
        public FormFork(MovieObj myMovie)
        {
            InitializeComponent();

            oldMovieTitle = myMovie.title.Trim();

            label1.Text = "\"Fork\" creates a copy of the Current Movie with a new title.";
            label2.Text = $"Current Movie title: {myMovie.title}";
            NewName.Text = $"{myMovie.title} Forked at {getTime()}";

        }

        private void FormFork_Load(object sender, EventArgs e)
        {
            NewName.Focus();
        }

        private string getTime()
        {
            string currentTime = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
            return currentTime;
        }

        private void Fork_Click(object sender, EventArgs e)
        {

            if (oldMovieTitle == NewName.Text.Trim())
            {
                MessageBox.Show("new name must not be the same as old name");
                this.Close();
            }
            
            Utils.forkTitle = NewName.Text;
            Utils.fork = true;
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Utils.fork = false;
            this.Close();
        }
    }
}
