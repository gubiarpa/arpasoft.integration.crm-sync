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
        public const string CondorConnKey = "CTConnKey";
        public const string DMConnKey = "DMConnKey";
        public const string NMConnKey = "NMConnKey";
        public const string IAConnKey = "IAConnKey";
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
        public const string Contacto = "api/contacto";
        public const string CuentaB2B = "api/cuentab2b";
        public const string CuentaB2C = "api/cuentab2c";
    }

    public static class RouteAction
    {
        public const string Auth = "auth";
        public const string Create = "create";
        public const string Read = "read";
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

    #region Formats
    public static class FormatTemplate
    {
        public const string LogLine = "{0}[{1}]\t[{2}]\t[{3}]\t[{4}]\t[{5}]\t[{6}]\n";
        public const string FileDate = "yyyyMMdd";
        public const string LongDate = "yyyy-MM-dd HH:mm:ss.fff";
    }
    #endregion

    #region Database
    public static class StoredProcedureName
    {
        #region CondorTravel
        public const string CT_Create_CuentaB2B = "CONDOR.CRM_PKG.SP_CREAR_CLIENTE";
        public const string CT_Update_CuentaB2B = "CONDOR.CRM_PKG.SP_ACTUALIZAR_CLIENTE";
        public const string CT_Create_Contacto = "CONDOR.CRM_PKG.SP_CREAR_CONTACTO";
        public const string CT_Update_Contacto = "CONDOR.CRM_PKG.SP_ACTUALIZAR_CONTACTO";
        #endregion

        #region DestinosMundiales
        public const string DM_Create_CuentaB2B = "DESTINOS_TRP.CRM_PKG.SP_CREAR_CLIENTE";
        public const string DM_Update_CuentaB2B = "DESTINOS_TRP.CRM_PKG.SP_ACTUALIZAR_CLIENTE";
        public const string DM_Create_Contacto = "DESTINOS_TRP.CRM_PKG.SP_CREAR_CONTACTO";
        public const string DM_Update_Contacto = "DESTINOS_TRP.CRM_PKG.SP_ACTUALIZAR_CONTACTO";
        #endregion

        #region Interagencias
        public const string IA_Create_CuentaB2B = "NUEVOMUNDO.CRM_PKG.SP_CREAR_CLIENTE";
        public const string IA_Update_CuentaB2B = "NUEVOMUNDO.CRM_PKG.SP_ACTUALIZAR_CLIENTE";
        public const string IA_Create_Contacto = "NUEVOMUNDO.CRM_PKG.SP_CREAR_CONTACTO";
        public const string IA_Update_Contacto = "NUEVOMUNDO.CRM_PKG.SP_ACTUALIZAR_CONTACTO";
        #endregion
    }

    public static class OutParameter
    {
        #region Error
        public const string CodigoError = "P_CODIGO_ERROR";
        public const string MensajeError = "P_MENSAJE_ERROR";
        #endregion

        #region Identifier
        public const string IdCuenta = "P_ID_CUENTA";
        public const string IdContacto = "P_ID_CONTACTO";
        #endregion
    }
    #endregion

    #region UnidadNegocio
    public enum UnidadNegocioKeys
    {
        CondorTravel,
        DestinosMundiales,
        NuevoMundo,
        InterAgencias
    }

    public static class UnidadNegocioShortNames
    {
        public const string CondorTravel = "CT";
        public const string DestinosMundiales = "DM";
        public const string NuevoMundo = "NM";
        public const string InterAgencias = "IA";
    }

    public static class Auxiliar
    {
        public const char ListSeparator = ';';
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