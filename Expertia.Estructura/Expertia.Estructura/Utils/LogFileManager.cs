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
        Field,
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
                case LogType.Field: return LogTypeMessage.Field;
                case LogType.Fail: return LogTypeMessage.Fail;
                case LogType.Warning: return LogTypeMessage.Warning;
                default: throw new Exception(LogTypeMessage.Undefined);
            }
        }

        public void WriteText(string text)
        {
            try
            {
                _fileIO.WriteContent(text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void WriteLine(LogType logType, string content, bool indent)
        {
            try
            {
                _fileIO.WriteContent(string.Format(
                    FormatTemplate.LogLine,                             // ◄ formato de línea de log
                    indent ? "\t" : string.Empty,                       // ◄ Tab o Empty
                    WriteType(logType),                                 // ◄ INFO, WARN, FAIL
                    DateTime.Now.ToString(FormatTemplate.LongDate),     // ◄ 2019-07-15 15:34:20.635
                    _clientFeatures.Method,                             // ◄ GET, POST
                    _clientFeatures.IP,                                 // ◄ 10.75.109.117
                    _clientFeatures.URL,                                // ◄ /api/cuentab2b/read
                    content));                                          // ◄ Mensaje
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
    }
}