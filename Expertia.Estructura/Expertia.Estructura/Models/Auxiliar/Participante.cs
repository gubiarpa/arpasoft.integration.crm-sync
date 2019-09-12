using Expertia.Estructura.Models.Foreign;

namespace Expertia.Estructura.Models.Auxiliar
{
    public class Participante
    {
        public Empleado EmpleadoOrEjecutivoResponsable { get; set; }
        public Empleado SupervisorKam { get; set; }
        public Empleado Gerente { get; set; }
        public UnidadNegocio UnidadNegocio { get; set; }
        public GrupoColaborador GrupoColabEjecRegionBranch { get; set; }
        public bool FlagPrincipal { get; set; }
    }
}