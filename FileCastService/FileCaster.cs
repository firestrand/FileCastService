using System;
using System.Collections.Generic;
using System.ServiceProcess;
using FileCastBusiness;
using System.IO;

namespace FileCastService
{
    public partial class FileCaster : ServiceBase
    {
        List<EnhancedFileSystemWatcher> _watchList;

        public List<EnhancedFileSystemWatcher> WatchList
        {
            get 
            {
                if(_watchList == null) _watchList = new List<EnhancedFileSystemWatcher>();
                return _watchList; 
            }
        }

        public FileCaster()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //Get configuration information
            FileCastConfig fileCastConfig = new FileCastConfig();
            //Wireup multiple FileSystemWatcher objects
            EnhancedFileSystemWatcher efsw;
            fileCastConfig.FileCastPairs.ForEach(f =>
            {
                efsw = new EnhancedFileSystemWatcher();
                efsw.Path = f.Source;
                efsw.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                efsw.Created += f.CastFile;
                efsw.Changed += f.CastFile;
                efsw.Renamed += f.CastFile;
                efsw.EnableRaisingEvents = true;
                WatchList.Add(efsw);
            });
        }

        protected override void OnStop()
        {
            WatchList.ForEach(efsw => efsw.Dispose());
        }
        public void TestStart()
        { this.OnStart(null); }
    }
}
