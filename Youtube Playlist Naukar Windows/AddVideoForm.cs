using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
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
            VideoUrlOrUrls = inputVideoUrlBox.Text;
        }
    }
}
