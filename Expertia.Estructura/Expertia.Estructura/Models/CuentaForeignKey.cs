namespace Expertia.Estructura.Models
{
    #region Base
    public class SimpleDesc
    {
        public string ID { get; set; }
        public string Descripcion { get; set; }
    }
    #endregion

    #region Cuenta
    public class TipoPersona : SimpleDesc { }

    public class PuntoContacto : SimpleDesc { }

    public class NivelImportancia : SimpleDesc { }

    public class TipoCuenta : SimpleDesc { }

    public class Estado : SimpleDesc { }

    public class PaisProcedencia : SimpleDesc { }
    #endregion

    #region CuentaB2B
    public class CondicionPago : SimpleDesc { }

    public class TipoMoneda : SimpleDesc { }
    #endregion

    #region CuentaB2C
    public class EstadoCivil : SimpleDesc { }

    public class Genero : SimpleDesc { }

    public class Nacionalidad : SimpleDesc { }

    public class GradoEstudios : SimpleDesc { }

    public class Profesion : SimpleDesc { }

    public class TipoViaje : SimpleDesc { }

    public class CategoriaViaje : SimpleDesc { }

    public class TipoAcompanante : SimpleDesc { }
    #endregion
}