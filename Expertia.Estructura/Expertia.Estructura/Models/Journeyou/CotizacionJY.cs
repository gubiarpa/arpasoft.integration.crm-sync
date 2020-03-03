using Expertia.Estructura.Models.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models.Journeyou
{
    public class Cotizacion_JY
    {
        public string Id_Oportunidad_Sf { get; set; }
        public string Id_Cotizacion_Sf { get; set; }
        public string Id_Cuenta_Sf { get; set; }
        public string Usuario { get; set; }
        public string Accion { get; set; }
        public string Tipo_Documento { get; set; }
        public string Documento { get; set; }
        public string Correo { get; set; }
        public string Region { get; set; }
        public string Cotizacion { get; set; }
        public string File { get; set; }
        public int Numero_Paxs { get; set; }
    }

    public class CotizacionJYResponse : ICrmApiResponse
    {
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public string Grupo { get; set; }
        public string Estado { get; set; }
        public float VentaEstimada { get; set; }
        public string FileSubfile { get; set; }
        public float VentaFile { get; set; }
        public float MargenFile { get; set; }
        public int PaxsFile { get; set; }
        public string EstadoFile { get; set; }
        public string FechaInicioViaje { get; set; }
    }

    public class CotizacionJYUpdResponse
    {
        public string ID_OPORTUNIDAD_SF { get; set; }
        public string ID_COTIZACION_SF { get; set; }
        public string ID_CUENTA_SF { get; set; }
        public string GRUPO { get; set; }
        public string ESTADO { get; set; }
        public float VENTA_ESTIMADA { get; set; }
        public string FILE_SUBFILE { get; set; }
        public float VENTA_FILE { get; set; }
        public float MARGEN_FILE { get; set; }
        public int PAXS_FILE { get; set; }
        public string ESTADO_FILE { get; set; }
        public DateTime FECHA_INICIO_VIAJE { get; set; }
        public string Pais { get; set; }
        public string Cotizacion { get; set; }
        public string CodigoRetorno { get; set; }
        public string MensajeRetorno { get; set; }
    }

    public class CotizacionJYUpd
    {
        public string Cotizacion { get; set; }
        public string File { get; set; }
        public string Es_Atencion { get; set; }
        public string Descripcion { get; set; }
    }

}