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

namespace Expertia.Estructura.Controllers
{
    [RoutePrefix(RoutePrefix.FileRetail)]
    public class FileRetailController : BaseController<object>
    {
        #region Properties               
        private IDatosUsuario _datosUsuario;
        private ICotizacionSRV_Repository _CotizacionSRV_Repository;
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
            CotizacionSRV_AW_Repository _CotizacionSRV_Repository = new CotizacionSRV_AW_Repository();

            List<FilePTACotVta> lstFilesPTACotVta = null;
            List<FilePTACotVta> lstFilesPTAOld = null;

            ArrayList lstFechasCotVta = new ArrayList(); /*Duda*/

            /*Se realizara el cambio de Estado a Facturado*/
            Int16 EstadoSeleccionado = (Int16)ENUM_ESTADOS_COT_VTA.Facturado;
            bool bolCambioEstado = true;

            /*Datos que se quitaran, solo lo agregamos para tener una mejor vision*/
            //bool bolEsCounterAdministrativo = objUsuarioSession.EsCounterAdminSRV;

            string ErrorAsociate = string.Empty;
            try
            {
                /*Validaciones*/
                validacionAssociate(ref FileAssociate, ref _responseAsociate, ref DtsUsuarioLogin);
                if (string.IsNullOrEmpty(_responseAsociate.CodigoError) == false) return Ok(_responseAsociate);

                UsuarioLogin objUsuarioLogin = _datosUsuario.Get_Dts_Usuario_Personal(FileAssociate.idUsuario);

                /*Cargar InfoFile*/

                FileSRV inforFile = CargarInfoFile((int)FileAssociate.LstFiles[0].Sucursal, FileAssociate.LstFiles[0].IdFilePTA);

                /*Obtenemos los datos del SRV, etc*/
                DtsCotizacionVta = _CotizacionSRV_Repository.Get_Datos_CotizacionVta(FileAssociate.idCotSRV_SF);

                if (FileAssociate.LstFiles[0].IdFilePTA == 896798)
                {
                    lstFilesPTACotVta = new List<FilePTACotVta>();
                    FilePTACotVta _FilePTACotVta = null;

                    foreach (FileSRV fileSRV in FileAssociate.LstFiles)
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
                        }
                    }
                    /*Logica si tiene asociado un servicio*/

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
                }

                /*Logica RC*/
                bool bolGeneroRC_OK = false;
                try
                {
                    bool bolGenerarRC = false;
                    StringBuilder sbDatosGeneraRC = new StringBuilder();

                    if (bolCambioEstado && (DtsUsuarioLogin.EsCounterAdminSRV == true ? DtsCotizacionVta.IdOfi : DtsUsuarioLogin.IdOfi) == 62)
                    {
                        List<PasarelaPago_Pedido> lstPedidos = _PedidoRetail_Repository.Get_Pedido_XSolicitud(null, null, null, null, FileAssociate.idCotSRV_SF, null, null);

                        /*********VARIABLES POR REVISAR Y TRATAR********/
                        double dblMontoPedido = 0;
                        double dblMontoPedidoRound = 0;
                        string strNroTarjeta = "";
                        string strNroTarjetaBaneado = "";
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

                        //foreach (PasarelaPago_Pedido objPasarelaPago_Pedido in lstPedidos)
                        //{
                        //    FormaPagoPedido objFormaPagoPedido = _PedidoRetail_Repository.Get_FormaPagoBy_IdPedido(objPasarelaPago_Pedido.IdPedido);

                        //    /*********CODIGO POR REVISAR Y TRATAR********/
                        //    if (objPasarelaPago_Pedido.IdEstadoPedido == Constantes_FileRetail.INT_ID_ESTADO_PEDIDO_VALIDADO || objPasarelaPago_Pedido.IdEstadoPedido == Constantes_FileRetail.INT_ID_ESTADO_PEDIDO_PAGADO)
                        //    {
                        //        if (objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_SAFETYPAY_ONLINE || objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_SAFETYPAY_CASH || objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_PAGOEFECTIVO_EC || objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_PAGOEFECTIVO_EC_ONLINE || objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_SOLO_TARJETA)
                        //        {
                        //            dblMontoPedido = objPasarelaPago_Pedido.MontoTarjeta;
                        //            dblMontoPedidoRound = objPasarelaPago_Pedido.MontoTarjeta;

                        //            if (objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_SAFETYPAY_ONLINE || objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_SAFETYPAY_CASH)
                        //            {
                        //                strIdForma = Constantes_FileRetail.STR_ID_FORMA_PTA_SAFETYPAY;
                        //                strIdValor = Constantes_FileRetail.STR_ID_VALOR_PTA_USD;
                        //                if (objPasarelaPago_Pedido.IdEstadoPedido == Constantes_FileRetail.INT_ID_ESTADO_PEDIDO_VALIDADO || objPasarelaPago_Pedido.IdEstadoPedido == Constantes_FileRetail.INT_ID_ESTADO_PEDIDO_PAGADO)
                        //                    bolGenerarRC = true;
                        //            }
                        //            else if (objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_PAGOEFECTIVO_EC || objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_PAGOEFECTIVO_EC_ONLINE)
                        //            {
                        //                strIdForma = Constantes_FileRetail.STR_ID_FORMA_PTA_PAGOEFECTIVO;
                        //                strIdValor = Constantes_FileRetail.STR_ID_VALOR_PTA_USD;
                        //                if (objPasarelaPago_Pedido.IdEstadoPedido == Constantes_FileRetail.INT_ID_ESTADO_PEDIDO_VALIDADO || objPasarelaPago_Pedido.IdEstadoPedido == Constantes_FileRetail.INT_ID_ESTADO_PEDIDO_PAGADO)
                        //                    bolGenerarRC = true;
                        //            }
                        //            else if (objFormaPagoPedido.IdFormaPago == Constantes_FileRetail.INT_ID_FORMA_PAGO_SOLO_TARJETA)
                        //            {
                        //                if (objPasarelaPago_Pedido.IdEstadoPedido == Constantes_FileRetail.INT_ID_ESTADO_PEDIDO_VALIDADO || objPasarelaPago_Pedido.IdEstadoPedido == Constantes_FileRetail.INT_ID_ESTADO_PEDIDO_PAGADO)
                        //                    bolGenerarRC = false;
                        //                if (objFormaPagoPedido.IdTipoTarjeta == Constantes_FileRetail.STR_ID_TIPO_TARJETA_VISA)
                        //                {
                        //                    strIdForma = Constantes_FileRetail.STR_ID_FORMA_PTA_VISA;
                        //                    strIdValor = Constantes_FileRetail.STR_ID_VALOR_PTA_UATP;
                        //                }
                        //                else if (objFormaPagoPedido.IdTipoTarjeta == Constantes_FileRetail.STR_ID_TIPO_TARJETA_MASTERCARD || objFormaPagoPedido.IdTipoTarjeta == Constantes_FileRetail.STR_ID_TIPO_TARJETA_MASTERCARD_CA)
                        //                {
                        //                    strIdForma = Constantes_FileRetail.STR_ID_FORMA_PTA_MASTERCARD;
                        //                    strIdValor = Constantes_FileRetail.STR_ID_VALOR_PTA_UATP;
                        //                }
                        //                else if (objFormaPagoPedido.IdTipoTarjeta == Constantes_FileRetail.STR_ID_TIPO_TARJETA_AMERICAN_EXPRESS)
                        //                {
                        //                    strIdForma = Constantes_FileRetail.STR_ID_FORMA_PTA_AMERICAN;
                        //                    strIdValor = Constantes_FileRetail.STR_ID_VALOR_PTA_UATP;
                        //                }
                        //                else if (objFormaPagoPedido.IdTipoTarjeta == Constantes_FileRetail.STR_ID_TIPO_TARJETA_DINERS)
                        //                {
                        //                    strIdForma = Constantes_FileRetail.STR_ID_FORMA_PTA_DINERS;
                        //                    strIdValor = Constantes_FileRetail.STR_ID_VALOR_PTA_UATP;
                        //                }
                        //            }

                        //            if (ConfigAccess.GetValueInAppSettings("STR_GENERAR_RC_PTA").Equals("1"))
                        //            {
                        //                if (bolGenerarRC && bolExisteSoloUnPedidoPagado && objPasarelaPago_Pedido.IdCotSRV.HasValue && lstFilesPTACotVta.Count > 0)
                        //                {
                        //                    CotizacionVta objCotizacionVta = objCotizacionVentaBO.ObtieneCotizacionXId(objPasarelaPago_Pedido.IdCotSRV.Value);
                        //                    ReservaVuelosBO objReservaVuelosBO = new ReservaVuelosBO();
                        //                    string strIdCiudadTmp = "";

                        //                    if (objCotizacionVta.IdReservaVuelos.HasValue)
                        //                    {
                        //                        // GENERAR RC SOLO PARA LOS DESTINOS QUE NO SON PUERTO MALDONADO, NI IQUITOS
                        //                        try
                        //                        {
                        //                            using (DataSet dtsReserva = objReservaVuelosBO.ObtieneReservaXId(objCotizacionVta.IdReservaVuelos.Value))
                        //                            {
                        //                                using (DataTable dtReserva = dtsReserva.Tables("RESERVA"))
                        //                                {
                        //                                    if (dtReserva.Rows.Count > 0)
                        //                                    {
                        //                                        DataRow drReserva = dtReserva.Rows(0);
                        //                                        if (!IsDBNull(drReserva("ES_NEGOCIADA")) && drReserva("ES_NEGOCIADA").ToString == "0")
                        //                                            bolEsTarifaPublicada = true;
                        //                                    }
                        //                                }
                        //                                using (DataTable dtSegementos = dtsReserva.Tables("SEGMENTO"))
                        //                                {
                        //                                    foreach (DataRow drSegmento in dtSegementos.Rows)
                        //                                    {
                        //                                        if (!IsDBNull(drSegmento("CIT_CIDSRC")))
                        //                                            strIdCiudadTmp = drSegmento("CIT_CIDSRC");
                        //                                        DateTime datFecActual = DateTime.Now;
                        //                                        if ((DateTime)datFecActual.ToString("dd/MM/yyyy") >= (DateTime)"01/01/2019")
                        //                                        {
                        //                                            if (strIdCiudadTmp.ToUpper().Contains("PEM"))
                        //                                            {
                        //                                                bolEsRutaSelva = true;
                        //                                                // fcondor: se comenta para generar RC para ruta selva
                        //                                                // bolGenerarRC = False 
                        //                                                bolGenerarRC = true;
                        //                                                break;
                        //                                            }
                        //                                        }
                        //                                        else if (strIdCiudadTmp.ToUpper().Contains("PEM") || strIdCiudadTmp.ToUpper().Contains("IQT"))
                        //                                        {
                        //                                            bolEsRutaSelva = true;
                        //                                            // fcondor: se comenta para generar RC para ruta selva
                        //                                            // bolGenerarRC = False 
                        //                                            bolGenerarRC = true;
                        //                                            break;
                        //                                        }



                        //                                        if (!IsDBNull(drSegmento("CIT_CIDTGT")))
                        //                                            strIdCiudadTmp = drSegmento("CIT_CIDTGT");
                        //                                        if ((DateTime)datFecActual.ToString("dd/MM/yyyy") >= (DateTime)"01/01/2019")
                        //                                        {
                        //                                            if (strIdCiudadTmp.ToUpper().Contains("PEM"))
                        //                                            {
                        //                                                bolEsRutaSelva = true;
                        //                                                // fcondor: se comenta para generar RC para ruta selva
                        //                                                // bolGenerarRC = False
                        //                                                bolGenerarRC = true;
                        //                                                break;
                        //                                            }
                        //                                        }
                        //                                        else if (strIdCiudadTmp.ToUpper().Contains("PEM") || strIdCiudadTmp.ToUpper().Contains("IQT"))
                        //                                        {
                        //                                            bolEsRutaSelva = true;
                        //                                            // fcondor: se comenta para generar RC para ruta selva
                        //                                            // bolGenerarRC = False
                        //                                            bolGenerarRC = true;
                        //                                            break;
                        //                                        }
                        //                                    }
                        //                                }
                        //                            }
                        //                        }
                        //                        catch (Exception ex)
                        //                        {
                        //                            string strIPUsuario = "";
                        //                            if (Request.ServerVariables("HTTP_X_FORWARDED_FOR") != null)
                        //                                strIPUsuario = Request.ServerVariables("HTTP_X_FORWARDED_FOR");
                        //                            else
                        //                                strIPUsuario = Request.ServerVariables("REMOTE_ADDR");
                        //                            // 'PROYALERTA
                        //                            NMailAlerta oNMailAlerta = new NMailAlerta();
                        //                            oNMailAlerta.EnvioCorreoRegistrarError("Error de " + ConstantesWeb.APP_NAME, this, ex, strIPUsuario + "|Genera RC en PTA - ObtieneReservaXId");
                        //                            oNMailAlerta = null/* TODO Change to default(_) if this is not a reference type */;
                        //                        }
                        //                    }

                        //                    if (bolGenerarRC)
                        //                    {
                        //                        // GENERAR RC SOLO PARA LOS DESTINOS QUE NO SON PUERTO MALDONADO, NI IQUITOS
                        //                        // fcondor: se comento para generar RC ruta selva
                        //                        // If objCotizacionVta.DestinosPref.ToUpper.Contains("PEM") OrElse _
                        //                        // objCotizacionVta.CodigoIATAPrincipal.ToUpper.Contains("PEM") OrElse _
                        //                        // objCotizacionVta.DestinosPref.ToUpper.Contains("IQT") OrElse _
                        //                        // objCotizacionVta.CodigoIATAPrincipal.ToUpper.Contains("IQT") Then
                        //                        // Dim objEnviarCorreo As New EnviarCorreo
                        //                        // objEnviarCorreo.Enviar_Log("Log: no se generó RC", sbDatosGeneraRC.ToString() & _
                        //                        // "<br><br><strong>Error: ruta selva<BR>intIdEstadoSelect:" & intIdEstadoSelect & "<br>bolCambioEstado=" & bolCambioEstado & "</strong>", True, System.Configuration.ConfigurationManager.AppSettings("STR_PATH_FILES_RC_WEB"), _
                        //                        // "RC_NO_OK_SRV_" & intIdCotVta)
                        //                        // Exit For
                        //                        // Else
                        //                        // lreque: se comenta para que se respete el monto tal cual se cobró al cliente
                        //                        // If objPasarelaPago_Pedido.Pedido_RptaPagoSafetyPay IsNot Nothing Then
                        //                        // Dim objRptaPagoSafetyPayTmp As RptaPagoSafetyPay = objPasarelaPagoBO.Obtiene_Rpta_SafetyPay(objPasarelaPago_Pedido.IdPedido)
                        //                        // Dim dblMontoPagarTmp As Double = objPasarelaPago_Pedido.MontoTarjeta.Value
                        //                        // If objRptaPagoSafetyPayTmp.lst_AmountType IsNot Nothing Then
                        //                        // For Each objAmountType As NuevoMundoUtility.AmountType In objRptaPagoSafetyPayTmp.lst_AmountType
                        //                        // If objAmountType.CurrencyID = "150" Then
                        //                        // dblMontoPedidoRound = objAmountType.Value
                        //                        // Exit For
                        //                        // End If
                        //                        // Next
                        //                        // End If
                        //                        // End If
                        //                        if (objPasarelaPago_Pedido.Pedido_RptaPagoUATP != null)
                        //                            strNroTarjetaBaneado = objNMWebUtility.Obtiene_NroTarjeta_Baneado(objPasarelaPago_Pedido.Pedido_RptaPagoUATP.NroTarjeta);

                        //                        foreach (FilePTACotVta objFilePTACotVtaTmp in lstFilesPTACotVta)
                        //                            sbDatosGeneraRC.Append("File: " + objFilePTACotVtaTmp.IdFilePTA + " - " + objFilePTACotVtaTmp.IdSucursal + " / Monto: " + objFilePTACotVtaTmp.Moneda + " - " + objFilePTACotVtaTmp.ImporteFacturado + " <br>");
                        //                        sbDatosGeneraRC.Append("EsUATP: " + objPasarelaPago_Pedido.EsUATP + "<br>" + Constants.vbCrLf);
                        //                        sbDatosGeneraRC.Append("IdPedido: " + objPasarelaPago_Pedido.IdPedido + "<br>" + Constants.vbCrLf);
                        //                        sbDatosGeneraRC.Append("dblMontoPedido: " + dblMontoPedido + "<br>" + Constants.vbCrLf);
                        //                        sbDatosGeneraRC.Append("dblMontoPedidoRound: " + dblMontoPedidoRound + "<br>" + Constants.vbCrLf);
                        //                        sbDatosGeneraRC.Append("strNroTarjeta: " + strNroTarjetaBaneado + "<br>" + Constants.vbCrLf);
                        //                        sbDatosGeneraRC.Append("strIdForma: " + strIdForma + "<br>" + Constants.vbCrLf);
                        //                        sbDatosGeneraRC.Append("strIdValor: " + strIdValor + "<br>" + Constants.vbCrLf);
                        //                        sbDatosGeneraRC.Append("FechaPedido: " + objPasarelaPago_Pedido.FechaPedido + "<br>" + Constants.vbCrLf);
                        //                        sbDatosGeneraRC.Append("dblTopeRC_UATP: " + dblTopeRC_UATP + "<br>" + Constants.vbCrLf);
                        //                        sbDatosGeneraRC.Append("IdCotSRV: " + intIdCotVta + "<br>" + Constants.vbCrLf);
                        //                        sbDatosGeneraRC.Append("EsRutaSelva: " + bolEsRutaSelva + "<br>" + Constants.vbCrLf);
                        //                        sbDatosGeneraRC.Append("EsTarifaPublicada: " + bolEsTarifaPublicada + "<br>" + Constants.vbCrLf);

                        //                        if (objPasarelaPago_Pedido.EsUATP)
                        //                        {
                        //                            if (dblMontoPedido > dblTopeRC_UATP)
                        //                            {
                        //                                sbDatosGeneraRC.Append("<strong>No generar RC por tope</strong><br>" + Constants.vbCrLf);
                        //                                bolGenerarRC = false;
                        //                            }
                        //                            // lreque:
                        //                            // 26/08 TEMPORAL HASTA VALIDAR BIEN LOS CASOS
                        //                            bolGenerarRC = false;
                        //                        }
                        //                        else
                        //                            bolGenerarRC = true;

                        //                        if (bolGenerarRC)
                        //                        {
                        //                            ArrayList alstResult = null/* TODO Change to default(_) if this is not a reference type */;
                        //                            string strMsgError = "";
                        //                            try
                        //                            {
                        //                                bool intInsertaReciboReserva = false;
                        //                                if (bolEsRutaSelva)
                        //                                {
                        //                                    if (bolEsTarifaPublicada)
                        //                                        // alstResult = objPTABO.Inserta_ReciboCaja_Prueba(lstFilesPTACotVta, objPasarelaPago_Pedido.EsUATP, _
                        //                                        // objPasarelaPago_Pedido.IdPedido, dblMontoPedido, dblMontoPedidoRound, strNroTarjeta, _
                        //                                        // strIdForma, strIdValor, "cajawebPUB", objPasarelaPago_Pedido.FechaPedido, Nothing, bolEsRutaSelva, bolEsTarifaPublicada)




                        //                                        // ultimo comentado

                        //                                        alstResult = objPTABO.Inserta_ReciboCaja(lstFilesPTACotVta, objPasarelaPago_Pedido.EsUATP, objPasarelaPago_Pedido.IdPedido, dblMontoPedido, dblMontoPedidoRound, strNroTarjeta, strIdForma, strIdValor, "cajawebPUB", objPasarelaPago_Pedido.FechaPedido, null/* TODO Change to default(_) if this is not a reference type */, bolEsRutaSelva, bolEsTarifaPublicada);
                        //                                    else
                        //                                        intInsertaReciboReserva = false;
                        //                                }
                        //                                else
                        //                                    // ultimo comentado

                        //                                    alstResult = objPTABO.Inserta_ReciboCaja(lstFilesPTACotVta, objPasarelaPago_Pedido.EsUATP, objPasarelaPago_Pedido.IdPedido, dblMontoPedido, dblMontoPedidoRound, strNroTarjeta, strIdForma, strIdValor, "cajaweb", objPasarelaPago_Pedido.FechaPedido, null/* TODO Change to default(_) if this is not a reference type */, bolEsRutaSelva, bolEsTarifaPublicada);
                        //                            }
                        //                            catch (Exception ex)
                        //                            {
                        //                                strMsgError = ex.ToString();

                        //                                string strIPUsuario = "";
                        //                                if (Request.ServerVariables("HTTP_X_FORWARDED_FOR") != null)
                        //                                    strIPUsuario = Request.ServerVariables("HTTP_X_FORWARDED_FOR");
                        //                                else
                        //                                    strIPUsuario = Request.ServerVariables("REMOTE_ADDR");
                        //                                // 'PROYALERTA
                        //                                NMailAlerta oNMailAlerta = new NMailAlerta();
                        //                                oNMailAlerta.EnvioCorreoRegistrarError("Error de " + ConstantesWeb.APP_NAME, this, ex, strIPUsuario + "|Inserta_ReciboCaja");
                        //                                oNMailAlerta = null/* TODO Change to default(_) if this is not a reference type */;
                        //                            }

                        //                            string strLog = "";
                        //                            EnviarCorreo objEnviarCorreo = new EnviarCorreo();
                        //                            if (alstResult != null && alstResult.Count > 0)
                        //                            {
                        //                                if (alstResult(0) == "1")
                        //                                    bolGeneroRC_OK = true;

                        //                                try
                        //                                {
                        //                                    strLog = alstResult(2);
                        //                                }
                        //                                catch (Exception ex)
                        //                                {
                        //                                }

                        //                                if (bolGeneroRC_OK)
                        //                                {
                        //                                    string strIdsRCTmp = alstResult(1);

                        //                                    string strIdsRC = "";

                        //                                    StringBuilder sbInfo = new StringBuilder();
                        //                                    sbInfo.Append("Se han generado los siguientes RC para el pedido nro. <strong>" + objPasarelaPago_Pedido.IdPedido + "</strong>:<br><br>" + Constants.vbCrLf);

                        //                                    foreach (string strIdRC in strIdsRCTmp.Split(";"))
                        //                                    {
                        //                                        if (!string.IsNullOrEmpty(strIdRC))
                        //                                        {
                        //                                            strIdsRC += strIdRC + ",";
                        //                                            sbInfo.Append("-RC " + strIdRC + "<br>" + Constants.vbCrLf);
                        //                                        }
                        //                                    }
                        //                                    if (strIdsRC.Length > 0)
                        //                                        strIdsRC = strIdsRC.Substring(0, strIdsRC.Length - 1);

                        //                                    objEnviarCorreo.Enviar_Log("Log: se generó RC", sbDatosGeneraRC.ToString() + "<br><br>Log:" + strLog + "<br><br>" + "Generación automática de RC nro. " + strIdsRC + "<br><br>" + sbInfo.ToString(), true, System.Configuration.ConfigurationManager.AppSettings("STR_PATH_FILES_RC_WEB"), "RC_OK_SRV_" + intIdCotVta + "_" + strIdsRC.Replace(",", "-"));

                        //                                    objEnviarCorreo.Enviar_CajaWeb(Application("WEB_CODIGO"), 1, 128, sbInfo.ToString(), "Generación automática de RC nro. " + strIdsRC, null/* TODO Change to default(_) if this is not a reference type */);
                        //                                }
                        //                                else
                        //                                {
                        //                                    objEnviarCorreo.Enviar_Log("Log: no se generó RC", sbDatosGeneraRC.ToString() + "<br><br>Log:" + strLog + "<br><br><strong>Error:" + strMsgError + "</strong>", true, System.Configuration.ConfigurationManager.AppSettings("STR_PATH_FILES_RC_WEB"), "RC_NO_OK_SRV_" + intIdCotVta);
                        //                                    try
                        //                                    {
                        //                                        MyAlert(strLog);
                        //                                        string strIPUsuario = "";
                        //                                        if (HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR") != null)
                        //                                            strIPUsuario = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR");
                        //                                        else
                        //                                            strIPUsuario = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR");
                        //                                        objCotizacionVentaBO.Inserta_Post_Cot(intIdCotVta, NuevoMundoUtility.ConstantesUtility.STR_ID_TIPO_POST_SRV_USUARIO, strLog, strIPUsuario, objUsuarioSession.LoginUsuario, objUsuarioSession.IdUsuario, objUsuarioSession.IdDep, objUsuarioSession.IdOfi, null/* TODO Change to default(_) if this is not a reference type */, null/* TODO Change to default(_) if this is not a reference type */, objPasarelaPago_Pedido.IdEstadoPedido, true, null/* TODO Change to default(_) if this is not a reference type */, true, null/* TODO Change to default(_) if this is not a reference type */, false, null/* TODO Change to default(_) if this is not a reference type */, null/* TODO Change to default(_) if this is not a reference type */, null/* TODO Change to default(_) if this is not a reference type */, null/* TODO Change to default(_) if this is not a reference type */, null/* TODO Change to default(_) if this is not a reference type */, null/* TODO Change to default(_) if this is not a reference type */, null/* TODO Change to default(_) if this is not a reference type */, null/* TODO Change to default(_) if this is not a reference type */);
                        //                                    }
                        //                                    catch (Exception ex)
                        //                                    {
                        //                                    }
                        //                                }
                        //                            }
                        //                            else
                        //                                objEnviarCorreo.Enviar_Log("Log: no se generó RC", sbDatosGeneraRC.ToString() + "<br><br>Log:" + strLog + "<br><br><strong>Error:" + strMsgError + "</strong>", true, System.Configuration.ConfigurationManager.AppSettings("STR_PATH_FILES_RC_WEB"), "RC_NO_OK_SRV_" + intIdCotVta);
                        //                            break;
                        //                        }
                        //                        else
                        //                        {
                        //                            EnviarCorreo objEnviarCorreo = new EnviarCorreo();
                        //                            objEnviarCorreo.Enviar_Log("Log: no se generó RC", sbDatosGeneraRC.ToString() + "<br><br><strong>Error: validaciones internas</strong>", true, System.Configuration.ConfigurationManager.AppSettings("STR_PATH_FILES_RC_WEB"), "RC_NO_OK_SRV_" + intIdCotVta);
                        //                        }
                        //                    }
                        //                    // PARA QUE NO GENERE OTRO RC
                        //                    bolGenerarRC = false;
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }
                catch (Exception ex)
                {
                    ///*********CODIGO POR REVISAR Y TRATAR********/
                    //string strIPUsuario = "";
                    //if (Request.ServerVariables("HTTP_X_FORWARDED_FOR") != null)
                    //    strIPUsuario = Request.ServerVariables("HTTP_X_FORWARDED_FOR");
                    //else
                    //    strIPUsuario = Request.ServerVariables("REMOTE_ADDR");
                    //// 'PROYALERTA
                    //NMailAlerta oNMailAlerta = new NMailAlerta();
                    //oNMailAlerta.EnvioCorreoRegistrarError("Error de " + ConstantesWeb.APP_NAME, this, ex, strIPUsuario + "|Inserta_ReciboCaja");
                    //oNMailAlerta = null/* TODO Change to default(_) if this is not a reference type */;
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
        private void validacionAssociate(ref AssociateFile _fileAssociate, ref AssociateFileRS _responseFile, ref UsuarioLogin UserLogin)
        {
            string mensajeError = string.Empty;
            //_pedido.IdLang = 1; _pedido.IdWeb = 0;

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
                mensajeError += "Envie la lista de files a asociar|";
            }
            else if (_fileAssociate.LstFiles.Count > 3)
            {
                mensajeError += "El maximo de files asociar es 3|";
            }
            else
            {
                int posListFile = 0;
                foreach (FileSRV fileSRV in _fileAssociate.LstFiles) /*Aqui se tendra que validar si existe registro del File con la Sucursal asociadas al vendedor (configurado en base al UsuarioId)*/
                {
                    if (fileSRV == null)
                    {
                        mensajeError += "Envie el registro " + posListFile + " de la lista de files a asociar|";
                        break;
                    }
                    if (fileSRV.IdFilePTA <= 0)
                    {
                        mensajeError += "Envie el IdFile del registro " + posListFile + " de la lista de files a asociar|";
                    }
                    /*if (fileSRV.Fecha == null)
                    {
                        mensajeError += "Envie la Fecha del registro " + posListFile + " de la lista de files a asociar|";
                    }
                    if (fileSRV.ImporteFact > 0)
                    {
                        mensajeError += "Envie el importe del registro " + posListFile + " de la lista de files a asociar|";
                    }*/
                    if (string.IsNullOrEmpty(fileSRV.Cliente))
                    {
                        mensajeError += "Envie el cliente del registro " + posListFile + " de la lista de files a asociar|";
                    }
                    /*if (string.IsNullOrEmpty(fileSRV.Moneda))
                    {
                        mensajeError += "Envie la moneda del registro " + posListFile + " de la lista de files a asociar|";
                    }*/

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

        public FileSRV CargarInfoFile(int sucursal, int idfile)
        {
            _CotizacionSRV_Repository = new CotizacionSRV_AW_Repository();
            FileSRV fileSRV = new FileSRV();

            string strIdFile = Convert.ToString(idfile);
            string strNumFile = "1"; // Si es el File 1, 2 o 3
            string strIdEstCot = "1";
            string strTipo = "1";

            strIdFile = strIdFile.Replace(",", "");

            DataTable dtImporteFile;
            int intIdOfiCotVta = 23;//CAMBIAR POR LA OFICINA 

            if (intIdOfiCotVta == 6)//si corresponde a destinos mundiales(6)
            {
                dtImporteFile = null;//dtImporteFile = objPTAFileBO.Obtiene_InfoFile_DM(sucursal, idfile);
            }
            else
            {
                dtImporteFile = _CotizacionSRV_Repository._Select_InfoFile(sucursal, idfile);
            }

            if (dtImporteFile == null)
            {
                //aca retornar el el codigo 'ER' y mensaje "lo que muestra este aviso"

                //objNMWebUtility.ExecuteJavaScript(this, "sin_info1", "<script>alert('No se ha encontrado información con el número de file ingresado en la sucursal seleccionada.'); </script>");
                //objNMWebUtility = null;
                //LimpiarDatosFile();
                //btnConfirmFile.Disabled = true;
            }
            else if (dtImporteFile.Rows.Count == 0)
            {
                //aca retornar el el codigo 'ER' y mensaje "lo que muestra este aviso"
                //objNMWebUtility.ExecuteJavaScript(this, "sin_info2", "<script>alert('No se ha encontrado información con el número de file ingresado en la sucursal seleccionada.'); </script>");
                //objNMWebUtility = null;
                //LimpiarDatosFile();
                //btnConfirmFile.Disabled = true;
            }
            else
            {
                DataRow drCliente = dtImporteFile.Rows[0];
                double dblImporteSumaUSD = 0;
                double dblImporteSumaSOL = 0;
                string cliente = "";
                double monto = 0;
                string moneda = "";
                // Dim dblImporteRestaUSD As Double = 0
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

                // este dato sirve para devolver en el response
                cliente = drCliente["NOMBRE_CLIENTE"].ToString();


                //monto se devuelve como dato al response
                if (strIdMoneda == "USD")
                {
                    monto = dblImporteSumaUSD;
                }
                else
                {
                    monto = dblImporteSumaSOL;
                }

                //moneda se devuelve como dato al response
                moneda = drCliente["ID_MONEDA"].ToString();

                fileSRV.Cliente = cliente;
                fileSRV.ImporteFact = monto;
                fileSRV.Moneda = moneda;
            }

            return fileSRV;
        }
        #endregion
    }
}
