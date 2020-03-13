using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.AppWebs;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.General;
using Expertia.Estructura.Repository.Retail;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
            _CotizacionSRV = new CotizacionSRV_AW_Repository();
            _datosUsuario = new DatosUsuario();
            _datosOficina = new DatosOficina();
            return unidadNegocioKey;
        }
        #endregion

        #region PublicMethod
        [Route(RouteAction.Create)]
        public IHttpActionResult Create(FactFileRetailReq models)
        {
            _factFileRepository = new FactFileRetailRepository();
            _CotizacionSRV = new CotizacionSRV_AW_Repository();
            _datosUsuario = new DatosUsuario();
            _datosOficina = new DatosOficina();
            string _result = "";
            string exceptionMsg = string.Empty;
            object response = null;
            try
            {
                UsuarioLogin objUsuarioLogin = _datosUsuario.Get_Dts_Usuario_Personal(models.IdUsuarioSrv_SF);
                int IdUsuario = objUsuarioLogin.IdUsuario;
                int IdOfi = objUsuarioLogin.IdOfi;
                int IdDep = objUsuarioLogin.IdDep;
                List<FileRetail> lstArchivos = new List<FileRetail>();
                ///Guardar Datos de Facturacion
                models.IdUsuario = IdUsuario;

                _result = Guardar_DesgloseCA(models);

                ///Subir Archivos
                //var objArchivos = new List<FileRetail>; //(List<FileRetail>)System.Web.HttpContext.Current.SetSessionStateBehavior(Constantes_SRV.SES_LISTA_ARCHIVOS_DESGLOSE_CA);
                //lstArchivos = objArchivos;

                ///Insertar APPWEBS.WFF_POST_COT_VTA
                string pStrTextoPost = TemplateHtml(models, lstArchivos);
                string strIPUsuCrea = "::1";

                ///ENviar CA TRUE
                Models.Retail.Oficina objOficina = new Models.Retail.Oficina();
                objOficina = _datosOficina.ObtieneOficinaXId(IdOfi);

                Boolean bolValor = EsAreaCounterPresencial(IdOfi, IdDep, objOficina.bolEsRipley);

                if (bolValor)
                {
                    bolValor = _CotizacionSRV._Liberar_UsuWeb_CA(models.Cot_Id);
                }

                _CotizacionSRV.Inserta_Post_Cot(models.Cot_Id, "1", pStrTextoPost,
                    strIPUsuCrea, objUsuarioLogin.LoginUsuario, IdUsuario,
                    IdDep, IdOfi, null, null, Constantes_SRV.INT_ID_ESTADO_COT_DERIVADO_A_CA, true, null,
                    false, null, false, IdUsuario, IdOfi,
                    IdDep, null, null, null, "", null);

                response = new
                {
                    CodigoError = "OK",
                    MensajeError = "",
                    idFactura = _result
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                exceptionMsg = ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    Request = models,
                    Response = response,
                    Exception = exceptionMsg
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }

        }

        private string Guardar_DesgloseCA(FactFileRetailReq model)
        {
            int IdDatosFactura;
            string result = string.Empty;

            try
            {
                //IdDatosFactura = (int)(new FactFileRetailRepository(UnidadNegocioKeys.AppWebs).GuardarDatosFacturacion(model)["pNumId_out"]);
                IdDatosFactura = (int)_factFileRepository.GuardarDatosFacturacion(model);

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
                throw ex;
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

        //private int Inserta_Post_Cot(int pIntIdCot, string pStrTipoPost, string pStrTextoPost,
        //    string pStrIPUsuCrea, string pStrLoginUsuCrea, int pIntIdUsuWeb,
        //    int pIntIdDep, int pIntIdOfi, List<ArchivoPostCot> pLstArchivos, List<FilePTACotVta> pLstFilesPTA, Int16 pIntIdEstado, bool pBolCambioEstado, string pLstFechasCotVta,
        //    bool pBolEsAutomatico, string pBytArchivoMail, bool pBolEsCounterAdmin, Nullable<int> pIntIdUsuWebCounterCrea, Nullable<int> pIntIdOfiCounterCrea,
        //    Nullable<int> pIntIdDepCounterCrea, Nullable<bool> pBolEsUrgenteEmision, Nullable<DateTime> pDatFecPlazoEmision, Nullable<Int16> pIntIdMotivoNoCompro, string pStrOtroMotivoNoCompro, Nullable<Double> pDblMontoEstimadoFile)
        //{
        //    int intIdPost = 0;

        //    intIdPost = _CotizacionSRV._Insert_Post_Cot(pIntIdCot, pStrTipoPost, pStrTextoPost, pStrIPUsuCrea,
        //                            pStrLoginUsuCrea, pIntIdUsuWeb, pIntIdDep, pIntIdOfi, pLstArchivos, pLstFilesPTA, pIntIdEstado,
        //                            pBolCambioEstado, pLstFechasCotVta, pBolEsAutomatico, pBytArchivoMail, pBolEsCounterAdmin,
        //                            pIntIdUsuWebCounterCrea, pIntIdOfiCounterCrea, pIntIdDepCounterCrea, pBolEsUrgenteEmision,
        //                            pDatFecPlazoEmision, pIntIdMotivoNoCompro, pStrOtroMotivoNoCompro, pDblMontoEstimadoFile);

        //    if (pLstArchivos != null)
        //    {
        //        foreach (ArchivoPostCot objArchivoPost in pLstArchivos)
        //        {

        //        }
        //    }

        //    if (pBolCambioEstado)
        //    {
        //        Post_SRV RQ_General_PostSRV = new Post_SRV();
        //        RQ_General_PostSRV.IdCot = pIntIdCot;
        //        RQ_General_PostSRV.LoginUsuCrea = pStrLoginUsuCrea;
        //        RQ_General_PostSRV.IPUsuCrea = pStrIPUsuCrea;
        //        RQ_General_PostSRV.IdEstado = pIntIdEstado;
        //        RQ_General_PostSRV.IdDep = pIntIdDep;
        //        RQ_General_PostSRV.IdOfi = pIntIdOfi;
        //        if (pBolEsCounterAdmin)
        //        {
        //            RQ_General_PostSRV.IdUsuWebCounterCrea = pIntIdUsuWebCounterCrea;
        //            RQ_General_PostSRV.IdDepCounterCrea = pIntIdDepCounterCrea;
        //            RQ_General_PostSRV.IdOfiCounterCrea = pIntIdOfiCounterCrea;
        //            RQ_General_PostSRV.EsAutomatico = pBolEsAutomatico;
        //            RQ_General_PostSRV.IdUsuWeb = pIntIdUsuWeb;
        //            _CotizacionSRV.UpdateEstadoCotVTA(RQ_General_PostSRV);
        //        }
        //        else
        //        {
        //            RQ_General_PostSRV.IdUsuWebCounterCrea = pIntIdUsuWeb;
        //            RQ_General_PostSRV.IdDepCounterCrea = pIntIdDep;
        //            RQ_General_PostSRV.IdOfiCounterCrea = pIntIdOfi;
        //            RQ_General_PostSRV.EsAutomatico = pBolEsAutomatico;
        //            RQ_General_PostSRV.IdUsuWeb = pIntIdUsuWeb;
        //            _CotizacionSRV.UpdateEstadoCotVTA(RQ_General_PostSRV);

        //        }
        //    }

        //    if (pIntIdEstado == 8 && pIntIdMotivoNoCompro.HasValue) // ESTADO NO COMPRO
        //    {
        //        _CotizacionSRV._Update_MotivoNoCompro(pIntIdCot, pIntIdMotivoNoCompro, pStrOtroMotivoNoCompro);
        //    }

        //    if (pIntIdEstado == 5) //ESTADO FACTURADO
        //    {
        //        Update_Importe_FilesPTABy_Cot(pIntIdCot, pIntIdUsuWeb, pIntIdOfi, pIntIdDep);

        //        if (pLstFilesPTA != null)
        //        {
        //            if (pLstFilesPTA.Count > 0)
        //            {
        //                bool bolBDNuevoMundo = true;

        //                double dblTipoCambio = _CotizacionSRV._Select_TipoCambio(DateTime.Now, "SOL", 1, bolBDNuevoMundo);

        //                foreach (FilePTACotVta objFilePTACotVta in pLstFilesPTA)
        //                {
        //                    if (pBolEsCounterAdmin)
        //                    {
        //                        _CotizacionSRV._Insert_FilePTA_Cot(objFilePTACotVta.IdCot, objFilePTACotVta.IdSuc, objFilePTACotVta.IdFilePTA, objFilePTACotVta.Moneda, dblTipoCambio, objFilePTACotVta.ImporteFacturado, pIntIdUsuWebCounterCrea.Value, pIntIdOfiCounterCrea.Value, pIntIdDepCounterCrea.Value);
        //                    }
        //                    else
        //                    {
        //                        _CotizacionSRV._Insert_FilePTA_Cot(objFilePTACotVta.IdCot, objFilePTACotVta.IdSuc, objFilePTACotVta.IdFilePTA, objFilePTACotVta.Moneda, dblTipoCambio, objFilePTACotVta.ImporteFacturado, pIntIdUsuWeb, pIntIdOfi, pIntIdDep);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else if (pIntIdEstado == 4)
        //    {
        //        if (pLstFechasCotVta != null)
        //        {
        //            for (int intX = 0; intX <= pLstFechasCotVta.Length - 1; intX++)
        //            {
        //                if (pBolEsCounterAdmin)
        //                {
        //                    _CotizacionSRV._Insert_FechaSalida_Cot(pIntIdCot, pLstFechasCotVta[intX].ToString(), pIntIdUsuWebCounterCrea.Value, pIntIdDepCounterCrea.Value, pIntIdOfiCounterCrea.Value);

        //                }
        //                else
        //                {

        //                    _CotizacionSRV._Insert_FechaSalida_Cot(pIntIdCot, pLstFechasCotVta[intX].ToString(), pIntIdUsuWeb, pIntIdDep, pIntIdOfi);

        //                }
        //            }
        //        }
        //    }

        //    if (pBytArchivoMail != null)//NO ENTRA (SACADO DEL SRV)
        //    {
        //        _CotizacionSRV._Insert_ArchivoMail_Post_Cot(pIntIdCot, intIdPost, pBytArchivoMail);
        //    }

        //    if (pDblMontoEstimadoFile.HasValue && pDblMontoEstimadoFile.Value > 0)
        //    {
        //        _CotizacionSRV._Update_MontoEstimadoFileBy_IdCotVta(pIntIdCot, pDblMontoEstimadoFile.Value);
        //    }

        //    return intIdPost;
        //}

        private void Update_Importe_FilesPTABy_Cot(int pIntIdCot, int pIntIdUsuWeb, int pIntIdOfi, int pIntIdDep)
        {
            try
            {
                List<FilePTACotVta> lstFiles = _CotizacionSRV._SelectFilesPTABy_IdCot(pIntIdCot, pIntIdUsuWeb, pIntIdOfi, pIntIdDep);

                foreach (FilePTACotVta objTempFilePTA in lstFiles)
                {
                    _Update_Importe_FilePTA(pIntIdCot, objTempFilePTA.IdSuc, objTempFilePTA.IdFilePTA, pIntIdUsuWeb, pIntIdOfi, pIntIdDep, "1");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        private void _Update_Importe_FilePTA(int pIntIdCot, int pIntIdSuc, int pIntIdFile, int pIntIdUsuWeb, int pIntIdOfi, int pIntIdDep, string pStrEsUpdUsuario)
        {
            try
            {
                DataTable dtImporteFile = _CotizacionSRV._Select_InfoFile(pIntIdSuc, pIntIdFile);

                if (dtImporteFile.Rows.Count == 0)
                {
                    ///Ssegun el srv no hace nada
                }
                else
                {
                    DataRow drCliente = dtImporteFile.Rows[0];
                    string strIdMoneda = drCliente["ID_MONEDA"].ToString();
                    double dblImporteSuma = 0;

                    foreach (DataRow drImporteFile in dtImporteFile.Rows)
                    {
                        if ((drImporteFile["ID_MONEDA"]) != null)
                        {
                            if (drImporteFile["FLAG"].ToString() == "1")
                                dblImporteSuma += (double)drImporteFile["IMPORTE_TOTAL"];
                        }
                    }

                    _CotizacionSRV._Actualiza_Imp_File_Cot(pIntIdCot, pIntIdSuc, pIntIdFile, strIdMoneda,
                                                           dblImporteSuma, pIntIdUsuWeb, pIntIdOfi, pIntIdDep, pStrEsUpdUsuario);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string TemplateHtml(FactFileRetailReq models, List<FileRetail> lstArchivos)
        {
            try
            {
                string str = GenerarHtmlByRender(@"~/App_Data/TemplateDesglose.html");

                //// Template Datos Facturacion

                str = str.Replace("[DK]", models.DK);
                str = str.Replace("[campania]", models.Campania);
                str = str.Replace("[SubCodigo]", models.SubCodigo);
                str = str.Replace("[Ejecutiva]", models.Ejecutiva);
                str = str.Replace("[NumFileNM]", models.NUmFile_DM);
                str = str.Replace("[NumFileDM]", models.NumFile_NM);
                str = str.Replace("[CCB]", models.CCB);
                str = str.Replace("[Ruc]", models.RUC);
                str = str.Replace("[Razon]", models.RAZON);
                str = str.Replace("[Correo]", models.Correo);
                str = str.Replace("[TipoDocum]", models.TipoDocumento);
                str = str.Replace("[Descripcion_Doc_Cid]", models.Doc_cid);
                str = str.Replace("[Documento]", models.Documento);
                str = str.Replace("[Nombre]", models.Nombre);
                str = str.Replace("[ApellidoP]", models.ApellidoPaterno);
                str = str.Replace("[ApellidoM]", models.ApellidoMateno);
                str = str.Replace("[OaRipley]", models.OARipley);
                str = str.Replace("[MontoOA]", string.Format("{0:0.00}", models.MontoOA));

                // Template Factura
                str = str.Replace("[Banco]", models.Banco);
                str = str.Replace("[CantidadMillas]", models.CantidadMillas);
                str = str.Replace("[MontoMillas]", string.Format("{0:0.00}", models.MontoMillas));

                // Template Detalles de N° Recibo
                StringBuilder sbPostsRC = new StringBuilder();
                foreach (ReciboDetalle Item in models.ReciboDetalle)
                    sbPostsRC.Append("<tr>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + Item.Sucursal + "</td>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + Item.NoRecibo + "</td>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + string.Format("{0:0.00}", Item.MontoRecibo) + "</td>" + "</tr>");

                str = str.Replace("[trContentRC]", sbPostsRC.ToString());

                // Template Detalles de Tarifas
                StringBuilder sbPostsDTF = new StringBuilder();
                double montoTotalADT = new double(), montoTotalCHD = new double(), montoTotalINF = new double();
                foreach (TarifaDetalle Item in models.TarifaDetalle)
                {
                    sbPostsDTF.Append("<tr>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + Item.GrupoServicio + "</td>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + Item.CantidadADT + "</td>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + string.Format("{0:0.00}", Item.MontoPorADT) + "</td>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + Item.CantidadCHD + "</td>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + string.Format("{0:0.00}", Item.MontoPorCHD) + "</td>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + Item.CantidadINF + "</td>" + "<td style='font-size:12px; padding:5px; text-align:center; border-top:1px solid black;'>" + string.Format("{0:0.00}", Item.MontoPorINF) + "</td>" + "</tr>");

                    montoTotalADT += Item.MontoPorADT;
                    montoTotalCHD += Item.MontoPorCHD;
                    montoTotalINF += Item.MontoPorINF;
                }

                sbPostsDTF.Append("<tr>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210); color:#BC0606;'><strong>Total</strong></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210); color:#BC0606;'></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210);'><strong>" + string.Format("{0:0.00}", montoTotalADT) + "</strong></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210); color:#BC0606;'></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210);'><strong>" + string.Format("{0:0.00}", montoTotalCHD) + "</strong></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210); color:#BC0606;'></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; background-color: rgb(202,207,210);'><strong>" + string.Format("{0:0.00}", montoTotalINF) + "</strong></td>" + "</tr>");

                string strMontoaCobrar = string.Format("{0:0.00}", (montoTotalADT + montoTotalCHD + montoTotalINF));
                sbPostsDTF.Append("<tr>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px; color:#BC0606;'><strong>Total a Cobrar</strong></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px;'><strong></strong></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px;'><strong></strong></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px;'><strong></strong></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px;'><strong></strong></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px;'><strong></strong></td>" + "<td style='font-size:13px; padding:5px; text-align:center; border-top:1px;'><strong>" + "$" + strMontoaCobrar + "</strong></td>" + "</tr>");

                str = str.Replace("[trContentDTF]", sbPostsDTF.ToString());

                // Template Archivos
                StringBuilder sbPostsArchivos = new StringBuilder();
                if (lstArchivos != null)
                {
                    foreach (FileRetail objArchivos in lstArchivos)
                        sbPostsArchivos.Append("<tr>" + "<td>" + "<span style='font-size: 8pt; font-family: Arial; width:130px;'>" + "<p>" + "<div style='width:130px;'>" + "<a class='dowloandarchivo' data-rutaarchivo='" + objArchivos.RutaArchivo + "' data-nombrearchivo='" + objArchivos.NomArchivo + "' href='" + objArchivos.URLArchivo + "'>" + objArchivos.NomArchivo + "</a>" + "</div>" + "</p>" + "</span>" + "</td>" + "</tr>");
                    str = str.Replace("[Archivos]", sbPostsArchivos.ToString());
                }
                else
                    str = str.Replace("[Archivos]", "");

                return str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
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

    }
}
