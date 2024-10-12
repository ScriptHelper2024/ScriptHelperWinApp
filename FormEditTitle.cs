using System;
using System.Windows.Forms;

namespace ScriptHelper
{
    public partial class FormEditTitle : Form
    {
        string myTitle = "";
        string originalTitle = "";
        string gptModel = "";
        public string finalTitle = "";
        MovieObj myMovie;
        FormApp1 mainForm;
        Boolean isError = false;
        public FormEditTitle(MovieObj theMovie, string myGptModel,FormApp1 appForm)
        {
            InitializeComponent();
            myTitle = theMovie.title.Trim(); 
            originalTitle = theMovie.title.Trim();
            myMovie = theMovie;
            gptModel = myGptModel;
            mainForm = appForm;
        }

        private void FormEditTitle_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            EditTitleBox.Text = myTitle;
            EditTitleBox.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            finalTitle = "";
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            finalTitle = "";
            if (EditTitleBox.Text.Trim() != originalTitle)
            {
                finalTitle = EditTitleBox.Text.Trim();
            }
            this.Close();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            string reply;


            if (myMovie.movieText != null && myMovie.movieText.Length > 200)
            {
                MovieTitle.Text = $"making title from \"Movie Text\"using {gptModel} ...";
                reply = await MyGPT.getTitle(myMovie.movieText, gptModel, mainForm);
                

                reply = reply.Trim();
                reply = reply.Replace("Title:", "");
                reply = reply.Trim();

                MovieTitle.Text = reply;


            }
            else
            {
                MessageBox.Show("To generate a \"Movie Title\", a \"Movie Text\" of at least 200 characters long is required");
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            EditTitleBox.Text = MovieTitle.Text;
            EditTitleBox.Focus();
        }
    }
}
