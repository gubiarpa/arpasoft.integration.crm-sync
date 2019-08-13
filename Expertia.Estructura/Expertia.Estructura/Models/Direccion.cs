using Expertia.Estructura.Models.Behavior;

namespace Expertia.Estructura.Models
{
    public class Direccion : UniqueBase
    {
        public TipoDireccion Tipo { get; set; }
        public string Descripcion { get; set; }
        public Pais Pais { get; set; }
        public Departamento Departamento { get; set; }
        public Ciudad Ciudad { get; set; }
        public Distrito Distrito { get; set; }
    }
}