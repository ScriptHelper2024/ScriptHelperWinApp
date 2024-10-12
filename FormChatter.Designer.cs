namespace ScriptHelper
{
    partial class FormChatter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ChatHistoryRTB = new System.Windows.Forms.RichTextBox();
            this.UserMessageTextBox = new System.Windows.Forms.TextBox();
            this.SendMessageButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.SystemPromptBox = new System.Windows.Forms.RichTextBox();
            this.ResetButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ChatHistoryRTB
            // 
            this.ChatHistoryRTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChatHistoryRTB.BackColor = System.Drawing.Color.White;
            this.ChatHistoryRTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.ChatHistoryRTB.Location = new System.Drawing.Point(3, 0);
            this.ChatHistoryRTB.Name = "ChatHistoryRTB";
            this.ChatHistoryRTB.ReadOnly = true;
            this.ChatHistoryRTB.Size = new System.Drawing.Size(892, 791);
            this.ChatHistoryRTB.TabIndex = 1;
            this.ChatHistoryRTB.Text = "";
            // 
            // UserMessageTextBox
            // 
            this.UserMessageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UserMessageTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UserMessageTextBox.Location = new System.Drawing.Point(12, 809);
            this.UserMessageTextBox.Multiline = true;
            this.UserMessageTextBox.Name = "UserMessageTextBox";
            this.UserMessageTextBox.Size = new System.Drawing.Size(1094, 81);
            this.UserMessageTextBox.TabIndex = 0;
            // 
            // SendMessageButton
            // 
            this.SendMessageButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SendMessageButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.SendMessageButton.Location = new System.Drawing.Point(1112, 809);
            this.SendMessageButton.Name = "SendMessageButton";
            this.SendMessageButton.Size = new System.Drawing.Size(123, 80);
            this.SendMessageButton.TabIndex = 2;
            this.SendMessageButton.Text = "Send Message";
            this.SendMessageButton.UseVisualStyleBackColor = true;
            this.SendMessageButton.Click += new System.EventHandler(this.SendMessageButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.SystemPromptBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ChatHistoryRTB);
            this.splitContainer1.Size = new System.Drawing.Size(1353, 791);
            this.splitContainer1.SplitterDistance = 451;
            this.splitContainer1.TabIndex = 3;
            // 
            // SystemPromptBox
            // 
            this.SystemPromptBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SystemPromptBox.Location = new System.Drawing.Point(0, 0);
            this.SystemPromptBox.Name = "SystemPromptBox";
            this.SystemPromptBox.Size = new System.Drawing.Size(448, 791);
            this.SystemPromptBox.TabIndex = 0;
            this.SystemPromptBox.Text = "";
            this.SystemPromptBox.TextChanged += new System.EventHandler(this.SystemPromptBox_TextChanged);
            // 
            // ResetButton
            // 
            this.ResetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ResetButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.ResetButton.Location = new System.Drawing.Point(1241, 809);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(123, 80);
            this.ResetButton.TabIndex = 4;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // FormChatter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1377, 902);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.SendMessageButton);
            this.Controls.Add(this.UserMessageTextBox);
            this.Name = "FormChatter";
            this.Text = "FormChatter";
            this.Load += new System.EventHandler(this.FormChatter_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox ChatHistoryRTB;
        private System.Windows.Forms.TextBox UserMessageTextBox;
        private System.Windows.Forms.Button SendMessageButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RichTextBox SystemPromptBox;
        private System.Windows.Forms.Button ResetButton;
    }
}