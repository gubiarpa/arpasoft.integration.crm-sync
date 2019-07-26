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
        TipoPersona,
        PuntoContacto,
        RecibirInformacion,
        NivelImportancia,
        TipoCuenta,
        Estado,
        PaisProcedencia,
        Documento,
        Direccion,
        Distrito,
        Ciudad,
        Departamento,
        País,
        Teléfono,
        Sitio,
        Correo,
        EmpleadoEjecResponsable,
        SupervisorKam,
        Gerente,
        UnidadNegocio,
        GrupoColabEjecRegionBranch,
        FlagPrincipal,
        InteresProdActividad,
        CanalInformación,
        Branch,
        IdiomaCliente
    }
    #endregion

    public class CuentaB2B_FK_MdmRepository : MdmBase<CuentaB2B>, ILookupId
    {        
        private const string ID_PARAMETER_NAME = "P_ID";
        private const string DESC_PARAMETER_NAME = "P_DESCRIPCION";
        
        private object _description;
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

        #region ExecuteForID
        private object ExecuteAndGetID(CuentaB2B_FK foreignKey)
        {
            try
            {
                AddInParameter(DESC_PARAMETER_NAME, _description); // Param: Descripción (IN)
                AddOutParameter(ID_PARAMETER_NAME, null); // Param: ID (OUT)
                ExecuteSPWithoutResults(GetSPName(foreignKey)); // Ejecutamos SP
                return GetOutParameter(ID_PARAMETER_NAME); // Recuperamos ID
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetSPName(CuentaB2B_FK foreignKey)
        {
            switch (foreignKey)
            {
                case CuentaB2B_FK.TipoPersona:
                    return "SP_BUSCAR_TIPOPERSONA";
                case CuentaB2B_FK.PuntoContacto:
                    return "SP_BUSCAR_PUNTOCONTACTO";
                case CuentaB2B_FK.RecibirInformacion:
                    return "SP_BUSCAR_RECIBIRINFORMACION";
                case CuentaB2B_FK.NivelImportancia:
                    return "SP_BUSCAR_NIVELIMPORTANCIA";
                case CuentaB2B_FK.TipoCuenta:
                    return "SP_BUSCAR_TIPOCUENTA";
                case CuentaB2B_FK.Estado:
                    return "SP_BUSCAR_ESTADO";
                case CuentaB2B_FK.PaisProcedencia:
                    return "SP_BUSCAR_PAISPROCEDENCIA";
                case CuentaB2B_FK.Documento:
                    return "SP_BUSCAR_DOCUMENTO";
                case CuentaB2B_FK.Direccion:
                    return "SP_BUSCAR_DIRECCION";
                case CuentaB2B_FK.Distrito:
                    return "SP_BUSCAR_DISTRITO";
                case CuentaB2B_FK.Ciudad:
                    return "SP_BUSCAR_CIUDAD";
                case CuentaB2B_FK.Departamento:
                    return "SP_BUSCAR_DEPARTAMENTO";
                case CuentaB2B_FK.País:
                    return "SP_BUSCAR_PAÍS";
                case CuentaB2B_FK.Teléfono:
                    return "SP_BUSCAR_TELÉFONO";
                case CuentaB2B_FK.Sitio:
                    return "SP_BUSCAR_SITIO";
                case CuentaB2B_FK.Correo:
                    return "SP_BUSCAR_CORREO";
                case CuentaB2B_FK.EmpleadoEjecResponsable:
                    return "SP_BUSCAR_EMPLEADOEJECRESPONSABLE";
                case CuentaB2B_FK.SupervisorKam:
                    return "SP_BUSCAR_SUPERVISORKAM";
                case CuentaB2B_FK.Gerente:
                    return "SP_BUSCAR_GERENTE";
                case CuentaB2B_FK.UnidadNegocio:
                    return "SP_BUSCAR_UNIDADNEGOCIO";
                case CuentaB2B_FK.GrupoColabEjecRegionBranch:
                    return "SP_BUSCAR_GRUPOCOLABEJECREGIONBRANCH";
                case CuentaB2B_FK.FlagPrincipal:
                    return "SP_BUSCAR_FLAGPRINCIPAL";
                case CuentaB2B_FK.InteresProdActividad:
                    return "SP_BUSCAR_INTERESPRODACTIVIDAD";
                case CuentaB2B_FK.CanalInformación:
                    return "SP_BUSCAR_CANALINFORMACIÓN";
                case CuentaB2B_FK.Branch:
                    return "SP_BUSCAR_BRANCH";
                case CuentaB2B_FK.IdiomaCliente:
                    return "SP_BUSCAR_IDIOMACLIENTE";
                default:
                    throw new Exception("SPName parameter is null. Invalid searching.");
            }
        }        
        #endregion
    }
}