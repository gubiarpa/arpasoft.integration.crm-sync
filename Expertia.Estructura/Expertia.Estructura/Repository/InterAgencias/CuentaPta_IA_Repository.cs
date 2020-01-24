using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Expertia.Estructura.Repository.InterAgencias
{
    public class CuentaPta_IA_Repository : OracleBase<object>, ICuentaPtaRepository
    {
        #region Constructor
        public CuentaPta_IA_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.Interagencias) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation Read(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.Interagencias)
        {
            var operation = new Operation();

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_OPORTUNIDAD
            AddParameter(OutParameter.CursorCuentaPta, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            string spName = string.Empty;
            switch (unidadNegocio)
            {
                case UnidadNegocioKeys.DestinosMundiales:
                    spName = StoredProcedureName.DM_Read_CuentaPta;
                    break;
                case UnidadNegocioKeys.Interagencias:
                    spName = StoredProcedureName.IA_Read_CuentaPta;
                    break;
                default:
                    throw new Exception(ApiResponseCode.ErrorCode);
            }
            ExecuteStoredProcedure(spName);
            operation[OutParameter.CursorCuentaPta] = ToCuentaPta(GetDtParameter(OutParameter.CursorCuentaPta));
            
            #endregion

            return operation;
        }

        public Operation Update(CuentaPta cuentaPta)
        {
            try
            {
                var operation = new Operation();

                #region Parameters
                /// (01) P_CODIGO_ERROR
                AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (02) P_MENSAJE_ERROR
                AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (03) P_ID_CUENTA
                AddParameter("P_ID_CUENTA", OracleDbType.Int32, cuentaPta.DkCuenta);
                /// (04) P_ID_CUENTA_CRM
                AddParameter("P_ID_CUENTA_CRM", OracleDbType.Varchar2, cuentaPta.IdCuentaCrm);
                /// (05) P_ES_ATENCION
                AddParameter("P_ES_ATENCION", OracleDbType.Varchar2, cuentaPta.CodigoError);
                /// (06) P_DESCRIPCION
                AddParameter("P_DESCRIPCION", OracleDbType.Varchar2, cuentaPta.MensajeError);
                /// (07) P_ACTUALIZADOS
                AddParameter(OutParameter.IdActualizados, OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);
                #endregion

                #region Invoke
                var spName = string.Empty;
                switch (_unidadNegocio)
                {
                    case UnidadNegocioKeys.DestinosMundiales:
                        spName = StoredProcedureName.DM_Update_CuentaPta;
                        break;
                    case UnidadNegocioKeys.Interagencias:
                        spName = StoredProcedureName.IA_Update_CuentaPta;
                        break;
                }
                ExecuteStoredProcedure(spName);
                operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
                operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
                operation[OutParameter.IdActualizados] = GetOutParameter(OutParameter.IdActualizados);
                #endregion

                return operation;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Parse
        private IEnumerable<CuentaPta> ToCuentaPta(DataTable dt)
        {
            try
            {
                var cuentaPtaList = new List<CuentaPta>();

                foreach (DataRow row in dt.Rows)
                {
                    #region Loading
                    var accion = row.StringParse("ACCION");
                    var dkCuenta = row.IntParse("DK_CUENTA");
                    var razonSocial = row.StringParse("RAZON_SOCIAL");
                    var nombreComercial = row.StringParse("NOMBRE_COMERCIAL");
                    var tipoCuenta = row.StringParse("TIPO_CUENTA");
                    var propietario = row.StringParse("PROPIETARIO");
                    var fechaAniversario = row.DateTimeParse("FECHA_ANIVERSARIO");
                    var tipoDocumentoIdentidad = row.StringParse("TIPO_DOCUMENTO_IDENTIDAD");
                    var documentoIdentidad = row.StringParse("DOCUMENTO_IDENTIDAD");
                    var tipoDireccion = row.StringParse("TIPO_DIRECCION");
                    var direccionResidencia = row.StringParse("DIRECCION_RESIDENCIA");
                    var paisResidencia = row.StringParse("PAIS_RESIDENCIA");
                    var departamentoResidencia = row.StringParse("DEPARTAMENTO_RESIDENCIA");
                    var ciudadResidencia = row.StringParse("CIUDAD_RESIDENCIA");
                    var distritoResidencia = row.StringParse("DISTRITO_RESIDENCIA");
                    var direccionFiscal = row.StringParse("DIRECCION_FISCAL");
                    var tipoTelefono1 = row.StringParse("TIPO_TELEFONO_1");
                    var telefono1 = row.StringParse("TELEFONO_1");
                    var tipoTelefono2 = row.StringParse("TIPO_TELEFONO_2");
                    var telefono2 = row.StringParse("TELEFONO_2");
                    var tipoTelefono3 = row.StringParse("TIPO_TELEFONO_3");
                    var telefono3 = row.StringParse("TELEFONO_3");
                    var telefonoEmergencia = row.StringParse("TELEFONO_EMERGENCIA");
                    var sitio_web = row.StringParse("SITIO_WEB");
                    var twitter = row.StringParse("TWITTER");
                    var facebook = row.StringParse("FACEBOOK");
                    var linkedin = row.StringParse("LINKEDIN");
                    var instagram = row.StringParse("INSTAGRAM");
                    var tipoPresenciaDigital = row.StringParse("TIPO_PRESENCIA_DIGITAL");
                    var urlPresenciaDigital = row.StringParse("URL_PRESENCIA_DIGITAL");
                    var tipoCorreo = row.StringParse("TIPO_CORREO");
                    var correo = row.StringParse("CORREO");
                    var asesor = row.StringParse("ASESOR");
                    var puntoContacto = row.StringParse("PUNTO_CONTACTO");
                    var condicionPago = row.StringParse("CONDICION_PAGO");
                    var limiteCredito = row.FloatParse("LIMITE_CREDITO");
                    var comentario = row.StringParse("COMENTARIO");
                    var categoriaValor = row.StringParse("CATEG_VALOR");
                    var categoriaPerfilActitudTecnologica = row.StringParse("CATEG_PERFIL_ACTITUD_TEC");
                    var categPerfilFidelidad = row.StringParse("CATEG_PERFIL_FIDELIDAD");
                    var incentivo = row.StringParse("INCENTIVO");
                    var estadoActivacion = row.StringParse("ESTADO_ACTIVACION");
                    var gds = row.StringParse("GDS");
                    var herramientas = row.StringParse("HERRAMIENTAS");
                    var facturacionAnual = row.FloatNullParse("FACTURACION_ANUAL");
                    var proyeccionFacturacionAnual = row.FloatNullParse("PROYECCION_FACT_ANUAL");
                    var inicioRelacionComercial = row.DateTimeParse("INICIO_RELACION_COMERCIAL");
                    #endregion

                    #region AddingElement
                    cuentaPtaList.Add(new CuentaPta()
                    {
                        Accion = accion,
                        DkCuenta = dkCuenta,
                        RazonSocial = razonSocial,
                        NombreComercial = nombreComercial,
                        TipoCuenta = tipoCuenta,
                        Propietario = propietario,
                        FechaAniversario = fechaAniversario,
                        TipoDocumentoIdentidad = tipoDocumentoIdentidad,
                        DocumentoIdentidad = documentoIdentidad,
                        TipoDireccion = tipoDireccion,
                        DireccionResidencia = direccionResidencia,
                        PaisResidencia = paisResidencia,
                        DepartamentoResidencia = departamentoResidencia,
                        CiudadResidencia = ciudadResidencia,
                        DistritoResidencia = distritoResidencia,
                        DireccionFiscal = direccionFiscal,
                        TipoTelefono1 = tipoTelefono1,
                        Telefono1 = telefono1,
                        TipoTelefono2 = tipoTelefono2,
                        Telefono2 = telefono2,
                        TipoTelefono3 = tipoTelefono3,
                        Telefono3 = telefono3,
                        TelefonoEmergencia = telefonoEmergencia,
                        SitioWeb = sitio_web,
                        Twitter = twitter,
                        Facebook = facebook,
                        LinkedIn = linkedin,
                        Instagram = instagram,
                        TipoPresenciaDigital = tipoPresenciaDigital,
                        UrlPresenciaDigital = urlPresenciaDigital,
                        TipoCorreo = tipoCorreo,
                        Correo = correo,
                        Asesor = asesor,
                        PuntoContacto = puntoContacto,
                        CondicionPago = condicionPago,
                        LimiteCredito = limiteCredito,
                        Comentario = comentario,
                        CategoriaValor = categoriaValor,
                        CategoriaPerfilActitudTecnologica = categoriaPerfilActitudTecnologica,
                        CategoriaPerfilFidelidad = categPerfilFidelidad,
                        Incentivo = incentivo,
                        EstadoActivacion = estadoActivacion,
                        GDS = gds,
                        Herramientas = herramientas,
                        FacturacionAnual = facturacionAnual,
                        ProyeccionFacturacionAnual = proyeccionFacturacionAnual,
                        InicioRelacionComercial = inicioRelacionComercial,
                        /// Datos de Reserva
                        CodigoError = string.Empty,
                        MensajeError = string.Empty,
                        Actualizados = -1
                    });
                    #endregion
                }

                return cuentaPtaList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}