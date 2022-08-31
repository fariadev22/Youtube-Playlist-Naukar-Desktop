
namespace Youtube_Playlist_Naukar_Windows
{
    partial class AddVideoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddVideoForm));
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.inputVideoUrlBox = new System.Windows.Forms.TextBox();
            this.addVideoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.Location = new System.Drawing.Point(183, 364);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(112, 34);
            this.cancelButton.TabIndex = 0;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(418, 364);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(112, 34);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // inputVideoUrlBox
            // 
            this.inputVideoUrlBox.AcceptsReturn = true;
            this.inputVideoUrlBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputVideoUrlBox.Location = new System.Drawing.Point(52, 118);
            this.inputVideoUrlBox.Multiline = true;
            this.inputVideoUrlBox.Name = "inputVideoUrlBox";
            this.inputVideoUrlBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.inputVideoUrlBox.Size = new System.Drawing.Size(610, 210);
            this.inputVideoUrlBox.TabIndex = 2;
            // 
            // addVideoLabel
            // 
            this.addVideoLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.addVideoLabel.AutoSize = true;
            this.addVideoLabel.Location = new System.Drawing.Point(52, 58);
            this.addVideoLabel.MaximumSize = new System.Drawing.Size(650, 0);
            this.addVideoLabel.Name = "addVideoLabel";
            this.addVideoLabel.Size = new System.Drawing.Size(628, 50);
            this.addVideoLabel.TabIndex = 3;
            this.addVideoLabel.Text = "Enter URL of YouTube video you want to add to your playlist. To add multiple vide" +
    "os, you can add multiple YouTube video URLs in separate lines.";
            // 
            // AddVideoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 424);
            this.Controls.Add(this.addVideoLabel);
            this.Controls.Add(this.inputVideoUrlBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(750, 480);
            this.MinimumSize = new System.Drawing.Size(750, 480);
            this.Name = "AddVideoForm";
            this.Text = "Add Video";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TextBox inputVideoUrlBox;
        private System.Windows.Forms.Label addVideoLabel;
    }
}