using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;

namespace TardySvc
{
    public partial class Tardy : ServiceBase
    {
        int timeout;
        string serviceName = "TardySvc";

        public Tardy()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (!(args.Length > 0 && int.TryParse(args[0], out timeout)))
                // Default to 1½ minute timeout
                timeout = 90 * 1000;

            // Set maximum timeout of 5 minutes
            timeout = Math.Min(timeout, 300 * 1000);

            if (!EventLog.SourceExists(serviceName))
                EventLog.CreateEventSource(serviceName, "Application");

            EventLog.WriteEntry(serviceName, String.Format("Starting {0} with {1}ms timeout", serviceName, timeout), EventLogEntryType.Information);
        }

        protected override void OnStop()
        {
            if (!EventLog.SourceExists(serviceName))
                EventLog.CreateEventSource(serviceName, "Application");

            EventLog.WriteEntry(serviceName, String.Format("Sleeping {0} with {1}ms timeout", serviceName, timeout), EventLogEntryType.Information);
            Thread.Sleep(timeout);
            EventLog.WriteEntry(serviceName, String.Format("Stopping {0}", serviceName, timeout), EventLogEntryType.Information);
        }
    }
}
