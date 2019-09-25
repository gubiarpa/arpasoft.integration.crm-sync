using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Condor;
using Expertia.Estructura.Repository.DestinosMundiales;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.Repository.NuevoMundo;
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
        public CuentaB2BController() : base()
        {
            #region BuildRepository
            _crmCollection.Add(UnidadNegocioKeys.CondorTravel, new CuentaB2B_CT_Repository());
            _crmCollection.Add(UnidadNegocioKeys.DestinosMundiales, new CuentaB2B_DM_Repository());
            _crmCollection.Add(UnidadNegocioKeys.InterAgencias, new CuentaB2B_IA_Repository());
            #endregion
        }

        [Route(RouteAction.Create)]
        public override IHttpActionResult Create(CuentaB2B entity)
        {
            try
            {
                #region UnidadNegocio
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:
                        _operCollection[UnidadNegocioKeys.CondorTravel] = _crmCollection[UnidadNegocioKeys.CondorTravel].Create(entity);
                        return Ok(new
                        {
                            Result = new
                            {
                                CondorTravel = new
                                {
                                    CodigoError = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.CodigoError].ToString(),
                                    MensajeError = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.MensajeError].ToString()
                                }
                            }
                        });
                    case UnidadNegocioKeys.NuevoMundo:
                        return NotFound();
                    case UnidadNegocioKeys.DestinosMundiales:
                    case UnidadNegocioKeys.InterAgencias:
                        _operCollection[UnidadNegocioKeys.DestinosMundiales] = _crmCollection[UnidadNegocioKeys.DestinosMundiales].Create(entity);
                        _operCollection[UnidadNegocioKeys.InterAgencias] = _crmCollection[UnidadNegocioKeys.InterAgencias].Create(entity);
                        return Ok(new
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
                        });
                    default:
                        return NotFound();
                }
                #endregion
            }
            catch (Exception ex)
            {
                ex.TryWriteLogObject(_logFileManager, _clientFeatures, LogType.Fail);
                return InternalServerError(ex);
            }
            finally
            {
                entity.TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }

        [Route(RouteAction.Update)]
        public override IHttpActionResult Update(CuentaB2B entity)
        {
            try
            {
                #region UnidadNegocio
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:
                        _operCollection[UnidadNegocioKeys.CondorTravel] = _crmCollection[UnidadNegocioKeys.CondorTravel].Update(entity);
                        return Ok(new
                        {
                            Result = new
                            {
                                CondorTravel = new
                                {
                                    CodigoError = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.CodigoError].ToString(),
                                    MensajeError = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.MensajeError].ToString()
                                }
                            }
                        });
                    case UnidadNegocioKeys.NuevoMundo:
                        return NotFound();
                    case UnidadNegocioKeys.DestinosMundiales:
                    case UnidadNegocioKeys.InterAgencias:
                        _operCollection[UnidadNegocioKeys.DestinosMundiales] = _crmCollection[UnidadNegocioKeys.DestinosMundiales].Update(entity);
                        _operCollection[UnidadNegocioKeys.InterAgencias] = _crmCollection[UnidadNegocioKeys.InterAgencias].Update(entity);
                        return Ok(new
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
                        });
                    default:
                        return NotFound();
                }
                #endregion
            }
            catch (Exception ex)
            {
                ex.TryWriteLogObject(_logFileManager, _clientFeatures, LogType.Fail);
                return InternalServerError(ex);
            }
            finally
            {
                entity.TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            _crmCollection.Clear();
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
                case UnidadNegocioKeys.NuevoMundo:
                    _crmCollection.Add(UnidadNegocioKeys.NuevoMundo, new CuentaB2B_NM_Repository());
                    break;
                default:
                    break;
            }
            return unidadNegocioKey;
        }
    }
}
