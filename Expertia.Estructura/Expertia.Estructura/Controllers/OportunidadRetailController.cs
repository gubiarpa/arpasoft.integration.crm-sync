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
using System.Collections;

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
                    FechaCreacion = DateTime.Now.AddHours(-new Random().Next(0, 240)).ToString("dd/MM/yyyy")
                });
            }


            try
            {
                var intIdUsuWeb = oportunidadRetail.IdUsuarioSrv_SF;
                var usuarioLogin = _datosUsuario.Get_Dts_Usuario_Personal(intIdUsuWeb);
                int? intIdCliCot = null;

                int intIdOcurrencias, pIntIdCot = 0;
                var objPersonal = (Personal)_repository.ObtienePersonalXId_TrustWeb(intIdUsuWeb)["pCurResult_out"];
                var objUsuarioWeb = (UsuarioWeb)_repository.ObtieneUsuarioWebXId(intIdUsuWeb)["pCurResult_out"];

                if (oportunidadRetail.IdCotSRV == null) // Nuevo SRV
                {
                    List<ClienteCot> clientes;

                    #region Get_HdIdCli
                    if ((new List<string> { "DNI", "PSP", "CEX" }).Contains(oportunidadRetail.IdTipoDoc))
                        clientes = (List<ClienteCot>)_repository.SelectByDocumento(oportunidadRetail.IdTipoDoc, oportunidadRetail.Numdoc)["pCurResult_out"];
                    else
                        clientes = (List<ClienteCot>)_repository.SelectByEmail(oportunidadRetail.EmailCli)[""];
                    var hdIdCli = clientes != null && clientes.Count > 0 ? clientes.ElementAt(0).IdCliCot : 0;
                    #endregion

                    #region IdClienteCotización
                    if (hdIdCli.Equals(0)) // ◄ String.IsNullOrEmpty(Trim(hdIdCli.Value))
                    {
                        /// (i) Inserta Cliente
                        intIdCliCot = (int)(_repository.InsertaClienteCotizacion(
                            oportunidadRetail.NombreCli,
                            oportunidadRetail.ApePatCli,
                            oportunidadRetail.ApeMatCli,
                            oportunidadRetail.EmailCli,
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
                            )["pNumIdNewCliCot_out"]);

                        /// (ii) Inserta Teléfonos
                        /// (iii) Inserta Archivos
                    }
                    else
                    {
                        intIdCliCot = hdIdCli;
                    }
                    #endregion

                    _repository.ActualizaClienteCotizacion(
                        intIdCliCot ?? 0,
                        oportunidadRetail.NombreCli/*usuarioLogin.NomCompletoUsuario*/,
                        oportunidadRetail.ApePatCli/*usuarioLogin.ApePatUsuario*/,
                        oportunidadRetail.ApeMatCli/*usuarioLogin.ApeMatUsuario*/,
                        oportunidadRetail.EmailCli/*usuarioLogin.EmailUsuario*/,
                        intIdUsuWeb
                        );

                    if (string.IsNullOrEmpty(oportunidadRetail.IdDestino) && oportunidadRetail.IdDestino.Length >= 3)
                        oportunidadRetail.IdDestino = oportunidadRetail.IdDestino.Substring(0, 3);

                    #region RegistraCotizacion
                    var intIdCotVta = (int)_repository.InsertaCotizacionVenta(
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
                        oportunidadRetail.IdCanalVenta,
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
                        )["pNumIdNewCot_out"];
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

                    var strCommentAttCli = !string.IsNullOrEmpty(oportunidadRetail.Comentario) ?
                        "<br /><br /><strong>Comentarios: " + oportunidadRetail.Comentario + "</strong>" :
                        string.Empty;

                    #region InsertaPostCot
                    pIntIdCot = intIdCotVta;
                    string pStrTipoPost = Constantes_SRV.ID_TIPO_POST_SRV_USUARIO;
                    string pStrTextoPost = "Asignado por " + usuarioLogin.LoginUsuario + strCommentAttCli;
                    string pStrIPUsuCrea = Constantes_SRV.IP_GENERAL;
                    string pStrLoginUsuCrea = usuarioLogin.LoginUsuario;
                    int pIntIdUsuWeb = intIdUsuWeb;
                    int pIntIdDep = objPersonal.IdDepartamento;
                    int pIntIdOfi = objPersonal.IdOficina;
                    List<ArchivoPostCot> pLstArchivos = null;
                    List<FilePTACotVta> pLstFilesPTA = null;
                    Int16 pIntIdEstado = (short)ENUM_ESTADOS_COT_VTA.Solicitado;
                    bool pBolCambioEstado = true;
                    ArrayList pLstFechasCotVta = null;
                    bool pBolEsAutomatico = false;
                    byte[] pBytArchivoMail = null;
                    bool pBolEsCounterAdmin = false;
                    int? pIntIdUsuWebCounterCrea = null;
                    int? pIntIdOfiCounterCrea = null;
                    int? pIntIdDepCounterCrea = null;
                    bool? pBolEsUrgenteEmision = null;
                    DateTime? pDatFecPlazoEmision = null;
                    Int16? pIntIdMotivoNoCompro = null;
                    string pStrOtroMotivoNoCompro = null;
                    double? pDblMontoEstimadoFile = null;

                    var intIdPost = _repository.Inserta_Post_Cot(
                        pIntIdCot,
                        pStrTipoPost,
                        pStrTextoPost,
                        pStrIPUsuCrea,
                        pStrLoginUsuCrea,
                        pIntIdUsuWeb,
                        pIntIdDep,
                        pIntIdOfi,
                        pLstArchivos,
                        pLstFilesPTA,
                        pIntIdEstado,
                        pBolCambioEstado,
                        pLstFechasCotVta,
                        pBolEsAutomatico,
                        pBytArchivoMail,
                        pBolEsCounterAdmin,
                        pIntIdUsuWebCounterCrea,
                        pIntIdOfiCounterCrea,
                        pIntIdDepCounterCrea,
                        pBolEsUrgenteEmision,
                        pDatFecPlazoEmision,
                        pIntIdMotivoNoCompro,
                        pStrOtroMotivoNoCompro,
                        pDblMontoEstimadoFile
                        );

                    if (pLstArchivos != null)
                    {
                        foreach (var objArchivoPost in pLstArchivos)
                        {
                            _repository.Inserta_Archivo_Post_Cot(
                                pIntIdCot,
                                intIdPost,
                                objArchivoPost.RutaArchivo,
                                objArchivoPost.NombreArchivo,
                                objArchivoPost.ExtensionArchivo,
                                objArchivoPost.Archivo
                                );
                        }
                    }


                    if (pBolCambioEstado)
                    {
                        if (pBolEsCounterAdmin)
                        {
                            _repository.Update_Estado_Cot_Vta(
                                pIntIdCot,
                                pStrLoginUsuCrea,
                                pStrIPUsuCrea,
                                pIntIdEstado,
                                pIntIdUsuWebCounterCrea.Value,
                                pIntIdDepCounterCrea.Value,
                                pIntIdOfiCounterCrea.Value,
                                pBolEsAutomatico,
                                pIntIdUsuWeb
                                );
                        }
                        else
                        {
                            _repository.Update_Estado_Cot_Vta(
                                pIntIdCot,
                                pStrLoginUsuCrea,
                                pStrIPUsuCrea,
                                pIntIdEstado,
                                pIntIdUsuWeb,
                                pIntIdDep,
                                pIntIdOfi,
                                pBolEsAutomatico,
                                null);
                            }
                    }

                    if (pIntIdEstado == 8 && pIntIdMotivoNoCompro.HasValue)
                    {
                        _repository.Update_MotivoNoCompro(pIntIdCot, pIntIdMotivoNoCompro, pStrOtroMotivoNoCompro);
                    }
                }
                #endregion
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

                var oportunidadRetailRes = new OportunidadRetailRes()
                {
                    CodigoError = "OK",
                    MensajeError = "Se agregó correctamente",
                    IdOportunidad_SF = oportunidadRetail.IdOportunidad_SF,
                    IdCotSrv = pIntIdCot,
                    FechaCreacion = DateTime.Now.ToString("dd/MM/yyyy")
                };

                return Ok(oportunidadRetailRes);
            }
            catch (Exception ex)
            {
                var oportunidadRetailResError = new OportunidadRetailRes()
                {
                    CodigoError = "ER",
                    MensajeError = ex.Message,
                    IdOportunidad_SF = oportunidadRetail.IdOportunidad_SF,
                    IdCotSrv = null,
                    FechaCreacion = DateTime.Now.ToString("dd/MM/yyyy")
                };

                return Ok(oportunidadRetailResError);
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
