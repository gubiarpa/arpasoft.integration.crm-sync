using EnvioAlertas;
using Expertia.Estructura.Controllers.Base;
using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.NuevoMundo;
using Expertia.Estructura.Models.Retail;
using Expertia.Estructura.Repository.AppWebs;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.General;
using Expertia.Estructura.Repository.NuevoMundo;
using Expertia.Estructura.Repository.Retail;
using Expertia.Estructura.Utils;
using Expertia.Estructura.ws_pax_aux.ws_pax_paxdoc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;

namespace Expertia.Estructura.Controllers
{
    /// CRMC008 : Registro y actualización de Oportunidad de venta en Sistema Expertia
    [RoutePrefix(RoutePrefix.OportunidadVentaNM)]
    public class OportunidadVentaNMController : BaseController
    {
        #region Properties
        private IOportunidadVentaNMRepository _oportunidadVentaNMRepository;
        private ICotizacionSRV_Repository _cotizacionSRV_Repository;
        private OportunidadRetailRepository _repository;
        private DatosUsuario _datosUsuario;
        #endregion

        protected override ControllerName _controllerName => ControllerName.OportunidadVentaNM;

        #region PublicMethods
        /*CRMC008*/
        [Route(RouteAction.Create)]
        public IHttpActionResult Create(OportunidadVentaNM oportunidadVentaNM)
        {            
            string exMessage = string.Empty;            
            object objRespuesta = null;            
            int? idCotizacion = 0;

            RptaOportunidadVentaNM rptaOportunidadVentaNM = new RptaOportunidadVentaNM();
            _repository = new OportunidadRetailRepository();            

            try
            {
                int? intIdCliCot = null;
                List<ClienteCot> ListClientes = null;
                ClienteCot Cliente_Cot = null;
                CotizacionVta DtsCotizacionVta = null;
                UsuarioLogin usuarioLogin = null;
                DateTime _fechaIgnoredTriggerCuenta = Convert.ToDateTime(ConfigurationManager.AppSettings["DATO_IGNORED_TRIGGER_CUENTA"]);

                valCreateOportunidadNM(ref oportunidadVentaNM, ref rptaOportunidadVentaNM, ref usuarioLogin, ref DtsCotizacionVta);
                if (string.IsNullOrEmpty(rptaOportunidadVentaNM.codigo) == false) return Ok(new { respuesta = rptaOportunidadVentaNM });

                //int intIdUsuWeb = oportunidadVentaNM.UsuarioCrea;
                int intIdDep = usuarioLogin.IdDep;
                int intIdOfi = usuarioLogin.IdOfi;
                string strIPUsuario = "127.0.0.0";
                string strTextoPost = string.Empty;
                bool bolCambioEstado = false, bolEsUrgenteEmision = false;                
                DateTime? datFechaPlazoEmision = null;

                #region ProcesoClientes
                Cliente_Cot = (ClienteCot)_oportunidadVentaNMRepository.SelectByCodeSF(oportunidadVentaNM.idCuenta_SF)["pCurResult_out"];

                if (!(Cliente_Cot != null))
                {                
                    if (string.IsNullOrEmpty(oportunidadVentaNM.IdTipoDoc) == false && string.IsNullOrEmpty(oportunidadVentaNM.NumDoc) == false && (new List<string> { "DNI", "PSP", "CEX" }).Contains(oportunidadVentaNM.IdTipoDoc))
                        ListClientes = (List<ClienteCot>)_repository.SelectByDocumento(oportunidadVentaNM.IdTipoDoc, oportunidadVentaNM.NumDoc)["pCurResult_out"];
                    else
                        ListClientes = (List<ClienteCot>)_repository.SelectByEmail(oportunidadVentaNM.EmailCli)["pCurResult_out"];

                    if (ListClientes != null && ListClientes.Count > 0)
                        Cliente_Cot = ListClientes[0];
                }       
                                
                if (Cliente_Cot != null)
                {                    
                    intIdCliCot = Cliente_Cot.IdCliCot;
                    /*Update*/
                    _oportunidadVentaNMRepository._Update((int)intIdCliCot, oportunidadVentaNM.NombreCli, oportunidadVentaNM.ApePatCli,
                        oportunidadVentaNM.ApeMatCli, oportunidadVentaNM.EmailCli, Cliente_Cot.EmailAlterCliCot, null,
                        oportunidadVentaNM.EnviarPromociones.Equals("1"), Cliente_Cot.Direccion,
                        oportunidadVentaNM.NumDoc, oportunidadVentaNM.IdTipoDoc, oportunidadVentaNM.IdUsuarioSrv_SF,
                        Webs_Cid.ID_WEB_WEBFAREFINDER, null, _fechaIgnoredTriggerCuenta);

                    bool valor = _oportunidadVentaNMRepository._Update_Estado_Promociones((int)intIdCliCot, oportunidadVentaNM.EnviarPromociones);

                    if (Cliente_Cot.RecibePromo == false & oportunidadVentaNM.EnviarPromociones.Equals("1"))
                    {
                        // Antes no estaba suscrito
                        NMMail.Mail_AgregaEmailListaBoletinNMV(7, 1, (oportunidadVentaNM.NombreCli + " " + oportunidadVentaNM.ApePatCli).ReplaceSpecialChars(), oportunidadVentaNM.EmailCli, Constantes_Mail.EMAIL_BOLETIN_NMV, true);

                        if ((intIdOfi == Constantes_SRV.INT_ID_OFI_CORPORATIVO_VACACIONAL & intIdDep == Constantes_SRV.INT_ID_DEP_COUNTER) | (intIdOfi == Constantes_SRV.INT_ID_OFI_NMV & intIdDep == Constantes_SRV.INT_ID_DEP_COUNTER))
                            NMMail.Mail_AgregaEmailListaBoletinNMV(7, 1, (oportunidadVentaNM.NombreCli + " " + oportunidadVentaNM.ApePatCli).ReplaceSpecialChars(), oportunidadVentaNM.EmailCli, Constantes_Mail.EMAIL_BOLETIN_CORPORATIVO_VACACIONAL, true);
                        else if (intIdOfi == Constantes_SRV.INT_ID_OFI_CALL_CENTER)
                            NMMail.Mail_AgregaEmailListaBoletinNMV(163, 1, (oportunidadVentaNM.NombreCli + " " + oportunidadVentaNM.ApePatCli).ReplaceSpecialChars(), oportunidadVentaNM.EmailCli, Constantes_Mail.EMAIL_BOLETIN_RIPLEY, true);
                    }
                    else if (Cliente_Cot.RecibePromo == true & !oportunidadVentaNM.EnviarPromociones.Equals("1"))
                    {
                        // Estaba suscrito y ahora ya no lo va a estar
                        NMMail.Mail_AgregaEmailListaBoletinNMV(7, 1, (oportunidadVentaNM.NombreCli + " " + oportunidadVentaNM.ApePatCli).ReplaceSpecialChars(), oportunidadVentaNM.EmailCli, Constantes_Mail.EMAIL_BOLETIN_NMV, false);

                        if (intIdOfi == Constantes_SRV.INT_ID_OFI_CORPORATIVO_VACACIONAL & intIdDep == Constantes_SRV.INT_ID_DEP_COUNTER)
                            NMMail.Mail_AgregaEmailListaBoletinNMV(7, 1, (oportunidadVentaNM.NombreCli + " " + oportunidadVentaNM.ApePatCli).ReplaceSpecialChars(), oportunidadVentaNM.EmailCli, Constantes_Mail.EMAIL_BOLETIN_CORPORATIVO_VACACIONAL, false);
                        else if (intIdOfi == Constantes_SRV.INT_ID_OFI_CALL_CENTER)
                            NMMail.Mail_AgregaEmailListaBoletinNMV(163, 1, (oportunidadVentaNM.NombreCli + " " + oportunidadVentaNM.ApePatCli).ReplaceSpecialChars(), oportunidadVentaNM.EmailCli, Constantes_Mail.EMAIL_BOLETIN_RIPLEY, false);
                    }
                }
                else 
                {
                    /*Insert*/
                    intIdCliCot = (int)_repository.InsertaClienteCotizacion(oportunidadVentaNM.NombreCli,oportunidadVentaNM.ApePatCli,
                                oportunidadVentaNM.ApeMatCli,oportunidadVentaNM.EmailCli,null,null,oportunidadVentaNM.EnviarPromociones.Equals("1"),
                                null,oportunidadVentaNM.NumDoc,oportunidadVentaNM.IdTipoDoc,oportunidadVentaNM.IdUsuarioSrv_SF,
                                Webs_Cid.ID_WEB_WEBFAREFINDER,null,false,null, _fechaIgnoredTriggerCuenta)["pNumIdNewCliCot_out"];

                    if (intIdCliCot != null && intIdCliCot > 0)
                    {
                        _oportunidadVentaNMRepository.RegistraCuenta(oportunidadVentaNM.idCuenta_SF, (int)intIdCliCot);                        
                    }
                        

                    /*START INSERT PAX PTA (Consultar si lo sacamos)*/
                    //try
                    //{
                    //    string PrimerNombre = string.Empty;
                    //    string SegundoNombre = string.Empty;                        

                    //    if (oportunidadVentaNM.NombreCli.Split(' ').Length > 0)
                    //        PrimerNombre = oportunidadVentaNM.NombreCli.Split(' ')[0];
                    //    if (oportunidadVentaNM.NombreCli.Split(' ').Length > 1)
                    //    {
                    //        SegundoNombre = oportunidadVentaNM.NombreCli.Split(' ')[1];
                    //        if (oportunidadVentaNM.NombreCli.Split(' ').Length > 2)
                    //            SegundoNombre += " " + oportunidadVentaNM.NombreCli.Split(' ')[2];
                    //    }
                    //    ws_pax_paxdoc InsertClienteNM = new ws_pax_paxdoc();
                    //    ws_pax_aux.ws_pax_paxdoc.r_message[] respond = InsertClienteNM.prc_web_insert_pax(oportunidadVentaNM.IdTipoDoc, oportunidadVentaNM.NumDoc,
                    //        oportunidadVentaNM.ApePatCli, oportunidadVentaNM.ApeMatCli, PrimerNombre, SegundoNombre, string.Empty, oportunidadVentaNM.EmailCli, string.Empty, string.Empty, string.Empty, string.Empty, usuarioLogin.LoginUsuario);
                    //}
                    //catch (Exception ex)
                    //{                                                
                    //    NMailAlerta oNMailAlerta = new NMailAlerta();
                    //    oNMailAlerta.EnvioCorreoRegistrarError("Error de " + Constantes_SRV.APP_NAME, this, ex, strIPUsuario + "|btnGrabarCli_ServerClick");
                    //    oNMailAlerta = null;
                    //}
                    /*END INSERT PAX PTA*/

                    if (oportunidadVentaNM.EnviarPromociones.Equals("1"))
                    {                        
                        if ((intIdOfi == Constantes_SRV.INT_ID_OFI_CORPORATIVO_VACACIONAL & intIdDep == Constantes_SRV.INT_ID_DEP_COUNTER))
                            NMMail.Mail_AgregaEmailListaBoletinNMV(7, 1, (oportunidadVentaNM.NombreCli + " " + oportunidadVentaNM.ApePatCli).ReplaceSpecialChars(), oportunidadVentaNM.EmailCli, Constantes_Mail.EMAIL_BOLETIN_CORPORATIVO_VACACIONAL, true);

                        NMMail.Mail_AgregaEmailListaBoletinNMV(7, 1, (oportunidadVentaNM.NombreCli + " " + oportunidadVentaNM.ApePatCli).ReplaceSpecialChars(), oportunidadVentaNM.EmailCli, Constantes_Mail.EMAIL_BOLETIN_NMV, true);                        
                    }
                }
                #endregion


                #region ProcesoCotizacion
                if (oportunidadVentaNM.IdCotSRV == null)
                {
                    /*Creamos la Cotizacion*/                                                            
                    if (usuarioLogin.IdUsuario == 1813)
                    {                            
                        intIdDep = 30;
                        intIdOfi = 23;
                    }

                    idCotizacion = _oportunidadVentaNMRepository.Inserta_Cot_Vta(oportunidadVentaNM.ModoIngreso, oportunidadVentaNM.Comentario,
                        usuarioLogin.NomCompletoUsuario, usuarioLogin.LoginUsuario, strIPUsuario, (int)intIdCliCot,
                        usuarioLogin.IdUsuario, intIdDep, intIdOfi, Webs_Cid.ID_WEB_WEBFAREFINDER, Lang_Cid.IdLangSpa,
                        oportunidadVentaNM.IdCanalVenta, oportunidadVentaNM.ServiciosAdicionales.Split(','),
                        oportunidadVentaNM.CiudadIata, null, 0, oportunidadVentaNM.IdDestino, oportunidadVentaNM.FechaIngreso,
                        oportunidadVentaNM.Fecharegreso, oportunidadVentaNM.CantidadAdultos, oportunidadVentaNM.CantidadNinos,
                        string.Empty, null, null, null, null, null, null, null, null, string.Empty, null);

                    if(idCotizacion != null && idCotizacion > 0)
                    {
                        /*_repository.RegistraOportunidad(oportunidadVentaNM.IdOportunidad_SF, (int)idCotizacion);*/
                        _oportunidadVentaNMRepository.RegistraOportunidad(oportunidadVentaNM.IdOportunidad_SF, (int)idCotizacion);
                    }

                    /*DtsCotizacionVta = _cotizacionSRV_Repository.Get_Datos_CotizacionVta((int)idCotizacion);*/
                }
                else {
                    idCotizacion = oportunidadVentaNM.IdCotSRV;

                    /**Cambios de Estado**/
                    if (oportunidadVentaNM.Estado != DtsCotizacionVta.IdEstado)
                    {
                        bolCambioEstado = true;
                        if (oportunidadVentaNM.Estado == (short)ENUM_ESTADOS_COT_VTA.NoCompro)
                        {
                            //strTextoPost = "<span class='texto_cambio_estado'>Cambio de estado a <strong>No Compro</strong></span>";
                        }
                    }

                    if (oportunidadVentaNM.Estado == (short)ENUM_ESTADOS_COT_VTA.DerivadoCA || oportunidadVentaNM.Estado == (short)ENUM_ESTADOS_COT_VTA.DerivadoCA_Paq)
                    {
                        bolEsUrgenteEmision = true;
                        datFechaPlazoEmision = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " " + oportunidadVentaNM.HoraEmision + ":00");
                        //strTextoPost = "<span class='texto_cambio_estado'>EMISIÓN ANTES DE: " + string.Format("dd/MM/yyyy HH:mm", datFechaPlazoEmision) + "</span><br>";
                    }

                    /*Es Emitido*/                    
                    if (oportunidadVentaNM.Emitido == true)
                    {
                        _cotizacionSRV_Repository._Update_EsEmitido((int)oportunidadVentaNM.IdCotSRV, true);                        
                    }

                    /*Validar las validaciones a considerar*/
                    /*if (usuarioLogin.IdOfi == Constantes_SRV.INT_ID_OFI_NMVCOM || (usuarioLogin.IdOfi == Constantes_SRV.INT_ID_OFI_NMV && usuarioLogin.IdDep == Constantes_SRV.INT_ID_DEP_EMERGENCIAS) || usuarioLogin.EsSupervisorSRV)
                    {
                        if ((!DtsCotizacionVta.EsEmitido) && oportunidadVentaNM.Emitido != null && oportunidadVentaNM.Emitido == true)
                        {
                            _cotizacionSRV_Repository._Update_EsEmitido((int)oportunidadVentaNM.IdCotSRV, true);
                        }
                    }*/

                    /*Asignarse*/
                    if (oportunidadVentaNM.Asignarse == true)
                    {
                        bool bolAsignado = _cotizacionSRV_Repository._Update_CounterAdministrativo((int)oportunidadVentaNM.IdCotSRV, usuarioLogin.IdUsuario);
                        //messageOK = (bolAsignado == false ? "La cotización ya ha sido asignado a un counter administrativo." : messageOK);
                    }

                    /*Requiere Firma Cliente (Consultar validacion de Pedidos - Del lado de SF?)*/
                    if (oportunidadVentaNM.RequiereFirmaCliente != null)
                    {
                        _cotizacionSRV_Repository._Update_Requiere_FirmaCliente_Cot((int)oportunidadVentaNM.IdCotSRV, (bool)oportunidadVentaNM.RequiereFirmaCliente);
                    }                        

                    /*Insertamos el Post*/
                    _cotizacionSRV_Repository.Inserta_Post_Cot(DtsCotizacionVta.IdCot, Constantes_SRV.ID_TIPO_POST_SRV_USUARIO, strTextoPost,
                        strIPUsuario, usuarioLogin.LoginUsuario, usuarioLogin.IdUsuario, usuarioLogin.IdDep, usuarioLogin.IdOfi, null, null,
                        oportunidadVentaNM.Estado, bolCambioEstado, null, false, null, usuarioLogin.EsCounterAdminSRV,
                        (usuarioLogin.EsCounterAdminSRV == true ? DtsCotizacionVta.IdUsuWeb : usuarioLogin.IdUsuario),
                        (usuarioLogin.EsCounterAdminSRV == true ? DtsCotizacionVta.IdOfi : usuarioLogin.IdOfi),
                        (usuarioLogin.EsCounterAdminSRV == true ? DtsCotizacionVta.IdDep : usuarioLogin.IdDep), bolEsUrgenteEmision,
                        datFechaPlazoEmision, oportunidadVentaNM.IdMotivoNoCompro, null, oportunidadVentaNM.MontoEstimado, 0);
                    /*Notas: En caso soliciten habilitar el Post, para el valor Emitido la estructura del Post es diferente, habria que hacer unas condicionales adicionales*/
                    /*Notas: En caso soliciten habilitar el Post, para la AutoAsignacion la estructura del Post es diferente, habria que hacer unas condicionales adicionales*/

                    /*Modalidad de Compra*/
                    if (oportunidadVentaNM.Estado == (short)ENUM_ESTADOS_COT_VTA.Facturado && (usuarioLogin.IdOfi == Constantes_SRV.INT_ID_OFI_NMVCOM || usuarioLogin.IdDep == Constantes_SRV.INT_ID_DEP_SISTEMAS || _oportunidadVentaNMRepository._EsCounterAdministratiivo(usuarioLogin.IdOfi))) //, usuarioLogin.IdDep, false))
                    {
                        _cotizacionSRV_Repository._Update_ModalidadCompra(DtsCotizacionVta.IdCot, (short)oportunidadVentaNM.ModalidadCompra);
                    }

                    if (usuarioLogin.IdOfi == Constantes_SRV.INT_ID_OFI_NMVCOM | usuarioLogin.IdOfi == Constantes_SRV.INT_ID_OFI_TRAVEL_STORE | usuarioLogin.IdDep == Constantes_SRV.INT_ID_DEP_SISTEMAS | usuarioLogin.IdOfi == Constantes_SRV.INT_ID_OFI_NMV | usuarioLogin.IdOfi == 116)
                    {
                        if (oportunidadVentaNM.MontoCompra != null && oportunidadVentaNM.Estado == (short)ENUM_ESTADOS_COT_VTA.DerivadoCA && DtsCotizacionVta.IdReservaVuelos == null)
                        {
                            _oportunidadVentaNMRepository._Update_DatosReservaVuelo_Manual_Cot(DtsCotizacionVta.IdCot, oportunidadVentaNM.CodReserva, Constantes_SRV.INT_ID_MONEDA_USD, System.Convert.ToDouble(oportunidadVentaNM.MontoCompra));
                        }
                    }
                }
                #endregion

                rptaOportunidadVentaNM.codigo = "OK";
                rptaOportunidadVentaNM.mensaje = (oportunidadVentaNM.Accion_SF.ToUpper().Trim() == "UPDATE" ? "Se actualizo correctamente" : "Se agregó correctamente");
                rptaOportunidadVentaNM.IdCotSrv = idCotizacion;                

                objRespuesta = new { respuesta = rptaOportunidadVentaNM };
                return Ok(objRespuesta);
            }
            catch (Exception ex)
            {
                exMessage = ex.Message;
                //rptaOportunidadVentaNM.codigo = "ER";
                //rptaOportunidadVentaNM.mensaje = ex.Message;
                //rptaOportunidadVentaNM.IdCotSrv = null;
                                
                //objRespuesta = new { respuesta = rptaOportunidadVentaNM };
                //return Ok(objRespuesta);
                return InternalServerError(ex);
            }
            finally
            {
                (new
                {
                    Body = oportunidadVentaNM,
                    IdCotSrv = idCotizacion,
                    Error = exMessage
                }).TryWriteLogObject(_logFileManager, _clientFeatures);
            }
        }
        #endregion

        #region Helpers
        private void valCreateOportunidadNM(ref OportunidadVentaNM _oportunidadVentaNM, ref RptaOportunidadVentaNM _rptaOportunidadVentaNM, ref UsuarioLogin UserLogin, ref CotizacionVta CotizacionVta)
        {   
            string mensajeError = string.Empty;
            
            if (_oportunidadVentaNM == null)
            {
                mensajeError += "Envie correctamente los parametros de entrada - RQ Nulo|";
            }
            if (string.IsNullOrEmpty(_oportunidadVentaNM.Accion_SF))
            {
                mensajeError += "La accion es un campo obligatorio|";
            }
            else if (_oportunidadVentaNM.Accion_SF.ToUpper().Trim() == "UPDATE" && _oportunidadVentaNM.IdCotSRV == null)
            {
                mensajeError += "El Id de Cotizacion es obligatorio al actualizar|";
            }
            else if (_oportunidadVentaNM.Accion_SF.ToUpper().Trim() == "INSERT" && _oportunidadVentaNM.IdCotSRV != null)
            {
                mensajeError += "Al insertar el Id de Cotizacion debe ser nulo|";
            }
            if (string.IsNullOrEmpty(_oportunidadVentaNM.idCuenta_SF))
            {
                mensajeError += "La cuenta SF es un campo obligatorio|";
            }
            if (string.IsNullOrEmpty(_oportunidadVentaNM.IdOportunidad_SF))
            {
                mensajeError += "La oportunidad SF es un campo obligatorio|";
            }
            if (_oportunidadVentaNM.IdCanalVenta <= 0)
            {
                mensajeError += "Envie un canal de venta correcto|";
            }
            if (string.IsNullOrEmpty(_oportunidadVentaNM.NombreCli))
            {
                mensajeError += "El nombre del cliente es un campo obligatorio|";
            }
            if (string.IsNullOrEmpty(_oportunidadVentaNM.ApePatCli))
            {
                mensajeError += "El apellido paterno del cliente es un campo obligatorio|";
            }
            if (string.IsNullOrEmpty(_oportunidadVentaNM.EmailCli))
            {
                mensajeError += "El correo del cliente es un campo obligatorio|";
            }
            if (string.IsNullOrEmpty(_oportunidadVentaNM.CiudadIata))
            {
                mensajeError += "La Ciudad Iata es un campo obligatorio|";
            }
            if (string.IsNullOrEmpty(_oportunidadVentaNM.IdDestino))
            {
                mensajeError += "Los destinos principales es un campo obligatorio|";
            }
            if (string.IsNullOrEmpty(_oportunidadVentaNM.EnviarPromociones))
            {
                mensajeError += "La opcion de envio de promociones es un campo obligatorio 0 - 1|";
            }            
            if (_oportunidadVentaNM.IdUsuarioSrv_SF <= 0)
            {
                mensajeError += "Envie un ID de usuario SRV valido|";
            }
            if (string.IsNullOrEmpty(_oportunidadVentaNM.Comentario))
            {
                mensajeError += "El comentario es un campo obligatorio|";
            }            
            if (_oportunidadVentaNM.Estado <= 0)
            {
                mensajeError += "Envie un estado valido|";
            }
            if (string.IsNullOrEmpty(_oportunidadVentaNM.ServiciosAdicionales))
            {
                mensajeError += "Los Servicios Adicionales son un campo obligatorio|";
            }
            if (_oportunidadVentaNM.ModoIngreso <= 0)
            {
                mensajeError += "Envie un Modo de Ingreso valido|";
            }
            if (_oportunidadVentaNM.CantidadAdultos <= 0)
            {
                mensajeError += "La cantidad de adultos es un campo obligatorio|";
            }
            if (_oportunidadVentaNM.CantidadNinos == null)
            {
                _oportunidadVentaNM.CantidadNinos = 0;                
            }

            /*Validacion en caso envien informacion*/           
            if (_oportunidadVentaNM.FechaIngreso != null && _oportunidadVentaNM.FechaIngreso < DateTime.Now)
            {
                mensajeError += "La fecha de salida debe ser mayor a la fecha actual|";                                
            }
            else if (_oportunidadVentaNM.FechaIngreso != null & _oportunidadVentaNM.Fecharegreso != null)
            {
                if (_oportunidadVentaNM.FechaIngreso > _oportunidadVentaNM.Fecharegreso)
                {
                    mensajeError += "La fecha de regreso debe ser mayor a la fecha de salida|";                    
                }
            }


            if (string.IsNullOrEmpty(mensajeError))
            {                
                /*Cargamos Datos del Usuario*/
                RepositoryByBusiness(null);
                UserLogin = _datosUsuario.Get_Dts_Usuario_Personal(_oportunidadVentaNM.IdUsuarioSrv_SF);
                if (UserLogin == null) { mensajeError += "ID del Usuario no registrado|"; }

                /*Validacion Oportunidad*/
                int intCotizacion_SF = _oportunidadVentaNMRepository._Select_CotId_X_OportunidadSF(_oportunidadVentaNM.IdOportunidad_SF);
                if(intCotizacion_SF <= 0 && _oportunidadVentaNM.Accion_SF.ToUpper().Trim() == "UPDATE") { mensajeError += "No es posible actualizar si la oportunidad no esta registrada|"; }
                else if (intCotizacion_SF > 0 && _oportunidadVentaNM.Accion_SF.ToUpper().Trim() == "INSERT") { mensajeError += "No es posible insertar si la oportunidad ya esta registrada|"; }
                else if (intCotizacion_SF > 0 && _oportunidadVentaNM.Accion_SF.ToUpper().Trim() == "UPDATE" && intCotizacion_SF != _oportunidadVentaNM.IdCotSRV) { mensajeError += "La cotizacion enviada es diferente a la registrada|"; }

                if (_oportunidadVentaNM.IdCotSRV != null && string.IsNullOrEmpty(mensajeError))
                {
                    CotizacionVta = _cotizacionSRV_Repository.Get_Datos_CotizacionVta((int)_oportunidadVentaNM.IdCotSRV);

                    if(CotizacionVta == null || CotizacionVta.IdCot == 0) {
                        cargarError(ref _rptaOportunidadVentaNM, "No existe informacion de la cotizacion enviada|");
                        return;
                    }

                    /*Validaciones Valores Opcionales*/                    
                    if (_oportunidadVentaNM.Estado != CotizacionVta.IdEstado)
                    {
                        if (_oportunidadVentaNM.Estado == (short)ENUM_ESTADOS_COT_VTA.DerivadoCA || _oportunidadVentaNM.Estado == (short)ENUM_ESTADOS_COT_VTA.Facturado)
                        {
                            if (_oportunidadVentaNM.Estado == (short)ENUM_ESTADOS_COT_VTA.Facturado)
                            {                                
                                if (UserLogin.IdOfi == Constantes_SRV.INT_ID_OFI_NMVCOM | UserLogin.IdDep == Constantes_SRV.INT_ID_DEP_SISTEMAS)
                                {
                                    if(!(_oportunidadVentaNM.ModalidadCompra != null && (_oportunidadVentaNM.ModalidadCompra == 0 || _oportunidadVentaNM.ModalidadCompra == 1)))
                                    {
                                        cargarError(ref _rptaOportunidadVentaNM, "Debe enviar la modalidad de compra|");
                                        return;
                                    }
                                }
                            }

                            if (_oportunidadVentaNM.Estado == (short)ENUM_ESTADOS_COT_VTA.DerivadoCA && 
                                (UserLogin.IdOfi == Constantes_SRV.INT_ID_OFI_NMVCOM | UserLogin.IdOfi == Constantes_SRV.INT_ID_OFI_TRAVEL_STORE |
                                UserLogin.IdDep == Constantes_SRV.INT_ID_DEP_SISTEMAS))
                            {
                                if (string.IsNullOrEmpty(_oportunidadVentaNM.HoraEmision))
                                {
                                    cargarError(ref _rptaOportunidadVentaNM, "Debe enviar la hora de emisión|");
                                    return;                                                                      
                                }
                            }
                            
                            List<int> lstPedidosSinBanco = _oportunidadVentaNMRepository._Select_Pedidos_SinBancoBy_IdCot((int)_oportunidadVentaNM.IdCotSRV);
                            if (lstPedidosSinBanco.Count > 0)
                            {
                                if (lstPedidosSinBanco.Count == 1)
                                {
                                    cargarError(ref _rptaOportunidadVentaNM, ("Para cambiar a este estado debe registrar la entidad bancaria en el pedido nro. " + lstPedidosSinBanco[0]));
                                    return;
                                }
                                else
                                {
                                    string strNrosPedido = "";
                                    foreach (int intIdPedido in lstPedidosSinBanco)
                                        strNrosPedido += intIdPedido + ", ";
                                    strNrosPedido += ".";
                                    strNrosPedido = strNrosPedido.Replace(", .", ".");                                    
                                    cargarError(ref _rptaOportunidadVentaNM, ("Para cambiar a este estado debe registrar la entidad bancaria en los sgtes. pedidos: " + strNrosPedido));
                                    return;
                                }                                
                            }

                            if (CotizacionVta.IdReservaVuelos == null && (_oportunidadVentaNM.Estado == (short)ENUM_ESTADOS_COT_VTA.DerivadoCA || _oportunidadVentaNM.Estado == (short)ENUM_ESTADOS_COT_VTA.DerivadoCA_Paq))
                            {
                                if (string.IsNullOrEmpty(_oportunidadVentaNM.CodReserva))
                                {
                                    cargarError(ref _rptaOportunidadVentaNM, "Debe ingresar el código de reserva de vuelo.");
                                    return;                                    
                                }
                                else if (_oportunidadVentaNM.MontoCompra == null)
                                {
                                    cargarError(ref _rptaOportunidadVentaNM, "Debe ingresar el monto de la venta.");
                                    return;                                  
                                }                                
                            }
                        }
                    }

                    if (_oportunidadVentaNM.Estado == (short)ENUM_ESTADOS_COT_VTA.Cotizado)
                    {
                        if (_oportunidadVentaNMRepository._EsCounterAdministratiivo(UserLogin.IdOfi))
                        {
                            if (_oportunidadVentaNM.MontoEstimado == null)
                            {
                                cargarError(ref _rptaOportunidadVentaNM, "Debe ingresar el monto estimado del file.");
                                return;                               
                            }                           
                        }
                    }

                    /*Validacion : Asignarse - Emitido*/
                    bool Asignar = false, Emitir = false; ;
                    if (_oportunidadVentaNM.Asignarse == true || _oportunidadVentaNM.Emitido == true)
                    {                    
                        if (UserLogin.IdOfi == Constantes_SRV.INT_ID_OFI_NMVCOM || (UserLogin.IdOfi == Constantes_SRV.INT_ID_OFI_NMV && UserLogin.IdDep == Constantes_SRV.INT_ID_DEP_EMERGENCIAS) || UserLogin.EsSupervisorSRV)
                        {
                            if (CotizacionVta.IdUsuWebCA.HasValue)
                            {
                                if (UserLogin.IdUsuario == CotizacionVta.IdUsuWebCA.Value)
                                    Asignar = false;
                                else
                                    Asignar = true;
                            }
                            else
                                Asignar = true;

                            if (!CotizacionVta.EsEmitido)
                                Emitir = true;
                        }
                        else if (UserLogin.EsCounterAdminSRV)
                        {
                            if (_oportunidadVentaNMRepository._EsCounterAdministratiivo(CotizacionVta.IdOfi) && _oportunidadVentaNMRepository._EsCounterAdministratiivo(UserLogin.IdOfi))
                            {
                                if (CotizacionVta.IdUsuWebCA.HasValue)
                                {
                                    if (UserLogin.IdUsuario == CotizacionVta.IdUsuWebCA.Value)
                                        Asignar = false;
                                    else
                                        Asignar = true;
                                }
                                else
                                    Asignar = true;
                            }
                        }

                        /*Validacion Asignarse (Actualizamos la Asignacion)*/
                        _oportunidadVentaNM.Asignarse = Asignar;

                        /*Validacion Emitido (Actualizamos el p_Emitido)*/
                        _oportunidadVentaNM.Emitido = Emitir;
                    }                     
                }
            }

            if (string.IsNullOrEmpty(mensajeError) == false)
            {             
                _rptaOportunidadVentaNM.codigo = "ER";
                _rptaOportunidadVentaNM.mensaje = "VA: " + mensajeError;
            }
        }

        private void cargarError(ref RptaOportunidadVentaNM rpOportunidadVentaNM, string errorText)
        {
            rpOportunidadVentaNM.codigo = "ER";
            rpOportunidadVentaNM.mensaje = "VA: " + errorText;
        }

        protected override UnidadNegocioKeys? RepositoryByBusiness(UnidadNegocioKeys? unidadNegocioKey)
        {
            unidadNegocioKey = (unidadNegocioKey == null ? UnidadNegocioKeys.AppWebs : unidadNegocioKey);
            _datosUsuario = new DatosUsuario();
            _cotizacionSRV_Repository = new CotizacionSRV_AW_Repository();
            _oportunidadVentaNMRepository = new OportunidadVentaNMRepository();
            return unidadNegocioKey;            
        }
        #endregion
    }
}
