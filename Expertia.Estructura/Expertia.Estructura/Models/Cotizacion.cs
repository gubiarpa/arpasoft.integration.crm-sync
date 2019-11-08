using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Behavior;
using Expertia.Estructura.Models.Foreign;
using System;

namespace Expertia.Estructura.Models
{
    public class Cotizacion : IUnidadNegocio
    {
        #region Properties
        public string IdCuenta { get; set; }
        public int Subcodigo { get; set; }
        public int IdCotizacion { get; set; }
        public string IdCuentaSalesforce { get; set; }
        public string IdOportunidadSalesforce { get; set; }
        public string IDCotizacionSalesforce { get; set; }
        public int IdCotizacionPta { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaRetorno { get; set; }
        public DateTime FechaCotizacion { get; set; }
        public DateTime FechaIniVigencia { get; set; }
        public DateTime FechaFinVigencia { get; set; }
        public DateTime FechaIniServicio { get; set; }
        public DateTime FechaFinServicio { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public float MarkupHotel { get; set; }
        public string Referencia { get; set; }
        public int NumeroAdultos { get; set; }
        public int NumeroNinos { get; set; }
        public string Rango { get; set; }
        public string LiberadosVenta { get; set; }
        public float CostoFinal { get; set; }
        public float VentaFinal { get; set; }
        public string TipoVuelo { get; set; }
        public string Destino { get; set; }
        public bool EsBloqueo { get; set; }
        public bool EsGrupo { get; set; }
        public string MotivoNoVenta { get; set; }
        public string Notas { get; set; }
        #endregion

        #region ForeignKey
        public UnidadNegocio UnidadNegocio { get; set; }
        public SimpleDesc Estado { get; set; }
        public SimpleDesc Branch { get; set; }
        public SimpleDesc Surcursal { get; set; }
        public SimpleDesc PuntoVenta { get; set; }
        public SimpleDesc Propietario { get; set; }
        public SimpleDesc Grupo { get; set; }
        public SimpleDesc Origen { get; set; }
        public SimpleDesc Cliente { get; set; }
        public SimpleDesc Contacto { get; set; }
        public SimpleDesc Pais { get; set; }
        public SimpleDesc Ciudad { get; set; }
        public SimpleDesc VendedorCounter { get; set; }
        public SimpleDesc VendedorCotizador { get; set; }
        public SimpleDesc VendedorReserva { get; set; }
        public SimpleDesc Usuario { get; set; }
        public SimpleDesc Clase { get; set; }
        public SimpleDesc Servicio { get; set; }
        public SimpleDesc CanalVenta { get; set; }
        public SimpleDesc Seguimiento { get; set; }
        public SimpleDesc PuntoContacto { get; set; }
        public SimpleDesc Counter { get; set; }
        public SimpleDesc ReservadoPor { get; set; }
        public SimpleDesc RegistradaPor{ get; set; }
        public SimpleDesc CondicionPago { get; set; }
        #endregion
    }
}