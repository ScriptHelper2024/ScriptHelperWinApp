namespace ScriptHelper
{
    partial class FormJumpStart
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
            this.MyBox = new System.Windows.Forms.RichTextBox();
            this.LoopCount = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.LoopCount)).BeginInit();
            this.SuspendLayout();
            // 
            // MyBox
            // 
            this.MyBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MyBox.Location = new System.Drawing.Point(23, 18);
            this.MyBox.Name = "MyBox";
            this.MyBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.MyBox.Size = new System.Drawing.Size(744, 275);
            this.MyBox.TabIndex = 0;
            this.MyBox.Text = "";
            this.MyBox.TextChanged += new System.EventHandler(this.MyBox_TextChanged);
            // 
            // LoopCount
            // 
            this.LoopCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoopCount.Location = new System.Drawing.Point(23, 308);
            this.LoopCount.Name = "LoopCount";
            this.LoopCount.Size = new System.Drawing.Size(54, 26);
            this.LoopCount.TabIndex = 1;
            this.LoopCount.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.LoopCount.ValueChanged += new System.EventHandler(this.LoopCount_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(98, 310);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "How Many Loops To Run?";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(23, 362);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(164, 50);
            this.button1.TabIndex = 3;
            this.button1.Text = "Run Jump Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(224, 362);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(153, 51);
            this.button2.TabIndex = 4;
            this.button2.Text = "Quit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(563, 362);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(204, 50);
            this.button3.TabIndex = 5;
            this.button3.Text = "  Clear Movie Text   Start From Movie Seed";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FormJumpStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LoopCount);
            this.Controls.Add(this.MyBox);
            this.Name = "FormJumpStart";
            this.Text = "FormJumpStart";
            this.Load += new System.EventHandler(this.FormJumpStart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LoopCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox MyBox;
        private System.Windows.Forms.NumericUpDown LoopCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}