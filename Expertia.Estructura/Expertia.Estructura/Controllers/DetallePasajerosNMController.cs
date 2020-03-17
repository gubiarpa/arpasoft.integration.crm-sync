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
    [RoutePrefix(RoutePrefix.DetallePasajerosNM)]
    public class DetallePasajerosNMController : BaseController<object>
    {
        #region Properties
        private IDetallePasajerosNMRepository _detallePasajerosNMRepository;
        protected override ControllerName _controllerName => ControllerName.DetallePasajerosNM;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<DetallePasajerosNM> detallePasajerosNMs = null;
            string error = string.Empty;
            object objEnvio = null;

            try
            {
                var _unidadNegocio = GetUnidadNegocio(unidadNegocio.Descripcion);
                RepositoryByBusiness(_unidadNegocio);
                _instants[InstantKey.Salesforce] = DateTime.Now;

                /// I. Consulta de Detalle Itinerario NM
                detallePasajerosNMs = (IEnumerable<DetallePasajerosNM>)(_detallePasajerosNMRepository.Send(_unidadNegocio))[OutParameter.CursorDetallePasajerosNM];
                if (detallePasajerosNMs == null || detallePasajerosNMs.ToList().Count.Equals(0)) return Ok(detallePasajerosNMs);

                /// Obtiene Token para envío a Salesforce
                var authSf = RestBase.GetToken();
                var token = authSf[OutParameter.SF_Token].ToString();
                var crmServer = authSf[OutParameter.SF_UrlAuth].ToString();

                /// preparación de cuenta para envio a Salesforce
                var detallePasajerosNMSF = new List<object>();
                foreach (var detallePasajeros in detallePasajerosNMs)
                {
                    detallePasajerosNMSF.Add(detallePasajeros.ToSalesforceEntity());
                }


                try
                {
                    /// Envío de CuentaNM a Salesforce
                    ClearQuickLog("body_request.json", "DetalleItinerarioNM"); /// ♫ Trace
                    objEnvio = new { cotizaciones = detallePasajerosNMSF };
                    QuickLog(objEnvio, "body_request.json", "DetalleItinerarioNM"); /// ♫ Trace


                    var responseDetallePasajeroNM = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.DetallePasajeroNMMethod, Method.POST, objEnvio, true, token);
                    if (responseDetallePasajeroNM.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        dynamic jsonResponse = (new JavaScriptSerializer()).DeserializeObject(responseDetallePasajeroNM.Content);

                        foreach (var detallePasajeroNM in detallePasajerosNMs)
                        {
                            foreach (var jsResponse in jsonResponse["Cotizaciones"])
                            {
                                detallePasajeroNM.CodigoError = jsResponse[OutParameter.SF_Codigo];
                                detallePasajeroNM.MensajeError = jsResponse[OutParameter.SF_Mensaje];
                                detallePasajeroNM.idOportunidad_SF = jsResponse[OutParameter.SF_IdOportunidad];
                                detallePasajeroNM.idPasajero_SF = jsResponse[OutParameter.SF_IdPasajero];

                                ///// Actualización de estado de Cuenta NM hacia ???????
                                //var updateResponse = _cuentaNMRepository.Update(cuentaNM);
                                //cuentaNM.Actualizados = int.Parse(updateResponse[OutParameter.IdActualizados].ToString());
                            }
                        }
                    }
                    else
                    {
                        error = responseDetallePasajeroNM.StatusCode.ToString();
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }

                return Ok(new { DetallePasajerosNM = detallePasajerosNMs });
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
                    LegacySystems = detallePasajerosNMs
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);
            _detallePasajerosNMRepository = new DetallePasajerosNMRepository(unidadNegocioKey);
            return unidadNegocioKey;
        }
    }
}
