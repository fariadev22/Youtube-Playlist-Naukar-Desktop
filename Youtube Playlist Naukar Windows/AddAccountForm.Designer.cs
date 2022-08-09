
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
            this.EmailCancel = new System.Windows.Forms.Button();
            this.EmailOk = new System.Windows.Forms.Button();
            this.Label = new System.Windows.Forms.Label();
            this.emailInputBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // EmailCancel
            // 
            this.EmailCancel.Location = new System.Drawing.Point(398, 194);
            this.EmailCancel.Name = "EmailCancel";
            this.EmailCancel.Size = new System.Drawing.Size(112, 34);
            this.EmailCancel.TabIndex = 8;
            this.EmailCancel.Text = "Cancel";
            this.EmailCancel.UseVisualStyleBackColor = true;
            // 
            // EmailOk
            // 
            this.EmailOk.Location = new System.Drawing.Point(230, 194);
            this.EmailOk.Name = "EmailOk";
            this.EmailOk.Size = new System.Drawing.Size(112, 34);
            this.EmailOk.TabIndex = 7;
            this.EmailOk.Text = "Ok";
            this.EmailOk.UseVisualStyleBackColor = true;
            this.EmailOk.Click += new System.EventHandler(this.EmailOk_Click);
            // 
            // Label
            // 
            this.Label.AutoSize = true;
            this.Label.Location = new System.Drawing.Point(46, 47);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(207, 25);
            this.Label.TabIndex = 5;
            this.Label.Text = "Enter new email address:";
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
            this.Controls.Add(this.EmailCancel);
            this.Controls.Add(this.EmailOk);
            this.Controls.Add(this.Label);
            this.Name = "AddAccountForm";
            this.Text = "AddAccountForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button EmailCancel;
        private System.Windows.Forms.Button EmailOk;
        private System.Windows.Forms.Label Label;
        private System.Windows.Forms.TextBox emailInputBox;
    }
}