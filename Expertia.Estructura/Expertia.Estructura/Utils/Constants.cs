﻿using System;

namespace Expertia.Estructura.Utils
{
    #region ConfigKeys
    public static class SecurityKeys
    {
        public const string Token = "MainToken";
        public const string SecreKey = "SecretKey";
        public const string ExpirationInMin = "ExpirationInMin";
        public const string AuthTimeoutKey = "AUTH_TIMEOUT";
        public const string CrmTimeoutKey = "CRM_TIMEOUT";
    }

    public static class ConnectionKeys
    {
        public const string CondorConnKey = "CTConnKey";
        public const string DMConnKey = "DMConnKey";
        public const string IAConnKey = "IAConnKey";
        public const string AWConnKey = "AWConnKey";
        public const string CondorConnKey_CL = "CTConnKey_CL";
        public const string CondorConnKey_EC = "CTConnKey_EC";
        public const string CondorConnKey_BR = "CTConnKey_BR";
    }

    public static class LogKeys
    {
        public const string LogPath = "LogPath";
        public const string LogName = "LogName";
        public const string LogDate = "LogDate";
    }
    #endregion

    #region ApiRoutes
    public static class RoutePrefix
    {
        public const string Contacto = "api/contacto";
        public const string CuentaB2B = "api/cuentab2b";
        public const string CuentaB2C = "api/cuentab2c";
        public const string Cotizacion = "api/cotizacion";
        public const string CotizacionJourneyou = "api/cotizacionjyou";
        public const string Subcodigo = "api/subcodigo";
        public const string File = "api/file";
        public const string Oportunidad = "api/oportunidad";
        public const string CuentaPta = "api/cuentapta";
        public const string ContactoPta = "api/contactopta";
        public const string RegionCuenta = "api/regioncuenta";
        public const string Ventas = "api/ventas";
        public const string FileRetail = "api/fileretail";
        public const string FacturacionFileRetail = "api/facturacionfileretail";
        public const string PedidoRetail = "api/pedidoretail";
    }

    public enum ActionMethod
    {
        Create,
        Update,
        Generate,
        Asociate
    }

    public static class RouteAction
    {
        public const string Create = "create";
        public const string Read = "read";
        public const string Update = "update";
        public const string Delete = "delete";
        public const string Generate = "generate";
        public const string Asociate = "asociate";
        public const string Send = "send";
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
        public const string UnidadNegocioNotFound = "Unidad de Negocio no encontrada";
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
        /// Cuenta B2B
        public const string CT_Create_CuentaB2B = "CONDOR.CRM_PKG.SP_CREAR_CLIENTE";
        public const string CT_Update_CuentaB2B = "CONDOR.CRM_PKG.SP_ACTUALIZAR_CLIENTE";
        /// Contacto
        public const string CT_Create_Contacto = "CONDOR.CRM_PKG.SP_CREAR_CONTACTO";
        public const string CT_Update_Contacto = "CONDOR.CRM_PKG.SP_ACTUALIZAR_CONTACTO";
        /// Cotización
        public const string CT_Obtiene_Cotizacion = "CONDOR.CRM_PKG.SP_OBTIENE_COTIZACION";
        /// File
        public const string CT_Obtiene_FileCT = "CONDOR.CRM_PKG.SP_OBTIENE_FILE";
        /// Región
        public const string CT_Register_RegionCuenta = "CONDOR.CRM_PKG.SP_ACTUALIZAR_ESTADO_CLIENTE";
        /// Venta
        public const string CT_Get_VentasResumen = "CONDOR.CRM_PKG.SP_RESUMEN_CLIENTE";
        #endregion

        #region JourneyYou
        /// Cotización
        public const string CT_Obtiene_NovedadCotizacion = "CONDOR.CRM_PKG.";
        #endregion

        #region DestinosMundiales
        // Cuenta B2B
        public const string DM_Create_CuentaB2B = "DESTINOS_TRP.CRM_PKG.SP_CREAR_CLIENTE";
        public const string DM_Update_CuentaB2B = "DESTINOS_TRP.CRM_PKG.SP_ACTUALIZAR_CLIENTE";
        // Contacto
        public const string DM_Create_Contacto = "DESTINOS_TRP.CRM_PKG.SP_CREAR_CONTACTO";
        public const string DM_Update_Contacto = "DESTINOS_TRP.CRM_PKG.SP_ACTUALIZAR_CONTACTO";
        // Cotización
        public const string DM_Generate_Cotizacion = "DESTINOS_TRP.CRM_PKG.SP_GENERAR_COTIZACION";
        public const string DM_Asociate_Cotizacion = "DESTINOS_TRP.CRM_PKG.SP_ASOCIAR_COTIZACION";
        public const string DM_Send_Cotizacion = "DESTINOS_TRP.CRM_PKG.SP_ENVIAR_COTIZACION";
        public const string DM_Read_Cotizacion = "DESTINOS_TRP.CRM_PKG.SP_LISTAR_COTIZACION";
        public const string DM_Update_Cotizacion = "DESTINOS_TRP.CRM_PKG.SP_ACTUALIZAR_ENVIO_COTIZACION";
        // Subcodigo
        public const string DM_Create_Subcodigo = "DESTINOS_TRP.CRM_PKG.SP_CREAR_SUBCODIGO";
        public const string DM_Read_Subcodigo = "DESTINOS_TRP.CRM_PKG.SP_LISTAR_SUBCODIGO";
        public const string DM_Update_Subcodigo = "DESTINOS_TRP.CRM_PKG.SP_ACTUALIZAR_ENVIO_SUBCODIGO";
        /// Oportunidad
        public const string DM_Read_Oportunidad = "DESTINOS_TRP.CRM_PKG.SP_LISTAR_OPORTUNIDAD";
        public const string DM_Update_Oportunidad = "DESTINOS_TRP.CRM_PKG.SP_ACTUALIZAR_ENVIO_OPOR";
        /// Cuenta PTA
        public const string DM_Read_CuentaPta = "DESTINOS_TRP.CRM_PKG.SP_LISTAR_CLIENTE";
        public const string DM_Update_CuentaPta = "DESTINOS_TRP.CRM_PKG.SP_ACTUALIZAR_ENVIO_CLIENTE";
        /// Contacto PTA
        public const string DM_Read_ContactoPta = "DESTINOS_TRP.CRM_PKG.SP_LISTAR_CONTACTO";
        public const string DM_Update_ContactoPta = "DESTINOS_TRP.CRM_PKG.SP_ACTUALIZAR_ENVIO_CONTACTO";
        #endregion

        #region Interagencias
        /// Cuenta B2B
        public const string IA_Create_CuentaB2B = "NUEVOMUNDO.CRM_PKG.SP_CREAR_CLIENTE";
        public const string IA_Update_CuentaB2B = "NUEVOMUNDO.CRM_PKG.SP_ACTUALIZAR_CLIENTE";
        /// Contacto
        public const string IA_Create_Contacto = "NUEVOMUNDO.CRM_PKG.SP_CREAR_CONTACTO";
        public const string IA_Update_Contacto = "NUEVOMUNDO.CRM_PKG.SP_ACTUALIZAR_CONTACTO";
        /// Subcodigo
        public const string IA_Create_Subcodigo = "NUEVOMUNDO.CRM_PKG.SP_CREAR_SUBCODIGO";
        public const string IA_Read_Subcodigo = "NUEVOMUNDO.CRM_PKG.SP_LISTAR_SUBCODIGO";
        public const string IA_Update_Subcodigo = "NUEVOMUNDO.CRM_PKG.SP_ACTUALIZAR_ENVIO_SUBCODIGO";
        /// Agencia PNR
        public const string IA_Read_AgenciaPnr = "NUEVOMUNDO.CRM_PKG.SP_LISTAR_AGENCIA_PNR_NOVEDAD";
        /// File
        public const string IA_Read_File = "NUEVOMUNDO.CRM_PKG.SP_ENVIAR_FILE_NOVEDAD";
        public const string IA_Update_File = "NUEVOMUNDO.CRM_PKG.SP_ACTUALIZAR_ENVIO_FILE";
        /// Boleto
        public const string IA_Update_Boleto = "NUEVOMUNDO.CRM_PKG.SP_ACTUALIZAR_ENVIO_BOLETO";
        /// Oportunidad
        public const string IA_Read_Oportunidad = "NUEVOMUNDO.CRM_PKG.SP_LISTAR_OPORTUNIDAD";
        public const string IA_Update_Oportunidad = "NUEVOMUNDO.CRM_PKG.SP_ACTUALIZAR_ENVIO_OPOR";
        /// Cuenta PTA
        public const string IA_Read_CuentaPta = "NUEVOMUNDO.CRM_PKG.SP_LISTAR_CLIENTE";
        public const string IA_Update_CuentaPta = "NUEVOMUNDO.CRM_PKG.SP_ACTUALIZAR_ENVIO_CLIENTE";
        /// Contacto PTA
        public const string IA_Read_ContactoPta = "NUEVOMUNDO.CRM_PKG.SP_LISTAR_CONTACTO";
        public const string IA_Update_ContactoPta = "NUEVOMUNDO.CRM_PKG.SP_ACTUALIZAR_ENVIO_CONTACTO";
        #endregion

        #region AppWebs
        /// Contacto
        public const string AW_Create_Contacto = "APPWEBS.CRM_PKG.SP_CREAR_CONTACTO";
        public const string AW_Update_Contacto = "APPWEBS.CRM_PKG.SP_ACTUALIZAR_CONTACTO";
        /// Oportunidad
        public const string AW_Read_Oportunidad = "APPWEBS.CRM_PKG.SP_LISTAR_OPORTUNIDAD";
        public const string AW_Update_Oportunidad = "APPWEBS.CRM_PKG.SP_ACTUALIZAR_ENVIO_OPOR";
        /// Contacto PTA
        public const string AW_Read_ContactoPta = "APPWEBS.CRM_PKG.SP_LISTAR_CONTACTO";
        public const string AW_Update_ContactoPta = "APPWEBS.CRM_PKG.SP_ACTUALIZAR_ENVIO_CONTACTO";

        ///Facturacion file retail
        public const string AW_Ins_factFileRetail = "APPWEBS.PKG_Desglose_CA.SP_ACTUALIZAR_DATOSFACTURACION";
        public const string AW_Upd_factFileRetail = "APPWEBS.PKG_Desglose_CA.SP_INSERTAR_DATOSFACTURACION";

        /// Pedido
        public const string AW_Create_Pedido = "APPWEBS.PKG_PAGO_ONLINE.SP_INS_PEDIDO";
        public const string AW_Rpta_SafetyPay = "APPWEBS.PKG_PAGO_ONLINE.SP_GET_RPTA_PAGO_SAFETYPAY";
        public const string AW_Get_Monedas_PedidoSF = "APPWEBS.PKG_PAGO_ONLINE.SP_GET_MONEDAS_PAGO_SAFETYPAY";
        public const string AW_Insert_FormaPago_Pedido = "APPWEBS.PKG_PAGO_ONLINE.SP_INS_FORMA_PAGO_PEDIDO";
        public const string AW_Update_FechaExpira_Pedido = "APPWEBS.PKG_PAGO_ONLINE.SP_UPD_FECHAEXPIRA_PEDIDO";
        public const string AW_Get_Mail_Web = "APPWEBS.PKG_MAIL_LWEB.SP_MW_OBTIENE_X_ID";
        public const string AW_Get_Datos_Usuario = "APPWEBS.PKG_PERSONAL.SP_OBTIENE_DATOS_X_USUARIO";
        public const string AW_Insert_Post_Cotizacion = "APPWEBS.PKG_COTIZACION_VTA_WFF.SP_INSERTA_POST_COT";
        public const string AW_Update_Estado_Cotizacion = "APPWEBS.PKG_COTIZACION_VTA_WFF.SP_ACTUALIZA_EST_COT";
        #endregion
    }

    public static class SalesforceKeys
    {
        #region SalesforceRest
        /// Server
        public const string AuthServer = "AUTH_SERVER";
        /// Methods
        public const string AuthMethod = "AUTH_METHODNAME";
        public const string SubcodigoMethod = "SUBCODIGO_METHODNAME";
        public const string PnrMethod = "PNR_METHODNAME";
        public const string FileMethod = "FILE_METHODNAME";
        public const string BoletoMethod = "BOLETO_METHODNAME";
        public const string OportunidadMethod = "OPORTUNIDAD_METHODNAME";
        public const string CotizacionListMethod = "COTIZACIONLIST_METHODNAME";
        public const string CuentaPtaMethod = "CUENTAPTA_METHODNAME";
        public const string ContactoPtaMethod = "CONTACTOPTA_METHODNAME";
        /// Actions
        public const string CreateAction = "Crear";
        public const string UpdateAction = "Actualizar";
        #endregion
    }

    public static class OutParameter
    {
        #region Size
        public const int DefaultSize = 4000;
        #endregion

        #region Default
        public static DateTime MinDate { get { return new DateTime(1900, 1, 1); } }
        #endregion

        #region Error
        public const string CodigoError = "P_CODIGO_ERROR";
        public const string MensajeError = "P_MENSAJE_ERROR";
        #endregion

        #region Identifier
        public const string IdCuenta = "P_ID_CUENTA";
        public const string IdContacto = "P_ID_CONTACTO";
        public const string IdCotizacion = "P_ID_COTIZACION";
        public const string IdSubcodigo = "P_ID_SUBCODIGO";
        public const string IdActualizados = "P_ACTUALIZADOS";
        public const string NumId = "pNumId_out";
        public const string IdDatosFactura = "ID_DATOS_FACTURA";
        public const string IdPedido = "pNumIdNewPedido_out";
        #endregion

        #region Cursors
        public const string CursorSubcodigo = "P_SUBCODIGO";
        public const string CursorCotizacionDM = "P_COTIZACION";
        public const string CursorCotizacion = "P_RECORDSET";
        public const string CursorFileCT = "P_RECORDSET";
        public const string CursorCotizacionDet = "P_COTIZACION_DETALLE";
        public const string CursorAgenciaPnr = "P_CLIENTE_PNR";
        public const string CursorFile = "P_FILE";
        public const string CursorBoleto = "P_BOLETO";
        public const string CursorOportunidad = "P_OPORTUNIDAD";
        public const string CursorCuentaPta = "P_CLIENTE";
        public const string CursorContactoPta = "P_CONTACTO";
        public const string CursorVentas = "P_CUR_RESU_VENTAS";
        public const string CursorSafetyPay = "pCurResult_out";
        public const string CursorMonedasPedidoSF = "pCurResult_out";
        public const string CursorMailWeb = "pCurResult_out";
        public const string CursorDtosPersonal = "pCurResult_out";
        #endregion

        #region Fields
        public const string NombrePuntoVenta = "P_NOMBRE_PUNTO_VENTA";
        public const string NumeroSubcodigo = "P_NUMERO_SUBCODIGO";
        public const string NombreGrupo = "P_NOMBRE_GRUPO";
        public const string FechaSalida = "P_FECHA_SALIDA";
        public const string FechaRetorno = "P_FECHA_RETORNO";
        public const string NombreOrigen = "P_NOMBRE_ORIGEN";
        public const string NombrePais = "P_NOMBRE_PAIS";
        public const string NombreCiudad = "P_NOMBRE_CIUDAD";
        public const string NombreVendedorCounter = "P_NOMBRE_VENDEDOR_COUNTER";
        public const string NombreVendedorCotizador = "P_NOMBRE_VENDEDOR_COTIZADOR";
        public const string NombreVendedorReserva = "P_NOMBRE_VENDEDOR_RESERVA";
        public const string NumeroIdPostSRV = "pNumIdNewPost_out";
        #endregion

        #region SalesforceParameters
        public const string SF_CodigoError = "CODIGO_ERROR";
        public const string SF_MensajeError = "MENSAJE_ERROR";
        public const string SF_IdOportunidad = "ID_OPORTUNIDAD_SF";
        public const string SF_IdCuenta = "ID_CUENTA_SF";
        public const string SF_IdContacto = "ID_CONTACTO_SF";

        public const string SF_Token = "TOKEN";
        public const string SF_UrlAuth = "URL_AUTHORIZED";
        #endregion
    }
    #endregion

    #region Country
    public static class CountryName
    {
        public const string Peru = "PE";
        public const string Chile = "CL";
        public const string Ecuador = "EC";
        public const string Brasil = "BR";
    }
    #endregion

    #region UnidadNegocio
    public enum UnidadNegocioKeys
    {
        CondorTravel,
        DestinosMundiales,
        Interagencias,
        AppWebs,
        CondorTravel_CL,
        CondorTravel_EC,
        CondorTravel_BR
    }

    public static class UnidadNegocioShortNames
    {
        public const string CondorTravel = "CT";
        public const string DestinosMundiales = "DM";
        public const string InterAgencias = "IA";
        public const string AppWebs = "AW";
        public const string CondorTravelCL = "CL";
    }

    public static class UnidadNegocioLongName
    {
        public const string CondorTravel = "Condor Travel";
        public const string DestinosMundiales = "Destinos Mundiales";
        public const string Interagencias = "Interagencias";
        public const string CondorTravelCL = "Condor Travel Chile";
    }

    public static class Auxiliar
    {
        public const char ListSeparator = ';';
    }

    public static class ApiResponseCode
    {
        public const string ErrorCode = "{ex}";
        public const string SI = "SI";
        public const string NO = "NO";
    }

    public static class DbResponseCode
    {
        // Response
        public const string DelayRetryKey = "DelayTimeInSecs";
        public const int DefaultDelay = 5;
        // Errors
        public const string Success = "OK";
        public const string CuentaYaExiste = "C1";
        public const string CuentaNoExiste = "C2";
        public const string ContactoYaExiste = "D1";
        public const string ContactoNoExiste = "D2";
    }

    public static class SfResponseCode
    {
        public const string CuentaYaExiste = "C1";
        public const string CuentaNoExiste = "C2";
        public const string FileYaExiste = "F1";
        public const string FileNoExiste = "F2";
        public const string BoletoYaExiste = "B1";
        public const string BoletoNoExiste = "B2";
    }

    public static class Constantes_SRV
    {
        public const string ID_TIPO_POST_SRV_USUARIO = "2";

        #region Estados
        public const short ID_ESTADO_COT_PENDIENTE_PAGO = 11;
        #endregion
    }

    public static class Constantes_Pedido
    {
        public const string ID_TIPO_PEDIDO_OTROS = "OTR";
        public const int ID_CANAL_VENTA_CONTACT_CENTER = 94;
        public const Int16 ID_FORMA_PAGO_SAFETYPAY_ONLINE = 6;
        public const string CODE_FPAGO_GENERAL = "SF";
        public const string USERAGCORPGENERAPEDIDOLOGO = "userAGCorpGeneraPedidoLogoEmail";
    }

    public static class Constantes_SafetyPay
    {
        public const string CurrencyUSD = "USD";
        public const string CodeSafetyPayOnline = "SF";
        public const int ID_MAIL_SOLICITUD_PAGO_SERVICIO_SF = 78;
    }

    public static class Webs_Cid
    {
        public const int DM_WEB_ID = 4;
        public const int NM_WEB_ID = 7;
        public const int ID_WEB_IA = 3;
        public const int ID_WEB_NMV_RECEPTIVO = 26;
        public const int ID_WEB_WEBFAREFINDER = 39;
    }

    public static class Oficina
    {
        public const int ID_OFI_NMV = 23;
        public const int ID_OFI_NMVCOM = 62;
    }

    public static class Departamento
    {
        public const int ID_DEP_INTERNO = 52;
        public const int ID_DEP_RECEPTIVO = 10;
        public const int ID_DEP_OPERACIONES = 20;
        public const int ID_DEP_COUNTER = 6;
        public const int ID_DEP_SISTEMAS = 11;
    }

    public static class UtilityCorreo
    {
        public const string ERR_FOLDER_PATH = @"C:\InetPubNM\Webs_Intranet\wwwRootInterAgencias\";
    }
    #endregion

    #region OperationResult
    public enum ResultType
    {
        Success,
        Fail
    }

    public enum InstantKey
    {
        Salesforce,
        Oracle
    }
    #endregion
}