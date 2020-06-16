using Expertia.Estructura.Models.Behavior;
using System;
using System.Collections.Generic;

namespace Expertia.Estructura.Models
{
    public class SolicitarFactFileNM
    {
        public string iddatosfacturacion { get; set; }
        public string estado { get; set; }
        public string dk { get; set; }
        public string subcodigo { get; set; }
        public string comisionista { get; set; }
        public string campania { get; set; }
        public string numfilenm { get; set; }
        public string numfiledm { get; set; }
        public string ccb { get; set; }
        public string facturaruc { get; set; }
        public string razonsocial { get; set; }
        public string correo { get; set; }
        public string tipodocventa { get; set; }
        public string tipodocidentidad { get; set; }
        public string numdocidentidad { get; set; }
        public string nombre { get; set; }
        public string apepaterno { get; set; }
        public string apemateno { get; set; }
        public string oaripley { get; set; }
        public string oamonto { get; set; }
        public string idusuario { get; set; }
        public float cotid { get; set; }
        public string banco { get; set; }
        public string cantidadmillas { get; set; }
        public string montomillas { get; set; }
        public List<ReciboDetalle> ReciboDetalleList { get; set; }
        public List<TarifaDetalle> TarifaDetalleList { get; set; }
        public List<Archivo> ArchivoList { get; set; }
        public string montocobrar { get; set; }
        public string observaciones { get; set; }
        public bool enviarCA { get; set; }
        public string accionsf { get; set; }
        public string idusuariosrv { get; set; }
        public string codigo { get; set; }
        public string mensaje { get; set; }
        #region Computadas
        public bool existeIdDatosFacturacion { get { return iddatosfacturacion != null; } }
        public bool existeArchivoList { get { return (ArchivoList != null && ArchivoList.Count > 0); } }
        public int idusuariosrv_SF { get { if (!int.TryParse(idusuariosrv, out int intIdUsuarioSrv)) intIdUsuarioSrv = 0; return intIdUsuarioSrv; } }
        #endregion
    }

    public class Archivo
    {
        public int IdArchivo { get; set; }
        public string RutaArchivo { get; set; }
        public string NomArchivo { get; set; }
        public string UrlArchivo { get; set; }
        public string ExtArchivo { get; set; }
        public int IdDatosFacturacion { get; set; }
        public int IdUsuWeb { get; set; }
        public int NumeroFiles { get; set; }
        public string Sucursal { get; set; }
        public DateTime FechaAsociacion { get; set; }
        public string Cliente { get; set; }
        public float Importe { get; set; }
        public int IdOportunidad_SF { get; set; }
        public int IdCotSrv_SF { get; set; }
    }
}