using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class AgenciaPnr : IUnidadNegocio
    {
        public UnidadNegocio UnidadNegocio { get; set; }
        public int DkAgencia { get; set; }
        public string PNR { get; set; }
        public int IdFile { get; set; }
        public SimpleDesc Sucursal { get; set; }
        public string IdOportunidadCrm { get; set; }
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
}