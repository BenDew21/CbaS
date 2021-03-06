using Combinatorics_Calculator.Logic.UI.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace Combinatorics_Calculator.Logic
{
    public class SquareWaveGeneratorTask : BackgroundWorker
    {
        private SquareWaveGenerator _generator;
        private int _delay;
        private bool _restart = false;

        public SquareWaveGeneratorTask(SquareWaveGenerator generator)
        {
            _generator = generator;

            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
            DoWork += SquareWaveGeneratorTask_DoWork;
            ProgressChanged += SquareWaveGeneratorTask_ProgressChanged;
            RunWorkerCompleted += SquareWaveGeneratorTask_RunWorkerCompleted;
        }

        public void Start()
        {
            RunWorkerAsync(_delay);
        }

        public void Stop()
        {
            _restart = false;
            CancelAsync();
        }

        public void Restart()
        {
            CancelAsync();
            _restart = true;
        }

        public void UpdateDelay(double newDelay)
        {
            _delay = Convert.ToInt32((1 / newDelay) * 1000);
            Debug.WriteLine(_delay);
            if (IsBusy)
            {
                CancelAsync();
                _restart = true;
            }
        }

        private void SquareWaveGeneratorTask_DoWork(object sender, DoWorkEventArgs e)
        {
            int delay = (int)e.Argument;
            while (!CancellationPending)
            {
                ReportProgress(-1, false);
                Thread.Sleep(delay);
                if (!CancellationPending)
                {
                    ReportProgress(-1, true);
                    Thread.Sleep(delay);
                }
            }
        }

        private void SquareWaveGeneratorTask_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            bool state = (bool)e.UserState;
            _generator.SetOutputting(state);
        }

        private void SquareWaveGeneratorTask_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_restart)
            {
                RunWorkerAsync(_delay);
            }
        }
    }
}