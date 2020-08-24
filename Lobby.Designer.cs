namespace DialogGame
{
    partial class Lobby
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
            this.TwoLabel = new System.Windows.Forms.RadioButton();
            this.ThreeLabel = new System.Windows.Forms.RadioButton();
            this.FourLabel = new System.Windows.Forms.RadioButton();
            this.numPlayersLabel = new System.Windows.Forms.Label();
            this.modeLabel = new System.Windows.Forms.Label();
            this.ClassicLabel = new System.Windows.Forms.RadioButton();
            this.TimeLimitLabel = new System.Windows.Forms.RadioButton();
            this.playersPanel = new System.Windows.Forms.Panel();
            this.TimeLimitBox = new System.Windows.Forms.ComboBox();
            this.numHumanPlayersLabel = new System.Windows.Forms.Label();
            this.numComputerPlayersLabel = new System.Windows.Forms.Label();
            this.humansPanel = new System.Windows.Forms.Panel();
            this.ZeroHumanButton = new System.Windows.Forms.RadioButton();
            this.FourHumanButton = new System.Windows.Forms.RadioButton();
            this.OneHumanButton = new System.Windows.Forms.RadioButton();
            this.TwoHumanButton = new System.Windows.Forms.RadioButton();
            this.ThreeHumanButton = new System.Windows.Forms.RadioButton();
            this.computerPanel = new System.Windows.Forms.Panel();
            this.ZeroComputerButton = new System.Windows.Forms.RadioButton();
            this.FourComputerButton = new System.Windows.Forms.RadioButton();
            this.OneComputerButton = new System.Windows.Forms.RadioButton();
            this.TwoComputerButton = new System.Windows.Forms.RadioButton();
            this.ThreeComputerButton = new System.Windows.Forms.RadioButton();
            this.warningLabel = new System.Windows.Forms.Label();
            this.PlayButton = new System.Windows.Forms.Button();
            this.difficultyLabel = new System.Windows.Forms.Label();
            this.difficultyPanel = new System.Windows.Forms.Panel();
            this.HardButton = new System.Windows.Forms.RadioButton();
            this.EasyButton = new System.Windows.Forms.RadioButton();
            this.playersPanel.SuspendLayout();
            this.humansPanel.SuspendLayout();
            this.computerPanel.SuspendLayout();
            this.difficultyPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TwoLabel
            // 
            this.TwoLabel.AutoSize = true;
            this.TwoLabel.Checked = true;
            this.TwoLabel.Location = new System.Drawing.Point(8, 21);
            this.TwoLabel.Name = "TwoLabel";
            this.TwoLabel.Size = new System.Drawing.Size(31, 17);
            this.TwoLabel.TabIndex = 2;
            this.TwoLabel.TabStop = true;
            this.TwoLabel.Text = "2";
            this.TwoLabel.UseVisualStyleBackColor = true;
            this.TwoLabel.CheckedChanged += new System.EventHandler(this.TwoLabel_CheckedChanged);
            // 
            // ThreeLabel
            // 
            this.ThreeLabel.AutoSize = true;
            this.ThreeLabel.Location = new System.Drawing.Point(62, 21);
            this.ThreeLabel.Name = "ThreeLabel";
            this.ThreeLabel.Size = new System.Drawing.Size(31, 17);
            this.ThreeLabel.TabIndex = 3;
            this.ThreeLabel.Text = "3";
            this.ThreeLabel.UseVisualStyleBackColor = true;
            this.ThreeLabel.CheckedChanged += new System.EventHandler(this.ThreeLabel_CheckedChanged);
            // 
            // FourLabel
            // 
            this.FourLabel.AutoSize = true;
            this.FourLabel.Location = new System.Drawing.Point(113, 21);
            this.FourLabel.Name = "FourLabel";
            this.FourLabel.Size = new System.Drawing.Size(31, 17);
            this.FourLabel.TabIndex = 4;
            this.FourLabel.Text = "4";
            this.FourLabel.UseVisualStyleBackColor = true;
            this.FourLabel.CheckedChanged += new System.EventHandler(this.FourLabel_CheckedChanged);
            // 
            // numPlayersLabel
            // 
            this.numPlayersLabel.AutoSize = true;
            this.numPlayersLabel.Location = new System.Drawing.Point(38, 54);
            this.numPlayersLabel.Name = "numPlayersLabel";
            this.numPlayersLabel.Size = new System.Drawing.Size(76, 13);
            this.numPlayersLabel.TabIndex = 5;
            this.numPlayersLabel.Text = "No. of Players:";
            // 
            // modeLabel
            // 
            this.modeLabel.AutoSize = true;
            this.modeLabel.Location = new System.Drawing.Point(38, 296);
            this.modeLabel.Name = "modeLabel";
            this.modeLabel.Size = new System.Drawing.Size(68, 13);
            this.modeLabel.TabIndex = 10;
            this.modeLabel.Text = "Game Mode:";
            // 
            // ClassicLabel
            // 
            this.ClassicLabel.AutoSize = true;
            this.ClassicLabel.Checked = true;
            this.ClassicLabel.Location = new System.Drawing.Point(128, 296);
            this.ClassicLabel.Name = "ClassicLabel";
            this.ClassicLabel.Size = new System.Drawing.Size(58, 17);
            this.ClassicLabel.TabIndex = 11;
            this.ClassicLabel.TabStop = true;
            this.ClassicLabel.Text = "Classic";
            this.ClassicLabel.UseVisualStyleBackColor = true;
            this.ClassicLabel.CheckedChanged += new System.EventHandler(this.ClassicLabel_CheckedChanged);
            // 
            // TimeLimitLabel
            // 
            this.TimeLimitLabel.AutoSize = true;
            this.TimeLimitLabel.Location = new System.Drawing.Point(207, 296);
            this.TimeLimitLabel.Name = "TimeLimitLabel";
            this.TimeLimitLabel.Size = new System.Drawing.Size(72, 17);
            this.TimeLimitLabel.TabIndex = 12;
            this.TimeLimitLabel.Text = "Time Limit";
            this.TimeLimitLabel.UseVisualStyleBackColor = true;
            this.TimeLimitLabel.CheckedChanged += new System.EventHandler(this.TimeLimitLabel_CheckedChanged);
            // 
            // playersPanel
            // 
            this.playersPanel.Controls.Add(this.TwoLabel);
            this.playersPanel.Controls.Add(this.ThreeLabel);
            this.playersPanel.Controls.Add(this.FourLabel);
            this.playersPanel.Location = new System.Drawing.Point(187, 33);
            this.playersPanel.Name = "playersPanel";
            this.playersPanel.Size = new System.Drawing.Size(167, 54);
            this.playersPanel.TabIndex = 14;
            // 
            // TimeLimitBox
            // 
            this.TimeLimitBox.FormattingEnabled = true;
            this.TimeLimitBox.Items.AddRange(new object[] {
            "5 minutes",
            "10 minutes",
            "30 minutes"});
            this.TimeLimitBox.Location = new System.Drawing.Point(293, 296);
            this.TimeLimitBox.Name = "TimeLimitBox";
            this.TimeLimitBox.Size = new System.Drawing.Size(79, 21);
            this.TimeLimitBox.TabIndex = 16;
            this.TimeLimitBox.Text = "5 minutes";
            this.TimeLimitBox.Visible = false;
            this.TimeLimitBox.SelectedIndexChanged += new System.EventHandler(this.TimeLimitBox_SelectedIndexChanged);
            // 
            // numHumanPlayersLabel
            // 
            this.numHumanPlayersLabel.AutoSize = true;
            this.numHumanPlayersLabel.Location = new System.Drawing.Point(38, 140);
            this.numHumanPlayersLabel.Name = "numHumanPlayersLabel";
            this.numHumanPlayersLabel.Size = new System.Drawing.Size(113, 13);
            this.numHumanPlayersLabel.TabIndex = 17;
            this.numHumanPlayersLabel.Text = "No. of Human Players:";
            // 
            // numComputerPlayersLabel
            // 
            this.numComputerPlayersLabel.AutoSize = true;
            this.numComputerPlayersLabel.Location = new System.Drawing.Point(38, 223);
            this.numComputerPlayersLabel.Name = "numComputerPlayersLabel";
            this.numComputerPlayersLabel.Size = new System.Drawing.Size(124, 13);
            this.numComputerPlayersLabel.TabIndex = 18;
            this.numComputerPlayersLabel.Text = "No. of Computer Players:";
            // 
            // humansPanel
            // 
            this.humansPanel.Controls.Add(this.ZeroHumanButton);
            this.humansPanel.Controls.Add(this.FourHumanButton);
            this.humansPanel.Controls.Add(this.OneHumanButton);
            this.humansPanel.Controls.Add(this.TwoHumanButton);
            this.humansPanel.Controls.Add(this.ThreeHumanButton);
            this.humansPanel.Location = new System.Drawing.Point(187, 120);
            this.humansPanel.Name = "humansPanel";
            this.humansPanel.Size = new System.Drawing.Size(222, 54);
            this.humansPanel.TabIndex = 19;
            // 
            // ZeroHumanButton
            // 
            this.ZeroHumanButton.AutoSize = true;
            this.ZeroHumanButton.Location = new System.Drawing.Point(8, 21);
            this.ZeroHumanButton.Name = "ZeroHumanButton";
            this.ZeroHumanButton.Size = new System.Drawing.Size(31, 17);
            this.ZeroHumanButton.TabIndex = 6;
            this.ZeroHumanButton.Text = "0";
            this.ZeroHumanButton.UseVisualStyleBackColor = true;
            this.ZeroHumanButton.CheckedChanged += new System.EventHandler(this.ZeroHumanButton_CheckedChanged);
            // 
            // FourHumanButton
            // 
            this.FourHumanButton.AutoSize = true;
            this.FourHumanButton.Location = new System.Drawing.Point(179, 21);
            this.FourHumanButton.Name = "FourHumanButton";
            this.FourHumanButton.Size = new System.Drawing.Size(31, 17);
            this.FourHumanButton.TabIndex = 5;
            this.FourHumanButton.Text = "4";
            this.FourHumanButton.UseVisualStyleBackColor = true;
            this.FourHumanButton.CheckedChanged += new System.EventHandler(this.FourHumanButton_CheckedChanged);
            // 
            // OneHumanButton
            // 
            this.OneHumanButton.AutoSize = true;
            this.OneHumanButton.Checked = true;
            this.OneHumanButton.Location = new System.Drawing.Point(50, 21);
            this.OneHumanButton.Name = "OneHumanButton";
            this.OneHumanButton.Size = new System.Drawing.Size(31, 17);
            this.OneHumanButton.TabIndex = 1;
            this.OneHumanButton.TabStop = true;
            this.OneHumanButton.Text = "1";
            this.OneHumanButton.UseVisualStyleBackColor = true;
            this.OneHumanButton.CheckedChanged += new System.EventHandler(this.OneHumanButton_CheckedChanged);
            // 
            // TwoHumanButton
            // 
            this.TwoHumanButton.AutoSize = true;
            this.TwoHumanButton.Location = new System.Drawing.Point(91, 21);
            this.TwoHumanButton.Name = "TwoHumanButton";
            this.TwoHumanButton.Size = new System.Drawing.Size(31, 17);
            this.TwoHumanButton.TabIndex = 2;
            this.TwoHumanButton.Text = "2";
            this.TwoHumanButton.UseVisualStyleBackColor = true;
            this.TwoHumanButton.CheckedChanged += new System.EventHandler(this.TwoHumanButton_CheckedChanged);
            // 
            // ThreeHumanButton
            // 
            this.ThreeHumanButton.AutoSize = true;
            this.ThreeHumanButton.Location = new System.Drawing.Point(136, 21);
            this.ThreeHumanButton.Name = "ThreeHumanButton";
            this.ThreeHumanButton.Size = new System.Drawing.Size(31, 17);
            this.ThreeHumanButton.TabIndex = 3;
            this.ThreeHumanButton.Text = "3";
            this.ThreeHumanButton.UseVisualStyleBackColor = true;
            this.ThreeHumanButton.CheckedChanged += new System.EventHandler(this.ThreeHumanButton_CheckedChanged);
            // 
            // computerPanel
            // 
            this.computerPanel.Controls.Add(this.ZeroComputerButton);
            this.computerPanel.Controls.Add(this.FourComputerButton);
            this.computerPanel.Controls.Add(this.OneComputerButton);
            this.computerPanel.Controls.Add(this.TwoComputerButton);
            this.computerPanel.Controls.Add(this.ThreeComputerButton);
            this.computerPanel.Location = new System.Drawing.Point(187, 198);
            this.computerPanel.Name = "computerPanel";
            this.computerPanel.Size = new System.Drawing.Size(222, 54);
            this.computerPanel.TabIndex = 20;
            // 
            // ZeroComputerButton
            // 
            this.ZeroComputerButton.AutoSize = true;
            this.ZeroComputerButton.Location = new System.Drawing.Point(8, 21);
            this.ZeroComputerButton.Name = "ZeroComputerButton";
            this.ZeroComputerButton.Size = new System.Drawing.Size(31, 17);
            this.ZeroComputerButton.TabIndex = 7;
            this.ZeroComputerButton.Text = "0";
            this.ZeroComputerButton.UseVisualStyleBackColor = true;
            this.ZeroComputerButton.CheckedChanged += new System.EventHandler(this.ZeroComputerButton_CheckedChanged);
            // 
            // FourComputerButton
            // 
            this.FourComputerButton.AutoSize = true;
            this.FourComputerButton.Location = new System.Drawing.Point(179, 21);
            this.FourComputerButton.Name = "FourComputerButton";
            this.FourComputerButton.Size = new System.Drawing.Size(31, 17);
            this.FourComputerButton.TabIndex = 5;
            this.FourComputerButton.Text = "4";
            this.FourComputerButton.UseVisualStyleBackColor = true;
            this.FourComputerButton.CheckedChanged += new System.EventHandler(this.FourComputerButton_CheckedChanged);
            // 
            // OneComputerButton
            // 
            this.OneComputerButton.AutoSize = true;
            this.OneComputerButton.Checked = true;
            this.OneComputerButton.Location = new System.Drawing.Point(50, 21);
            this.OneComputerButton.Name = "OneComputerButton";
            this.OneComputerButton.Size = new System.Drawing.Size(31, 17);
            this.OneComputerButton.TabIndex = 1;
            this.OneComputerButton.TabStop = true;
            this.OneComputerButton.Text = "1";
            this.OneComputerButton.UseVisualStyleBackColor = true;
            this.OneComputerButton.CheckedChanged += new System.EventHandler(this.OneComputerButton_CheckedChanged);
            // 
            // TwoComputerButton
            // 
            this.TwoComputerButton.AutoSize = true;
            this.TwoComputerButton.Location = new System.Drawing.Point(91, 21);
            this.TwoComputerButton.Name = "TwoComputerButton";
            this.TwoComputerButton.Size = new System.Drawing.Size(31, 17);
            this.TwoComputerButton.TabIndex = 2;
            this.TwoComputerButton.Text = "2";
            this.TwoComputerButton.UseVisualStyleBackColor = true;
            this.TwoComputerButton.CheckedChanged += new System.EventHandler(this.TwoComputerButton_CheckedChanged);
            // 
            // ThreeComputerButton
            // 
            this.ThreeComputerButton.AutoSize = true;
            this.ThreeComputerButton.Location = new System.Drawing.Point(136, 21);
            this.ThreeComputerButton.Name = "ThreeComputerButton";
            this.ThreeComputerButton.Size = new System.Drawing.Size(31, 17);
            this.ThreeComputerButton.TabIndex = 3;
            this.ThreeComputerButton.Text = "3";
            this.ThreeComputerButton.UseVisualStyleBackColor = true;
            this.ThreeComputerButton.CheckedChanged += new System.EventHandler(this.ThreeComputerButton_CheckedChanged);
            // 
            // warningLabel
            // 
            this.warningLabel.AutoSize = true;
            this.warningLabel.Location = new System.Drawing.Point(318, 367);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(47, 13);
            this.warningLabel.TabIndex = 21;
            this.warningLabel.Text = "Warning";
            // 
            // PlayButton
            // 
            this.PlayButton.Location = new System.Drawing.Point(373, 382);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(112, 30);
            this.PlayButton.TabIndex = 22;
            this.PlayButton.Text = "Play";
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // difficultyLabel
            // 
            this.difficultyLabel.AutoSize = true;
            this.difficultyLabel.Location = new System.Drawing.Point(431, 184);
            this.difficultyLabel.Name = "difficultyLabel";
            this.difficultyLabel.Size = new System.Drawing.Size(50, 13);
            this.difficultyLabel.TabIndex = 23;
            this.difficultyLabel.Text = "Difficulty:";
            // 
            // difficultyPanel
            // 
            this.difficultyPanel.Controls.Add(this.HardButton);
            this.difficultyPanel.Controls.Add(this.EasyButton);
            this.difficultyPanel.Location = new System.Drawing.Point(429, 200);
            this.difficultyPanel.Name = "difficultyPanel";
            this.difficultyPanel.Size = new System.Drawing.Size(66, 52);
            this.difficultyPanel.TabIndex = 24;
            // 
            // HardButton
            // 
            this.HardButton.AutoSize = true;
            this.HardButton.Location = new System.Drawing.Point(4, 27);
            this.HardButton.Name = "HardButton";
            this.HardButton.Size = new System.Drawing.Size(48, 17);
            this.HardButton.TabIndex = 1;
            this.HardButton.Text = "Hard";
            this.HardButton.UseVisualStyleBackColor = true;
            this.HardButton.CheckedChanged += new System.EventHandler(this.HardButton_CheckedChanged);
            // 
            // EasyButton
            // 
            this.EasyButton.AutoSize = true;
            this.EasyButton.Checked = true;
            this.EasyButton.Location = new System.Drawing.Point(4, 4);
            this.EasyButton.Name = "EasyButton";
            this.EasyButton.Size = new System.Drawing.Size(48, 17);
            this.EasyButton.TabIndex = 0;
            this.EasyButton.TabStop = true;
            this.EasyButton.Text = "Easy";
            this.EasyButton.UseVisualStyleBackColor = true;
            this.EasyButton.CheckedChanged += new System.EventHandler(this.EasyButton_CheckedChanged);
            // 
            // Lobby
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 442);
            this.Controls.Add(this.difficultyPanel);
            this.Controls.Add(this.difficultyLabel);
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.warningLabel);
            this.Controls.Add(this.computerPanel);
            this.Controls.Add(this.humansPanel);
            this.Controls.Add(this.numComputerPlayersLabel);
            this.Controls.Add(this.numHumanPlayersLabel);
            this.Controls.Add(this.TimeLimitBox);
            this.Controls.Add(this.playersPanel);
            this.Controls.Add(this.TimeLimitLabel);
            this.Controls.Add(this.ClassicLabel);
            this.Controls.Add(this.modeLabel);
            this.Controls.Add(this.numPlayersLabel);
            this.Name = "Lobby";
            this.Text = "Lobby";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.playersPanel.ResumeLayout(false);
            this.playersPanel.PerformLayout();
            this.humansPanel.ResumeLayout(false);
            this.humansPanel.PerformLayout();
            this.computerPanel.ResumeLayout(false);
            this.computerPanel.PerformLayout();
            this.difficultyPanel.ResumeLayout(false);
            this.difficultyPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RadioButton TwoLabel;
        private System.Windows.Forms.RadioButton ThreeLabel;
        private System.Windows.Forms.RadioButton FourLabel;
        private System.Windows.Forms.Label numPlayersLabel;
        private System.Windows.Forms.Label modeLabel;
        private System.Windows.Forms.RadioButton ClassicLabel;
        private System.Windows.Forms.RadioButton TimeLimitLabel;
        private System.Windows.Forms.Panel playersPanel;
        private System.Windows.Forms.ComboBox TimeLimitBox;
        private System.Windows.Forms.Label numHumanPlayersLabel;
        private System.Windows.Forms.Label numComputerPlayersLabel;
        private System.Windows.Forms.Panel humansPanel;
        private System.Windows.Forms.RadioButton FourHumanButton;
        private System.Windows.Forms.RadioButton OneHumanButton;
        private System.Windows.Forms.RadioButton TwoHumanButton;
        private System.Windows.Forms.RadioButton ThreeHumanButton;
        private System.Windows.Forms.Panel computerPanel;
        private System.Windows.Forms.RadioButton FourComputerButton;
        private System.Windows.Forms.RadioButton OneComputerButton;
        private System.Windows.Forms.RadioButton TwoComputerButton;
        private System.Windows.Forms.RadioButton ThreeComputerButton;
        private System.Windows.Forms.Label warningLabel;
        private System.Windows.Forms.RadioButton ZeroHumanButton;
        private System.Windows.Forms.RadioButton ZeroComputerButton;
        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.Label difficultyLabel;
        private System.Windows.Forms.Panel difficultyPanel;
        private System.Windows.Forms.RadioButton HardButton;
        private System.Windows.Forms.RadioButton EasyButton;
    }
}

