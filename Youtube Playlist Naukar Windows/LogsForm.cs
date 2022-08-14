using System.Collections.Generic;
using System.Windows.Forms;

namespace Youtube_Playlist_Naukar_Windows
{
    public partial class LogsForm : Form
    {
        public LogsForm(List<string> logMessages)
        {
            InitializeComponent();
            logMessages ??= new List<string>();

            foreach (var logMessage in logMessages)
            {
                logMessagesList.Items.Add(logMessage);
            }
        }
    }
}
