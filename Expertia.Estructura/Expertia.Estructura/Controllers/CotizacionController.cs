using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.DestinosMundiales;
using Expertia.Estructura.Utils;
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
        [Route(RouteAction.Create)]
        public IHttpActionResult Create(Cotizacion entity)
        {
            return MakeAction(entity, GetUnidadNegocio(entity.UnidadNegocio.Descripcion), ActionMethod.Update);
        }
        #endregion

        #region Auxiliar
        private IHttpActionResult MakeAction(Cotizacion entity, UnidadNegocioKeys? unidadNegocio, ActionMethod actionMethod)
        {
            object error = null, logResult = null;
            try
            {
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                object result;

                _instants[InstantKey.Salesforce] = DateTime.Now;
                switch (actionMethod)
                {
                    case ActionMethod.Create:
                        _operCollection[unidadNegocio] = _crmCollection[unidadNegocio].Create(entity);
                        break;
                    default:
                        return NotFound();
                }
                LoadResults(unidadNegocio, out logResult, out result);
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

        private void LoadResults(UnidadNegocioKeys? unidadNegocio, out object logResult, out object result)
        {
            switch (unidadNegocio)
            {
                case UnidadNegocioKeys.CondorTravel:
                    #region Log
                    logResult = new
                    {
                        Codes = GetErrorResult(UnidadNegocioKeys.CondorTravel),
                        IdCuenta = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.IdCuenta].ToString(),
                        IdContacto = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.IdContacto].ToString()
                    };
                    #endregion
                    #region Client
                    result = new
                    {
                        Result = new
                        {
                            CondorTravel = new
                            {
                                CodigoError = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.CodigoError].ToString(),
                                MensajeError = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.MensajeError].ToString(),
                                IdCuenta = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.IdCuenta].ToString(),
                                IdContacto = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.IdContacto].ToString()
                            }
                        }
                    };
                    #endregion
                    break;
                case UnidadNegocioKeys.DestinosMundiales:
                case UnidadNegocioKeys.InterAgencias:
                    #region Log
                    logResult = new
                    {
                        Result = new
                        {
                            DestinosMundiales = new
                            {
                                Codes = GetErrorResult(UnidadNegocioKeys.DestinosMundiales),
                                IdCuenta = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCuenta].ToString(),
                                IdContacto = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdContacto].ToString()
                            },
                            InterAgencias = new
                            {
                                Codes = GetErrorResult(UnidadNegocioKeys.InterAgencias),
                                IdCuenta = _operCollection[UnidadNegocioKeys.InterAgencias][OutParameter.IdCuenta].ToString(),
                                IdContacto = _operCollection[UnidadNegocioKeys.InterAgencias][OutParameter.IdContacto].ToString()
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
                                IdContacto = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdContacto].ToString()
                            },
                            InterAgencias = new
                            {
                                CodigoError = _operCollection[UnidadNegocioKeys.InterAgencias][OutParameter.CodigoError].ToString(),
                                MensajeError = _operCollection[UnidadNegocioKeys.InterAgencias][OutParameter.MensajeError].ToString(),
                                IdCuenta = _operCollection[UnidadNegocioKeys.InterAgencias][OutParameter.IdCuenta].ToString(),
                                IdContacto = _operCollection[UnidadNegocioKeys.InterAgencias][OutParameter.IdContacto].ToString()
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
                case UnidadNegocioKeys.CondorTravel:
                    //_crmCollection.Add(UnidadNegocioKeys.CondorTravel, new Cotizacion_CT_Repository());
                    break;
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
