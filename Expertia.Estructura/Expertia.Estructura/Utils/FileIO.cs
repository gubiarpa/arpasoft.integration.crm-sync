using Expertia.Estructura.Utils.Behavior;
using System;
using System.IO;

namespace Expertia.Estructura.Utils
{
    public class FileIO : IFileIO
    {
        private string _path;
        private string _name;
        private string _logFormat;

        public string FullName => _path + _name;
        public string LineFormat { get => _path + _name; set => _logFormat = value; }

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
            WriteContent(FullName, content);
        }

        public void WriteContent(string[] contents)
        {
            foreach (var content in contents)
            {
                WriteContent(content);
            }
        }

        #region StaticMethods
        public static string ReadContent(string fullName)
        {
            try
            {
                return File.ReadAllText(fullName);
            }
            catch
            {
                return null;
            }
        }

        public static void WriteContent(string fullName, string content)
        {
            try
            {
                File.AppendAllText(fullName, content);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}