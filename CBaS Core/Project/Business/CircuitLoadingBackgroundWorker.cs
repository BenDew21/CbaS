using System;
using System.Collections.Generic;
using System.ComponentModel;
using CBaSCore.Project.Storage;

namespace CBaSCore.Project.Business
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
            var circuits = (Dictionary<int, Circuit>) e.Argument;
            var total = circuits.Count;
            var progressIncrementor = 100 / total;
            var completed = 0;

            foreach (var circuit in circuits.Values) ReportProgress(completed * progressIncrementor, "Loading Circuit " + circuit.Name);
        }
    }
}