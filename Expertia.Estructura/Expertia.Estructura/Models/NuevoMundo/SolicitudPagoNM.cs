using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class SolicitudPagoNM : ICrmApiResponse
    {
        public string idOportunidad_SF { get; set; }
        public string Identificador_NM { get; set; }
        public int IdPedido { get; set; }
        public string pasarela { get; set; }
        public string fechaPedido { get; set; }
        public string estado1 { get; set; }
        public string estado2 { get; set; }
        public string resultado { get; set; }
        public string montoPagar { get; set; }
        public string rcGenerado { get; set; }
        public string lineaAereaValidadora { get; set; }
        public string formaPago { get; set; }
        public string entidadBancaria { get; set; }
        public string nroTarjeta { get; set; }
        public string titularTarjeta { get; set; }
        public string expiracion { get; set; }
        public string thReniec { get; set; }
        public string marcaTC { get; set; }
        public string tipoTC { get; set; }
        public string nivelTC { get; set; }
        public string paisTC { get; set; }
        public string EsAutenticada { get; set; }
        public string Detalle { get; set; }
        public string LinkPago { get; set; }
        public string CodAutorTarj { get; set; }
        public string TipoImporte { get; set; }
        public string MontoImporte { get; set; }
        public string PlazoDePago { get; set; }
        public string Error { get; set; }
        public string CodCanje { get; set; }
        public string Puntos { get; set; }
        public string ipCliente { get; set; }
        public string docTitular { get; set; }
        public float? FEE { get; set; }
        public float? GEM { get; set; }
        public float? PEF { get; set; }        
        public string accion_SF { get; set; }
        public int idFormpaPago { get; set; }
        public float igv { get; set; }
        public float montoPagarDbl { get; set; }
        public int WebCid { get; set; }
        public int IdCotizacion { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }        
    }

    public class RptaSolicitudPagoSF : ICrmApiResponse
    {
        public string idOportunidad_SF { get; set; }
        public string IdRegSolicitudPago_SF { get; set; }
        public string Identificador_NM { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
    }
}