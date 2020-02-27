using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using System.Linq;
using System.Web;

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
}