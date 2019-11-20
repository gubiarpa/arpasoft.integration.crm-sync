using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class AgenciaPnr : ICrmApiResponse
    {
        public string UnidadNegocio { get; set; }
        public int DkAgencia { get; set; }
        public string PNR { get; set; }
        public int IdFile { get; set; }
        public SimpleDesc Sucursal { get; set; }
        public string IdOportunidad { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
    }

    public class File
    {
        public string IdOportunidad { get; set; }
        public string Accion { get; set; }
        public int IdFile { get; set; }
        public string EstadoFile { get; set; }
        public string UnidadNegocio { get; set; }
        public string Sucursal { get; set; }
        public string NombreGrupo { get; set; }
        public string Counter { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Cliente { get; set; }
        public string Subcodigo { get; set; }
        public string Contacto { get; set; }
        public string CondicionPago { get; set; }
        public int NumPasajeros { get; set; }
        public float Costo { get; set; }
        public float Venta { get; set; }
        public float ComisionAgencia { get; set; }
    }

    public class FileSalesforce
    {
        public string Objeto { get; set; }
        public string Estado_File { get; set; }
        public string Unidad_Negocio { get; set; }
        public string Sucursal { get; set; }
        public string Nombre_Grupo { get; set; }
        public string Counter { get; set; }
        public DateTime Fecha_Apertura { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }
        public string Cliente { get; set; }
        public string Subcodigo { get; set; }
        public string Condicion_Pago { get; set; }
        public string Num_Pasajeros { get; set; }
        public string Costo { get; set; }
        public string Venta { get; set; }
        public string Comision_Agencia { get; set; }
    }

    public class Boleto
    {
        public string IdOportunidad { get; set; }
        public string Accion { get; set; }
        public int IdFile { get; set; }
        public string Sucursal { get; set; }
        public string NumeroBoleto { get; set; }
        public string EstadoBoleto { get; set; }
        public string Pnr { get; set; }
        public string TipoBoleto { get; set; }
        public string LineaAerea { get; set; }
        public string Ruta { get; set; }
        public string TipoRuta { get; set; }
        public string CiudadDestino { get; set; }
        public string PuntoEmision { get; set; }
        public string NombrePasajero { get; set; }
        public string InfanteAdulto { get; set; }
        public string FechaEmision { get; set; }
        public string EmitidoCanje { get; set; }
        public string AgenteQuienEmite { get; set; }
        public float MontoTarifa { get; set; }
        public float MontoComision { get; set; }
        public float MontoTotal { get; set; }
        public string FormaPago { get; set; }
        public string Reembolsado { get; set; }
        public string PagoConTarjeta { get; set; }
        public string TieneWaiver { get; set; }
        public string TipoWaiver { get; set; }
        public float MontoWaiver { get; set; }
        public string Pagado { get; set; }
        public string Comprobante { get; set; }
    }
}