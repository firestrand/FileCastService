using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Timers;

namespace FileCastService
{
    public class EnhancedFileSystemWatcher:FileSystemWatcher
    {
        public event EventHandler NetworkPathUnavailable;
        public event EventHandler NetworkPathAvailable;

        private Timer _fileSystemTimer;

        public bool NetworkAvailable { get; set; }

        public EnhancedFileSystemWatcher()
        {
            _fileSystemTimer = new Timer(250d);
            _fileSystemTimer.Elapsed += new ElapsedEventHandler(_fileSystemTimer_Elapsed);
            _fileSystemTimer.Start();
        }

        private void _fileSystemTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DirectoryInfo testDir = new DirectoryInfo(Path.ToString());
            if (NetworkAvailable)
            {
                if (!testDir.Exists)
                {
                    NetworkAvailable = false;
                    if (NetworkPathUnavailable != null)
                        NetworkPathUnavailable(this, null);
                }
            }
            else
            {
                if (testDir.Exists)
                {
                    NetworkAvailable = true;
                    if (NetworkPathAvailable != null)
                        NetworkPathAvailable(this, null);
                }
            }
        }
    }
}
