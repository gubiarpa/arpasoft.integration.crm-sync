using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Expertia.Estructura.Repository.Condor
{
    public class Contacto_CT_Repository : OracleBase<Contacto>, ICrud<Contacto>
    {
        public Contacto_CT_Repository() : base(ConnectionKeys.CondorConnKey)
        {
        }

        public Operation Create(Contacto entity)
        {
            Operation operation = new Operation();
            object value;

            #region Testing
            //entity.ID = "290788";
            var nombre_usuario = "nsanchez";
            var nombre_empresa = "CONDOR TRAVEL";
            #endregion

            try
            {
                #region Parameters
                // (01) P_CODIGO_ERROR
                AddParameter("P_CODIGO_ERROR", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, 4000);
                // (02) P_MENSAJE_ERROR
                AddParameter("P_MENSAJE_ERROR", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, 4000);
                // (03) P_NOMBRE_USUARIO
                AddParameter("P_NOMBRE_USUARIO", OracleDbType.Varchar2, nombre_usuario); // ◄ No hay campos de auditoría
                // (04) P_NOMBRE_EMPRESA
                AddParameter("P_NOMBRE_EMPRESA", OracleDbType.Varchar2, nombre_empresa); // ◄ Especificar el nombre
                // (00) P_COD_CLIENTE_MDM
                AddParameter("P_COD_CLIENTE_MDM", OracleDbType.Varchar2, null);
                // (00) P_COD_CLIENTE_CRM
                AddParameter("P_COD_CLIENTE_CRM", OracleDbType.Varchar2, entity.IdCuentaSalesForce);
                // (00) P_COD_CONTACTO_MDM
                AddParameter("P_COD_CONTACTO_MDM", OracleDbType.Varchar2, null);
                // (00) P_COD_CONTACTO_CRM
                AddParameter("P_COD_CONTACTO_CRM", OracleDbType.Varchar2, entity.IdSalesForce);
                // (00) P_NOMBRES
                AddParameter("P_NOMBRES", OracleDbType.Varchar2, entity.Nombre);
                // (00) P_APELLIDO_PATERNO
                AddParameter("P_APELLIDO_PATERNO", OracleDbType.Varchar2, entity.ApePaterno);
                // (00) P_APELLIDO_MATERNO
                AddParameter("P_APELLIDO_MATERNO", OracleDbType.Varchar2, entity.ApeMaterno);
                // (00) P_ESTADO_CIVIL
                AddParameter("P_ESTADO_CIVIL", OracleDbType.Varchar2, entity.EstadoCivil);
                // (00) P_GENERO
                AddParameter("P_GENERO", OracleDbType.Varchar2, entity.Genero);
                // (00) P_FECHA_NACIMIENTO
                AddParameter("P_FECHA_NACIMIENTO", OracleDbType.Varchar2, entity.FechaNacimiento);
                // (00) P_EMAIL1
                if ((entity.Correos != null) && (entity.Correos.ToList().Count > 0)) value = entity.Correos.ToList()[0].Descripcion; else value = DBNull.Value;
                AddParameter("P_EMAIL1", OracleDbType.Varchar2, value);
                // (00) P_EMAIL2
                if ((entity.Correos != null) && (entity.Correos.ToList().Count > 1)) value = entity.Correos.ToList()[1].Descripcion; else value = DBNull.Value;
                AddParameter("P_EMAIL2", OracleDbType.Varchar2, value);
                // (00) P_TELEFONO1
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 0)) value = entity.Telefonos.ToList()[0].Numero; else value = DBNull.Value;
                AddParameter("P_TELEFONO1", OracleDbType.Varchar2, value);
                // (00) P_TELEFONO2
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 1)) value = entity.Telefonos.ToList()[1].Numero; else value = DBNull.Value;
                AddParameter("P_TELEFONO2", OracleDbType.Varchar2, value);
                // (00) P_IDIOMA
                AddParameter("P_IDIOMA", OracleDbType.Varchar2, entity.IdiomasComunicCliente.ToList()[0].ID);
                // (00) P_CARGO
                AddParameter("P_CARGO", OracleDbType.Varchar2, entity.CargoEmpresa);
                // (00) P_ACTIVO
                AddParameter("P_ACTIVO", OracleDbType.Varchar2, entity.Estado);
                // (25) P_NOTAS
                AddParameter("P_NOTAS", OracleDbType.Varchar2, entity.Comentarios);
                #endregion

                #region Invoke
                ExecuteSPWithoutResults("CONDOR.CRM_PKG.SP_CREAR_CONTACTO");

                operation["P_CODIGO_ERROR"] = GetOutParameter("P_CODIGO_ERROR");
                operation["P_MENSAJE_ERROR"] = GetOutParameter("P_MENSAJE_ERROR");
                operation[Operation.Result] = ResultType.Success;
                #endregion

                return operation;
            }
            catch (Exception ex)
            {
                #region ErrorOut
                operation[Operation.Result] = ResultType.Fail;
                operation[Operation.ErrorMessage] = ex.Message;
                #endregion

                throw ex;
            }
        }

        public Operation Delete(Contacto entity)
        {
            throw new NotImplementedException();
        }

        public Operation Read(Contacto entity)
        {
            throw new NotImplementedException();
        }

        public Operation Update(Contacto entity)
        {
            Operation operation = new Operation();
            object value;

            #region Testing
            //entity.ID = "290788";
            var nombre_usuario = "nsanchez";
            var nombre_empresa = "CONDOR TRAVEL";
            #endregion

            try
            {
                #region Parameters
                // (01) P_CODIGO_ERROR
                AddParameter("P_CODIGO_ERROR", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, 4000);
                // (02) P_MENSAJE_ERROR
                AddParameter("P_MENSAJE_ERROR", OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, 4000);
                // (03) P_NOMBRE_USUARIO
                AddParameter("P_NOMBRE_USUARIO", OracleDbType.Varchar2, nombre_usuario); // ◄ No hay campos de auditoría
                // (04) P_NOMBRE_EMPRESA
                AddParameter("P_NOMBRE_EMPRESA", OracleDbType.Varchar2, nombre_empresa); // ◄ Especificar el nombre
                // (00) P_COD_CLIENTE_MDM
                AddParameter("P_COD_CLIENTE_MDM", OracleDbType.Varchar2, null);
                // (00) P_COD_CLIENTE_CRM
                AddParameter("P_COD_CLIENTE_CRM", OracleDbType.Varchar2, entity.IdCuentaSalesForce);
                // (00) P_COD_CONTACTO_MDM
                AddParameter("P_COD_CONTACTO_MDM", OracleDbType.Varchar2, null);
                // (00) P_COD_CONTACTO_CRM
                AddParameter("P_COD_CONTACTO_CRM", OracleDbType.Varchar2, entity.IdSalesForce);
                // (00) P_NOMBRES
                AddParameter("P_NOMBRES", OracleDbType.Varchar2, entity.Nombre);
                // (00) P_APELLIDO_PATERNO
                AddParameter("P_APELLIDO_PATERNO", OracleDbType.Varchar2, entity.ApePaterno);
                // (00) P_APELLIDO_MATERNO
                AddParameter("P_APELLIDO_MATERNO", OracleDbType.Varchar2, entity.ApeMaterno);
                // (00) P_ESTADO_CIVIL
                AddParameter("P_ESTADO_CIVIL", OracleDbType.Varchar2, entity.EstadoCivil);
                // (00) P_GENERO
                AddParameter("P_GENERO", OracleDbType.Varchar2, entity.Genero);
                // (00) P_FECHA_NACIMIENTO
                AddParameter("P_FECHA_NACIMIENTO", OracleDbType.Varchar2, entity.FechaNacimiento);
                // (00) P_EMAIL1
                if ((entity.Correos != null) && (entity.Correos.ToList().Count > 0)) value = entity.Correos.ToList()[0].Descripcion; else value = DBNull.Value;
                AddParameter("P_EMAIL1", OracleDbType.Varchar2, value);
                // (00) P_EMAIL2
                if ((entity.Correos != null) && (entity.Correos.ToList().Count > 1)) value = entity.Correos.ToList()[1].Descripcion; else value = DBNull.Value;
                AddParameter("P_EMAIL2", OracleDbType.Varchar2, value);
                // (00) P_TELEFONO1
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 0)) value = entity.Telefonos.ToList()[0].Numero; else value = DBNull.Value;
                AddParameter("P_TELEFONO1", OracleDbType.Varchar2, value);
                // (00) P_TELEFONO2
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 1)) value = entity.Telefonos.ToList()[1].Numero; else value = DBNull.Value;
                AddParameter("P_TELEFONO2", OracleDbType.Varchar2, value);
                // (00) P_IDIOMA
                AddParameter("P_IDIOMA", OracleDbType.Varchar2, entity.IdiomasComunicCliente.ToList()[0].ID);
                // (00) P_CARGO
                AddParameter("P_CARGO", OracleDbType.Varchar2, entity.CargoEmpresa);
                // (00) P_ACTIVO
                AddParameter("P_ACTIVO", OracleDbType.Varchar2, entity.Estado);
                // (25) P_NOTAS
                AddParameter("P_NOTAS", OracleDbType.Varchar2, entity.Comentarios);
                #endregion

                #region Invoke
                ExecuteSPWithoutResults("CONDOR.CRM_PKG.SP_ACTUALIZAR_CONTACTO");

                operation["P_CODIGO_ERROR"] = GetOutParameter("P_CODIGO_ERROR");
                operation["P_MENSAJE_ERROR"] = GetOutParameter("P_MENSAJE_ERROR");
                operation[Operation.Result] = ResultType.Success;
                #endregion

                return operation;
            }
            catch (Exception ex)
            {
                #region ErrorOut
                operation[Operation.Result] = ResultType.Fail;
                operation[Operation.ErrorMessage] = ex.Message;
                #endregion

                throw ex;
            }
        }

        protected override IEnumerable<Contacto> DataTableToEnumerable(DataTable dt)
        {
            throw new NotImplementedException();
        }
    }
}