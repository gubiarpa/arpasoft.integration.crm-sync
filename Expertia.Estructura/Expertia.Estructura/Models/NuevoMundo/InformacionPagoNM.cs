using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class InformacionPagoNM : ICrmApiResponse, IActualizado
    {
        public string idOportunidad_SF { get; set; }
        public string tipoServicio { get; set; }
        public string tipoPasajero { get; set; }
        public float totalBoleto { get; set; }
        public float tarifaNeto { get; set; }
        public float impuestos { get; set; }
        public float cargos { get; set; }
        public string nombreHotel { get; set; }
        public float totalPagar { get; set; }
        public float numHabitacionPaquete { get; set; }
        public float cantidadPasajeroPaq { get; set; }
        public float precioUnitarioPaq { get; set; }
        public float totalUnitarioPaq { get; set; }
        public float precioTotalPorHabitacionPaq { get; set; }
        public float precioTotalHabitacionesPaq { get; set; }
        public float gastosAdministrativosPaq { get; set; }
        public float precioTotalPagarPaq { get; set; }
        public string accion_SF { get; set; }
        public string IdInformacionPago_SF { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public int Actualizados { get; set; } = -1;
    }
}