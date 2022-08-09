using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Youtube_Playlist_Naukar_Windows
{
    public partial class AddAccountForm : Form
    {
        public string UserEmailAddress { get; set; }

        public AddAccountForm()
        {
            InitializeComponent();
            EmailOk.DialogResult = DialogResult.OK;
            EmailCancel.DialogResult = DialogResult.Cancel;
            AcceptButton = EmailOk;
            CancelButton = EmailCancel;
        }

        private void EmailOk_Click(object sender, EventArgs e)
        {
            UserEmailAddress = emailInputBox.Text;
        }
    }
}
