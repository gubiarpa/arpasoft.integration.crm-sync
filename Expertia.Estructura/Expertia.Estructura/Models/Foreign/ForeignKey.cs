using Expertia.Estructura.Models.Behavior;
using Expertia.Estructura.Utils;
using System.Linq;

namespace Expertia.Estructura.Models.Foreign
{
    #region Auditoria
    public class SystemUser : SimpleDesc { }
    #endregion

    #region Cuenta
    public class CanalInformacion : MultipleDesc { }
    public class Branch : MultipleDesc { }
    public class IdiomaComunicCliente : MultipleDesc { }
    public class InteresProdActiv : MultipleDesc { }
    public class PuntoContacto : SimpleDesc { }
    public class NivelImportancia : SimpleDesc { }
    public class Estado : SimpleDesc { }
    // Participantes
    public class Empleado : SimpleDesc { }
    public class GrupoColaborador : SimpleDesc { }
    public class FlagPrincipal : SimpleDesc { }
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
    public class EmpresaCondicionPago : SimpleDesc
    {
        public UnidadNegocioKeys? GetUnidadNegocioKey()
        {
            string unidadNegocioName = Descripcion; // Ejm. "CONDOR TRAVEL"

            if (ConfigAccess.GetValueInAppSettings(UnidadNegocioKeys.CondorTravel.GetKeyValues()).ToUpper().Split(Utils.Auxiliar.ListSeparator).ToList().Contains(unidadNegocioName.ToUpper()))
            {
                return UnidadNegocioKeys.CondorTravel;
            }
            else if (ConfigAccess.GetValueInAppSettings(UnidadNegocioKeys.DestinosMundiales.GetKeyValues()).ToUpper().Split(Utils.Auxiliar.ListSeparator).ToList().Contains(unidadNegocioName.ToUpper()))
            {
                return UnidadNegocioKeys.DestinosMundiales;
            }
            else if (ConfigAccess.GetValueInAppSettings(UnidadNegocioKeys.Interagencias.GetKeyValues()).ToUpper().Split(Utils.Auxiliar.ListSeparator).ToList().Contains(unidadNegocioName.ToUpper()))
            {
                return UnidadNegocioKeys.Interagencias;
            }

            else if (ConfigAccess.GetValueInAppSettings(UnidadNegocioKeys.CondorTravelCL.GetKeyValues()).ToUpper().Split(Utils.Auxiliar.ListSeparator).ToList().Contains(unidadNegocioName.ToUpper()))
            {
                return UnidadNegocioKeys.CondorTravelCL;
            }

            return null;
        }
    }
    public class TipoCondicionPago : SimpleDesc { }
    public class TipoMoneda : SimpleDesc { }
    public class CategoriaValor : AlterDesc { }
    public class CategoriaPerfilActitudTecnologica : AlterDesc { }
    public class CategoriaPerfilFidelidad : AlterDesc { }
    public class Incentivo : AlterDesc { }
    public class MotivoEstado : AlterDesc { }
    public class Herramientas : MultipleDesc { }
    public class GDS : MultipleDesc { }
    public class GrupoComunicacion : MultipleDesc { }
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