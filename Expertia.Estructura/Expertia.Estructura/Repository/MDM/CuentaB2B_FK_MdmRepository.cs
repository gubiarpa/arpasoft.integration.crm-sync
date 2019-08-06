using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.MDM
{    
    #region ForeignKeys
    public enum CuentaB2B_FK
    {
        // Cuenta
        TipoPersona,
        PuntoContacto,
        NivelImportancia,
        TipoCuenta,
        Estado,
        PaisProcedencia,
        TipoDocumento,
        // Direcciones
        TipoDireccion,
        Distrito,
        Ciudad,
        Departamento,
        Pais,
        // Teléfono
        TipoTelefono,
        TipoSitio,
        TipoCorreo,
        EmpleadoEjecResponsable,
        SupervisorKam,
        Gerente,
        UnidadNegocio,
        GrupoColabEjecRegionBranch,
        FlagPrincipal,
        TipoInteresProdActiv,
        CanalInformación,
        RegionMercadoBranch,
        IdiomaComunicCliente,
        // Cuenta B2B
        CondicionPago,
        TipoMonedaDeLineaCredito
    }
    #endregion

    public class CuentaB2B_FK_MdmRepository : MdmBase<CuentaB2B>, ILookupId
    {
        #region Constants
        private const string ID_PARAMETER_NAME = "P_ID";
        private const string DESC_PARAMETER_NAME = "P_DESCRIPCION";
        #endregion

        #region Properties
        private object _description;
        #endregion

        #region MainMethod
        public object LookUpByDescription(CuentaB2B_FK foreignKey, object description)
        {
            try
            {
                _description = description;

                if (description == null)
                    throw new Exception("Description field is null. Invalid searching.");

                var idParameterValue = ExecuteAndGetID(foreignKey);                
                _description = null;

                return idParameterValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ExecuteForID
        private object ExecuteAndGetID(CuentaB2B_FK foreignKey)
        {
            try
            {
                //AddParameter(DESC_PARAMETER_NAME, _description); // Param: Descripción (IN)
                //AddParameter(ID_PARAMETER_NAME, null, ParameterDirection.Output); // Param: ID (OUT)
                ExecuteSPWithoutResults(GetSPName(foreignKey)); // Ejecutamos SP
                //return GetOutParameter(ID_PARAMETER_NAME); // Recuperamos ID
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetSPName(CuentaB2B_FK foreignKey)
        {
            string pkgName = DataBaseKeys.MdmPkg, spName = string.Empty;
            switch (foreignKey)
            {
                case CuentaB2B_FK.TipoPersona:
                    spName = "SP_BUSCAR_TIPOPERSONA";
                    break;
                case CuentaB2B_FK.PuntoContacto:
                    spName = "SP_BUSCAR_PUNTOCONTACTO";
                    break;                
                case CuentaB2B_FK.NivelImportancia:
                    spName = "SP_BUSCAR_NIVELIMPORTANCIA";
                    break;
                case CuentaB2B_FK.TipoCuenta:
                    spName = "SP_BUSCAR_TIPOCUENTA";
                    break;
                case CuentaB2B_FK.Estado:
                    spName = "SP_BUSCAR_ESTADO";
                    break;
                case CuentaB2B_FK.PaisProcedencia:
                    spName = "SP_BUSCAR_PAISPROCEDENCIA";
                    break;
                case CuentaB2B_FK.TipoDocumento:
                    spName = "SP_BUSCAR_DOCUMENTO";
                    break;
                case CuentaB2B_FK.TipoDireccion:
                    spName = "SP_BUSCAR_DIRECCION";
                    break;
                case CuentaB2B_FK.Distrito:
                    spName = "SP_BUSCAR_DISTRITO";
                    break;
                case CuentaB2B_FK.Ciudad:
                    spName = "SP_BUSCAR_CIUDAD";
                    break;
                case CuentaB2B_FK.Departamento:
                    spName = "SP_BUSCAR_DEPARTAMENTO";
                    break;
                case CuentaB2B_FK.Pais:
                    spName = "SP_BUSCAR_PAÍS";
                    break;
                case CuentaB2B_FK.TipoTelefono:
                    spName = "SP_BUSCAR_TELÉFONO";
                    break;
                case CuentaB2B_FK.TipoSitio:
                    spName = "SP_BUSCAR_SITIO";
                    break;
                case CuentaB2B_FK.TipoCorreo:
                    spName = "SP_BUSCAR_CORREO";
                    break;
                case CuentaB2B_FK.EmpleadoEjecResponsable:
                    spName = "SP_BUSCAR_EMPLEADOEJECRESPONSABLE";
                    break;
                case CuentaB2B_FK.SupervisorKam:
                    spName = "SP_BUSCAR_SUPERVISORKAM";
                    break;
                case CuentaB2B_FK.Gerente:
                    spName = "SP_BUSCAR_GERENTE";
                    break;
                case CuentaB2B_FK.UnidadNegocio:
                    spName = "SP_BUSCAR_UNIDADNEGOCIO";
                    break;
                case CuentaB2B_FK.GrupoColabEjecRegionBranch:
                    spName = "SP_BUSCAR_GRUPOCOLABEJECREGIONBRANCH";
                    break;
                case CuentaB2B_FK.FlagPrincipal:
                    spName = "SP_BUSCAR_FLAGPRINCIPAL";
                    break;
                case CuentaB2B_FK.TipoInteresProdActiv:
                    spName = "SP_BUSCAR_INTERESPRODACTIVIDAD";
                    break;
                case CuentaB2B_FK.CanalInformación:
                    spName = "SP_BUSCAR_CANALINFORMACIÓN";
                    break;
                case CuentaB2B_FK.RegionMercadoBranch:
                    spName = "SP_BUSCAR_BRANCH";
                    break;
                case CuentaB2B_FK.IdiomaComunicCliente:
                    spName = "SP_BUSCAR_IDIOMACLIENTE";
                    break;
                case CuentaB2B_FK.CondicionPago:
                    spName = "SP_BUSCAR_CONDICIONPAGO";
                    break;
                case CuentaB2B_FK.TipoMonedaDeLineaCredito:
                    spName = "SP_BUSCAR_TIPOMONEDADELINEACREDITO";
                    break;
                default:
                    throw new Exception("SPName parameter is null. Invalid searching.");
            }
            return string.Format("{0}.{1}", pkgName, spName);
        }
        #endregion
    }
}