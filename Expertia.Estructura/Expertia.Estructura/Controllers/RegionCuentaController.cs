using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.Condor;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.RegionCuenta)]
    public class RegionCuentaController : BaseController<RegionCuenta>
    {
        IRegionCuentaRepository _regionCuentaRepository;

        [Route(RouteAction.Create)]
        public IHttpActionResult Create(RegionCuenta regionCuenta)
        {
            object result = null;
            string error = string.Empty;
            try
            {
                RepositoryByBusiness(GetUnidadNegocio(regionCuenta.Region));
                var operation = _regionCuentaRepository.Register(regionCuenta);
                LoadResults(UnidadNegocioKeys.CondorTravel, operation, out result);

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
                    Error = error,
                    Result = result,
                    Body = regionCuenta
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }

        private void LoadResults(UnidadNegocioKeys unidadNegocio, Operation operation, out object result)
        {
            result = null;
            switch (unidadNegocio)
            {
                case UnidadNegocioKeys.CondorTravel:
                    #region Log
                    result = new
                    {
                        Code = operation[OutParameter.CodigoError].ToString(),
                        Message = operation[OutParameter.MensajeError].ToString()
                    };
                    #endregion
                    break;
            }
        }

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            _regionCuentaRepository = new RegionCuentaRepository(unidadNegocioKey);
            return unidadNegocioKey;
        }

        private UnidadNegocioKeys? GetUnidadNegocio(string countryName)
        {
            switch (countryName)
            {
                case CountryName.Peru: return UnidadNegocioKeys.CondorTravel;
                case CountryName.Chile: return UnidadNegocioKeys.CondorTravel_CL;
                case CountryName.Ecuador: return UnidadNegocioKeys.CondorTravel_EC;
                case CountryName.Brasil: return UnidadNegocioKeys.CondorTravel_BR;
            }
            return null;
        }
    }
}
