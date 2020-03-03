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
    [RoutePrefix(RoutePrefix.OportunidadRetail)]
    public class OportunidadRetailController : BaseController
    {
        #region Properties
        private UnidadNegocioKeys? _unidadNegocioKey;
        private OportunidadRetailRepository _repository;
        private DatosUsuario _datosUsuario;
        #endregion

        #region Constructor
        public OportunidadRetailController()
        {
            _unidadNegocioKey = RepositoryByBusiness(UnidadNegocioKeys.AppWebs);
        }
        #endregion

        #region PublicMethods
        [Route(RouteAction.Create)]
        public IHttpActionResult Create(OportunidadRetailReq oportunidadRetail)
        {
            if (DateTime.Now.Hour >= 0) // Data Provisional
            {
                return Ok(new OportunidadRetailRes()
                {
                    CodigoError = "OK",
                    MensajeError = "La oportunidad se creó correctamente",
                    IdOportunidad_SF = oportunidadRetail.IdOportunidad_SF,
                    IdCotSrv = new Random().Next(100000, 999999),
                    FechaCreacion = DateTime.Now.AddHours(- new Random().Next(0, 240)).ToString("dd/MM/yyyy")
                });
            }

            try
            {
                var intIdUsuWeb = oportunidadRetail.IdUsuarioSrv_SF;
                var usuarioLogin = _datosUsuario.Get_Dts_Usuario_Personal(intIdUsuWeb);
                int? intIdCliCot = null;

                int intIdOcurrencias;
                var objPersonal = (Personal)_repository.ObtienePersonalXId_TrustWeb(intIdUsuWeb)[""];
                var objUsuarioWeb = (UsuarioWeb)_repository.ObtieneUsuarioWebXId(intIdUsuWeb)[""];

                if (oportunidadRetail.IdCotSRV == null) // Nuevo SRV
                {
                    List<ClienteCot> clientes;

                    #region Get_HdIdCli
                    if ((new List<string> { "DNI", "PSP", "CEX" }).Contains(oportunidadRetail.IdTipoDoc))
                        clientes = (List<ClienteCot>)_repository.SelectByDocumento(oportunidadRetail.IdTipoDoc, oportunidadRetail.Numdoc)[""];
                    else
                        clientes = (List<ClienteCot>)_repository.SelectByEmail(oportunidadRetail.EmailCli)[""];
                    var hdIdCli = clientes.ElementAt(0).IdCliCot;
                    #endregion

                    #region IdClienteCotización
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
                    #endregion

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
                    var intIdCotVta = (long?)_repository.InsertaCotizacionVenta(
                        3,
                        null,
                        objPersonal.NomCompletoPer,
                        objUsuarioWeb.LoginUsuWeb,
                        Constantes_SRV.IP_GENERAL,
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
                        null
                        )[""];
                    #endregion

                    #region RegistraIngresoCliente
                    intIdOcurrencias = (int)_repository.InsertaIngresoCliente(
                        DateTime.Now,
                        objPersonal.NomCompletoPer,
                        objPersonal.ApePatPer,
                        objPersonal.ApeMatPer,
                        objPersonal.EmailPer,
                        oportunidadRetail.MotivoCrea, 
                        null,
                        oportunidadRetail.IdDestino,
                        intIdUsuWeb,
                        objPersonal.IdOficina,
                        objPersonal.IdDepartamento, 
                        DateTime.Now,
                        intIdUsuWeb,
                        objPersonal.IdOficina,
                        objPersonal.IdDepartamento,
                        oportunidadRetail.Comentario,
                        Constantes_SRV.IP_GENERAL,
                        intIdCotVta, 
                        intIdCliCot,
                        oportunidadRetail.IdTipoDoc,
                        oportunidadRetail.Numdoc,
                        null
                        )["pNumIdOcurr_out"];
                    #endregion

                    #region EnvioPromociones
                    if (_repository.ValidarEnvioPromociones(intIdOcurrencias, oportunidadRetail.EnviarPromociones))
                    {
                        _repository.EnviarPromociones(
                            objPersonal,
                            oportunidadRetail,
                            oportunidadRetail.EnviarPromociones.Equals("SI"),
                            objPersonal.NomCompletoPer,
                            objPersonal.ApePatPer,
                            objPersonal.EmailPer);
                    }
                    #endregion

                    var strCommentAttCli = string.IsNullOrEmpty(oportunidadRetail.Comentario) ?
                        "<br /><br /><strong>Comentarios: " + oportunidadRetail.Comentario + "</strong>" :
                        string.Empty;

                    #region InsertaPost
                    _repository.Inserta_Post_Cot(
                        (int)intIdCotVta,
                        Constantes_SRV.ID_TIPO_POST_SRV_USUARIO,
                        "Asignado por " + usuarioLogin.LoginUsuario + strCommentAttCli,
                        Constantes_SRV.IP_GENERAL,
                        usuarioLogin.LoginUsuario,
                        intIdUsuWeb,
                        objPersonal.IdDepartamento,
                        objPersonal.IdOficina,
                        null,
                        null,
                        (short)ENUM_ESTADOS_COT_VTA.Solicitado,
                        true,
                        null,
                        false,
                        null,
                        false,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null
                        );
                    #endregion
                }
                else
                {
                    #region RegistraIngresoCliente
                    intIdOcurrencias = (int)_repository.InsertaIngresoCliente(
                        DateTime.Now,
                        objPersonal.NomCompletoPer,
                        objPersonal.ApePatPer,
                        objPersonal.ApeMatPer,
                        objPersonal.EmailPer,
                        oportunidadRetail.MotivoCrea,
                        null,
                        oportunidadRetail.IdDestino,
                        intIdUsuWeb,
                        objPersonal.IdOficina,
                        objPersonal.IdDepartamento,
                        null,
                        intIdUsuWeb,
                        objPersonal.IdOficina,
                        objPersonal.IdDepartamento,
                        oportunidadRetail.Comentario,
                        Constantes_SRV.IP_GENERAL,
                        null,
                        null,
                        oportunidadRetail.IdTipoDoc,
                        oportunidadRetail.Numdoc,
                        null
                        )["pNumIdOcurr_out"];
                    #endregion

                    #region EnvioPromociones
                    if (_repository.ValidarEnvioPromociones(intIdOcurrencias, oportunidadRetail.EnviarPromociones))
                    {
                        _repository.EnviarPromociones(
                            objPersonal,
                            oportunidadRetail,
                            oportunidadRetail.EnviarPromociones.Equals("SI"),
                            objPersonal.NomCompletoPer,
                            objPersonal.ApePatPer,
                            objPersonal.EmailPer);
                    }
                    #endregion
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
            _datosUsuario = new DatosUsuario(unidadNegocioKey);
            return unidadNegocioKey;
        }
    }
}
