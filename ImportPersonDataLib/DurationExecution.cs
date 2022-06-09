using ImportPersonDataLib.Interfaces;
using System;
using System.Diagnostics;

namespace ImportPersonDataLib
{
    public class DurationExecution : IDurationExecution
    {
        private readonly Stopwatch stopWatch;
        private string elapsedTime;

        public string ElapsedTime { get { return elapsedTime; } }

        public DurationExecution()
        {
            stopWatch = new Stopwatch();
        }

        public void Start()
        {
            stopWatch.Start();
        }


        public void Stop()
        {
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            elapsedTime = "RunTime " + String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        }
    }
}
