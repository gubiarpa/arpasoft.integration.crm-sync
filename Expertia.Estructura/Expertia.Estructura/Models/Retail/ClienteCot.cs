using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models.Retail
{
    public class ClienteCot
    {
        private int intIdCliCot;
        private string strNomCliCot = "";
        private string strApeCliCot = "";
        private string strApeMatCliCot = "";
        private string strEmailCliCot = "";
        private string strEmailAlterCliCot = "";
        private bool bolRecibePromo = false;
        private string strNumTelfPrincipal = "";
        private List<TelefonoCliCot> lstTelfsClitCot;
        private string strDireccion = "";
        private string strIdTipDoc = "";
        private string strNumDoc = "";
        private int intIdWeb;
        private List<ArchivoCliCot> lstArchivosClitCot;
        private DateTime? datFechaNac;
        private int IntIdUsuWeb;
        private bool BolEsAdicional;


        public int IdUsuWeb
        {
            get
            {
                return IntIdUsuWeb;
            }
            set
            {
                IntIdUsuWeb = value;
            }
        }

        public bool EsAdicional
        {
            get
            {
                return BolEsAdicional;
            }
            set
            {
                BolEsAdicional = value;
            }
        }

        public int IdCliCot
        {
            get
            {
                return intIdCliCot;
            }
            set
            {
                intIdCliCot = value;
            }
        }

        public string NomCliCot
        {
            get
            {
                return strNomCliCot;
            }
            set
            {
                strNomCliCot = value.Trim();
            }
        }

        public string ApeCliCot
        {
            get
            {
                return strApeCliCot;
            }
            set
            {
                strApeCliCot = value.Trim();
            }
        }

        public string ApeMatCliCot
        {
            get
            {
                return strApeMatCliCot;
            }
            set
            {
                strApeMatCliCot = value.Trim();
            }
        }

        public string EmailCliCot
        {
            get
            {
                return strEmailCliCot;
            }
            set
            {
                strEmailCliCot = value.Trim();
            }
        }

        public string EmailAlterCliCot
        {
            get
            {
                return strEmailAlterCliCot;
            }
            set
            {
                strEmailAlterCliCot = value.Trim();
            }
        }

        public bool RecibePromo
        {
            get
            {
                return bolRecibePromo;
            }
            set
            {
                bolRecibePromo = value;
            }
        }

        public int IdWeb
        {
            get
            {
                return intIdWeb;
            }
            set
            {
                intIdWeb = value;
            }
        }

        public List<TelefonoCliCot> ListaTelfCli
        {
            get
            {
                return lstTelfsClitCot;
            }
            set
            {
                lstTelfsClitCot = value;
            }
        }

        public string Direccion
        {
            get
            {
                return strDireccion;
            }
            set
            {
                strDireccion = value.Trim();
            }
        }

        public string IdTipDoc
        {
            get
            {
                return strIdTipDoc;
            }
            set
            {
                strIdTipDoc = value.Trim();
            }
        }

        public string NumDoc
        {
            get
            {
                return strNumDoc;
            }
            set
            {
                strNumDoc = value.Trim();
            }
        }

        public List<ArchivoCliCot> ListaArchivosCli
        {
            get
            {
                return lstArchivosClitCot;
            }
            set
            {
                lstArchivosClitCot = value;
            }
        }

        public string NomApeCompletoCliCot
        {
            get
            {
                return strNomCliCot + " " + strApeCliCot;
            }
        }

        public string ApeNomCompletoCliCot
        {
            get
            {
                return strApeCliCot + " " + strNomCliCot;
            }
        }

        public string ApeNomCompletoCliCotOrden
        {
            get
            {
                return (strApeCliCot + " " + strNomCliCot).ToUpper();
            }
        }

        public string NumTelfPrincipalCliCot
        {
            get
            {
                if (lstTelfsClitCot != null)
                {
                    if (lstTelfsClitCot.Count > 0)
                        return lstTelfsClitCot[0].NumTelf;
                }
                return "";
            }
        }

        public string NumTelfsCliCot
        {
            get
            {
                string strNumTelfs = "";
                if (lstTelfsClitCot != null)
                {
                    for (Int16 intX = 0; intX <= lstTelfsClitCot.Count - 1; intX++)
                    {
                        if (lstTelfsClitCot[intX].IdTipoTelf.Equals(1))
                            // Particular
                            strNumTelfs += lstTelfsClitCot[intX].NumTelf + " (P)" + "\n";
                        else if (lstTelfsClitCot[intX].IdTipoTelf.Equals(2))
                            // Celular
                            strNumTelfs += lstTelfsClitCot[intX].NumTelf + " (C)" + "\n";
                        else if (lstTelfsClitCot[intX].IdTipoTelf.Equals(3))
                            // Trabajo
                            // strNumTelfs &= lstTelfsClitCot.Item(intX).NumTelf & " / " & _
                            // lstTelfsClitCot.Item(intX).AnexoTelf & " (T)" & vbCrLf
                            strNumTelfs += lstTelfsClitCot[intX].NumTelf + " (T)" + "\n";
                        else
                            strNumTelfs += lstTelfsClitCot[intX].NumTelf + "\n";
                    }
                }
                return strNumTelfs;
            }
        }

        public string EmailsCliCot
        {
            get
            {
                return strEmailCliCot + "\n" + strEmailAlterCliCot;
            }
        }

        public Nullable<DateTime> FechaNac
        {
            get
            {
                return datFechaNac;
            }
            set
            {
                datFechaNac = value;
            }
        }
    }

}