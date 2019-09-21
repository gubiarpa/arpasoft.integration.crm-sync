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
            try
            {
                #region UnidadNegocio
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:
                        _operation[UnidadNegocioKeys.CondorTravel.GetFullName()] = _crmCollection[UnidadNegocioKeys.CondorTravel].Create(entity);
                        break;
                    case UnidadNegocioKeys.NuevoMundo:
                        _operation[UnidadNegocioKeys.NuevoMundo.GetFullName()] = _crmCollection[UnidadNegocioKeys.NuevoMundo].Create(entity);
                        break;
                    case UnidadNegocioKeys.DestinosMundiales:
                    case UnidadNegocioKeys.InterAgencias:
                        _operation[UnidadNegocioKeys.DestinosMundiales.GetFullName()] = _crmCollection[UnidadNegocioKeys.DestinosMundiales].Create(entity);
                        _operation[UnidadNegocioKeys.InterAgencias.GetFullName()] = _crmCollection[UnidadNegocioKeys.InterAgencias].Create(entity);
                        break;
                    default:
                        break;
                }
                #endregion

                #region Response
                entity.WriteLogObject(_logFileManager, _clientFeatures);

                return Ok(new
                {
                    Result = new
                    {
                        Type = ResultType.Success,
                        Operation = _operation
                    },
                    Entity = entity
                });
                #endregion
            }
            catch (Exception ex)
            {
                ex.WriteLogObject(_logFileManager, _clientFeatures, LogType.Fail);
                return InternalServerError(ex);
            }
            finally
            {
                _operation = null;
            }
        }

        [Route(RouteAction.Update)]
        public override IHttpActionResult Update(Contacto entity)
        {
            try
            {
                #region UnidadNegocio
                entity.UnidadNegocio.ID = GetUnidadNegocio(entity.UnidadNegocio.Descripcion);
                switch (RepositoryByBusiness(entity.UnidadNegocio.ID))
                {
                    case UnidadNegocioKeys.CondorTravel:
                        _operation[UnidadNegocioKeys.CondorTravel.GetFullName()] = _crmCollection[UnidadNegocioKeys.CondorTravel].Update(entity);
                        break;
                    case UnidadNegocioKeys.NuevoMundo:
                        _operation[UnidadNegocioKeys.NuevoMundo.GetFullName()] = _crmCollection[UnidadNegocioKeys.NuevoMundo].Update(entity);
                        break;
                    case UnidadNegocioKeys.DestinosMundiales:
                    case UnidadNegocioKeys.InterAgencias:
                        _operation[UnidadNegocioKeys.DestinosMundiales.GetFullName()] = _crmCollection[UnidadNegocioKeys.DestinosMundiales].Update(entity);
                        _operation[UnidadNegocioKeys.InterAgencias.GetFullName()] = _crmCollection[UnidadNegocioKeys.InterAgencias].Update(entity);
                        break;
                    default:
                        break;
                }
                #endregion

                #region Response
                entity.WriteLogObject(_logFileManager, _clientFeatures);

                return Ok(new
                {
                    Result = new
                    {
                        Type = ResultType.Success,
                        Operation = _operation
                    },
                    Entity = entity
                });
                #endregion
            }
            catch (Exception ex)
            {
                ex.WriteLogObject(_logFileManager, _clientFeatures, LogType.Fail);
                return InternalServerError(ex);
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
