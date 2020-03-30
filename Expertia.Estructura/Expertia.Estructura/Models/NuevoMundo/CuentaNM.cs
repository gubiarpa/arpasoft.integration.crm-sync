using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class CuentaNM : ICrmApiResponse, IActualizado
    {
        public string idCuentaSrv_SF { get; set; }
        public string nombreCliente { get; set; }
        public string apellidoCliente { get; set; }
        //public string apeMatCli { get; set; }
        public string emailCliente { get; set; }
        public bool enviarPromociones { get; set; }
        public string tipoTelefono1 { get; set; }
        public float pais1 { get; set; }
        public float numero1 { get; set; }
        public string tipoTelefono2 { get; set; }
        public float pais2 { get; set; }
        public float numero2 { get; set; }
        public string tipoTelefono3 { get; set; }
        public float pais3 { get; set; }
        public float numero3 { get; set; }
        public string direccion { get; set; }
        public string razonSocial { get; set; }
        public bool aceptarPoliticas { get; set; }
        public float ruc { get; set; }
        public float idUsuarioSrv_Sf { get; set; }
        public string accion_SF { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
        public string idCuenta_Sf { get; set; }
        public int Actualizados { get; set; } = -1;
    }
}