
namespace Youtube_Playlist_Naukar_Windows
{
    partial class AddAccountForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddAccountForm));
            this.emailCancelButton = new System.Windows.Forms.Button();
            this.emailOkButton = new System.Windows.Forms.Button();
            this.emailLabel = new System.Windows.Forms.Label();
            this.emailInputBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // emailCancelButton
            // 
            this.emailCancelButton.Location = new System.Drawing.Point(390, 194);
            this.emailCancelButton.Name = "emailCancelButton";
            this.emailCancelButton.Size = new System.Drawing.Size(112, 34);
            this.emailCancelButton.TabIndex = 8;
            this.emailCancelButton.Text = "Cancel";
            this.emailCancelButton.UseVisualStyleBackColor = true;
            // 
            // emailOkButton
            // 
            this.emailOkButton.Location = new System.Drawing.Point(230, 194);
            this.emailOkButton.Name = "emailOkButton";
            this.emailOkButton.Size = new System.Drawing.Size(112, 34);
            this.emailOkButton.TabIndex = 7;
            this.emailOkButton.Text = "Ok";
            this.emailOkButton.UseVisualStyleBackColor = true;
            this.emailOkButton.Click += new System.EventHandler(this.EmailOk_Click);
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Location = new System.Drawing.Point(56, 49);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(207, 25);
            this.emailLabel.TabIndex = 5;
            this.emailLabel.Text = "Enter new email address:";
            // 
            // emailInputBox
            // 
            this.emailInputBox.Location = new System.Drawing.Point(56, 100);
            this.emailInputBox.Name = "emailInputBox";
            this.emailInputBox.Size = new System.Drawing.Size(618, 31);
            this.emailInputBox.TabIndex = 9;
            // 
            // AddAccountForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 275);
            this.Controls.Add(this.emailInputBox);
            this.Controls.Add(this.emailCancelButton);
            this.Controls.Add(this.emailOkButton);
            this.Controls.Add(this.emailLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(752, 331);
            this.MinimumSize = new System.Drawing.Size(752, 331);
            this.Name = "AddAccountForm";
            this.Text = "Add Account";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button emailCancelButton;
        private System.Windows.Forms.Button emailOkButton;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.TextBox emailInputBox;
    }
}