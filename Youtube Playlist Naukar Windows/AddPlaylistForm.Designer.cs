
namespace Youtube_Playlist_Naukar_Windows
{
    partial class AddPlaylistForm
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
            this.Message = new System.Windows.Forms.Label();
            this.urlLabel = new System.Windows.Forms.Label();
            this.urlBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Message
            // 
            this.Message.AutoSize = true;
            this.Message.Location = new System.Drawing.Point(43, 41);
            this.Message.Name = "Message";
            this.Message.Size = new System.Drawing.Size(0, 25);
            this.Message.TabIndex = 0;
            // 
            // urlLabel
            // 
            this.urlLabel.AutoSize = true;
            this.urlLabel.Location = new System.Drawing.Point(43, 71);
            this.urlLabel.Name = "urlLabel";
            this.urlLabel.Size = new System.Drawing.Size(426, 25);
            this.urlLabel.TabIndex = 1;
            this.urlLabel.Text = "Enter URL of YouTube playlist that you contribute to:";
            // 
            // urlBox
            // 
            this.urlBox.Location = new System.Drawing.Point(43, 124);
            this.urlBox.Name = "urlBox";
            this.urlBox.Size = new System.Drawing.Size(638, 31);
            this.urlBox.TabIndex = 2;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(227, 214);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(112, 34);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.Ok_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(396, 214);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(112, 34);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // AddPlaylistForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 275);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.urlBox);
            this.Controls.Add(this.urlLabel);
            this.Controls.Add(this.Message);
            this.MaximumSize = new System.Drawing.Size(752, 331);
            this.MinimumSize = new System.Drawing.Size(752, 331);
            this.Name = "AddPlaylistForm";
            this.Text = "Add Contributor Playlist";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Message;
        private System.Windows.Forms.Label urlLabel;
        private System.Windows.Forms.TextBox urlBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}