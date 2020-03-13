using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class CuentaNM : ICrmApiResponse, IActualizado
    {
        public string nombreCli { get; set; }
        public string apePatCli { get; set; }
        public string apeMatCli { get; set; }
        public string idCuenta_Sf { get; set; }
        public string eMailCli { get; set; }
        public bool enviarPromociones { get; set; }
        public string tipoTelefono1 { get; set; }
        public float codPais1 { get; set; }
        public float numero1 { get; set; }
        public string tipoTelefono2 { get; set; }
        public float codPais2 { get; set; }
        public float numero2 { get; set; }
        public string tipoTelefono3 { get; set; }
        public float codPais3 { get; set; }
        public float numero3 { get; set; }
        public string direccion { get; set; }
        public string razonSocial { get; set; }
        public bool aceptarPoliticas { get; set; }
        public float ruc { get; set; }
        public float idUsuarioSrv_Sf { get; set; }
        public string accion_Sf { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public int Actualizados { get; set; } = -1;
        public string idCuentaCrm { get; set; }
    }
}