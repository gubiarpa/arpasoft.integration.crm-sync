using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
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
                WriteEntityLog(entity);
                // Aquí va la interacción con BD
                return Ok();
            }
            catch (Exception ex)
            {
                _logFileManager.WriteLine(LogType.Fail, ex.Message);
                return InternalServerError(ex);
            }            
        }

        [Route(RouteAction.Update)]
        public override IHttpActionResult Update(Contacto entity)
        {
            try
            {
                WriteEntityLog(entity);
                // Aquí va la interacción con BD
                return Ok();
            }
            catch (Exception ex)
            {
                _logFileManager.WriteLine(LogType.Fail, ex.Message);
                return InternalServerError(ex);
            }
        }

        #region Log
        protected override void WriteAllFieldsLog(Contacto entity)
        {
            int i = 0;
            WriteFieldLog("IdSalesForce", entity.IdSalesForce);
            WriteFieldLog("IdClienteSalesforce", entity.IdCuentaSalesForce);
            WriteFieldLog("Nombre", entity.Nombre);
            WriteFieldLog("ApePaterno", entity.ApePaterno);
            WriteFieldLog("ApeMaterno", entity.ApeMaterno);
            WriteFieldLog("FechaNacimiento", entity.FechaNacimiento);
            WriteFieldLog("EstadoCivil", entity.EstadoCivil);
            WriteFieldLog("Genero", entity.Genero);
            WriteFieldLog("Nacionalidad", entity.Nacionalidad);
            #region WriteFieldLog("Documentos", entity.Documentos);
            i = 0;
            if (entity.Documentos != null)
                foreach (var documento in entity.Documentos)
                {
                    WriteFieldLog("Documento[{0}].Tipo", i, documento.Tipo);
                    WriteFieldLog("Documento[{0}].Numero", i, documento.Numero);
                    i++;
                }
            else
                WriteFieldLog("Documentos");
            #endregion
            WriteFieldLog("LogoFoto", entity.LogoFoto);
            WriteFieldLog("Hijos", entity.Hijos);
            WriteFieldLog("Profesion", entity.Profesion);
            WriteFieldLog("CargoEmpresa", entity.CargoEmpresa);
            WriteFieldLog("TiempoEmpresa", entity.TiempoEmpresa);
            #region WriteFieldLog("Direcciones", entity.Direcciones);
            i = 0;
            if (entity.Direcciones != null)
                foreach (var direccion in entity.Direcciones)
                {
                    WriteFieldLog("Direccion[{0}].Tipo", i, direccion.Tipo);
                    WriteFieldLog("Direccion[{0}].Descripción", i, direccion.Descripcion);
                    WriteFieldLog("Direccion[{0}].Pais", i, direccion.Pais);
                    WriteFieldLog("Direccion[{0}].Departamento", i, direccion.Departamento);
                    WriteFieldLog("Direccion[{0}].Ciudad", i, direccion.Ciudad);
                    WriteFieldLog("Direccion[{0}].Distrito", i, direccion.Distrito);
                    i++;
                }
            else
                WriteFieldLog("Direcciones");
            #endregion
            #region WriteFieldLog("Telefonos", entity.Telefonos);
            i = 0;
            if (entity.Telefonos != null)
                foreach (var telefono in entity.Telefonos)
                {
                    WriteFieldLog("Telefono[{0}].Tipo", i, telefono.Tipo);
                    WriteFieldLog("Telefono[{0}].Numero", i, telefono.Numero);
                    i++;
                }
            else
                WriteFieldLog("Telefonos");
            #endregion
            #region WriteFieldLog("Sitios", entity.Sitios);
            i = 0;
            if (entity.Sitios != null)
                foreach (var sitio in entity.Sitios)
                {
                    WriteFieldLog("Sitio[{0}].Tipo", i, sitio.Tipo);
                    WriteFieldLog("Sitio[{0}].Descripcion", i, sitio.Descripcion);
                    i++;
                }
            else
                WriteFieldLog("Sitios");
            #endregion
            #region WriteFieldLog("Correos", entity.Correos);
            i = 0;
            if (entity.Correos != null)
                foreach (var correo in entity.Correos)
                {
                    WriteFieldLog("Correo[{0}].Tipo", i, correo.Tipo);
                    WriteFieldLog("Correo[{0}].Descripcion", i, correo.Descripcion);
                    i++;
                }
            else
                WriteFieldLog("Correos");
            #endregion
            #region WriteFieldLog("IdiomasComunicCliente", entity.IdiomasComunicCliente);
            i = 0;
            if (entity.IdiomasComunicCliente != null)
                foreach (var idioma in entity.IdiomasComunicCliente)
                {
                    WriteFieldLog("Idioma[{0}].ID", i, idioma.ID);
                    i++;
                }
            else
                WriteFieldLog("Idiomas");
            #endregion 
            WriteFieldLog("NivelRiesgo", entity.NivelRiesgo);
            WriteFieldLog("RegiónMercadoBranch", entity.RegiónMercadoBranch);
            WriteFieldLog("Estado", entity.Estado);
            WriteFieldLog("Comentarios", entity.Comentarios);
            WriteFieldLog("OrigenContacto", entity.OrigenContacto);
        }
        #endregion
    }
}
