using CBaS_Core.Project.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CBaS_Core.Project.Business
{
    public class CircuitLoadingBackgroundWorker : BackgroundWorker
    {
        public CircuitLoadingBackgroundWorker()
        {
            DoWork += CircuitLoadingBackgroundWorker_DoWork;
            RunWorkerCompleted += CircuitLoadingBackgroundWorker_RunWorkerCompleted;
            WorkerReportsProgress = true;
            RunWorkerAsync();
        }

        private void CircuitLoadingBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CircuitLoadingBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Dictionary<int, Circuit> circuits = (Dictionary<int, Circuit>)e.Argument;
            int total = circuits.Count;
            int progressIncrementor = 100 / total;
            int completed = 0;

            foreach (var circuit in circuits.Values)
            {
                ReportProgress(completed * progressIncrementor, "Loading Circuit " + circuit.Name);
            }
        }
    }
}