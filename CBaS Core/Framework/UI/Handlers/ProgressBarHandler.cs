using System.ComponentModel;
using System.Windows;
using CBaSCore.Framework.Business;
using Label = System.Windows.Controls.Label;
using ProgressBar = System.Windows.Controls.ProgressBar;

namespace CBaSCore.Framework.UI.Handlers
{
    /// <summary>
    /// ProgressBarHandler - Singleton class to act as entry point for all background tasks which contribute to the application progress bar
    /// </summary>
    public class ProgressBarHandler
    {
        /// <summary>
        /// Static reference for this class
        /// </summary>
        private static ProgressBarHandler _instance;

        /// <summary>
        /// The worker bound to the Progress Bar
        /// </summary>
        private BaseProgressAwareTask _boundWorker;

        /// <summary>
        /// The Progress Bar
        /// </summary>
        private ProgressBar _progressBar;
        
        /// <summary>
        /// The Progress Label
        /// </summary>
        private Label _progressLabel;

        /// <summary>
        /// Get the singleton instance
        /// </summary>
        /// <returns>The singleton instance</returns>
        public static ProgressBarHandler GetInstance()
        {
            return _instance ??= new ProgressBarHandler();
        }

        /// <summary>
        /// Set the controls
        /// </summary>
        /// <param name="progressBar">The Progress Bar</param>
        /// <param name="label">The Label</param>
        public void SetControls(ProgressBar progressBar, Label label)
        {
            _progressBar = progressBar;
            _progressLabel = label;
        }

        /// <summary>
        /// Bind a new background task
        /// </summary>
        /// <param name="worker">The new background task</param>
        public void Bind(BaseProgressAwareTask worker)
        {
            _boundWorker = worker;
            _boundWorker.ProgressChanged += Worker_ProgressChanged;
            _boundWorker.RunWorkerCompleted += Worker_Completed;

            _progressBar.Visibility = Visibility.Visible;
            _progressLabel.Visibility = Visibility.Visible;
            
            _boundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Bind a new background task
        /// </summary>
        /// <param name="worker">The new background task</param>
        /// <param name="parameter">The parameter to pass to the background task</param>
        public void Bind(BaseProgressAwareTask worker, object parameter)
        {
            _boundWorker = worker;
            _boundWorker.ProgressChanged += Worker_ProgressChanged;
            _boundWorker.RunWorkerCompleted += Worker_Completed;

            _progressBar.Visibility = Visibility.Visible;
            _progressLabel.Visibility = Visibility.Visible;
            
            _boundWorker.RunWorkerAsync(parameter);
        }
        
        /// <summary>
        /// Called when the background task is complete
        /// </summary>
        /// <param name="sender">The background task</param>
        /// <param name="e">The task complete event args</param>
        private void Worker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            _boundWorker.WorkerCompleted(sender, e);
            Unbind();
        }

        /// <summary>
        /// Unbind the background task and reset the controls
        /// </summary>
        private void Unbind()
        {
            _boundWorker.ProgressChanged -= Worker_ProgressChanged;
            _boundWorker = null;

            _progressBar.Value = 0;
            _progressLabel.Content = "";
            
            _progressBar.Visibility = Visibility.Visible;
            _progressLabel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Called when task progress has changed
        /// </summary>
        /// <param name="sender">The background task</param>
        /// <param name="e">The progress changed event args</param>
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var progressPercentage = e.ProgressPercentage;
            
            if (progressPercentage != -1)
            {
                _progressBar.IsIndeterminate = false;
            }
            
            _progressBar.Value = e.ProgressPercentage;

            if (e.UserState is ProgressChangedObject progObject)
            {
                progObject.Action();
                _progressLabel.Content = progObject.Message;
                
                if (progressPercentage != -1)
                {
                    _progressLabel.Content += " (" + progressPercentage + "%)";
                }
            }
        }
    }
}