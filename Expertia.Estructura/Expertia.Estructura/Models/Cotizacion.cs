using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class Cotizacion : IUnidadNegocio
    {
        #region Properties
        public string IdCuenta { get; set; }
        public int Subcodigo { get; set; }
        public string IdCotizacion { get; set; }
        public string IdCuentaSalesforce { get; set; }
        public string IdOportunidadSalesforce { get; set; }
        public string IDCotizacionSalesforce { get; set; }
        public int IdCotizacionPta { get; set; }
        public DateTime FechaSalida { get; set; }
        public DateTime FechaRetorno { get; set; }
        #endregion

        #region ForeignKey
        public UnidadNegocio UnidadNegocio { get; set; }
        public SimpleDesc Surcursal { get; set; }
        public SimpleDesc PuntoVenta { get; set; }
        public SimpleDesc Grupo { get; set; }
        public SimpleDesc Origen { get; set; }
        public SimpleDesc Pais { get; set; }
        public SimpleDesc Ciudad { get; set; }
        public SimpleDesc VendedorCounter { get; set; }
        public SimpleDesc VendedorCotizador { get; set; }
        public SimpleDesc VendedorReserva { get; set; }
        public SimpleDesc Usuario { get; set; }
        #endregion
    }
}