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
    public partial class FormCollate : Form
    {
        List<SceneObj> myScenes;
        MovieObj myMovie;
        public FormCollate(List<SceneObj> inScenes, MovieObj inMovie)
        {
            InitializeComponent();

            // enable cut and paste in Richtextboxes

            Utils.EnableFormRightClickForCutPaste(this);

            myScenes = inScenes;
            myMovie = inMovie;

        }

        private void FormCollate_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            DoMovieText.Checked = true;
            DoSceneHint.Checked = true;
            DoSceneText.Checked = true;
            DoSceneScript.Checked = true;
           
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            doDetailedCollate(1);
        }

        private void doDetailedCollate(int box1)
        {
            int sceneNumber = 1;
            string collateBoxText = "";

            if (DoMovieText.Checked)
            {

                collateBoxText += "Movie Text\r\n\r\n";
                collateBoxText += myMovie.movieText;
                collateBoxText += "\r\n";

            }

            collateBoxText += "Scenes: \r\n\r\n";

            foreach (SceneObj scene in myScenes)
            {
                if (anySceneChecked()) collateBoxText += $"Scene {sceneNumber}: " + scene.Title + "\r\n\r\n";

                sceneNumber++;

                if (DoSceneHint.Checked)
                {
                    if (scene.Hint.Length > 0)
                    {

                        collateBoxText += "Hint: \r\n\r\n";
                        collateBoxText += scene.Hint + "\r\n\r\n";
                    }
                    else
                    {
                        collateBoxText += "No Scene Hint\r\n\r\n";
                    }

                }

                if (DoSceneText.Checked)
                {
                    if (scene.NarrativeText.Length > 0)
                    {

                        collateBoxText += "Text: \r\n\r\n";
                        collateBoxText += scene.NarrativeText + "\r\n\r\n";
                    }
                    else
                    {
                        collateBoxText += "No Scene Text\r\n\r\n";
                    }

                }

                if (DoSceneScript.Checked)
                {
                    if (scene.SceneScript.Length > 0)
                    {

                        collateBoxText += "Script: \r\n\r\n";
                        collateBoxText += scene.SceneScript + "\r\n\r\n";
                    }
                    else
                    {
                        collateBoxText += "No Scene Script\r\n\r\n";
                    }

                }

            }
               

            CollateBox.Text = collateBoxText;

        }

        private void doStandardCollate()
        {
            string collateBoxText = "";
            int sceneNumber = 1;
            Boolean doFlag = true;
            foreach (SceneObj scene in myScenes)
            {
                collateBoxText += $"Scene {sceneNumber}: " + scene.Title + "\r\n\r\n";

                sceneNumber++;
                doFlag = true;

                if (scene.SceneScript.Trim().Length > 0 && doFlag)
                {
                    
                    collateBoxText += "Scene Script: " + scene.SceneScript + "\r\n\r\n";
                    
                    doFlag = false;

                }

                if (scene.NarrativeText.Trim().Length > 0 && doFlag)
                {
                    
                        
                   collateBoxText += "Scene Text: " + scene.NarrativeText + "\r\n\r\n";
                   doFlag = false;
                }

                if (scene.Hint.Trim().Length > 0 && doFlag)
                {
                   
                    collateBoxText += "Scene Hint: " + scene.Hint + "\r\n\r\n";
                    doFlag = false;
                    
                }

                

                

            }

            CollateBox.Text += collateBoxText;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private Boolean anySceneChecked()
        {
            Boolean anyChecked = false;

            
            if (DoSceneHint.Checked) { anyChecked = true; }
            if (DoSceneText.Checked) { anyChecked = true; } 
            if (DoSceneScript.Checked) { anyChecked = true; }

            return anyChecked;

        }

        private void StandardCollate_Click(object sender, EventArgs e)
        {
            doStandardCollate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            copyTextRTB(CollateBox);
        }

        private void copyTextRTB(RichTextBox myBox)
        {
            if (!string.IsNullOrEmpty(myBox.Text))
            {
                Clipboard.SetText(myBox.Text);
            }

        }
    }
}
