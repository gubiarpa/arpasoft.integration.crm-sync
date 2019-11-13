using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    public class AgenciaPnr : IUnidadNegocio
    {
        public UnidadNegocio UnidadNegocio { get; set; }
        public int DkAgencia { get; set; }
        public string PNR { get; set; }
        public int IdFile { get; set; }
        public int IdSucursal { get; set; }
        public string IdOportunidadCrm { get; set; }
    }

    public class FileBoleto
    {
        public string IdOportunidad { get; set; }
        public string Objeto { get; set; }
        public string Accion { get; set; }
        public int IdFile { get; set; }
        public string EstadoFile { get; set; }
        public string UnidadNegocio { get; set; }
        public string Sucursal { get; set; }
        public string EjecutivaCuenta { get; set; }
        public string NombreGrupo { get; set; }
        public string Counter { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Cliente { get; set; }
        public string Subcodigo { get; set; }
        public string Contacto { get; set; }
        public string TipoFile { get; set; }
        public string ClienteCliente { get; set; }
        public string PaisProcedencia { get; set; }
        public string CondicionPago { get; set; }
        public string Idioma { get; set; }
        public int NumPasajeros { get; set; }
        public int Costo { get; set; }
        public int Venta { get; set; }
        public int Comision { get; set; }
        public int Boleto { get; set; }
        public string EstadoBoleto { get; set; }
        public string Pnr { get; set; }
        public string TipoBoleto { get; set; }
        public string LineaAerea { get; set; }
        public string Ruta { get; set; }
        public string TipoRuta { get; set; }
        public string CiudadDestino { get; set; }
        public string PuntoEmision { get; set; }
        public string NombrePasajero { get; set; }
        public string InfanteConAdulto { get; set; }
        public DateTime FechaEmision { get; set; }
        public string EmitidoCanje { get; set; }
        public string AgenteQuienEmite { get; set; }
        public int MontoTarifa { get; set; }
        public int MontoComision { get; set; }
        public int MontoTotal { get; set; }
        public string FormaPago { get; set; }
        public string Reembolsado { get; set; }
        public string PagoConTarjeta { get; set; }
        public string TieneWaiver { get; set; }
        public string TipoWaiver { get; set; }
        public int MontoWaiver { get; set; }
        public string Pagado { get; set; }
        public string Comprobante { get; set; }
    }
}