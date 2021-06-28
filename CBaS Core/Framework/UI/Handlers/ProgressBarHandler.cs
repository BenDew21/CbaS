using System.ComponentModel;
using System.Windows.Controls;

namespace CBaS_Core.Framework.UI.Handlers
{
    public class ProgressBarHandler
    {
        private static ProgressBarHandler _instance = null;

        private ProgressBar _progressBar;
        private Label _progressLabel;

        private BackgroundWorker _boundWorker;

        public static ProgressBarHandler GetInstance()
        {
            if (_instance == null) _instance = new ProgressBarHandler();
            return _instance;
        }

        public void SetControls(ProgressBar progressBar, Label label)
        {
            _progressBar = progressBar;
            _progressLabel = label;
        }

        public void Bind(BackgroundWorker worker)
        {
            _boundWorker = worker;
            _boundWorker.ProgressChanged += Worker_ProgressChanged;
        }

        public void Unbind()
        {
            _boundWorker.ProgressChanged -= Worker_ProgressChanged;
            _boundWorker = null;

            _progressBar.Value = 0;
            _progressLabel.Content = "Done";
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _progressBar.Value = e.ProgressPercentage;
            _progressLabel.Content = e.UserState;
        }
    }
}