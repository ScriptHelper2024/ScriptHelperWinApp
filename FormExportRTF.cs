using System;
using System.IO;
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
    public partial class FormExportRTF : Form
    {
        List<SceneObj> localScenes;
        Boolean fillFlag = false;
        string title;
        string JPRRTFScript = "";
        public FormExportRTF(List<SceneObj> myScenes, MovieObj myMovie)
        {
            InitializeComponent();
            localScenes = myScenes;
            this.StartPosition = FormStartPosition.CenterScreen;
            title = myMovie.title;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (JPRRTFScript.Length < 5)
            {
                MessageBox.Show("Need to generate script first");
                return;

            }
            
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                
                if (RTFCheck.Checked)
                {
                    saveFileDialog.Filter = "RTF Files (*.rtf)|*.rtf|All Files (*.*)|*.*";
                    saveFileDialog.DefaultExt = "rtf";
                }
                else
                {
                    saveFileDialog.Filter = "TXT Files (*.txt)|*.txt|All Files (*.*)|*.*";
                    saveFileDialog.DefaultExt = "txt";
                }
                

                


                saveFileDialog.Title = "Select a directory and file name to save";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    
                    if (RTFCheck.Checked)
                    {
                        // File.WriteAllText(saveFileDialog.FileName, ScriptBox.Rtf);
                        File.WriteAllText(saveFileDialog.FileName, JPRRTFScript);
                    }
                    else
                    {
                        File.WriteAllText(saveFileDialog.FileName, ScriptBox.Text);
                    }
                    
                    

                    this.Close();
                }
                else
                {
                    
                }
            }
        }

        private void FormExportRTF_Load(object sender, EventArgs e)
        {
            RTFCheck.Checked = true;
            TXTCheck.Checked = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<string> working = new List<string>();
            string work = "";
            working.Add(title);
            working.Add("");

            int sceneKount = 0;

            foreach (SceneObj scene in localScenes)
            {
                sceneKount = sceneKount + 1;
                if (scene.SceneScript.Trim().Length > 0)
                {

                    working.AddRange(Utils.cleanupScriptArray(scene.SceneScript));
                    working.Add("");
                }
                else if (FillScenes.Checked)
                {
                    working.Add("");
                    working.Add($"[No Script For Scene {sceneKount}: {scene.Title}]");
                    working.Add("");
                }

            }

            
            work = Utils.assembleFORMATTEDScriptFromArrayRTF(working);
            JPRRTFScript = work;

            ScriptBox.Rtf = work;
        }

        private void TXTCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (TXTCheck.Checked) { RTFCheck.Checked = false; }
        }

        private void RTFCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (RTFCheck.Checked) { TXTCheck.Checked = false;}
        }
    }
}
