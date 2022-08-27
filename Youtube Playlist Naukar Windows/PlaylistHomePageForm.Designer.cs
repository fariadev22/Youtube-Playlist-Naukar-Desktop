
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
            this.components = new System.ComponentModel.Container();
            this.searchBar = new System.Windows.Forms.TextBox();
            this.findDuplicatesButton = new System.Windows.Forms.Button();
            this.refreshVideosButton = new System.Windows.Forms.Button();
            this.addVideosButton = new System.Windows.Forms.Button();
            this.deleteVideoButton = new System.Windows.Forms.Button();
            this.MessageLogger = new System.Windows.Forms.Label();
            this.searchPanel = new System.Windows.Forms.Panel();
            this.searchIcon = new System.Windows.Forms.PictureBox();
            this.videoPreviewThumbnail = new System.Windows.Forms.Panel();
            this.addedByValue = new System.Windows.Forms.LinkLabel();
            this.playlistPositionValue = new System.Windows.Forms.Label();
            this.privacyStatusValue = new System.Windows.Forms.Label();
            this.addedByLabel = new System.Windows.Forms.Label();
            this.descriptionValue = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.addedOnValue = new System.Windows.Forms.Label();
            this.durationValue = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.privacyStatusLabel = new System.Windows.Forms.Label();
            this.durationLabel = new System.Windows.Forms.Label();
            this.titleValue = new System.Windows.Forms.Label();
            this.playlistPositionLabel = new System.Windows.Forms.Label();
            this.urlLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.videoOwnerLabel = new System.Windows.Forms.Label();
            this.addedOnLabel = new System.Windows.Forms.Label();
            this.urlValue = new System.Windows.Forms.LinkLabel();
            this.title = new System.Windows.Forms.Label();
            this.ownerLabel = new System.Windows.Forms.Label();
            this.videoOwnerValue = new System.Windows.Forms.LinkLabel();
            this.createdOn = new System.Windows.Forms.Label();
            this.createdOnLabel = new System.Windows.Forms.Label();
            this.videoThumbnailPreview = new System.Windows.Forms.PictureBox();
            this.playlistNameLabel = new System.Windows.Forms.Label();
            this.totalVideosLabel = new System.Windows.Forms.Label();
            this.playlistVideosDataView = new System.Windows.Forms.DataGridView();
            this.totalVideosValue = new System.Windows.Forms.Label();
            this.playlistNameValue = new System.Windows.Forms.Label();
            this.viewPrivateVideosButton = new System.Windows.Forms.Button();
            this.returnHomeButton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.searchPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchIcon)).BeginInit();
            this.videoPreviewThumbnail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.videoThumbnailPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playlistVideosDataView)).BeginInit();
            this.SuspendLayout();
            // 
            // searchBar
            // 
            this.searchBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchBar.Location = new System.Drawing.Point(241, 27);
            this.searchBar.Name = "searchBar";
            this.searchBar.PlaceholderText = "Enter query to search playlist videos.";
            this.searchBar.Size = new System.Drawing.Size(715, 31);
            this.searchBar.TabIndex = 1;
            this.searchBar.TextChanged += new System.EventHandler(this.searchBar_TextChanged);
            // 
            // findDuplicatesButton
            // 
            this.findDuplicatesButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.findDuplicatesButton.Location = new System.Drawing.Point(851, 211);
            this.findDuplicatesButton.Name = "findDuplicatesButton";
            this.findDuplicatesButton.Size = new System.Drawing.Size(156, 34);
            this.findDuplicatesButton.TabIndex = 3;
            this.findDuplicatesButton.Text = "Find Duplicates";
            this.findDuplicatesButton.UseVisualStyleBackColor = true;
            this.findDuplicatesButton.Click += new System.EventHandler(this.findDuplicates_Click);
            // 
            // refreshVideosButton
            // 
            this.refreshVideosButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.refreshVideosButton.Location = new System.Drawing.Point(851, 154);
            this.refreshVideosButton.Name = "refreshVideosButton";
            this.refreshVideosButton.Size = new System.Drawing.Size(155, 34);
            this.refreshVideosButton.TabIndex = 4;
            this.refreshVideosButton.Text = "Refresh Videos";
            this.refreshVideosButton.UseVisualStyleBackColor = true;
            this.refreshVideosButton.Click += new System.EventHandler(this.refreshVideos_Click);
            // 
            // addVideosButton
            // 
            this.addVideosButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.addVideosButton.Location = new System.Drawing.Point(851, 97);
            this.addVideosButton.Name = "addVideosButton";
            this.addVideosButton.Size = new System.Drawing.Size(155, 34);
            this.addVideosButton.TabIndex = 5;
            this.addVideosButton.Text = "Add Video(s)";
            this.addVideosButton.UseVisualStyleBackColor = true;
            this.addVideosButton.Click += new System.EventHandler(this.addVideos_Click);
            // 
            // deleteVideoButton
            // 
            this.deleteVideoButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.deleteVideoButton.Location = new System.Drawing.Point(842, 109);
            this.deleteVideoButton.Name = "deleteVideoButton";
            this.deleteVideoButton.Size = new System.Drawing.Size(155, 34);
            this.deleteVideoButton.TabIndex = 6;
            this.deleteVideoButton.Text = "Delete Video";
            this.deleteVideoButton.UseVisualStyleBackColor = true;
            this.deleteVideoButton.Click += new System.EventHandler(this.deleteVideo_Click);
            // 
            // MessageLogger
            // 
            this.MessageLogger.AutoSize = true;
            this.MessageLogger.Location = new System.Drawing.Point(630, 345);
            this.MessageLogger.Name = "MessageLogger";
            this.MessageLogger.Size = new System.Drawing.Size(0, 25);
            this.MessageLogger.TabIndex = 8;
            // 
            // searchPanel
            // 
            this.searchPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchPanel.Controls.Add(this.searchIcon);
            this.searchPanel.Location = new System.Drawing.Point(241, 26);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(758, 34);
            this.searchPanel.TabIndex = 9;
            // 
            // searchIcon
            // 
            this.searchIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchIcon.Image = global::Youtube_Playlist_Naukar_Windows.Properties.Resources.search;
            this.searchIcon.Location = new System.Drawing.Point(727, 2);
            this.searchIcon.Name = "searchIcon";
            this.searchIcon.Size = new System.Drawing.Size(30, 30);
            this.searchIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.searchIcon.TabIndex = 0;
            this.searchIcon.TabStop = false;
            // 
            // videoPreviewThumbnail
            // 
            this.videoPreviewThumbnail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.videoPreviewThumbnail.Controls.Add(this.addedByValue);
            this.videoPreviewThumbnail.Controls.Add(this.playlistPositionValue);
            this.videoPreviewThumbnail.Controls.Add(this.privacyStatusValue);
            this.videoPreviewThumbnail.Controls.Add(this.addedByLabel);
            this.videoPreviewThumbnail.Controls.Add(this.descriptionValue);
            this.videoPreviewThumbnail.Controls.Add(this.label14);
            this.videoPreviewThumbnail.Controls.Add(this.addedOnValue);
            this.videoPreviewThumbnail.Controls.Add(this.durationValue);
            this.videoPreviewThumbnail.Controls.Add(this.descriptionLabel);
            this.videoPreviewThumbnail.Controls.Add(this.privacyStatusLabel);
            this.videoPreviewThumbnail.Controls.Add(this.durationLabel);
            this.videoPreviewThumbnail.Controls.Add(this.titleValue);
            this.videoPreviewThumbnail.Controls.Add(this.playlistPositionLabel);
            this.videoPreviewThumbnail.Controls.Add(this.urlLabel);
            this.videoPreviewThumbnail.Controls.Add(this.titleLabel);
            this.videoPreviewThumbnail.Controls.Add(this.videoOwnerLabel);
            this.videoPreviewThumbnail.Controls.Add(this.addedOnLabel);
            this.videoPreviewThumbnail.Controls.Add(this.urlValue);
            this.videoPreviewThumbnail.Controls.Add(this.title);
            this.videoPreviewThumbnail.Controls.Add(this.ownerLabel);
            this.videoPreviewThumbnail.Controls.Add(this.videoOwnerValue);
            this.videoPreviewThumbnail.Controls.Add(this.createdOn);
            this.videoPreviewThumbnail.Controls.Add(this.createdOnLabel);
            this.videoPreviewThumbnail.Controls.Add(this.videoThumbnailPreview);
            this.videoPreviewThumbnail.Controls.Add(this.deleteVideoButton);
            this.videoPreviewThumbnail.Location = new System.Drawing.Point(7, 537);
            this.videoPreviewThumbnail.Name = "videoPreviewThumbnail";
            this.videoPreviewThumbnail.Size = new System.Drawing.Size(1015, 252);
            this.videoPreviewThumbnail.TabIndex = 10;
            // 
            // addedByValue
            // 
            this.addedByValue.AutoSize = true;
            this.addedByValue.Location = new System.Drawing.Point(498, 140);
            this.addedByValue.Name = "addedByValue";
            this.addedByValue.Size = new System.Drawing.Size(19, 25);
            this.addedByValue.TabIndex = 44;
            this.addedByValue.TabStop = true;
            this.addedByValue.Text = "-";
            // 
            // playlistPositionValue
            // 
            this.playlistPositionValue.AutoSize = true;
            this.playlistPositionValue.Location = new System.Drawing.Point(498, 110);
            this.playlistPositionValue.Name = "playlistPositionValue";
            this.playlistPositionValue.Size = new System.Drawing.Size(19, 25);
            this.playlistPositionValue.TabIndex = 42;
            this.playlistPositionValue.Text = "-";
            // 
            // privacyStatusValue
            // 
            this.privacyStatusValue.AutoSize = true;
            this.privacyStatusValue.Location = new System.Drawing.Point(497, 192);
            this.privacyStatusValue.Name = "privacyStatusValue";
            this.privacyStatusValue.Size = new System.Drawing.Size(19, 25);
            this.privacyStatusValue.TabIndex = 41;
            this.privacyStatusValue.Text = "-";
            // 
            // addedByLabel
            // 
            this.addedByLabel.AutoSize = true;
            this.addedByLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.addedByLabel.Location = new System.Drawing.Point(308, 137);
            this.addedByLabel.Name = "addedByLabel";
            this.addedByLabel.Size = new System.Drawing.Size(100, 25);
            this.addedByLabel.TabIndex = 40;
            this.addedByLabel.Text = "Added By:";
            // 
            // descriptionValue
            // 
            this.descriptionValue.AutoSize = true;
            this.descriptionValue.Location = new System.Drawing.Point(497, 217);
            this.descriptionValue.Name = "descriptionValue";
            this.descriptionValue.Size = new System.Drawing.Size(19, 25);
            this.descriptionValue.TabIndex = 38;
            this.descriptionValue.Text = "-";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(497, 188);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(19, 25);
            this.label14.TabIndex = 37;
            this.label14.Text = "-";
            // 
            // addedOnValue
            // 
            this.addedOnValue.AutoSize = true;
            this.addedOnValue.Location = new System.Drawing.Point(497, 85);
            this.addedOnValue.Name = "addedOnValue";
            this.addedOnValue.Size = new System.Drawing.Size(19, 25);
            this.addedOnValue.TabIndex = 35;
            this.addedOnValue.Text = "-";
            // 
            // durationValue
            // 
            this.durationValue.AutoSize = true;
            this.durationValue.Location = new System.Drawing.Point(497, 60);
            this.durationValue.Name = "durationValue";
            this.durationValue.Size = new System.Drawing.Size(19, 25);
            this.durationValue.TabIndex = 34;
            this.durationValue.Text = "-";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.descriptionLabel.Location = new System.Drawing.Point(308, 214);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(114, 25);
            this.descriptionLabel.TabIndex = 33;
            this.descriptionLabel.Text = "Description:";
            // 
            // privacyStatusLabel
            // 
            this.privacyStatusLabel.AutoSize = true;
            this.privacyStatusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.privacyStatusLabel.Location = new System.Drawing.Point(308, 188);
            this.privacyStatusLabel.Name = "privacyStatusLabel";
            this.privacyStatusLabel.Size = new System.Drawing.Size(137, 25);
            this.privacyStatusLabel.TabIndex = 32;
            this.privacyStatusLabel.Text = "Privacy Status:";
            // 
            // durationLabel
            // 
            this.durationLabel.AutoSize = true;
            this.durationLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.durationLabel.Location = new System.Drawing.Point(308, 57);
            this.durationLabel.Name = "durationLabel";
            this.durationLabel.Size = new System.Drawing.Size(92, 25);
            this.durationLabel.TabIndex = 31;
            this.durationLabel.Text = "Duration:";
            // 
            // titleValue
            // 
            this.titleValue.AutoSize = true;
            this.titleValue.Location = new System.Drawing.Point(497, 10);
            this.titleValue.Name = "titleValue";
            this.titleValue.Size = new System.Drawing.Size(19, 25);
            this.titleValue.TabIndex = 30;
            this.titleValue.Text = "-";
            // 
            // playlistPositionLabel
            // 
            this.playlistPositionLabel.AutoSize = true;
            this.playlistPositionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.playlistPositionLabel.Location = new System.Drawing.Point(308, 109);
            this.playlistPositionLabel.Name = "playlistPositionLabel";
            this.playlistPositionLabel.Size = new System.Drawing.Size(172, 25);
            this.playlistPositionLabel.TabIndex = 29;
            this.playlistPositionLabel.Text = "Position in Playlist:";
            // 
            // urlLabel
            // 
            this.urlLabel.AutoSize = true;
            this.urlLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.urlLabel.Location = new System.Drawing.Point(308, 32);
            this.urlLabel.Name = "urlLabel";
            this.urlLabel.Size = new System.Drawing.Size(51, 25);
            this.urlLabel.TabIndex = 28;
            this.urlLabel.Text = "URL:";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.titleLabel.Location = new System.Drawing.Point(308, 7);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(55, 25);
            this.titleLabel.TabIndex = 27;
            this.titleLabel.Text = "Title:";
            // 
            // videoOwnerLabel
            // 
            this.videoOwnerLabel.AutoSize = true;
            this.videoOwnerLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.videoOwnerLabel.Location = new System.Drawing.Point(308, 163);
            this.videoOwnerLabel.Name = "videoOwnerLabel";
            this.videoOwnerLabel.Size = new System.Drawing.Size(128, 25);
            this.videoOwnerLabel.TabIndex = 26;
            this.videoOwnerLabel.Text = "Video Owner:";
            // 
            // addedOnLabel
            // 
            this.addedOnLabel.AutoSize = true;
            this.addedOnLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.addedOnLabel.Location = new System.Drawing.Point(308, 83);
            this.addedOnLabel.Name = "addedOnLabel";
            this.addedOnLabel.Size = new System.Drawing.Size(103, 25);
            this.addedOnLabel.TabIndex = 25;
            this.addedOnLabel.Text = "Added On:";
            // 
            // urlValue
            // 
            this.urlValue.AutoSize = true;
            this.urlValue.Location = new System.Drawing.Point(497, 32);
            this.urlValue.Name = "urlValue";
            this.urlValue.Size = new System.Drawing.Size(19, 25);
            this.urlValue.TabIndex = 17;
            this.urlValue.TabStop = true;
            this.urlValue.Text = "-";
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
            // videoOwnerValue
            // 
            this.videoOwnerValue.AutoSize = true;
            this.videoOwnerValue.Location = new System.Drawing.Point(497, 163);
            this.videoOwnerValue.Name = "videoOwnerValue";
            this.videoOwnerValue.Size = new System.Drawing.Size(19, 25);
            this.videoOwnerValue.TabIndex = 18;
            this.videoOwnerValue.TabStop = true;
            this.videoOwnerValue.Text = "-";
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
            // videoThumbnailPreview
            // 
            this.videoThumbnailPreview.ImageLocation = "default_image.png";
            this.videoThumbnailPreview.Location = new System.Drawing.Point(16, 43);
            this.videoThumbnailPreview.Name = "videoThumbnailPreview";
            this.videoThumbnailPreview.Size = new System.Drawing.Size(277, 157);
            this.videoThumbnailPreview.TabIndex = 0;
            this.videoThumbnailPreview.TabStop = false;
            // 
            // playlistNameLabel
            // 
            this.playlistNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.playlistNameLabel.AutoSize = true;
            this.playlistNameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.playlistNameLabel.Location = new System.Drawing.Point(851, 425);
            this.playlistNameLabel.Name = "playlistNameLabel";
            this.playlistNameLabel.Size = new System.Drawing.Size(133, 25);
            this.playlistNameLabel.TabIndex = 11;
            this.playlistNameLabel.Text = "Playlist Name:";
            // 
            // totalVideosLabel
            // 
            this.totalVideosLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.totalVideosLabel.AutoSize = true;
            this.totalVideosLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.totalVideosLabel.Location = new System.Drawing.Point(851, 354);
            this.totalVideosLabel.Name = "totalVideosLabel";
            this.totalVideosLabel.Size = new System.Drawing.Size(121, 25);
            this.totalVideosLabel.TabIndex = 12;
            this.totalVideosLabel.Text = "Total Videos:";
            // 
            // playlistVideosDataView
            // 
            this.playlistVideosDataView.AllowUserToAddRows = false;
            this.playlistVideosDataView.AllowUserToDeleteRows = false;
            this.playlistVideosDataView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playlistVideosDataView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.playlistVideosDataView.Location = new System.Drawing.Point(23, 82);
            this.playlistVideosDataView.Name = "playlistVideosDataView";
            this.playlistVideosDataView.ReadOnly = true;
            this.playlistVideosDataView.RowHeadersWidth = 62;
            this.playlistVideosDataView.RowTemplate.Height = 84;
            this.playlistVideosDataView.Size = new System.Drawing.Size(805, 449);
            this.playlistVideosDataView.TabIndex = 13;
            // 
            // totalVideosValue
            // 
            this.totalVideosValue.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.totalVideosValue.AutoSize = true;
            this.totalVideosValue.Location = new System.Drawing.Point(860, 388);
            this.totalVideosValue.Name = "totalVideosValue";
            this.totalVideosValue.Size = new System.Drawing.Size(19, 25);
            this.totalVideosValue.TabIndex = 39;
            this.totalVideosValue.Text = "-";
            // 
            // playlistNameValue
            // 
            this.playlistNameValue.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.playlistNameValue.AutoSize = true;
            this.playlistNameValue.Location = new System.Drawing.Point(861, 463);
            this.playlistNameValue.Name = "playlistNameValue";
            this.playlistNameValue.Size = new System.Drawing.Size(19, 25);
            this.playlistNameValue.TabIndex = 40;
            this.playlistNameValue.Text = "-";
            // 
            // viewPrivateVideosButton
            // 
            this.viewPrivateVideosButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.viewPrivateVideosButton.Location = new System.Drawing.Point(851, 269);
            this.viewPrivateVideosButton.Name = "viewPrivateVideosButton";
            this.viewPrivateVideosButton.Size = new System.Drawing.Size(156, 63);
            this.viewPrivateVideosButton.TabIndex = 41;
            this.viewPrivateVideosButton.Text = "View Private Videos";
            this.viewPrivateVideosButton.UseVisualStyleBackColor = true;
            this.viewPrivateVideosButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // returnHomeButton
            // 
            this.returnHomeButton.Location = new System.Drawing.Point(25, 26);
            this.returnHomeButton.Name = "returnHomeButton";
            this.returnHomeButton.Size = new System.Drawing.Size(201, 34);
            this.returnHomeButton.TabIndex = 42;
            this.returnHomeButton.Text = "Return to Home";
            this.returnHomeButton.UseVisualStyleBackColor = true;
            this.returnHomeButton.Click += new System.EventHandler(this.returnHomeButton_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // PlaylistHomePageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 794);
            this.Controls.Add(this.returnHomeButton);
            this.Controls.Add(this.viewPrivateVideosButton);
            this.Controls.Add(this.playlistNameValue);
            this.Controls.Add(this.totalVideosValue);
            this.Controls.Add(this.playlistVideosDataView);
            this.Controls.Add(this.totalVideosLabel);
            this.Controls.Add(this.playlistNameLabel);
            this.Controls.Add(this.videoPreviewThumbnail);
            this.Controls.Add(this.MessageLogger);
            this.Controls.Add(this.addVideosButton);
            this.Controls.Add(this.refreshVideosButton);
            this.Controls.Add(this.findDuplicatesButton);
            this.Controls.Add(this.searchBar);
            this.Controls.Add(this.searchPanel);
            this.Name = "PlaylistHomePageForm";
            this.Text = "PlaylistHomePageForm";
            this.searchPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.searchIcon)).EndInit();
            this.videoPreviewThumbnail.ResumeLayout(false);
            this.videoPreviewThumbnail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.videoThumbnailPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playlistVideosDataView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox searchBar;
        private System.Windows.Forms.Button findDuplicatesButton;
        private System.Windows.Forms.Button refreshVideosButton;
        private System.Windows.Forms.Button addVideosButton;
        private System.Windows.Forms.Button deleteVideoButton;
        private System.Windows.Forms.Label MessageLogger;
        private System.Windows.Forms.Panel searchPanel;
        private System.Windows.Forms.PictureBox searchIcon;
        private System.Windows.Forms.Panel videoPreviewThumbnail;
        private System.Windows.Forms.PictureBox videoThumbnailPreview;
        private System.Windows.Forms.LinkLabel urlValue;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label ownerLabel;
        private System.Windows.Forms.LinkLabel videoOwnerValue;
        private System.Windows.Forms.Label createdOn;
        private System.Windows.Forms.Label createdOnLabel;
        private System.Windows.Forms.Label playlistPositionLabel;
        private System.Windows.Forms.Label urlLabel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label videoOwnerLabel;
        private System.Windows.Forms.Label addedOnLabel;
        private System.Windows.Forms.Label titleValue;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label privacyStatusLabel;
        private System.Windows.Forms.Label durationLabel;
        private System.Windows.Forms.Label playlistNameLabel;
        private System.Windows.Forms.Label totalVideosLabel;
        private System.Windows.Forms.DataGridView playlistVideosDataView;
        private System.Windows.Forms.Label descriptionValue;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label addedOnValue;
        private System.Windows.Forms.Label durationValue;
        private System.Windows.Forms.Label totalVideosValue;
        private System.Windows.Forms.Label playlistNameValue;
        private System.Windows.Forms.Button viewPrivateVideosButton;
        private System.Windows.Forms.Label addedByLabel;
        private System.Windows.Forms.Label privacyStatusValue;
        private System.Windows.Forms.Label playlistPositionValue;
        private System.Windows.Forms.LinkLabel addedByValue;
        private System.Windows.Forms.Button returnHomeButton;
        private System.Windows.Forms.Timer timer1;
    }
}