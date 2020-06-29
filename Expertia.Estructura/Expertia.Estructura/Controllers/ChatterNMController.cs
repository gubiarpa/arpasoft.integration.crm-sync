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
    /// CRMEC004 : Chatter
    [RoutePrefix(RoutePrefix.ChatterNM)]
    public class ChatterNMController : BaseController<object>
    {
        #region Properties
        private IChatterNMRepository _chatterNMRepository;
        protected override ControllerName _controllerName => ControllerName.ChatterNM;
        #endregion

        #region PublicMethods
        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<ChatterNM> chatterNMs = null;
            List<RptaChatterSF> ListRptaChatterSF_Fail = null;
            RptaChatterSF _rptaChatterSF = null;

            object SFResponse = null;
            string error = string.Empty;
            object objEnvio = null;

            try
            {
                var _unidadNegocio = RepositoryByBusiness(unidadNegocio.Descripcion.ToUnidadNegocio());

                /// I. Consulta de Informacion Pago NM
                chatterNMs = (IEnumerable<ChatterNM>)(_chatterNMRepository.GetPostCotizaciones())[OutParameter.CursorChatterNM];
                if (chatterNMs == null || chatterNMs.ToList().Count.Equals(0)) return Ok(false);

                /// Obtiene Token para envío a Salesforce
                var authSf = RestBase.GetToken();
                var token = authSf[OutParameter.SF_Token].ToString();
                var crmServer = authSf[OutParameter.SF_UrlAuth].ToString();

                /// preparación del canal de comunicacion para envio a Salesforce
                var chatterNMSF = new List<object>();
                foreach (var chatter in chatterNMs)
                {
                    chatterNMSF.Add(chatter.ToSalesforceEntity());
                }
                
                try
                {
                    /// Envío de Informacion de Canal de Comunicacion a Salesforce                    
                    objEnvio = new { listadatos = chatterNMSF }; /**POR DEFINIR**/
                    QuickLog(objEnvio, "body_request.json", "ChatterNM", previousClear: true); /// ♫ Trace
                    
                    var responseChatterNM = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.ChatterNMMethod, Method.POST, objEnvio, true, token);
                    if (responseChatterNM.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        dynamic jsonResponse = (new JavaScriptSerializer()).DeserializeObject(responseChatterNM.Content);
                        QuickLog(jsonResponse, "body_response.json", "ChatterNM", previousClear: true); /// ♫ Trace

                        SFResponse = jsonResponse["respuestas"];
                        ListRptaChatterSF_Fail = new List<RptaChatterSF>();
                        foreach (var chatterNM in jsonResponse["respuestas"])
                        {
                            try
                            {
                                #region Deserialize
                                _rptaChatterSF = new RptaChatterSF();

                                _rptaChatterSF.CodigoError = "OK";
                                _rptaChatterSF.MensajeError = "TST";
                                _rptaChatterSF.idOportunidad_SF = "006R000000WAUr4IAH";
                                _rptaChatterSF.IdRegPostCotSrv_SF = "006R000000WAUr4IAC";
                                _rptaChatterSF.Identificador_NM = "16";

                                _rptaChatterSF.CodigoError = chatterNM[OutParameter.SF_Codigo];
                                _rptaChatterSF.MensajeError = chatterNM[OutParameter.SF_Mensaje];
                                _rptaChatterSF.idOportunidad_SF = chatterNM[OutParameter.SF_IdOportunidad2];
                                _rptaChatterSF.IdRegPostCotSrv_SF = chatterNM[OutParameter.SF_IdRegPostCotSrv];
                                _rptaChatterSF.Identificador_NM = chatterNM[OutParameter.SF_IdentificadorNM];
                                #endregion

                                #region ReturnToDB
                                var updOperation = _chatterNMRepository.Update(_rptaChatterSF);

                                if (Convert.IsDBNull(updOperation[OutParameter.IdActualizados]) == true || updOperation[OutParameter.IdActualizados].ToString().ToLower().Contains("null") || Convert.ToInt32(updOperation[OutParameter.IdActualizados].ToString()) <= 0)
                                {
                                    error = error + "Error en el Proceso de Actualizacion - No Actualizo Ningun Registro. Identificador NM : " + _rptaChatterSF.Identificador_NM.ToString() + "||||";
                                    ListRptaChatterSF_Fail.Add(_rptaChatterSF);
                                    /*Analizar si se deberia grabar en una tabla de bd para posteriormente darle seguimiento*/
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                error = error + "Error en el Proceso de Actualizacion - Response SalesForce : " + ex.Message + "||||";
                                ListRptaChatterSF_Fail.Add(_rptaChatterSF);
                                /*Analizar si se deberia grabar en una tabla de bd para posteriormente darle seguimiento*/
                            }
                        }
                    }
                    else
                    {
                        error = responseChatterNM.StatusCode.ToString();
                        if (responseChatterNM != null && responseChatterNM.Content != null)
                        {
                            QuickLog(responseChatterNM.Content, "body_response.json", "ChatterNM", previousClear: true); /// ♫ Trace
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
                    Request = objEnvio,
                    Response = SFResponse,
                    Rpta_NoUpdate_Fail = ListRptaChatterSF_Fail,                                        
                    Exception = error
                    //LegacySystems = chatterNMs
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);
            _chatterNMRepository = new ChatterNMRepository(unidadNegocioKey);
            return unidadNegocioKey;
        }
    }
}
