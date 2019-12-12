using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.RestManager.Base;
using Expertia.Estructura.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.CuentaPta)]
    public class CuentaPtaController : BaseController<object>
    {
        #region Properties
        private ICuentaPtaRepository _cuentaPtaRepository;
        #endregion

        #region Constructor
        public CuentaPtaController()
        {
        }
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<CuentaPta> cuentasPtas = null;
            try
            {
                var _unidadNegocio = GetUnidadNegocio(unidadNegocio.Descripcion);
                RepositoryByBusiness(_unidadNegocio);
                _instants[InstantKey.Salesforce] = DateTime.Now;

                /// I. Consulta de Cuentas PTA
                cuentasPtas = (IEnumerable<CuentaPta>)(_cuentaPtaRepository.Read())[OutParameter.CursorCuentaPta];
                if (cuentasPtas == null || cuentasPtas.ToList().Count.Equals(0)) return Ok(cuentasPtas);

                /// Obtiene Token para envío a Salesforce
                var token = RestBase.GetTokenByKey(SalesforceKeys.AuthServer, SalesforceKeys.AuthMethod);

                foreach (var cuentaPta in cuentasPtas)
                {
                    try
                    {
                        /// Envío de CuentaPTA a Salesforce
                        cuentaPta.UnidadNegocio = unidadNegocio.Descripcion;
                        cuentaPta.CodigoError = cuentaPta.MensajeError = string.Empty;
                        var cuentaPtaSf = ToSalesforceEntity(cuentaPta);
                        var responseCuentaPta = RestBase.ExecuteByKey(SalesforceKeys.CrmServer, SalesforceKeys.CuentaPtaMethod, Method.POST, cuentaPtaSf, true, token);
                        if (responseCuentaPta.StatusCode.Equals(HttpStatusCode.OK))
                        {
                            dynamic jsonReponse = (new JavaScriptSerializer()).DeserializeObject(responseCuentaPta.Content);
                            cuentaPta.CodigoError = jsonReponse[OutParameter.SF_CodigoError];
                            cuentaPta.MensajeError = jsonReponse[OutParameter.SF_MensajeError];
                            cuentaPta.IdCuentaCrm = jsonReponse[OutParameter.SF_IdCuenta];

                            /// Actualización de estado de Cuenta PTA hacia PTA
                            var updateResponse = _cuentaPtaRepository.Update(cuentaPta);
                            cuentaPta.Actualizados = int.Parse(updateResponse[OutParameter.IdActualizados].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        cuentaPta.CodigoError = ApiResponseCode.ErrorCode;
                        cuentaPta.MensajeError = ex.Message;
                    }   
                }
                return Ok(new { CuentasPta = cuentasPtas });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    UnidadNegocio = unidadNegocio,
                    LegacySystems = cuentasPtas
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region Auxiliar
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.Interagencias:
                    _cuentaPtaRepository = new CuentaPta_IA_Repository();
                    break;
            }
            return unidadNegocioKey;
        }
        #endregion

        #region Salesforce
        private object ToSalesforceEntity(CuentaPta cuentaPta)
        {
            try
            {
                return new
                {
                    info = new
                    {
                        accion = cuentaPta.Accion,
                        dkCuenta = cuentaPta.DkCuenta.ToString(),
                        razonSocial = cuentaPta.RazonSocial,
                        nombreComercial = cuentaPta.NombreComercial,
                        tipoCuenta = cuentaPta.TipoCuenta,
                        propietario = cuentaPta.Propietario,
                        fechaAniversario = cuentaPta.FechaAniversario.ToString("dd/MM/yyyy"),
                        tipoDocumentoIdentidad = cuentaPta.TipoDocumentoIdentidad,
                        documentoIdentidad = cuentaPta.DocumentoIdentidad,
                        tipoDireccion = cuentaPta.TipoDireccion,
                        direccionResidencia = cuentaPta.DireccionResidencia,
                        paisResidencia = cuentaPta.PaisResidencia,
                        departamentoResidencia = cuentaPta.DepartamentoResidencia,
                        ciudadResidencia = cuentaPta.CiudadResidencia,
                        distritoResidencia = cuentaPta.DistritoResidencia,
                        direccionFiscal = cuentaPta.DireccionFiscal,
                        tipoTelefono1 = cuentaPta.TipoTelefono1,
                        telefono1 = cuentaPta.Telefono1,
                        tipoTelefono2 = cuentaPta.TipoTelefono2,
                        telefono2 = cuentaPta.Telefono2,
                        tipoTelefono3 = cuentaPta.TipoTelefono3,
                        telefono3 = cuentaPta.Telefono3,
                        telefonoEmergencia = cuentaPta.TelefonoEmergencia,
                        sitioWeb = cuentaPta.SitioWeb,
                        twitter = cuentaPta.Twitter,
                        facebook = cuentaPta.Facebook,
                        linkedin = cuentaPta.LinkedIn,
                        instagram = cuentaPta.Instagram,
                        tipoPresenciaDigital = cuentaPta.TipoPresenciaDigital,
                        urlPresenciaDigital = cuentaPta.UrlPresenciaDigital,
                        tipoCorreo = cuentaPta.TipoCorreo,
                        correo = cuentaPta.Correo,
                        asesorIA = cuentaPta.Asesor_IA,
                        asesorDM = cuentaPta.Asesor_DM,
                        puntoContacto = cuentaPta.PuntoContacto,
                        condicionPagoIA = cuentaPta.CondicionPago_IA,
                        condicionPagoDM = cuentaPta.CondicionPago_DM,
                        limiteCredito = cuentaPta.LimiteCredito.ToString("0.00"),
                        comentario = cuentaPta.Comentario,
                        categValor = cuentaPta.CategoriaValor,
                        categPerfilActitudTec = cuentaPta.CategoriaPerfilActitudTecnologica,
                        categPerfilFidelidad = cuentaPta.CategoriaPerfilFidelidad,
                        incentivo = cuentaPta.Incentivo,
                        estadoActivacion = cuentaPta.EstadoActivacion,
                        gds = cuentaPta.GDS,
                        herramientas = cuentaPta.Herramientas,
                        facturacionAnual = cuentaPta.FacturacionAnual,
                        proyeccionFactAnual = cuentaPta.ProyeccionFacturacionAnual,
                        inicioRelacionComercial = cuentaPta.InicioRelacionComercial.ToString("dd/MM/yyyy")
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
