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
    public class DetalleHotelNMController : BaseController<object>
    {
        #region Properties
        private IDetalleHotelNMRepository _detalleHotelNMRepository;
        protected override ControllerName _controllerName => ControllerName.DetalleHotelNM;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<DetalleHotelNM> detalleHotelNMs = null;
            string error = string.Empty;
            object objEnvio = null;

            try
            {
                var _unidadNegocio = GetUnidadNegocio(unidadNegocio.Descripcion);
                RepositoryByBusiness(_unidadNegocio);
                _instants[InstantKey.Salesforce] = DateTime.Now;

                /// I. Consulta de Detalle Itinerario NM
                detalleHotelNMs = (IEnumerable<DetalleHotelNM>)(_detalleHotelNMRepository.Send(_unidadNegocio))[OutParameter.CursorDetalleHotelNM];
                if (detalleHotelNMs == null || detalleHotelNMs.ToList().Count.Equals(0)) return Ok(detalleHotelNMs);

                /// Obtiene Token para envío a Salesforce
                var authSf = RestBase.GetToken();
                var token = authSf[OutParameter.SF_Token].ToString();
                var crmServer = authSf[OutParameter.SF_UrlAuth].ToString();

                /// preparación de hotel para envio a Salesforce
                var detalleHotelNMSF = new List<object>();
                foreach (var detalleHotel in detalleHotelNMs)
                {
                    detalleHotelNMSF.Add(detalleHotel.ToSalesforceEntity());
                }


                try
                {
                    /// Envío de CuentaNM a Salesforce
                    ClearQuickLog("body_request.json", "DetalleHotelNM"); /// ♫ Trace
                    objEnvio = new { cotizaciones = detalleHotelNMSF };
                    QuickLog(objEnvio, "body_request.json", "DetalleHotelNM"); /// ♫ Trace


                    var responseDetalleHotelNM = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.DetalleHotelNMMethod, Method.POST, objEnvio, true, token);
                    if (responseDetalleHotelNM.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        dynamic jsonResponse = (new JavaScriptSerializer()).DeserializeObject(responseDetalleHotelNM.Content);

                        foreach (var detalleHotelNM in detalleHotelNMs)
                        {
                            foreach (var jsResponse in jsonResponse["Cotizaciones"])
                            {
                                detalleHotelNM.CodigoError = jsResponse[OutParameter.SF_Codigo];
                                detalleHotelNM.MensajeError = jsResponse[OutParameter.SF_Mensaje];
                                detalleHotelNM.idOportunidad_SF = jsResponse[OutParameter.SF_IdOportunidad];
                                detalleHotelNM.idDetalleHotel_SF = jsResponse[OutParameter.SF_IdDetalleItinerario];

                                ///// Actualización de estado de Cuenta NM hacia ???????
                                //var updateResponse = _cuentaNMRepository.Update(cuentaNM);
                                //cuentaNM.Actualizados = int.Parse(updateResponse[OutParameter.IdActualizados].ToString());
                            }
                        }
                    }
                    else
                    {
                        error = responseDetalleHotelNM.StatusCode.ToString();
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }

                return Ok(new { DetalleHotelNM = detalleHotelNMs });
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
                    LegacySystems = detalleHotelNMs
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);
            _detalleHotelNMRepository = new DetalleHotelNMRepository(unidadNegocioKey);
            return unidadNegocioKey;
        }
    }
}
