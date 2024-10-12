namespace ScriptHelper
{
    partial class FormLargeView
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
            this.originalRichTextBox = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.resultRichTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.analysisRichTextBox = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // originalRichTextBox
            // 
            this.originalRichTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.originalRichTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.originalRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.originalRichTextBox.Font = new System.Drawing.Font("Courier New", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.originalRichTextBox.Location = new System.Drawing.Point(19, 70);
            this.originalRichTextBox.Margin = new System.Windows.Forms.Padding(10);
            this.originalRichTextBox.Name = "originalRichTextBox";
            this.originalRichTextBox.Size = new System.Drawing.Size(684, 1321);
            this.originalRichTextBox.TabIndex = 0;
            this.originalRichTextBox.Text = "";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(19, 1404);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(209, 59);
            this.button1.TabIndex = 1;
            this.button1.Text = "Toggle Dark Mode";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // resultRichTextBox
            // 
            this.resultRichTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.resultRichTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.resultRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.resultRichTextBox.Font = new System.Drawing.Font("Courier New", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultRichTextBox.Location = new System.Drawing.Point(723, 70);
            this.resultRichTextBox.Margin = new System.Windows.Forms.Padding(10);
            this.resultRichTextBox.Name = "resultRichTextBox";
            this.resultRichTextBox.Size = new System.Drawing.Size(808, 1321);
            this.resultRichTextBox.TabIndex = 4;
            this.resultRichTextBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Original";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(804, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "Result";
            // 
            // analysisRichTextBox
            // 
            this.analysisRichTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.analysisRichTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.analysisRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.analysisRichTextBox.Font = new System.Drawing.Font("Courier New", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.analysisRichTextBox.Location = new System.Drawing.Point(1565, 70);
            this.analysisRichTextBox.Margin = new System.Windows.Forms.Padding(10);
            this.analysisRichTextBox.Name = "analysisRichTextBox";
            this.analysisRichTextBox.Size = new System.Drawing.Size(759, 1321);
            this.analysisRichTextBox.TabIndex = 7;
            this.analysisRichTextBox.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1594, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 25);
            this.label3.TabIndex = 8;
            this.label3.Text = "Analysis";
            // 
            // FormLargeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(2406, 1469);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.analysisRichTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.resultRichTextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.originalRichTextBox);
            this.Name = "FormLargeView";
            this.Text = "FormLargeView";
            this.Resize += new System.EventHandler(this.FormLargeView_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox originalRichTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox resultRichTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox analysisRichTextBox;
        private System.Windows.Forms.Label label3;
    }
}