using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.DestinosMundiales;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.Utils;
using System;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    /// <summary>
    /// Entidad exclusiva para Destinos Mundiales e Interagencias
    /// </summary>
    [RoutePrefix(RoutePrefix.Subcodigo)]
    public class SubcodigoController : BaseController<Subcodigo>
    {
        #region Constructor
        public SubcodigoController()
        {
            _crmCollection.Add(UnidadNegocioKeys.DestinosMundiales, new Subcodigo_DM_Repository());
            _crmCollection.Add(UnidadNegocioKeys.InterAgencias, new Subcodigo_IA_Repository());
        }
        #endregion

        #region PublicMethods
        [Route(RouteAction.Create)]
        public IHttpActionResult Create(Subcodigo entity)
        {
            object dm_error = null, dm_logResult = null, ia_error = null, ia_logResult = null;
            try
            {
                object dm_result = null, ia_result = null;
                _instants[InstantKey.Salesforce] = DateTime.Now;
                /*
                Task[] tasks = {
                    new Task(() => { /// ▼ Destinos Mundiales
                        try
                        {
                            _operCollection[UnidadNegocioKeys.DestinosMundiales] =  _crmCollection[UnidadNegocioKeys.DestinosMundiales].Create(entity);
                            LoadResults(UnidadNegocioKeys.DestinosMundiales, out dm_logResult, out dm_result);
                        }
                        catch (Exception ex)
                        {
                            dm_error = ex;
                        }
                    }),
                    new Task(() => { /// ▼ Interagencias
                        try
                        {
                            _operCollection[UnidadNegocioKeys.InterAgencias] =  _crmCollection[UnidadNegocioKeys.InterAgencias].Create(entity);
                            LoadResults(UnidadNegocioKeys.InterAgencias, out ia_logResult, out ia_result);
                        }
                        catch (Exception ex)
                        {
                            ia_error = ex;
                        }
                    })
                };
                foreach (var task in tasks) task.Start();
                Task.WaitAll(tasks);
                */

                _operCollection[UnidadNegocioKeys.DestinosMundiales] = _crmCollection[UnidadNegocioKeys.DestinosMundiales].Create(entity);
                LoadResults(UnidadNegocioKeys.DestinosMundiales, out dm_logResult, out dm_result);

                _operCollection[UnidadNegocioKeys.InterAgencias] = _crmCollection[UnidadNegocioKeys.InterAgencias].Create(entity);
                LoadResults(UnidadNegocioKeys.InterAgencias, out ia_logResult, out ia_result);



                _instants[InstantKey.Oracle] = DateTime.Now;
                return Ok(new
                {
                    DestinosMundiales = dm_result,
                    Interagencias = ia_result
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    LegacySystems = new { DestinosMundiales = dm_logResult, Intergencias = ia_logResult },
                    Instants = GetInstants(),
                    Error = new { DestinosMundiales = dm_error, Interagencias = ia_error },
                    Body = entity
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region Auxiliar
        private void LoadResults(UnidadNegocioKeys? unidadNegocio, out object logResult, out object result)
        {
            #region Log
            logResult = new
            {
                Result = new
                {
                    CodigoError = _operCollection[unidadNegocio][OutParameter.CodigoError].ToString(),
                    MensajeError = _operCollection[unidadNegocio][OutParameter.MensajeError].ToString(),
                    IdSubcodigo = _operCollection[unidadNegocio][OutParameter.IdSubcodigo].ToString()
                }
            };
            #endregion
            #region Client
            result = new
            {
                Result = new
                {
                    CodigoError = _operCollection[unidadNegocio][OutParameter.CodigoError].ToString(),
                    MensajeError = _operCollection[unidadNegocio][OutParameter.MensajeError].ToString(),
                    IdSubcodigo = _operCollection[unidadNegocio][OutParameter.IdSubcodigo].ToString()
                }
            };
            #endregion
        }
        #endregion

        #region NotImplemented
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            return unidadNegocioKey;
        }
        #endregion
    }
}
