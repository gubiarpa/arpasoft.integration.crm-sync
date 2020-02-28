using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Models.Retail
{
    public class UsuarioWeb
    {
        public enum TIPO_USUARIO
        {
            CLIENTE = 0,
            PERSONAL = 1
        }

        public int IdUsuWeb { get; set; }
        public string LoginUsuWeb { get; set; }
        public TIPO_USUARIO TipoUsuario { get; set; }
        public int IdUsuWebSybase { get; set; }
    }

}