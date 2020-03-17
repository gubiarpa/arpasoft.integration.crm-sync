using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.RestManager.Base;
using Expertia.Estructura.RestManager.RestParse;
using Expertia.Estructura.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.Subcodigo)]
    public class SubcodigoController : BaseController<object>
    {
        #region Properties
        private IDictionary<UnidadNegocioKeys?, ISubcodigoRepository> _subcodigoCollection;
        protected override ControllerName _controllerName => ControllerName.Subcodigo;
        #endregion

        #region Constructor
        public SubcodigoController()
        {
            _subcodigoCollection = new Dictionary<UnidadNegocioKeys?, ISubcodigoRepository>();
        }

        #endregion

        #region PublicMethods
        [Route(RouteAction.Create)]
        public IHttpActionResult Create(Subcodigo subcodigo)
        {
            object response = null;
            var errorDetail = string.Empty;
            try
            {
                RepositoryByBusiness(UnidadNegocioKeys.DestinosMundiales);

                /// Variables
                Operation operResult_DM = null, operResult_IA = null;
                //int idSubcodigo_DM = 0, idSubcodigo_IA = 0;
                var tasks = new List<Task>();

                /// Tareas
                tasks.Add(new Task(() =>
                {
                    operResult_DM = _subcodigoCollection[UnidadNegocioKeys.DestinosMundiales].Create(subcodigo);
                }));
                tasks.Add(new Task(() =>
                {
                    operResult_IA = _subcodigoCollection[UnidadNegocioKeys.Interagencias].Create(subcodigo);
                }));
                tasks.ForEach(t => t.Start());
                Task.WaitAll(tasks.ToArray());

                response = new
                {
                    Response = new
                    {
                        DestinosMundiales = new
                        {
                            CodigoError = operResult_DM[OutParameter.CodigoError].ToString(),
                            MensajeError = operResult_DM[OutParameter.MensajeError].ToString(),
                            IdSubcodigo = int.TryParse(operResult_DM[OutParameter.IdSubcodigo].ToString(), out int idSubcodigo_DM) ? idSubcodigo_DM : -1
                        },
                        Interagencias = new
                        {
                            CodigoError = operResult_IA[OutParameter.CodigoError].ToString(),
                            MensajeError = operResult_IA[OutParameter.MensajeError].ToString(),
                            IdSubcodigo = int.TryParse(operResult_IA[OutParameter.IdSubcodigo].ToString(), out int idSubcodigo_IA) ? idSubcodigo_IA : -1
                        }
                    }
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                errorDetail = ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    Request = subcodigo,
                    Response = response,
                    Exception = errorDetail
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }

        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<Subcodigo> subcodigos = null;
            var errorDetail = string.Empty;
            try
            {
                RepositoryByBusiness(unidadNegocio.Descripcion.ToUnidadNegocio());

                /// Consulta de Subcodigos a PTA
                subcodigos = (IEnumerable<Subcodigo>)(_subcodigoCollection[unidadNegocio.Descripcion.ToUnidadNegocio()].Read())[OutParameter.CursorSubcodigo];
                if (subcodigos == null || subcodigos.ToList().Count.Equals(0)) return Ok();

                /// Obtiene Token para envío a Salesforce
                var authSf = RestBase.GetToken();
                var token = authSf[OutParameter.SF_Token].ToString();
                var crmServer = authSf[OutParameter.SF_UrlAuth].ToString();

                foreach (var subcodigo in subcodigos)
                {
                    try
                    {
                        /// Envío de subcodigo a Salesforce
                        var responseSubcodigo = RestBase.ExecuteByKeyWithServer(crmServer, SalesforceKeys.SubcodigoMethod, Method.POST, subcodigo.ToSalesforceEntity(), true, token);
                        if (responseSubcodigo.StatusCode.Equals(HttpStatusCode.OK))
                        {
                            dynamic jsonResponse = new JavaScriptSerializer().DeserializeObject(responseSubcodigo.Content);
                            subcodigo.CodigoError = jsonResponse[OutParameter.SF_CodigoError];
                            subcodigo.MensajeError = jsonResponse[OutParameter.SF_MensajeError];

                            /// Actualización de estado de subcodigo a PTA
                            var updateResponse = _subcodigoCollection[unidadNegocio.Descripcion.ToUnidadNegocio()].Update(subcodigo);
                            subcodigo.Actualizados_DM = int.Parse(updateResponse[OutParameter.IdActualizados].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        subcodigo.CodigoError = ApiResponseCode.ErrorCode;
                        subcodigo.MensajeError = ex.Message;
                    }
                }

                return Ok(subcodigos);
            }
            catch (Exception ex)
            {
                subcodigos = null;
                errorDetail = ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    UnidadNegocio = unidadNegocio.Descripcion,
                    Exception = errorDetail,
                    LegacySystems = subcodigos
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region Auxiliar
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.DestinosMundiales:
                case UnidadNegocioKeys.Interagencias:
                    _subcodigoCollection[UnidadNegocioKeys.DestinosMundiales] = new Subcodigo_IA_Repository(UnidadNegocioKeys.DestinosMundiales);
                    _subcodigoCollection[UnidadNegocioKeys.Interagencias] = new Subcodigo_IA_Repository(UnidadNegocioKeys.Interagencias);
                    break;
            }
            return unidadNegocioKey;
        }
        #endregion
    }
}
