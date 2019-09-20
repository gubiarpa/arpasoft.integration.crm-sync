using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Linq;

namespace Expertia.Estructura.Repository.Condor
{
    public class CuentaB2B_CT_Repository : OracleBase<CuentaB2B>, ICrud<CuentaB2B>, ISameSPName<CuentaB2B>
    {
        public CuentaB2B_CT_Repository() : base(ConnectionKeys.CondorConnKey)
        {
        }

        #region PublicMethods
        public Operation Create(CuentaB2B entity)
        {
            return ExecuteOperation(entity, "DESTINOS_TRP.CRM_PKG.SP_CREAR_CLIENTE", entity.Auditoria.CreateUser.Descripcion);
        }

        public Operation Update(CuentaB2B entity)
        {
            return ExecuteOperation(entity, "CONDOR.CRM_PKG.SP_ACTUALIZAR_CLIENTE", entity.Auditoria.ModifyUser.Descripcion);
        }
        #endregion

        #region CommonMethods
        public Operation ExecuteOperation(CuentaB2B entity, string SPName, string userName)
        {
            try
            {
                Operation operation = new Operation();
                object value;

                #region Parameters
                // (01) P_CODIGO_ERROR
                AddParameter("P_CODIGO_ERROR", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, 4000);
                // (02) P_MENSAJE_ERROR
                AddParameter("P_MENSAJE_ERROR", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, 4000);
                // (03) P_NOMBRE_USUARIO
                AddParameter("P_NOMBRE_USUARIO", OracleDbType.Varchar2, userName); // ◄ No hay campos de auditoría
                // (04) P_NOMBRE_EMPRESA
                AddParameter("P_NOMBRE_EMPRESA", OracleDbType.Varchar2, entity.UnidadNegocio.Descripcion); // ◄ Especificar el nombre
                // (05) P_BRANCH
                AddParameter("P_BRANCH", OracleDbType.Varchar2, entity.Branches.Array); // ◄ No se tiene ID, sino descripción                
                // (06) P_COD_CLIENTE_MDM
                AddParameter("P_COD_CLIENTE_MDM", OracleDbType.Varchar2, entity.ID);
                // (07) P_COD_CLIENTE_CRM
                AddParameter("P_COD_CLIENTE_CRM", OracleDbType.Varchar2, entity.IdSalesForce);
                // (08) P_NOMBRE_CLIENTE
                AddParameter("P_NOMBRE_CLIENTE", OracleDbType.Varchar2, entity.RazonSocial);
                // (09) P_ALIAS_CLIENTE
                AddParameter("P_ALIAS_CLIENTE", OracleDbType.Varchar2, entity.Alias.Coalesce());
                // (10) P_NOMBRE_TIPO_CLIENTE
                AddParameter("P_NOMBRE_TIPO_CLIENTE", OracleDbType.Varchar2, entity.TipoCuenta.Descripcion); // ◄ No se tiene ID, sino descripción
                // (11) P_ESTADO_CLIENTE
                AddParameter("P_ESTADO_CLIENTE", OracleDbType.Varchar2, entity.Estado.Descripcion);
                // (12) P_NOMBRE_CONDICION_PAGO
                if ((entity.CondicionesPago != null) && (entity.CondicionesPago.ToList().Count > 0)) value = entity.CondicionesPago.ToList()[0].Tipo.Descripcion; else value = DBNull.Value;
                AddParameter("P_NOMBRE_CONDICION_PAGO", OracleDbType.Varchar2, value);
                // (13) P_NOMBRE_TIPO_PERSONA
                AddParameter("P_NOMBRE_TIPO_PERSONA", OracleDbType.Varchar2, entity.TipoPersona.Descripcion);
                // (14) P_NOMBRE_PAIS
                AddParameter("P_NOMBRE_PAIS", OracleDbType.Varchar2, entity.Direcciones.ToList()[0].Pais.Descripcion);
                // (15) P_NOMBRE_CIUDAD
                AddParameter("P_NOMBRE_CIUDAD", OracleDbType.Varchar2, entity.Direcciones.ToList()[0].Ciudad.Descripcion);
                // (16) P_NOMBRE_IDIOMA
                AddParameter("P_NOMBRE_IDIOMA", OracleDbType.Varchar2, entity.IdiomasComunicCliente.Array);
                // (17) P_EMAIL1
                if ((entity.Correos != null) && (entity.Correos.ToList().Count > 0)) value = entity.Correos.ToList()[0].Descripcion; else value = DBNull.Value;
                AddParameter("P_EMAIL1", OracleDbType.Varchar2, value);
                // (18) P_EMAIL2
                if ((entity.Correos != null) && (entity.Correos.ToList().Count > 1)) value = entity.Correos.ToList()[1].Descripcion; else value = DBNull.Value;
                AddParameter("P_EMAIL2", OracleDbType.Varchar2, value);
                // (19) P_TELEFONO_OFICINA
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 0)) value = entity.Telefonos.ToList()[0].Numero; else value = DBNull.Value;
                AddParameter("P_TELEFONO_OFICINA", OracleDbType.Varchar2, value);
                // (20) P_TELEFONO_CELULAR */
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 1)) value = entity.Telefonos.ToList()[1].Numero; else value = DBNull.Value;
                AddParameter("P_TELEFONO_CELULAR", OracleDbType.Varchar2, value);
                // (21) P_TELEFONO_ADICIONAL */
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 2)) value = entity.Telefonos.ToList()[2].Numero; else value = DBNull.Value;
                AddParameter("P_TELEFONO_ADICIONAL", OracleDbType.Varchar2, value);
                // (22) P_TELEFONO_EMERGENCIA */
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 3)) value = entity.Telefonos.ToList()[3].Numero; else value = DBNull.Value;
                AddParameter("P_TELEFONO_EMERGENCIA", OracleDbType.Varchar2, value);
                // (23) P_SITIO_WEB */
                AddParameter("P_SITIO_WEB", OracleDbType.Varchar2, entity.Sitios.ToList()[0].Descripcion.Coalesce(DBNull.Value));
                // (24) P_DIRECCION */
                AddParameter("P_DIRECCION", OracleDbType.Varchar2, entity.Direcciones.ToList()[0].Descripcion.Coalesce(DBNull.Value));
                // (25) P_NOTAS */
                AddParameter("P_NOTAS", OracleDbType.Varchar2, entity.Comentarios);
                #endregion

                #region Invoke
                ExecuteSPWithoutResults(SPName);

                operation["P_CODIGO_ERROR"] = GetOutParameter("P_CODIGO_ERROR");
                operation["P_MENSAJE_ERROR"] = GetOutParameter("P_MENSAJE_ERROR");
                operation[Operation.Result] = ResultType.Success;
                #endregion

                return operation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region NonImplemented
        public Operation Delete(CuentaB2B entity)
        {
            throw new NotImplementedException();
        }

        public Operation Read(CuentaB2B entity)
        {
            throw new NotImplementedException();
        }
        #endregion



        

        

    }
}