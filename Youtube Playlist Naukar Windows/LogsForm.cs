using System.Drawing;
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

        public void AddRow(
            string urlId, string youtubeUrl, string logMessage)
        {
            logTable.Rows.Add(urlId, youtubeUrl, logMessage);
        }

        public void UpdateRow(
            string urlId, string youtubeUrl, string logMessage,
            bool isSuccess)
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
                            r.Cells["Id"].Value
                                .ToString() == urlId);

            if (row != null)
            {
                row.Cells["statusColumn"].Value = logMessage;
                if (isSuccess)
                {
                    row.Cells["statusColumn"].Style.BackColor =
                        Color.LimeGreen;
                }
                else
                {
                    row.Cells["statusColumn"].Style.BackColor =
                        Color.OrangeRed;
                }

                row.Cells["statusColumn"].Style.ForeColor =
                    Color.White;
            }
        }
    }
}
