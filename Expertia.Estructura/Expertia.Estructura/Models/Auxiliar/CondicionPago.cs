using Expertia.Estructura.Models.Foreign;

namespace Expertia.Estructura.Models.Auxiliar
{
    public class CondicionPago
    {
        public EmpresaCondicionPago Empresa { get; set; }
        public TipoCondicionPago TipoCondicionPago { get; set; }
    }
}