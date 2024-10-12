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
    public partial class FormNewMovie : Form
    {
        public MovieObj newMovie;
        public Boolean abortFlag = true;
        List<string> ratingsUSA = new List<string>();
        public FormNewMovie()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            Utils.EnableFormRightClickForCutPaste(this);
            
            newMovie = new MovieObj();

            ratingsUSA = Utils.getUSARatings();

            USARatings.DataSource = ratingsUSA;
            USARatings.SelectedIndex = 0;
            newMovie.ratingUSA = (string)USARatings.SelectedItem;
        }

        private void FormNewMovie_Load(object sender, EventArgs e)
        {

        }

        private void TimeLength_ValueChanged(object sender, EventArgs e)
        {
            newMovie.timeLength = (int)TimeLength.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            abortFlag = true;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Title.Text.Trim().Length < 5)
            {
                MessageBox.Show("Please enter a title for the movie that is at least 5 characters long");
                return;
            }
            abortFlag = false;
            this.Close();

        }

        private void Title_TextChanged(object sender, EventArgs e)
        {
            newMovie.title = (string)Title.Text;
        }

        private void Genre_TextChanged(object sender, EventArgs e)
        {
            newMovie.genre = (string)Genre.Text;
        }

        private void Audience_TextChanged(object sender, EventArgs e)
        {
            newMovie.audience = (string)Audience.Text;
        }

        private void Guidance_TextChanged(object sender, EventArgs e)
        {
            newMovie.guidance = (string)Guidance.Text;
        }

        private void Guidance_TextChanged_1(object sender, EventArgs e)
        {
            newMovie.guidance = (string)Guidance.Text;
        }

        private void USARatings_SelectedIndexChanged(object sender, EventArgs e)
        {
            newMovie.ratingUSA = (string)USARatings.SelectedItem;
        }
    }
}
