using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models
{
    public class Credentials
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class Token
    {
        public Guid Key { get; set; }
        public DateTime Expiry { get; set; }
    }
}