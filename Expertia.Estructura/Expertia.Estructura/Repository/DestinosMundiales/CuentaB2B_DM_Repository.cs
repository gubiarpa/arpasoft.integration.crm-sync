using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Linq;

namespace Expertia.Estructura.Repository.DestinosMundiales
{
    public class CuentaB2B_DM_Repository : OracleBase<CuentaB2B>, ICrud<CuentaB2B>, ISameSPName<CuentaB2B>
    {
        public CuentaB2B_DM_Repository() : base(ConnectionKeys.DMConnKey)
        {
        }

        #region PublicMethods
        public Operation Create(CuentaB2B entity)
        {
            return ExecuteOperation(entity, "DESTINOS_TRP.CRM_PKG.SP_CREAR_CLIENTE");
        }

        public Operation Update(CuentaB2B entity)
        {
            return ExecuteOperation(entity, "DESTINOS_TRP.CRM_PKG.SP_ACTUALIZAR_CLIENTE");
        }
        #endregion

        #region Auxiliar
        public Operation ExecuteOperation(CuentaB2B entity, string SPName)
        {
            try
            {
                Operation operation = new Operation();
                object value;

                #region Parameters
                // (01) P_CODIGO_ERROR
                value = DBNull.Value;
                AddParameter("P_CODIGO_ERROR", OracleDbType.Varchar2, value, ParameterDirection.Output, 4000);
                // (02) P_MENSAJE_ERROR
                value = DBNull.Value;
                AddParameter("P_MENSAJE_ERROR", OracleDbType.Varchar2, value, ParameterDirection.Output, 4000);
                // (03) P_NOMBRE_USUARIO
                value = entity.Auditoria.CreateUser.Descripcion;
                AddParameter("P_NOMBRE_USUARIO", OracleDbType.Varchar2, value);
                // (04) P_ID_CUENTA_SALESFORCE
                value = entity.IdSalesForce;
                AddParameter("P_ID_CUENTA_SALESFORCE", OracleDbType.Varchar2, value);
                // (05) P_RAZON_SOCIAL
                value = entity.RazonSocial;
                AddParameter("P_RAZON_SOCIAL", OracleDbType.Varchar2, value);
                // (06) P_FECHA_ANIVERSARIO
                value = entity.FechaNacimOrAniv;
                AddParameter("P_FECHA_ANIVERSARIO", OracleDbType.Varchar2, value);
                // (07) P_NOMBRE_PAIS_PROCEDENCIA
                value = entity.PaisProcedencia.Descripcion;
                AddParameter("P_NOMBRE_PAIS_PROCEDENCIA", OracleDbType.Varchar2, value);
                // (08) P_TIPO_DOCUMENTO_IDENTIDAD
                if ((entity.Documentos != null) && (entity.Documentos.ToList().Count > 0)) value = entity.Documentos.ToList()[0].Tipo.Descripcion; else value = DBNull.Value;
                AddParameter("P_TIPO_DOCUMENTO_IDENTIDAD", OracleDbType.Varchar2, value);
                // (09) P_NUMERO_DOCUMENTO
                if ((entity.Documentos != null) && (entity.Documentos.ToList().Count > 0)) value = entity.Documentos.ToList()[0].Numero; else value = DBNull.Value;
                AddParameter("P_NUMERO_DOCUMENTO", OracleDbType.Varchar2, value);
                // (10) P_TIPO_DIRECCION
                if ((entity.Direcciones != null) && (entity.Direcciones.ToList().Count > 0)) value = entity.Direcciones.ToList()[0].Tipo.Descripcion; else value = DBNull.Value;
                AddParameter("P_TIPO_DIRECCION", OracleDbType.Varchar2, value);
                // (11) P_DIRECCION
                if ((entity.Direcciones != null) && (entity.Direcciones.ToList().Count > 0)) value = entity.Direcciones.ToList()[0].Descripcion; else value = DBNull.Value;
                AddParameter("P_DIRECCION", OracleDbType.Varchar2, value);
                // (12) P_NOMBRE_PAIS
                if ((entity.Direcciones != null) && (entity.Direcciones.ToList().Count > 0)) value = entity.Direcciones.ToList()[0].Pais.Descripcion; else value = DBNull.Value;
                AddParameter("P_NOMBRE_PAIS", OracleDbType.Varchar2, value);
                // (13) P_DEPARTAMENTO
                if ((entity.Direcciones != null) && (entity.Direcciones.ToList().Count > 0)) value = entity.Direcciones.ToList()[0].Departamento.Descripcion; else value = DBNull.Value;
                AddParameter("P_DEPARTAMENTO", OracleDbType.Varchar2, value);
                // (14) P_CIUDAD
                if ((entity.Direcciones != null) && (entity.Direcciones.ToList().Count > 0)) value = entity.Direcciones.ToList()[0].Ciudad.Descripcion; else value = DBNull.Value;
                AddParameter("P_CIUDAD", OracleDbType.Varchar2, value);
                // (15) P_DISTRITO
                if ((entity.Direcciones != null) && (entity.Direcciones.ToList().Count > 0)) value = entity.Direcciones.ToList()[0].Distrito.Descripcion; else value = DBNull.Value;
                AddParameter("P_DISTRITO", OracleDbType.Varchar2, value);
                // (16) P_DIRECCION_FISCAL
                if ((entity.Direcciones != null) && (entity.Direcciones.ToList().Count > 1)) value = entity.Direcciones.ToList()[1].Descripcion; else value = DBNull.Value;
                AddParameter("P_DIRECCION_FISCAL", OracleDbType.Varchar2, value);
                // (17) P_TIPO_TELEFONO
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 0)) value = entity.Telefonos.ToList()[0].Tipo.Descripcion; else value = DBNull.Value;
                AddParameter("P_TIPO_TELEFONO", OracleDbType.Varchar2, value);
                // (18) P_TELEFONO
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 0)) value = entity.Telefonos.ToList()[0].Numero; else value = DBNull.Value;
                AddParameter("P_TELEFONO", OracleDbType.Varchar2, value);
                // (19) P_TIPO_TELEFONO_1
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 1)) value = entity.Telefonos.ToList()[1].Tipo.Descripcion; else value = DBNull.Value;
                AddParameter("P_TIPO_TELEFONO_1", OracleDbType.Varchar2, value);
                // (20) P_TELEFONO_1
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 1)) value = entity.Telefonos.ToList()[1].Numero; else value = DBNull.Value;
                AddParameter("P_TELEFONO_1", OracleDbType.Varchar2, value);
                // (21) P_TIPO_TELEFONO_2
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 2)) value = entity.Telefonos.ToList()[2].Tipo.Descripcion; else value = DBNull.Value;
                AddParameter("P_TIPO_TELEFONO_2", OracleDbType.Varchar2, value);
                // (22) P_TELEFONO_2
                if ((entity.Telefonos != null) && (entity.Telefonos.ToList().Count > 2)) value = entity.Telefonos.ToList()[2].Numero; else value = DBNull.Value;
                AddParameter("P_TELEFONO_2", OracleDbType.Varchar2, value);
                // (23) P_SITIO_WEB
                if ((entity.Sitios != null) && (entity.Sitios.ToList().Count > 0)) value = entity.Sitios.ToList()[0].Descripcion; else value = DBNull.Value;
                AddParameter("P_SITIO_WEB", OracleDbType.Varchar2, value);
                // (24) P_CORREO
                if ((entity.Correos != null) && (entity.Correos.ToList().Count > 0)) value = entity.Correos.ToList()[0].Descripcion; else value = DBNull.Value;
                AddParameter("P_CORREO", OracleDbType.Varchar2, value);
                // (25) P_PROPIETARIO
                if ((entity.Participantes != null) && (entity.Participantes.ToList().Count > 0)) value = entity.Participantes.ToList()[0].EmpleadoOrEjecutivoResponsable.Descripcion; else value = DBNull.Value;
                AddParameter("P_PROPIETARIO", OracleDbType.Varchar2, value);
                // (26) P_PUNTO_CONTACTO
                value = entity.PuntoContacto.Descripcion;
                AddParameter("P_PUNTO_CONTACTO", OracleDbType.Varchar2, value);
                // (27) P_NOMBRE_CONDICION_PAGO
                #region CondicionPago
                value = entity.CondicionesPago.ToList().SingleOrDefault(x => x.Empresa.GetUnidadNegocioKey().Equals(UnidadNegocioKeys.DestinosMundiales)).Tipo.Descripcion;
                AddParameter("P_NOMBRE_CONDICION_PAGO", OracleDbType.Varchar2, value);
                #endregion
                // (28) P_COMENTARIO
                value = entity.Comentarios;
                AddParameter("P_COMENTARIO", OracleDbType.Varchar2, value);
                // (29) P_TIPO_CUENTA
                value = entity.TipoCuenta.Descripcion;
                AddParameter("P_TIPO_CUENTA", OracleDbType.Varchar2, value);
                // (30) P_CATEG_VALOR
                value = entity.CategoriaValor.Descripcion;
                AddParameter("P_CATEG_VALOR", OracleDbType.Varchar2, value);
                // (31) P_CATEG_PERFIL_ACTITUD_TEC
                value = entity.CategoriaPerfilActitudTecnologica.Descripcion;
                AddParameter("P_CATEG_PERFIL_ACTITUD_TEC", OracleDbType.Varchar2, value);
                // (32) P_CATEG_PERFIL_FIDELIDAD
                value = entity.CategoriaPerfilFidelidad.Descripcion;
                AddParameter("P_CATEG_PERFIL_FIDELIDAD", OracleDbType.Varchar2, value);
                // (33) P_INCENTIVO
                value = entity.Incentivo.Descripcion;
                AddParameter("P_INCENTIVO", OracleDbType.Varchar2, value);
                // (34) P_ESTADO_ACTIVACION
                value = entity.Estado.Descripcion;
                AddParameter("P_ESTADO_ACTIVACION", OracleDbType.Varchar2, value);
                // (34) P_ID_CUENTA
                value = DBNull.Value;
                AddParameter("P_ID_CUENTA", OracleDbType.Varchar2, value, ParameterDirection.Output, 1000);
                #endregion

                #region Invoke
                ExecuteSPWithoutResults(SPName);

                operation["P_CODIGO_ERROR"] = GetOutParameter("P_CODIGO_ERROR");
                operation["P_MENSAJE_ERROR"] = GetOutParameter("P_MENSAJE_ERROR");
                operation["P_ID_CUENTA"] = GetOutParameter("P_ID_CUENTA");
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
            throw new System.NotImplementedException();
        }

        public Operation Read(CuentaB2B entity)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}