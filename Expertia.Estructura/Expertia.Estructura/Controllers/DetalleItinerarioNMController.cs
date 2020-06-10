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
        private  IDetalleItinerarioNMRepository _detalleItinerarioNMRepository;
        protected override ControllerName _controllerName => ControllerName.DetalleItinerarioNM;
        #endregion

        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<DetalleItinerarioNM> detItinerarioList = null;
            List<RptaItinerarioSF> ListRptaItinerarioSF_Fail = new List<RptaItinerarioSF>();
            RptaItinerarioSF _rptaItinerarioSF = null;

            object SFResponse = null;
            string error = string.Empty;
            object objEnvio = null;

            try
            {                
                var _unidadNegocio = RepositoryByBusiness(unidadNegocio.Descripcion.ToUnidadNegocio());

                /// I. Consulta de Detalle Itinerario NM
                detItinerarioList = (IEnumerable<DetalleItinerarioNM>)(_detalleItinerarioNMRepository.GetItinerarios())[OutParameter.CursorDetalleItinerarioNM];
                if (detItinerarioList == null || detItinerarioList.ToList().Count.Equals(0)) return Ok(false);

                /// II. Obtiene Token y URL para envío a Salesforce
                var authSf = RestBase.GetToken();
                var token = authSf[OutParameter.SF_Token].ToString();
                var crmServer = authSf[OutParameter.SF_UrlAuth].ToString();

                /// III. Construímos lista para enviar a SF
                var detItinerarioNM_SF = new List<object>();
                foreach (var detItinerario in detItinerarioList)
                {
                    detItinerarioNM_SF.Add(detItinerario.ToSalesforceEntity());
                }                    

                try
                {
                    /// Envío de CuentaNM a Salesforce
                    objEnvio = new { listadatos = detItinerarioNM_SF };
                    QuickLog(objEnvio, "body_request.json", "DetalleItinerarioNM", previousClear: true); /// ♫ Trace

                    var responseDetItinerarioNM = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.DetItinerarioNMMethod, Method.POST, objEnvio, true, token);
                    if (responseDetItinerarioNM.StatusCode.Equals(HttpStatusCode.OK))
                    {                        
                        dynamic jsonResponse = new JavaScriptSerializer().DeserializeObject(responseDetItinerarioNM.Content);
                        QuickLog(jsonResponse, "body_response.json", "DetalleItinerarioNM", previousClear: true); /// ♫ Trace

                        SFResponse = jsonResponse["respuestas"];
                        foreach (var item in jsonResponse["respuestas"])
                        {
                            try
                            {
                                #region Deserialize
                                _rptaItinerarioSF = new RptaItinerarioSF();

                                _rptaItinerarioSF.CodigoError = "OK";
                                _rptaItinerarioSF.MensajeError = "TST";
                                _rptaItinerarioSF.idOportunidad_SF = "006R000000WAUr4IAH";
                                _rptaItinerarioSF.idItinerario_SF = "006R000000WAUr4IAC";
                                _rptaItinerarioSF.Identificador_NM = "2";

                                _rptaItinerarioSF.CodigoError = item[OutParameter.SF_Codigo];
                                _rptaItinerarioSF.MensajeError = item[OutParameter.SF_Mensaje];
                                _rptaItinerarioSF.idOportunidad_SF = item[OutParameter.SF_IdOportunidad2];
                                _rptaItinerarioSF.idItinerario_SF = item[OutParameter.SF_IdItinerario];
                                _rptaItinerarioSF.Identificador_NM = item[OutParameter.SF_IdentificadorNM];
                                #endregion
                                           
                                #region ReturnToDB
                                var updOperation = _detalleItinerarioNMRepository.Update(_rptaItinerarioSF);

                                if (Convert.IsDBNull(updOperation[OutParameter.IdActualizados]) == true || updOperation[OutParameter.IdActualizados].ToString().ToLower().Contains("null") || Convert.ToInt32(updOperation[OutParameter.IdActualizados].ToString()) <= 0)
                                {
                                    error = error + "Error en el Proceso de Actualizacion - No Actualizo Ningun Registro. Identificador NM : " + _rptaItinerarioSF.Identificador_NM.ToString() + "||||";
                                    ListRptaItinerarioSF_Fail.Add(_rptaItinerarioSF);
                                    /*Analizar si se deberia grabar en una tabla de bd para posteriormente darle seguimiento*/
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {                                    
                                error = error + "Error en el Proceso de Actualizacion - Response SalesForce : " + ex.Message + "||||";
                                ListRptaItinerarioSF_Fail.Add(_rptaItinerarioSF);
                                /*Analizar si se deberia grabar en una tabla de bd para posteriormente darle seguimiento*/
                            }
                        }
                    }
                    else
                    {
                        error = responseDetItinerarioNM.StatusCode.ToString();
                        if(responseDetItinerarioNM != null && responseDetItinerarioNM.Content != null)
                        {
                            QuickLog(responseDetItinerarioNM.Content, "body_response.json", "DetalleItinerarioNM", previousClear: true); /// ♫ Trace
                        }
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }

                return Ok(true);
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
                    Response = SFResponse,
                    Rpta_NoUpdate_Fail = ListRptaItinerarioSF_Fail,                  
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
