using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models.Retail
{
    public class TelefonoCliCot
    {
        private int intIdCliCot;
        private Int16 intIdTelfCli;
        private Nullable<Int16> intCodPaisTelf;
        private Nullable<Int16> intCodAreaTelf;
        private string strNumTelf = "";
        private string strAnexoTelf = "";
        private bool bolEsTelfPrincipal = false;
        private Int16 intIdTipoTelf;
        private string strNomTipoTelf = "";

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

        public Int16 IdTelfCli
        {
            get
            {
                return intIdTelfCli;
            }
            set
            {
                intIdTelfCli = value;
            }
        }

        public Nullable<Int16> CodPaisTelf
        {
            get
            {
                return intCodPaisTelf;
            }
            set
            {
                intCodPaisTelf = value;
            }
        }

        public Nullable<Int16> CodAreaTelf
        {
            get
            {
                return intCodAreaTelf;
            }
            set
            {
                intCodAreaTelf = value;
            }
        }

        public string NumTelf
        {
            get
            {
                return strNumTelf;
            }
            set
            {
                strNumTelf = value.Trim();
            }
        }

        public string AnexoTelf
        {
            get
            {
                return strAnexoTelf;
            }
            set
            {
                strAnexoTelf = value.Trim();
            }
        }

        public bool EsTelfPrincipal
        {
            get
            {
                return bolEsTelfPrincipal;
            }
            set
            {
                bolEsTelfPrincipal = value;
            }
        }

        public Int16 IdTipoTelf
        {
            get
            {
                return intIdTipoTelf;
            }
            set
            {
                intIdTipoTelf = value;
            }
        }

        public string NomTipoTelf
        {
            get
            {
                return strNomTipoTelf;
            }
            set
            {
                strNomTipoTelf = value.Trim();
            }
        }
    }

}