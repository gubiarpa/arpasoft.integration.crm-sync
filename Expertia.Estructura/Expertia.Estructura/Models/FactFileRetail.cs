using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    public class FactFileRetailReq
    {
        public int IdDatosFacturacion { get; set; }
        public int Estado { get; set; }
        public string DK { get; set; }
        public string SubCodigo { get; set; }
        public string Ejecutiva { get; set; }
        public string NumFile_NM { get; set; }
        public string NUmFile_DM { get; set; }
        public string CCB { get; set; }
        public string RUC { get; set; }
        public string RAZON { get; set; }
        public string TipoDocumento { get; set; }
        public string Documento { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMateno { get; set; }
        public string OARipley { get; set; }
        public double? MontoOA { get; set; }
        private int NumId_out { get; set; }
        public int IdUsuario { get; set; }
        public int Cot_Id { get; set; }
        public string Campania { get; set; }
        public string Correo { get; set; }
        public string Banco { get; set; }
        public string CantidadMillas { get; set; }
        public double MontoMillas { get; set; }
        public string Observacion { get; set; }
        public string Doc_cid { get; set; }
        private string Descripcion_Doc_Cid { get; set; }
        public List<ReciboDetalle> ReciboDetalle { get; set; }
        public List<TarifaDetalle> TarifaDetalle { get; set; }
        public List<FileRetail> Archivos { get; set; }
        public string MontoTotalCobrar { get; set; }
        public string Observaciones { get; set; }
        public string IdOportunidad_SF { get; set; }
        public string IdCotSrv_SF { get; set; }
        public string accion_SF { get; set; }
        public int IdUsuarioSrv_SF { get; set; }

    }
    public class TarifaDetalle
    {
        private int IdDetalleTarifa { get; set; }
        public int CantidadADT { get; set; }
        public int CantidadCHD { get; set; }
        public int CantidadINF { get; set; }
        public int IdGrupoServicio { get; set; }
        public double TarifaPorADT { get; set; }
        public double TarifaPorCHD { get; set; }
        public double TarifaINF { get; set; }
        public double MontoPorADT { get; set; }
        public double MontoPorCHD { get; set; }
        public double MontoPorINF { get; set; }
        public int IdDatosFacturacion { get; set; }
        public string GrupoServicio { get; set; }

    }
    public class ReciboDetalle
    {
        public int IdDetalleNoRecibo{get;set;}
        public string Sucursal { get; set; }
        public string NoRecibo { get; set; }
        public double MontoRecibo { get; set; }
        public int IdSucursal { get; set; }
        private int Estado { get; set; }
        public int IdDatosFacturacion { get; set; }

    }
    public class FileRetail
    {
        private int IdArchivo { get; set; }
        private int IdDatosFacturacion { get; set; }
        public string RutaArchivo { get; set; }
        public string NomArchivo{get;set;}
        private string ExtensionArchivo { get; set; }
        private int IdUsuWebCrea { get; set; }
        public string URLArchivo { get; set; }
        public int NumeroFiles { get; set; }
        public string Sucursal { get; set; }
        public string FechaAsociacion { get; set; }
        public string Cliente { get; set; }
        public string Importe { get; set; }
        public int IdOportunidad_SF { get; set; }
        public int IdCotSrv_SF { get; set; }
    }
    public class FactFileRetailRes
    {
    }
}