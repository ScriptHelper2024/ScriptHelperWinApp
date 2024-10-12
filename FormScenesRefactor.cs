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
    public partial class FormScenesRefactor : Form
    {
        int originalSceneCount = 0;
        int originalTokenCount = 0;
        public FormScenesRefactor()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            
        }

        private void FormScenesRefactor_Load(object sender, EventArgs e)
        {
            SceneCount.Value = Utils.currentSceneCount;
            TokensInHintText.Value = Utils.currentTokenCount;

            originalSceneCount = Utils.currentSceneCount;
            originalTokenCount = Utils.currentTokenCount;
            
            RefactorSummary.Text = $"Refactor Scenes from Scene #{Utils.currentSceneNumber + 1} to Scene #{Utils.currentSceneCount} \r\n \r\n";
            RefactorSummary.Text += $"Refactoring will erase all Scene Narratives, Notes, and Scripts for Scenes #{Utils.currentSceneNumber + 1} to ";
            RefactorSummary.Text += $" Scene #{Utils.currentSceneCount}";
            
            IncludeMovieText.Checked = true;
            Utils.refactor = false;
            Utils.refactorMovieText = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void SceneCount_ValueChanged(object sender, EventArgs e)
        {
            Utils.currentSceneCount = (int)SceneCount.Value;
            RefactorSummary.Text = $"Refactor Scenes from Scene #{Utils.currentSceneNumber + 1} to Scene #{Utils.currentSceneCount} \r\n \r\n";
            RefactorSummary.Text += $"Refactoring will erase all Scene Narratives, Notes, and Scripts for Scenes #{Utils.currentSceneNumber + 1} to ";
            RefactorSummary.Text += $" Scene #{Utils.currentSceneCount}";

        }
    

        private void TokensInHintText_ValueChanged(object sender, EventArgs e)
        {
            Utils.currentTokenCount = (int)TokensInHintText.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Utils.refactor = false;
            Utils.currentSceneCount = originalSceneCount;
            Utils.currentTokenCount = originalTokenCount;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Utils.refactor = true;
            
            if (IncludeMovieText.Checked == true)
            {
                Utils.refactorMovieText = true;
            }
            else
            {
                Utils.refactorMovieText = false;
            }
            
            this.Close();
        }
    }
}
