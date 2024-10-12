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
    public partial class FormFullAuto : Form
    {
        int myMakeSceneCount, myTokenLength, mySceneCount;
        
        string myMovieText, myMovieSeed;

        public Boolean fullAutoFlag = false;
        public string autoType = "";

        private void button1_Click(object sender, EventArgs e)
        {
            fullAutoFlag = false;
            this.Close();
        }

        private void FromText_CheckedChanged(object sender, EventArgs e)
        {
            if (FromText.Checked)
            {
                autoType = "FromText";
                FromSeed.Checked = false;
                FromScenes.Checked = false;
            }
        }

        private void FromScenes_CheckedChanged(object sender, EventArgs e)
        {
            if (FromScenes.Checked)
            {
                autoType = "FromScenes";
                FromSeed.Checked = false;
                FromText.Checked = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!FromSeed.Checked && !FromText.Checked && !FromScenes.Checked)
            { 
                MessageBox.Show("No Full Auto Option Selected"); 
                
            }
            else
            {
                fullAutoFlag = true;
                this.Close();
            }    
            
            
        }

        public FormFullAuto(int makeSceneCount,  int sceneCount, string movieSeed, string movieText)
        {
            InitializeComponent();
            myMakeSceneCount = makeSceneCount;
            
            mySceneCount = sceneCount;
            this.StartPosition = FormStartPosition.CenterScreen;

            if (movieSeed.Trim().Length == 0 )
            {
                MessageBox.Show("a Movie Seed is required to run Full Auto");
                return;
            }

            if (movieText.Trim().Length == 0 )
            { 
                FromText.Enabled = false;
            }

            if (sceneCount == 0)
            {
                FromScenes.Enabled = false;
            }

        }

        private void FromSeed_CheckedChanged(object sender, EventArgs e)
        {
           if (FromSeed.Checked)
            {
                autoType = "FromSeed";
                FromText.Checked = false;
                FromScenes.Checked = false;
            }
        }

        private void FormFullAuto_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;

            label1.Text = "Full Auto will start at the specified location and will create as needed everything to complete a Movie Script ";
            label2.Text = "Any existing Scene Texts or Scripts will be deleted, as will the Scenes if you select either of the first two options."; 
            label3.Text = "The number of Scenes and length of Scene Seeds to create will use the values from the Movie Tab which are: ";
            label4.Text = $"Number of Scenes: {myMakeSceneCount}    Length of Scene Seeds in Tokens: {myTokenLength}";
            label5.Text = "Full Auto will require a long time to run. Depends on number of Scenes.  Could easily be two hours or more.";
            label6.Text = "When completed, use \"Collate\" to assemble full script.";
        }
    }
}
