
namespace Youtube_Playlist_Naukar_Windows
{
    partial class DetailsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DetailsForm));
            this.intro = new System.Windows.Forms.Label();
            this.versionDetails = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // intro
            // 
            this.intro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.intro.AutoSize = true;
            this.intro.Location = new System.Drawing.Point(93, 130);
            this.intro.MaximumSize = new System.Drawing.Size(510, 0);
            this.intro.Name = "intro";
            this.intro.Size = new System.Drawing.Size(452, 50);
            this.intro.TabIndex = 0;
            this.intro.Text = "This application was developed by Faria Rehman. Please contact her directly for a" +
    "ny issues.";
            this.intro.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // versionDetails
            // 
            this.versionDetails.AutoSize = true;
            this.versionDetails.Location = new System.Drawing.Point(193, 301);
            this.versionDetails.Name = "versionDetails";
            this.versionDetails.Size = new System.Drawing.Size(258, 25);
            this.versionDetails.TabIndex = 1;
            this.versionDetails.Text = "Version 1.0, Developed in 2022";
            // 
            // DetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 335);
            this.Controls.Add(this.versionDetails);
            this.Controls.Add(this.intro);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(655, 391);
            this.MinimumSize = new System.Drawing.Size(655, 391);
            this.Name = "DetailsForm";
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label intro;
        private System.Windows.Forms.Label versionDetails;
    }
}