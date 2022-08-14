
namespace Youtube_Playlist_Naukar_Windows
{
    partial class PlaylistHomePageForm
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
            this.returnToMainMenu = new System.Windows.Forms.Button();
            this.searchBar = new System.Windows.Forms.TextBox();
            this.findDuplicates = new System.Windows.Forms.Button();
            this.refreshVideos = new System.Windows.Forms.Button();
            this.addVideos = new System.Windows.Forms.Button();
            this.deleteVideo = new System.Windows.Forms.Button();
            this.MessageLogger = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.selectVideoLabel = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.positionLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.urlLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.linkLabel = new System.Windows.Forms.LinkLabel();
            this.title = new System.Windows.Forms.Label();
            this.ownerLabel = new System.Windows.Forms.Label();
            this.ownerLinkLabel = new System.Windows.Forms.LinkLabel();
            this.createdOn = new System.Windows.Forms.Label();
            this.createdOnLabel = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.TitleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Duration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OwnerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // returnToMainMenu
            // 
            this.returnToMainMenu.Location = new System.Drawing.Point(14, 25);
            this.returnToMainMenu.Name = "returnToMainMenu";
            this.returnToMainMenu.Size = new System.Drawing.Size(203, 34);
            this.returnToMainMenu.TabIndex = 0;
            this.returnToMainMenu.Text = "Return to Main Menu";
            this.returnToMainMenu.UseVisualStyleBackColor = true;
            this.returnToMainMenu.Click += new System.EventHandler(this.returnToMainMenu_Click);
            // 
            // searchBar
            // 
            this.searchBar.Location = new System.Drawing.Point(235, 27);
            this.searchBar.Name = "searchBar";
            this.searchBar.PlaceholderText = "Enter query to search playlist videos.";
            this.searchBar.Size = new System.Drawing.Size(721, 31);
            this.searchBar.TabIndex = 1;
            this.searchBar.TextChanged += new System.EventHandler(this.searchBar_TextChanged);
            // 
            // findDuplicates
            // 
            this.findDuplicates.Location = new System.Drawing.Point(851, 211);
            this.findDuplicates.Name = "findDuplicates";
            this.findDuplicates.Size = new System.Drawing.Size(156, 34);
            this.findDuplicates.TabIndex = 3;
            this.findDuplicates.Text = "Find Duplicates";
            this.findDuplicates.UseVisualStyleBackColor = true;
            this.findDuplicates.Click += new System.EventHandler(this.findDuplicates_Click);
            // 
            // refreshVideos
            // 
            this.refreshVideos.Location = new System.Drawing.Point(851, 154);
            this.refreshVideos.Name = "refreshVideos";
            this.refreshVideos.Size = new System.Drawing.Size(155, 34);
            this.refreshVideos.TabIndex = 4;
            this.refreshVideos.Text = "Refresh Videos";
            this.refreshVideos.UseVisualStyleBackColor = true;
            this.refreshVideos.Click += new System.EventHandler(this.refreshVideos_Click);
            // 
            // addVideos
            // 
            this.addVideos.Location = new System.Drawing.Point(851, 97);
            this.addVideos.Name = "addVideos";
            this.addVideos.Size = new System.Drawing.Size(155, 34);
            this.addVideos.TabIndex = 5;
            this.addVideos.Text = "Add Video(s)";
            this.addVideos.UseVisualStyleBackColor = true;
            this.addVideos.Click += new System.EventHandler(this.addVideos_Click);
            // 
            // deleteVideo
            // 
            this.deleteVideo.Location = new System.Drawing.Point(842, 109);
            this.deleteVideo.Name = "deleteVideo";
            this.deleteVideo.Size = new System.Drawing.Size(155, 34);
            this.deleteVideo.TabIndex = 6;
            this.deleteVideo.Text = "Delete Video";
            this.deleteVideo.UseVisualStyleBackColor = true;
            this.deleteVideo.Click += new System.EventHandler(this.deleteVideo_Click);
            // 
            // MessageLogger
            // 
            this.MessageLogger.AutoSize = true;
            this.MessageLogger.Location = new System.Drawing.Point(630, 345);
            this.MessageLogger.Name = "MessageLogger";
            this.MessageLogger.Size = new System.Drawing.Size(0, 25);
            this.MessageLogger.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(230, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(769, 34);
            this.panel1.TabIndex = 9;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Youtube_Playlist_Naukar_Windows.Properties.Resources.search;
            this.pictureBox1.Location = new System.Drawing.Point(727, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(30, 30);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label18);
            this.panel2.Controls.Add(this.label19);
            this.panel2.Controls.Add(this.label15);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.positionLabel);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.urlLabel);
            this.panel2.Controls.Add(this.titleLabel);
            this.panel2.Controls.Add(this.linkLabel);
            this.panel2.Controls.Add(this.title);
            this.panel2.Controls.Add(this.ownerLabel);
            this.panel2.Controls.Add(this.ownerLinkLabel);
            this.panel2.Controls.Add(this.createdOn);
            this.panel2.Controls.Add(this.createdOnLabel);
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Controls.Add(this.deleteVideo);
            this.panel2.Controls.Add(this.selectVideoLabel);
            this.panel2.Location = new System.Drawing.Point(7, 537);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1015, 252);
            this.panel2.TabIndex = 10;
            // 
            // selectVideoLabel
            // 
            this.selectVideoLabel.AutoSize = true;
            this.selectVideoLabel.Location = new System.Drawing.Point(412, 118);
            this.selectVideoLabel.Name = "selectVideoLabel";
            this.selectVideoLabel.Size = new System.Drawing.Size(243, 25);
            this.selectVideoLabel.TabIndex = 39;
            this.selectVideoLabel.Text = "Select a video to view details.";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(499, 210);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(19, 25);
            this.label15.TabIndex = 38;
            this.label15.Text = "-";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(499, 185);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(19, 25);
            this.label14.TabIndex = 37;
            this.label14.Text = "-";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(499, 107);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(19, 25);
            this.label13.TabIndex = 36;
            this.label13.Text = "-";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(499, 82);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(19, 25);
            this.label12.TabIndex = 35;
            this.label12.Text = "-";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(499, 57);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(19, 25);
            this.label11.TabIndex = 34;
            this.label11.Text = "-";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(308, 210);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(114, 25);
            this.label8.TabIndex = 33;
            this.label8.Text = "Description:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(308, 185);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(137, 25);
            this.label7.TabIndex = 32;
            this.label7.Text = "Privacy Status:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(308, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 25);
            this.label6.TabIndex = 31;
            this.label6.Text = "Duration:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(499, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 25);
            this.label5.TabIndex = 30;
            this.label5.Text = "-";
            // 
            // positionLabel
            // 
            this.positionLabel.AutoSize = true;
            this.positionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.positionLabel.Location = new System.Drawing.Point(308, 107);
            this.positionLabel.Name = "positionLabel";
            this.positionLabel.Size = new System.Drawing.Size(172, 25);
            this.positionLabel.TabIndex = 29;
            this.positionLabel.Text = "Position in Playlist:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(308, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 25);
            this.label1.TabIndex = 28;
            this.label1.Text = "URL:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(308, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 25);
            this.label2.TabIndex = 27;
            this.label2.Text = "Title:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(308, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 25);
            this.label3.TabIndex = 26;
            this.label3.Text = "Video Owner:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(308, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 25);
            this.label4.TabIndex = 25;
            this.label4.Text = "Added On:";
            // 
            // urlLabel
            // 
            this.urlLabel.AutoSize = true;
            this.urlLabel.Location = new System.Drawing.Point(412, 565);
            this.urlLabel.Name = "urlLabel";
            this.urlLabel.Size = new System.Drawing.Size(47, 25);
            this.urlLabel.TabIndex = 22;
            this.urlLabel.Text = "URL:";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(412, 540);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(48, 25);
            this.titleLabel.TabIndex = 21;
            this.titleLabel.Text = "Title:";
            // 
            // linkLabel
            // 
            this.linkLabel.AutoSize = true;
            this.linkLabel.Location = new System.Drawing.Point(499, 29);
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Size = new System.Drawing.Size(19, 25);
            this.linkLabel.TabIndex = 17;
            this.linkLabel.TabStop = true;
            this.linkLabel.Text = "-";
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Location = new System.Drawing.Point(584, 540);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(19, 25);
            this.title.TabIndex = 23;
            this.title.Text = "-";
            // 
            // ownerLabel
            // 
            this.ownerLabel.AutoSize = true;
            this.ownerLabel.Location = new System.Drawing.Point(412, 615);
            this.ownerLabel.Name = "ownerLabel";
            this.ownerLabel.Size = new System.Drawing.Size(68, 25);
            this.ownerLabel.TabIndex = 20;
            this.ownerLabel.Text = "Owner:";
            // 
            // ownerLinkLabel
            // 
            this.ownerLinkLabel.AutoSize = true;
            this.ownerLinkLabel.Location = new System.Drawing.Point(499, 143);
            this.ownerLinkLabel.Name = "ownerLinkLabel";
            this.ownerLinkLabel.Size = new System.Drawing.Size(19, 25);
            this.ownerLinkLabel.TabIndex = 18;
            this.ownerLinkLabel.TabStop = true;
            this.ownerLinkLabel.Text = "-";
            // 
            // createdOn
            // 
            this.createdOn.AutoSize = true;
            this.createdOn.Location = new System.Drawing.Point(584, 640);
            this.createdOn.Name = "createdOn";
            this.createdOn.Size = new System.Drawing.Size(19, 25);
            this.createdOn.TabIndex = 24;
            this.createdOn.Text = "-";
            // 
            // createdOnLabel
            // 
            this.createdOnLabel.AutoSize = true;
            this.createdOnLabel.Location = new System.Drawing.Point(412, 640);
            this.createdOnLabel.Name = "createdOnLabel";
            this.createdOnLabel.Size = new System.Drawing.Size(106, 25);
            this.createdOnLabel.TabIndex = 19;
            this.createdOnLabel.Text = "Created On:";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(16, 43);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(277, 157);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label9.Location = new System.Drawing.Point(851, 425);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(133, 25);
            this.label9.TabIndex = 11;
            this.label9.Text = "Playlist Name:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label10.Location = new System.Drawing.Point(851, 354);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(121, 25);
            this.label10.TabIndex = 12;
            this.label10.Text = "Total Videos:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TitleColumn,
            this.Duration,
            this.OwnerName});
            this.dataGridView1.Location = new System.Drawing.Point(23, 82);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 33;
            this.dataGridView1.Size = new System.Drawing.Size(805, 449);
            this.dataGridView1.TabIndex = 13;
            // 
            // TitleColumn
            // 
            this.TitleColumn.Frozen = true;
            this.TitleColumn.HeaderText = "Title";
            this.TitleColumn.MinimumWidth = 8;
            this.TitleColumn.Name = "TitleColumn";
            this.TitleColumn.ReadOnly = true;
            this.TitleColumn.Width = 150;
            // 
            // Duration
            // 
            this.Duration.Frozen = true;
            this.Duration.HeaderText = "Duration";
            this.Duration.MinimumWidth = 8;
            this.Duration.Name = "Duration";
            this.Duration.ReadOnly = true;
            this.Duration.Width = 150;
            // 
            // OwnerName
            // 
            this.OwnerName.Frozen = true;
            this.OwnerName.HeaderText = "Owner Name";
            this.OwnerName.MinimumWidth = 8;
            this.OwnerName.Name = "OwnerName";
            this.OwnerName.ReadOnly = true;
            this.OwnerName.Width = 150;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(860, 388);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(19, 25);
            this.label16.TabIndex = 39;
            this.label16.Text = "-";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(861, 463);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(19, 25);
            this.label17.TabIndex = 40;
            this.label17.Text = "-";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(851, 269);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(156, 63);
            this.button1.TabIndex = 41;
            this.button1.Text = "View Private Videos";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label19.Location = new System.Drawing.Point(308, 132);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(100, 25);
            this.label19.TabIndex = 40;
            this.label19.Text = "Added By:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(499, 175);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(19, 25);
            this.label18.TabIndex = 41;
            this.label18.Text = "-";
            // 
            // PlaylistHomePageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 794);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.MessageLogger);
            this.Controls.Add(this.addVideos);
            this.Controls.Add(this.refreshVideos);
            this.Controls.Add(this.findDuplicates);
            this.Controls.Add(this.searchBar);
            this.Controls.Add(this.returnToMainMenu);
            this.Controls.Add(this.panel1);
            this.Name = "PlaylistHomePageForm";
            this.Text = "PlaylistHomePageForm";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button returnToMainMenu;
        private System.Windows.Forms.TextBox searchBar;
        private System.Windows.Forms.Button findDuplicates;
        private System.Windows.Forms.Button refreshVideos;
        private System.Windows.Forms.Button addVideos;
        private System.Windows.Forms.Button deleteVideo;
        private System.Windows.Forms.Label MessageLogger;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label urlLabel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.LinkLabel linkLabel;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label ownerLabel;
        private System.Windows.Forms.LinkLabel ownerLinkLabel;
        private System.Windows.Forms.Label createdOn;
        private System.Windows.Forms.Label createdOnLabel;
        private System.Windows.Forms.Label positionLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label selectVideoLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn TitleColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Duration;
        private System.Windows.Forms.DataGridViewTextBoxColumn OwnerName;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
    }
}