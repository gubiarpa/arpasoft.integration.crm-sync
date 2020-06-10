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
    /// CRMEC003_2 : Registro de Detalle de Pasajeros
    [RoutePrefix(RoutePrefix.DetallePasajerosNM)]
    public class DetallePasajerosNMController : BaseController
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
            List<RptaPasajeroSF> ListRptaPasajeroSF_Fail = null;
            RptaPasajeroSF _rptaPasajeroSF = null;

            object SFResponse = null;
            string error = string.Empty;
            object objEnvio = null;

            try
            {
                var _unidadNegocio = RepositoryByBusiness(unidadNegocio.Descripcion.ToUnidadNegocio());                
                
                /// I. Consulta de Detalle Itinerario NM
                detallePasajerosNMs = (IEnumerable<DetallePasajerosNM>)(_detallePasajerosNMRepository.GetPasajeros())[OutParameter.CursorDetallePasajerosNM];
                if (detallePasajerosNMs == null || detallePasajerosNMs.ToList().Count.Equals(0)) return Ok(false);

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
                    objEnvio = new { listadatos = detallePasajerosNMSF };
                    QuickLog(objEnvio, "body_request.json", "DetallePasajerosNM", previousClear: true); /// ♫ Trace
                    
                    var responseDetallePasajeroNM = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.DetallePasajeroNMMethod, Method.POST, objEnvio, true, token);
                    if (responseDetallePasajeroNM.StatusCode.Equals(HttpStatusCode.OK))
                    {                        
                        dynamic jsonResponse = new JavaScriptSerializer().DeserializeObject(responseDetallePasajeroNM.Content);
                        QuickLog(jsonResponse, "body_response.json", "DetallePasajerosNM", previousClear: true); /// ♫ Trace

                        SFResponse = jsonResponse["respuestas"];
                        ListRptaPasajeroSF_Fail = new List<RptaPasajeroSF>();
                        foreach (var detallePasajeroNM in jsonResponse["respuestas"])
                        {
                            try
                            {
                                #region Deserialize
                                _rptaPasajeroSF = new RptaPasajeroSF();

                                _rptaPasajeroSF.CodigoError = "OK";
                                _rptaPasajeroSF.MensajeError = "TST";
                                _rptaPasajeroSF.idOportunidad_SF = "006R000000WAUr4IAH";
                                _rptaPasajeroSF.idPasajero_SF = "006R000000WAUr4IAC";
                                _rptaPasajeroSF.Identificador_NM = "VU-2";

                                _rptaPasajeroSF.CodigoError = detallePasajeroNM[OutParameter.SF_Codigo];
                                _rptaPasajeroSF.MensajeError = detallePasajeroNM[OutParameter.SF_Mensaje];
                                _rptaPasajeroSF.idOportunidad_SF = detallePasajeroNM[OutParameter.SF_IdOportunidad2];
                                _rptaPasajeroSF.idPasajero_SF = detallePasajeroNM[OutParameter.SF_IdPasajero];
                                _rptaPasajeroSF.Identificador_NM = detallePasajeroNM[OutParameter.SF_IdentificadorNM];
                                #endregion

                                #region ReturnToDB
                                var updOperation = _detallePasajerosNMRepository.Update(_rptaPasajeroSF);

                                if (Convert.IsDBNull(updOperation[OutParameter.IdActualizados]) == true || updOperation[OutParameter.IdActualizados].ToString().ToLower().Contains("null") || Convert.ToInt32(updOperation[OutParameter.IdActualizados].ToString()) <= 0)
                                {
                                    error = error + "Error en el Proceso de Actualizacion - No Actualizo Ningun Registro. Identificador NM : " + _rptaPasajeroSF.Identificador_NM.ToString() + "||||";
                                    ListRptaPasajeroSF_Fail.Add(_rptaPasajeroSF);
                                    /*Analizar si se deberia grabar en una tabla de bd para posteriormente darle seguimiento*/
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                error = error + "Error en el Proceso de Actualizacion - Response SalesForce : " + ex.Message + "||||";
                                ListRptaPasajeroSF_Fail.Add(_rptaPasajeroSF);
                                /*Analizar si se deberia grabar en una tabla de bd para posteriormente darle seguimiento*/
                            }
                        }
                    }
                    else
                    {
                        error = responseDetallePasajeroNM.StatusCode.ToString();
                        if (responseDetallePasajeroNM != null && responseDetallePasajeroNM.Content != null)
                        {
                            QuickLog(responseDetallePasajeroNM.Content, "body_response.json", "DetallePasajeroNM", previousClear: true); /// ♫ Trace
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
                    Rpta_NoUpdate_Fail = ListRptaPasajeroSF_Fail,                    
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
