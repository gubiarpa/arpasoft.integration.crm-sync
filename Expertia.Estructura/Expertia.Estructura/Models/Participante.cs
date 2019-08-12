namespace Expertia.Estructura.Models
{
    public class Participante
    {
        public string EmpleadoOrEjecutivoResponsable { get; set; }
        public string SupervisorKam { get; set; }
        public string Gerente { get; set; }
        public string UnidadNegocio { get; set; }
        public string GrupoColabEjecRegionBranch { get; set; }
        public bool FlagPrincipal { get; set; }
    }
}