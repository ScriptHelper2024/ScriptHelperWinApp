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
    public partial class FormAddScene : Form
    {
        int startIndex = 0;
        public SceneObj newScene { get; set; }  = null;
        public Boolean added { get; set; } = false;

        List<SceneObj> myScenesList = null;
        public FormAddScene(int index, List<SceneObj> scenesList)
        {
            InitializeComponent();

            // enable cut and paste in Richtextboxes

            Utils.EnableFormRightClickForCutPaste(this);

            startIndex = index;
            myScenesList = scenesList;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void FormAddScene_Load(object sender, EventArgs e)
        {
            label1.Text = $"Adding a new Scene after Scene #{startIndex + 1}:\r\n\r\n{myScenesList[startIndex].Title}";



        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (SceneTitle.Text.Trim().Length > 5 && SceneHint.Text.Trim().Length > 20)
            {
                newScene = new SceneObj();
                newScene.Title = SceneTitle.Text;
                newScene.Hint = SceneHint.Text;
                added = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Title must have at least 5 characters and Hint 20 characters");

            }


        }
    }
}
