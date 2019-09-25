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
    [RoutePrefix(RoutePrefix.Contacto)]
    public class ContactoController : BaseController<Contacto>
    {
        [Route(RouteAction.Create)]
        public override IHttpActionResult Create(Contacto entity)
        {
            object result = null;
            try
            {
                #region UnidadNegocio
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:
                        _operCollection[UnidadNegocioKeys.CondorTravel] = _crmCollection[UnidadNegocioKeys.CondorTravel].Create(entity);
                        result = new
                        {
                            Result = new
                            {
                                CondorTravel = new
                                {
                                    CodigoError = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.CodigoError].ToString(),
                                    MensajeError = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.MensajeError].ToString()
                                }
                            }
                        };
                        break;
                    case UnidadNegocioKeys.NuevoMundo:
                        return NotFound();
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
                        break;
                    default:
                        return NotFound();
                }
                return Ok(result);
                #endregion
            }
            catch (Exception ex)
            {
                ex.TryWriteLogObject(_logFileManager, _clientFeatures, LogType.Fail);
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    BusinessUnity = entity.UnidadNegocio.Descripcion,
                    LegacySystems = result,
                    Body = entity
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }

        [Route(RouteAction.Update)]
        public override IHttpActionResult Update(Contacto entity)
        {
            object result = null;
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
                                    MensajeError = _operCollection[UnidadNegocioKeys.CondorTravel][OutParameter.MensajeError].ToString()
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
                        break;
                    default:
                        return NotFound();
                }
                return Ok(result);
                #endregion
            }
            catch (Exception ex)
            {
                ex.TryWriteLogObject(_logFileManager, _clientFeatures, LogType.Fail);
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    BusinessUnity = entity.UnidadNegocio.Descripcion,
                    LegacySystems = result,
                    Body = entity
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            switch (unidadNegocioKey)
            {
                case UnidadNegocioKeys.CondorTravel:
                    _crmCollection.Add(UnidadNegocioKeys.CondorTravel, new Contacto_CT_Repository());
                    break;
                case UnidadNegocioKeys.NuevoMundo:
                    break;
                case UnidadNegocioKeys.DestinosMundiales:
                case UnidadNegocioKeys.InterAgencias:
                    _crmCollection.Add(UnidadNegocioKeys.DestinosMundiales, new Contacto_DM_Repository());
                    _crmCollection.Add(UnidadNegocioKeys.InterAgencias, new Contacto_IA_Repository());
                    break;
                default:
                    break;
            }
            return unidadNegocioKey;
        }
    }
}
