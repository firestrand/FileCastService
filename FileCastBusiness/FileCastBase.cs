using System;

namespace FileCastBusiness
{
    abstract public class FileCastBase
    {
        protected static void LogEntry(string entry)
        {
            if (!System.Diagnostics.EventLog.SourceExists("FileCastService"))
                System.Diagnostics.EventLog.CreateEventSource(
                   "FileCastService", "Application");
            using (System.Diagnostics.EventLog EventLog1 = new System.Diagnostics.EventLog { Source = "FileCastService" })
            {
                EventLog1.WriteEntry(entry);
            }

        }
    }
}
