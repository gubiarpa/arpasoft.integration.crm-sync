using Expertia.Estructura.Models.Behavior;
using System;

namespace Expertia.Estructura.Models
{
    public class RptaCuentaSF : ICrmApiResponse
    {
        public string idCuenta_SF { get; set; }
        public string Identificador_NM { get; set; }
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
    }

    public class CuentaNM 
    {
        public string idCuenta_SF { get; set; }
        public string idCuenta_NM { get; set; }
        public string nombreCliente { get; set; }
        public string apellidoCliente { get; set; }
        public string apeMatCli { get; set; }
        public string emailCliente { get; set; }
        public bool enviarPromociones { get; set; }
        public string tipoTelefono1 { get; set; }
        public string pais1 { get; set; }
        public string numero1 { get; set; }
        public string codArea1 { get; set; }
        public string Anexo1 { get; set; }
        public string tipoTelefono2 { get; set; }
        public string pais2 { get; set; }
        public string numero2 { get; set; }
        public string codArea2 { get; set; }
        public string Anexo2 { get; set; }
        public string tipoTelefono3 { get; set; }
        public string pais3 { get; set; }
        public string numero3 { get; set; }
        public string codArea3 { get; set; }
        public string Anexo3 { get; set; }
        public string direccion { get; set; }   
        public int idUsuarioSrv_SF { get; set; }
        public string accion_SF { get; set; }        
    }
}