using System;
using System.Collections.Generic;
using System.IO;

namespace FileCastBusiness
{
    [Serializable]
    public class FileCastPair : FileCastBase
    {
        public string Source { get; set; }

        private List<string> _destinations;
        public List<string> Destinations
        {
            get
            {
                if (_destinations == null)
                    _destinations = new List<string>();
                return _destinations;
            }
            set
            {
                //validate a valid UNC path, throw an error otherwise
                _destinations = value;
            }
        }

        public void CastFile(object source, FileSystemEventArgs e)
        {
            //Validate Source
            DirectoryInfo di;
            FileInfo fi = new FileInfo(e.FullPath); //Use FileInfo to get around OS locking of the file

            if (!fi.Exists)
            {
                LogEntry(String.Format("Source file %1 does not exist", Source));
            }

            string fileName = Path.GetFileName(e.FullPath);
            try
            {
                foreach (string s in Destinations)
                {
                    di = new DirectoryInfo(s);
                    if (!di.Exists)
                    {
                        di.Create();
                    }
                    if (!File.Exists(Path.Combine(s, fileName)))
                    {
                        using (FileStream fs = fi.Open(FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
                        {
                            fi.CopyTo(Path.Combine(s, fileName), false);
                        }
                        //File.Copy(e.FullPath, Path.Combine(s, fileName), false);
                    }
                }
                
            }catch(Exception ex)
            {
                //log exception
                LogEntry(ex.Message);
                //throw new Exception(String.Format("An error has occured.\r\n%1", ex.Message));
            }
        }
        
    }
}
