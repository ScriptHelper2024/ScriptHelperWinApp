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
    public partial class FormCharacterDescription : Form
    {
        CharacterObj myCharacter;
        public string briefDescription = "";
        public Boolean successFlag = false;
        public FormCharacterDescription(CharacterObj workCharacter)
        {
            InitializeComponent();
            myCharacter = workCharacter;
        }

        private void FormCharacterDescription_Load(object sender, EventArgs e)
        {
            label1.Text = $"Provide brief description of character: {myCharacter.tagName}  age: {myCharacter.age}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            briefDescription = CharacterBriefDescription.Text;  
            successFlag= true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            successFlag= false;
            this.Close();
        }
    }
}
