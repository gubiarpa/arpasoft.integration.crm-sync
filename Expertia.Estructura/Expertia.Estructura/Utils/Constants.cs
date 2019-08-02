namespace Expertia.Estructura.Utils
{
    #region ConfigKeys
    public static class SecurityKeys
    {
        public const string Token = "MainToken";
        public const string SecreKey = "SecretKey";
        public const string ExpirationInMin = "ExpirationInMin";
    }

    public static class ConnectionKeys
    {
        public const string MDMConnKey = "MdmConnKey";
        public const string CondorConnKey = "";
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
        public const string Login = "api/login";
        public const string Contacto  = "api/contacto";
        public const string CuentaB2B = "api/cuentab2b";
        public const string CuentaB2C = "api/cuentab2c";        
    }

    public static class RouteAction
    {
        public const string Auth = "auth";
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
        public const string Field = "FILD";
        public const string Fail = "FAIL";
        public const string Warning = "WARN";
        public const string Undefined = "Undefined Type";
    }

    public static class LogLineMessage
    {
        public const string Unauthorized = "Unauthorized";
        public const string BadRequest = "Bad Request";
    }
    #endregion

    #region DataBaseKeys
    public static class DataBaseKeys
    {
        public const string MdmPkg = "MdmPkg";
    }
    #endregion

    #region Formats
    public static class FormatTemplate
    {
        public const string LogLine = "{0}[{1}]\t[{2}]\t[{3}]\t[{4}]\t[{5}]\t[{6}]\n";
        public const string FileDate = "yyyyMMdd";
        public const string LongDate = "yyyy-MM-dd HH:mm:ss.fff";
    }
    #endregion

    #region OperationResult
    public enum ResultType
    {
        Success,
        Fail
    }
    #endregion
}