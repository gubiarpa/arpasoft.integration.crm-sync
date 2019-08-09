using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.Condor;
using Expertia.Estructura.Utils;
using System;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.Contacto)]
    public class ContactoController : BaseController<Contacto>
    {
        private ICrud<Contacto> _mdmRepository;
        private ICrud<Contacto> _rbRepository;

        public ContactoController() : base()
        {
            _rbRepository = new Contacto_RbRepository();
        }

        [Route(RouteAction.Create)]
        public override IHttpActionResult Create(Contacto entity)
        {
            try
            {
                var operationResult = _rbRepository.Create(entity);
                WriteEntityInLog(entity);
                return Ok(new
                {
                    Result = new
                    {
                        Type = ResultType.Success,
                        CodError = operationResult["P_CODIGO_ERROR"],
                        MensajeError = operationResult["P_MENSAJE_ERROR"]
                    },
                    Entity = entity
                });
            }
            catch (Exception ex)
            {
                WriteObjectInLog(ex, LogType.Fail);
                return InternalServerError(ex);
            }            
        }

        [Route(RouteAction.Update)]
        public override IHttpActionResult Update(Contacto entity)
        {
            try
            {
                var operationResult = _rbRepository.Update(entity);
                WriteEntityInLog(entity);
                return Ok(new
                {
                    Result = new
                    {
                        Type = ResultType.Success,
                        CodError = operationResult["P_CODIGO_ERROR"],
                        MensajeError = operationResult["P_MENSAJE_ERROR"]
                    },
                    Entity = entity
                });
            }
            catch (Exception ex)
            {
                WriteObjectInLog(ex, LogType.Fail);
                return InternalServerError(ex);
            }
        }
    }
}
