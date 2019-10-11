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
    public class Contacto_CT_Repository : OracleBase<Contacto>, ICrud<Contacto>, ISameSPName<Contacto>
    {
        public Contacto_CT_Repository() : base(ConnectionKeys.CondorConnKey)
        {
        }

        #region PublicMethods
        public Operation Create(Contacto entity)
        {
            return ExecuteOperation(entity, StoredProcedureName.CT_Create_Contacto, entity.Auditoria.CreateUser.Descripcion);
        }

        public Operation Update(Contacto entity)
        {
            return ExecuteOperation(entity, StoredProcedureName.CT_Update_Contacto, entity.Auditoria.ModifyUser.Descripcion);
        }
        #endregion

        #region CommonMethods
        public Operation ExecuteOperation(Contacto entity, string SPName, string userName)
        {
            try
            {
                Operation operation = new Operation();
                object value;

                #region Parameters
                // (01) P_CODIGO_ERROR
                AddParameter("P_CODIGO_ERROR", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                // (02) P_MENSAJE_ERROR
                AddParameter("P_MENSAJE_ERROR", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
                // (03) P_NOMBRE_USUARIO
                AddParameter("P_NOMBRE_USUARIO", OracleDbType.Varchar2, userName);
                // (04) P_NOMBRE_EMPRESA
                AddParameter("P_NOMBRE_EMPRESA", OracleDbType.Varchar2, entity.UnidadNegocio.Descripcion);
                // (05) P_COD_CLIENTE_MDM
                AddParameter("P_COD_CLIENTE_MDM", OracleDbType.Varchar2, DBNull.Value);
                // (06) P_COD_CLIENTE_CRM
                AddParameter("P_COD_CLIENTE_CRM", OracleDbType.Varchar2, entity.IdCuentaSalesForce);
                // (07) P_COD_CONTACTO_MDM
                AddParameter("P_COD_CONTACTO_MDM", OracleDbType.Varchar2, DBNull.Value);
                // (08) P_COD_CONTACTO_CRM
                AddParameter("P_COD_CONTACTO_CRM", OracleDbType.Varchar2, entity.IdSalesforce);
                // (09) P_NOMBRES
                AddParameter("P_NOMBRES", OracleDbType.Varchar2, entity.Nombre);
                // (10) P_APELLIDO_PATERNO
                AddParameter("P_APELLIDO_PATERNO", OracleDbType.Varchar2, entity.ApePaterno);
                // (10) P_APELLIDO_MATERNO
                AddParameter("P_APELLIDO_MATERNO", OracleDbType.Varchar2, entity.ApeMaterno);
                // (11) P_ESTADO_CIVIL
                AddParameter("P_ESTADO_CIVIL", OracleDbType.Varchar2, entity.EstadoCivil.Descripcion);
                // (12) P_GENERO
                AddParameter("P_GENERO", OracleDbType.Varchar2, entity.Genero.Descripcion);
                // (13) P_FECHA_NACIMIENTO
                if (entity.FechaNacimiento == null) value = DBNull.Value; else value = entity.FechaNacimiento;
                AddParameter("P_FECHA_NACIMIENTO", OracleDbType.Date, value);
                // (14) P_EMAIL1
                if ((entity.Correos != null) && (entity.Correos.ToList().Count > 0)) value = entity.Correos.ToList()[0].Descripcion; else value = DBNull.Value;
                AddParameter("P_EMAIL1", OracleDbType.Varchar2, value);
                // (15) P_EMAIL2
                if ((entity.Correos != null) && (entity.Correos.ToList().Count > 1)) value = entity.Correos.ToList()[1].Descripcion; else value = DBNull.Value;
                AddParameter("P_EMAIL2", OracleDbType.Varchar2, value);
                // (16) P_TELEFONO1
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 0)) value = entity.Telefonos.ToList()[0].Numero; else value = DBNull.Value;
                AddParameter("P_TELEFONO1", OracleDbType.Varchar2, value);
                // (17) P_TELEFONO2
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 1)) value = entity.Telefonos.ToList()[1].Numero; else value = DBNull.Value;
                AddParameter("P_TELEFONO2", OracleDbType.Varchar2, value);
                // (18) P_IDIOMA
                AddParameter("P_IDIOMA", OracleDbType.Varchar2, entity.IdiomasComunicCliente.Array);
                // (19) P_CARGO
                AddParameter("P_CARGO", OracleDbType.Varchar2, entity.CargoEmpresa.Descripcion);
                // (20) P_ACTIVO → ¿Éste es el campo?
                AddParameter("P_ACTIVO", OracleDbType.Varchar2, entity.Estado.Descripcion);
                // (21) P_NOTAS
                AddParameter("P_NOTAS", OracleDbType.Varchar2, entity.Comentarios);
                // (22) P_ID_CUENTA
                value = DBNull.Value;
                AddParameter("P_ID_CUENTA", OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
                // (23) P_ID_CUENTA
                value = DBNull.Value;
                AddParameter("P_ID_CONTACTO", OracleDbType.Varchar2, value, ParameterDirection.Output, OutParameter.DefaultSize);
                #endregion

                #region Invoke
                ExecuteSPWithoutResults(SPName);

                operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
                operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
                operation[OutParameter.IdCuenta] = entity.ID = GetOutParameter(OutParameter.IdCuenta).ToString();
                operation[OutParameter.IdContacto] = entity.IDCuenta = GetOutParameter(OutParameter.IdContacto).ToString();
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
    }
}