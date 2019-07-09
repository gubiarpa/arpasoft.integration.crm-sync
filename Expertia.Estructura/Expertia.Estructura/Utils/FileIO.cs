using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Utils
{
    public class FileIO
    {
        protected string _path { get; set; }
        protected string _name { get; set; }

        protected string FullName
        {
            get { return _path + _name; }
        }

        public FileIO(string path, string name)
        {
            _path = path; _name = name;
        }

        public void WriteContent(string content)
        {
            File.AppendAllText(FullName, content);                       
        }

        public string ReadContent()
        {
            return File.ReadAllText(FullName);
        }

        #region StaticMethods
        public static void WriteContent(string fullName, string content)
        {
            File.AppendAllText(fullName, content);
        }
        #endregion
    }
}