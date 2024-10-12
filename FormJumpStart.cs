using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScriptHelper
{
    public partial class FormJumpStart : Form
    {
        string myMovieHint = "";
        public string myMovieText = "";
        public int loopCount;
        FormApp1 myForm;
        public int lengthMovieText;
        public FormJumpStart(string movieHint, string movieText, FormApp1 formApp1)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            myMovieHint = movieHint;
            myMovieText = movieText;
            lengthMovieText = myMovieText.Trim().Length;
            myForm = formApp1;
        }

        private void FormJumpStart_Load(object sender, EventArgs e)
        {
            loopCount = (int)LoopCount.Value;

            if (lengthMovieText == 0) { button3.Visible = false; } else { button3.Visible = true; }
            Utils.jumpStartFlag = false;
            if (myMovieHint.Length < 25 && myMovieText.Length < 25)
            {
                MessageBox.Show("To run Movie Jump Start, your Movie Hint or Movie Text must be at least 25 characters long");
                this.Close();
            }

                        
            if (lengthMovieText == 0)
            {
                MyBox.Text = "Jump Start is a quick way to get started with a script. \r\n\r\nIt will take your Movie Seed and do the following:\r\n\r\n";
                MyBox.Text += "1. Generate a Movie Text\r\n\r\n";
                MyBox.Text += $"2. Executes a loop {LoopCount.Value} times that generates a Magic Movie Note and then Applies it to the Movie Text\r\n\r\n";
                MyBox.Text += "3. Return a new Movie Text\r\n\r\n";
                MyBox.Text += "You can modiy the number of times the loop runs by changing the number in the box below.\r\n\r\n";
                MyBox.Text += "Jump Start will take roughly 2 minutes per loop.\r\n\r\n";
                MyBox.Text += "Return a new Movie Text";
            }
            else
            {
                MyBox.Text = "Jump Start is a quick way to get started with a script. \r\n\r\nIt will take your Movie Text and do the following:\r\n\r\n";
                MyBox.Text += "1. Start with your current Movie Text\r\n\r\n";
                MyBox.Text += $"2. Executes a loop {LoopCount.Value} times that generates a Magic Movie Note and then Applies it to the Movie Text\r\n\r\n";
                MyBox.Text += "3. Return a new Movie Text\r\n\r\n";
                MyBox.Text += "You can modiy the number of times the loop runs by changing the number in the box below.\r\n\r\n";
                MyBox.Text += "Jump Start will take roughly 2 minutes per loop.\r\n\r\n";
                
            }
            
        }

        private void LoopCount_ValueChanged(object sender, EventArgs e)
        {
            loopCount = (int)LoopCount.Value;

            if (lengthMovieText == 0)
            {
                MyBox.Text = "Jump Start is a quick way to get started with a script. \r\n\r\nIt will take your Movie Seed and do the following:\r\n\r\n";
                MyBox.Text += "1. Generate a Movie Text\r\n\r\n";
                MyBox.Text += $"2. Executes a loop {LoopCount.Value} times that generates a Magic Movie Note and then Applies it to the Movie Text\r\n\r\n";
                MyBox.Text += "3. Return a new Movie Text\r\n\r\n";
                MyBox.Text += "You can modiy the number of times the loop runs by changing the number in the box below.\r\n\r\n";
                MyBox.Text += "Jump Start will take roughly 2 minutes per loop.\r\n\r\n";
               
            }
            else
            {
                MyBox.Text = "Jump Start is a quick way to get started with a script. \r\n\r\nIt will take your Movie Text and do the following:\r\n\r\n";
                MyBox.Text += "1. Start with your current Movie Text\r\n\r\n";
                MyBox.Text += $"2. Executes a loop {LoopCount.Value} times that generates a Magic Movie Note and then Applies it to the Movie Text\r\n\r\n";
                MyBox.Text += "3. Return a new Movie Text\r\n\r\n";
                MyBox.Text += "You can modiy the number of times the loop runs by changing the number in the box below.\r\n\r\n";
                MyBox.Text += "Jump Start will take roughly 2 minutes per loop.\r\n\r\n";
                
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
             
            

            Utils.jumpStartFlag = true;
            this.Close();
        }

        private void MyBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            myForm.clearMovieText();
            myMovieText = "";
            lengthMovieText = 0;
            button3.Visible = false;

            MyBox.Text = "Jump Start is a quick way to get started with a script. \r\n\r\nIt will take your Movie Seed and do the following:\r\n\r\n";
            MyBox.Text += "1. Generate a Movie Text\r\n\r\n";
            MyBox.Text += $"2. Executes a loop {LoopCount.Value} times that generates a Magic Movie Note and then Applies it to the Movie Text\r\n\r\n";
            MyBox.Text += "3. Return a new Movie Text\r\n\r\n";
            MyBox.Text += "You can modiy the number of times the loop runs by changing the number in the box below.\r\n\r\n";
            MyBox.Text += "Jump Start will take roughly 2 minutes per loop.\r\n\r\n";
            MyBox.Text += "Return a new Movie Text";
        }
    }
}
