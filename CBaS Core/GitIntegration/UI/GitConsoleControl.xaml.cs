using System.Diagnostics;
using System.Windows.Controls;
using CBaSCore.GitIntegration.Business;

namespace CBaSCore.GitIntegration.UI
{
    public partial class GitConsoleControl : UserControl
    {
        public GitConsoleControl()
        {
            InitializeComponent();
            GitHandler.GetInstance().SetConsoleControl(this);
        }

        public void Log(string text)
        {
            Debug.WriteLine(text);
            
            if (TextBlockConsole.Text == "")
            {
                TextBlockConsole.Text += text;
            }
            else
            {
                TextBlockConsole.Text += "\n" + text;
            }
        }
    }
}