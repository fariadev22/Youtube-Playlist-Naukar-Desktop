
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
            this.Email = new System.Windows.Forms.Label();
            this.playlistsTabs = new System.Windows.Forms.TabControl();
            this.Owner = new System.Windows.Forms.TabPage();
            this.ownerPlaylistsList = new System.Windows.Forms.ListView();
            this.Contributor = new System.Windows.Forms.TabPage();
            this.contributorPlaylistsList = new System.Windows.Forms.ListView();
            this.RemoveContributorPlaylistButton = new System.Windows.Forms.Button();
            this.AddContributorPlaylistButton = new System.Windows.Forms.Button();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            this.RefreshPlaylistsButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.PlaylistsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.RefreshPlaylistsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AccountMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.SwitchAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.ForgetCurrentAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.AddNewAccountMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.LoggerLabel = new System.Windows.Forms.Label();
            this.playlistsTabs.SuspendLayout();
            this.Owner.SuspendLayout();
            this.Contributor.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Email
            // 
            this.Email.AutoSize = true;
            this.Email.Location = new System.Drawing.Point(571, 43);
            this.Email.Name = "Email";
            this.Email.Size = new System.Drawing.Size(119, 25);
            this.Email.TabIndex = 0;
            this.Email.Text = "EmailAddress";
            // 
            // playlistsTabs
            // 
            this.playlistsTabs.Controls.Add(this.Owner);
            this.playlistsTabs.Controls.Add(this.Contributor);
            this.playlistsTabs.Location = new System.Drawing.Point(12, 115);
            this.playlistsTabs.Name = "playlistsTabs";
            this.playlistsTabs.SelectedIndex = 0;
            this.playlistsTabs.Size = new System.Drawing.Size(666, 323);
            this.playlistsTabs.TabIndex = 1;
            // 
            // Owner
            // 
            this.Owner.Controls.Add(this.ownerPlaylistsList);
            this.Owner.Location = new System.Drawing.Point(4, 34);
            this.Owner.Name = "Owner";
            this.Owner.Padding = new System.Windows.Forms.Padding(3);
            this.Owner.Size = new System.Drawing.Size(658, 285);
            this.Owner.TabIndex = 0;
            this.Owner.Text = "Owner Playlists";
            this.Owner.UseVisualStyleBackColor = true;
            // 
            // ownerPlaylistsList
            // 
            this.ownerPlaylistsList.HideSelection = false;
            this.ownerPlaylistsList.Location = new System.Drawing.Point(6, 6);
            this.ownerPlaylistsList.Name = "ownerPlaylistsList";
            this.ownerPlaylistsList.Size = new System.Drawing.Size(646, 273);
            this.ownerPlaylistsList.TabIndex = 5;
            this.ownerPlaylistsList.UseCompatibleStateImageBehavior = false;
            this.ownerPlaylistsList.DoubleClick += new System.EventHandler(this.ownerPlaylistsList_DoubleClicked);
            // 
            // Contributor
            // 
            this.Contributor.Controls.Add(this.contributorPlaylistsList);
            this.Contributor.Controls.Add(this.RemoveContributorPlaylistButton);
            this.Contributor.Controls.Add(this.AddContributorPlaylistButton);
            this.Contributor.Location = new System.Drawing.Point(4, 34);
            this.Contributor.Name = "Contributor";
            this.Contributor.Padding = new System.Windows.Forms.Padding(3);
            this.Contributor.Size = new System.Drawing.Size(658, 285);
            this.Contributor.TabIndex = 1;
            this.Contributor.Text = "Contributor Playlists";
            this.Contributor.UseVisualStyleBackColor = true;
            // 
            // contributorPlaylistsList
            // 
            this.contributorPlaylistsList.HideSelection = false;
            this.contributorPlaylistsList.Location = new System.Drawing.Point(7, 54);
            this.contributorPlaylistsList.Name = "contributorPlaylistsList";
            this.contributorPlaylistsList.Size = new System.Drawing.Size(651, 228);
            this.contributorPlaylistsList.TabIndex = 4;
            this.contributorPlaylistsList.UseCompatibleStateImageBehavior = false;
            this.contributorPlaylistsList.DoubleClick += new System.EventHandler(this.contributorPlaylistsList_DoubleClicked);
            // 
            // RemoveContributorPlaylistButton
            // 
            this.RemoveContributorPlaylistButton.Location = new System.Drawing.Point(530, 12);
            this.RemoveContributorPlaylistButton.Name = "RemoveContributorPlaylistButton";
            this.RemoveContributorPlaylistButton.Size = new System.Drawing.Size(112, 34);
            this.RemoveContributorPlaylistButton.TabIndex = 3;
            this.RemoveContributorPlaylistButton.Text = "Remove";
            this.RemoveContributorPlaylistButton.UseVisualStyleBackColor = true;
            this.RemoveContributorPlaylistButton.Click += new System.EventHandler(this.RemoveContributorPlaylistButton_Click);
            // 
            // AddContributorPlaylistButton
            // 
            this.AddContributorPlaylistButton.Location = new System.Drawing.Point(402, 12);
            this.AddContributorPlaylistButton.Name = "AddContributorPlaylistButton";
            this.AddContributorPlaylistButton.Size = new System.Drawing.Size(122, 34);
            this.AddContributorPlaylistButton.TabIndex = 0;
            this.AddContributorPlaylistButton.Text = "Add";
            this.AddContributorPlaylistButton.UseVisualStyleBackColor = true;
            this.AddContributorPlaylistButton.Click += new System.EventHandler(this.AddContributorPlaylistButton_Click);
            // 
            // imageList3
            // 
            this.imageList3.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList3.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList3.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // RefreshPlaylistsButton
            // 
            this.RefreshPlaylistsButton.Location = new System.Drawing.Point(684, 395);
            this.RefreshPlaylistsButton.Name = "RefreshPlaylistsButton";
            this.RefreshPlaylistsButton.Size = new System.Drawing.Size(95, 34);
            this.RefreshPlaylistsButton.TabIndex = 2;
            this.RefreshPlaylistsButton.Text = "Refresh";
            this.RefreshPlaylistsButton.UseVisualStyleBackColor = true;
            this.RefreshPlaylistsButton.Click += new System.EventHandler(this.RefreshPlaylistsButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(241, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Choose a playlist to manage:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PlaylistsMenu,
            this.AccountMenu,
            this.AboutMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 33);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // PlaylistsMenu
            // 
            this.PlaylistsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RefreshPlaylistsMenuItem});
            this.PlaylistsMenu.Name = "PlaylistsMenu";
            this.PlaylistsMenu.Size = new System.Drawing.Size(90, 29);
            this.PlaylistsMenu.Text = "Playlists";
            // 
            // RefreshPlaylistsMenuItem
            // 
            this.RefreshPlaylistsMenuItem.Name = "RefreshPlaylistsMenuItem";
            this.RefreshPlaylistsMenuItem.Size = new System.Drawing.Size(239, 34);
            this.RefreshPlaylistsMenuItem.Text = "Refresh Playlists";
            this.RefreshPlaylistsMenuItem.Click += new System.EventHandler(this.RefreshPlaylists_Click);
            // 
            // AccountMenu
            // 
            this.AccountMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SwitchAccount,
            this.ForgetCurrentAccount,
            this.AddNewAccountMenuItem});
            this.AccountMenu.Name = "AccountMenu";
            this.AccountMenu.Size = new System.Drawing.Size(93, 29);
            this.AccountMenu.Text = "Account";
            // 
            // SwitchAccount
            // 
            this.SwitchAccount.Name = "SwitchAccount";
            this.SwitchAccount.Size = new System.Drawing.Size(299, 34);
            this.SwitchAccount.Text = "Switch Account";
            this.SwitchAccount.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.SwitchToAccountWithSelectedEmail);
            // 
            // ForgetCurrentAccount
            // 
            this.ForgetCurrentAccount.Name = "ForgetCurrentAccount";
            this.ForgetCurrentAccount.Size = new System.Drawing.Size(299, 34);
            this.ForgetCurrentAccount.Text = "Forget Current Account";
            this.ForgetCurrentAccount.Click += new System.EventHandler(this.ForgetCurrentAccount_Click);
            // 
            // AddNewAccountMenuItem
            // 
            this.AddNewAccountMenuItem.Name = "AddNewAccountMenuItem";
            this.AddNewAccountMenuItem.Size = new System.Drawing.Size(299, 34);
            this.AddNewAccountMenuItem.Text = "Add New Account";
            this.AddNewAccountMenuItem.Click += new System.EventHandler(this.AddNewAccountMenuItem_Click);
            // 
            // AboutMenu
            // 
            this.AboutMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewDetails});
            this.AboutMenu.Name = "AboutMenu";
            this.AboutMenu.Size = new System.Drawing.Size(78, 29);
            this.AboutMenu.Text = "About";
            // 
            // ViewDetails
            // 
            this.ViewDetails.Name = "ViewDetails";
            this.ViewDetails.Size = new System.Drawing.Size(270, 34);
            this.ViewDetails.Text = "View Details";
            this.ViewDetails.Click += new System.EventHandler(this.ViewDetails_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageList2
            // 
            this.imageList2.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList2.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // LoggerLabel
            // 
            this.LoggerLabel.AutoSize = true;
            this.LoggerLabel.Location = new System.Drawing.Point(516, 115);
            this.LoggerLabel.Name = "LoggerLabel";
            this.LoggerLabel.Size = new System.Drawing.Size(0, 25);
            this.LoggerLabel.TabIndex = 5;
            // 
            // HomePageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LoggerLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RefreshPlaylistsButton);
            this.Controls.Add(this.playlistsTabs);
            this.Controls.Add(this.Email);
            this.Controls.Add(this.menuStrip1);
            this.Name = "HomePageForm";
            this.Text = "HomePageForm";
            this.playlistsTabs.ResumeLayout(false);
            this.Owner.ResumeLayout(false);
            this.Contributor.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Email;
        private System.Windows.Forms.TabControl playlistsTabs;
        private System.Windows.Forms.TabPage Contributor;
        private System.Windows.Forms.Button AddContributorPlaylistButton;
        private System.Windows.Forms.Button RefreshPlaylistsButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button RemoveContributorPlaylistButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem PlaylistsMenu;
        private System.Windows.Forms.ToolStripMenuItem RefreshPlaylistsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AccountMenu;
        private System.Windows.Forms.ToolStripMenuItem SwitchAccount;
        private System.Windows.Forms.ToolStripMenuItem ForgetCurrentAccount;
        private System.Windows.Forms.ToolStripMenuItem AboutMenu;
        private System.Windows.Forms.ToolStripMenuItem ViewDetails;
        private System.Windows.Forms.TabPage Owner;
        private System.Windows.Forms.ListView ownerPlaylistsList;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ImageList imageList3;
        private System.Windows.Forms.ListView contributorPlaylistsList;
        private System.Windows.Forms.Label LoggerLabel;
        private ToolStripMenuItem AddNewAccountMenuItem;
    }
}