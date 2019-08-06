using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.Condor;
using Expertia.Estructura.Repository.MDM;
using Expertia.Estructura.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Expertia.Estructura.Controllers
{
    /// <summary>
    /// Mantenimiento para Contactos B2B
    /// </summary>
    [RoutePrefix(RoutePrefix.CuentaB2B)]
    public class CuentaB2BController : BaseController<CuentaB2B>
    {
        private ICrud<CuentaB2B> _mdmRepository;
        private ICrud<CuentaB2B> _rbRepository;

        public CuentaB2BController() : base()
        {
            _mdmRepository = new CuentaB2B_MdmRepository();
            _rbRepository = new CuentaB2B_RbRepository();
        }

        [Route(RouteAction.Create)]
        public override IHttpActionResult Create(CuentaB2B entity)
        {
            try
            {
                var operationResult = new Operation();
                operationResult[Operation.Result] = ResultType.Success;
                _rbRepository.Create(entity);
                WriteEntityLog(entity);                
                return Ok(operationResult);
            }
            catch (Exception ex)
            {
                _logFileManager.WriteLine(LogType.Fail, ex.Message);
                return InternalServerError(ex);
            }
        }

        [Route(RouteAction.Update)]
        public override IHttpActionResult Update(CuentaB2B entity)
        {
            try
            {
                var operationResult = new Operation();
                operationResult[Operation.Result] = ResultType.Success;
                //_mdmRepository.Update(entity);
                WriteEntityLog(entity);
                return Ok();
            }
            catch (Exception ex)
            {
                _logFileManager.WriteLine(LogType.Fail, ex.Message);
                return InternalServerError(ex);
            }
        }
    }
}
