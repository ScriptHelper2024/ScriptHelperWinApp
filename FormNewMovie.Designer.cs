namespace ScriptHelper
{
    partial class FormNewMovie
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
            this.USARatings = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Title = new System.Windows.Forms.TextBox();
            this.Genre = new System.Windows.Forms.TextBox();
            this.Audience = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Guidance = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.TimeLength = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.FormNewMovieToolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.TimeLength)).BeginInit();
            this.SuspendLayout();
            // 
            // USARatings
            // 
            this.USARatings.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.USARatings.FormattingEnabled = true;
            this.USARatings.ItemHeight = 20;
            this.USARatings.Location = new System.Drawing.Point(36, 46);
            this.USARatings.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.USARatings.Name = "USARatings";
            this.USARatings.Size = new System.Drawing.Size(138, 164);
            this.USARatings.TabIndex = 0;
            this.USARatings.SelectedIndexChanged += new System.EventHandler(this.USARatings_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(38, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Rating: USA";
            this.FormNewMovieToolTip.SetToolTip(this.label1, "Set an MPA content rating");
            // 
            // Title
            // 
            this.Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.Location = new System.Drawing.Point(287, 56);
            this.Title.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(448, 26);
            this.Title.TabIndex = 2;
            this.Title.TextChanged += new System.EventHandler(this.Title_TextChanged);
            // 
            // Genre
            // 
            this.Genre.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Genre.Location = new System.Drawing.Point(286, 105);
            this.Genre.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Genre.Name = "Genre";
            this.Genre.Size = new System.Drawing.Size(449, 26);
            this.Genre.TabIndex = 3;
            this.Genre.TextChanged += new System.EventHandler(this.Genre_TextChanged);
            // 
            // Audience
            // 
            this.Audience.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Audience.Location = new System.Drawing.Point(287, 155);
            this.Audience.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Audience.Name = "Audience";
            this.Audience.Size = new System.Drawing.Size(448, 26);
            this.Audience.TabIndex = 4;
            this.Audience.TextChanged += new System.EventHandler(this.Audience_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(758, 58);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Title";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(758, 107);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(590, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Genre  eg:  historic drama,  romantic comedy, horror, science fiction,  etc";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(762, 155);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(626, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Audience(s)  eg: all, toddler, preschool, preteen, young adult, adult men, etc  ";
            // 
            // Guidance
            // 
            this.Guidance.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Guidance.Location = new System.Drawing.Point(286, 239);
            this.Guidance.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Guidance.Name = "Guidance";
            this.Guidance.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.Guidance.Size = new System.Drawing.Size(450, 219);
            this.Guidance.TabIndex = 8;
            this.Guidance.Text = "";
            this.Guidance.TextChanged += new System.EventHandler(this.Guidance_TextChanged_1);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(286, 209);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "Guidance";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label46.Location = new System.Drawing.Point(284, 595);
            this.label46.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(385, 20);
            this.label46.TabIndex = 51;
            this.label46.Text = "Contemprary take on \'Who\'s Afraid of Virginia Woolf?\'";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label45.Location = new System.Drawing.Point(286, 574);
            this.label45.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(430, 20);
            this.label45.TabIndex = 50;
            this.label45.Text = "Lord of the Flies with 1980s yuppies rather than school boys";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label44.Location = new System.Drawing.Point(286, 554);
            this.label44.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(487, 20);
            this.label44.TabIndex = 49;
            this.label44.Text = "No plot character study of Williamsburg Brooklyn slackers circa 1999";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label43.Location = new System.Drawing.Point(286, 534);
            this.label43.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(340, 20);
            this.label43.TabIndex = 48;
            this.label43.Text = "Depth psychology version of Homer\'s \'The Iliad\'";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label42.Location = new System.Drawing.Point(286, 514);
            this.label42.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(401, 20);
            this.label42.TabIndex = 47;
            this.label42.Text = "Judd Apatow style end of high school movie - 2024 style";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label40.Location = new System.Drawing.Point(286, 493);
            this.label40.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(517, 20);
            this.label40.TabIndex = 46;
            this.label40.Text = "\'Texas Chainsaw Massacre\' meets \'Sex In The City\' meets George Carlin";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.Location = new System.Drawing.Point(286, 460);
            this.label41.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(91, 20);
            this.label41.TabIndex = 52;
            this.label41.Text = "Examples:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(38, 284);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(233, 24);
            this.label6.TabIndex = 54;
            this.label6.Text = "Target Length (minutes)";
            this.FormNewMovieToolTip.SetToolTip(this.label6, "Ideal movie length (affects the generation of Movie Text and Expansive Notes)");
            // 
            // TimeLength
            // 
            this.TimeLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimeLength.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TimeLength.Location = new System.Drawing.Point(41, 254);
            this.TimeLength.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TimeLength.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.TimeLength.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TimeLength.Name = "TimeLength";
            this.TimeLength.Size = new System.Drawing.Size(95, 28);
            this.TimeLength.TabIndex = 53;
            this.FormNewMovieToolTip.SetToolTip(this.TimeLength, "Ideal movie length (affects the generation of Movie Text and Expansive Notes)");
            this.TimeLength.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.TimeLength.ValueChanged += new System.EventHandler(this.TimeLength_ValueChanged);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(21, 538);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 57);
            this.button1.TabIndex = 55;
            this.button1.Text = "Create";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(140, 538);
            this.button2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 57);
            this.button2.TabIndex = 56;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FormNewMovie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1327, 675);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TimeLength);
            this.Controls.Add(this.label41);
            this.Controls.Add(this.label46);
            this.Controls.Add(this.label45);
            this.Controls.Add(this.label44);
            this.Controls.Add(this.label43);
            this.Controls.Add(this.label42);
            this.Controls.Add(this.label40);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Guidance);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Audience);
            this.Controls.Add(this.Genre);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.USARatings);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FormNewMovie";
            this.Text = "FormNewMovie";
            this.Load += new System.EventHandler(this.FormNewMovie_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TimeLength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox USARatings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Title;
        private System.Windows.Forms.TextBox Genre;
        private System.Windows.Forms.TextBox Audience;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox Guidance;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown TimeLength;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ToolTip FormNewMovieToolTip;
    }
}