namespace ScriptHelper
{
    partial class FormStart
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
            this.components = new System.ComponentModel.Container();
            this.CreateNewMovie = new System.Windows.Forms.Button();
            this.MovieTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PreviousMovies = new System.Windows.Forms.ListBox();
            this.OpenMovie = new System.Windows.Forms.Button();
            this.ExitStart = new System.Windows.Forms.Button();
            this.DeleteMovie = new System.Windows.Forms.Button();
            this.Version = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.FormStartToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // CreateNewMovie
            // 
            this.CreateNewMovie.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreateNewMovie.Location = new System.Drawing.Point(22, 40);
            this.CreateNewMovie.Name = "CreateNewMovie";
            this.CreateNewMovie.Size = new System.Drawing.Size(194, 57);
            this.CreateNewMovie.TabIndex = 0;
            this.CreateNewMovie.Text = "Create New Movie";
            this.FormStartToolTip.SetToolTip(this.CreateNewMovie, "Jump straight into a fresh Movie (skips creation of a Movie Profile)");
            this.CreateNewMovie.UseVisualStyleBackColor = true;
            this.CreateNewMovie.Click += new System.EventHandler(this.CreateNewMovie_Click);
            // 
            // MovieTitle
            // 
            this.MovieTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MovieTitle.Location = new System.Drawing.Point(236, 72);
            this.MovieTitle.Name = "MovieTitle";
            this.MovieTitle.Size = new System.Drawing.Size(385, 26);
            this.MovieTitle.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(232, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(336, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "New Movie Title (can change later)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "Previous Movies";
            // 
            // PreviousMovies
            // 
            this.PreviousMovies.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PreviousMovies.FormattingEnabled = true;
            this.PreviousMovies.ItemHeight = 20;
            this.PreviousMovies.Location = new System.Drawing.Point(22, 154);
            this.PreviousMovies.Name = "PreviousMovies";
            this.PreviousMovies.ScrollAlwaysVisible = true;
            this.PreviousMovies.Size = new System.Drawing.Size(745, 184);
            this.PreviousMovies.TabIndex = 4;
            // 
            // OpenMovie
            // 
            this.OpenMovie.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenMovie.Location = new System.Drawing.Point(22, 375);
            this.OpenMovie.Name = "OpenMovie";
            this.OpenMovie.Size = new System.Drawing.Size(146, 40);
            this.OpenMovie.TabIndex = 5;
            this.OpenMovie.Text = "Open Movie";
            this.OpenMovie.UseVisualStyleBackColor = true;
            this.OpenMovie.Click += new System.EventHandler(this.OpenMovie_Click);
            // 
            // ExitStart
            // 
            this.ExitStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitStart.Location = new System.Drawing.Point(22, 430);
            this.ExitStart.Name = "ExitStart";
            this.ExitStart.Size = new System.Drawing.Size(183, 40);
            this.ExitStart.TabIndex = 6;
            this.ExitStart.Text = "Exit";
            this.ExitStart.UseVisualStyleBackColor = true;
            this.ExitStart.Click += new System.EventHandler(this.ExitStart_Click);
            // 
            // DeleteMovie
            // 
            this.DeleteMovie.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteMovie.Location = new System.Drawing.Point(188, 375);
            this.DeleteMovie.Name = "DeleteMovie";
            this.DeleteMovie.Size = new System.Drawing.Size(153, 39);
            this.DeleteMovie.TabIndex = 7;
            this.DeleteMovie.Text = "Delete Movie";
            this.DeleteMovie.UseVisualStyleBackColor = true;
            this.DeleteMovie.Click += new System.EventHandler(this.DeleteMovie_Click);
            // 
            // Version
            // 
            this.Version.AutoSize = true;
            this.Version.Location = new System.Drawing.Point(30, 12);
            this.Version.Name = "Version";
            this.Version.Size = new System.Drawing.Size(42, 13);
            this.Version.TabIndex = 8;
            this.Version.Text = "Version";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(0, 483);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(361, 376);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(180, 39);
            this.button1.TabIndex = 12;
            this.button1.Text = "Import .shf File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(558, 375);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(166, 38);
            this.button2.TabIndex = 13;
            this.button2.Text = "Export .shf File";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(654, 46);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(99, 77);
            this.button3.TabIndex = 14;
            this.button3.Text = "Alt Create New  (test)";
            this.FormStartToolTip.SetToolTip(this.button3, "Alternate process that begins by creating a Movie Profile");
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FormStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 505);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.Version);
            this.Controls.Add(this.DeleteMovie);
            this.Controls.Add(this.ExitStart);
            this.Controls.Add(this.OpenMovie);
            this.Controls.Add(this.PreviousMovies);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MovieTitle);
            this.Controls.Add(this.CreateNewMovie);
            this.Name = "FormStart";
            this.Text = "ScriptHelper Start";
            this.Load += new System.EventHandler(this.Start_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CreateNewMovie;
        private System.Windows.Forms.TextBox MovieTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox PreviousMovies;
        private System.Windows.Forms.Button OpenMovie;
        private System.Windows.Forms.Button ExitStart;
        private System.Windows.Forms.Button DeleteMovie;
        private System.Windows.Forms.Label Version;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolTip FormStartToolTip;
    }
}