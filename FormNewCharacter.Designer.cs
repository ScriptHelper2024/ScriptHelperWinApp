namespace ScriptHelper
{
    partial class FormNewCharacter
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
            this.FirstNameTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CharacterAge = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.Immortal = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.NewCharacterDescription = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.CharacterAge)).BeginInit();
            this.SuspendLayout();
            // 
            // FirstNameTextbox
            // 
            this.FirstNameTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FirstNameTextbox.Location = new System.Drawing.Point(119, 81);
            this.FirstNameTextbox.Margin = new System.Windows.Forms.Padding(2);
            this.FirstNameTextbox.Name = "FirstNameTextbox";
            this.FirstNameTextbox.Size = new System.Drawing.Size(130, 26);
            this.FirstNameTextbox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(34, 85);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "FIrst Name";
            // 
            // CharacterAge
            // 
            this.CharacterAge.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CharacterAge.Location = new System.Drawing.Point(119, 132);
            this.CharacterAge.Margin = new System.Windows.Forms.Padding(2);
            this.CharacterAge.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.CharacterAge.Name = "CharacterAge";
            this.CharacterAge.Size = new System.Drawing.Size(96, 26);
            this.CharacterAge.TabIndex = 2;
            this.CharacterAge.ValueChanged += new System.EventHandler(this.CharacterAge_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(79, 132);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Age";
            // 
            // Immortal
            // 
            this.Immortal.AutoSize = true;
            this.Immortal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Immortal.Location = new System.Drawing.Point(241, 132);
            this.Immortal.Margin = new System.Windows.Forms.Padding(2);
            this.Immortal.Name = "Immortal";
            this.Immortal.Size = new System.Drawing.Size(89, 24);
            this.Immortal.TabIndex = 4;
            this.Immortal.TabStop = true;
            this.Immortal.Text = "Immortal";
            this.Immortal.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(122, 335);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(147, 43);
            this.button1.TabIndex = 5;
            this.button1.Text = "Add Character";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(363, 335);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(147, 43);
            this.button2.TabIndex = 6;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(34, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Add New Character";
            // 
            // NewCharacterDescription
            // 
            this.NewCharacterDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewCharacterDescription.Location = new System.Drawing.Point(119, 211);
            this.NewCharacterDescription.Name = "NewCharacterDescription";
            this.NewCharacterDescription.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.NewCharacterDescription.Size = new System.Drawing.Size(388, 103);
            this.NewCharacterDescription.TabIndex = 8;
            this.NewCharacterDescription.Text = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(118, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Brief Description";
            // 
            // FormNewCharacter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 407);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.NewCharacterDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Immortal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CharacterAge);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FirstNameTextbox);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormNewCharacter";
            this.Text = "FormNewCharacter";
            this.Load += new System.EventHandler(this.FormNewCharacter_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CharacterAge)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox FirstNameTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown CharacterAge;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton Immortal;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox NewCharacterDescription;
        private System.Windows.Forms.Label label4;
    }
}