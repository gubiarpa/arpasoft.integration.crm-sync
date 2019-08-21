using Expertia.Estructura.Models.Behavior;
using Expertia.Estructura.Models.Foreign;

namespace Expertia.Estructura.Models.Auxiliar
{
    public class Direccion : IUnique
    {
        public string ID { get; set; }
        public TipoDireccion Tipo { get; set; }
        public string Descripcion { get; set; }
        public Pais Pais { get; set; }
        public Departamento Departamento { get; set; }
        public Ciudad Ciudad { get; set; }
        public Distrito Distrito { get; set; }
    }
}