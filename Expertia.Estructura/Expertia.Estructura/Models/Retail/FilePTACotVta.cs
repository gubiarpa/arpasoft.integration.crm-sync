using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models.Retail
{
    public class FilePTACotVta
    {
        private int intIdCot;
        private short intIdSuc;
        private int intIdFilePTA;
        private DateTime datFecha;
        private string strMoneda = "";
        private double dblImporteFact;
        private double dblTípoCambio;

        private string strNomSuc = "";
        private DateTime datFechaCierreVta;
        private short intAuditoria;
        private string strNomCliente = "";
        private string strNomVendedor = "";

        private int intDK;
        private string strDescDK = "";
        private short? intIdSubCodigo;
        private string strDescSubCodigo = "";

        public int IdCot
        {
            get
            {
                return intIdCot;
            }
            set
            {
                intIdCot = value;
            }
        }

        public Int16 IdSucursal
        {
            get
            {
                return intIdSuc;
            }
            set
            {
                intIdSuc = value;
            }
        }

        public int IdFilePTA
        {
            get
            {
                return intIdFilePTA;
            }
            set
            {
                intIdFilePTA = value;
            }
        }

        public DateTime Fecha
        {
            get
            {
                return datFecha;
            }
            set
            {
                datFecha = value;
            }
        }

        public string Moneda
        {
            get
            {
                return strMoneda;
            }
            set
            {
                strMoneda = value;
            }
        }

        public double ImporteFacturado
        {
            get
            {
                return dblImporteFact;
            }
            set
            {
                dblImporteFact = value;
            }
        }

        public double TipoCambio
        {
            get
            {
                return dblTípoCambio;
            }
            set
            {
                dblTípoCambio = value;
            }
        }

        public string NombreSucursal
        {
            get
            {
                return strNomSuc;
            }
            set
            {
                strNomSuc = value;
            }
        }

        public DateTime FechaCierreVta
        {
            get
            {
                return datFechaCierreVta;
            }
            set
            {
                datFechaCierreVta = value;
            }
        }

        public string FormatFechaAlta
        {
            get
            {
                return this.Fecha.ToString("dd/MM/yyyy HH:mm");
            }
        }

        public string FormatFechaCierraVta
        {
            get
            {
                return this.FechaCierreVta.ToString("dd/MM/yyyy HH:mm");
            }
        }

        public Int16 Auditoria
        {
            get
            {
                return intAuditoria;
            }
            set
            {
                intAuditoria = value;
            }
        }

        public string NomAuditoria
        {
            get
            {
                switch (intAuditoria)
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
            get
            {
                return strNomCliente;
            }
            set
            {
                strNomCliente = value.Trim();
            }
        }

        public string NomVendedor
        {
            get
            {
                return strNomVendedor;
            }
            set
            {
                strNomVendedor = value.Trim();
            }
        }

        public int DK
        {
            get
            {
                return intDK;
            }
            set
            {
                intDK = value;
            }
        }

        public string DescDK
        {
            get
            {
                return strDescDK;
            }
            set
            {
                strDescDK = value.Trim();
            }
        }

        public Nullable<Int16> IdSubCodigo
        {
            get
            {
                return intIdSubCodigo;
            }
            set
            {
                intIdSubCodigo = value;
            }
        }

        public string DescSubCodigo
        {
            get
            {
                return strDescSubCodigo;
            }
            set
            {
                strDescSubCodigo = value.Trim();
            }
        }

        public string DK_Desc
        {
            get
            {
                return this.DK + " - " + this.DescDK;
            }
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