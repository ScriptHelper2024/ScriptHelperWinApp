using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Serialization;

namespace ScriptHelper
{
    public partial class FormDumpJSON : Form
    {
        public Boolean dumpJSONFlag = false;
        private FolderBrowserDialog folderBrowserDialog;
        private string jsonString;
        private string dumpFile;
        public FormDumpJSON()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            folderBrowserDialog = new FolderBrowserDialog()
            {
                Description = "Select a directory in which to save JSON Dump Files",
                RootFolder = Environment.SpecialFolder.MyComputer,
                ShowNewFolderButton = true
            };

        }

        private void button2_Click(object sender, EventArgs e)
        {
            dumpJSONFlag = false;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Utils.checkDumpPWD(DumpPWD.Text.Trim()))
            {
                dumpJSONFlag = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect password");
            }
        }
    }
}
