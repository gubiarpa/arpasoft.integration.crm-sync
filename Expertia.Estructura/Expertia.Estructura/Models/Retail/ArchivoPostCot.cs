using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models.Retail
{
    public class ArchivoPostCot
    {
        private int intIdCot;
        private short intIdPost;
        private short intIdArchivo;
        private string strRutaArchivo = "";
        private string strNomArchivo = "";
        private string strExtensionArchivo = "";
        private byte[] bytArchivo;

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

        public short IdPost
        {
            get
            {
                return intIdPost;
            }
            set
            {
                intIdPost = value;
            }
        }

        public short IdArchivo
        {
            get
            {
                return intIdArchivo;
            }
            set
            {
                intIdArchivo = value;
            }
        }

        public string RutaArchivo
        {
            get
            {
                return strRutaArchivo;
            }
            set
            {
                strRutaArchivo = value.Trim();
            }
        }

        public string NombreArchivo
        {
            get
            {
                return strNomArchivo;
            }
            set
            {
                strNomArchivo = value.Trim();
            }
        }

        public string ExtensionArchivo
        {
            get
            {
                return strExtensionArchivo;
            }
            set
            {
                strExtensionArchivo = value.Trim().ToLower();
            }
        }

        public byte[] Archivo
        {
            get
            {
                return bytArchivo;
            }
            set
            {
                bytArchivo = value;
            }
        }
    }

}