using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Microsoft.VisualBasic;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;

namespace Expertia.Estructura.Repository.General
{
    public class EnviarCorreo : OracleBase<VentasRequest>, IEnviarCorreo
    {
        #region Constructor
        public EnviarCorreo(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {

        }
        #endregion

        #region Funciones Publicas
        public bool Enviar_SolicitudPagoServicioSafetyPay(string IdUsuario,int pIntIdWeb, int pIntIdLang, int pIntIdCotVta, string pStrEmailTO, string pStrEMailCC, string pStrNomCli, string pStrApeCli, string pStrURLPago, string pStrNomCompletoUsuWeb, string pStrEmailUsuWeb, Int16 pIntIdFormaPago, string pStrIdTransaction, int pIntIdPedido, double pDblMontoPagar, string pStrFechaExpiraPago, List<AmountType> pLstAmountSafetyPay, List<PaymentLocationType> pLstPaymentLocationSafetyPay)
        {            
            CorreoNM objCorreo = null;
            NMMail objNMMail = new NMMail();
            EncriptaCadena objEncriptaCadena = new EncriptaCadena();
            bool _return = false;

            try
            {
                string strNroPedidoEncriptado = objEncriptaCadena.DES_Encrypt(pIntIdPedido.ToString(), "m0t0rvu3l0s");

                objCorreo = Get_Correo_Web(pIntIdWeb, pIntIdLang, Constantes_SafetyPay.ID_MAIL_SOLICITUD_PAGO_SERVICIO_SF);
                if (objCorreo != null)
                {
                    objNMMail.MailServer = objCorreo.HostCorreo;
                    objNMMail.AddMailFrom = objCorreo.FromCorreo;

                    objNMMail.MailSubject = "Solicitud de Compra Nro. " + pIntIdCotVta + " para " + pStrNomCli + " " + pStrApeCli + " - SafetyPay";
                    objNMMail.AddMailsTo = pStrEmailTO;
                    objNMMail.AddMailsCC = pStrEMailCC;
                    objNMMail.AddMailsCC = objCorreo.CCCorreo;
                    objNMMail.AddMailsBCC = objCorreo.BCCCorreo;
                    objNMMail.AddMailsBCC = pStrEmailUsuWeb;

                    if (objCorreo.FormatoCorreo.ToUpper() == "HTML")
                        objNMMail.MailHTML = true;
                    else
                        objNMMail.MailHTML = false;

                    string strFechaExpiraTmp = pStrFechaExpiraPago;
                    if (pStrFechaExpiraPago.Split('(').Length >= 2) {
                        strFechaExpiraTmp = pStrFechaExpiraPago.Split('(')[0];
                    }                    

                    System.Text.StringBuilder objStringBuilder = new System.Text.StringBuilder();
                    objStringBuilder.Append("<div style='font-family: Arial; font-size:12px; padding:10px;'>" + Constants.vbCrLf);
                    objStringBuilder.Append("<p>" + Constants.vbCrLf);
                    objStringBuilder.Append("<strong>Estimado(a) " + pStrNomCli + ":</strong>" + Constants.vbCrLf);
                    objStringBuilder.Append("</p>" + Constants.vbCrLf);
                    objStringBuilder.Append("<p><strong></strong>" + Constants.vbCrLf);
                    objStringBuilder.Append("A continuaci&oacute;n, te enviamos la boleta de SafetyPay para que puedas realizar el pago de tu Solicitud de Compra Nro. <strong>" + pIntIdCotVta + "</strong>." + Constants.vbCrLf);
                    objStringBuilder.Append("</p>" + Constants.vbCrLf);
                    objStringBuilder.Append("<table cellpadding='2' class='texto_general'>" + Constants.vbCrLf);
                    objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                    objStringBuilder.Append("<td>" + Constants.vbCrLf);
                    objStringBuilder.Append("<table cellpadding='5' border='1' style='border-collapse:collapse; border-style:solid;border-color:black'><tr><td>" + Constants.vbCrLf);
                    objStringBuilder.Append("<table class='texto_general' cellpadding='2'>" + Constants.vbCrLf);
                    objStringBuilder.Append("<tr><td style='color:#CD0200;' align='center'><strong>Total a Pagar</strong></td></tr>" + Constants.vbCrLf);
                    objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                    objStringBuilder.Append("<td align='center' class='total_pagar' style='font-size:20px'>" + Constants.vbCrLf);
                    objStringBuilder.Append("US$ " + pDblMontoPagar.ToString("0.00") + Constants.vbCrLf);
                    objStringBuilder.Append("</td>" + Constants.vbCrLf);
                    objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                    objStringBuilder.Append("</table>" + Constants.vbCrLf);
                    objStringBuilder.Append("</td></tr></table>" + Constants.vbCrLf);
                    objStringBuilder.Append("</td>" + Constants.vbCrLf);
                    objStringBuilder.Append("<td width='15'>&nbsp;</td>" + Constants.vbCrLf);
                    objStringBuilder.Append("<td>" + Constants.vbCrLf);
                    objStringBuilder.Append("<table class='texto_general'>" + Constants.vbCrLf);
                    objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                    objStringBuilder.Append("<td align='center'>" + Constants.vbCrLf);
                    objStringBuilder.Append("Tu c&oacute;digo de SafetyPay es <span class='alerta'>" + pStrIdTransaction + "</span>" + Constants.vbCrLf);
                    objStringBuilder.Append("</td>" + Constants.vbCrLf);
                    objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                    objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                    objStringBuilder.Append("<td>" + Constants.vbCrLf);
                    objStringBuilder.Append("<table>" + Constants.vbCrLf);
                    objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                    objStringBuilder.Append("<td>" + Constants.vbCrLf);
                    objStringBuilder.Append("<img src='http://www.nmviajes.com/images/logos/safetypay.jpg'>" + Constants.vbCrLf);
                    objStringBuilder.Append("</td>" + Constants.vbCrLf);
                    objStringBuilder.Append("<td>" + Constants.vbCrLf);
                    objStringBuilder.Append("&nbsp;" + Constants.vbCrLf);
                    objStringBuilder.Append("</td>" + Constants.vbCrLf);
                    objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                    objStringBuilder.Append("</table>" + Constants.vbCrLf);
                    objStringBuilder.Append("</td>" + Constants.vbCrLf);
                    objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                    objStringBuilder.Append("</table>" + Constants.vbCrLf);
                    objStringBuilder.Append("</td>" + Constants.vbCrLf);
                    objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                    objStringBuilder.Append("</table>" + Constants.vbCrLf);
                    objStringBuilder.Append("<br />" + Constants.vbCrLf);

                    if (pLstPaymentLocationSafetyPay == null || pLstPaymentLocationSafetyPay.Count == 0)
                    {
                        objStringBuilder.Append("<table cellpadding='3'>" + Constants.vbCrLf);
                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general'>-<span class='link_correo'><a href='https://www.viabcp.com/wps/portal/viabcpp/personas' target='_blank'>Banco de Crédito del Perú</a></span></td>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td width='50'>&nbsp;</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td><a href='https://www.viabcp.com/wps/portal/viabcpp/personas' target='_blank'><img src='http://www.nmviajes.com/images/pagos/logo-banco-de-crédito.jpg' width='45' height='20' border='0'/></a></td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);

                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general' colspan='3'>1. Ingrese a <strong><a href='https://www.viabcp.com/wps/portal/viabcpp/personas' target='_blank'>www.viabcp.com</a></strong> con su clave de acceso.</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general' colspan='3'>2. Seleccione la opción <strong>'Pagos y Transferencias' - 'Pago de servicios'</strong>.</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general' colspan='3'>3. Seleccione <strong>'Empresas Diversas' - 'SafetyPay'</strong> y el tipo de moneda.</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general' colspan='3'>4. Ingrese su código de pago <strong>" + pStrIdTransaction + "</strong>, verifique el monto, confirme el pago con su <strong>clave token</strong> y listo!</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);

                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general' colspan='3'></td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);

                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general'>-<span class='link_correo'><a href='https://www.bbvacontinental.pe/' target='_blank'>BBVA Continental</a></span></td>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td width='50'>&nbsp;</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td><a href='https://www.bbvacontinental.pe/' target='_blank'><img src='http://www.nmviajes.com/images/pagos/logo-bbva.jpg' width='45' height='20' border='0'/></a></td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);

                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general' colspan='3'>1. Ingrese a <strong><a href='https://www.bbvacontinental.pe/' target='_blank'>www.bbvacontinental.com</a></strong> con su clave de acceso.</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general' colspan='3'>2. Seleccione <strong>'Pago de Servicios'</strong> y dentro <strong>'Otras Opciones'</strong> elija <strong>'Paga con SafetyPay'</strong>.</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general' colspan='3'>3. Ingrese su código de pago <strong>" + pStrIdTransaction + "</strong>, monto a pagar y seleccione la cuenta de cargo.</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general' colspan='3'>4. Confirme la transacción con su <strong>clave SMS</strong> y listo!</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);

                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general' colspan='3'></td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general'>-<span class='link_correo'><a href='http://www.interbank.com.pe/' target='_blank'>Interbank</a></span></td>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td width='50'>&nbsp;</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td><a href='http://www.interbank.com.pe/' target='_blank'><img src='http://www.nmviajes.com/images/pagos/logo-interbank.jpg' width='45' height='20' border='0'/></a></td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general' colspan='3'>1. Ingrese a <strong><a href='http://www.interbank.com.pe/' target='_blank'>www.interbank.com.pe</a></strong> con su clave de acceso.</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general' colspan='3'>2. Seleccione la opción <strong>'Pago de recibos' - 'Diversas Empresas'</strong>.</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general' colspan='3'>3. Seleccione <strong>la cuenta de cargo</strong> - Elija <strong>'SafetyPay'</strong>.</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general' colspan='3'>4. Ingrese su código de pago <strong>" + pStrIdTransaction + "</strong>, verifique el monto, confirme el pago con su <strong>clave SMS</strong> y listo!</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);

                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general' colspan='3'></td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general'>-<span class='link_correo'><a href='http://www.scotiabank.com.pe/Personas/Default' target='_blank'>Scotiabank</a></span></td>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td width='50'>&nbsp;</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td><a href='http://www.scotiabank.com.pe/Personas/Default' target='_blank'><img src='http://www.nmviajes.com/images/pagos/logo-scotiabank.jpg' width='45' height='20' border='0'/></a></td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general' colspan='3'>1. Ingrese a <strong><a href='http://www.scotiabank.com.pe/Personas/Default' target='_blank'>www.scotiabank.com.pe</a></strong> con su clave de acceso.</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general' colspan='3'>2. Seleccione la opción <strong>'Pagos' - 'Buscar empresas'</strong> y digite SafetyPay.</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general' colspan='3'>3. Ingrese el <strong>código de pago " + pStrIdTransaction + "</strong> en <strong>'número de referencia'</strong> y confirme el importe a pagar.</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td class='texto_general' colspan='3'>4. Seleccione la <strong>cuenta de cargo</strong>, confirme la transacción con su <strong>clave token</strong> y listo!</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("</table>" + Constants.vbCrLf);
                    }
                    else
                    {
                        objStringBuilder.Append("<table class='texto_general'>" + Constants.vbCrLf);
                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td colspan='2'>" + Constants.vbCrLf);
                        objStringBuilder.Append("Puedes realizar el pago en los siguientes establecimientos antes del <span class='alerta'>" + strFechaExpiraTmp + "</span>:" + Constants.vbCrLf);
                        objStringBuilder.Append("</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td colspan='2'>&nbsp;</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);

                        objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("<td colspan='2'>" + Constants.vbCrLf);
                        objStringBuilder.Append("<table cellpadding='3'>" + Constants.vbCrLf);

                        foreach (PaymentLocationType objPaymentLocation in pLstPaymentLocationSafetyPay)
                        {
                            if (!objPaymentLocation.Name.ToUpper().Contains("CAJA"))
                            {
                                objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                                objStringBuilder.Append("<td class='texto_general'>-<span class='link_correo'><a href='http://www.nmviajes.com/pagos-online/safetypay?id=" + strNroPedidoEncriptado + "' target='_blank'>" + objPaymentLocation.Name + "</a></span></td>" + Constants.vbCrLf);
                                objStringBuilder.Append("<td width='50'>&nbsp;</td>" + Constants.vbCrLf);

                                if (pIntIdWeb.Equals(Webs_Cid.NM_WEB_ID))
                                {
                                    objStringBuilder.Append("<td><a href='http://www.nmviajes.com/pagos-online/safetypay?id=" + strNroPedidoEncriptado + "' target='_blank'><img src='http://www.nmviajes.com/images/logos/" + objPaymentLocation.Name.Replace(" ", "-").ToLower() + ".jpg' width='45' height='20' border='0'/></a></td>" + Constants.vbCrLf);
                                }
                                else
                                {
                                    objStringBuilder.Append("<td><img src='http://www.nmviajes.com/images/logos/" + objPaymentLocation.Name.Replace(" ", "-").ToLower() + ".jpg' width='45' height='20' border='0'/></td>" + Constants.vbCrLf);
                                }                                    
                                objStringBuilder.Append("</tr>" + Constants.vbCrLf);

                                foreach (PaymentStepType objStep in objPaymentLocation.lstPaymentStepType)
                                {
                                    objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                                    objStringBuilder.Append("<td class='texto_general' colspan='3'>" + objStep.Step + ". " + objStep.Value + "</td>" + Constants.vbCrLf);
                                    objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                                }
                                objStringBuilder.Append("<tr>" + Constants.vbCrLf);
                                objStringBuilder.Append("<td class='texto_general' colspan='3'></td>" + Constants.vbCrLf);
                                objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                            }
                        }

                        objStringBuilder.Append("</table>" + Constants.vbCrLf);
                        objStringBuilder.Append("</td>" + Constants.vbCrLf);
                        objStringBuilder.Append("</tr>" + Constants.vbCrLf);
                        objStringBuilder.Append("</table>" + Constants.vbCrLf);
                    }

                    if (ConfigurationManager.AppSettings[Constantes_Pedido.USERAGCORPGENERAPEDIDOLOGO].Contains(IdUsuario))
                    {
                        objCorreo.HeaderCorreo = objCorreo.HeaderCorreo.Replace("logo_correo", "logo_correo_AGCORP");
                    }
                    
                    objNMMail.MailBody = objCorreo.HeaderCorreo + objStringBuilder.ToString() + objCorreo.FooterCorreo;
                    objNMMail.SendMail(objCorreo.UsuarioCredentials, objCorreo.PasswordCredentials);
                                       
                    objNMMail = null;
                    _return = true;
                }
            }
            catch (Exception ex)
            {
                /*throw new Exception(ex.ToString());*/
                _return = false;
            }
            return _return;
        }

        public void Enviar_Log(string pStrAsunto, string pStrBody, bool pBolGenerarArchivo, string pStrCarpeta, string pStrNomArchivo)
        {
            try
            {
                if (pBolGenerarArchivo)
                {
                    NMMail objNMMail = new NMMail();                    
                    objNMMail.AddMailFrom = "errores-webmaster@inboxplace.com";
                    objNMMail.AddMailsTo = "webmaster@inboxplace.com ";
                    // objNMMail.AddMailsTo() = "luis_reque@hotmail.com"
                    objNMMail.MailServer = "10.75.103.230";
                    objNMMail.MailHTML = true;
                    objNMMail.MailSubject = pStrAsunto;
                    objNMMail.MailBody = pStrBody;
                    objNMMail.SendMail("errores-webmaster@inboxplace.com", "1Nb0xplac3");

                    if (!string.IsNullOrEmpty(pStrCarpeta))
                    {
                        string sFecha = DateTime.Now.ToString("yyyyMMdd");
                        string sHora = DateTime.Now.ToString("HHmmss");

                        string strRuta = pStrCarpeta + sFecha + @"\";
                        bool bolExiste = System.IO.Directory.Exists(strRuta);
                        if (!(bolExiste == true))
                            System.IO.Directory.CreateDirectory(strRuta);

                        System.IO.File.WriteAllText(strRuta + pStrNomArchivo, pStrBody);
                    }
                }
                else
                {
                    NMMail objNMMail = new NMMail();                    
                    objNMMail.AddMailFrom = "errores-webmaster@inboxplace.com";
                    objNMMail.AddMailsTo = "webmaster@inboxplace.com ";
                    // objNMMail.AddMailsTo() = "luis_reque@hotmail.com"
                    objNMMail.MailServer = "10.75.103.230";
                    objNMMail.MailHTML = true;
                    objNMMail.MailSubject = pStrAsunto;
                    objNMMail.MailBody = pStrBody;
                    objNMMail.SendMail("errores-webmaster@inboxplace.com", "1Nb0xplac3");
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Enviar_CajaWeb(int pIntIdWeb, int pIntIdLang, int pIntIdMail, string pStrHTMLBody, string pStrAsunto, string pStrEmailCounter)
        {
            NMMail objMail = new NMMail();
            try
            {                
                CorreoNM objCorreo = new CorreoNM(); 
                objCorreo = Get_Correo_Web(pIntIdWeb, pIntIdLang, pIntIdMail);

                if (objCorreo != null)
                {
                    objMail.MailServer = "10.75.102.2";
                    objMail.MailSubject = pStrAsunto;
                    objMail.MailFrom = new System.Net.Mail.MailAddress(objCorreo.FromCorreo);

                    if (!string.IsNullOrEmpty(pStrEmailCounter))
                        objMail.MailTo = new System.Net.Mail.MailAddress(pStrEmailCounter);

                    if (!string.IsNullOrEmpty(objCorreo.ToCorreo))
                        objMail.AddMailsTo = objCorreo.ToCorreo;
                    if (!string.IsNullOrEmpty(objCorreo.CCCorreo))
                    {
                        if (objCorreo.CCCorreo != "")
                            objMail.AddMailsCC = objCorreo.CCCorreo;
                    }
                    if (!string.IsNullOrEmpty(objCorreo.BCCCorreo))
                    {
                        if (objCorreo.BCCCorreo != "")
                            objMail.AddMailsBCC = objCorreo.BCCCorreo;
                    }

                    objMail.MailHTML = true;
                    objMail.MailBody = objCorreo.StyleFileCorreo + objCorreo.HeaderCorreo + pStrHTMLBody + objCorreo.FooterCorreo;
                    objMail.SendMail(objCorreo.UsuarioCredentials, objCorreo.PasswordCredentials);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                objMail = null;
            }
        }
        #endregion

        #region Procesos BD
        private CorreoNM Get_Correo_Web(int pIntIdWeb, int pIntIdLang, int pIntIdMail)
        {
            try
            {
                CorreoNM RptaCorreo = new CorreoNM();

                #region Parameter
                AddParameter("pNumIdWeb_in", OracleDbType.Int64, pIntIdWeb, ParameterDirection.Input);
                AddParameter("pNumIdLang_in", OracleDbType.Int64, pIntIdLang, ParameterDirection.Input);
                AddParameter("pNumIdMail_in", OracleDbType.Int64, pIntIdMail, ParameterDirection.Input);                
                AddParameter(OutParameter.CursorMailWeb, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
                #endregion

                #region Invoke                
                ExecuteStoredProcedure(StoredProcedureName.AW_Get_Mail_Web);
                RptaCorreo = FillRptaMailWeb(GetDtParameter(OutParameter.CursorMailWeb));
                #endregion

                return RptaCorreo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Auxiliares
        private CorreoNM FillRptaMailWeb(DataTable dt = null)
        {
            try
            {
                CorreoNM objCorreo = null;
                if (dt != null && dt.Rows[0] != null)
                {
                    DataRow rowBDCorreo = dt.Rows[0];
                    objCorreo = new CorreoNM();

                    #region Loading
                    objCorreo.IdCorreo = Convert.ToInt32(rowBDCorreo["MAIL_CID"]);
                    objCorreo.FromCorreo = (rowBDCorreo["MLWEB_FROM"].ToString() == null ? string.Empty: rowBDCorreo["MLWEB_FROM"].ToString());
                    objCorreo.DisplayFromCorreo = (rowBDCorreo["MLWEB_FROM_DISPLAY"].ToString() == null ? string.Empty : rowBDCorreo["MLWEB_FROM_DISPLAY"].ToString());
                    objCorreo.ToCorreo = (rowBDCorreo["MLWEB_TO"].ToString() == null ? string.Empty : rowBDCorreo["MLWEB_TO"].ToString());
                    objCorreo.CCCorreo = (rowBDCorreo["MLWEB_CC"].ToString() == null ? string.Empty : rowBDCorreo["MLWEB_CC"].ToString());
                    objCorreo.BCCCorreo = (rowBDCorreo["MLWEB_BCC"].ToString() == null ? string.Empty : rowBDCorreo["MLWEB_BCC"].ToString());
                    objCorreo.StyleAddressCorreo = (rowBDCorreo["MLWEB_STYLEADDRESS"].ToString() == null ? string.Empty : rowBDCorreo["MLWEB_STYLEADDRESS"].ToString());
                    objCorreo.SubjectCorreo = (rowBDCorreo["MLWEB_SUBJECT"].ToString() == null ? string.Empty : rowBDCorreo["MLWEB_SUBJECT"].ToString());
                    objCorreo.FormatoCorreo = (rowBDCorreo["MLWEB_FORMAT"].ToString() == null ? string.Empty : rowBDCorreo["MLWEB_FORMAT"].ToString());
                    objCorreo.HeaderCorreo = (rowBDCorreo["MLWEB_HEADER"].ToString() == null ? string.Empty : rowBDCorreo["MLWEB_HEADER"].ToString());
                    objCorreo.FooterCorreo = (rowBDCorreo["MLWEB_FOOTER"].ToString() == null ? string.Empty : rowBDCorreo["MLWEB_FOOTER"].ToString());
                    objCorreo.HostCorreo = (rowBDCorreo["MLWEB_HOST"].ToString() == null ? string.Empty : rowBDCorreo["MLWEB_HOST"].ToString());
                    objCorreo.StyleFileCorreo = (rowBDCorreo["MLWEB_STYLEFILE"].ToString() == null ? string.Empty : rowBDCorreo["MLWEB_STYLEFILE"].ToString());
                    objCorreo.LogoCorreo = (rowBDCorreo["MLWEB_LOGO"].ToString() == null ? string.Empty : rowBDCorreo["MLWEB_LOGO"].ToString());
                    objCorreo.BCCCorreoAgy = (rowBDCorreo["MLWEB_BCC_AGY"].ToString() == null ? string.Empty : rowBDCorreo["MLWEB_BCC_AGY"].ToString());
                    objCorreo.BCCCorreoSist = (rowBDCorreo["MLWEB_BCC_SIS"].ToString() == null ? string.Empty : rowBDCorreo["MLWEB_BCC_SIS"].ToString());
                    objCorreo.UsuarioCredentials = (rowBDCorreo["USER_CREDENTIALS"].ToString() == null ? string.Empty : rowBDCorreo["USER_CREDENTIALS"].ToString());
                    objCorreo.PasswordCredentials = (rowBDCorreo["PASSWORD_CREDENTIALS"].ToString() == null ? string.Empty : rowBDCorreo["PASSWORD_CREDENTIALS"].ToString());
                    objCorreo.IdWeb = Convert.ToInt32(rowBDCorreo["WEBS_CID"]);
                    objCorreo.IdLang = Convert.ToInt32(rowBDCorreo["LANG_CID"]);
                    objCorreo.UsuarioCredentials = (rowBDCorreo["USER_CREDENTIALS"].ToString() == null ? string.Empty : rowBDCorreo["USER_CREDENTIALS"].ToString());
                    objCorreo.PasswordCredentials = (rowBDCorreo["PASSWORD_CREDENTIALS"].ToString() == null ? string.Empty : rowBDCorreo["PASSWORD_CREDENTIALS"].ToString());
                    #endregion
                }
                return objCorreo;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}