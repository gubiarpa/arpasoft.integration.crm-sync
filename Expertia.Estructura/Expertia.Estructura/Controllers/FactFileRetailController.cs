using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.Retail;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.FacturacionFileRetail)]
    public class FactFileRetailController : BaseController
    {
        #region Properties  
        private IFactFileRepository _factFileRepository;
        private ICotizacionSRV_Repository _CotizacionSRV;
        private IDatosUsuario _datosUsuario;
        private IDatosOficina _datosOficina;
        #endregion
        #region Auxiliar
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            _factFileRepository = new FactFileRetailRepository();
            return unidadNegocioKey;
        }
        #endregion

        #region PublicMethod
        [Route(RouteAction.Create)]
        public IHttpActionResult Create(FactFileRetailReq models)
        {
            string result = "";
            UsuarioLogin objUsuarioLogin = _datosUsuario.Get_Dts_Usuario_Personal(models.IdUsuarioSrv_SF);
            int IdUsuario = objUsuarioLogin.IdUsuario; 
            int IdOfi = objUsuarioLogin.IdOfi; 
            int IdDep = objUsuarioLogin.IdDep; 
            List<FileRetail> lstArchivos = new List<FileRetail>();
            ///Guardar Datos de Facturacion
            models.IdUsuario = IdUsuario;
            
            result = Guardar_DesgloseCA(models);

            ///Subir Archivos
            //var objArchivos = new List<FileRetail>; //(List<FileRetail>)System.Web.HttpContext.Current.SetSessionStateBehavior(Constantes_SRV.SES_LISTA_ARCHIVOS_DESGLOSE_CA);
            //lstArchivos = objArchivos;

            ///Insertar APPWEBS.WFF_POST_COT_VTA
            string pStrTextoPost = TemplateHtml(models, lstArchivos);
            string strIPUsuCrea = "127.0.0.0";
            
            ///ENviar CA TRUE
            Models.Retail.Oficina objOficina = new Models.Retail.Oficina();
            objOficina = _datosOficina.ObtieneOficinaXId(IdOfi);
            
            Boolean bolValor = EsAreaCounterPresencial(IdOfi, IdDep, objOficina.bolEsRipley);
            
            if (bolValor)
            {
                bolValor = _CotizacionSRV._Liberar_UsuWeb_CA(models.Cot_Id);
            }
            
            Inserta_Post_Cot(models.Cot_Id, "1", pStrTextoPost,
                strIPUsuCrea, objUsuarioLogin.LoginUsuario, IdUsuario,
                IdDep, IdOfi, null, null, Constantes_SRV.INT_ID_ESTADO_COT_DERIVADO_A_CA, true, null,
                false, null, false, IdUsuario, IdOfi,
                IdDep,null,null,null,"",null);
            


            return null;
        }
        
        private string Guardar_DesgloseCA(FactFileRetailReq model)
        {
            int IdDatosFactura;
            string result = string.Empty;

            try
            {
                //IdDatosFactura = (int)(new FactFileRetailRepository(UnidadNegocioKeys.AppWebs).GuardarDatosFacturacion(model)["pNumId_out"]);
                IdDatosFactura = (int)_factFileRepository.GuardarDatosFacturacion(model)["pNumId_out"];

                if (model.IdDatosFacturacion != 0)
                {
                    //new FactFileRetailRepository(UnidadNegocioKeys.AppWebs).EliminarDetalleTarifa(model.IdDatosFacturacion);
                    _factFileRepository.EliminarDetalleTarifa(model.IdDatosFacturacion);
                    //new FactFileRetailRepository(UnidadNegocioKeys.AppWebs).EliminarDetalleNoRecibos(model.IdDatosFacturacion);
                    _factFileRepository.EliminarDetalleNoRecibos(model.IdDatosFacturacion);
                }

                //new FactFileRetailRepository(UnidadNegocioKeys.AppWebs).GuardarDetalleTarifa(model, IdDatosFactura);
                _factFileRepository.GuardarDetalleTarifa(model, IdDatosFactura);
                //new FactFileRetailRepository(UnidadNegocioKeys.AppWebs).GuardarDetalleNoRecibo(model, IdDatosFactura);
                _factFileRepository.GuardarDetalleNoRecibo(model, IdDatosFactura);

                result = Convert.ToString(IdDatosFactura);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
        
        private Boolean EsAreaCounterPresencial(int pIntIdOfi, int pIntIdDep, Boolean pBolEsRipley)
        {
            if ((pIntIdOfi == Constantes_SRV.INT_ID_OFI_CORPORATIVO_VACACIONAL && pIntIdDep == Constantes_SRV.INT_ID_DEP_COUNTER) || 
                (pIntIdOfi == Constantes_SRV.INT_ID_OFI_NMV && pIntIdDep == Constantes_SRV.INT_ID_DEP_COUNTER) || 
                (pIntIdOfi == Constantes_SRV.INT_ID_OFI_CALL_CENTER && pIntIdDep == Constantes_SRV.INT_ID_DEP_CALL_CENTER) || 
                (pIntIdOfi == Constantes_SRV.INT_ID_OFI_NMV && pIntIdDep == Constantes_SRV.INT_ID_DEP_SISTEMAS) || 
                (pIntIdOfi == Constantes_SRV.INT_ID_OFI_NMV && pIntIdDep == Constantes_SRV.INT_ID_DEP_LARCOMAR) || 
                pBolEsRipley)
                return true;
            else
                return false;
        }
        
        private int Inserta_Post_Cot(int pIntIdCot, string pStrTipoPost, string pStrTextoPost, 
            string pStrIPUsuCrea, string pStrLoginUsuCrea, int pIntIdUsuWeb, 
            int pIntIdDep, int pIntIdOfi, List<ArchivoPostCot> pLstArchivos, List<FilePTACotVta> pLstFilesPTA, Int16 pIntIdEstado, bool pBolCambioEstado, string pLstFechasCotVta, 
            bool pBolEsAutomatico, string pBytArchivoMail, bool pBolEsCounterAdmin, Nullable<int> pIntIdUsuWebCounterCrea, Nullable<int> pIntIdOfiCounterCrea, 
            Nullable<int> pIntIdDepCounterCrea, Nullable<bool> pBolEsUrgenteEmision, Nullable<DateTime> pDatFecPlazoEmision, Nullable<Int16> pIntIdMotivoNoCompro, string pStrOtroMotivoNoCompro, Nullable<Double> pDblMontoEstimadoFile)
        {
            int intIdPost = 0;
            
            intIdPost = _CotizacionSRV._Insert_Post_Cot(pIntIdCot, pStrTipoPost, pStrTextoPost, pStrIPUsuCrea,
                                    pStrLoginUsuCrea, pIntIdUsuWeb, pIntIdDep, pIntIdOfi, pLstArchivos, pLstFilesPTA, pIntIdEstado,
                                    pBolCambioEstado, pLstFechasCotVta, pBolEsAutomatico, pBytArchivoMail, pBolEsCounterAdmin,
                                    pIntIdUsuWebCounterCrea, pIntIdOfiCounterCrea, pIntIdDepCounterCrea, pBolEsUrgenteEmision,
                                    pDatFecPlazoEmision, pIntIdMotivoNoCompro, pStrOtroMotivoNoCompro, pDblMontoEstimadoFile);

            if (pLstArchivos != null)
            {
                foreach (ArchivoPostCot objArchivoPost in pLstArchivos)
                {

                }
            }

            if (pBolCambioEstado)
            {
                Post_SRV RQ_General_PostSRV = new Post_SRV();
                RQ_General_PostSRV.IdCot = pIntIdCot;
                RQ_General_PostSRV.LoginUsuCrea = pStrLoginUsuCrea;
                RQ_General_PostSRV.IPUsuCrea = pStrIPUsuCrea;
                RQ_General_PostSRV.IdEstado = pIntIdEstado;
                if (pBolEsCounterAdmin)
                {
                    RQ_General_PostSRV.IdUsuWebCounterCrea = pIntIdUsuWebCounterCrea;
                    RQ_General_PostSRV.IdDepCounterCrea = pIntIdDepCounterCrea;
                    RQ_General_PostSRV.IdOfiCounterCrea = pIntIdOfiCounterCrea;
                    RQ_General_PostSRV.EsAutomatico = pBolEsAutomatico;
                    RQ_General_PostSRV.IdUsuWeb = pIntIdUsuWeb;
                    _CotizacionSRV.UpdateEstadoCotVTA(RQ_General_PostSRV);
                }
                else
                {
                    RQ_General_PostSRV.IdUsuWebCounterCrea = pIntIdUsuWeb;
                    RQ_General_PostSRV.IdDepCounterCrea = pIntIdDep;
                    RQ_General_PostSRV.IdOfiCounterCrea = pIntIdOfi;
                    RQ_General_PostSRV.EsAutomatico = pBolEsAutomatico;
                    RQ_General_PostSRV.IdUsuWeb = 0;
                    _CotizacionSRV.UpdateEstadoCotVTA(RQ_General_PostSRV);

                }
            }

            if (pIntIdEstado == 8 && pIntIdMotivoNoCompro.HasValue) // ESTADO NO COMPRO
            {
                _CotizacionSRV._Update_MotivoNoCompro(pIntIdCot, pIntIdMotivoNoCompro, pStrOtroMotivoNoCompro);
            }

            if (pIntIdEstado == 5) //ESTADO FACTURADO
            {
                Update_Importe_FilesPTABy_Cot(pIntIdCot, pIntIdUsuWeb, pIntIdOfi, pIntIdDep);
            }

            return 0;
        }

        private void Update_Importe_FilesPTABy_Cot(int pIntIdCot, int pIntIdUsuWeb, int pIntIdOfi, int pIntIdDep)
        {
            List<FilePTACotVta> lstFiles = _CotizacionSRV._SelectFilesPTABy_IdCot(pIntIdCot, pIntIdUsuWeb, pIntIdOfi, pIntIdDep);

            foreach (FilePTACotVta objTempFilePTA in lstFiles)
            {
                //_Update_Importe_FilePTA(pIntIdCot, objTempFilePTA.IdSuc, objTempFilePTA.IdFilePTA, pIntIdUsuWeb, pIntIdOfi, pIntIdDep, "1");
            }
                
        }
        #endregion

        private string TemplateHtml(FactFileRetailReq models, List<FileRetail> lstArchivos)
        {
            try
            {
                //WebUtility objWebUtility = new WebUtility();
                string str = "";// objWebUtility.GenerarHtmlByRender("TemplateDesglose.html");

                //// Template Datos Facturacion
                //str = Replace(str, "[DK]", models.datosFacturacion.DK);
                //str = Replace(str, "[campania]", models.datosFacturacion.Campania);
                //str = Replace(str, "[SubCodigo]", models.datosFacturacion.SubCodigo);
                //str = Replace(str, "[Ejecutiva]", models.datosFacturacion.Ejecutiva);
                //str = Replace(str, "[NumFileNM]", models.datosFacturacion.NumfileNM);
                //str = Replace(str, "[NumFileDM]", models.datosFacturacion.NumfileDM);
                //str = Replace(str, "[CCB]", models.datosFacturacion.CCB);
                //str = Replace(str, "[Ruc]", models.datosFacturacion.RUC);
                //str = Replace(str, "[Razon]", models.datosFacturacion.RAZON);
                //str = Replace(str, "[Correo]", models.datosFacturacion.Correo);
                //str = Replace(str, "[TipoDocum]", models.datosFacturacion.TipoDocumento);
                //str = Replace(str, "[Descripcion_Doc_Cid]", models.datosFacturacion.Descripcion_Doc_Cid);
                //str = Replace(str, "[Documento]", models.datosFacturacion.Documento);
                //str = Replace(str, "[Nombre]", models.datosFacturacion.Nombre);
                //str = Replace(str, "[ApellidoP]", models.datosFacturacion.ApellidoP);
                //str = Replace(str, "[ApellidoM]", models.datosFacturacion.ApellidoM);
                //str = Replace(str, "[OaRipley]", models.datosFacturacion.OARippley);
                //str = Replace(str, "[MontoOA]", string.Format("{0:0.00}", models.datosFacturacion.MontoOA));

                //// Template Factura
                //str = Replace(str, "[Banco]", models.datosFacturacion.Banco);
                //str = Replace(str, "[CantidadMillas]", models.datosFacturacion.CantidadMillas);
                //str = Replace(str, "[MontoMillas]", string.Format("{0:0.00}", models.datosFacturacion.MontoMillas));

                //// Template Detalles de N° Recibo
                //StringBuilder sbPostsRC = new StringBuilder();
                //foreach (DetalleNoRecibo Item in models.detalleNoRecibo)
                //    sbPostsRC.Append("<tr>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + Item.Sucursal + "</td>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + Item.NoRecibo + "</td>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + string.Format("{0:0.00}", Item.MontoRecibo) + "</td>" + "</tr>");
                //str = Strings.Replace(str, "[trContentRC]", sbPostsRC.ToString());

                //// Template Detalles de Tarifas
                //StringBuilder sbPostsDTF = new StringBuilder();
                //double montoTotalADT = new double(), montoTotalCHD = new double(), montoTotalINF = new double();
                //foreach (DetalleTarifa Item in models.detalleTarifa)
                //{
                //    sbPostsDTF.Append("<tr>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + Item.GrupoServicio + "</td>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + Item.CantidadADT + "</td>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + string.Format("{0:0.00}", Item.MontoPorADT) + "</td>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + Item.CatindadCHD + "</td>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + string.Format("{0:0.00}", Item.MontoPorCHD) + "</td>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + Item.CantidadINF + "</td>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + string.Format("{0:0.00}", Item.MontoPorINF) + "</td>" + "</tr>");

                //    montoTotalADT += Item.MontoPorADT;
                //    montoTotalCHD += Item.MontoPorCHD;
                //    montoTotalINF += Item.MontoPorINF;
                //}
                //sbPostsDTF.Append("<tr>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210); color:#BC0606;'><strong>Total</strong></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210); color:#BC0606;'></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210);'><strong>" + string.Format("{0:0.00}", montoTotalADT) + "</strong></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210); color:#BC0606;'></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210);'><strong>" + string.Format("{0:0.00}", montoTotalCHD) + "</strong></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210); color:#BC0606;'></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210);'><strong>" + string.Format("{0:0.00}", montoTotalINF) + "</strong></td>" + "</tr>");

                //string strMontoaCobrar = string.Format("{0:0.00}", (montoTotalADT + montoTotalCHD + montoTotalINF));
                //sbPostsDTF.Append("<tr>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; color:#BC0606;'><strong>Total a Cobrar</strong></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px;'><strong></strong></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px;'><strong></strong></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px;'><strong></strong></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px;'><strong></strong></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px;'><strong></strong></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px;'><strong>" + "$" + strMontoaCobrar + "</strong></td>" + "</tr>");

                //str = Strings.Replace(str, "[trContentDTF]", sbPostsDTF.ToString());

                //// Template Archivos
                //StringBuilder sbPostsArchivos = new StringBuilder();
                //if (!lstArchivos == null)
                //{
                //    foreach (Archivos objArchivos in lstArchivos)
                //        sbPostsArchivos.Append("<tr>" + "<td>" + "<span style='font-size: 8pt; font-family: Arial; width:130px;'>" + "<p>" + "<div style='width:130px;'>" + "<a class='dowloandarchivo' data-rutaarchivo='" + objArchivos.RutaArchivo + "' data-nombrearchivo='" + objArchivos.NombreArchivo + "' href='" + objArchivos.URLArchivo + "'>" + objArchivos.NombreArchivo + "</a>" + "</div>" + "</p>" + "</span>" + "</td>" + "</tr>");
                //    str = Strings.Replace(str, "[Archivos]", sbPostsArchivos.ToString());
                //}
                //else
                //    str = Strings.Replace(str, "[Archivos]", "");


                // Template Observacion
                str = "";// Replace(str, "[Observacion]", models.datosFacturacion.Observacion);

                return str;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
