using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.AppWebs;
using Expertia.Estructura.Repository.General;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.RestManager.Base;
using Expertia.Estructura.RestManager.RestParse;
using Expertia.Estructura.Utils;
using System;
using System.Web.Http;
using System.Collections.Generic;
using Expertia.Estructura.ws_compra;
using System.Linq;
using RestSharp;
using System.Net;
using System.Web.Script.Serialization;
using Expertia.Estructura.Models.Retail;
using Expertia.Estructura.Repository.Retail;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.FileSRVRetail)]
    public class FileSRVRetailController : BaseController<object>
    {
        #region Properties       
        private IFileSRVRetailRepository _fileSrvRetailRepository;
        #endregion

        #region PublicMethods
       
        [Route(RouteAction.Send)]
        public IHttpActionResult Send()
        {
            IEnumerable<FilesAsociadosSRV> ListFilesAsociadosSRV = null;
            List<FilesAsociadosSRVResponse> ListFileAsociadosSRVResponse = new List<FilesAsociadosSRVResponse>();
            List<Respuesta> ListRpta = new List<Respuesta>();
            string errorEnvio = string.Empty;
            

            try 
            {                
                RepositoryByBusiness(null);

                /*Consulta de files asociadiados a BD*/
                ListFilesAsociadosSRV = (IEnumerable<FilesAsociadosSRV>)_fileSrvRetailRepository.GetFilesAsociadosSRV()[OutParameter.CursorFilesAsociadosSRV];
                if (ListFilesAsociadosSRV == null || ListFilesAsociadosSRV.ToList().Count.Equals(0)) return Ok(false);

                /*Obtiene Token de envío a Salesforce*/
                var authSf = RestBase.GetToken();
                var token = authSf[OutParameter.SF_Token].ToString();
                var crmServer = authSf[OutParameter.SF_UrlAuth].ToString();

                ///*Por cada file...*/
                //var FilesAsociadosSF = new List<object>();
                //foreach (var fileAsociado in ListFilesAsociadosSRV)
                //    FilesAsociadosSF.Add(fileAsociado.ToSalesforceEntity());

                //try
                //{
                //    var objEnvio = new { info = FilesAsociadosSF };
                //    /*Envío de files asociados a Salesforce*/
                //    var responseFilesAsociados = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.PedidosProcesadosMethod, Method.POST, objEnvio, true, token);
                //    if (responseFilesAsociados.StatusCode.Equals(HttpStatusCode.OK))
                //    {
                //        dynamic jsonResponse = new JavaScriptSerializer().DeserializeObject(responseFilesAsociados.Content);
                //        foreach (var fileAsociado in ListFilesAsociadosSRV)
                //        {
                //            foreach (var jsResponse in jsonResponse["Solicitudes"])
                //            {
                //                if (fileAsociado.file_id == jsResponse[OutParameter.SF_IdSolicitudPago])
                //                {
                //                    /*Validamos Y Actualizamos en BD*/

                //                }                                
                //            }
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    errorEnvio = errorEnvio + ApiResponseCode.ErrorCode + " - " + ex.Message + " ||.";                        
                //}



                foreach (var fileAsociado in ListFilesAsociadosSRV)
                {
                    try
                    {
                        var responseFilesAsociados = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.OportunidadAsocMethod, Method.POST, fileAsociado.ToSalesforceEntity(), true, token);
                        
                        if (responseFilesAsociados.StatusCode.Equals(HttpStatusCode.OK))
                        {
                            dynamic jsonResponse = new JavaScriptSerializer().DeserializeObject(responseFilesAsociados.Content);
                            FilesAsociadosSRVResponse FileAsociadosSRVResponse = new FilesAsociadosSRVResponse();
                            FileAsociadosSRVResponse.id_oportunidad_sf = jsonResponse[OutParameter.SF_IdOportunidad];
                            FileAsociadosSRVResponse.codigo_error = jsonResponse[OutParameter.SF_CodigoError];
                            FileAsociadosSRVResponse.mensaje_error = jsonResponse[OutParameter.SF_MensajeError];
                            ListFileAsociadosSRVResponse.Add(FileAsociadosSRVResponse);
                            /// Actualización de estado File Oportundad
                            var operation = _fileSrvRetailRepository.Actualizar_EnvioCotRetail(FileAsociadosSRVResponse);
                            Respuesta Rpta = new Respuesta();
                            Rpta.CodigoError = operation[OutParameter.CodigoError].ToString();
                            Rpta.MensajeError = operation[OutParameter.MensajeError].ToString();
                            Rpta.Numero_Afectados = operation[OutParameter.NumeroActualizados].ToString();
                            ListRpta.Add(Rpta);
                        }
                    }
                    catch (Exception ex)
                    {
                        errorEnvio = ex.Message;
                    }
                   
                }

                return Ok(ListFileAsociadosSRVResponse);
            }            
            catch (Exception ex)
            {
                errorEnvio = errorEnvio + " / " + ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    Request = ListFilesAsociadosSRV,                    
                    Response = ListFileAsociadosSRVResponse,
                    Rpta = ListRpta,
                    Exception = errorEnvio
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }

        
        #endregion

        #region Auxiliar
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);

            _fileSrvRetailRepository = new FileSRVRetailRepository(unidadNegocioKey);            
            return unidadNegocioKey;
        }
        #endregion

    }
}
