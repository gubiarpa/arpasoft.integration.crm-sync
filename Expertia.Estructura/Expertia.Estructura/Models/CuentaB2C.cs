namespace Expertia.Estructura.Models
{
    public class CuentaB2C : Cuenta
    {
        #region Properties
        public string Nombre { get; set; }
        public string ApePaterno { get; set; }
        public string ApeMaterno { get; set; }
        public string PreferenciasGenerales { get; set; }
        public string ConsideracionesSalud { get; set; }
        #endregion

        #region ForeignKey
        public EstadoCivil EstadoCivil { get; set; }
        public Genero Genero { get; set; }
        public Nacionalidad Nacionalidad { get; set; }
        public GradoEstudios GradoEstudios { get; set; }
        public Profesion Profesion { get; set; }
        public TipoViaje TipoViaje { get; set; }
        public CategoriaViaje CategoriaViaje { get; set; }
        public TipoAcompanante TipoAcompanante { get; set; }
        #endregion
    }
}