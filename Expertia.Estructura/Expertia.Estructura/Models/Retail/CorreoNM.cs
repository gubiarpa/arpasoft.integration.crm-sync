using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.VisualBasic;

namespace Expertia.Estructura.Models
{
    public class CorreoNM
    {
        private int intIdCorreo;
        private string strNomCorreo = "";
        private string strFromCorreo = "";
        private string strDisplayFromCorreo = "";
        private string strToCorreo = "";
        private string strCCCorreo = "";
        private string strBCCCorreo = "";
        private string strStyleAddressCorreo = "";
        private string strSubjectCorreo = "";
        private string strFormatoCorreo = "";
        private string strHeaderCorreo = "";
        private string strFooterCorreo = "";
        private string strHostCorreo = "";
        private string strStyleFileCorreo = "";
        private string strLogoCorreo = "";
        private string strBodyCorreo = "";
        private int intIdWeb;
        private int intIdLang;

        private string strBCCCorreoAgy = "";
        private string strBCCCorreoSist = "";

        private string strUsuarioCredentials = "centroonline@agcorp.pe";
        private string strPasswordCredentials = "C3ntr02017";

        public int IdCorreo
        {
            get
            {
                return intIdCorreo;
            }
            set
            {
                intIdCorreo = value;
            }
        }

        public string NombreCorreo
        {
            get
            {
                return strNomCorreo;
            }
            set
            {
                strNomCorreo = Strings.Trim(value);
            }
        }

        public string FromCorreo
        {
            get
            {
                return strFromCorreo;
            }
            set
            {
                strFromCorreo = Strings.Trim(value);
            }
        }

        public string DisplayFromCorreo
        {
            get
            {
                return strDisplayFromCorreo;
            }
            set
            {
                strDisplayFromCorreo = Strings.Trim(value);
            }
        }

        public string ToCorreo
        {
            get
            {
                return strToCorreo;
            }
            set
            {
                strToCorreo = Strings.Trim(value);
            }
        }

        public string CCCorreo
        {
            get
            {
                return strCCCorreo;
            }
            set
            {
                strCCCorreo = Strings.Trim(value);
            }
        }

        public string BCCCorreo
        {
            get
            {
                return strBCCCorreo;
            }
            set
            {
                strBCCCorreo = Strings.Trim(value);
            }
        }

        public string StyleAddressCorreo
        {
            get
            {
                return strStyleAddressCorreo;
            }
            set
            {
                strStyleAddressCorreo = Strings.Trim(value);
            }
        }

        public string SubjectCorreo
        {
            get
            {
                return strSubjectCorreo;
            }
            set
            {
                strSubjectCorreo = Strings.Trim(value);
            }
        }

        public string FormatoCorreo
        {
            get
            {
                return strFormatoCorreo;
            }
            set
            {
                strFormatoCorreo = Strings.Trim(value);
            }
        }

        public string HeaderCorreo
        {
            get
            {
                return strHeaderCorreo;
            }
            set
            {
                strHeaderCorreo = Strings.Trim(value);
            }
        }

        public string FooterCorreo
        {
            get
            {
                return strFooterCorreo;
            }
            set
            {
                strFooterCorreo = Strings.Trim(value);
            }
        }

        public string HostCorreo
        {
            get
            {
                return strHostCorreo;
            }
            set
            {
                strHostCorreo = Strings.Trim(value);
            }
        }

        public string StyleFileCorreo
        {
            get
            {
                return strStyleFileCorreo;
            }
            set
            {
                strStyleFileCorreo = Strings.Trim(value);
            }
        }

        public string LogoCorreo
        {
            get
            {
                return strLogoCorreo;
            }
            set
            {
                strLogoCorreo = Strings.Trim(value);
            }
        }

        public string BodyCorreo
        {
            get
            {
                return strBodyCorreo;
            }
            set
            {
                strBodyCorreo = Strings.Trim(value);
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

        public int IdLang
        {
            get
            {
                return intIdLang;
            }
            set
            {
                intIdLang = value;
            }
        }

        public string BCCCorreoAgy
        {
            get
            {
                return strBCCCorreoAgy;
            }
            set
            {
                strBCCCorreoAgy = Strings.Trim(value);
            }
        }

        public string BCCCorreoSist
        {
            get
            {
                return strBCCCorreoSist;
            }
            set
            {
                strBCCCorreoSist = Strings.Trim(value);
            }
        }

        public string UsuarioCredentials
        {
            get
            {
                return strUsuarioCredentials;
            }
            set
            {
                strUsuarioCredentials = Strings.Trim(value);
            }
        }

        public string PasswordCredentials
        {
            get
            {
                return strPasswordCredentials;
            }
            set
            {
                strPasswordCredentials = Strings.Trim(value);
            }
        }
    }
}