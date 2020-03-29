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
                detItinerarioList = (IEnumerable<DetalleItinerarioNM>)(_detalleItinerarioNMRepository.Read())[OutParameter.CursorDetalleItinerarioNM];
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
                    objEnvio = new { listadatos = detItinerarioNM_SF };
                    QuickLog(objEnvio, "body_request.json", "DetalleItinerarioNM", previousClear: true); /// ♫ Trace
                    var responseDetItinerarioNM = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.DetItinerarioNMMethod, Method.POST, objEnvio, true, token);
                    if (responseDetItinerarioNM.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        dynamic jsonResponse = (new JavaScriptSerializer()).DeserializeObject(responseDetItinerarioNM.Content);
                        try
                        {
                            var responseList = jsonResponse["respuestas"]; // Obtiene todo el json
                            QuickLog(responseList, "body_response.json");
                            foreach (var item in responseList)
                            {
                                try
                                {
                                    #region Deserialize
                                    var mensaje = (item["mensaje"] ?? string.Empty).ToString();
                                    var idOportunidad_SF = (item["idOportunidad_SF"] ?? string.Empty).ToString();
                                    var idItinerario_SF = (item["idItinerario_SF"] ?? string.Empty).ToString();
                                    var codigo = (item["codigo"] ?? string.Empty).ToString();
                                    #endregion

                                    var detItinerario = detItinerarioList.FirstOrDefault(c => c.idOportunidad_SF.Equals(idOportunidad_SF));

                                    detItinerario.CodigoError = codigo;
                                    detItinerario.MensajeError = mensaje;
                                    detItinerario.idItinerario_SF = idItinerario_SF;

                                    #region ReturnToDB
                                    var updOperation = _detalleItinerarioNMRepository.Update(detItinerario)[OutParameter.IdActualizados];
                                    if (int.TryParse(updOperation.ToString(), out int actualizados)) detItinerario.Actualizados = actualizados;
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
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

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);
            _detalleItinerarioNMRepository = new DetalleItinerarioNMRepository(unidadNegocioKey);
            return unidadNegocioKey;
        }
    }
}
