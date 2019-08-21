using Expertia.Estructura.Models.Auxiliar;

namespace Expertia.Estructura.Models.Behavior
{
    public interface IAuditable
    {
        Auditoria Auditoria { get; set; }
    }
}
