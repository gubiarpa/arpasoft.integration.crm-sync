namespace Expertia.Estructura.Utils
{
    #region ConfigKeys
    public static class SecurityKeys
    {
        public const string Token = "MainToken";
    }

    public static class DataBaseKeys
    {
        public const string ConnectionString = "";
    }

    public static class LogKeys
    {
        public const string LogPath = "LogPath";
        public const string LogName = "LogName";
    }
    #endregion

    #region ApiRoutes
    public static class RoutePrefix
    {
        public const string Contacto  = "api/contacto";
        public const string CuentaB2B = "api/cuentab2b";
        public const string CuentaB2C = "api/cuentab2c";        
    }

    public static class RouteAction
    {
        public const string Create = "create";
        public const string Read   = "read";
        public const string Update = "update";
        public const string Delete = "delete";
    }
    #endregion

    #region Messages
    public static class LogTypeMessage
    {
        public const string Info = "INFO";
        public const string Fail = "FAIL";
        public const string Warning = "WARN";
        public const string Undefined = "Undefined Type";
    }
    #endregion

    #region Formats
    public static class FormatTemplate
    {
        public const string LogLine = "[{0}]\t[{1}]\t[{2}]\t[{3}]\t[{4}]\t[{5}]\n";
        public const string FileDate = "yyyyMMdd";
        public const string LongDate = "yyyy-MM-dd HH:mm:ss.fff";
    }
    #endregion
}