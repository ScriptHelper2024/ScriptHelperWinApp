namespace ScriptHelper
{
    partial class FormScenesRefactor
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
            this.SceneCount = new System.Windows.Forms.NumericUpDown();
            this.TokensInHintText = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.RefactorSummary = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.IncludeMovieText = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.SceneCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TokensInHintText)).BeginInit();
            this.SuspendLayout();
            // 
            // SceneCount
            // 
            this.SceneCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SceneCount.Location = new System.Drawing.Point(40, 147);
            this.SceneCount.Name = "SceneCount";
            this.SceneCount.Size = new System.Drawing.Size(46, 22);
            this.SceneCount.TabIndex = 0;
            this.SceneCount.ValueChanged += new System.EventHandler(this.SceneCount_ValueChanged);
            // 
            // TokensInHintText
            // 
            this.TokensInHintText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TokensInHintText.Location = new System.Drawing.Point(42, 183);
            this.TokensInHintText.Name = "TokensInHintText";
            this.TokensInHintText.Size = new System.Drawing.Size(43, 22);
            this.TokensInHintText.TabIndex = 1;
            this.TokensInHintText.ValueChanged += new System.EventHandler(this.TokensInHintText_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(102, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(235, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Total Scenes For This Movie";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(102, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(191, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Tokens In Scene Hints";
            // 
            // RefactorSummary
            // 
            this.RefactorSummary.AutoSize = true;
            this.RefactorSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RefactorSummary.Location = new System.Drawing.Point(37, 63);
            this.RefactorSummary.Name = "RefactorSummary";
            this.RefactorSummary.Size = new System.Drawing.Size(130, 16);
            this.RefactorSummary.TabIndex = 4;
            this.RefactorSummary.Text = "RefactorSummary";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(44, 257);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 46);
            this.button1.TabIndex = 5;
            this.button1.Text = "Refactor";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(214, 257);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(104, 45);
            this.button2.TabIndex = 6;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // IncludeMovieText
            // 
            this.IncludeMovieText.AutoSize = true;
            this.IncludeMovieText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IncludeMovieText.Location = new System.Drawing.Point(44, 224);
            this.IncludeMovieText.Name = "IncludeMovieText";
            this.IncludeMovieText.Size = new System.Drawing.Size(295, 20);
            this.IncludeMovieText.TabIndex = 7;
            this.IncludeMovieText.Text = "Include Movie Text in Prompt Template";
            this.IncludeMovieText.UseVisualStyleBackColor = true;
            // 
            // FormScenesRefactor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.IncludeMovieText);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.RefactorSummary);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TokensInHintText);
            this.Controls.Add(this.SceneCount);
            this.Name = "FormScenesRefactor";
            this.Text = "FormScenesRefactor";
            this.Load += new System.EventHandler(this.FormScenesRefactor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SceneCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TokensInHintText)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown SceneCount;
        private System.Windows.Forms.NumericUpDown TokensInHintText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label RefactorSummary;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox IncludeMovieText;
    }
}