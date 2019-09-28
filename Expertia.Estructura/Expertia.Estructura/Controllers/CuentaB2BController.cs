using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Condor;
using Expertia.Estructura.Repository.DestinosMundiales;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.Utils;
using System;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    /// <summary>
    /// Mantenimiento para Contactos B2B
    /// </summary>
    [RoutePrefix(RoutePrefix.CuentaB2B)]
    public class CuentaB2BController : BaseController<CuentaB2B>
    {
        [Route(RouteAction.Create)]
        public override IHttpActionResult Create(CuentaB2B entity)
        {
            object result = null, error = null;
            try
            {
                #region UnidadNegocio
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:
                        #region Try-CondorTravel
                        _operCollection[UnidadNegocioKeys.CondorTravel] = _crmCollection[UnidadNegocioKeys.CondorTravel].Create(entity);
                        _codigoError = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.CodigoError].ToString();
                        #endregion

                        #region Response
                        result = new
                        {
                            Result = new
                            {
                                CondorTravel = new
                                {
                                    CodigoError = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.CodigoError].ToString(),
                                    MensajeError = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.MensajeError].ToString(),
                                    IdCuenta = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.IdCuenta].ToString()
                                }
                            }
                        };
                        #endregion
                        break;
                    case UnidadNegocioKeys.DestinosMundiales:
                    case UnidadNegocioKeys.InterAgencias:
                        _operCollection[UnidadNegocioKeys.DestinosMundiales] = _crmCollection[UnidadNegocioKeys.DestinosMundiales].Create(entity);
                        _operCollection[UnidadNegocioKeys.InterAgencias] = _crmCollection[UnidadNegocioKeys.InterAgencias].Create(entity);
                        result = new
                        {
                            Result = new
                            {
                                DestinosMundiales = new
                                {
                                    CodigoError = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.CodigoError].ToString(),
                                    MensajeError = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.MensajeError].ToString(),
                                    IdCuenta = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCuenta].ToString()
                                },
                                InterAgencias = new
                                {
                                    CodigoError = _operCollection[UnidadNegocioKeys.InterAgencias][OutParameter.CodigoError].ToString(),
                                    MensajeError = _operCollection[UnidadNegocioKeys.InterAgencias][OutParameter.MensajeError].ToString(),
                                    IdCuenta = _operCollection[UnidadNegocioKeys.InterAgencias][OutParameter.IdCuenta].ToString()
                                }
                            }
                        };
                        break;
                    default:
                        return NotFound();
                }
                return Ok(result);
                #endregion
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
                    LegacySystems = result,
                    Error = error,
                    Body = entity
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }

        [Route(RouteAction.Update)]
        public override IHttpActionResult Update(CuentaB2B entity)
        {
            object result = null, error = null;
            try
            {
                #region UnidadNegocio
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:
                        _operCollection[UnidadNegocioKeys.CondorTravel] = _crmCollection[UnidadNegocioKeys.CondorTravel].Update(entity);
                        result = new
                        {
                            Result = new
                            {
                                CondorTravel = new
                                {
                                    CodigoError = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.CodigoError].ToString(),
                                    MensajeError = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.MensajeError].ToString(),
                                    IdCuenta = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.IdCuenta].ToString()
                                }
                            }
                        };
                        break;
                    case UnidadNegocioKeys.NuevoMundo:
                        return NotFound();
                    case UnidadNegocioKeys.DestinosMundiales:
                    case UnidadNegocioKeys.InterAgencias:
                        _operCollection[UnidadNegocioKeys.DestinosMundiales] = _crmCollection[UnidadNegocioKeys.DestinosMundiales].Update(entity);
                        _operCollection[UnidadNegocioKeys.InterAgencias] = _crmCollection[UnidadNegocioKeys.InterAgencias].Update(entity);
                        result = new
                        {
                            Result = new
                            {
                                DestinosMundiales = new
                                {
                                    CodigoError = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.CodigoError].ToString(),
                                    MensajeError = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.MensajeError].ToString(),
                                    IdCuenta = _operCollection[UnidadNegocioKeys.DestinosMundiales][OutParameter.IdCuenta].ToString()
                                },
                                InterAgencias = new
                                {
                                    CodigoError = _operCollection[UnidadNegocioKeys.InterAgencias][OutParameter.CodigoError].ToString(),
                                    MensajeError = _operCollection[UnidadNegocioKeys.InterAgencias][OutParameter.MensajeError].ToString(),
                                    IdCuenta = _operCollection[UnidadNegocioKeys.InterAgencias][OutParameter.IdCuenta].ToString()
                                }
                            }
                        };
                        break;
                    default:
                        return NotFound();
                }
                return Ok(result);
                #endregion
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
                    LegacySystems = result,
                    Error = error,
                    Body = entity
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.CondorTravel:
                    _crmCollection.Add(UnidadNegocioKeys.CondorTravel, new CuentaB2B_CT_Repository());
                    break;
                case UnidadNegocioKeys.DestinosMundiales:
                case UnidadNegocioKeys.InterAgencias:
                    _crmCollection.Add(UnidadNegocioKeys.DestinosMundiales, new CuentaB2B_DM_Repository());
                    _crmCollection.Add(UnidadNegocioKeys.InterAgencias, new CuentaB2B_IA_Repository());
                    break;
                default:
                    break;
            }
            return unidadNegocioKey;
        }
    }
}
