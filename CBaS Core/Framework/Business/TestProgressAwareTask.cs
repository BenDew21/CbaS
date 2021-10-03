using System.ComponentModel;
using System.Threading;
using CBaSCore.GitIntegration.Business;

namespace CBaSCore.Framework.Business
{
    public class TestProgressAwareTask : BaseProgressAwareTask
    {
        protected override void TaskDoWork(object sender, DoWorkEventArgs e)
        {
            var baseTask = sender as BaseProgressAwareTask;
            baseTask.ReportProgress(33, new ProgressChangedObject()
            {
                Message = "Started",
                Action = (() => GitHandler.GetInstance().LogToConsole("Started"))
            });
            
            Thread.Sleep(5000);
            
            baseTask.ReportProgress(66, new ProgressChangedObject()
            {
                Message = "In Progress",
                Action = (() => GitHandler.GetInstance().LogToConsole("In Progress"))
            });
            
            Thread.Sleep(5000);
            
            baseTask.ReportProgress(100, new ProgressChangedObject()
            {
                Message = "Completed",
                Action = (() => GitHandler.GetInstance().LogToConsole("Completed"))
            });
            
            Thread.Sleep(1000);
        }

        public override void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // throw new System.NotImplementedException();
            GitHandler.GetInstance().LogToConsole("WorkerCompleted");
        }
    }
}