using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models.Retail
{
    [Serializable()]
    public class ArchivoCliCot
    {
        private int intIdCliCot;
        private Int16 intIdArchivo;
        private string strRutaArchivo = "";
        private string strNomArchivo = "";
        private string strExtensionArchivo = "";
        private byte[] bytArchivo;
        private int intIdUsuWebCrea;
        private string strURLArchivo = "";

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

        public Int16 IdArchivo
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

        public int IdUsuWeb
        {
            get
            {
                return intIdUsuWebCrea;
            }
            set
            {
                intIdUsuWebCrea = value;
            }
        }

        public string URLArchivo
        {
            get
            {
                return strURLArchivo;
            }
            set
            {
                strURLArchivo = value.Trim();
            }
        }
    }
}