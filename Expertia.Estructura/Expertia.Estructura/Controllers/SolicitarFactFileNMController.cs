using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Journeyou;
using Expertia.Estructura.Repository.AppWebs;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.General;
using Expertia.Estructura.Repository.NuevoMundo;
using Expertia.Estructura.RestManager.Base;
using Expertia.Estructura.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Expertia.Estructura.Controllers
{
    /// Expertia 1 : Solicitud de facturacion del file / Desglose CA
    [RoutePrefix(RoutePrefix.SolicitarFactFileNM)]
    public class SolicitarFactFileNMController : BaseController
    {
        private SolicitarFactFileNMRepository _solicitarFactFileNMRepository;
        private ICotizacionSRV_Repository _cotizSrvRepository;
               
        protected override ControllerName _controllerName => ControllerName.FileOportunidadNM;
        private DatosUsuario _datosUsuario;
        private DatosOficina _datosOficina;

        public SolicitarFactFileNMController()
        {
            _datosUsuario = new DatosUsuario();
            _datosOficina = new DatosOficina();
            _solicitarFactFileNMRepository = new SolicitarFactFileNMRepository(UnidadNegocioKeys.AppWebs);
            _cotizSrvRepository = new CotizacionSRV_AW_Repository(UnidadNegocioKeys.AppWebs);
        }

        #region PublicMethods
        [Route(RouteAction.Read)]
        public IHttpActionResult Read(SolicitarFactFileNM solicitarFactFileNM)
        {
            var exMessage = string.Empty;
            try
            {
                var usuarioLogin = _datosUsuario.Get_Dts_Usuario_Personal_NM(solicitarFactFileNM.idusuariosrv_SF);
                if (usuarioLogin != null && usuarioLogin.IdUsuario != solicitarFactFileNM.idusuariosrv_SF) { solicitarFactFileNM.idusuariosrv_SF = usuarioLogin.IdUsuario; }

                var result = _solicitarFactFileNMRepository.GuardarDesgloseCA(solicitarFactFileNM);

                if (solicitarFactFileNM.existeArchivoList)
                {
                    _solicitarFactFileNMRepository.GuardarArchivo(solicitarFactFileNM, result, int.Parse(solicitarFactFileNM.idusuario));
                }

                if (solicitarFactFileNM.existeIdDatosFacturacion)
                {
                    var archivoList = _solicitarFactFileNMRepository.ObtenerArchivos(solicitarFactFileNM.iddatosfacturacion);
                }

                var textoPost = TemplateHtml(solicitarFactFileNM);

                if (solicitarFactFileNM.enviarCA)
                {
                    var objOficina = _datosOficina.ObtieneOficinaXId(usuarioLogin.IdOfi);
                    if (_solicitarFactFileNMRepository.EsAreaCounterPresencial(usuarioLogin.IdOfi, usuarioLogin.IdDep, objOficina.bolEsRipley))
                    {
                        _cotizSrvRepository._Liberar_UsuWeb_CA(solicitarFactFileNM.intCotId);
                    }

                    /*
                    _cotizSrvRepository.Inserta_Post_Cot(
                        solicitarFactFileNM.intCotId,
                        "1",
                        textoPost,
                        );
                    */
                }

                return Ok(new
                {
                    Codigo = DbResponseCode.Success,
                    Mensaje = "El proceso se realizó con éxito"
                });
            }
            catch (Exception ex)
            {
                exMessage = ex.Message;
                return Ok(new
                {
                    Codigo = "ER",
                    Mensaje = ex.Message
                });
            }
            finally
            {
                (new
                {
                    Request = solicitarFactFileNM,
                    Response = new
                    {
                        Codigo = "ER",
                        Mensaje = exMessage
                    }
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region Auxiliar
        private string GenerarHtmlByRender(string pStrNombreAspx)
        {
            string strRpta = "";
            // Try
            StringWriter _writer = new StringWriter();
            //System.Web.HttpContext.Current.Server.Execute(pStrNombreAspx, _writer, true);
            System.Web.HttpContext.Current.Server.Execute(pStrNombreAspx, _writer);
            if (!string.IsNullOrEmpty(_writer.ToString()))
                strRpta = _writer.ToString();

            return strRpta;
        }

        private string TemplateHtml(SolicitarFactFileNM solicitarFactFileNM)
        {
            try
            {
                var str = GenerarHtmlByRender(@"~/App_Data/TemplateDesglose.html");

                str = str.Replace("[DK]", solicitarFactFileNM.dk);
                str = str.Replace("[campania]", solicitarFactFileNM.campania);
                str = str.Replace("[SubCodigo]", solicitarFactFileNM.subcodigo);
                    //.Replace("[Ejecutiva]", solicitarFactFileNM.ejecutiva)
                str = str.Replace("[NumFileNM]", solicitarFactFileNM.numfilenm);
                str = str.Replace("[NumFileDM]", solicitarFactFileNM.numfiledm);
                str = str.Replace("[CCB]", solicitarFactFileNM.ccb);
                    //.Replace("[Ruc]", solicitarFactFileNM.ruc);
                str = str.Replace("[Razon]", solicitarFactFileNM.razonsocial);
                str = str.Replace("[Correo]", solicitarFactFileNM.correo);
                str = str.Replace("[TipoDocum]", solicitarFactFileNM.tipodocidentidad);
                    //.Replace("[Descripcion_Doc_Cid]", solicitarFactFileNM.Descripcion_Doc_Cid);
                str = str.Replace("[Documento]", solicitarFactFileNM.numdocidentidad);
                str = str.Replace("[Nombre]", solicitarFactFileNM.nombre);
                str = str.Replace("[ApellidoP]", solicitarFactFileNM.apepaterno);
                str = str.Replace("[ApellidoM]", solicitarFactFileNM.apemateno);
                str = str.Replace("[OaRipley]", solicitarFactFileNM.oaripley);
                str = str.Replace("[MontoOA]", solicitarFactFileNM.oamonto);
                str = str.Replace("[Banco]", solicitarFactFileNM.banco);
                str = str.Replace("[CantidadMillas]", solicitarFactFileNM.cantidadmillas);
                str = str.Replace("[MontoMillas]", solicitarFactFileNM.montomillas);

                // Template Detalles de N° Recibo
                StringBuilder sbPostsRC = new StringBuilder();
                if (!(solicitarFactFileNM.ReciboDetalleList == null || solicitarFactFileNM.ReciboDetalleList.Count == 0))
                {
                    foreach (var reciboDetalle in solicitarFactFileNM.ReciboDetalleList)
                    {
                        sbPostsRC.Append(
                            "<tr>" +
                            "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" +
                            reciboDetalle.Sucursal +
                            "</td>" +
                            "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" +
                            reciboDetalle.NoRecibo +
                            "</td>" +
                            "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" +
                            string.Format("{0:0.00}", reciboDetalle.MontoRecibo) +
                            "</td>" +
                            "</tr>");
                    }
                }
                str = str.Replace("[trContentRC]", sbPostsRC.ToString());

                var sbPostsDTF = new StringBuilder();
                var montoTotalADT = new double(); var montoTotalCHD = new double(); var montoTotalINF = new double();
                if (!(solicitarFactFileNM.TarifaDetalleList == null || solicitarFactFileNM.TarifaDetalleList.Count == 0))
                {
                    foreach (var tarifaDetalle in solicitarFactFileNM.TarifaDetalleList)
                    {
                        sbPostsDTF.Append(
                            "<tr>" +
                            "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" +
                            tarifaDetalle.GrupoServicio +
                            "</td>" +
                            "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" +
                            tarifaDetalle.CantidadADT +
                            "</td>" +
                            "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" +
                            string.Format("{0:0.00}", tarifaDetalle.MontoPorADT) +
                            "</td>" +
                            "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" +
                            tarifaDetalle.CantidadCHD +
                            "</td>" +
                            "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" +
                            string.Format("{0:0.00}", tarifaDetalle.MontoPorCHD) +
                            "</td>" +
                            "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" +
                            tarifaDetalle.CantidadINF +
                            "</td>" +
                            "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" +
                            string.Format("{0:0.00}", tarifaDetalle.MontoPorINF) +
                            "</td>" +
                            "</tr>");

                        montoTotalADT += tarifaDetalle.MontoPorADT;
                        montoTotalCHD += tarifaDetalle.MontoPorCHD;
                        montoTotalINF += tarifaDetalle.MontoPorINF;
                    }
                }

                sbPostsDTF.Append(
                    "<tr>" +
                    "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210); color:#BC0606;'><strong>Total</strong></td>" +
                    "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210); color:#BC0606;'></td>" +
                    "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210);'><strong>" +
                    string.Format("{0:0.00}", montoTotalADT) +
                    "</strong></td>" +
                    "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210); color:#BC0606;'></td>" +
                    "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210);'><strong>" +
                    string.Format("{0:0.00}", montoTotalCHD) +
                    "</strong></td>" +
                    "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210); color:#BC0606;'></td>" +
                    "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210);'><strong>" +
                    string.Format("{0:0.00}", montoTotalINF) +
                    "</strong></td>" +
                    "</tr>");

                string strMontoaCobrar = string.Format("{0:0.00}", (montoTotalADT + montoTotalCHD + montoTotalINF));

                sbPostsDTF.Append(
                    "<tr>" +
                    "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; color:#BC0606;'><strong>Total a Cobrar</strong></td>" +
                    "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px;'><strong></strong></td>" +
                    "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px;'><strong></strong></td>" +
                    "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px;'><strong></strong></td>" +
                    "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px;'><strong></strong></td>" +
                    "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px;'><strong></strong></td>" +
                    "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px;'><strong>" +
                    "$" +
                    strMontoaCobrar +
                    "</strong></td>" +
                    "</tr>");

                str = str.Replace("[trContentDTF]", sbPostsDTF.ToString());

                /// Template Archivos
                var sbPostsArchivos = new StringBuilder();
                var archivoList = solicitarFactFileNM.ArchivoList;
                if (!(archivoList == null || archivoList.Count == 0))
                {
                    foreach (var objArchivos in archivoList)
                        sbPostsArchivos.Append(
                            "<tr>" +
                            "<td>" +
                            "<span style='font-size: 8pt; font-family: Arial; width:130px;'>" +
                            "<p>" +
                            "<div style='width:130px;'>" +
                            "<a class='dowloandarchivo' data-rutaarchivo='" +
                            objArchivos.RutaArchivo +
                            "' data-nombrearchivo='" +
                            objArchivos.NomArchivo +
                            "' href='" +
                            objArchivos.UrlArchivo +
                            "'>" +
                            objArchivos.NomArchivo +
                            "</a>" +
                            "</div>" +
                            "</p>" +
                            "</span>" +
                            "</td>" +
                            "</tr>");
                    str = str.Replace("[Archivos]", sbPostsArchivos.ToString());
                }
                else
                {
                    str = str.Replace("[Archivos]", string.Empty);
                }

                return str;
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);
            _solicitarFactFileNMRepository = new SolicitarFactFileNMRepository(unidadNegocioKey);
            return unidadNegocioKey;
        }
        #endregion
    }
}
