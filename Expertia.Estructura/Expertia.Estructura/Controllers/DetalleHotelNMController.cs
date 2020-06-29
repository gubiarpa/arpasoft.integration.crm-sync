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
    /// CRMEC003_3 : Registro de Detalle del Hotel
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
            List<RptaHotelSF> ListRptaHotelSF_Fail = null;
            RptaHotelSF _rptaHotelSF = null;

            object SFResponse = null;
            string error = string.Empty;
            object objEnvio = null;

            try
            {
                var _unidadNegocio = RepositoryByBusiness(unidadNegocio.Descripcion.ToUnidadNegocio());                                

                /// I. Consulta de Detalle Itinerario NM
                detalleHotelNMs = (IEnumerable<DetalleHotelNM>)(_detalleHotelNMRepository.GetDetalleHoteles())[OutParameter.CursorDetalleHotelNM];
                if (detalleHotelNMs == null || detalleHotelNMs.ToList().Count.Equals(0)) return Ok(false);

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
                    objEnvio = new { listadatos = detalleHotelNMSF };
                    QuickLog(objEnvio, "body_request.json", "DetalleHotelNM", previousClear: true); /// ♫ Trace
                    
                    var responseDetalleHotelNM = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.DetalleHotelNMMethod, Method.POST, objEnvio, true, token);
                    if (responseDetalleHotelNM.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        dynamic jsonResponse = (new JavaScriptSerializer()).DeserializeObject(responseDetalleHotelNM.Content);
                        QuickLog(jsonResponse, "body_response.json", "DetalleHotelNM", previousClear: true); /// ♫ Trace

                        SFResponse = jsonResponse["respuestas"];
                        ListRptaHotelSF_Fail = new List<RptaHotelSF>();
                        foreach (var detalleHotelNM in jsonResponse["respuestas"])
                        {
                            try
                            {
                                #region Deserialize
                                _rptaHotelSF = new RptaHotelSF();

                                _rptaHotelSF.CodigoError = "OK";
                                _rptaHotelSF.MensajeError = "TST";
                                _rptaHotelSF.idOportunidad_SF = "006R000000WAUr4IAH";
                                _rptaHotelSF.idDetalleHotel_SF = "006R000000WAUr4IAC";
                                _rptaHotelSF.Identificador_NM = "1";

                                _rptaHotelSF.CodigoError = detalleHotelNM[OutParameter.SF_Codigo];
                                _rptaHotelSF.MensajeError = detalleHotelNM[OutParameter.SF_Mensaje];
                                _rptaHotelSF.idOportunidad_SF = detalleHotelNM[OutParameter.SF_IdOportunidad2];
                                _rptaHotelSF.idDetalleHotel_SF = detalleHotelNM[OutParameter.SF_IdDetalleHotel];
                                _rptaHotelSF.Identificador_NM = detalleHotelNM[OutParameter.SF_IdentificadorNM];
                                #endregion

                                #region ReturnToDB
                                var updOperation = _detalleHotelNMRepository.Update(_rptaHotelSF);

                                if (Convert.IsDBNull(updOperation[OutParameter.IdActualizados]) == true || updOperation[OutParameter.IdActualizados].ToString().ToLower().Contains("null") || Convert.ToInt32(updOperation[OutParameter.IdActualizados].ToString()) <= 0)
                                {
                                    error = error + "Error en el Proceso de Actualizacion - No Actualizo Ningun Registro. Identificador NM : " + _rptaHotelSF.Identificador_NM.ToString() + "||||";
                                    ListRptaHotelSF_Fail.Add(_rptaHotelSF);
                                    /*Analizar si se deberia grabar en una tabla de bd para posteriormente darle seguimiento*/
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                error = error + "Error en el Proceso de Actualizacion - Response SalesForce : " + ex.Message + "||||";
                                ListRptaHotelSF_Fail.Add(_rptaHotelSF);
                                /*Analizar si se deberia grabar en una tabla de bd para posteriormente darle seguimiento*/
                            }
                        }
                    }
                    else
                    {
                        error = responseDetalleHotelNM.StatusCode.ToString();
                        if (responseDetalleHotelNM != null && responseDetalleHotelNM.Content != null)
                        {
                            QuickLog(responseDetalleHotelNM.Content, "body_response.json", "DetalleHotelNM", previousClear: true); /// ♫ Trace
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
                    Rpta_NoUpdate_Fail = ListRptaHotelSF_Fail,
                    Exception = error
                    //LegacySystems = detalleHotelNMs
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
