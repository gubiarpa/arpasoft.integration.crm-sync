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
            _crmCollection.Add(UnidadNegocioKeys.NuevoMundo, new CuentaB2B_NM_Repository());
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
                        _operation[UnidadNegocioNames.CondorTravel] = _crmCollection[UnidadNegocioKeys.CondorTravel].Create(entity);
                        break;
                    case UnidadNegocioKeys.NuevoMundo:
                        _operation[UnidadNegocioNames.NuevoMundo] = _crmCollection[UnidadNegocioKeys.NuevoMundo].Create(entity);
                        break;
                    case UnidadNegocioKeys.DestinosMundiales:
                    case UnidadNegocioKeys.InterAgencias:
                        _operation[UnidadNegocioNames.DestinosMundiales] = _crmCollection[UnidadNegocioKeys.DestinosMundiales].Create(entity);
                        _operation[UnidadNegocioNames.InterAgencias] = _crmCollection[UnidadNegocioKeys.InterAgencias].Create(entity);
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
