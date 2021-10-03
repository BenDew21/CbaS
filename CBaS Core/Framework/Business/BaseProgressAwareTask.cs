using System.ComponentModel;
using CBaSCore.Framework.UI.Handlers;

namespace CBaSCore.Framework.Business
{
    /// <summary>
    /// Delegate to handle code execution when thread reports progress
    /// </summary>
    public delegate void ThreadAction();
    
    /// <summary>
    /// BaseProgressAwareTask - Background task which reports progress to the application progress bar
    /// </summary>
    public abstract class BaseProgressAwareTask : BackgroundWorker
    {
        /// <summary>
        /// Method run on a new thread
        /// </summary>
        /// <param name="sender">The sending task, usually this object</param>
        /// <param name="e">Event args</param>
        protected abstract void TaskDoWork(object sender, DoWorkEventArgs e);
        
        /// <summary>
        /// Method called when the thread has finished
        /// </summary>
        /// <param name="sender">The sending task, usually this object</param>
        /// <param name="e">Event args</param>
        public abstract void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e);
        
        /// <summary>
        /// Run the task
        /// </summary>
        public void Run()
        {
            DoWork += TaskDoWork;
            WorkerReportsProgress = true;
            
            ProgressBarHandler.GetInstance().Bind(this);
        }
        
        /// <summary>
        /// Run the task with a supplied argument
        /// </summary>
        /// <param name="argument">Argument to pass to the task</param>
        public void Run(object argument)
        {
            DoWork += TaskDoWork;
            RunWorkerCompleted += WorkerCompleted;

            ProgressBarHandler.GetInstance().Bind(this, argument);
        }
    }
}