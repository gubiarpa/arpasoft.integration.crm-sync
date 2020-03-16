using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class DetallePasajerosNM : ICrmApiResponse, IActualizado
    {
        public string idOportunidad_SF { get; set; }
        public string tipo { get; set; }
        public string pais { get; set; }
        public string apellidos { get; set; }
        public string nombre { get; set; }
        public string tipoDocumento { get; set; }
        public float nroDocumento { get; set; }
        public string fechaNacimiento { get; set; }
        public string foid { get; set; }
        public float fee { get; set; }
        public string NombreReniec { get; set; }
        public string numHabitacionPaquete { get; set; }
        public string accion_SF { get; set; }
        public string idPasajero_SF { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public int Actualizados { get; set; } = -1;
    }
}