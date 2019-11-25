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
                if (cuentasPtas == null || cuentasPtas.ToList().Count.Equals(0)) return Ok();

                /// Obtiene Token para envío a Salesforce
                var token = RestBase.GetTokenByKey(SalesforceKeys.AuthServer, SalesforceKeys.AuthMethod);

                var cuentasPtasTasks = new List<Task>();
                foreach (var cuentaPta in cuentasPtas)
                {
                    var cuentaPtaTask = new Task(() =>
                    {
                        try
                        {
                            /// Envío de CuentaPTA a Salesforce
                            cuentaPta.UnidadNegocio = unidadNegocio.Descripcion;
                            cuentaPta.CodigoError = cuentaPta.MensajeError = string.Empty;
                            var cuentaPtaSf = ToSalesforceEntity(cuentaPta);
                            var responseCuentaPta = RestBase.ExecuteByKey(SalesforceKeys.CrmServer, SalesforceKeys.CuentaPtaMethod, Method.POST, cuentaPta, true, token);
                            if (responseCuentaPta.StatusCode.Equals(HttpStatusCode.OK))
                            {
                                JsonManager.LoadText(responseCuentaPta.Content);
                                cuentaPta.CodigoError = JsonManager.GetSetting(OutParameter.SF_CodigoError);
                                cuentaPta.MensajeError = JsonManager.GetSetting(OutParameter.SF_MensajeError);
                                cuentaPta.DkCuenta = JsonManager.GetSetting(OutParameter.SF_IdCuenta);
                            }

                            /// Actualización de estado de Cuenta PTA hacia PTA
                            if (!string.IsNullOrEmpty(cuentaPta.CodigoError)) _cuentaPtaRepository.update
                        }
                        catch
                        {
                        }
                    });
                    cuentaPtaTask.Start();
                    cuentasPtasTasks.Add(cuentaPtaTask);
                }
                Task.WaitAll(cuentasPtasTasks.ToArray());
                return Ok(new { CuentasPta = cuentasPtas });
            }
            catch (Exception ex)
            {
                throw ex;
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
                    Accion = cuentaPta.Accion,
                    DkCuenta = cuentaPta.DkCuenta,
                    RazonSocial = cuentaPta.RazonSocial,
                    NombreComercial = cuentaPta.NombreComercial,
                    TipoCuenta = cuentaPta.TipoCuenta,
                    Propietario = cuentaPta.Propietario,
                    FechaAniversario = cuentaPta.FechaAniversario,
                    TipoDocumentoIdentidad = cuentaPta.TipoDocumentoIdentidad,
                    DocumentoIdentidad = cuentaPta.DocumentoIdentidad,
                    TipoDireccion = cuentaPta.TipoDireccion,
                    DireccionResidencia = cuentaPta.DireccionResidencia,
                    PaisResidencia = cuentaPta.PaisResidencia,
                    DepartamentoResidencia = cuentaPta.DepartamentoResidencia,
                    CiudadResidencia = cuentaPta.CiudadResidencia,
                    DistritoResidencia = cuentaPta.DistritoResidencia,
                    DireccionFiscal = cuentaPta.DireccionFiscal,
                    TipoTelefono1 = cuentaPta.TipoTelefono1,
                    Telefono1 = cuentaPta.Telefono1,
                    TipoTelefono2 = cuentaPta.TipoTelefono2,
                    Telefono2 = cuentaPta.Telefono2,
                    TipoTelefono3 = cuentaPta.TipoTelefono3,
                    Telefono3 = cuentaPta.Telefono3,
                    TelefonoEmergencia = cuentaPta.TelefonoEmergencia,
                    SitioWeb = cuentaPta.SitioWeb,
                    Twitter = cuentaPta.Twitter,
                    Facebook = cuentaPta.Facebook,
                    Linkedin = cuentaPta.LinkedIn,
                    Instagram = cuentaPta.Instagram,
                    TipoPresenciaDigital = cuentaPta.TipoPresenciaDigital,
                    UrlPresenciaDigital = cuentaPta.UrlPresenciaDigital,
                    TipoCorreo = cuentaPta.TipoCorreo,
                    Correo = cuentaPta.Correo,
                    AsesorIA = cuentaPta.Asesor_IA,
                    AsesorDM = cuentaPta.Asesor_DM,
                    PuntoContacto = cuentaPta.PuntoContacto,
                    CondicionPagoIA = cuentaPta.CondicionPago_IA,
                    CondicionPagoDM = cuentaPta.CondicionPago_DM,
                    LimiteCredito = cuentaPta.LimiteCredito,
                    Comentario = cuentaPta.Comentario,
                    CategValor = cuentaPta.CategoriaValor,
                    CategPerfilActitudTec = cuentaPta.CategoriaPerfilActitudTecnologica,
                    CategPerfilFidelidad = cuentaPta.CategoriaPerfilFidelidad,
                    Incentivo = cuentaPta.Incentivo,
                    EstadoActivacion = cuentaPta.EstadoActivacion,
                    Gds = cuentaPta.GDS,
                    Herramientas = cuentaPta.Herramientas,
                    FacturacionAnual = cuentaPta.FacturacionAnual,
                    ProyeccionFactAnual = cuentaPta.ProyeccionFacturacionAnual,
                    InicioRelacionComercial = cuentaPta.InicioRelacionComercial
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
