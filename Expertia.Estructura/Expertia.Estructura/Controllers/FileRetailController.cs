using Expertia.Estructura.Models;
using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.AppWebs;
using Expertia.Estructura.Repository.General;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Expertia.Estructura.Models.Behavior;
using System.Collections;
using System.Text;
using System.Data;
using Microsoft.VisualBasic;
using Expertia.Estructura.Repository.Retail;
using EnvioAlertas;
using System.Configuration;

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.FileRetail)]
    public class FileRetailController : BaseController<object>
    {
        #region Properties               
        private IDatosUsuario _datosUsuario;
        private ICotizacionSRV_Repository _CotizacionSRV_Repository;
        private IFileSRVRetailRepository _FileSRVRetailRepository;
        private IPedidoRepository _PedidoRetail_Repository;
        #endregion

        #region Constructor
        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            if (unidadNegocioKey == null)
            {
                unidadNegocioKey = UnidadNegocioKeys.AppWebs;
            }
            _CotizacionSRV_Repository = new CotizacionSRV_AW_Repository();
            _datosUsuario = new DatosUsuario(unidadNegocioKey);
            return unidadNegocioKey;
        }
        #endregion

        #region Metodos Publicos
        [Route(RouteAction.Asociate)]
        public IHttpActionResult AsociateFile(AssociateFile FileAssociate)
        {
            AssociateFileRS _responseAsociate = new AssociateFileRS();
            CotizacionVta DtsCotizacionVta = new CotizacionVta();
            UsuarioLogin DtsUsuarioLogin = null;                         
            
            _CotizacionSRV_Repository = new CotizacionSRV_AW_Repository();
            _FileSRVRetailRepository = new FileSRVRetailRepository();
            _PedidoRetail_Repository = new Pedido_AW_Repository();

            List<FilePTACotVta> lstFilesPTACotVta = null;
            List<FilePTACotVta> lstFilesPTAOld = null;
            List<FileSRV> ListFile_Info = null;

            ArrayList lstFechasCotVta = new ArrayList(); /*Duda*/

            /*Se realizara el cambio de Estado a Facturado*/
            Int16 EstadoSeleccionado = (Int16)ENUM_ESTADOS_COT_VTA.Facturado;
            bool bolCambioEstado = true;

            /*Datos que se quitaran, solo lo agregamos para tener una mejor vision*/                                    
            string ErrorAsociate = string.Empty;
            try
            {
                /*Validaciones*/
                validacionAssociate(ref FileAssociate, ref _responseAsociate, ref DtsUsuarioLogin, ref ListFile_Info);
                if (string.IsNullOrEmpty(_responseAsociate.CodigoError) == false) return Ok(_responseAsociate);

                if (FileAssociate.LstFiles[0].accion_SF == Constantes_FileRetail.STR_ASOCIAR_FILE)
                {   
                    /*Obtenemos los datos del SRV, etc*/
                    DtsCotizacionVta = _CotizacionSRV_Repository.Get_Datos_CotizacionVta(FileAssociate.idCotSRV_SF);

                    lstFilesPTACotVta = new List<FilePTACotVta>();
                    FilePTACotVta _FilePTACotVta = null;
                    bool UpdateResponse = true;

                    foreach (FileSRV fileSRV in ListFile_Info)
                    {
                        if (!string.IsNullOrEmpty(fileSRV.IdFilePTA.ToString()))
                        {
                            _FilePTACotVta = new FilePTACotVta();
                            _FilePTACotVta.IdCot = FileAssociate.idCotSRV_SF;
                            _FilePTACotVta.IdSuc = Convert.ToInt16(fileSRV.Sucursal);
                            _FilePTACotVta.IdFilePTA = fileSRV.IdFilePTA;
                            _FilePTACotVta.ImporteFacturado = fileSRV.ImporteFact;
                            _FilePTACotVta.Moneda = fileSRV.Moneda;
                            lstFilesPTACotVta.Add(_FilePTACotVta);

                            if(UpdateResponse == true)/*Cargamos algunos valores al Response*/
                            {
                                _responseAsociate.NumFile = fileSRV.IdFilePTA;
                                _responseAsociate.Cliente = fileSRV.Cliente;
                                _responseAsociate.Importe = fileSRV.ImporteFact;
                                UpdateResponse = false;
                            }
                        }
                    }
                                       
                    /*Obtenemos los Files anteriores (Si los tiene)*/
                    lstFilesPTAOld = _CotizacionSRV_Repository._SelectFilesPTABy_IdCot(FileAssociate.idCotSRV_SF, 0, 0, 0);

                    /*Insertamos el POST*/
                    string strTextoPost = "<span class='texto_cambio_estado'>Cambio de estado a <strong>Facturado</strong> y asociación de Files a la cotización</span><br><br>";
                    _CotizacionSRV_Repository.Inserta_Post_Cot(FileAssociate.idCotSRV_SF, Constantes_SRV.ID_TIPO_POST_SRV_USUARIO, strTextoPost,
                        "127.0.0.0", DtsUsuarioLogin.LoginUsuario, DtsUsuarioLogin.IdUsuario, DtsUsuarioLogin.IdDep, DtsUsuarioLogin.IdOfi, null, lstFilesPTACotVta,
                        EstadoSeleccionado, bolCambioEstado, null, false, null, DtsUsuarioLogin.EsCounterAdminSRV,
                        (DtsUsuarioLogin.EsCounterAdminSRV == true ? DtsCotizacionVta.IdUsuWeb : DtsUsuarioLogin.IdUsuario),
                        (DtsUsuarioLogin.EsCounterAdminSRV == true ? DtsCotizacionVta.IdOfi : DtsUsuarioLogin.IdOfi),
                        (DtsUsuarioLogin.EsCounterAdminSRV == true ? DtsCotizacionVta.IdDep : DtsUsuarioLogin.IdDep), null, null, null, null, null);
                  
                    bool bolGeneroRC_OK = false;
                    try
                    {
                        bool bolGenerarRC = false;
                        StringBuilder sbDatosGeneraRC = new StringBuilder();

                        if (bolCambioEstado && (DtsUsuarioLogin.EsCounterAdminSRV == true ? DtsCotizacionVta.IdOfi : DtsUsuarioLogin.IdOfi) == 62)
                        {
                            List<PasarelaPago_Pedido> lstPedidos = _PedidoRetail_Repository.Get_Pedido_XSolicitud(null, null, null, null, FileAssociate.idCotSRV_SF, null, null);

                            double dblMontoPedido = 0;
                            double dblMontoPedidoRound = 0;
                            string strNroTarjeta = "";
                            string strNroTarjetaBaneado = string.Empty;
                            string strIdForma = "";
                            string strIdValor = "";
                            double dblTopeRC_UATP = System.Convert.ToDouble(ConfigAccess.GetValueInAppSettings("DBL_TOPE_GENERAR_RC_UATP"));
                            bool bolEsRutaSelva = false;
                            bool bolEsTarifaPublicada = false;

                            bool bolExisteSoloUnPedidoPagado = true;
                            Int16 intCantPedidosPagados = 0; /*CANTIDAD DE PEDIDOS QUE ESTÁN PAGADOS. SOLO SI TIENE UN PEDIDO PAGADO SE GENERA RC*/

                            foreach (PasarelaPago_Pedido objPasarelaPago_Pedido in lstPedidos)
                            {
                                if (objPasarelaPago_Pedido.IdEstadoPedido == Constantes_Pedido.INT_ID_ESTADO_PEDIDO_PAGADO)
                                    intCantPedidosPagados += 1;
                                if (intCantPedidosPagados > 1)
                                {
                                    bolExisteSoloUnPedidoPagado = false;
                                    break;
                                }
                            }

                            foreach (PasarelaPago_Pedido objPasarelaPago_Pedido in lstPedidos)
                            {
                                FormaPagoPedido objFormaPagoPedido = _PedidoRetail_Repository.Get_FormaPagoBy_IdPedido(objPasarelaPago_Pedido.IdPedido);
                                                                
                                if (objPasarelaPago_Pedido.IdEstadoPedido == Constantes_FileRetail.INT_ID_ESTADO_PEDIDO_VALIDADO || objPasarelaPago_Pedido.IdEstadoPedido == Constantes_FileRetail.INT_ID_ESTADO_PEDIDO_PAGADO)
                                {
                                    if (objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_SAFETYPAY_ONLINE || objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_SAFETYPAY_CASH || objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_PAGOEFECTIVO_EC || objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_PAGOEFECTIVO_EC_ONLINE || objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_SOLO_TARJETA)
                                    {
                                        dblMontoPedido = (double)objPasarelaPago_Pedido.MontoTarjeta;
                                        dblMontoPedidoRound = (double)objPasarelaPago_Pedido.MontoTarjeta;

                                        if (objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_SAFETYPAY_ONLINE || objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_SAFETYPAY_CASH)
                                        {
                                            strIdForma = Constantes_FileRetail.STR_ID_FORMA_PTA_SAFETYPAY;
                                            strIdValor = Constantes_FileRetail.STR_ID_VALOR_PTA_USD;
                                            if (objPasarelaPago_Pedido.IdEstadoPedido == Constantes_FileRetail.INT_ID_ESTADO_PEDIDO_VALIDADO || objPasarelaPago_Pedido.IdEstadoPedido == Constantes_FileRetail.INT_ID_ESTADO_PEDIDO_PAGADO)
                                                bolGenerarRC = true;
                                        }
                                        else if (objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_PAGOEFECTIVO_EC || objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_PAGOEFECTIVO_EC_ONLINE)
                                        {
                                            strIdForma = Constantes_FileRetail.STR_ID_FORMA_PTA_PAGOEFECTIVO;
                                            strIdValor = Constantes_FileRetail.STR_ID_VALOR_PTA_USD;
                                            if (objPasarelaPago_Pedido.IdEstadoPedido == Constantes_FileRetail.INT_ID_ESTADO_PEDIDO_VALIDADO || objPasarelaPago_Pedido.IdEstadoPedido == Constantes_FileRetail.INT_ID_ESTADO_PEDIDO_PAGADO)
                                                bolGenerarRC = true;
                                        }
                                        else if (objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_SOLO_TARJETA)
                                        {
                                            if (objPasarelaPago_Pedido.IdEstadoPedido == Constantes_FileRetail.INT_ID_ESTADO_PEDIDO_VALIDADO || objPasarelaPago_Pedido.IdEstadoPedido == Constantes_FileRetail.INT_ID_ESTADO_PEDIDO_PAGADO)
                                                bolGenerarRC = false;
                                            if (objFormaPagoPedido.IdTipoTarjeta == Constantes_FileRetail.STR_ID_TIPO_TARJETA_VISA)
                                            {
                                                strIdForma = Constantes_FileRetail.STR_ID_FORMA_PTA_VISA;
                                                strIdValor = Constantes_FileRetail.STR_ID_VALOR_PTA_UATP;
                                            }
                                            else if (objFormaPagoPedido.IdTipoTarjeta == Constantes_FileRetail.STR_ID_TIPO_TARJETA_MASTERCARD || objFormaPagoPedido.IdTipoTarjeta == Constantes_FileRetail.STR_ID_TIPO_TARJETA_MASTERCARD_CA)
                                            {
                                                strIdForma = Constantes_FileRetail.STR_ID_FORMA_PTA_MASTERCARD;
                                                strIdValor = Constantes_FileRetail.STR_ID_VALOR_PTA_UATP;
                                            }
                                            else if (objFormaPagoPedido.IdTipoTarjeta == Constantes_FileRetail.STR_ID_TIPO_TARJETA_AMERICAN_EXPRESS)
                                            {
                                                strIdForma = Constantes_FileRetail.STR_ID_FORMA_PTA_AMERICAN;
                                                strIdValor = Constantes_FileRetail.STR_ID_VALOR_PTA_UATP;
                                            }
                                            else if (objFormaPagoPedido.IdTipoTarjeta == Constantes_FileRetail.STR_ID_TIPO_TARJETA_DINERS)
                                            {
                                                strIdForma = Constantes_FileRetail.STR_ID_FORMA_PTA_DINERS;
                                                strIdValor = Constantes_FileRetail.STR_ID_VALOR_PTA_UATP;
                                            }
                                        }

                                        if (ConfigAccess.GetValueInAppSettings("STR_GENERAR_RC_PTA").Equals("1"))
                                        {
                                            if (bolGenerarRC && bolExisteSoloUnPedidoPagado && objPasarelaPago_Pedido.IdCotSRV.HasValue && lstFilesPTACotVta.Count > 0)
                                            {                                               
                                                if (bolGenerarRC)
                                                {
                                                    foreach (FilePTACotVta objFilePTACotVtaTmp in lstFilesPTACotVta)
                                                    {
                                                        sbDatosGeneraRC.Append("File: " + objFilePTACotVtaTmp.IdFilePTA + " - " + objFilePTACotVtaTmp.IdSuc + " / Monto: " + objFilePTACotVtaTmp.Moneda + " - " + objFilePTACotVtaTmp.ImporteFacturado + " <br>");
                                                    }
                                                        
                                                    sbDatosGeneraRC.Append("EsUATP: " + objPasarelaPago_Pedido.EsUATP + "<br>" + Constants.vbCrLf);
                                                    sbDatosGeneraRC.Append("IdPedido: " + objPasarelaPago_Pedido.IdPedido + "<br>" + Constants.vbCrLf);
                                                    sbDatosGeneraRC.Append("dblMontoPedido: " + dblMontoPedido + "<br>" + Constants.vbCrLf);
                                                    sbDatosGeneraRC.Append("dblMontoPedidoRound: " + dblMontoPedidoRound + "<br>" + Constants.vbCrLf);
                                                    sbDatosGeneraRC.Append("strNroTarjeta: " + strNroTarjetaBaneado + "<br>" + Constants.vbCrLf);
                                                    sbDatosGeneraRC.Append("strIdForma: " + strIdForma + "<br>" + Constants.vbCrLf);
                                                    sbDatosGeneraRC.Append("strIdValor: " + strIdValor + "<br>" + Constants.vbCrLf);
                                                    sbDatosGeneraRC.Append("FechaPedido: " + objPasarelaPago_Pedido.FechaPedido + "<br>" + Constants.vbCrLf);
                                                    sbDatosGeneraRC.Append("dblTopeRC_UATP: " + dblTopeRC_UATP + "<br>" + Constants.vbCrLf);
                                                    sbDatosGeneraRC.Append("IdCotSRV: " + DtsCotizacionVta.IdCot + "<br>" + Constants.vbCrLf);
                                                    sbDatosGeneraRC.Append("EsRutaSelva: " + bolEsRutaSelva + "<br>" + Constants.vbCrLf);
                                                    sbDatosGeneraRC.Append("EsTarifaPublicada: " + bolEsTarifaPublicada + "<br>" + Constants.vbCrLf);

                                                    if (objPasarelaPago_Pedido.EsUATP)
                                                    {
                                                        if (dblMontoPedido > dblTopeRC_UATP)
                                                        {
                                                            sbDatosGeneraRC.Append("<strong>No generar RC por tope</strong><br>" + Constants.vbCrLf);
                                                            bolGenerarRC = false;
                                                        }                                               
                                                        bolGenerarRC = false;
                                                    }
                                                    else
                                                    {
                                                        bolGenerarRC = true;
                                                    }   

                                                    if (bolGenerarRC)
                                                    {
                                                        ArrayList alstResult = null;
                                                        string strMsgError = "";

                                                        try
                                                        {
                                                            bool intInsertaReciboReserva = false;
                                                            if (bolEsRutaSelva)
                                                            {
                                                                if (bolEsTarifaPublicada)
                                                                {
                                                                    alstResult = _FileSRVRetailRepository.Inserta_ReciboCaja(lstFilesPTACotVta, objPasarelaPago_Pedido.EsUATP, objPasarelaPago_Pedido.IdPedido, dblMontoPedido, dblMontoPedidoRound, strNroTarjeta, strIdForma, strIdValor, "cajawebPUB", objPasarelaPago_Pedido.FechaPedido, null, bolEsRutaSelva, bolEsTarifaPublicada);
                                                                }
                                                                else
                                                                {
                                                                    intInsertaReciboReserva = false;
                                                                }
                                                            }
                                                            else {
                                                                alstResult = _FileSRVRetailRepository.Inserta_ReciboCaja(lstFilesPTACotVta, objPasarelaPago_Pedido.EsUATP, objPasarelaPago_Pedido.IdPedido, dblMontoPedido, dblMontoPedidoRound, strNroTarjeta, strIdForma, strIdValor, "cajaweb", objPasarelaPago_Pedido.FechaPedido, null, bolEsRutaSelva, bolEsTarifaPublicada);
                                                            }                                                                
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            strMsgError = ex.ToString();
                                                            string strIPUsuario = "127.0.0.0";                                                          
                                                            NMailAlerta oNMailAlerta = new NMailAlerta();
                                                            oNMailAlerta.EnvioCorreoRegistrarError("Error de " + Constantes_SRV.APP_NAME, this, ex, strIPUsuario + "|Inserta_ReciboCaja");
                                                            oNMailAlerta = null;
                                                        }

                                                        string strLog = "";
                                                        EnviarCorreo objEnviarCorreo = new EnviarCorreo();
                                                        if (alstResult != null && alstResult.Count > 0)
                                                        {
                                                            if (alstResult[0].ToString() == "1") { bolGeneroRC_OK = true; }
                                                            try
                                                            {
                                                                strLog = alstResult[2].ToString();
                                                            }
                                                            catch (Exception ex){}

                                                            if (bolGeneroRC_OK)
                                                            {
                                                                string strIdsRCTmp = alstResult[1].ToString();
                                                                string strIdsRC = "";

                                                                StringBuilder sbInfo = new StringBuilder();
                                                                sbInfo.Append("Se han generado los siguientes RC para el pedido nro. <strong>" + objPasarelaPago_Pedido.IdPedido + "</strong>:<br><br>" + Constants.vbCrLf);

                                                                foreach (string strIdRC in strIdsRCTmp.Split(';'))
                                                                {
                                                                    if (!string.IsNullOrEmpty(strIdRC))
                                                                    {
                                                                        strIdsRC += strIdRC + ",";
                                                                        sbInfo.Append("-RC " + strIdRC + "<br>" + Constants.vbCrLf);
                                                                    }
                                                                }
                                                                if (strIdsRC.Length > 0)
                                                                    strIdsRC = strIdsRC.Substring(0, strIdsRC.Length - 1);

                                                                objEnviarCorreo.Enviar_Log("Log: se generó RC", sbDatosGeneraRC.ToString() + "<br><br>Log:" + strLog + "<br><br>" + "Generación automática de RC nro. " + strIdsRC + "<br><br>" + sbInfo.ToString(), true, ConfigurationManager.AppSettings["STR_PATH_FILES_RC_WEB"], "RC_OK_SRV_" + DtsCotizacionVta.IdCot + "_" + strIdsRC.Replace(",", "-"));

                                                                objEnviarCorreo.Enviar_CajaWeb(Webs_Cid.ID_WEB_WEBFAREFINDER, 1, 128, sbInfo.ToString(), "Generación automática de RC nro. " + strIdsRC, null);
                                                            }
                                                            else
                                                            {
                                                                objEnviarCorreo.Enviar_Log("Log: no se generó RC", sbDatosGeneraRC.ToString() + "<br><br>Log:" + strLog + "<br><br><strong>Error:" + strMsgError + "</strong>", true, ConfigurationManager.AppSettings["STR_PATH_FILES_RC_WEB"], "RC_NO_OK_SRV_" + DtsCotizacionVta.IdCot);
                                                                try
                                                                {                                                                    
                                                                    string strIPUsuario = "127.0.0.0";                                                                 
                                                                    _CotizacionSRV_Repository.Inserta_Post_Cot(DtsCotizacionVta.IdCot, Constantes_SRV.ID_TIPO_POST_SRV_USUARIO, strLog, strIPUsuario, DtsUsuarioLogin.LoginUsuario, DtsUsuarioLogin.IdUsuario, DtsUsuarioLogin.IdDep, DtsUsuarioLogin.IdOfi, null, null, objPasarelaPago_Pedido.IdEstadoPedido, true, null, true, null, false, null, null, null, null, null, null, null, null);
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                }
                                                            }
                                                        }
                                                        else
                                                            objEnviarCorreo.Enviar_Log("Log: no se generó RC", sbDatosGeneraRC.ToString() + "<br><br>Log:" + strLog + "<br><br><strong>Error:" + strMsgError + "</strong>", true, ConfigurationManager.AppSettings["STR_PATH_FILES_RC_WEB"], "RC_NO_OK_SRV_" + DtsCotizacionVta.IdCot);
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        EnviarCorreo objEnviarCorreo = new EnviarCorreo();
                                                        objEnviarCorreo.Enviar_Log("Log: no se generó RC", sbDatosGeneraRC.ToString() + "<br><br><strong>Error: validaciones internas</strong>", true, ConfigurationManager.AppSettings["STR_PATH_FILES_RC_WEB"], "RC_NO_OK_SRV_" + DtsCotizacionVta.IdCot);
                                                    }
                                                }                                                
                                                bolGenerarRC = false; /*Para que no genere otro RC*/
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {                    
                        string strIPUsuario = "127.0.0.0";                       
                        NMailAlerta oNMailAlerta = new NMailAlerta();
                        oNMailAlerta.EnvioCorreoRegistrarError("Error de " + Constantes_SRV.APP_NAME, this, ex, strIPUsuario + "|Inserta_ReciboCaja");
                        oNMailAlerta = null;
                    }

                    if (!bolGeneroRC_OK)
                    {
                        if (EstadoSeleccionado == (Int16)ENUM_ESTADOS_COT_VTA.Facturado && bolCambioEstado)
                        {
                            Enviar_MailCajaWeb(DtsCotizacionVta.IdCot, EstadoSeleccionado, DtsUsuarioLogin.EmailUsuario, DtsUsuarioLogin);
                        }                            
                    }

                    try
                    {
                        Nullable<bool> bolUATPExoneradoFirmaCliente = default(Boolean?);

                        /*Si en el SRV es verdadero, en PTA es falso y viceversa*/
                        bolUATPExoneradoFirmaCliente = true;
                        string hdMuestraModCompra = "1";
                        bool rbModCompraPresencial = false; bool rbModCompraNoPresencial = true;

                        if (hdMuestraModCompra == "1")
                        {
                            Int16 intIdModalidadCompra = -1;
                            if (rbModCompraPresencial == true)
                                intIdModalidadCompra = 1;
                            else if (rbModCompraNoPresencial == true)
                                intIdModalidadCompra = 0;
                            if (intIdModalidadCompra >= 0)
                                _CotizacionSRV_Repository._Update_ModalidadCompra(DtsCotizacionVta.IdCot, intIdModalidadCompra);
                        }

                        if (EstadoSeleccionado == (short)ENUM_ESTADOS_COT_VTA.Facturado || DtsCotizacionVta.IdEstado == (short)ENUM_ESTADOS_COT_VTA.Facturado)
                        {                            
                            if (lstFilesPTACotVta != null)
                            {                                
                                List<FilePTACotVta> lstFilesPTANew = null;
                                if (lstFilesPTAOld != null)
                                {                                    
                                    bool bolEsFileNuevo = true;
                                    lstFilesPTANew = new List<FilePTACotVta>();
                                    foreach (FilePTACotVta objFilePTANew in lstFilesPTACotVta)
                                    {                                 
                                        bolEsFileNuevo = true;
                                        foreach (FilePTACotVta objFilePTAOld in lstFilesPTAOld)
                                        {
                                            if (objFilePTANew.IdFilePTA == objFilePTAOld.IdFilePTA & objFilePTANew.IdSuc == objFilePTAOld.IdSuc)
                                                bolEsFileNuevo = false;
                                        }
                                        if (bolEsFileNuevo)
                                        {
                                            lstFilesPTANew.Add(objFilePTANew);
                                        }
                                    }
                                }
                                else
                                {
                                    lstFilesPTANew = lstFilesPTACotVta;
                                }

                                if (lstFilesPTANew.Count > 0)
                                {
                                    try
                                    {
                                        if (DtsUsuarioLogin.EsCounterAdminSRV || DtsUsuarioLogin.EsSupervisorSRV)
                                        {
                                            // objUsuarioSession.IdUsuario = NMConstantesUtility.INT_ID_USUWEB_OCAYO OrElse _
                                            // objUsuarioSession.IdUsuario = NMConstantesUtility.INT_ID_USUWEB_YCASAVERDE Then
                                            // strLog &= "5,"
                                            if (!string.IsNullOrEmpty(DtsUsuarioLogin.IdVendedorPTA) & !string.IsNullOrEmpty(DtsCotizacionVta.IdVendedorPTACrea))
                                            {
                                                // strLog &= "6,"
                                                // If (objUsuarioSession.IdOfi = NMConstantesUtility.INT_ID_OFI_NMVCOM Or _
                                                // objUsuarioSession.IdDep = NMConstantesUtility.INT_ID_DEP_SISTEMAS) And _
                                                // (CInt(hdIdOfiCot.Value) = NMConstantesUtility.INT_ID_OFI_NMVCOM Or _
                                                // CInt(hdIdDepCot.Value) = NMConstantesUtility.INT_ID_DEP_SISTEMAS) Then
                                                // strLog &= "3,"
                                                // objPTAFileBO.Actualiza_FechaCierreVenta(lstFilesPTANew, NMConstantesUtility.INT_ID_EMPRESA_GNM_NM, _
                                                // objUsuarioSession.IdVendedorPTA, hdIdVendCreaCot.Value, objUsuarioSession.LoginUsuario, _
                                                // bolUATPExoneradoFirmaCliente, litNomUsuCreaCot.Text)
                                                // strLog &= "4,"
                                                // End If
                                                // strLog &= "Vend: " & hdIdVendCreaCot.Value
                                                //NuevoMundoBusiness.Vendedor_RepMetaBO objVendedor_RepMetaBO = new NuevoMundoBusiness.Vendedor_RepMetaBO();
                                                if (_FileSRVRetailRepository._Existe_Vendedor_SubArea(Constantes_SRV.INT_ID_AREA_NMVCOM_METAS, Constantes_SRV.INT_ID_SUBAREA_NMVCOM_BOLETOS_METAS, DtsCotizacionVta.IdVendedorPTACrea))
                                                    _FileSRVRetailRepository._Update_FechaCierreVenta(lstFilesPTANew, IdEmpresas.INT_ID_EMPRESA_GNM_NM, DtsUsuarioLogin.IdVendedorPTA, DtsCotizacionVta.IdVendedorPTACrea, DtsUsuarioLogin.LoginUsuario, bolUATPExoneradoFirmaCliente, DtsCotizacionVta.NomCompletoUsuCrea,false);
                                                else if (_FileSRVRetailRepository._Existe_Vendedor_SubArea(Constantes_SRV.INT_ID_AREA_NMVCOM_METAS, Constantes_SRV.INT_ID_SUBAREA_NMVCOM_PERSONAL_PAQUETE, DtsCotizacionVta.IdVendedorPTACrea))
                                                    _FileSRVRetailRepository._Update_FechaCierreVenta(lstFilesPTANew, IdEmpresas.INT_ID_EMPRESA_GNM_NM, DtsUsuarioLogin.IdVendedorPTA, DtsCotizacionVta.IdVendedorPTACrea, DtsUsuarioLogin.LoginUsuario, bolUATPExoneradoFirmaCliente, DtsCotizacionVta.NomCompletoUsuCrea,false);                                                
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        string strIPUsuario = "127.0.0.0";
                                        NMailAlerta oNMailAlerta = new NMailAlerta();
                                        oNMailAlerta.EnvioCorreoRegistrarError("Error de " + Constantes_SRV.APP_NAME, this, ex, strIPUsuario + "|Actualiza_FechaCierreVenta en " + DtsCotizacionVta.IdCot);
                                        oNMailAlerta = null;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string strIPUsuario = "127.0.0.0";                       
                        NMailAlerta oNMailAlerta = new NMailAlerta();
                        oNMailAlerta.EnvioCorreoRegistrarError("Error de " + Constantes_SRV.APP_NAME, this, ex, strIPUsuario + "|Actualiza_FechaCierreVenta");
                        oNMailAlerta = null;
                    }

                    try
                    {
                        if (lstFilesPTACotVta != null)
                        {
                            foreach (FilePTACotVta objFilePTACotVta in lstFilesPTACotVta)
                            {
                                _FileSRVRetailRepository._Insert_TextoFile(objFilePTACotVta.IdFilePTA, objFilePTACotVta.IdSuc, "Código Cotización SRV: " + DtsCotizacionVta.IdCot, DtsUsuarioLogin.LoginUsuario, ConstantesWeb_Codigos.INT_ID_EMP_PTA_NMV);
                            }
                        }
                    }
                    catch (Exception ex)
                    {                       
                        string strIPUsuario = "127.0.0.0";                       
                        NMailAlerta oNMailAlerta = new NMailAlerta();
                        oNMailAlerta.EnvioCorreoRegistrarError("Error de " + Constantes_SRV.APP_NAME, this, ex, strIPUsuario + "|Inserta_TextoFile");
                        oNMailAlerta = null;
                    }

                    _responseAsociate.CodigoError = "OK";                    
                }
                else if (FileAssociate.LstFiles[0].accion_SF == Constantes_FileRetail.STR_DESASOCIAR_FILE)
                {
                    string _MsgDeleteAsocite = string.Empty;string _MsgDeleteAsociteFN = string.Empty;
                    foreach(FileSRV _fileDS in ListFile_Info)
                    {
                        _FileSRVRetailRepository._Delete_Cot_File(FileAssociate.idCotSRV_SF, _fileDS.IdFilePTA);

                        _MsgDeleteAsocite = "Usuario " + DtsUsuarioLogin.LoginUsuario + " ha eliminado el File " + _fileDS.IdFilePTA.ToString() + " del SRV " + FileAssociate.idCotSRV_SF.ToString();
                        _MsgDeleteAsociteFN = _MsgDeleteAsociteFN + _MsgDeleteAsocite;

                        _FileSRVRetailRepository._Insert(DtsUsuarioLogin.IdUsuario, Constantes_FileRetail.PAGE_DESASOCIARSRV,
                             _MsgDeleteAsocite,1, Webs_Cid.ID_WEB_WEBFAREFINDER, 
                             Constantes_FileRetail.PAGE_DESASOCIARSRV + "/PKG_COTIZACION_VTA_WFF/SP_DEL_COTIZACION_FILE", 
                             "127.0.0.0");
                    }

                    _responseAsociate.MensajeError = _MsgDeleteAsociteFN;
                    _responseAsociate.CodigoError = "OK";
                    return Ok(_responseAsociate);
                }

                return Ok(_responseAsociate);
            }
            catch (Exception ex)
            {
                ErrorAsociate = ex.Message;
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    Request = FileAssociate,
                    Response = _responseAsociate,
                    Exception = ErrorAsociate
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region Auxiliares
        private void Enviar_MailCajaWeb(int pIntIdCotVta, int pIntIdEstado, string pStrEmailCounter, UsuarioLogin _usuarioLogin)
        {
            UsuarioLogin objUsuarioSession = _usuarioLogin;
            //PasarelaPagoBO objPasarelaPagoBO = new PasarelaPagoBO();
            string strIPUsuCrea = "127.0.0.0";
            try
            {                
                List<PasarelaPago_Pedido> lstPedidos = _PedidoRetail_Repository.Get_Pedido_XSolicitud(null, null, null, null, pIntIdCotVta, null, null);

                StringBuilder sbHTMLDatosPedidos = new StringBuilder();
                FormaPagoPedido objFormaPagoPedido = null;RptaPagoEfectivoEC objRptaPagoEfectivoEC = null;RptaPagoSafetyPay objRptaPagoSafetyPay = null;

                string strLinkPago = "";
                bool bolTienePedido = false;

                sbHTMLDatosPedidos.Append("<strong>Pedidos del SRV " + pIntIdCotVta + "</strong><br /><br />" + Constants.vbCrLf);
                sbHTMLDatosPedidos.Append("<table>" + Constants.vbCrLf);
                foreach (PasarelaPago_Pedido objPasarelaPago_Pedido in lstPedidos)
                {
                    if (objPasarelaPago_Pedido.IdEstadoPedido == Constantes_Pedido.INT_ID_ESTADO_PEDIDO_PAGADO)
                    {
                        objFormaPagoPedido = objPasarelaPago_Pedido.FormaPagoPedido;
                        if (objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_PAGOEFECTIVO_EC || objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_PAGOEFECTIVO_EC_ONLINE || objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_SAFETYPAY_CASH || objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_SAFETYPAY_ONLINE)
                        {
                            sbHTMLDatosPedidos.Append("<tr>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("<td colspan='2' class='titulo_datos_pago'>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("PEDIDO NRO. " + objPasarelaPago_Pedido.IdPedido + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("<a name='pedido" + objPasarelaPago_Pedido.IdPedido + "'></a>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("</td>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("</tr>" + Constants.vbCrLf);

                            objRptaPagoEfectivoEC = objPasarelaPago_Pedido.RptaPagoEfectivoEC;
                            objRptaPagoSafetyPay = objPasarelaPago_Pedido.RptaPagoSafetyPay;

                            sbHTMLDatosPedidos.Append("<tr>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("<td colspan='2'>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("<table width='690' border='1' cellpadding='2' cellspacing='0' style='border-collapse:collapse'>" + Constants.vbCrLf);

                            sbHTMLDatosPedidos.Append("<tr>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("<td width='100'><strong>Pasarela:</strong></td>" + Constants.vbCrLf);
                            if (objFormaPagoPedido == null)
                                sbHTMLDatosPedidos.Append("<td colspan='3'>-</td>" + Constants.vbCrLf);
                            else
                            {
                                string strIdioma = "";                                
                                if (objFormaPagoPedido.IdTipoTarjeta == Constantes_FileRetail.STR_ID_TIPO_TARJETA_VISA)
                                    sbHTMLDatosPedidos.Append("<td colspan='3'><strong>Visa Online</strong> " + strIdioma + "</td>" + Constants.vbCrLf);
                                else if (objFormaPagoPedido.IdTipoTarjeta == Constantes_FileRetail.STR_ID_TIPO_TARJETA_MASTERCARD || objFormaPagoPedido.IdTipoTarjeta == Constantes_FileRetail.STR_ID_TIPO_TARJETA_MASTERCARD_CA)
                                    sbHTMLDatosPedidos.Append("<td colspan='3'><strong>Mastercard Online</strong> " + strIdioma + "</td>" + Constants.vbCrLf);
                                else if (objFormaPagoPedido.IdTipoTarjeta == Constantes_FileRetail.STR_ID_TIPO_TARJETA_DINERS)
                                    sbHTMLDatosPedidos.Append("<td colspan='3'><strong>Diners Online</strong> " + strIdioma + "</td>" + Constants.vbCrLf);
                                else if (objFormaPagoPedido.IdTipoTarjeta == Constantes_FileRetail.STR_ID_TIPO_TARJETA_AMERICAN_EXPRESS)
                                    sbHTMLDatosPedidos.Append("<td colspan='3'><strong>American Express Online</strong> " + strIdioma + "</td>" + Constants.vbCrLf);
                                else if (objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_PAGOEFECTIVO_EC || objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_PAGOEFECTIVO_EC_ONLINE)
                                {
                                    sbHTMLDatosPedidos.Append("<td colspan='3'>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("<table>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("<tr>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("<td>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("<strong>" + objFormaPagoPedido.NomFormaPago + " " + strIdioma + "</strong></td>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("<td><img src='http://www.webfarefinder.com/resources/imgs/pay/pago-efectivo-ec.jpg' alt='' width='29' height='19'/>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("</td>" + Constants.vbCrLf);
                                    if (objRptaPagoEfectivoEC != null)
                                    {
                                        sbHTMLDatosPedidos.Append("<td>&nbsp;&nbsp;</td>" + Constants.vbCrLf);
                                        sbHTMLDatosPedidos.Append("<td>CIP: " + "</td>" + Constants.vbCrLf);
                                        if (string.IsNullOrEmpty(objRptaPagoEfectivoEC.CIP))
                                            sbHTMLDatosPedidos.Append("<td><strong>No se generó el CIP</strong></td>" + Constants.vbCrLf);
                                        else
                                            sbHTMLDatosPedidos.Append("<td><strong>" + objRptaPagoEfectivoEC.CIP + "</strong></td>" + Constants.vbCrLf);
                                        if (string.IsNullOrEmpty(objRptaPagoEfectivoEC.CIP))
                                            sbHTMLDatosPedidos.Append("<td>&nbsp;</td>" + Constants.vbCrLf);
                                        else if (objRptaPagoEfectivoEC.FechaExpiraPago.HasValue)
                                        {
                                            if (objRptaPagoEfectivoEC.EstadoCIP != Constantes_SafetyPay.STR_NOM_ESTADO_CIP_CANCELADO)
                                            {
                                                sbHTMLDatosPedidos.Append("<td>&nbsp;&nbsp;</td>" + Constants.vbCrLf);
                                                string strEstiloExpirado = "";
                                                if (objRptaPagoEfectivoEC.FechaExpiraPago.Value > DateTime.Now)
                                                    strEstiloExpirado = "class='tx_autorizada'";
                                                else
                                                    strEstiloExpirado = "class='tx_denegada'";
                                                sbHTMLDatosPedidos.Append("<td>Expiración: <span " + strEstiloExpirado + ">" + objRptaPagoEfectivoEC.FechaExpiraPago.Value.ToString("dd/MM/yyyy HH:mm") + "</span></td>" + Constants.vbCrLf);
                                            }
                                        }
                                    }
                                    sbHTMLDatosPedidos.Append("<td>&nbsp;&nbsp;</td>" + Constants.vbCrLf);
                                    // sbHTMLDatosPedidos.Append("<td><a href='http://nuevomundoviajes.pe/safetypay/info-establecimientos-pago.aspx' target='_blank'>Acerca de SafetyPay</a></td>" & vbCrLf)
                                    sbHTMLDatosPedidos.Append("<td></td>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("</tr>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("</table>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("</td>" + Constants.vbCrLf);
                                }
                                else if (objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_SAFETYPAY_CASH)
                                {
                                    sbHTMLDatosPedidos.Append("<td colspan='3'>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("<table>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("<tr>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("<td>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("<strong>Efectivo " + strIdioma + "</strong></td>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("<td><img src='http://www.webfarefinder.com/resources/imgs/pay/safetypay2.jpg' alt=''/>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("</td>" + Constants.vbCrLf);

                                    if (objRptaPagoSafetyPay != null)
                                    {
                                        sbHTMLDatosPedidos.Append("<td>&nbsp;&nbsp;</td>" + Constants.vbCrLf);
                                        sbHTMLDatosPedidos.Append("<td><strong>Código de Pago:</strong></td>" + Constants.vbCrLf);
                                        if (string.IsNullOrEmpty(objRptaPagoSafetyPay.TransaccionIdentifier))
                                            sbHTMLDatosPedidos.Append("<td><strong>No se generó Código de Pago</strong></td>" + Constants.vbCrLf);
                                        else
                                            sbHTMLDatosPedidos.Append("<td><strong>" + objRptaPagoSafetyPay.TransaccionIdentifier + "</strong></td>" + Constants.vbCrLf);
                                        if (string.IsNullOrEmpty(objRptaPagoSafetyPay.ExpirationDateTime))
                                            sbHTMLDatosPedidos.Append("<td>&nbsp;</td>" + Constants.vbCrLf);
                                        else
                                        {
                                            sbHTMLDatosPedidos.Append("<td>&nbsp;&nbsp;</td>" + Constants.vbCrLf);
                                            string strExpirationDateTime = objRptaPagoSafetyPay.ExpirationDateTime;
                                            string[] arrExpirationDateTime = strExpirationDateTime.Split('(');
                                            sbHTMLDatosPedidos.Append("<td>Expiración:</td><td><span class='tx_denegada'>" + arrExpirationDateTime[0].ToString() + "</span></td>" + Constants.vbCrLf);
                                        }
                                    }
                                    sbHTMLDatosPedidos.Append("<td>&nbsp;&nbsp;</td>" + Constants.vbCrLf);
                                    // sbHTMLDatosPedidos.Append("<td><a href='http://nuevomundoviajes.pe/pago-efectivo/info-establecimientos-pago.aspx' target='_blank'>Acerca de SafetyPay</a></td>" & vbCrLf)
                                    sbHTMLDatosPedidos.Append("<td></td>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("</tr>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("</table>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("</td>" + Constants.vbCrLf);
                                }
                                else if (objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_SAFETYPAY_ONLINE)
                                {
                                    sbHTMLDatosPedidos.Append("<td colspan='3'>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("<table>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("<tr>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("<td>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("<strong>Online " + strIdioma + "</strong></td>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("<td><img src='http://www.webfarefinder.com/resources/imgs/pay/safetypay2.jpg' alt=''/>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("</td>" + Constants.vbCrLf);

                                    if (objRptaPagoSafetyPay != null)
                                    {
                                        sbHTMLDatosPedidos.Append("<td>&nbsp;&nbsp;</td>" + Constants.vbCrLf);
                                        sbHTMLDatosPedidos.Append("<td style='text-align: center;'><strong>Código de Pago:</strong></td>" + Constants.vbCrLf);
                                        if (string.IsNullOrEmpty(objRptaPagoSafetyPay.TransaccionIdentifier))
                                            sbHTMLDatosPedidos.Append("<td><strong>No se generó Código de Pago</strong></td>" + Constants.vbCrLf);
                                        else
                                            sbHTMLDatosPedidos.Append("<td><strong>" + objRptaPagoSafetyPay.TransaccionIdentifier + "</strong></td>" + Constants.vbCrLf);
                                        if (string.IsNullOrEmpty(objRptaPagoSafetyPay.ExpirationDateTime))
                                            sbHTMLDatosPedidos.Append("<td>&nbsp;</td>" + Constants.vbCrLf);
                                        else
                                        {
                                            sbHTMLDatosPedidos.Append("<td>&nbsp;&nbsp;</td>" + Constants.vbCrLf);
                                            string strExpirationDateTime = objRptaPagoSafetyPay.ExpirationDateTime;
                                            string[] arrExpirationDateTime = strExpirationDateTime.Split('(');
                                            sbHTMLDatosPedidos.Append("<td>Expiración:</td><td><span class='tx_denegada'>" + arrExpirationDateTime[0].ToString() + "</span></td>" + Constants.vbCrLf);
                                        }
                                    }
                                    sbHTMLDatosPedidos.Append("<td>&nbsp;&nbsp;</td>" + Constants.vbCrLf);
                                    // sbHTMLDatosPedidos.Append("<td><a href='http://nuevomundoviajes.pe/pago-efectivo/info-establecimientos-pago.aspx' target='_blank'>Acerca de SafetyPay</a></td>" & vbCrLf)
                                    sbHTMLDatosPedidos.Append("<td></td>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("</tr>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("</table>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("</td>" + Constants.vbCrLf);
                                }
                                else if (objPasarelaPago_Pedido.EsFonoPago)
                                    sbHTMLDatosPedidos.Append("<td colspan='3'><strong>FonoPago</strong></td>" + Constants.vbCrLf);
                                else if (objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_SOLO_PUNTOS)
                                    sbHTMLDatosPedidos.Append("<td colspan='3'><strong>Canje de Puntos</strong></td>" + Constants.vbCrLf);
                                else
                                    sbHTMLDatosPedidos.Append("<td colspan='3'><strong>-</strong></td>" + Constants.vbCrLf);
                            }
                            sbHTMLDatosPedidos.Append("</tr>" + Constants.vbCrLf);

                            sbHTMLDatosPedidos.Append("<tr>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("<td><strong>Estado::</strong></td>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("<td colspan='3'>" + Constants.vbCrLf);
                            switch (objPasarelaPago_Pedido.IdEstadoPedido)
                            {
                                case Constantes_Pedido.INT_ID_ESTADO_PEDIDO_PAGADO:
                                    {
                                        sbHTMLDatosPedidos.Append("<span class='tx_autorizada'>" + Constants.vbCrLf);
                                        break;
                                    }

                                case Constantes_Pedido.INT_ID_ESTADO_PEDIDO_PENDIENTE:
                                case Constantes_Pedido.INT_ID_ESTADO_PEDIDO_EN_PROCESO:
                                    {
                                        sbHTMLDatosPedidos.Append("<span class='tx_denegada'>" + Constants.vbCrLf);
                                        break;
                                    }

                                default:
                                    {
                                        sbHTMLDatosPedidos.Append("<span class='tx_denegada'>" + Constants.vbCrLf);
                                        break;
                                    }
                            }

                            if (objRptaPagoEfectivoEC == null)
                                sbHTMLDatosPedidos.Append(objPasarelaPago_Pedido.NomEstadoPedido + Constants.vbCrLf);
                            else if (objRptaPagoEfectivoEC.FechaCancelado.HasValue)
                            {
                                if (objRptaPagoEfectivoEC.EstadoCIP == Constantes_SafetyPay.STR_NOM_ESTADO_CIP_CANCELADO)
                                {
                                    if (objRptaPagoEfectivoEC.FechaCancelado.HasValue)
                                    {
                                        /*DateDiff(DateInterval.Minute, objRptaPagoEfectivoEC.FechaCancelado.Value, DateTime.Now)*/
                                        if (DateTime.Now.Subtract(objRptaPagoEfectivoEC.FechaCancelado.Value).TotalMinutes > 15)
                                            sbHTMLDatosPedidos.Append(objPasarelaPago_Pedido.NomEstadoPedido + " <strong>(hace m&aacute;s de 15 min).</strong>" + Constants.vbCrLf);
                                        else
                                            sbHTMLDatosPedidos.Append(objPasarelaPago_Pedido.NomEstadoPedido + " <strong>(hace " + DateTime.Now.Subtract(objRptaPagoEfectivoEC.FechaCancelado.Value).TotalMinutes + " min).</strong>" + Constants.vbCrLf);
                                    }
                                    else
                                        sbHTMLDatosPedidos.Append(objPasarelaPago_Pedido.NomEstadoPedido + Constants.vbCrLf);
                                }
                                else if (objRptaPagoEfectivoEC.EnvioRpta.HasValue)
                                    sbHTMLDatosPedidos.Append(objPasarelaPago_Pedido.NomEstadoPedido + " <img src='http://www.webfarefinder.com/resources/imgs/alarm.gif'/> <span class='texto_resaltado'>PagoEfectivo s&iacute; envi&oacute; respuesta.</span>" + Constants.vbCrLf);
                                else
                                    sbHTMLDatosPedidos.Append(objPasarelaPago_Pedido.NomEstadoPedido + Constants.vbCrLf);
                            }
                            else if (objPasarelaPago_Pedido.IdEstadoPedido == Constantes_Pedido.INT_ID_ESTADO_PEDIDO_PENDIENTE && objRptaPagoEfectivoEC.EnvioRpta.HasValue)
                                sbHTMLDatosPedidos.Append(objPasarelaPago_Pedido.NomEstadoPedido + " <img src='http://www.webfarefinder.com/resources/imgs/alarm.gif'/> <span class='texto_resaltado'>PagoEfectivo s&iacute; envi&oacute; respuesta.</span>" + Constants.vbCrLf);
                            else
                                sbHTMLDatosPedidos.Append(objPasarelaPago_Pedido.NomEstadoPedido + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("</span>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("</td>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("</tr>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("<tr>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("<td><strong>Resultado:</strong></td>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("<td colspan='3'>" + Constants.vbCrLf);

                            if (objFormaPagoPedido == null)
                                sbHTMLDatosPedidos.Append("-");
                            else if (objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_PAGOEFECTIVO_EC || objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_PAGOEFECTIVO_EC_ONLINE)
                            {
                                if (objPasarelaPago_Pedido.TipoMotor == Constantes_Pedido.ID_TIPO_PEDIDO_OTROS & objRptaPagoEfectivoEC == null)
                                    sbHTMLDatosPedidos.Append("El CIP está pendiente de pago." + Constants.vbCrLf);
                                else if (objRptaPagoEfectivoEC != null)
                                {
                                    if (objRptaPagoEfectivoEC.Estado == "1")
                                    {
                                        if (string.IsNullOrEmpty(objRptaPagoEfectivoEC.EstadoCIP))
                                            // sbHTMLDatosPedidos.Append("El estado del CIP es: <span class='tx_denegada'><strong>Generado (Pendiente)</strong></span>." & vbCrLf)
                                            sbHTMLDatosPedidos.Append("El CIP está pendiente de pago." + Constants.vbCrLf);
                                        else if (objRptaPagoEfectivoEC.EstadoCIP == Constantes_SafetyPay.STR_NOM_ESTADO_CIP_GENERADO | objRptaPagoEfectivoEC.EstadoCIP == Constantes_SafetyPay.STR_NOM_ESTADO_CIP_GENERADA)
                                        {
                                            // sbHTMLDatosPedidos.Append("El estado del CIP es: <strong>GENERADO (Pendiente)</strong>." & vbCrLf)
                                            if (objRptaPagoEfectivoEC.FechaExtorno.HasValue)
                                                sbHTMLDatosPedidos.Append("El CIP está pendiente de pago por extorno. Se extornó el : " + objRptaPagoEfectivoEC.FechaExtorno.Value.ToString("dd/MM/yyyy HH:mm") + "." + Constants.vbCrLf);
                                            else
                                                sbHTMLDatosPedidos.Append("El CIP está pendiente de pago." + Constants.vbCrLf);
                                        }
                                        else if (objRptaPagoEfectivoEC.EstadoCIP == Constantes_SafetyPay.STR_NOM_ESTADO_CIP_EXPIRADO)
                                            sbHTMLDatosPedidos.Append("El CIP ha expirado." + Constants.vbCrLf);
                                        else if (objRptaPagoEfectivoEC.EstadoCIP == Constantes_SafetyPay.STR_NOM_ESTADO_CIP_CANCELADO)
                                        {
                                            // sbHTMLDatosPedidos.Append("El estado del CIP es: <span class='tx_autorizada'><strong>" & objRptaPagoEfectivoEC.EstadoCIP.ToUpper & "</strong></span>." & vbCrLf)
                                            if (objRptaPagoEfectivoEC.FechaCancelado.HasValue)
                                                sbHTMLDatosPedidos.Append("El CIP ha sido pagado el <strong>" + objRptaPagoEfectivoEC.FechaCancelado.Value + "</strong>" + Constants.vbCrLf);
                                            else
                                                sbHTMLDatosPedidos.Append("El CIP ha sido pagado." + Constants.vbCrLf);
                                        }
                                        else
                                            sbHTMLDatosPedidos.Append("El estado del CIP es: <span class='tx_denegada'><strong>" + objRptaPagoEfectivoEC.EstadoCIP.ToLower().ToUpper() + "</strong></span>." + Constants.vbCrLf);
                                    }
                                    else
                                        sbHTMLDatosPedidos.Append("Ha ocurrido un error: " + objRptaPagoEfectivoEC.MensajeRpta + "." + Constants.vbCrLf);
                                }
                                else
                                    sbHTMLDatosPedidos.Append("No se registró el CIP." + Constants.vbCrLf);
                            }
                            else
                                sbHTMLDatosPedidos.Append("-" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("</td>");
                            sbHTMLDatosPedidos.Append("</tr>" + Constants.vbCrLf);

                            sbHTMLDatosPedidos.Append("<tr>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("<td><strong>Fecha Pedido:</strong></td>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("<td colspan='3'>" + objPasarelaPago_Pedido.FechaPedido + "</td>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("</tr>" + Constants.vbCrLf);
                            if (objPasarelaPago_Pedido.TipoMotor == Constantes_Pedido.ID_TIPO_PEDIDO_OTROS)
                            {
                                if (!objPasarelaPago_Pedido.EsFonoPago)
                                {
                                    sbHTMLDatosPedidos.Append("<tr>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("<td valign='top'><strong>Detalle:</strong></td>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("<td colspan='3'>" + objPasarelaPago_Pedido.DetalleServicio + "</td>" + Constants.vbCrLf);
                                    sbHTMLDatosPedidos.Append("</tr>" + Constants.vbCrLf);
                                }
                            }

                            sbHTMLDatosPedidos.Append("<tr>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("<td><strong>Monto a pagar:</strong></td>" + Constants.vbCrLf);

                            if (string.IsNullOrEmpty(objPasarelaPago_Pedido.IdLAValidadora))
                            {
                                if (objPasarelaPago_Pedido.MontoTarjeta.HasValue)
                                {
                                    if (objRptaPagoSafetyPay != null)
                                    {
                                        // Se retira validación, ya que se debe considerar el monto del pedido tal cual
                                        // Dim objRptaPagoSafetyPayTmp As RptaPagoSafetyPay = objPasarelaPagoBO.Obtiene_Rpta_SafetyPay(objPasarelaPago_Pedido.IdPedido)
                                        double dblMontoPagarTmp = objPasarelaPago_Pedido.MontoTarjeta.Value;
                                        // If objRptaPagoSafetyPayTmp.lst_AmountType IsNot Nothing Then
                                        // For Each objAmountType As NuevoMundoUtility.AmountType In objRptaPagoSafetyPayTmp.lst_AmountType
                                        // If objAmountType.CurrencyID = "150" Then
                                        // dblMontoPagarTmp = objAmountType.Value
                                        // Exit For
                                        // End If
                                        // Next
                                        // End If
                                        sbHTMLDatosPedidos.Append("<td colspan='3'><strong>" + dblMontoPagarTmp.ToString("#0.00") + "</strong></td>" + Constants.vbCrLf);
                                    }
                                    else
                                        sbHTMLDatosPedidos.Append("<td colspan='3'><strong>" + objPasarelaPago_Pedido.MontoTarjeta.Value.ToString("#0.00") + "</strong></td>" + Constants.vbCrLf);
                                }
                                else
                                    sbHTMLDatosPedidos.Append("<td colspan='3'><strong>-</strong></td>" + Constants.vbCrLf);
                            }
                            else
                            {
                                if (objPasarelaPago_Pedido.MontoTarjeta.HasValue)
                                    sbHTMLDatosPedidos.Append("<td width='200'><strong>" + objPasarelaPago_Pedido.MontoTarjeta.Value.ToString("#0.00") + "</strong></td>" + Constants.vbCrLf);
                                else
                                    sbHTMLDatosPedidos.Append("<td width='200'><strong>-</strong></td>" + Constants.vbCrLf);
                                sbHTMLDatosPedidos.Append("<td width='150'><strong>Línea Aérea Validadora:</strong></td>" + Constants.vbCrLf);
                                sbHTMLDatosPedidos.Append("<td>" + objPasarelaPago_Pedido.IdLAValidadora + "</td>" + Constants.vbCrLf);
                            }
                            sbHTMLDatosPedidos.Append("</tr>" + Constants.vbCrLf);

                            if (objPasarelaPago_Pedido.TipoMotor == Constantes_Pedido.ID_TIPO_PEDIDO_OTROS)
                            {
                                if (objFormaPagoPedido != null)
                                {
                                    if (!objPasarelaPago_Pedido.EsFonoPago && objFormaPagoPedido.IdFormaPago != Constantes_FileRetail.INT_ID_FORMA_PAGO_PAGOEFECTIVO_EC && objFormaPagoPedido.IdFormaPago != Constantes_FileRetail.INT_ID_FORMA_PAGO_PAGOEFECTIVO_EC_ONLINE && objFormaPagoPedido.IdFormaPago != Constantes_FileRetail.INT_ID_FORMA_PAGO_SAFETYPAY_CASH && objFormaPagoPedido.IdFormaPago != Constantes_FileRetail.INT_ID_FORMA_PAGO_SAFETYPAY_ONLINE && objFormaPagoPedido.IdFormaPago != Constantes_FileRetail.INT_ID_FORMA_PAGO_SAFETYPAY_INTERNAC)
                                    {
                                        sbHTMLDatosPedidos.Append("<tr>" + Constants.vbCrLf);
                                        sbHTMLDatosPedidos.Append("<td><strong>Link Pago:</strong></td>" + Constants.vbCrLf);
                                        /*strLinkPago = Obtiene_LinkPago(objPasarelaPago_Pedido.IdWeb, objPasarelaPago_Pedido.IdPedido, objPasarelaPago_Pedido.IdCotSRV.Value);*/
                                        sbHTMLDatosPedidos.Append("<td colspan='3'><a href='" + strLinkPago + "' target='_blank'>" + strLinkPago + "</a></td>" + Constants.vbCrLf);
                                        sbHTMLDatosPedidos.Append("</tr>" + Constants.vbCrLf);
                                    }
                                }
                            }

                            sbHTMLDatosPedidos.Append("<tr>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("<td width='95'><strong>Forma de Pago:</strong></td>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("<td width='180'>" + Constants.vbCrLf);
                            if (objFormaPagoPedido == null)
                                sbHTMLDatosPedidos.Append("-");
                            else
                                sbHTMLDatosPedidos.Append("<strong>" + objFormaPagoPedido.NomFormaPago + "</strong>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("</td>" + Constants.vbCrLf);

                            sbHTMLDatosPedidos.Append("<td width='125'></td>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("<td width='280'>" + Constants.vbCrLf);    
                            
                            sbHTMLDatosPedidos.Append("</td>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("</tr>" + Constants.vbCrLf);
                           
                            sbHTMLDatosPedidos.Append("</table>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("</td>" + Constants.vbCrLf);
                            sbHTMLDatosPedidos.Append("</tr>" + Constants.vbCrLf);
                            bolTienePedido = true;
                        }
                    }
                }
                sbHTMLDatosPedidos.Append("</table>" + Constants.vbCrLf);

                if (bolTienePedido)
                {
                    _CotizacionSRV_Repository = new CotizacionSRV_AW_Repository();
                    _FileSRVRetailRepository = new FileSRVRetailRepository();
                    List<FilePTACotVta> lstFilesPTA = _CotizacionSRV_Repository._SelectFilesPTABy_IdCot(pIntIdCotVta,0,0,0);

                    StringBuilder sbListado = new StringBuilder();
                    sbListado.Append("<strong>Comprobantes y Boletos por File</strong><br /><br />" + Constants.vbCrLf);
                    sbListado.Append("<table cellspacing='0' cellpadding='4' border='0' style='border-collapse:collapse;'>" + Constants.vbCrLf);
                    sbListado.Append("<tr style='color:White;background-color:#1F5ABD;font-weight:bold;'>" + Constants.vbCrLf);
                    sbListado.Append("<td class='dato_comp'>File</td>" + Constants.vbCrLf);
                    sbListado.Append("<td class='dato_comp'>Sucursal</td>" + Constants.vbCrLf);
                    sbListado.Append("<td class='dato_comp'>Comprobante</td>" + Constants.vbCrLf);
                    sbListado.Append("<td class='dato_comp'>Nro. Boleto</td>" + Constants.vbCrLf);
                    sbListado.Append("</tr>" + Constants.vbCrLf);

                    string strLinkBoletoHTML = "";
                    string strFileAnterior = "";
                    string strComprobanteAnterior = "";
                    Int16 intX;
                    string strEstiloFile = "";
                    string strEstiloComprobante = "";
                    bool bolExistenComprobantesBoletos = false;

                    foreach (FilePTACotVta objFilePTA in lstFilesPTA)
                    {
                        strFileAnterior = "";
                        strComprobanteAnterior = "";
                        intX = 1;
                        using (DataTable dtComprobantesBoletos = _FileSRVRetailRepository._Get_ComprobantesBoletosBy_IdFile(objFilePTA.IdFilePTA, objFilePTA.IdSuc))
                        {
                            foreach (DataRow drComprobantesBoletos in dtComprobantesBoletos.Rows)
                            {
                                sbListado.Append("<tr>" + Constants.vbCrLf);
                                if (!bolExistenComprobantesBoletos)
                                    bolExistenComprobantesBoletos = true;
                                if (strFileAnterior != objFilePTA.IdFilePTA.ToString() + objFilePTA.IdSuc.ToString())
                                {
                                    if (intX >= dtComprobantesBoletos.Rows.Count)
                                        strEstiloFile = "dato_comp_ultimo1";
                                    else
                                        strEstiloFile = "dato_file";
                                    sbListado.Append("<td class='" + strEstiloFile + "'>" + objFilePTA.IdFilePTA + "</td>" + Constants.vbCrLf);
                                    sbListado.Append("<td class='" + strEstiloFile + "'>" + objFilePTA.NombreSucursal + "</td>" + Constants.vbCrLf);
                                    strFileAnterior = objFilePTA.IdFilePTA.ToString() + objFilePTA.IdSuc.ToString();
                                }
                                else
                                {
                                    if (intX >= dtComprobantesBoletos.Rows.Count)
                                        strEstiloFile = "dato_comp_ultimo2";
                                    else
                                        strEstiloFile = "dato_file_leftright";
                                    sbListado.Append("<td class='" + strEstiloFile + "'></td>" + Constants.vbCrLf);
                                    sbListado.Append("<td class='" + strEstiloFile + "'></td>" + Constants.vbCrLf);
                                }
                                if (strComprobanteAnterior != drComprobantesBoletos["ID_TIPO_DE_COMPROBANTE"].ToString() + drComprobantesBoletos["ID_FACTURA_CABEZA"].ToString() + drComprobantesBoletos["ID_SUCURSAL"].ToString() + drComprobantesBoletos["NUMERO_SERIE"].ToString())
                                {
                                    if (intX >= dtComprobantesBoletos.Rows.Count)
                                        strEstiloComprobante = "dato_comp_ultimo1";
                                    else
                                        strEstiloComprobante = "dato_comp";
                                    // pStrTipoComp & "-" & pStrNroSerie.PadLeft(3, "0") & "-" & pStrNroComp
                                    sbListado.Append("<td class='" + strEstiloComprobante + "'>" + Constants.vbCrLf);
                                    sbListado.Append(drComprobantesBoletos["ID_TIPO_DE_COMPROBANTE"].ToString() + "-" + drComprobantesBoletos["NUMERO_SERIE"].ToString() + "-" + drComprobantesBoletos["ID_FACTURA_CABEZA"].ToString() + Constants.vbCrLf);
                                    sbListado.Append("</td>" + Constants.vbCrLf);

                                    strComprobanteAnterior = drComprobantesBoletos["ID_TIPO_DE_COMPROBANTE"].ToString() + drComprobantesBoletos["ID_FACTURA_CABEZA"].ToString() + drComprobantesBoletos["ID_SUCURSAL"].ToString() + drComprobantesBoletos["NUMERO_SERIE"].ToString();
                                }
                                else
                                {
                                    if (intX >= dtComprobantesBoletos.Rows.Count)
                                        strEstiloComprobante = "dato_comp_ultimo2";
                                    else
                                        strEstiloComprobante = "dato_comp_leftright";
                                    sbListado.Append("<td class='" + strEstiloComprobante + "'></td>" + Constants.vbCrLf);
                                }

                                if (Convert.IsDBNull(drComprobantesBoletos["PNR_CODE"]))
                                    strLinkBoletoHTML = drComprobantesBoletos["NRO_BOLETO"].ToString();
                                else
                                    strLinkBoletoHTML = drComprobantesBoletos["NRO_BOLETO"].ToString();
                                sbListado.Append("<td class='dato_boleto'>" + strLinkBoletoHTML + "</td>" + Constants.vbCrLf);
                                sbListado.Append("</tr>" + Constants.vbCrLf);
                                intX += 1;
                            }
                        }
                    }
                    sbListado.Append("</table>" + Constants.vbCrLf);

                    int intIdWeb = Webs_Cid.ID_WEB_WEBFAREFINDER;
                    Int16 intIdLang = Lang_Cid.IdLangSpa;

                    if (bolExistenComprobantesBoletos)
                    {
                        EnviarCorreo objEnviarCorreo = new EnviarCorreo();
                        objEnviarCorreo.Enviar_CajaWeb(intIdWeb, intIdLang, 128, sbListado.ToString() + "<br /><br />" + sbHTMLDatosPedidos.ToString(), "Pedidos pagados del SRV " + pIntIdCotVta, pStrEmailCounter);
                    }
                    else
                    {
                        EnviarCorreo objEnviarCorreo = new EnviarCorreo();
                        objEnviarCorreo.Enviar_CajaWeb(intIdWeb, intIdLang, 128, sbHTMLDatosPedidos.ToString(), "Pedidos pagados del SRV " + pIntIdCotVta, pStrEmailCounter);
                    }
                }
            }
            catch (Exception ex)
            {
                NMailAlerta oNMailAlerta = new NMailAlerta();
                oNMailAlerta.EnvioCorreoRegistrarError("Error de " + Constantes_SRV.APP_NAME, this, ex, "127.0.0.0" + "|Enviar_MailCajaWeb - SRV ");
                oNMailAlerta = null;
            }
        }


        private void validacionAssociate(ref AssociateFile _fileAssociate, ref AssociateFileRS _responseFile, ref UsuarioLogin UserLogin, ref List<FileSRV> ListFile_InfoSRV)
        {
            string mensajeError = string.Empty;            

            if (_fileAssociate.idCotSRV_SF <= 0)
            {
                mensajeError += "Envie el codigo de SRV|";
            }
            if (_fileAssociate.idoportunidad_SF == null)
            {
                mensajeError += "Envie el codigo de Oportunidad|";
            }
            if (_fileAssociate.LstFiles == null)
            {
                mensajeError += "Envie la lista de files a asociar/desasociar|";
            }
            else if (_fileAssociate.LstFiles.Count > 3)
            {
                mensajeError += "El maximo de files asociar es 3|";
            }
            else
            {
                int posListFile = 0;
                FileSRV _fileSRV_Info = null;
                DataTable _dtfilesAsociadosSRV = null;
                foreach (FileSRVRQ fileSRV in _fileAssociate.LstFiles) /*Aqui se tendra que validar si existe registro del File con la Sucursal asociadas al vendedor (configurado en base al UsuarioId)*/
                {
                    if (fileSRV == null)
                    {
                        mensajeError += "Envie el registro " + posListFile + " de la lista de files|";
                        break;
                    }
                    if (fileSRV.IdFilePTA <= 0)
                    {
                        mensajeError += "Envie el IdFile del registro " + posListFile + " de la lista de files|";
                        break;
                    }
                    if (fileSRV.Sucursal <= 0)
                    {
                        mensajeError += "Envie la SucursalId del registro " + posListFile + " de la lista de files|";
                        break;
                    }
                    if (string.IsNullOrEmpty(mensajeError) == false) { break; }

                    /*Realizamos la conexion a la BD*/
                    if (string.IsNullOrEmpty(fileSRV.accion_SF))
                    {
                        mensajeError += "Envie la accion a realizar del file " + posListFile + " de la lista de files|";
                        break;
                    }
                    else if(fileSRV.accion_SF == Constantes_FileRetail.STR_ASOCIAR_FILE)
                    {
                        if(posListFile > 0 && _fileAssociate.LstFiles[0].accion_SF != fileSRV.accion_SF)
                        {
                            mensajeError += "La accion debe ser la misma en todos los files|";
                            break;
                        }
                        _fileSRV_Info = new FileSRV();
                        _fileSRV_Info = CargarInfoFile((int)fileSRV.Sucursal, fileSRV.IdFilePTA);
                    }
                    else if (fileSRV.accion_SF == Constantes_FileRetail.STR_DESASOCIAR_FILE)
                    {
                        if (posListFile > 0 && _fileAssociate.LstFiles[0].accion_SF != fileSRV.accion_SF)
                        {
                            mensajeError += "La accion debe ser la misma en todos los files|";
                            break;
                        }

                        if (posListFile == 0)
                        {
                            _dtfilesAsociadosSRV = _FileSRVRetailRepository._SelectFilesIdBy_IdCot(_fileAssociate.idCotSRV_SF);
                        }

                        _fileSRV_Info = null;
                        if (_dtfilesAsociadosSRV != null && _dtfilesAsociadosSRV.Rows.Count > 0)
                        {                            
                            foreach (DataRow row in _dtfilesAsociadosSRV.Rows)
                            {
                                if(row.IntParse("FILE_ID") == fileSRV.IdFilePTA && row.IntParse("SUC_ID") == fileSRV.Sucursal)
                                {
                                    _fileSRV_Info = new FileSRV() {
                                        IdFilePTA = row.IntParse("FILE_ID"),
                                        Sucursal = row.IntParse("SUC_ID")
                                    };                                    
                                }
                            }                            
                        }
                        else
                        {
                            mensajeError += "No existe asociacion del SRV " + _fileAssociate.idCotSRV_SF.ToString() + " con el file " + fileSRV.IdFilePTA.ToString() + "|";
                            break;
                        }                       
                    }
                    else if(fileSRV.accion_SF != Constantes_FileRetail.STR_DESASOCIAR_FILE)
                    {
                        mensajeError += "Accion a realizar no soportada del file " + posListFile + " de la lista de files|";
                        break;
                    }
                                        
                    if (_fileSRV_Info != null)
                    {
                        if(ListFile_InfoSRV == null) { ListFile_InfoSRV = new List<FileSRV>(); }
                        ListFile_InfoSRV.Add(_fileSRV_Info);
                        //if (_fileSRV_Info.ImporteFact > 0)
                        //{
                        //    mensajeError += "No se tiene el importe del registro " + posListFile + " de la lista de files a asociar|";
                        //}
                        //if (string.IsNullOrEmpty(_fileSRV_Info.Cliente))
                        //{
                        //    mensajeError += "No se tiene el cliente del registro " + posListFile + " de la lista de files a asociar|";
                        //}
                        //if (string.IsNullOrEmpty(_fileSRV_Info.Moneda))
                        //{
                        //    mensajeError += "No se tiene la moneda del registro " + posListFile + " de la lista de files a asociar|";
                        //}
                    }
                    else
                    {
                        mensajeError += "No hay informacion/asociacion con los datos proporcionados del File " + posListFile + "|";
                        break;
                    }

                    if (string.IsNullOrEmpty(mensajeError) == false) { break; }
                    posListFile++;
                }
            }

            if (_fileAssociate.idUsuario > 0)
            {
                /*Cargamos Datos del Usuario*/
                RepositoryByBusiness(null);
                UserLogin = _datosUsuario.Get_Dts_Usuario_Personal(Convert.ToInt32(_fileAssociate.idUsuario));
                if (UserLogin == null) { mensajeError += "ID del Usuario no registrado|"; }
            }
            else
            {
                mensajeError += "Envie el ID del Usuario correctamente|";
            }

            if (string.IsNullOrEmpty(mensajeError) == false)
            {
                _responseFile.CodigoError = "ER";
                _responseFile.MensajeError = "VA " + mensajeError;
            }
        }

        private FileSRV CargarInfoFile(int sucursal, int idfile)
        {
            _CotizacionSRV_Repository = new CotizacionSRV_AW_Repository();
            FileSRV fileSRV = new FileSRV();
            string strIdFile = Convert.ToString(idfile);
         
            strIdFile = strIdFile.Replace(",", "");
            DataTable dtImporteFile;
            dtImporteFile = _CotizacionSRV_Repository._Select_InfoFile(sucursal, idfile);
           
            if (dtImporteFile == null){}
            else if (dtImporteFile.Rows.Count == 0){}
            else
            {
                DataRow drCliente = dtImporteFile.Rows[0];
                double dblImporteSumaUSD = 0;
                double dblImporteSumaSOL = 0;
                string cliente = "";
                double monto = 0;
                string moneda = "";                
                string strIdMoneda = drCliente["ID_MONEDA"].ToString();

                foreach (DataRow drImporteFile in dtImporteFile.Rows)
                {
                    if (drImporteFile["ID_MONEDA"] != null)
                    {
                        if (drImporteFile["ID_MONEDA"].ToString() == "USD")
                        {
                            if (drImporteFile["FLAG"].ToString() == "1")
                            {
                                dblImporteSumaUSD += (double)drImporteFile["IMPORTE_TOTAL"];
                            }
                        }
                        else
                        {
                            if (drImporteFile["FLAG"].ToString() == "1")
                            {
                                dblImporteSumaSOL += (double)drImporteFile["IMPORTE_TOTAL"];
                            }
                        }
                    }
                }
                                
                cliente = drCliente["NOMBRE_CLIENTE"].ToString();

                if (strIdMoneda == "USD")
                {
                    monto = dblImporteSumaUSD;
                }
                else
                {
                    monto = dblImporteSumaSOL;
                }
                                
                moneda = drCliente["ID_MONEDA"].ToString();
                /*string fecha = Convert.ToDateTime(drCliente["FECHA_EMISION"]).ToShortDateString();*/
                fileSRV.Cliente = cliente;
                fileSRV.ImporteFact = monto;
                fileSRV.Moneda = moneda;
                fileSRV.IdFilePTA = idfile;
                fileSRV.Sucursal = sucursal;
            }

            return fileSRV;
        }
        #endregion
    }
}
