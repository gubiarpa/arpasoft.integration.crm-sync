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
    public class DetalleItinerarioNMController : BaseController<object>
    {
        #region Properties
        private ISendRepository<DetalleItinerarioNM> _detalleItinerarioNMRepository;
        protected override ControllerName _controllerName => ControllerName.DetalleItinerarioNM;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<DetalleItinerarioNM> detItinerarioList = null;
            string error = string.Empty;
            object objEnvio = null;

            try
            {
                var _unidadNegocio = RepositoryByBusiness(GetUnidadNegocio(unidadNegocio.Descripcion));

                /// I. Consulta de Detalle Itinerario NM
                detItinerarioList = (IEnumerable<DetalleItinerarioNM>)(_detalleItinerarioNMRepository.Read(_unidadNegocio))[OutParameter.CursorDetalleItinerarioNM];
                if (detItinerarioList == null || detItinerarioList.ToList().Count.Equals(0)) return Ok(detItinerarioList);

                /// II. Obtiene Token y URL para envío a Salesforce
                var authSf = RestBase.GetToken();
                var token = authSf[OutParameter.SF_Token].ToString();
                var crmServer = authSf[OutParameter.SF_UrlAuth].ToString();

                /// III. Construímos lista para enviar a SF
                var detItinerarioNM_SF = new List<object>();
                foreach (var detItinerario in detItinerarioList)
                    detItinerarioNM_SF.Add(detItinerario.ToSalesforceEntity());

                try
                {
                    /// Envío de CuentaNM a Salesforce
                    objEnvio = new { listaDatos = detItinerarioNM_SF };
                    QuickLog(objEnvio, "body_request.json", "DetalleItinerarioNM", previousClear: true); /// ♫ Trace
                    var responseDetItinerarioNM = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.DetItinerarioNMMethod, Method.POST, objEnvio, true, token);
                    if (responseDetItinerarioNM.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        dynamic jsonResponse = (new JavaScriptSerializer()).DeserializeObject(responseDetItinerarioNM.Content);

                        try
                        {

                        }
                        catch (Exception)
                        {

                            throw;
                        }

                        foreach (var detalleItinerarioNM in detItinerarioList)
                        {
                            foreach (var jsResponse in jsonResponse["Cotizaciones"])
                            {
                                detalleItinerarioNM.CodigoError = jsResponse[OutParameter.SF_Codigo];
                                detalleItinerarioNM.MensajeError = jsResponse[OutParameter.SF_Mensaje];
                                detalleItinerarioNM.idOportunidad_SF = jsResponse[OutParameter.SF_IdOportunidad];
                                detalleItinerarioNM.idItinerario_SF = jsResponse[OutParameter.SF_IdDetalleItinerario];

                                /// Actualización de estado de Cuenta NM hacia ???????
                                //var updateResponse = _detalleItinerarioNMRepository.Update(cuentaNM);
                                //cuentaNM.Actualizados = (int)(updateResponse[OutParameter.IdActualizados].ToString());
                            }
                        }
                    }
                    else
                    {
                        error = responseDetItinerarioNM.StatusCode.ToString();
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }

                return Ok(new { DetalleItinerarioNM = detItinerarioList });
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
                    LegacySystems = detItinerarioList
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
