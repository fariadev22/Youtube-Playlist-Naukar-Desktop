using System;
using System.Windows.Forms;

namespace Youtube_Playlist_Naukar_Windows
{
    public partial class AddAccountForm : Form
    {
        public string UserEmailAddress { get; set; }

        public AddAccountForm()
        {
            InitializeComponent();
            AcceptButton = emailOkButton;
            emailOkButton.DialogResult = DialogResult.OK;
            CancelButton = emailCancelButton;
            emailCancelButton.DialogResult = DialogResult.Cancel;
        }

        private void EmailOk_Click(object sender, EventArgs e)
        {
            UserEmailAddress = emailInputBox.Text;
        }
    }
}
