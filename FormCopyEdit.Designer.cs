namespace ScriptHelper
{
    partial class FormCopyEdit
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
            this.enrichenRadioButton = new System.Windows.Forms.RadioButton();
            this.depurpleRadioButton = new System.Windows.Forms.RadioButton();
            this.runButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // enrichenRadioButton
            // 
            this.enrichenRadioButton.AutoSize = true;
            this.enrichenRadioButton.Location = new System.Drawing.Point(107, 12);
            this.enrichenRadioButton.Name = "enrichenRadioButton";
            this.enrichenRadioButton.Size = new System.Drawing.Size(67, 17);
            this.enrichenRadioButton.TabIndex = 0;
            this.enrichenRadioButton.TabStop = true;
            this.enrichenRadioButton.Text = "Enrichen";
            this.enrichenRadioButton.UseVisualStyleBackColor = true;
            this.enrichenRadioButton.CheckedChanged += new System.EventHandler(this.enrichenRadioButton_CheckedChanged);
            // 
            // depurpleRadioButton
            // 
            this.depurpleRadioButton.AutoSize = true;
            this.depurpleRadioButton.Checked = true;
            this.depurpleRadioButton.Location = new System.Drawing.Point(107, 35);
            this.depurpleRadioButton.Name = "depurpleRadioButton";
            this.depurpleRadioButton.Size = new System.Drawing.Size(71, 17);
            this.depurpleRadioButton.TabIndex = 1;
            this.depurpleRadioButton.TabStop = true;
            this.depurpleRadioButton.Text = "De-purple";
            this.depurpleRadioButton.UseVisualStyleBackColor = true;
            this.depurpleRadioButton.CheckedChanged += new System.EventHandler(this.depurpleRadioButton_CheckedChanged);
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(71, 75);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(143, 23);
            this.runButton.TabIndex = 2;
            this.runButton.Text = "Run Selected Copy Edit";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // FormCopyEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 120);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.depurpleRadioButton);
            this.Controls.Add(this.enrichenRadioButton);
            this.Name = "FormCopyEdit";
            this.Text = "Copy Edit - Select Copy Style";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton enrichenRadioButton;
        private System.Windows.Forms.RadioButton depurpleRadioButton;
        private System.Windows.Forms.Button runButton;
    }
}