using System.Linq;
using System.Windows.Forms;

namespace Youtube_Playlist_Naukar_Windows
{
    public partial class LogsForm : Form
    {
        public LogsForm()
        {
            InitializeComponent();
        }

        public void AddRow(string youtubeUrl, string logMessage)
        {
            logTable.Rows.Add(youtubeUrl, logMessage);
        }

        public void UpdateRow(string youtubeUrl, string logMessage)
        {
            if (logTable.Rows.Count <= 0)
            {
                return;
            }

            var row =
                logTable.Rows
                    .Cast<DataGridViewRow>()
                    .FirstOrDefault(
                        r => 
                            r.Cells["urlColumn"].Value
                                .ToString() == youtubeUrl);

            if (row != null)
            {
                row.Cells["statusColumn"].Value = logMessage;
            }
        }
    }
}
