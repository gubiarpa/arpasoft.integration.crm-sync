using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class SolicitudPagoNM : ICrmApiResponse, IActualizado
    {
        public string idOportunidad_SF { get; set; }
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
        public string ipCliente { get; set; }
        public string docTitular { get; set; }
        public string mensaje { get; set; }
        public string mensajeError { get; set; }
        public string accion_SF { get; set; }
        public string IdRegSolicitudPago_SF { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public int Actualizados { get; set; } = -1;
    }
}