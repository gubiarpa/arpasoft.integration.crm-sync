using Expertia.Estructura.Controllers.Behavior;
using Expertia.Estructura.Utils.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Utils
{
    public enum LogType
    {
        Info,
        Fail,
        Warning
    }

    public class LogFileManager : ILogFileManager
    {
        IFileIO _fileIO;
        IClientFeatures _clientFeatures;

        public LogFileManager(string filePathKey, string fileNameKey)
        {
            _fileIO = new FileIO(
                ConfigAccess.GetValueInAppSettings(filePathKey),
                string.Format(ConfigAccess.GetValueInAppSettings(fileNameKey), DateTime.Now.ToString(FormatTemplate.FileDate)));
            _clientFeatures = new ClientFeatures();
        }

        private string WriteType(LogType logtype)
        {
            switch (logtype)
            {
                case LogType.Info: return LogTypeMessage.Info;
                case LogType.Fail: return LogTypeMessage.Fail;
                case LogType.Warning: return LogTypeMessage.Warning;
                default: throw new Exception(LogTypeMessage.Undefined);
            }
        }

        public void WriteLine(LogType logType, string content)
        {
            try
            {
                _fileIO.WriteContent(string.Format(
                    FormatTemplate.LogLine, // ◄ formato de línea de log
                    WriteType(logType), // ◄ INFO, WARN, FAIL
                    DateTime.Now.ToString(FormatTemplate.LongDate), // ◄ 2019-07-15 15:34:20.635
                    _clientFeatures.Method, // ◄ GET, POST
                    _clientFeatures.IP, // ◄ 10.75.109.117
                    _clientFeatures.Uri, // ◄ /api/cuentab2b/read
                    content)); // ◄ Mensaje
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
    }
}