using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Configuration;

namespace FileCastBusiness
{
    [Serializable]
    public class FileCastConfig : FileCastBase
    {
        private List<FileCastPair> _fileCastPairs;
        public List<FileCastPair> FileCastPairs
        {
            get
            {
                if (_fileCastPairs == null)
                    _fileCastPairs = new List<FileCastPair>();
                return _fileCastPairs;
            }
            set
            {
                _fileCastPairs = value;
            }
        }
        internal FileCastConfig(XmlNode section)
        {
            if (section == null)
                throw new Exception("FileCastConfig-Node does not exist.");

            //use XML Serialization to get FileCastPairs
            XmlSerializer xmlSer = new XmlSerializer(typeof(List<FileCastPair>));
            using (StringReader stringReader = new StringReader(section.InnerXml))
            {
                using (XmlTextReader xmlReader = new XmlTextReader(stringReader))
                {
                    FileCastPairs = xmlSer.Deserialize(xmlReader) as List<FileCastPair>;
                    xmlReader.Close();
                }
                stringReader.Close();
            }
        }
        public FileCastConfig()
        {
            FileCastConfig config = ConfigurationSettings.GetConfig("FileCastConfig") as FileCastConfig;
            if (config == null)
            {
                //Log an error
                const string message = "FileCastConfig Section does not exist or is corrupt";
                LogEntry(message);
                throw new Exception(message);
            }
            _fileCastPairs = config.FileCastPairs;
        }
        public FileCastConfig(List<FileCastPair> fileCastPairs)
        {
            FileCastPairs = fileCastPairs;
        }
        public override string ToString()
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(List<FileCastPair>));
                xmlSer.Serialize(memStream, FileCastPairs);
                Byte[] bytes = memStream.ToArray();
                string retStr = Encoding.UTF8.GetString(bytes);
                memStream.Close();
                return retStr;
            }
        }
    }
    public class FileCastConfigSectionHandler : IConfigurationSectionHandler
    {
        #region IConfigurationSectionHandler Members

        public object Create(object parent, object configContext, XmlNode section)
        {
            return new FileCastConfig(section);
        }

        #endregion
    }
}
