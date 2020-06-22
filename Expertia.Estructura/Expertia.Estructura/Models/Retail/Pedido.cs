using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Behavior;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    public class Pedido : IUnidadNegocio
    {
        public string IdUsuario { get; set; }
        public int? IdLang { get; set; }
        public int? IdWeb { get; set; }
        public string IPUsuario { get; set; }
        public string Browser { get; set; }
        public string DetalleServicio { get; set; }
        public string CodePasarelaPago { get; set; }
        public string Email { get; set; }
        public int? TiempoExpiracionCIP { get; set; }
        public string Monto { get; set; }
        public int IdCotVta { get; set; }
        public int? IdCanalVta { get; set; }
        public string NombreClienteCot { get; set; }
        public string ApellidoClienteCot { get; set; }
        public UnidadNegocio UnidadNegocio { get; set; }
        public string IdOportunidad_SF {get;set;}
        public string IdSolicitudpago_SF { get; set; }
    }

    public class DatosPedido : IUnidadNegocio
    {
        public string accion_SF { get; set; }
        public string IdUsuario { get; set; }
        public int? IdLang { get; set; }
        public int? IdWeb { get; set; }
        public string IPUsuario { get; set; }
        public string Browser { get; set; }
        public string DetalleServicio { get; set; }
        public string CodePasarelaPago { get; set; }
        public string Email { get; set; }
        public int? TiempoExpiracionCIP { get; set; }
        public string Monto { get; set; }
        public int IdCotVta { get; set; }
        public int? IdCanalVta { get; set; }
        public string NombreClienteCot { get; set; }
        public string ApellidoClienteCot { get; set; }
        public UnidadNegocio UnidadNegocio { get; set; }
        public string IdOportunidad_SF { get; set; }
        public string IdSolicitudpago_SF { get; set; }
        public string Codigo { get; set; }
        public string FechaRegistro { get; set; }
        public string Estado { get; set; }
        public string ModoIngreso { get; set; }
        public string CanalVenta { get; set; }
        public string Empresa { get; set; }
        public string Cliente { get; set; }
        public string DatosContacto { get; set; }
        public string EmailContacto { get; set; }
        public string RegistradoPor { get; set; }
        public string MetaBuscador { get; set; }
        public string OrdenAtencion { get; set; }
        public string Evento { get; set; }
        public string UsuarioLogin { get; set; }
        public string Moneda { get; set; }
        public string NumeroPedido { get; set; }
        public string TipoPasarela { get; set; }
        public string CodigoAutorizacion { get; set; }
        public string FechaPedido { get; set; }
        public string CodigoLineaAerea { get; set; }
        public string DetalleServicioEc { get; set; }
        public double TotalPagar { get; set; }
        public string ExpiracionPedido { get; set; }
        public string MailDestinatario { get; set; }
        public string Resultado { get; set; }
        public string FechaPasarela { get; set; }
        public double MontoPagar { get; set; }
        public string LineaAereaValidadora { get; set; }
        public string RCGenerado { get; set; }
        public string FormaPago { get; set; }
        public string NumeroTarjeta { get; set; }
        public string TipoTarjeta { get; set; }
        public string EntidadBancaria { get; set; }
        public string TitularTarjeta { get; set; }
        public string DocumentoTitular { get; set; }
        public string Expiracion { get; set; }
        public string THReniec { get; set; }
        public string MarcaTipoCambio { get; set; }
        public string TipoTipoCambio { get; set; }
        public string NivelTipoCambio { get; set; }
        public string PaisTipoCambio { get; set; }
        public string IPCliente { get; set; }
        public float? Fee { get; set; }
        public float? Pef { get; set; }
        public float? Gem { get; set; }
        public int? NumCuotas { get; set; }
    }

    #region NuevoMundo
    public class DatosPedido_NM
    {
        public string Accion_SF { get; set; }
        public string IdUsuario { get; set; }
        public int? IdLang { get; set; }
        public int? IdWeb { get; set; }
        public string IPUsuario { get; set; }
        public string Browser { get; set; }
        public string DetalleServicio { get; set; }
        public string CodePasarelaPago { get; set; }
        public string Email { get; set; }
        public int? TiempoExpiracionCIP { get; set; }
        public string Monto { get; set; }
        public int IdCotVta { get; set; }
        public int? IdCanalVta { get; set; }
        public string NombreClienteCot { get; set; }
        public string ApellidoClienteCot { get; set; }
        public UnidadNegocio UnidadNegocio { get; set; }
        public string IdOportunidad_SF { get; set; }
        public string IdSolicitudpago_SF { get; set; }
        public SRV_NM SRV { get; set; }
        public Pedido_NM Pedido { get; set; }
        public int? NumCuotas { get; set; }
        #region ToRetail
        public DatosPedido ToRetail()
        {
            var datosPedido = new DatosPedido()
            {
                accion_SF = this.Accion_SF,
                IdUsuario = this.IdUsuario,
                IdLang = this.IdLang,
                IdWeb = this.IdWeb,
                IPUsuario = this.IPUsuario,
                Browser = this.Browser,
                DetalleServicio = this.DetalleServicio,
                CodePasarelaPago = this.CodePasarelaPago,
                Email = this.Email,
                TiempoExpiracionCIP = this.TiempoExpiracionCIP,
                Monto = this.Monto,
                IdCotVta = this.IdCotVta,
                IdCanalVta = this.IdCanalVta,
                NombreClienteCot = this.NombreClienteCot,
                ApellidoClienteCot = this.ApellidoClienteCot,
                UnidadNegocio = this.UnidadNegocio,
                IdOportunidad_SF = this.IdOportunidad_SF,
                IdSolicitudpago_SF = this.IdSolicitudpago_SF,
                #region SRV
                Codigo = this.SRV.Codigo,
                FechaRegistro = this.SRV.FechaRegistro,
                Estado = this.SRV.Estado,
                ModoIngreso = this.SRV.ModoIngreso,
                CanalVenta = this.SRV.CanalVenta,
                Empresa = this.SRV.Empresa,
                Cliente = this.SRV.Cliente,
                DatosContacto = this.SRV.DatosContacto,
                EmailContacto = this.SRV.EmailContacto,
                RegistradoPor = this.SRV.RegistradoPor,
                MetaBuscador = this.SRV.MetaBuscador,
                OrdenAtencion = this.SRV.OrdenAtencion,
                Evento = this.SRV.Evento,
                UsuarioLogin = this.SRV.UsuarioLogin,
                Moneda = this.SRV.Moneda,
                #endregion
                #region Pedido
                NumeroPedido = this.Pedido.NumeroPedido,
                TipoPasarela = this.Pedido.TipoPasarela,
                CodigoAutorizacion = this.Pedido.CodigoAutorizacion,
                FechaPedido = this.Pedido.FechaPedido,
                CodigoLineaAerea = this.Pedido.CodigoLineaAerea,
                DetalleServicioEc = this.Pedido.DetalleServicioEc,
                TotalPagar = this.Pedido.TotalPagar,
                ExpiracionPedido = this.Pedido.ExpiracionPedido,
                MailDestinatario = this.Pedido.MailDestinatario,
                #endregion
                #region Pedido.Pasarela
                Resultado = this.Pedido.Pasarela.Resultado,
                FechaPasarela = this.Pedido.Pasarela.FechaPasarela,
                MontoPagar = this.Pedido.Pasarela.MontoPagar,
                LineaAereaValidadora = this.Pedido.Pasarela.LineaAereaValidadora,
                RCGenerado = this.Pedido.Pasarela.RCGenerado,
                FormaPago = this.Pedido.Pasarela.FormaPago,
                NumeroTarjeta = this.Pedido.Pasarela.NumeroTarjeta,
                TipoTarjeta = this.Pedido.Pasarela.TipoTarjeta,
                EntidadBancaria = this.Pedido.Pasarela.EntidadBancaria,
                TitularTarjeta = this.Pedido.Pasarela.TitularTarjeta,
                DocumentoTitular = this.Pedido.Pasarela.DocumentoTitular,
                Expiracion = this.Pedido.Pasarela.Expiracion,
                THReniec = this.Pedido.Pasarela.THReniec,
                MarcaTipoCambio = this.Pedido.Pasarela.MarcaTipoCambio,
                TipoTipoCambio = this.Pedido.Pasarela.TipoTipoCambio,
                NivelTipoCambio = this.Pedido.Pasarela.NivelTipoCambio,
                PaisTipoCambio = this.Pedido.Pasarela.PaisTipoCambio,
                IPCliente = this.Pedido.Pasarela.IPCliente,
                #endregion
                #region Pedido.DesgloseFeeFacturacion
                Fee = this.Pedido.DesgloseFeeFacturacion.Fee,
                Pef = this.Pedido.DesgloseFeeFacturacion.Pef,
                Gem = this.Pedido.DesgloseFeeFacturacion.Gem,
                #endregion
                NumCuotas = this.NumCuotas
            };

            return datosPedido;
        }
        #endregion
    }

    public class SRV_NM
    {
        public string Codigo { get; set; }
        public string FechaRegistro { get; set; }
        public string Estado { get; set; }
        public string ModoIngreso { get; set; }
        public string CanalVenta { get; set; }
        public string Empresa { get; set; }
        public string Cliente { get; set; }
        public string DatosContacto { get; set; }
        public string EmailContacto { get; set; }
        public string RegistradoPor { get; set; }
        public string MetaBuscador { get; set; }
        public string OrdenAtencion { get; set; }
        public string Evento { get; set; }
        public string UsuarioLogin { get; set; }
        public string Moneda { get; set; }
    }

    public class Pedido_NM
    {
        public string NumeroPedido { get; set; }
        public string TipoPasarela { get; set; }
        public string CodigoAutorizacion { get; set; }
        public string FechaPedido { get; set; }
        public string CodigoLineaAerea { get; set; }
        public string DetalleServicioEc { get; set; }
        public double TotalPagar { get; set; }
        public string ExpiracionPedido { get; set; }
        public string MailDestinatario { get; set; }
        public Pasarela_NM Pasarela { get; set; }
        public DesgloseFeeFact_NM DesgloseFeeFacturacion { get; set; }
    }

    public class Pasarela_NM
    {
        public string Resultado { get; set; }
        public string FechaPasarela { get; set; }
        public double MontoPagar { get; set; }
        public string LineaAereaValidadora { get; set; }
        public string RCGenerado { get; set; }
        public string FormaPago { get; set; }
        public string NumeroTarjeta { get; set; }
        public string TipoTarjeta { get; set; }
        public string EntidadBancaria { get; set; }
        public string TitularTarjeta { get; set; }
        public string DocumentoTitular { get; set; }
        public string Expiracion { get; set; }
        public string THReniec { get; set; }
        public string MarcaTipoCambio { get; set; }
        public string TipoTipoCambio { get; set; }
        public string NivelTipoCambio { get; set; }
        public string PaisTipoCambio { get; set; }
        public string IPCliente { get; set; }
    }

    public class DesgloseFeeFact_NM
    {
        public float? Fee { get; set; }
        public float? Pef { get; set; }
        public float? Gem { get; set; }

    }
    #endregion

    public class PedidoRS : ICrmApiResponse
    {
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public int IdPedido { get; set; }
        public string LinkPago { get; set; }
        public string CodigoCIP { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public string FechaExp { get { return FechaExpiracion.ToString("yyyy-MM-dd HH:mm:ss.fff"); } }
        public string CodigoTransaction { get; set; }
        public string CodigoOperacion { get; set; }
        public bool CorreoEnviado { get; set; }
    }

    public class PedidosProcesados
    {
        public int idPedido { get; set; }
        public string codigoTransaccion { get; set; } /*Codigo de pago generado por SafetyPay*/
        public string idSolicitudPago_SF { get; set; } /*Identificador de la solicitud de pago (Oportunidad SaleForce)*/
        public string estadoPago { get; set; } /*estado de pago*/
        public string estadoProcess { get; set; }
    }

    public class FormaPagoPedido
    {
        public int IdPedido { get; set; }
        private string strIdTipoTarjeta;
        public int TotalPtos { get; set; }
        public int PtosPagoCliente { get; set; }
        public double MontoPago { get; set; }
        public double PuntosXDolar { get; set; }
        public double TipoCambio { get; set; }
        private string strCodAutorizacionPtos;
        public Int16 IdFormaPago { get; set; }
        private string strNomTarjeta;
        private string strNomFormaPago;
        private string strTitularTarjeta;
        public Nullable<short> NroCuotas { get; set; } = null;
        private string strTipoCuotas;
        public string AutorizationTarjeta { get; set; }
        public string BancoEmisorTarjeta { get; set; }
        public string EsPayu { get; set; }
        public string TypeCard { get; set; }
        
        public string IdTipoTarjeta
        {
            get{return strIdTipoTarjeta;}
            set{strIdTipoTarjeta = Strings.Trim(value);}
        }

        public string CodAutorizacionPtos
        {
            get{return strCodAutorizacionPtos;}
            set{strCodAutorizacionPtos = Strings.Trim(value);}
        }
                
        public string NomTarjeta
        {
            get{return strNomTarjeta;}
            set{strNomTarjeta = Strings.Trim(value);}
        }

        public string NomFormaPago
        {
            get{return strNomFormaPago;}
            set{strNomFormaPago = Strings.Trim(value);}
        }

        public string TitularTarjeta
        {
            get{return strTitularTarjeta;}
            set{strTitularTarjeta = Strings.Trim(value);}
        }
        
        public string TipoCuotas
        {
            get{return strTipoCuotas;}
            set{strTipoCuotas = Strings.Trim(value);}
        }
    }

    public class RptaPagoVisa
    {
        public int IdPedido { get; set; }
        private string strHostRespuesta;
        public string CodTienda { get; set; }
        public int HostNroOrden { get; set; }
        private string strHostCodAccion;
        private string strHostNroTarjeta;
        private string strHostECI;
        private string strHostCodAutoriza;
        private string strHostOrigenTarjeta;
        private string strHostNomEmisor;
        private string strHostDescECI;
        private string strHostCodResCVV2;
        public Nullable<double> HostImporteAutorizado { get; set; }
        private string strHostMensajeError;
        private string strHostTarjetaHabiente;

        public string HostRespuesta
        {
            get{return strHostRespuesta;}
            set{strHostRespuesta = Strings.Trim(value);}
        }
               
        public string HostCodAccion
        {
            get{return strHostCodAccion;}
            set{strHostCodAccion = Strings.Trim(value);}
        }

        public string HostNroTarjeta
        {
            get{return strHostNroTarjeta;}
            set{strHostNroTarjeta = Strings.Trim(value);}
        }

        public string HostECI
        {
            get{return strHostECI;}
            set{strHostECI = Strings.Trim(value);}
        }

        public string HostCodAutoriza
        {
            get{return strHostCodAutoriza;}
            set{strHostCodAutoriza = Strings.Trim(value);}
        }

        public string HostNomEmisor
        {
            get{return strHostNomEmisor;}
            set{strHostNomEmisor = Strings.Trim(value);}
        }

        public string HostDescECI
        {
            get{return strHostDescECI;}
            set{strHostDescECI = Strings.Trim(value);}
        }

        public string HostCodResCVV2
        {
            get{return strHostCodResCVV2;}
            set{strHostCodResCVV2 = Strings.Trim(value);}
        }

        public string HostOrigenTarjeta
        {
            get{return strHostOrigenTarjeta;}
            set{strHostOrigenTarjeta = Strings.Trim(value);}
        }

        public string HostMensajeError
        {
            get{return strHostMensajeError;}
            set{strHostMensajeError = Strings.Trim(value);}
        }

        public string HostTarjetaHabiente
        {
            get{return strHostTarjetaHabiente;}
            set{strHostTarjetaHabiente = Strings.Trim(value);}
        }
    }

    public class RptaPagoMastercard
    {
        public int IdPedido { get; set; }
        public string strResultTx;
        public string strCodAuth;
        public string strNroCuotas;
        public string strFecPrimeraCuota;
        public string strMonedaCuota;
        public string strMontoCuota;
        public string strMontoTotal;
        public string strMonedaMontoTotal;
        public string strFechaTx;
        public string strHoraTx;
        public string strCodRpta;
        public string strCodPais;
        public string strNroTarjeta;
        public string strSecCode;
        public string strMensajeRpta;
        public string strCodCli;
        public string strCodPaisTx;
        public string strFirmaHMACSHA1;
        public string strEmailUsuario;
        public string strNroPedidoMC;
        public string strNroRefGenWeb;
        public string strMsgCodRpta;
        public string strNomBancoEmisor;

        public string ResultTx
        {
            get{return strResultTx;}
            set{strResultTx = Strings.Trim(value);}
        }

        public string CodAuth
        {
            get{return strCodAuth;}
            set{strCodAuth = Strings.Trim(value);}
        }

        public string NroCuotas
        {
            get{return strNroCuotas;}
            set{strNroCuotas = Strings.Trim(value);}
        }

        public string FecPrimeraCuota
        {
            get{return strFecPrimeraCuota;}
            set{strFecPrimeraCuota = Strings.Trim(value);}
        }

        public string MonedaCuota
        {
            get{return strMonedaCuota;}
            set{strMonedaCuota = Strings.Trim(value);}
        }

        public string MontoCuota
        {
            get{return strMontoCuota;}
            set{strMontoCuota = Strings.Trim(value);}
        }

        public string MontoTotal
        {
            get{return strMontoTotal;}
            set{strMontoTotal = Strings.Trim(value);}
        }

        public string MonedaMontoTotal
        {
            get{return strMonedaMontoTotal;}
            set{strMonedaMontoTotal = Strings.Trim(value);}
        }

        public string FechaTx
        {
            get{return strFechaTx;}
            set{strFechaTx = Strings.Trim(value);}
        }

        public string HoraTx
        {
            get{return strHoraTx;}
            set{strHoraTx = Strings.Trim(value);}
        }

        public string CodRpta
        {
            get{return strCodRpta;}
            set{strCodRpta = Strings.Trim(value);}
        }

        public string CodPais
        {
            get{return strCodPais;}
            set{strCodPais = Strings.Trim(value);}
        }

        public string NroTarjeta
        {
            get{return strNroTarjeta;}
            set{strNroTarjeta = Strings.Trim(value);}
        }

        public string SecCode
        {
            get{return strSecCode;}
            set{strSecCode = Strings.Trim(value);}
        }

        public string MensajeRpta
        {
            get{return strMensajeRpta;}
            set{strMensajeRpta = Strings.Trim(value);}
        }

        public string CodCli
        {
            get{return strCodCli;}
            set{strCodCli = Strings.Trim(value);}
        }

        public string CodPaisTx
        {
            get{return strCodPaisTx;}
            set{strCodPaisTx = Strings.Trim(value);}
        }

        public string FirmaHMACSHA1
        {
            get{return strFirmaHMACSHA1;}
            set{strFirmaHMACSHA1 = Strings.Trim(value);}
        }

        public string EmailUsuario
        {
            get{return strEmailUsuario;}
            set{strEmailUsuario = Strings.Trim(value);}
        }

        public string NroPedidoMC
        {
            get{return strNroPedidoMC;}
            set{strNroPedidoMC = Strings.Trim(value);}
        }

        public string NroRefGenWeb
        {
            get{return strNroRefGenWeb;}
            set{strNroRefGenWeb = Strings.Trim(value);}
        }

        public string MsgCodRpta
        {
            get{return strMsgCodRpta;}
            set{strMsgCodRpta = Strings.Trim(value);}
        }

        public string NomBancoEmisor
        {
            get{return strNomBancoEmisor;}
            set{strNomBancoEmisor = Strings.Trim(value);}
        }
    }

    public class RptaPagoUATP {
        public int IdPedido { get; set; }
        public DateTime FechaRpta { get; set; }
        public string strIdTipoTarjeta;
        public string strNroTarjeta;
        public string strAnioExpiraTarjeta;
        public string strMesExpiraTarjeta;
        public string strTitularTarjeta;
        public string strCodSeguridadTarjeta;
        public string strBancoEmisor;
        public string strMensajeRpta;
        public string strIdTipoDocTitularTarjeta;
        public string strNumDocTitularTarjeta;
        public bool ResultadoOK { get; set; } = false;
        public string strTelfCliente;
        public string strFechacobropayu;
          
        public string IdTipoTarjeta
        {
            get{return strIdTipoTarjeta;}
            set{strIdTipoTarjeta = Strings.Trim(value);}
        }

        public string NroTarjeta
        {
            get{return strNroTarjeta;}
            set{strNroTarjeta = Strings.Trim(value);}
        }

        public string AnioExpiraTarjeta
        {
            get{return strAnioExpiraTarjeta;}
            set{strAnioExpiraTarjeta = Strings.Trim(value);}
        }

        public string MesExpiraTarjeta
        {
            get{return strMesExpiraTarjeta;}
            set{strMesExpiraTarjeta = Strings.Trim(value);}
        }

        public string TitularTarjeta
        {
            get{return strTitularTarjeta;}
            set{strTitularTarjeta = Strings.Trim(value);}
        }

        public string CodSeguridadTarjeta
        {
            get{return strCodSeguridadTarjeta;}
            set{strCodSeguridadTarjeta = Strings.Trim(value);}
        }

        public string BancoEmisor
        {
            get{return strBancoEmisor;}
            set{strBancoEmisor = Strings.Trim(value);}
        }

        public string MensajeRpta
        {
            get{return strMensajeRpta;}
            set{strMensajeRpta = Strings.Trim(value);}
        }

        public string Fechacobropayu
        {
            get{return strFechacobropayu;}
            set{strFechacobropayu = Strings.Trim(value);}
        }
        public string IdTipoDocTitularTarjeta
        {
            get{return strIdTipoDocTitularTarjeta;}
            set{strIdTipoDocTitularTarjeta = Strings.Trim(value);}
        }

        public string NumDocTitularTarjeta
        {
            get{return strNumDocTitularTarjeta;}
            set{strNumDocTitularTarjeta = Strings.Trim(value);}
        }

        public string NroTarjetaBaneado
        {
            get
            {
                if (string.IsNullOrEmpty(this.NroTarjeta) || this.NroTarjeta.Length < 13)
                    return "";
                else
                    return this.NroTarjeta.Substring(0, 6) + "*****" + this.NroTarjeta.Substring(12, this.NroTarjeta.Length - 12);
            }
        }

        public string TelfCliente
        {
            get{return strTelfCliente;}
            set{strTelfCliente = Strings.Trim(value);}
        }              
    }

    public class RptaPagoEfectivoEC {
        public Nullable<int> IdPedido { get; set; } = null;
        private string strCIP;
        private string strEstado;
        private string strXMLInformacionCIP;
        private string strMensajeRpta;
        public double MontoTotalPagar { get; set; }
        public string CodigoBarras { get; set; }
        public Nullable<DateTime> FechaExpiraPago { get; set; }
        private string strEstadoCIP;
        private string strEstadoConciliado;
        public Nullable<DateTime> FechaCancelado { get; set; } = null;
        public Nullable<DateTime> FechaConciliado { get; set; } = null;
        public Nullable<DateTime> FechaExtorno { get; set; } = null;
        public Nullable<bool> EnvioRpta { get; set; } = null;
             
        public string CIP
        {
            get{return strCIP;}
            set{strCIP = Strings.Trim(value);}
        }

        public string Estado
        {
            get{return strEstado;}
            set{strEstado = Strings.Trim(value);}
        }

        public string XMLInformacionCIP
        {
            get{return strXMLInformacionCIP;}
            set{strXMLInformacionCIP = Strings.Trim(value);}
        }

        public string MensajeRpta
        {
            get{return strMensajeRpta;}
            set{strMensajeRpta = Strings.Trim(value);}
        }

        public string EstadoCIP
        {
            get{return strEstadoCIP;}
            set{strEstadoCIP = Strings.Trim(value);}
        }

        public string EstadoConciliado
        {
            get{return strEstadoConciliado;}
            set{strEstadoConciliado = Strings.Trim(value);}
        }
    }

    public class PasarelaPago_Pedido
    {
        public int IdPedido { get; set; }
        public DateTime FechaPedido { get; set; }
        private string strTipoMotor;
        private string strIdSesion;
        public Nullable<int> IdReservaVuelo { get; set; }
        public Nullable<int> IdReservaPaquete { get; set; }
        private string strTipoPaquete;
        public int IdWeb { get; set; }
        public Int16 IdLang { get; set; }
        private string strIPUsuario;
        private string strBrowserUsuario;
        public bool OKPagoTarjeta { get; set; } = false;
        public bool OKPagoPuntos { get; set; } = false;
        public Nullable<double> MontoTarjeta { get; set; }
        public Int16 IdEstadoPedido { get; set; }
        private string strNomEstadoPedido;
        public Nullable<int> IdCotSRV { get; set; }
        private string strDetalleServicio;
        public string IdPaisUsuario { get; set; }
        private string strNomPaisUsuario;
        public bool EsUATP { get; set; } = false;
        public string IdLAValidadora { get; set; }
        public Nullable<double> FEE_ResVue { get; set; }
        public bool EsFonoPago { get; set; } = false;
        public string IdPedidoEncriptado { get; set; }
        public Nullable<int> IdRecibo { get; set; } = null;
        public Nullable<DateTime> FechaGeneraRecibo { get; set; } = null;
        public Nullable<int> IdSucursalRC { get; set; } = null;
        public Nullable<double> MontoDscto { get; set; }
        public FormaPagoPedido FormaPagoPedido { get; set; }
        public RptaPagoVisa RptaPagoVisa { get; set; }
        public RptaPagoMastercard RptaPagoMastercard { get; set; }
        public RptaPagoUATP RptaPagoUATP { get; set; }
        public RptaPagoEfectivoEC RptaPagoEfectivoEC { get; set; }
        public RptaPagoSafetyPay RptaPagoSafetyPay { get; set; }
        public Nullable<DateTime> datFechaexpira { get; set; }
        public Nullable<DateTime> datFechaexpiracion { get; set; }
        public bool EsPayu { get; set; }
        private string strPNR_MT;

        public string PNR_MT
        {
            get{return strPNR_MT;}
            set{strPNR_MT = Strings.Trim(value);}
        }
        
        public string TipoMotor
        {
            get{return strTipoMotor;}
            set{strTipoMotor = Strings.Trim(value);}
        }

        public string IdSesion
        {
            get{return strIdSesion;}
            set{strIdSesion = Strings.Trim(value);}
        }
              
        public string TipoPaquete
        {
            get{return strTipoPaquete;}
            set{strTipoPaquete = Strings.Trim(value);}
        }
                
        public string IPUsuario
        {
            get{return strIPUsuario;}
            set{strIPUsuario = Strings.Trim(value);}
        }

        public string BrowserUsuario
        {
            get{return strBrowserUsuario;}
            set{strBrowserUsuario = Strings.Trim(value);}
        }

        public string NomEstadoPedido
        {
            get{return strNomEstadoPedido;}
            set{strNomEstadoPedido = Strings.Trim(value);}
        }
               
        public string DetalleServicio
        {
            get{return strDetalleServicio;}
            set{strDetalleServicio = Strings.Trim(value);}
        }
        
        public string NomPaisUsuario
        {
            get{return strNomPaisUsuario;}
            set{strNomPaisUsuario = Strings.Trim(value);}
        }        
    }

}