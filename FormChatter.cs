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
    public partial class FormChatter : Form
    {
        private string GptModel { get; set; }
        private string SystemPrompt { get; set; }
        private string ChatTypeString { get; set; }
        private FormApp1 FormApp1 { get; set; }
        private List<ChatMessageModel> ChatMessages { get; set; }

        public FormChatter(string gptModel, string systemPrompt, string chatTypeString, FormApp1 formApp1)
        {
            InitializeComponent();
            SystemPromptBox.Text = systemPrompt;
            SystemPrompt = systemPrompt;
            GptModel = gptModel;
            ChatTypeString = chatTypeString;
            FormApp1 = formApp1;
            ChatMessages = new List<ChatMessageModel>
            {
                new ChatMessageModel
                {
                    Content = systemPrompt,
                    Role = "system"
                }
            };

            // capture the keystrokes entered into the user message text box, if it is enter, then send the message, if ctrl+enter, then add a new line
            UserMessageTextBox.KeyDown += new KeyEventHandler(UserMessageTextBox_KeyDown);

            // focus the user message text box
            UserMessageTextBox.Focus();

            // set the form window title to the chat type string
            this.Text = "Chat with the " + chatTypeString;
        }

        private void UserMessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Control)
            {
                UserMessageTextBox.AppendText("\r\n");
            }
            else if (e.KeyCode == Keys.Enter)
            {
                SendMessageButton_Click(sender, e);
            }
        }

        private async void SendMessageButton_Click(object sender, EventArgs e)
        {
            ChatHistoryRTB.AppendText($"YOU: ");
            ChatHistoryRTB.AppendText($"{UserMessageTextBox.Text}");
            ChatHistoryRTB.AppendText("\r\n");
            ChatHistoryRTB.AppendText("\r\n");
            ChatHistoryRTB.AppendText("================================================================================================");
            ChatHistoryRTB.AppendText("\r\n");
            ChatHistoryRTB.AppendText("\r\n");
            ChatHistoryRTB.AppendText("Waiting for response...");

            ChatMessages.Add(new ChatMessageModel
            {
                Content = UserMessageTextBox.Text,
                Role = "user"
            });

            // scroll to bottom
            ChatHistoryRTB.SelectionStart = ChatHistoryRTB.Text.Length;

            // set the send message button to disabled until we get a response
            SendMessageButton.Enabled = false;
            UserMessageTextBox.Enabled = false;
            UserMessageTextBox.Text = "";
            // refresh the form so that the button is disabled
            Refresh();

            var response = await MyGPT.SendChatterMessage("ChatMovieText", GptModel, ChatMessages, FormApp1);
            ChatMessages.Add(new ChatMessageModel
            {
                Content = response,
                Role = "system"
            });
            ChatHistoryRTB.AppendText("\r\n");
            ChatHistoryRTB.AppendText("\r\n");
            ChatHistoryRTB.AppendText("SYSTEM: ");
            ChatHistoryRTB.AppendText($"{response}");
            ChatHistoryRTB.AppendText("\r\n");
            ChatHistoryRTB.AppendText("\r\n");
            ChatHistoryRTB.AppendText("================================================================================================");
            ChatHistoryRTB.AppendText("\r\n");
            ChatHistoryRTB.AppendText("\r\n");

            // scroll to bottom
            ChatHistoryRTB.SelectionStart = ChatHistoryRTB.Text.Length;

            // set the send message button to enabled
            SendMessageButton.Enabled = true;
            UserMessageTextBox.Enabled = true;
        }

        private void FormChatter_Load(object sender, EventArgs e)
        {
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ChatMessages = new List<ChatMessageModel>
            {
                new ChatMessageModel
                {
                    Content = SystemPrompt,
                    Role = "system"
                }
            };
            ChatHistoryRTB.Text = "";
        }

        private void SystemPromptBox_TextChanged(object sender, EventArgs e)
        {
            SystemPrompt = SystemPromptBox.Text;
            // replace the first message in the chat messages list with the new system prompt
            ChatMessages[0] = new ChatMessageModel
            {
                Content = SystemPrompt,
                Role = "system"
            };
        }
    }
}
