using Expertia.Estructura.Utils.Behavior;
using System;

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

        public LogFileManager(string filePathKey, string fileNameKey)
        {
            _fileIO = new FileIO(
                ConfigAccess.GetValueInAppSettings(filePathKey),
                string.Format(ConfigAccess.GetValueInAppSettings(fileNameKey), DateTime.Now.ToString(ConfigAccess.GetValueInAppSettings(LogKeys.LogDate))));
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
    }
}