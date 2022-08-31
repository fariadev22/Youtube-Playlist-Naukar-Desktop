
using System.Windows.Forms;

namespace Youtube_Playlist_Naukar_Windows
{
    partial class HomePageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomePageForm));
            this.email = new System.Windows.Forms.Label();
            this.playlistsTabs = new System.Windows.Forms.TabControl();
            this.ownerTab = new System.Windows.Forms.TabPage();
            this.ownerPlaylistsList = new System.Windows.Forms.ListView();
            this.contributorTab = new System.Windows.Forms.TabPage();
            this.contributorPlaylistsList = new System.Windows.Forms.ListView();
            this.removeContributorPlaylistButton = new System.Windows.Forms.Button();
            this.addContributorPlaylistButton = new System.Windows.Forms.Button();
            this.refreshPlaylistsButton = new System.Windows.Forms.Button();
            this.choosePlaylistLabel = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.playlistsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshPlaylistsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.switchAccountMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forgetCurrentAccountMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewAccountMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.viewDetailsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoggerLabel = new System.Windows.Forms.Label();
            this.playlistThumbnailPreview = new System.Windows.Forms.PictureBox();
            this.createdOnLabel = new System.Windows.Forms.Label();
            this.ownerLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.urlLabel = new System.Windows.Forms.Label();
            this.totalVideosLabel = new System.Windows.Forms.Label();
            this.titleValue = new System.Windows.Forms.Label();
            this.privacyStatusValue = new System.Windows.Forms.Label();
            this.totalVideosValue = new System.Windows.Forms.Label();
            this.playlistDetailsPanel = new System.Windows.Forms.Panel();
            this.descriptionValue = new System.Windows.Forms.Label();
            this.createdOnValue = new System.Windows.Forms.Label();
            this.privacyStatusLabel = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.ownerValue = new System.Windows.Forms.LinkLabel();
            this.urlValue = new System.Windows.Forms.LinkLabel();
            this.descriptionToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.playlistsTabs.SuspendLayout();
            this.ownerTab.SuspendLayout();
            this.contributorTab.SuspendLayout();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playlistThumbnailPreview)).BeginInit();
            this.playlistDetailsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // email
            // 
            this.email.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.email.AutoSize = true;
            this.email.Location = new System.Drawing.Point(1022, 46);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(19, 25);
            this.email.TabIndex = 0;
            this.email.Text = "-";
            // 
            // playlistsTabs
            // 
            this.playlistsTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playlistsTabs.Controls.Add(this.ownerTab);
            this.playlistsTabs.Controls.Add(this.contributorTab);
            this.playlistsTabs.Location = new System.Drawing.Point(16, 115);
            this.playlistsTabs.Name = "playlistsTabs";
            this.playlistsTabs.SelectedIndex = 0;
            this.playlistsTabs.Size = new System.Drawing.Size(1040, 459);
            this.playlistsTabs.TabIndex = 1;
            // 
            // ownerTab
            // 
            this.ownerTab.Controls.Add(this.ownerPlaylistsList);
            this.ownerTab.Location = new System.Drawing.Point(4, 34);
            this.ownerTab.Name = "ownerTab";
            this.ownerTab.Padding = new System.Windows.Forms.Padding(3);
            this.ownerTab.Size = new System.Drawing.Size(1032, 421);
            this.ownerTab.TabIndex = 0;
            this.ownerTab.Text = "Owner Playlists";
            this.ownerTab.UseVisualStyleBackColor = true;
            // 
            // ownerPlaylistsList
            // 
            this.ownerPlaylistsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ownerPlaylistsList.HideSelection = false;
            this.ownerPlaylistsList.Location = new System.Drawing.Point(6, 6);
            this.ownerPlaylistsList.Name = "ownerPlaylistsList";
            this.ownerPlaylistsList.Size = new System.Drawing.Size(1015, 412);
            this.ownerPlaylistsList.TabIndex = 0;
            this.ownerPlaylistsList.UseCompatibleStateImageBehavior = false;
            this.ownerPlaylistsList.SelectedIndexChanged += new System.EventHandler(this.OwnerPlaylistsList_SelectedIndexChanged);
            this.ownerPlaylistsList.DoubleClick += new System.EventHandler(this.OwnerPlaylistsList_DoubleClicked);
            // 
            // contributorTab
            // 
            this.contributorTab.Controls.Add(this.contributorPlaylistsList);
            this.contributorTab.Controls.Add(this.removeContributorPlaylistButton);
            this.contributorTab.Controls.Add(this.addContributorPlaylistButton);
            this.contributorTab.Location = new System.Drawing.Point(4, 34);
            this.contributorTab.Name = "contributorTab";
            this.contributorTab.Padding = new System.Windows.Forms.Padding(3);
            this.contributorTab.Size = new System.Drawing.Size(1032, 421);
            this.contributorTab.TabIndex = 1;
            this.contributorTab.Text = "Contributor Playlists";
            this.contributorTab.UseVisualStyleBackColor = true;
            // 
            // contributorPlaylistsList
            // 
            this.contributorPlaylistsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.contributorPlaylistsList.HideSelection = false;
            this.contributorPlaylistsList.Location = new System.Drawing.Point(6, 54);
            this.contributorPlaylistsList.Name = "contributorPlaylistsList";
            this.contributorPlaylistsList.Size = new System.Drawing.Size(1020, 361);
            this.contributorPlaylistsList.TabIndex = 4;
            this.contributorPlaylistsList.UseCompatibleStateImageBehavior = false;
            this.contributorPlaylistsList.SelectedIndexChanged += new System.EventHandler(this.ContributorPlaylistsList_SelectedIndexChanged);
            this.contributorPlaylistsList.DoubleClick += new System.EventHandler(this.ContributorPlaylistsList_DoubleClicked);
            // 
            // removeContributorPlaylistButton
            // 
            this.removeContributorPlaylistButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeContributorPlaylistButton.Location = new System.Drawing.Point(914, 11);
            this.removeContributorPlaylistButton.Name = "removeContributorPlaylistButton";
            this.removeContributorPlaylistButton.Size = new System.Drawing.Size(112, 34);
            this.removeContributorPlaylistButton.TabIndex = 3;
            this.removeContributorPlaylistButton.Text = "Remove";
            this.removeContributorPlaylistButton.UseVisualStyleBackColor = true;
            this.removeContributorPlaylistButton.Click += new System.EventHandler(this.RemoveContributorPlaylistButton_Click);
            // 
            // addContributorPlaylistButton
            // 
            this.addContributorPlaylistButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addContributorPlaylistButton.Location = new System.Drawing.Point(786, 11);
            this.addContributorPlaylistButton.Name = "addContributorPlaylistButton";
            this.addContributorPlaylistButton.Size = new System.Drawing.Size(122, 34);
            this.addContributorPlaylistButton.TabIndex = 0;
            this.addContributorPlaylistButton.Text = "Add";
            this.addContributorPlaylistButton.UseVisualStyleBackColor = true;
            this.addContributorPlaylistButton.Click += new System.EventHandler(this.AddContributorPlaylistButton_Click);
            // 
            // refreshPlaylistsButton
            // 
            this.refreshPlaylistsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshPlaylistsButton.Location = new System.Drawing.Point(934, 102);
            this.refreshPlaylistsButton.Name = "refreshPlaylistsButton";
            this.refreshPlaylistsButton.Size = new System.Drawing.Size(112, 34);
            this.refreshPlaylistsButton.TabIndex = 2;
            this.refreshPlaylistsButton.Text = "Refresh";
            this.refreshPlaylistsButton.UseVisualStyleBackColor = true;
            this.refreshPlaylistsButton.Click += new System.EventHandler(this.RefreshPlaylistsButton_Click);
            // 
            // choosePlaylistLabel
            // 
            this.choosePlaylistLabel.AutoSize = true;
            this.choosePlaylistLabel.Location = new System.Drawing.Point(16, 77);
            this.choosePlaylistLabel.Name = "choosePlaylistLabel";
            this.choosePlaylistLabel.Size = new System.Drawing.Size(241, 25);
            this.choosePlaylistLabel.TabIndex = 3;
            this.choosePlaylistLabel.Text = "Choose a playlist to manage:";
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playlistsMenu,
            this.accountMenu,
            this.aboutMenu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1068, 33);
            this.menuStrip.TabIndex = 4;
            this.menuStrip.Text = "menuStrip1";
            // 
            // playlistsMenu
            // 
            this.playlistsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshPlaylistsMenuItem});
            this.playlistsMenu.Name = "playlistsMenu";
            this.playlistsMenu.Size = new System.Drawing.Size(90, 29);
            this.playlistsMenu.Text = "Playlists";
            // 
            // refreshPlaylistsMenuItem
            // 
            this.refreshPlaylistsMenuItem.Name = "refreshPlaylistsMenuItem";
            this.refreshPlaylistsMenuItem.Size = new System.Drawing.Size(239, 34);
            this.refreshPlaylistsMenuItem.Text = "Refresh Playlists";
            this.refreshPlaylistsMenuItem.Click += new System.EventHandler(this.RefreshPlaylistsMenuItem_Click);
            // 
            // accountMenu
            // 
            this.accountMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.switchAccountMenuItem,
            this.forgetCurrentAccountMenuItem,
            this.addNewAccountMenuItem});
            this.accountMenu.Name = "accountMenu";
            this.accountMenu.Size = new System.Drawing.Size(93, 29);
            this.accountMenu.Text = "Account";
            // 
            // switchAccountMenuItem
            // 
            this.switchAccountMenuItem.Name = "switchAccountMenuItem";
            this.switchAccountMenuItem.Size = new System.Drawing.Size(299, 34);
            this.switchAccountMenuItem.Text = "Switch Account";
            this.switchAccountMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.SwitchAccountWithSelectedEmail);
            // 
            // forgetCurrentAccountMenuItem
            // 
            this.forgetCurrentAccountMenuItem.Name = "forgetCurrentAccountMenuItem";
            this.forgetCurrentAccountMenuItem.Size = new System.Drawing.Size(299, 34);
            this.forgetCurrentAccountMenuItem.Text = "Forget Current Account";
            this.forgetCurrentAccountMenuItem.Click += new System.EventHandler(this.ForgetCurrentAccountMenuItem_Click);
            // 
            // addNewAccountMenuItem
            // 
            this.addNewAccountMenuItem.Name = "addNewAccountMenuItem";
            this.addNewAccountMenuItem.Size = new System.Drawing.Size(299, 34);
            this.addNewAccountMenuItem.Text = "Add New Account";
            this.addNewAccountMenuItem.Click += new System.EventHandler(this.AddNewAccountMenuItem_Click);
            // 
            // aboutMenu
            // 
            this.aboutMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewDetailsMenuItem});
            this.aboutMenu.Name = "aboutMenu";
            this.aboutMenu.Size = new System.Drawing.Size(78, 29);
            this.aboutMenu.Text = "About";
            // 
            // viewDetailsMenuItem
            // 
            this.viewDetailsMenuItem.Name = "viewDetailsMenuItem";
            this.viewDetailsMenuItem.Size = new System.Drawing.Size(209, 34);
            this.viewDetailsMenuItem.Text = "View Details";
            this.viewDetailsMenuItem.Click += new System.EventHandler(this.ViewDetailsMenuItem_Click);
            // 
            // LoggerLabel
            // 
            this.LoggerLabel.AutoSize = true;
            this.LoggerLabel.Location = new System.Drawing.Point(516, 115);
            this.LoggerLabel.Name = "LoggerLabel";
            this.LoggerLabel.Size = new System.Drawing.Size(0, 25);
            this.LoggerLabel.TabIndex = 5;
            // 
            // playlistThumbnailPreview
            // 
            this.playlistThumbnailPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.playlistThumbnailPreview.ImageLocation = "default_image.png";
            this.playlistThumbnailPreview.Location = new System.Drawing.Point(10, 25);
            this.playlistThumbnailPreview.MaximumSize = new System.Drawing.Size(277, 157);
            this.playlistThumbnailPreview.Name = "playlistThumbnailPreview";
            this.playlistThumbnailPreview.Size = new System.Drawing.Size(277, 157);
            this.playlistThumbnailPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.playlistThumbnailPreview.TabIndex = 6;
            this.playlistThumbnailPreview.TabStop = false;
            // 
            // createdOnLabel
            // 
            this.createdOnLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.createdOnLabel.AutoSize = true;
            this.createdOnLabel.Location = new System.Drawing.Point(345, 688);
            this.createdOnLabel.Name = "createdOnLabel";
            this.createdOnLabel.Size = new System.Drawing.Size(106, 25);
            this.createdOnLabel.TabIndex = 7;
            this.createdOnLabel.Text = "Created On:";
            // 
            // ownerLabel
            // 
            this.ownerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ownerLabel.AutoSize = true;
            this.ownerLabel.Location = new System.Drawing.Point(345, 663);
            this.ownerLabel.Name = "ownerLabel";
            this.ownerLabel.Size = new System.Drawing.Size(68, 25);
            this.ownerLabel.TabIndex = 10;
            this.ownerLabel.Text = "Owner:";
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(345, 588);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(48, 25);
            this.titleLabel.TabIndex = 11;
            this.titleLabel.Text = "Title:";
            // 
            // urlLabel
            // 
            this.urlLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.urlLabel.AutoSize = true;
            this.urlLabel.Location = new System.Drawing.Point(345, 613);
            this.urlLabel.Name = "urlLabel";
            this.urlLabel.Size = new System.Drawing.Size(47, 25);
            this.urlLabel.TabIndex = 12;
            this.urlLabel.Text = "URL:";
            // 
            // totalVideosLabel
            // 
            this.totalVideosLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.totalVideosLabel.AutoSize = true;
            this.totalVideosLabel.Location = new System.Drawing.Point(345, 638);
            this.totalVideosLabel.Name = "totalVideosLabel";
            this.totalVideosLabel.Size = new System.Drawing.Size(112, 25);
            this.totalVideosLabel.TabIndex = 14;
            this.totalVideosLabel.Text = "Total Videos:";
            // 
            // titleValue
            // 
            this.titleValue.AutoSize = true;
            this.titleValue.Location = new System.Drawing.Point(518, 8);
            this.titleValue.Name = "titleValue";
            this.titleValue.Size = new System.Drawing.Size(19, 25);
            this.titleValue.TabIndex = 15;
            this.titleValue.Text = "-";
            // 
            // privacyStatusValue
            // 
            this.privacyStatusValue.AutoSize = true;
            this.privacyStatusValue.Location = new System.Drawing.Point(518, 133);
            this.privacyStatusValue.Name = "privacyStatusValue";
            this.privacyStatusValue.Size = new System.Drawing.Size(19, 25);
            this.privacyStatusValue.TabIndex = 16;
            this.privacyStatusValue.Text = "-";
            // 
            // totalVideosValue
            // 
            this.totalVideosValue.AutoSize = true;
            this.totalVideosValue.Location = new System.Drawing.Point(518, 58);
            this.totalVideosValue.Name = "totalVideosValue";
            this.totalVideosValue.Size = new System.Drawing.Size(19, 25);
            this.totalVideosValue.TabIndex = 18;
            this.totalVideosValue.Text = "-";
            // 
            // playlistDetailsPanel
            // 
            this.playlistDetailsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.playlistDetailsPanel.Controls.Add(this.descriptionValue);
            this.playlistDetailsPanel.Controls.Add(this.createdOnValue);
            this.playlistDetailsPanel.Controls.Add(this.privacyStatusValue);
            this.playlistDetailsPanel.Controls.Add(this.totalVideosValue);
            this.playlistDetailsPanel.Controls.Add(this.privacyStatusLabel);
            this.playlistDetailsPanel.Controls.Add(this.descriptionLabel);
            this.playlistDetailsPanel.Controls.Add(this.titleValue);
            this.playlistDetailsPanel.Controls.Add(this.ownerValue);
            this.playlistDetailsPanel.Controls.Add(this.urlValue);
            this.playlistDetailsPanel.Controls.Add(this.playlistThumbnailPreview);
            this.playlistDetailsPanel.Location = new System.Drawing.Point(12, 580);
            this.playlistDetailsPanel.Name = "playlistDetailsPanel";
            this.playlistDetailsPanel.Size = new System.Drawing.Size(1044, 207);
            this.playlistDetailsPanel.TabIndex = 20;
            // 
            // descriptionValue
            // 
            this.descriptionValue.AutoSize = true;
            this.descriptionValue.Location = new System.Drawing.Point(518, 157);
            this.descriptionValue.Name = "descriptionValue";
            this.descriptionValue.Size = new System.Drawing.Size(19, 25);
            this.descriptionValue.TabIndex = 24;
            this.descriptionValue.Text = "-";
            // 
            // createdOnValue
            // 
            this.createdOnValue.AutoSize = true;
            this.createdOnValue.Location = new System.Drawing.Point(518, 108);
            this.createdOnValue.Name = "createdOnValue";
            this.createdOnValue.Size = new System.Drawing.Size(19, 25);
            this.createdOnValue.TabIndex = 23;
            this.createdOnValue.Text = "-";
            // 
            // privacyStatusLabel
            // 
            this.privacyStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.privacyStatusLabel.AutoSize = true;
            this.privacyStatusLabel.Location = new System.Drawing.Point(333, 133);
            this.privacyStatusLabel.Name = "privacyStatusLabel";
            this.privacyStatusLabel.Size = new System.Drawing.Size(124, 25);
            this.privacyStatusLabel.TabIndex = 22;
            this.privacyStatusLabel.Text = "Privacy Status:";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(333, 158);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(106, 25);
            this.descriptionLabel.TabIndex = 21;
            this.descriptionLabel.Text = "Description:";
            // 
            // ownerValue
            // 
            this.ownerValue.AutoSize = true;
            this.ownerValue.Location = new System.Drawing.Point(518, 83);
            this.ownerValue.Name = "ownerValue";
            this.ownerValue.Size = new System.Drawing.Size(19, 25);
            this.ownerValue.TabIndex = 1;
            this.ownerValue.TabStop = true;
            this.ownerValue.Text = "-";
            this.ownerValue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OwnerValue_LinkClicked);
            // 
            // urlValue
            // 
            this.urlValue.AutoSize = true;
            this.urlValue.Location = new System.Drawing.Point(518, 33);
            this.urlValue.Name = "urlValue";
            this.urlValue.Size = new System.Drawing.Size(19, 25);
            this.urlValue.TabIndex = 0;
            this.urlValue.TabStop = true;
            this.urlValue.Text = "-";
            this.urlValue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.UrlValue_LinkClicked);
            // 
            // HomePageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 794);
            this.Controls.Add(this.totalVideosLabel);
            this.Controls.Add(this.urlLabel);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.ownerLabel);
            this.Controls.Add(this.createdOnLabel);
            this.Controls.Add(this.LoggerLabel);
            this.Controls.Add(this.choosePlaylistLabel);
            this.Controls.Add(this.refreshPlaylistsButton);
            this.Controls.Add(this.playlistsTabs);
            this.Controls.Add(this.email);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.playlistDetailsPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1090, 850);
            this.Name = "HomePageForm";
            this.Text = "Home";
            this.playlistsTabs.ResumeLayout(false);
            this.ownerTab.ResumeLayout(false);
            this.contributorTab.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.playlistThumbnailPreview)).EndInit();
            this.playlistDetailsPanel.ResumeLayout(false);
            this.playlistDetailsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label email;
        private System.Windows.Forms.TabControl playlistsTabs;
        private System.Windows.Forms.TabPage contributorTab;
        private System.Windows.Forms.Button addContributorPlaylistButton;
        private System.Windows.Forms.Button refreshPlaylistsButton;
        private System.Windows.Forms.Label choosePlaylistLabel;
        private System.Windows.Forms.Button removeContributorPlaylistButton;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem playlistsMenu;
        private System.Windows.Forms.ToolStripMenuItem refreshPlaylistsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accountMenu;
        private System.Windows.Forms.ToolStripMenuItem switchAccountMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forgetCurrentAccountMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutMenu;
        private System.Windows.Forms.ToolStripMenuItem viewDetailsMenuItem;
        private System.Windows.Forms.ListView contributorPlaylistsList;
        private System.Windows.Forms.Label LoggerLabel;
        private ToolStripMenuItem addNewAccountMenuItem;
        private PictureBox playlistThumbnailPreview;
        private Label createdOnLabel;
        private Label ownerLabel;
        private Label titleLabel;
        private Label urlLabel;
        private Label totalVideosLabel;
        private Label titleValue;
        private Label privacyStatusValue;
        private Label totalVideosValue;
        private Panel playlistDetailsPanel;
        private LinkLabel ownerValue;
        private LinkLabel urlValue;
        private Label descriptionLabel;
        private Label privacyStatusLabel;
        private Label descriptionValue;
        private Label createdOnValue;
        private ListView ownerPlaylistsList;
        private TabPage ownerTab;
        private ToolTip descriptionToolTip;
    }
}