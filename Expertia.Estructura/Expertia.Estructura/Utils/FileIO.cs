using Expertia.Estructura.Utils.Behavior;
using System;
using System.IO;

namespace Expertia.Estructura.Utils
{
    public class FileIO : IFileIO
    {
        private DateTime _now;

        private string _path;
        private string _name;
        private string _logFormat;

        public string FullName => _path + _name;
        public string LineFormat { get => _path + _name; set => _logFormat = value; }

        public FileIO(DateTime now, string path, string name)
        {
            _now = now; _path = path; _name = name;
        }

        public string ReadContent()
        {
            return ReadContent(FullName);
        }

        public void WriteContent(string content)
        {
            /// Carpetas
            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path); /// Objeto

            _path += _now.Year.ToString("0000") + "\\";
            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path); /// Año

            _path += _now.Year.ToString("0000") + _now.Month.ToString("00") + "\\";
            if (!Directory.Exists(_path)) Directory.CreateDirectory(_path); /// Mes

            /// Impresión
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