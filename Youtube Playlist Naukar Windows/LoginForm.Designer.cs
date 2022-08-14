
namespace Youtube_Playlist_Naukar_Windows
{
    partial class LoginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loginButton = new System.Windows.Forms.Button();
            this.welcomeLabel = new System.Windows.Forms.Label();
            this.loginMotivationLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(300, 194);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(179, 62);
            this.loginButton.TabIndex = 0;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // welcomeLabel
            // 
            this.welcomeLabel.AutoSize = true;
            this.welcomeLabel.Location = new System.Drawing.Point(341, 52);
            this.welcomeLabel.Name = "welcomeLabel";
            this.welcomeLabel.Size = new System.Drawing.Size(90, 25);
            this.welcomeLabel.TabIndex = 1;
            this.welcomeLabel.Text = "Welcome!";
            // 
            // loginMotivationLabel
            // 
            this.loginMotivationLabel.AutoSize = true;
            this.loginMotivationLabel.Location = new System.Drawing.Point(226, 136);
            this.loginMotivationLabel.Name = "loginMotivationLabel";
            this.loginMotivationLabel.Size = new System.Drawing.Size(389, 25);
            this.loginMotivationLabel.TabIndex = 2;
            this.loginMotivationLabel.Text = "Login to start managing your YouTube playlists.";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 331);
            this.Controls.Add(this.loginMotivationLabel);
            this.Controls.Add(this.welcomeLabel);
            this.Controls.Add(this.loginButton);
            this.MaximumSize = new System.Drawing.Size(843, 387);
            this.MinimumSize = new System.Drawing.Size(843, 387);
            this.Name = "LoginForm";
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Label welcomeLabel;
        private System.Windows.Forms.Label loginMotivationLabel;
    }
}

