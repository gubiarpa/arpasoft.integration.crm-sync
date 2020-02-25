using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Utils
{
    public class UsuarioLogin
    {
        public int IdUsuario { get; set; }
        public string NomUsuario { get; set; }
        public string ApePatUsuario { get; set; }
        public string ApeMatUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string EstadoUsuario { get; set; }
        public string LoginUsuario { get; set; }
        public string NomCompletoUsuario { get; set; }
        public TIPO_USUARIO TipoUsuario { get; set; }
        public string RazSocCliEmp { get; set; }
        public int IdCliEmp { get; set; }
        public int IdDep { get; set; }
        public int IdOfi { get; set; }
        public int IdEmp { get; set; }
        public int IdUsuWebSybase { get; set; }
        public string IdVendedorPTA { get; set; }
        public bool EsCounterAdminSRV { get; set; } = false;
        public int IdUsuETravel { get; set; }
        public Nullable<int> IdPlanilla { get; set; }
        public Nullable<int> IdEmpresaPlanilla { get; set; }
        public string AutoLoginTrp { get; set; }
        public Nullable<DateTime> FecUpdate { get; set; }
        public bool EsSupervisorSRV { get; set; } = false;
        public Nullable<DateTime> FecExpiraPass { get; set; }
        public bool CambiarClave { get; set; }
        public string MensajeCambiarClave { get; set; }
        public string Documento { get; set; }
        public string ClienteDk { get; set; }
        public string TelfCasaUsuario { get; set; }
        public string TelfMovilUsuario { get; set; }
        public string TelfOficinaUsuario { get; set; }
    }
    public enum TIPO_USUARIO : short
    {
        CLIENTE = 0,
        PERSONAL = 1
    }
}