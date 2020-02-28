using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Linq;
using System.Web;
using Expertia.Estructura.Models.Behavior;
using Expertia.Estructura.Models.Auxiliar;

namespace Expertia.Estructura.Models
{
    public class SRV
    {
    }

    public class Post_SRV
    {
        public int IdCot { get; set; }
        public short TipoPost { get; set; }
        public string TextoPost { get; set; }
        public string IPUsuCrea { get; set; }
        public string LoginUsuCrea { get; set; }
        public int IdUsuWeb { get; set; }
        public int IdDep { get; set; }
        public int IdOfi { get; set; }
        public List<ArchivoPostCot> Archivos { get; set; }
        public List<FilePTACotVta> LstFilesPTA { get; set; }
        public Int16 IdEstado { get; set; }
        public bool CambioEstado { get; set; }
        public ArrayList LstFechasCotVta { get; set; }
        public bool EsAutomatico { get; set; }
        public byte[] ArchivoMail { get; set; }
        public bool EsCounterAdmin { get; set; }
        public Nullable<int> IdUsuWebCounterCrea { get; set; }
        public Nullable<int> IdOfiCounterCrea { get; set; }
        public Nullable<int> IdDepCounterCrea { get; set; }
        public Nullable<bool> EsUrgenteEmision { get; set; }
        public Nullable<DateTime> FecPlazoEmision { get; set; }
        public Nullable<Int16> IdMotivoNoCompro { get; set; }
        public string OtroMotivoNoCompro { get; set; }
        public Nullable<double> MontoEstimadoFile { get; set; }
    }

    public class ArchivoPostCot
    {
        private int IdCot { get; set; }
        private Int16 IdPost { get; set; }
        private Int16 IdArchivo { get; set; }
        private string RutaArchivo { get; set; }
        private string NomArchivo { get; set; }
        private string ExtensionArchivo { get; set; }
        private byte[] Archivo { get; set; }
    }

    public class AssociateFile
    {        
        public int idUsuario { get; set; }
        public bool ModalidadCompra { get; set; }
        public int idoportunidad_SF { get; set; }
        public int idCotSRV_SF { get; set; }
        public List<FileSRV> LstFiles { get; set; }
        /*Duda si agregar comentario y nota de seguimiento (aplica al cambiar de estado a facturado - Informativo)*/
    }

    public class AssociateFileRS :ICrmApiResponse
    {
        public string CodigoError { get; set; }
        public string MensajeError { get; set; }
    }

    public class FileSRV
    {
        public int IdFilePTA { get; set; }
        public DateTime Fecha { get; set; }
        public double Sucursal { get; set; }
        public string Cliente { get; set; }
        public double ImporteFact { get; set; }
        public string Moneda { get; set; }
    }

    public class FilePTACotVta
    {
        public int IdCot { get; set; }
        public Int16 IdSuc { get; set; }
        public int IdFilePTA { get; set; }
        public DateTime Fecha { get; set; }
        public string Moneda { get; set; }
        public double ImporteFact { get; set; }
        public double TípoCambio { get; set; }

        public string NomSuc { get; set; }
        private DateTime FechaCierreVta { get; set; }
        private Int16 Auditoria { get; set; }
        private string strNomCliente = "";
        private string strNomVendedor = "";

        private int DK { get; set; }
        private string strDescDK = "";
        private Nullable<Int16> IdSubCodigo { get; set; }
        private string strDescSubCodigo = "";
        public Double ImporteFacturado { get; set; }
        public Double TipoCambio { get; set; }
        public string NombreSucursal { get; set; }


        public string FormatFechaAlta
        {
            get {return this.Fecha.ToString("dd/MM/yyyy HH:mm");}
        }

        public string FormatFechaCierraVta
        {
            get {return this.FechaCierreVta.ToString("dd/MM/yyyy HH:mm");}
        }
              
        public string NomAuditoria
        {
            get
            {
                switch (Auditoria)
                {
                    case 1:
                        {
                            return "Aprobado";
                        }

                    case 2:
                        {
                            return "No Aprobado";
                        }

                    default:
                        {
                            return "Pendiente";
                        }
                }
            }
        }

        public string NomCliente
        {
            get{return strNomCliente;}
            set{strNomCliente = Strings.Trim(value);}
        }

        public string NomVendedor
        {
            get{return strNomVendedor;}
            set{strNomVendedor = Strings.Trim(value);}
        }
     
        public string DescDK
        {
            get{return strDescDK;}
            set{strDescDK = Strings.Trim(value);}
        }           

        public string DescSubCodigo
        {
            get{return strDescSubCodigo;}
            set{strDescSubCodigo = Strings.Trim(value);}
        }

        public string DK_Desc
        {
            get{return this.DK + " - " + this.DescDK;}
        }

        public string IdSubCodigo_Desc
        {
            get
            {
                if (this.IdSubCodigo.HasValue)
                    return this.IdSubCodigo.Value + " - " + this.DescSubCodigo;
                else
                    return "";
            }
        }
    }

    public class CotizacionVta
    {
        public int IdCot { get; set; }
        public Nullable<DateTime> FechaCot { get; set; } = null;
        public Int16 IdModoIng { get; set; }
        public Int16 IdCanalVta { get; set; }
        private string strTextoSol = string.Empty;
        private string strNomUsuCrea = string.Empty;
        private string strLoginUsuCrea = string.Empty;
        private string strCodIATAPrincipal = string.Empty;
        public Int16 IdEstado { get; set; }
        public int IdCliCot { get; set; }
        public int IdUsuWeb { get; set; }
        public int IdDep { get; set; }
        public int IdOfi { get; set; }
        public int IdWeb { get; set; }
        public int IdLang { get; set; }
        public Nullable<int> IdEmpCot { get; set; }
        private string strRazSocEmpCot = string.Empty;
        private string strNomCanalVta = string.Empty;
        private string strNomEstadoCot = string.Empty;
        private string strNomDep = string.Empty;
        private string strNomOfi = string.Empty;
        private string strNomModoIng = string.Empty;
        public Int16 CantVecesFact { get; set; }
        private string strDestinosPref = string.Empty;
        public Nullable<DateTime> FecSalida { get; set; } = null;
        public Nullable<DateTime> FecRegreso { get; set; } = null;
        public Nullable<Int16> CantPaxAdulto { get; set; } = null;
        public Nullable<Int16> CantPaxNiños { get; set; } = null;
        private string strEmailUsuWebCrea = string.Empty;
        public Nullable<int> IdReservaVuelos { get; set; } = null;
        public Nullable<int> IdReservaPaquete { get; set; } = null;
        public Nullable<Int16> IdSucursalReservaPaquete { get; set; } = null;
        public string TipoPaquete { get; set; } = null;
        public Nullable<int> IdReservaAuto { get; set; } = null;
        public Nullable<int> IdReservaHotel { get; set; } = null;
        public Nullable<int> IdReservaSeguro { get; set; } = null;
        public Nullable<bool> RequiereFirmaCliente { get; set; }
        private string strCodReservaVueManual = string.Empty;
        public Nullable<double> MontoReservaVueManual { get; set; } = null;
        private string strIdVendedorPTACrea = string.Empty;
        public Nullable<Int16> IdModalidadCompra { get; set; } = null;

        public Nullable<bool> EsUrgenteEmision { get; set; }
        public Nullable<DateTime> FechaPlazoEmision { get; set; }
        public Nullable<int> IdUsuWebCA { get; set; }
        private string strLoginUsuWebCA;
        public bool EsEmitido { get; set; } = false;
        public Nullable<int> IdCompra { get; set; }
        private string strNomGrupo = string.Empty;
        public Nullable<double> MontoEstimadoFile { get; set; }
        public int EsAereo { get; set; }
        public string CliCod_Mail { get; set; } = string.Empty;
        public string EsPaqDinamico { get; set; } = string.Empty;
        public Nullable<int> HotelResId { get; set; }
        public string IPUsuCrea { get; set; }
        public string[] ArrayServicios { get; set; }
        public Int16 IdEstOtro { get; set; }
        public string PaisResidencia { get; set; }
        public Nullable<int> IdWebPaq { get; set; }
        public Nullable<decimal> MontoDscto { get; set; }
        public int IdOAtencion { get; set; }
        public int IdEvento { get; set; }
        public string sufijoMT { get; set; }
        public int IdReserva2MT { get; set; }
        public string Metabuscador { get; set; }
        public bool EsComisionable { get; set; }
        public Nullable<double> PorcIncentivoVentaNoComisionable { get; set; }
        public Nullable<double> TotlIncentivoVentaNoComisionable { get; set; }
        public Nullable<double> PorcComisionVentaNoComisionable { get; set; }
        public Nullable<double> TotlComisionVentaNoComisionable { get; set; }
               
        public enum MODO_INGRESO_COT_VTA : short
        {
            Teléfono = 1,
            Email = 2,
            Presencial = 3,
            Web = 4
        }
               
        public string NomModoIngreso
        {
            get{return strNomModoIng;}
            set{strNomModoIng = Strings.Trim(value);}
        }

        public string TextoSolicitud
        {
            get{return strTextoSol;}
            set{strTextoSol = Strings.Trim(value);}
        }

        public string NomCompletoUsuCrea
        {
            get{return strNomUsuCrea;}
            set{strNomUsuCrea = Strings.Trim(value);}
        }

        public string LoginUsuWeb
        {
            get{return strLoginUsuCrea;}
            set{strLoginUsuCrea = Strings.Trim(value);}
        }
       
        public string NomEstadoCot
        {
            get{return strNomEstadoCot;}
            set{strNomEstadoCot = Strings.Trim(value);}
        }

        public string NomDep
        {
            get{return strNomDep;}
            set{strNomDep = Strings.Trim(value);}
        }

        public string NomOfi
        {
            get{return strNomOfi;}
            set{strNomOfi = Strings.Trim(value);}
        }

        public string NomPtoVta
        {
            get{return strNomOfi + " - " + strNomDep;}
        }
                
        public string NomCanalVta
        {
            get{return strNomCanalVta;}
            set{strNomCanalVta = Strings.Trim(value);}
        }
        
        public string CodigoIATAPrincipal
        {
            get{return strCodIATAPrincipal;}
            set{strCodIATAPrincipal = Strings.Trim(value);}
        }
              
        public string RazSocEmpCot
        {
            get{return strRazSocEmpCot;}
            set{strRazSocEmpCot = Strings.Trim(value);}
        }

        public string DestinosPref
        {
            get{return strDestinosPref;}
            set{strDestinosPref = Strings.Trim(value);}
        }
        
        public string EmailUsuWebCrea
        {
            get{return strEmailUsuWebCrea;}
            set{strEmailUsuWebCrea = Strings.Trim(value);}
        }
        
        public string CodReservaVueManual
        {
            get{return strCodReservaVueManual;}
            set{strCodReservaVueManual = Strings.Trim(value);}
        }
               
        public string IdVendedorPTACrea
        {
            get{return strIdVendedorPTACrea;}
            set{strIdVendedorPTACrea = Strings.Trim(value);}
        }
                
        public string LoginUsuWebCA
        {
            get{return strLoginUsuWebCA;}
            set{strLoginUsuWebCA = Strings.Trim(value);}
        }
               
        public string NomGrupo
        {
            get{return strNomGrupo;}
            set{strNomGrupo = Strings.Trim(value);}
        }
    }

}