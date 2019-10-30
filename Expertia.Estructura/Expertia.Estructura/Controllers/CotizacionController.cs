using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.DestinosMundiales;
using Expertia.Estructura.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.Cotizacion)]
    public class CotizacionController : BaseController<Cotizacion>
    {
        #region Properties
        #endregion

        #region Constructor
        public CotizacionController()
        {
        }
        #endregion

        #region PublicMethods
        [Route(RouteAction.Generate)]
        public IHttpActionResult Generate(Cotizacion entity)
        {
            object error = null, logResult = null;
            try
            {
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                object result;
                _instants[InstantKey.Salesforce] = DateTime.Now;
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.DestinosMundiales:
                        _operCollection[UnidadNegocioKeys.DestinosMundiales] = _crmCollection[UnidadNegocioKeys.DestinosMundiales].Generate(entity);
                        LoadResults(UnidadNegocioKeys.DestinosMundiales, out logResult, out result);
                        break;
                    default:
                        return NotFound();
                }
                _instants[InstantKey.Oracle] = DateTime.Now;
                return Ok(result);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    BusinessUnity = entity.UnidadNegocio.Descripcion,
                    LegacySystems = logResult,
                    Instants = GetInstants(),
                    Error = error,
                    Body = entity
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }

        [Route(RouteAction.Asociate)]
        public IHttpActionResult Asociate(Cotizacion entity)
        {
            object error = null, logResult = null;
            try
            {
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                object result;
                _instants[InstantKey.Salesforce] = DateTime.Now;
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.DestinosMundiales:
                        _operCollection[UnidadNegocioKeys.DestinosMundiales] = _crmCollection[UnidadNegocioKeys.DestinosMundiales].Asociate(entity);
                        LoadResults(UnidadNegocioKeys.DestinosMundiales, out logResult, out result);
                        break;
                    default:
                        return NotFound();
                }
                _instants[InstantKey.Oracle] = DateTime.Now;
                return Ok(result);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    BusinessUnity = entity.UnidadNegocio.Descripcion,
                    LegacySystems = logResult,
                    Instants = GetInstants(),
                    Error = error,
                    Body = entity
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }

        [Route(RouteAction.Send)]
<<<<<<< Updated upstream
        public IHttpActionResult Send(object obj)
        {
            try
            {
                // var oper = new Cotizacion_DM_Repository(UnidadNegocioKeys.DestinosMundiales).GetAllModified();
                obj.TryWriteLogObject(_logFileManager, _clientFeatures);
                return Ok();
=======
        public IHttpActionResult Send()
        {
            try
            {
                Guid guid = Guid.NewGuid();
                System.IO.File.AppendAllText(
                    "C:\\IIS\\log\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " :: (1) " + guid.ToString()  + "\n");
                System.Threading.Thread.Sleep(5000);
                System.IO.File.AppendAllText(
                    "C:\\IIS\\log\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " :: (2) " + guid.ToString()  + "\n");
                return Ok(guid);
>>>>>>> Stashed changes
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Auxiliar
        private void LoadResults(UnidadNegocioKeys? unidadNegocio, out object logResult, out object result)
        {
            switch (unidadNegocio)
            {
                case UnidadNegocioKeys.DestinosMundiales:
                    #region Log
                    logResult = new
                    {
                        Result = new
                        {
                            DestinosMundiales = new
                            {
                                Codes = GetErrorResult(UnidadNegocioKeys.DestinosMundiales),
                                IdCuenta = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCuenta].ToString(),
                                IdCotizacion = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCotizacion].ToString()
                            }
                        }
                    };
                    #endregion
                    #region Client
                    result = new
                    {
                        Result = new
                        {
                            DestinosMundiales = new
                            {
                                CodigoError = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.CodigoError].ToString(),
                                MensajeError = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.MensajeError].ToString(),
                                IdCuenta = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCuenta].ToString(),
                                IdCotizacion = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCotizacion].ToString()
                            }
                        }
                    };
                    #endregion
                    break;
                default:
                    logResult = null; result = null;
                    break;
            }
        }

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.DestinosMundiales:
                    _crmCollection.Add(UnidadNegocioKeys.DestinosMundiales, new Cotizacion_DM_Repository());
                    break;
                default:
                    break;
            }
            return unidadNegocioKey; // Devuelve el mismo parámetro
        }
        #endregion
    }
}
