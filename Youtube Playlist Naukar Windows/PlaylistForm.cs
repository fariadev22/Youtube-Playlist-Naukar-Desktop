using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Youtube_Playlist_Naukar_Windows.Models;

namespace Youtube_Playlist_Naukar_Windows
{
    public partial class PlaylistForm : Form
    {
        private UserPlayList CurrentPlaylist;

        private HomePageForm HomePage;

        public PlaylistForm(
            HomePageForm homepage,
            UserPlayList userPlaylist)
        {
            InitializeComponent();
            CurrentPlaylist = userPlaylist;
            HomePage = homepage;
        }

        private void PlaylistForm_Load(object sender, EventArgs e)
        {

        }
    }
}
