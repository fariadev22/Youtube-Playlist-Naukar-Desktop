using System;
using System.Windows.Forms;

namespace Youtube_Playlist_Naukar_Windows
{
    public partial class AddVideoForm : Form
    {
        public string VideoUrlOrUrls { get; set; }

        public AddVideoForm()
        {
            InitializeComponent();
            okButton.DialogResult = DialogResult.OK;
            cancelButton.DialogResult = DialogResult.Cancel;
            AcceptButton = okButton;
            CancelButton = cancelButton;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;
            VideoUrlOrUrls = inputVideoUrlBox.Text;

            if (string.IsNullOrWhiteSpace(VideoUrlOrUrls))
            {
                urlValidator.Text = @"No input URL(s) provided.";
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}
