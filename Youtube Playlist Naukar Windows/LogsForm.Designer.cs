
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogsForm));
            this.logTable = new System.Windows.Forms.DataGridView();
            this.urlColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.logTable)).BeginInit();
            this.SuspendLayout();
            // 
            // logTable
            // 
            this.logTable.AllowUserToAddRows = false;
            this.logTable.AllowUserToDeleteRows = false;
            this.logTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.logTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.urlColumn,
            this.statusColumn});
            this.logTable.Location = new System.Drawing.Point(23, 35);
            this.logTable.Name = "logTable";
            this.logTable.ReadOnly = true;
            this.logTable.RowHeadersWidth = 62;
            this.logTable.RowTemplate.Height = 33;
            this.logTable.Size = new System.Drawing.Size(739, 403);
            this.logTable.TabIndex = 0;
            // 
            // urlColumn
            // 
            this.urlColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.urlColumn.Frozen = true;
            this.urlColumn.HeaderText = "URL";
            this.urlColumn.MinimumWidth = 8;
            this.urlColumn.Name = "urlColumn";
            this.urlColumn.ReadOnly = true;
            this.urlColumn.Width = 79;
            // 
            // statusColumn
            // 
            this.statusColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.statusColumn.HeaderText = "Status";
            this.statusColumn.MinimumWidth = 8;
            this.statusColumn.Name = "statusColumn";
            this.statusColumn.ReadOnly = true;
            // 
            // LogsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.logTable);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(822, 506);
            this.MinimumSize = new System.Drawing.Size(822, 506);
            this.Name = "LogsForm";
            this.Text = "Video Insertion Progress";
            ((System.ComponentModel.ISupportInitialize)(this.logTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView logTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn urlColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusColumn;
    }
}