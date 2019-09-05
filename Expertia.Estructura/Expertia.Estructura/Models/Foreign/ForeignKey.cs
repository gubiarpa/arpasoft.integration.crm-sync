using Expertia.Estructura.Models.Behavior;

namespace Expertia.Estructura.Models.Foreign
{
    #region Auditoria
    public class SystemUser : SimpleDesc { }
    #endregion

    #region Cuenta
    public class UnidadNegocio : SimpleDesc { }
    public class CanalInformacion : SimpleDesc { }
    public class Branch : SimpleDesc { }
    public class IdiomaComunicCliente : SimpleDesc { }
    public class InteresProdActiv : SimpleDesc { }
    public class PuntoContacto : SimpleDesc { }
    public class NivelImportancia : SimpleDesc { }
    public class Estado : SimpleDesc { }
    // Tipos
    public class TipoCorreo : SimpleDesc { }
    public class TipoCuenta : SimpleDesc { }
    public class TipoDocumento : SimpleDesc { }
    public class TipoDireccion : SimpleDesc { }
    public class TipoPersona : SimpleDesc { }
    public class TipoSitio : SimpleDesc { }
    public class TipoTelefono : SimpleDesc { }
    // Ubicación
    public class Pais : SimpleDesc { }
    public class Departamento : SimpleDesc { }
    public class Ciudad : SimpleDesc { }
    public class Distrito : SimpleDesc { }
    #endregion

    #region CuentaB2B
    public class CondicionPago : SimpleDesc { }
    public class TipoMoneda : SimpleDesc { }
    public class CategoriaValor : SimpleDesc { }
    public class CategoriaPerfilActitudTecnologica : SimpleDesc { }
    public class CategoriaPerfilFidelidad : SimpleDesc { }
    public class Incentivo : SimpleDesc { }
    public class MotivoEstado : SimpleDesc { }
    public class Herramientas : SimpleDesc { }
    public class GDS : SimpleDesc { }
    public class GrupoComunicacion : SimpleDesc { }
    #endregion

    #region CuentaB2C
    public class EstadoCivil : SimpleDesc { }
    public class Genero : SimpleDesc { }
    public class Nacionalidad : SimpleDesc { }
    public class GradoEstudios : SimpleDesc { }
    public class Profesion : SimpleDesc { }
    public class CargoEmpresa : SimpleDesc { }
    public class NivelRiesgo : SimpleDesc { }
    public class RegionMercadoBranch : SimpleDesc { }
    public class TipoViaje : SimpleDesc { }
    public class CategoriaViaje : SimpleDesc { }
    public class TipoAcompanante : SimpleDesc { }
    #endregion

    #region Pago
    public class FormaPago : SimpleDesc { }
    #endregion
}