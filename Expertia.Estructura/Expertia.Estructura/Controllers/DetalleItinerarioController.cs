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
    [RoutePrefix(RoutePrefix.DetalleItinerarioNM)]
    public class DetalleItinerarioController : BaseController<object>
    {
        #region Properties
        private IDetalleItinerarioNMRepository _detalleItinerarioNMRepository;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<DetalleItinerarioNM> detalleItinerarioNMs = null;
            string error = string.Empty;
            object objEnvio = null;

            try
            {
                var _unidadNegocio = GetUnidadNegocio(unidadNegocio.Descripcion);
                RepositoryByBusiness(_unidadNegocio);
                _instants[InstantKey.Salesforce] = DateTime.Now;

                /// I. Consulta de Detalle Itinerario NM
                detalleItinerarioNMs = (IEnumerable<DetalleItinerarioNM>)(_detalleItinerarioNMRepository.Send(_unidadNegocio))[OutParameter.CursorDetalleItinerarioNM];
                if (detalleItinerarioNMs == null || detalleItinerarioNMs.ToList().Count.Equals(0)) return Ok(detalleItinerarioNMs);

                /// Obtiene Token para envío a Salesforce
                var authSf = RestBase.GetToken();
                var token = authSf[OutParameter.SF_Token].ToString();
                var crmServer = authSf[OutParameter.SF_UrlAuth].ToString();

                /// preparación de cuenta para envio a Salesforce
                var detalleItinerarioNMSF = new List<object>();
                foreach (var detalleItinerario in detalleItinerarioNMs)
                {
                    detalleItinerarioNMSF.Add(detalleItinerario.ToSalesforceEntity());
                }


                try
                {
                    /// Envío de CuentaNM a Salesforce
                    ClearQuickLog("body_request.json", "DetalleItinerarioNM"); /// ♫ Trace
                    objEnvio = new { cotizaciones = detalleItinerarioNMSF };
                    QuickLog(objEnvio, "body_request.json", "DetalleItinerarioNM"); /// ♫ Trace


                    var responseCuentaNM = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.DetalleItinerarioNMMethod, Method.POST, objEnvio, true, token);
                    if (responseCuentaNM.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        dynamic jsonResponse = (new JavaScriptSerializer()).DeserializeObject(responseCuentaNM.Content);

                        foreach (var detalleItinerarioNM in detalleItinerarioNMs)
                        {
                            foreach (var jsResponse in jsonResponse["Cotizaciones"])
                            {
                                detalleItinerarioNM.CodigoError = jsResponse[OutParameter.SF_Codigo];
                                detalleItinerarioNM.MensajeError = jsResponse[OutParameter.SF_Mensaje];
                                detalleItinerarioNM.idOportunidad_SF = jsResponse[OutParameter.SF_IdOportunidad];
                                detalleItinerarioNM.idItinerario_SF = jsResponse[OutParameter.SF_IdDetalleItinerario];

                                ///// Actualización de estado de Cuenta NM hacia ???????
                                //var updateResponse = _cuentaNMRepository.Update(cuentaNM);
                                //cuentaNM.Actualizados = int.Parse(updateResponse[OutParameter.IdActualizados].ToString());
                            }
                        }
                    }
                    else
                    {
                        error = responseCuentaNM.StatusCode.ToString();
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }

                return Ok(new { DetalleItinerarioNM = detalleItinerarioNMs });
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
                    LegacySystems = detalleItinerarioNMs
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);
            _detalleItinerarioNMRepository = new DetalleItinerarioNMRepository(unidadNegocioKey);
            return unidadNegocioKey;
        }
    }
}
