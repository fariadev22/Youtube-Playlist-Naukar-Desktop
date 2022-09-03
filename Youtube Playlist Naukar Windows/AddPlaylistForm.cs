using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Youtube_Playlist_Naukar_Windows.Models;
using Youtube_Playlist_Naukar_Windows.Utilities;

namespace Youtube_Playlist_Naukar_Windows
{
    public partial class AddPlaylistForm : Form
    {
        public string PlaylistUrlFromUser { get; set; }

        private Dictionary<string, UserPlayList> 
            _userContributorPlaylists;
        
        public AddPlaylistForm(
            Dictionary<string, UserPlayList> userContributorPlaylists)
        {
            InitializeComponent();
            AcceptButton = okButton;
            okButton.DialogResult = DialogResult.OK;
            CancelButton = cancelButton;
            cancelButton.DialogResult = DialogResult.Cancel;
            _userContributorPlaylists = userContributorPlaylists;
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            PlaylistUrlFromUser = urlBox.Text;
            DialogResult = DialogResult.None;

            if (string.IsNullOrWhiteSpace(PlaylistUrlFromUser))
            {
                urlValidator.Text = @"No URL provided.";
            }
            else
            {
                bool isValidUrl =
                    CommonUtilities.TryGetPlaylistIdFromYoutubeUrl(
                        PlaylistUrlFromUser, out string playListId);

                if (!isValidUrl)
                {
                    urlValidator.Text = @"URL is invalid.";
                }
                else if (_userContributorPlaylists.ContainsKey(
                    playListId))
                {
                    urlValidator.Text = @"Playlist already exists.";
                }
                else
                {
                    urlValidator.Text = "";
                    DialogResult = DialogResult.OK;
                }
            }
        }
    }
}
