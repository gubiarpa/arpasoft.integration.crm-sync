using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
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
            var startReq = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            entity.ID = (new Random()).Next(0, 1000);
            return Ok(new
                {
                    ID = entity.ID,
                    TimeRequest = new TimeRequest()
                    {
                        Start = startReq,
                        End = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                    }
                });
        }

        [Route(RouteAction.Update)]
        public override IHttpActionResult Update(Contacto entity)
        {
            var startReq = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            return Ok(new
                {
                    timeRequest = new
                    {
                        StartRequest = startReq,
                        EndRequest = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")
                    }
                });
        }

        #region Log
        protected override void WriteAllFieldsLog(Contacto entity)
        {
            WriteFieldLog("IdSalesForce", entity.IdSalesForce);
            WriteFieldLog("ID", entity.ID);
            WriteFieldLog("Nombre", entity.Nombre);
            WriteFieldLog("ApePaterno", entity.ApePaterno);
            WriteFieldLog("ApeMaterno", entity.ApeMaterno);
            WriteFieldLog("FechaNacimiento", entity.FechaNacimiento);
            WriteFieldLog("EstadoCivil", entity.EstadoCivil);
            WriteFieldLog("Genero", entity.Genero);
            WriteFieldLog("Nacionalidad", entity.Nacionalidad);
            WriteFieldLog("Documentos", entity.Documentos);
            WriteFieldLog("LogoFoto", entity.LogoFoto);
            WriteFieldLog("Hijos", entity.Hijos);
            WriteFieldLog("Profesion", entity.Profesion);
            WriteFieldLog("CargoEmpresa", entity.CargoEmpresa);
            WriteFieldLog("TiempoEmpresa", entity.TiempoEmpresa);
            WriteFieldLog("Direcciones", entity.Direcciones);
            WriteFieldLog("Pais", entity.Pais);
            WriteFieldLog("Departamento", entity.Departamento);
            WriteFieldLog("Ciudad", entity.Ciudad);
            WriteFieldLog("Distrito", entity.Distrito);
            WriteFieldLog("Telefonos", entity.Telefonos);
            WriteFieldLog("Sitios", entity.Sitios);
            WriteFieldLog("Correos", entity.Correos);
            WriteFieldLog("IdiomasComunicCliente", entity.IdiomasComunicCliente);
            WriteFieldLog("NivelRiesgo", entity.NivelRiesgo);
            WriteFieldLog("RegiónMercadoBranch", entity.RegiónMercadoBranch);
            WriteFieldLog("Estado", entity.Estado);
            WriteFieldLog("Comentarios", entity.Comentarios);
            WriteFieldLog("OrigenContacto", entity.OrigenContacto);
        }
        #endregion
    }
}
