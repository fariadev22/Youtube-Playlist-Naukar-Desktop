
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
            this.Label = new System.Windows.Forms.Label();
            this.urlBox = new System.Windows.Forms.TextBox();
            this.Ok = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
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
            // Label
            // 
            this.Label.AutoSize = true;
            this.Label.Location = new System.Drawing.Point(43, 82);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(426, 25);
            this.Label.TabIndex = 1;
            this.Label.Text = "Enter URL of YouTube playlist that you contribute to:";
            // 
            // urlBox
            // 
            this.urlBox.Location = new System.Drawing.Point(43, 124);
            this.urlBox.Name = "urlBox";
            this.urlBox.Size = new System.Drawing.Size(638, 31);
            this.urlBox.TabIndex = 2;
            // 
            // Ok
            // 
            this.Ok.Location = new System.Drawing.Point(227, 229);
            this.Ok.Name = "Ok";
            this.Ok.Size = new System.Drawing.Size(112, 34);
            this.Ok.TabIndex = 3;
            this.Ok.Text = "Ok";
            this.Ok.UseVisualStyleBackColor = true;
            this.Ok.Click += new System.EventHandler(this.Ok_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(395, 229);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(112, 34);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // AddPlaylistForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 275);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Ok);
            this.Controls.Add(this.urlBox);
            this.Controls.Add(this.Label);
            this.Controls.Add(this.Message);
            this.Name = "AddPlaylistForm";
            this.Text = "AddPlaylistForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Message;
        private System.Windows.Forms.Label Label;
        private System.Windows.Forms.TextBox urlBox;
        private System.Windows.Forms.Button Ok;
        private System.Windows.Forms.Button Cancel;
    }
}