using System;
using System.Linq;
using Microsoft.VisualBasic;
using System.Net.Mail;
using Expertia.Estructura.Models;

namespace Expertia.Estructura.Utils
{
    public class NMMail
    {
        private System.Net.Mail.MailMessage oMail = new System.Net.Mail.MailMessage();
        private System.Net.Mail.SmtpClient oSmtp = new System.Net.Mail.SmtpClient();
        private System.Net.Mail.SmtpClient oSmtp2 = new System.Net.Mail.SmtpClient();
        private CorreoNM objCorreo;

        private string mvarMailFrom;
        
        private string mvarMailSubject;
        private string mvarMailCc;
        private string mvarMailBcc;
        private string mvarMailServer;
        private string mvarMailBody;
        private string mvarMailHTML;

        private bool mailSent = false;
        private string pStrDisplay;

        public MailAddress MailFrom
        {
            set
            {
                oMail.From = value;
            }
        }
        public MailAddress MailTo
        {
            set
            {
                oMail.To.Add(value);
            }
        }
        public string MailSubject
        {
            set
            {
                oMail.Subject = value;
            }
        }
        public MailAddress MailCc
        {
            set
            {
                oMail.CC.Add(value);
            }
        }
        public MailAddress MailBcc
        {
            set
            {
                oMail.Bcc.Add(value);
            }
        }
        public string MailServer
        {
            set
            {
                oSmtp.Host = value;
            }
        }
        public string MailBody
        {
            set
            {
                oMail.Body = value;
            }
        }
        public bool MailHTML
        {
            set
            {
                oMail.IsBodyHtml = value;
            }
        }
        
        public System.Net.Mail.MailPriority MailPrioridad
        {
            set
            {
                oMail.Priority = value;
            }
        }

        public void AddAttachment_Stream(System.IO.Stream pObjStream, string pStrNomArchivo)
        {
            Attachment objAttachment = new Attachment(pObjStream, pStrNomArchivo);
            oMail.Attachments.Add(objAttachment);
        }
        public void AddAttachment(string pStrNomArchivo)
        {
            Attachment objAttachment = new Attachment(pStrNomArchivo);
            if (System.IO.File.Exists(pStrNomArchivo))
                oMail.Attachments.Add(objAttachment);
        }
        public int ContarArchivosAtachados()
        {
            int cuenta;
            cuenta = oMail.Attachments.Count();
            return cuenta;
        }

        public string AddMailFrom
        {
            set
            {
                if (!string.IsNullOrEmpty(Strings.Trim(value)))
                    oMail.From = new MailAddress(Strings.Trim(value));
            }
        }

        public string AddMailsCC
        {
            set
            {
                if (!string.IsNullOrEmpty(Strings.Trim(value)))
                {
                    for (int intX = 0; intX <= value.Split(';').Length - 1; intX++)
                    {
                        if (!string.IsNullOrEmpty(Strings.Trim(value.Split(';')[intX])))
                            oMail.CC.Add(Strings.Trim(value.Split(';')[intX]));
                    }
                }
            }
        }

        public string AddMailsBCC
        {
            set
            {
                if (!string.IsNullOrEmpty(Strings.Trim(value)))
                {
                    for (int intX = 0; intX <= value.Split(';').Length - 1; intX++)
                    {
                        if (!string.IsNullOrEmpty(Strings.Trim(value.Split(';')[intX])))
                            oMail.Bcc.Add(Strings.Trim(value.Split(';')[intX]));
                    }
                }
            }
        }

        public string AddMailsTo
        {
            set
            {
                if (!string.IsNullOrEmpty(Strings.Trim(value)))
                {
                    for (int intX = 0; intX <= value.Split(';').Length - 1; intX++)
                    {
                        if (!string.IsNullOrEmpty(Strings.Trim(value.Split(';')[intX])))
                            oMail.To.Add(value.Split(';')[intX]);
                    }
                }
            }
        }

        public void SendMail(string pStrUsuarioCredential, string pStrPassCredential)
        {
            string Line = "1,";

            try
            {
                oSmtp.UseDefaultCredentials = false;
                Line += "3,";
                oSmtp.EnableSsl = false;
                if (!pStrUsuarioCredential.Contains("inboxplace"))
                {
                    oSmtp.Credentials = new System.Net.NetworkCredential(pStrUsuarioCredential, pStrPassCredential, "smtp.office365.com");
                    oSmtp.EnableSsl = true;
                    oSmtp.Port = 587;
                }
                else
                    oSmtp.Credentials = new System.Net.NetworkCredential(pStrUsuarioCredential, pStrPassCredential);
                Line += "4,";
                oSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                
                Line += "5,";
                oSmtp.Send(oMail);
                Line += "6,";
            }        
            catch (Exception ex)
            {
                if (("El servidor no admite conexiones seguras.||" + "Server does not support secure connections.||" + "El certificado remoto no es válido según el procedimiento de validación.||" + "The remote certificate is invalid according to the validation procedure.").Contains(ex.Message))
                    SendMail_CS(pStrUsuarioCredential, pStrPassCredential);
                else
                {
                    Line += "7,";
                    //NMMailError objNMMailError = new NMMailError();
                    //objNMMailError.EnviarError("**ERROR-- SendMail", "*Exception: " + ex.Message + " <br/> " + "*StackTrace: " + ex.StackTrace + "<br/>" + "*Seguimiento: " + Line + "<br/>" + "*From: " + oMail.From.Address + " <br/> " + "*To: " + oMail.To.ToString() + " <br/> " + "*Subject: " + oMail.Subject + " <br/> " + "*MailServer: " + oSmtp.Host + " <br/> " + "*pStrUsuarioCredential: " + pStrUsuarioCredential + " <br/> " + "*pStrPassCredential: " + pStrPassCredential + " <br/> ", NMMailError.PRIORITY.NORMAL);
                    //objNMMailError = null;
                }
            }
        }
        public void SendMail_CS(string pStrUsuarioCredential, string pStrPassCredential)
        {
            string Line = "1,";
            try
            {
                oSmtp.UseDefaultCredentials = false;
                oSmtp.Credentials = null;
                oSmtp.EnableSsl = false;
                oSmtp.Port = 25;
                oSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                oSmtp.Send(oMail);
            }
            catch (Exception ex)
            {
                Line += "7,";
                //NMMailError objNMMailError = new NMMailError();
                //objNMMailError.EnviarError("**ERROR-- SendMail SC", "*Exception: " + ex.Message + " <br/> " + "*StackTrace: " + ex.StackTrace + "<br/>" + "*Seguimiento: " + Line + "<br/>" + "*From: " + oMail.From.Address + " <br/> " + "*To: " + oMail.To.ToString() + " <br/> " + "*Subject: " + oMail.Subject + " <br/> " + "*MailServer: " + oSmtp.Host + " <br/> " + "*pStrUsuarioCredential: " + pStrUsuarioCredential + " <br/> " + "*pStrPassCredential: " + pStrPassCredential + " <br/> ", NMMailError.PRIORITY.NORMAL);
                //objNMMailError = null;
            }
        }
       
        public void SendMail(string Token, CorreoNM correo = null)
        {
            oSmtp.SendCompleted += SendCompletedCallback;
            objCorreo = correo;
            oSmtp.SendAsync(oMail, Token);
        }

        public void SendMail_Validation(string usuario, string contrasenia)
        {
            oSmtp.Credentials = new System.Net.NetworkCredential(usuario, contrasenia);
            oSmtp.Send(oMail);
        }

        public void SendCompletedCallback(System.Object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            string token = System.Convert.ToString(e.UserState.ToString());

            if (e.Error != null)
                Serializer(token, e);

            objCorreo = null;
            oSmtp = null;
            oMail = null;
            mailSent = true;
        }

        private void Serializer(string Token, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            System.IO.StreamWriter oStreamWriter;
            System.Xml.Serialization.XmlSerializer oSerializer;
            string file_date, file_name, file_err_data;
            System.IO.StreamWriter fp;

            try
            {
                {
                    var withBlock = DateTime.Now;
                    file_date = withBlock.Day + "-" + withBlock.Month + "-" + withBlock.Year + "-" + withBlock.Hour + "-" + withBlock.Minute + "-" + withBlock.Second + "-" + withBlock.Millisecond;
                }
                file_name = Token + "_" + objCorreo.IdWeb + "_" + objCorreo.IdLang + "_" + file_date;
                file_err_data = "Err_" + file_name;

                oSerializer = new System.Xml.Serialization.XmlSerializer(typeof(CorreoNM));
                oStreamWriter = new System.IO.StreamWriter(UtilityCorreo.ERR_FOLDER_PATH + file_name + ".xml");
                oSerializer.Serialize(oStreamWriter, objCorreo);

                fp = System.IO.File.CreateText(UtilityCorreo.ERR_FOLDER_PATH + file_err_data + ".txt");
                fp.WriteLine(e.Error.InnerException);
                fp.Close();

                oStreamWriter.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                oSerializer = null;
                oStreamWriter = null;
                fp = null;
            }
        }

        public void Dispose()
        {
            oMail.Dispose();
        }
               
        public void SendMail_BodyEncoding_Text(string pStrAsunto, string pStrBody, string pStrTo)
        {
            var objMailMessage = new MailMessage();
            System.Net.Mail.SmtpClient oSmtp = new System.Net.Mail.SmtpClient();
            try
            {
                var objFrom = new MailAddress("sms@gruponuevomundo.com.pe", "sms@gruponuevomundo.com.pe");
                objMailMessage.To.Add(pStrTo);
                objMailMessage.From = objFrom;
                objMailMessage.Subject = pStrAsunto;
                objMailMessage.Body = pStrBody;
                objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                objMailMessage.IsBodyHtml = false;

                oSmtp.UseDefaultCredentials = true;
                oSmtp.Credentials = new System.Net.NetworkCredential("sms@gruponuevomundo.com.pe", "nms1m4sSM");
                oSmtp.EnableSsl = false;
                oSmtp.Port = 25;
                oSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                oSmtp.Host = "10.75.102.2";
                oSmtp.Send(objMailMessage);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                objMailMessage = null;
            }
        }
        public void SendMail_Test(string pStrAsunto, string pStrBody, string pStrTo)
        {
            var objMailMessage = new MailMessage();
            System.Net.Mail.SmtpClient oSmtp = new System.Net.Mail.SmtpClient();
            try
            {
                var objFrom = new MailAddress("webmaster@nmviajes.com");
                objMailMessage.To.Add(pStrTo);
                objMailMessage.From = objFrom;
                objMailMessage.Subject = pStrAsunto;
                objMailMessage.Body = pStrBody;
                objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                objMailMessage.IsBodyHtml = false;

                oSmtp.UseDefaultCredentials = false;
                oSmtp.Credentials = new System.Net.NetworkCredential("clientes@nmviajes.com", "", "smtp.office365.com");
                oSmtp.EnableSsl = true;
                oSmtp.Port = 587;
                oSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                oSmtp.Host = "smtp.office365.com";
                oSmtp.Send(objMailMessage);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                objMailMessage = null;
            }
        }

    }
}