using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Utils
{
    public static class RoutePrefix
    {
        public const string Contacto  = "api/contacto";
        public const string CuentaB2B = "api/cuentab2b";
        public const string CuentaB2C = "api/cuentab2c";        
    }

    public static class HttpAction
    {
        public const string Create = "create";
        public const string Read   = "read";
        public const string Update = "update";
        public const string Delete = "delete";
    }
}