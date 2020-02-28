using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Expertia.Estructura.Models.Retail;
using Expertia.Estructura.Repository.Retail;
using Expertia.Estructura.Repository.General;

namespace Expertia.Estructura.Controllers
{
    public class OportunidadRetailController : BaseController
    {
        #region Properties
        private OportunidadRetailRepository _repository;
        private DatosUsuario _datosUsuario;
        #endregion

        #region PublicMethods
        public IHttpActionResult Read(OportunidadRetailReq oportunidadRetail)
        {
            try
            {
                var intIdUsuWeb = oportunidadRetail.IdUsuarioSrv_SF;
                var usuarioLogin = _datosUsuario.Get_Dts_Usuario_Personal(intIdUsuWeb);
                int? intIdCliCot = null;

                if (oportunidadRetail.IdCotSRV == null) // Nuevo SRV
                {
                    var objPersonal = (Personal)_repository.ObtienePersonalXId_TrustWeb(intIdUsuWeb)[""];
                    var objUsuarioWeb = (UsuarioWeb)_repository.ObtieneUsuarioWebXId(intIdUsuWeb)[""];
                    List<ClienteCot> clientes;

                    #region Get_HdIdCli
                    if ((new List<string> { "DNI", "PSP", "CEX" }).Contains(oportunidadRetail.IdTipoDoc))
                        clientes = (List<ClienteCot>)_repository.SelectByDocumento(oportunidadRetail.IdTipoDoc, oportunidadRetail.Numdoc)[""];
                    else
                        clientes = (List<ClienteCot>)_repository.SelectByEmail(oportunidadRetail.EmailCli)[""];
                    var hdIdCli = clientes.ElementAt(0).IdCliCot;
                    #endregion

                    if (hdIdCli.Equals("0")) // ◄ String.IsNullOrEmpty(Trim(hdIdCli.Value))
                    {
                        intIdCliCot = (int?)_repository.InsertaClienteCotizacion(
                            usuarioLogin.NomCompletoUsuario,
                            usuarioLogin.ApePatUsuario,
                            usuarioLogin.ApeMatUsuario,
                            usuarioLogin.EmailUsuario,
                            null,
                            null,
                            oportunidadRetail.EnviarPromociones.Equals("1"),
                            null,
                            oportunidadRetail.Numdoc,
                            oportunidadRetail.IdTipoDoc,
                            intIdUsuWeb,
                            39,
                            null,
                            false,
                            null,
                            null
                            )["pNumIdNewCliCot_out"];
                    }
                    else
                    {
                        intIdCliCot = hdIdCli;
                    }

                    _repository.ActualizaClienteCotizacion(
                        intIdCliCot ?? 0,
                        usuarioLogin.NomCompletoUsuario,
                        usuarioLogin.ApePatUsuario,
                        usuarioLogin.ApeMatUsuario,
                        usuarioLogin.EmailUsuario,
                        intIdUsuWeb
                        );

                    if (string.IsNullOrEmpty(oportunidadRetail.IdDestino) && oportunidadRetail.IdDestino.Length >= 3)
                        oportunidadRetail.IdDestino = oportunidadRetail.IdDestino.Substring(0, 3);

                    #region RegistraCotizacion
                    var intIdCotVta = _repository.InsertaCotizacionVenta(
                        3,
                        null,
                        objPersonal.NomCompletoPer,
                        objUsuarioWeb.LoginUsuWeb,
                        "",
                        intIdCliCot ?? 0,
                        intIdUsuWeb,
                        objPersonal.IdDepartamento,
                        objPersonal.IdOficina,
                        39,
                        1,
                        0 /*oportunidadRetail.IdCanalVenta*/, // ◄ Consultar con Gustavo por la especificación
                        null,
                        oportunidadRetail.IdDestino,
                        null,
                        1,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null);
                    #endregion
                }
                else
                {

                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            _repository = new OportunidadRetailRepository(unidadNegocioKey);
            return unidadNegocioKey;
        }
    }
}
