namespace ScriptHelper
{
    partial class FormOutline
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
            this.outlineTextBox = new System.Windows.Forms.RichTextBox();
            this.copyOutlineButton = new System.Windows.Forms.Button();
            this.replaceMovieSeedButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // outlineTextBox
            // 
            this.outlineTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outlineTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outlineTextBox.Location = new System.Drawing.Point(6, 6);
            this.outlineTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.outlineTextBox.Name = "outlineTextBox";
            this.outlineTextBox.Size = new System.Drawing.Size(834, 738);
            this.outlineTextBox.TabIndex = 0;
            this.outlineTextBox.Text = "";
            // 
            // copyOutlineButton
            // 
            this.copyOutlineButton.Location = new System.Drawing.Point(725, 753);
            this.copyOutlineButton.Name = "copyOutlineButton";
            this.copyOutlineButton.Size = new System.Drawing.Size(115, 31);
            this.copyOutlineButton.TabIndex = 1;
            this.copyOutlineButton.Text = "Copy to Clipboard";
            this.copyOutlineButton.UseVisualStyleBackColor = true;
            this.copyOutlineButton.Click += new System.EventHandler(this.copyOutlineButton_Click);
            // 
            // replaceMovieSeedButton
            // 
            this.replaceMovieSeedButton.Location = new System.Drawing.Point(525, 753);
            this.replaceMovieSeedButton.Name = "replaceMovieSeedButton";
            this.replaceMovieSeedButton.Size = new System.Drawing.Size(184, 31);
            this.replaceMovieSeedButton.TabIndex = 2;
            this.replaceMovieSeedButton.Text = "Replace Movie Seed With Outline";
            this.replaceMovieSeedButton.UseVisualStyleBackColor = true;
            this.replaceMovieSeedButton.Click += new System.EventHandler(this.replaceMovieSeedButton_Click);
            // 
            // FormOutline
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 792);
            this.Controls.Add(this.replaceMovieSeedButton);
            this.Controls.Add(this.copyOutlineButton);
            this.Controls.Add(this.outlineTextBox);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormOutline";
            this.Text = "Movie Text Outline";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox outlineTextBox;
        private System.Windows.Forms.Button copyOutlineButton;
        private System.Windows.Forms.Button replaceMovieSeedButton;
    }
}