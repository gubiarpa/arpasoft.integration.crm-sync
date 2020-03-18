using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.NuevoMundo;
using Expertia.Estructura.RestManager.Base;
using Expertia.Estructura.RestManager.RestParse;
using Expertia.Estructura.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.DetalleHotelNM)]
    public class SolicitudPagoNMController : BaseController<object>
    {
        #region Properties
        private ISolicitudPagoNMRepository _solicitudPagoNMRepository;
        protected override ControllerName _controllerName => ControllerName.SolicitudPagoNM;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<SolicitudPagoNM> solicitudPagoNMs = null;
            string error = string.Empty;
            object objEnvio = null;

            try
            {
                var _unidadNegocio = GetUnidadNegocio(unidadNegocio.Descripcion);
                RepositoryByBusiness(_unidadNegocio);
                _instants[InstantKey.Salesforce] = DateTime.Now;

                /// I. Consulta de Solicitud Pago NM
                solicitudPagoNMs = (IEnumerable<SolicitudPagoNM>)(_solicitudPagoNMRepository.Send(_unidadNegocio))[OutParameter.CursorSolicitudPagoNM];
                if (solicitudPagoNMs == null || solicitudPagoNMs.ToList().Count.Equals(0)) return Ok(solicitudPagoNMs);

                /// Obtiene Token para envío a Salesforce
                var authSf = RestBase.GetToken();
                var token = authSf[OutParameter.SF_Token].ToString();
                var crmServer = authSf[OutParameter.SF_UrlAuth].ToString();

                /// preparación de hotel para envio a Salesforce
                var solicitudPagoNMSF = new List<object>();
                foreach (var solicitudPago in solicitudPagoNMs)
                {
                    solicitudPagoNMSF.Add(solicitudPago.ToSalesforceEntity());
                }


                try
                {
                    /// Envío de CuentaNM a Salesforce
                    ClearQuickLog("body_request.json", "SolicitudPagoNM"); /// ♫ Trace
                    objEnvio = new { cotizaciones = solicitudPagoNMSF };
                    QuickLog(objEnvio, "body_request.json", "SolicitudPagoNM"); /// ♫ Trace


                    var responseSolicitudPagoNM = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.SolicitudPagoNMMethod, Method.POST, objEnvio, true, token);
                    if (responseSolicitudPagoNM.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        dynamic jsonResponse = (new JavaScriptSerializer()).DeserializeObject(responseSolicitudPagoNM.Content);

                        foreach (var solicitudPagoNM in solicitudPagoNMs)
                        {
                            foreach (var jsResponse in jsonResponse["Cotizaciones"])
                            {
                                solicitudPagoNM.CodigoError = jsResponse[OutParameter.SF_Codigo];
                                solicitudPagoNM.MensajeError = jsResponse[OutParameter.SF_Mensaje];
                                solicitudPagoNM.idOportunidad_SF = jsResponse[OutParameter.SF_IdOportunidad];
                                solicitudPagoNM.IdRegSolicitudPago_SF = jsResponse[OutParameter.SF_IdRegSolicitudPago];

                                ///// Actualización de estado de Cuenta NM hacia ???????
                                //var updateResponse = _cuentaNMRepository.Update(cuentaNM);
                                //cuentaNM.Actualizados = int.Parse(updateResponse[OutParameter.IdActualizados].ToString());
                            }
                        }
                    }
                    else
                    {
                        error = responseSolicitudPagoNM.StatusCode.ToString();
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }

                return Ok(new { SolicitudPagoNM = solicitudPagoNMs});
            }
            catch (Exception ex)
            {
                error = error + " / " + ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    UnidadNegocio = unidadNegocio.Descripcion,
                    Error = error,
                    LegacySystems = solicitudPagoNMs
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);
            _solicitudPagoNMRepository = new SolicitudPagoNMRepository(unidadNegocioKey);
            return unidadNegocioKey;
        }
    }
}
