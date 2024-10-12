namespace ScriptHelper
{
    partial class FormCollate
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
            this.CollateBox = new System.Windows.Forms.RichTextBox();
            this.DoMovieText = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.DoSceneText = new System.Windows.Forms.CheckBox();
            this.DoSceneHint = new System.Windows.Forms.CheckBox();
            this.DoSceneScript = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.StandardCollate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CollateBox
            // 
            this.CollateBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CollateBox.Location = new System.Drawing.Point(555, 70);
            this.CollateBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CollateBox.Name = "CollateBox";
            this.CollateBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.CollateBox.Size = new System.Drawing.Size(1105, 719);
            this.CollateBox.TabIndex = 0;
            this.CollateBox.Text = "";
            // 
            // DoMovieText
            // 
            this.DoMovieText.AutoSize = true;
            this.DoMovieText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DoMovieText.Location = new System.Drawing.Point(39, 129);
            this.DoMovieText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DoMovieText.Name = "DoMovieText";
            this.DoMovieText.Size = new System.Drawing.Size(217, 29);
            this.DoMovieText.TabIndex = 4;
            this.DoMovieText.Text = "Include Movie Text";
            this.DoMovieText.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(21, 480);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(477, 69);
            this.button1.TabIndex = 1;
            this.button1.Text = "Detailed Collate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DoSceneText
            // 
            this.DoSceneText.AutoSize = true;
            this.DoSceneText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DoSceneText.Location = new System.Drawing.Point(21, 665);
            this.DoSceneText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DoSceneText.Name = "DoSceneText";
            this.DoSceneText.Size = new System.Drawing.Size(145, 29);
            this.DoSceneText.TabIndex = 5;
            this.DoSceneText.Text = "Scene Text";
            this.DoSceneText.UseVisualStyleBackColor = true;
            // 
            // DoSceneHint
            // 
            this.DoSceneHint.AutoSize = true;
            this.DoSceneHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DoSceneHint.Location = new System.Drawing.Point(21, 628);
            this.DoSceneHint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DoSceneHint.Name = "DoSceneHint";
            this.DoSceneHint.Size = new System.Drawing.Size(140, 29);
            this.DoSceneHint.TabIndex = 6;
            this.DoSceneHint.Text = "Scene Hint";
            this.DoSceneHint.UseVisualStyleBackColor = true;
            // 
            // DoSceneScript
            // 
            this.DoSceneScript.AutoSize = true;
            this.DoSceneScript.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DoSceneScript.Location = new System.Drawing.Point(21, 702);
            this.DoSceneScript.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.DoSceneScript.Name = "DoSceneScript";
            this.DoSceneScript.Size = new System.Drawing.Size(158, 29);
            this.DoSceneScript.TabIndex = 7;
            this.DoSceneScript.Text = "Scene Script";
            this.DoSceneScript.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 576);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 29);
            this.label1.TabIndex = 8;
            this.label1.Text = "For Each Scene:";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(39, 70);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(157, 47);
            this.button2.TabIndex = 9;
            this.button2.Text = "Exit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // StandardCollate
            // 
            this.StandardCollate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StandardCollate.Location = new System.Drawing.Point(39, 166);
            this.StandardCollate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.StandardCollate.Name = "StandardCollate";
            this.StandardCollate.Size = new System.Drawing.Size(459, 75);
            this.StandardCollate.TabIndex = 10;
            this.StandardCollate.Text = "Standard Collate";
            this.StandardCollate.UseVisualStyleBackColor = true;
            this.StandardCollate.Click += new System.EventHandler(this.StandardCollate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(35, 256);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(419, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "One item per scene by priority: Script, Text, Hint";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(1557, 15);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(103, 48);
            this.button3.TabIndex = 12;
            this.button3.Text = "Copy";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(35, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(860, 22);
            this.label3.TabIndex = 13;
            this.label3.Text = "Collate uses the most recently created version of Movie Texts, Scene Texts, and S" +
    "cene Scripts";
            // 
            // FormCollate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1697, 805);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.StandardCollate);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DoSceneScript);
            this.Controls.Add(this.DoSceneHint);
            this.Controls.Add(this.DoSceneText);
            this.Controls.Add(this.DoMovieText);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CollateBox);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormCollate";
            this.Text = "FormCollate";
            this.Load += new System.EventHandler(this.FormCollate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox CollateBox;
        private System.Windows.Forms.CheckBox DoMovieText;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox DoSceneText;
        private System.Windows.Forms.CheckBox DoSceneHint;
        private System.Windows.Forms.CheckBox DoSceneScript;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button StandardCollate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label3;
    }
}