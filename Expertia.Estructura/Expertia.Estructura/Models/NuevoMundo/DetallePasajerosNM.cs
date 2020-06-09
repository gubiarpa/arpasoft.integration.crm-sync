using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class DetallePasajerosNM : ICrmApiResponse
    {
        public string idOportunidad_SF { get; set; }
        public string idPasajero_SF { get; set; }
        public string Identificador_NM { get; set; }
        public int id_reserva { get; set; }
        public string IdPasajero { get; set; }
        public string tipo { get; set; }
        public string pais { get; set; }
        public string apellidos { get; set; }
        public string nombre { get; set; }
        public string tipoDocumento { get; set; }
        public string nroDocumento { get; set; }
        public string fechaNacimiento { get; set; }
        public string Genero { get; set; }
        public string FOID { get; set; }
        public float FEE { get; set; }
        public string NombreReniec { get; set; }
        public string numHabitacionPaquete { get; set; }
        public string accion_SF { get; set; }        
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }        
    }

    public class RptaPasajeroSF : ICrmApiResponse
    {
        public string idOportunidad_SF { get; set; }
        public string idPasajero_SF { get; set; }
        public string Identificador_NM { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
    }
}