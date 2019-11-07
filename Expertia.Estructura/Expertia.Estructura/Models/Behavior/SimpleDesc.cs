namespace Expertia.Estructura.Models.Behavior
{
    public class SimpleDesc //: IUnique
    {
        public SimpleDesc(string descripcion = null)
        {
            Descripcion = descripcion;
        }

        public string Descripcion { get; set; }
    }

    public class AlterDesc
    {
        public string Description { get; set; }
    }
}