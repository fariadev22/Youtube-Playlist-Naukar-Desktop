using System;
using System.Windows.Forms;

namespace Youtube_Playlist_Naukar_Windows
{
    public partial class AddPlaylistForm : Form
    {
        public string PlaylistUrlFromUser { get; set; }

        public AddPlaylistForm()
        {
            InitializeComponent();
            AcceptButton = okButton;
            okButton.DialogResult = DialogResult.OK;
            CancelButton = cancelButton;
            cancelButton.DialogResult = DialogResult.Cancel;
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            PlaylistUrlFromUser = urlBox.Text;
        }
    }
}
