
namespace Youtube_Playlist_Naukar_Windows
{
    partial class LogsForm
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
            this.logMessagesList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // logMessagesList
            // 
            this.logMessagesList.FormattingEnabled = true;
            this.logMessagesList.ItemHeight = 25;
            this.logMessagesList.Location = new System.Drawing.Point(12, 9);
            this.logMessagesList.Name = "logMessagesList";
            this.logMessagesList.Size = new System.Drawing.Size(776, 429);
            this.logMessagesList.TabIndex = 0;
            // 
            // LogsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.logMessagesList);
            this.Name = "LogsForm";
            this.Text = "LogsForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox logMessagesList;
    }
}