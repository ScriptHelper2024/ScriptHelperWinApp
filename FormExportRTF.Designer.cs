namespace ScriptHelper
{
    partial class FormExportRTF
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
            this.ScriptBox = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.FillScenes = new System.Windows.Forms.CheckBox();
            this.RTFCheck = new System.Windows.Forms.CheckBox();
            this.TXTCheck = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ScriptBox
            // 
            this.ScriptBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScriptBox.Location = new System.Drawing.Point(35, 56);
            this.ScriptBox.Name = "ScriptBox";
            this.ScriptBox.Size = new System.Drawing.Size(437, 467);
            this.ScriptBox.TabIndex = 0;
            this.ScriptBox.Text = "";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(515, 56);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 46);
            this.button1.TabIndex = 2;
            this.button1.Text = "Save File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(515, 123);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(117, 41);
            this.button2.TabIndex = 3;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(35, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(136, 38);
            this.button3.TabIndex = 4;
            this.button3.Text = "Make Your Script";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FillScenes
            // 
            this.FillScenes.AutoSize = true;
            this.FillScenes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FillScenes.Location = new System.Drawing.Point(177, 22);
            this.FillScenes.Name = "FillScenes";
            this.FillScenes.Size = new System.Drawing.Size(345, 20);
            this.FillScenes.TabIndex = 5;
            this.FillScenes.Text = "Insert Scene Titles For Scenes Without Scripts";
            this.FillScenes.UseVisualStyleBackColor = true;
            // 
            // RTFCheck
            // 
            this.RTFCheck.AutoSize = true;
            this.RTFCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RTFCheck.Location = new System.Drawing.Point(518, 225);
            this.RTFCheck.Name = "RTFCheck";
            this.RTFCheck.Size = new System.Drawing.Size(119, 20);
            this.RTFCheck.TabIndex = 0;
            this.RTFCheck.Text = "RTF for Word";
            this.RTFCheck.UseVisualStyleBackColor = true;
            this.RTFCheck.CheckedChanged += new System.EventHandler(this.RTFCheck_CheckedChanged);
            // 
            // TXTCheck
            // 
            this.TXTCheck.AutoSize = true;
            this.TXTCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTCheck.Location = new System.Drawing.Point(518, 251);
            this.TXTCheck.Name = "TXTCheck";
            this.TXTCheck.Size = new System.Drawing.Size(152, 20);
            this.TXTCheck.TabIndex = 1;
            this.TXTCheck.Text = "TXT for Final Draft";
            this.TXTCheck.UseVisualStyleBackColor = true;
            this.TXTCheck.CheckedChanged += new System.EventHandler(this.TXTCheck_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(515, 192);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "File Format:";
            // 
            // FormExportRTF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 552);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TXTCheck);
            this.Controls.Add(this.RTFCheck);
            this.Controls.Add(this.FillScenes);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ScriptBox);
            this.Name = "FormExportRTF";
            this.Text = "FormExportRTF";
            this.Load += new System.EventHandler(this.FormExportRTF_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox ScriptBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox FillScenes;
        private System.Windows.Forms.CheckBox RTFCheck;
        private System.Windows.Forms.CheckBox TXTCheck;
        private System.Windows.Forms.Label label1;
    }
}