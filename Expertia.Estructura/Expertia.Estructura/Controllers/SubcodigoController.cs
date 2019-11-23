using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Behavior;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.RestManager.Base;
using Expertia.Estructura.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    /// <summary>
    /// Entidad exclusiva para Destinos Mundiales e Interagencias
    /// </summary>
    [RoutePrefix(RoutePrefix.Subcodigo)]
    public class SubcodigoController : BaseController<object>
    {
        #region Properties
        private ISubcodigoRepository _subcodigoRepository;
        private ICrmApiResponse _response;
        #endregion

        #region Constructor
        public SubcodigoController()
        {
        }
        #endregion

        #region PublicMethods
        [Route(RouteAction.Create)]
        public IHttpActionResult Create(Subcodigo subcodigo)
        {
            object result = null;
            try
            {
                var unidadNegocio = RepositoryByBusiness(subcodigo.UnidadNegocio.ToUnidadNegocio());
                _instants[InstantKey.Salesforce] = DateTime.Now;

                var operResult = _subcodigoRepository.Create(subcodigo);
                _response = new CrmApiResponse(operResult[OutParameter.CodigoError].ToString(), operResult[OutParameter.MensajeError].ToString());
                result = new
                {
                    Result = _response,
                    Response = new
                    {
                        IdSubcodigo = operResult[OutParameter.IdSubcodigo].ToString()
                    }
                };

                _instants[InstantKey.Oracle] = DateTime.Now;
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    Instants = GetInstants(),
                    Result = result,
                    Body = subcodigo
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }

        [Route(RouteAction.Send)]
        public IHttpActionResult Send(UnidadNegocio unidadNegocio)
        {
            IEnumerable<Subcodigo> subcodigos = null;
            try
            {
                var unidadNegocioType = RepositoryByBusiness(unidadNegocio.Descripcion.ToUnidadNegocio());

                /// Consulta de Subcodigos a PTA
                subcodigos = (IEnumerable<Subcodigo>)(_subcodigoRepository.Read())[OutParameter.CursorSubcodigo];

                if (subcodigos == null || subcodigos.ToList().Count.Equals(0)) return Ok();

                /// Obtiene Token para envío a Salesforce
                var token = RestBase.GetTokenByKey(SalesforceKeys.AuthServer, SalesforceKeys.AuthMethod);

                var subcodigoTasks = new List<Task>();
                foreach (var subcodigo in subcodigos)
                {
                    var task = new Task(() =>
                    {
                        try
                        {
                            /// Envío de subcodigo a Salesforce
                            var subcodigoSf = ToSalesforceEntity(subcodigo);
                            var response = RestBase.ExecuteByKey(SalesforceKeys.CrmServer, SalesforceKeys.SubcodigoMethod, Method.POST, subcodigoSf, true, token);
                            if (response.StatusCode.Equals(HttpStatusCode.OK))
                            {
                                JsonManager.LoadText(response.Content);
                                subcodigo.CodigoError = JsonManager.GetSetting(OutParameter.SF_CodigoError);
                                subcodigo.MensajeError = JsonManager.GetSetting(OutParameter.SF_MensajeError);
                            }

                            /// Actualización de estado de subcodigo a PTA
                            if (!string.IsNullOrEmpty(subcodigo.CodigoError)) _subcodigoRepository.Update(subcodigo);
                        }
                        catch
                        {
                        }
                    });
                    task.Start();
                    subcodigoTasks.Add(task);
                }

                Task.WaitAll(subcodigoTasks.ToArray());
                return Ok(subcodigos);
            }
            catch (Exception ex)
            {
                subcodigos = null;
                throw ex;
            }
            finally
            {
                (new
                {
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
                case UnidadNegocioKeys.CondorTravel:
                    break;
                case UnidadNegocioKeys.Interagencias:
                    _subcodigoRepository = new Subcodigo_IA_Repository();
                    break;
                default:
                    break;
            }
            return unidadNegocioKey;
        }

        private object ToSalesforceEntity(Subcodigo subcodigo)
        {
            try
            {
                return new
                {
                    info = new
                    {
                        UnidadNegocio = subcodigo.UnidadNegocio,
                        Accion = subcodigo.Accion,
                        DkAgencia = subcodigo.DkAgencia.ToString(),
                        CorrelativoSubcodigo = subcodigo.CorrelativoSubcodigo.ToString(),
                        DireccionSucursal = subcodigo.DireccionSucursal,
                        EstadoSucursal = subcodigo.EstadoSucursal,
                        NombreSucursal = subcodigo.NombreSucursal,
                        Promotor = subcodigo.Promotor,
                        CondicionPago = subcodigo.CondicionPago
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
