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

        public LogFileManager(string filePathKey, string fileNameKey, ControllerName controllerName)
        {
            DateTime now = DateTime.Now;
            _fileIO = new FileIO(
                /// Param 1: Now
                now,
                /// Param 2: Path
                string.Format(
                    ConfigAccess.GetValueInAppSettings(filePathKey),
                    controllerName.ToString()
                    ),
                /// Param 3: File Name
                string.Format(
                    ConfigAccess.GetValueInAppSettings(fileNameKey),
                    now.ToString(ConfigAccess.GetValueInAppSettings(LogKeys.LogDate))
                    ));
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