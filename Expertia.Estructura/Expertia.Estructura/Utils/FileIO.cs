using Expertia.Estructura.Utils.Behavior;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Utils
{
    public class FileIO : IFileIO
    {
        private string _path;
        private string _name;
        private string _logFormat;

        public string FullName => _path + _name;

        public string LogFormat { get => _path + _name; set => _logFormat = value; }

        public FileIO(string path, string name)
        {
            _path = path; _name = name;
        }

        public string ReadContent()
        {
            return ReadContent(FullName);
        }

        public void WriteContent(string content)
        {
            WriteContent(FullName, content + "\n");
        }

        #region StaticMethods
        public static string ReadContent(string fullName)
        {
            return File.ReadAllText(fullName);
        }

        public static void WriteContent(string fullName, string content)
        {
            File.AppendAllText(fullName, content);
        }        
        #endregion
    }
}