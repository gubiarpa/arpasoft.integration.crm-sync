using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.InterAgencias
{
    public class ContactoPta_IA_Repository : OracleBase<ContactoPta>, IContactoPtaRepository
    {
        #region Contructor
        public ContactoPta_IA_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.Interagencias) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation GetContactos()
        {
            var operation = new Operation();
            try
            {
                #region Parameters
                /// (1) P_CODIGO_ERROR
                AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (2) P_MENSAJE_ERROR
                AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (3) P_CONTACTO
                AddParameter(OutParameter.CursorContactoPta, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.IA_Read_ContactoPta);
                operation[OutParameter.CursorContactoPta] = ToContactoPta(GetDtParameter(OutParameter.CursorContactoPta));
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return operation;
        }

        public Operation Update(ContactoPta cuentaPta)
        {
            var operation = new Operation();
            try
            {
                #region Parameters
                /// (01) P_CODIGO_ERROR
                AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (02) P_MENSAJE_ERROR
                AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                /// (03) P_ID_CUENTA
                AddParameter("P_ID_CUENTA", OracleDbType.Int32, cuentaPta.DkAgencia);
                /// (04) P_CORRELATIVO
                AddParameter("P_CORRELATIVO", OracleDbType.Int32, cuentaPta.Correlativo);
                /// (05) P_USUWEB_ID
                AddParameter("P_USUWEB_ID", OracleDbType.Int32, cuentaPta.UsuarioWebId);
                /// (06) P_ID_CUENTA_CRM
                AddParameter("P_ID_CUENTA_CRM", OracleDbType.Varchar2, cuentaPta.IdCuentaCrm);
                /// (07) P_ID_CONTACTO_CRM
                AddParameter("P_ID_CONTACTO_CRM", OracleDbType.Varchar2, cuentaPta.IdContactoCrm);
                /// (08) P_ES_ATENCION
                AddParameter("P_ES_ATENCION", OracleDbType.Varchar2, cuentaPta.CodigoError);
                /// (09) P_DESCRIPCION
                AddParameter("P_DESCRIPCION", OracleDbType.Varchar2, cuentaPta.MensajeError);
                /// (10) P_ACTUALIZADOS
                AddParameter(OutParameter.IdActualizados, OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);
                #endregion

                #region Invoke
                ExecuteStoredProcedure(StoredProcedureName.IA_Update_ContactoPta);
                operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
                operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
                operation[OutParameter.IdActualizados] = GetOutParameter(OutParameter.IdActualizados);
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return operation;
        }
        #endregion

        #region Auxiliar
        private IEnumerable<ContactoPta> ToContactoPta(DataTable dt)
        {
            try
            {
                var contactoPtaList = new List<ContactoPta>();
                foreach (DataRow row in dt.Rows)
                {
                    #region Loading
                    var accion = row.StringParse("ACCION");
                    var dkAgencia = row.StringParse("DK_CUENTA");
                    var primerNombre = row.StringParse("PRIMER_NOMBRE");
                    var segundoNombre = row.StringParse("SEGUNDO_NOMBRE");
                    var apellidoPaterno = row.StringParse("APELLIDO_PATERNO");
                    var apellidoMaterno = row.StringParse("APELLIDO_MATERNO");
                    var estadoCivil = row.StringParse("ESTADO_CIVIL");
                    var cargo = row.StringParse("CARGO");
                    var tipoContacto = row.StringParse("TIPO_CONTACTO");
                    var genero = row.StringParse("GENERO");
                    var fechaNacimiento = row.DateTimeParse("FECHA_NACIMIENTO");
                    var tieneHijos = row.IntParse("TIENE_HIJOS");
                    var tipoDocumentoIdentidad = row.StringParse("TIPO_DOCUMENTO_IDENTIDAD");
                    var documentoIdentidad = row.StringParse("DOCUMENTO_IDENTIDAD");
                    var direccion = row.StringParse("DIRECCION");
                    var twitter  = row.StringParse("TWITTER");
                    var facebook = row.StringParse("FACEBOOK");
                    var linkedIn = row.StringParse("LINKEDIN");
                    var instagram = row.StringParse("INSTAGRAM");
                    var tipoPresenciaDigital = row.StringParse("TIPO_PRESENCIA_DIGITAL");
                    var urlPresenciaDigital = row.StringParse("URL_PRESENCIA_DIGITAL");
                    var tipoTelefono1 = row.StringParse("TIPO_TELEFONO_1");
                    var telefono1 = row.StringParse("TELEFONO_1");
                    var tipoTelefono2 = row.StringParse("TIPO_TELEFONO_2");
                    var telefono2 = row.StringParse("TELEFONO_2");
                    var telefonoEmergencia = row.StringParse("TELEFONO_EMERGENCIA");
                    var tipoCorreo = row.StringParse("TIPO_CORREO");
                    var correo = row.StringParse("CORREO");
                    var estadoContacto = row.StringParse("ESTADO_CONTACTO");
                    var esContactoMarketing = row.StringParse("ES_CONTACTO_MARKETING");
                    var correlativo = row.IntParse("CORRELATIVO");
                    var usuWebId = row.IntParse("USUWEB_ID");
                    #endregion

                    #region AddingElement
                    contactoPtaList.Add(new ContactoPta()
                    { 
                        Accion = accion,
                        DkAgencia = dkAgencia,
                        PrimerNombre = primerNombre,
                        SegundoNombre = segundoNombre,
                        ApellidoPaterno = apellidoPaterno,
                        ApellidoMaterno = apellidoMaterno,
                        EstadoCivil = estadoCivil,
                        Cargo = cargo,
                        TipoContacto = tipoContacto,
                        Genero = genero,
                        FechaNacimiento = fechaNacimiento,
                        TieneHijos = tieneHijos.Equals(1),
                        TipoDocumentoIdentidad = tipoDocumentoIdentidad,
                        DocumentoIdentidad = documentoIdentidad,
                        Direccion = direccion,
                        Twitter = twitter,
                        Facebook = facebook,
                        LinkedIn = linkedIn,
                        Instagram = instagram,
                        TipoPresenciaDigital = tipoPresenciaDigital,
                        UrlPresenciaDigital = urlPresenciaDigital,
                        TipoTelefono1 = tipoTelefono1,
                        Telefono1 = telefono1,
                        TipoTelefono2 = tipoTelefono2,
                        Telefono2 = telefono2,
                        TelefonoEmergencia = telefonoEmergencia,
                        TipoCorreo = tipoCorreo,
                        Correo = correo,
                        EstadoContacto = estadoContacto,
                        EsContactoMarketing = esContactoMarketing,
                        Correlativo = correlativo,
                        UsuarioWebId = usuWebId
                    });
                    #endregion
                }
                return contactoPtaList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}