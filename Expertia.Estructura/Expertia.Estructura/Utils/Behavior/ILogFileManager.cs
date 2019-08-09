namespace Expertia.Estructura.Utils.Behavior
{
    public interface ILogFileManager
    {
        void WriteText(string text);
        void WriteLine(LogType logType, string content, bool indent = false);
    }
}
