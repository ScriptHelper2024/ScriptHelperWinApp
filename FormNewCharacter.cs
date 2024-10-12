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
    public partial class FormNewCharacter : Form
    {
        public string firstName { get; set; } = string.Empty;

        public int age { get; set; } = -1;

        public string description { get; set; } = string.Empty;

        public Boolean successFlag = false;
        public FormNewCharacter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            if (Immortal.Checked) { age = 10000; }

            if (FirstNameTextbox.Text.Length < 2)
            {
                MessageBox.Show("Need to enter a first name with at least 2 characters long");
                return;
            }

            if (NewCharacterDescription.Text.Length < 10)
            {
                MessageBox.Show("Brief Description must be at least 10 characters long");
                return;
            }
           

            if ((int)CharacterAge.Value <= 0)
            {
                MessageBox.Show("Age must be greater than zero");
                return;
            }

            successFlag= true;

            firstName = FirstNameTextbox.Text;

            age = (int)CharacterAge.Value;

            description = NewCharacterDescription.Text.Trim();

            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            successFlag= false;
            this.Close();
        }

        private void FormNewCharacter_Load(object sender, EventArgs e)
        {
            Immortal.Checked = false;
        }

        private void CharacterAge_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
