using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.Condor
{
    public class CuentaB2B_RbRepository : OracleBase<CuentaB2B>, ICrud<CuentaB2B>
    {
        public CuentaB2B_RbRepository() : base(ConnectionKeys.CondorConnKey)
        {
        }

        public Operation Create(CuentaB2B entity)
        {
            Operation operation = new Operation();
            try
            {
                string codigoError = string.Empty,
                    mensajeError = string.Empty,
                    nombreUsuario = string.Empty,
                    nombreEmpresa = string.Empty;

                /* (01) P_CODIGO_ERROR */
                AddParameter("P_CODIGO_ERROR", OracleDbType.Varchar2, codigoError, ParameterDirection.Output);
                /* (02) P_MENSAJE_ERROR */
                AddParameter("P_MENSAJE_ERROR", OracleDbType.Varchar2, mensajeError, ParameterDirection.Output);
                /* (03) P_NOMBRE_USUARIO */
                AddParameter("P_NOMBRE_USUARIO", OracleDbType.Varchar2, nombreUsuario); // ◄ No hay campos de auditoría
                /* (04) P_NOMBRE_EMPRESA */
                AddParameter("P_NOMBRE_EMPRESA", OracleDbType.Varchar2, nombreEmpresa); // ◄ Especificar el nombre
                /* (05) P_BRANCH */
                AddParameter("P_BRANCH", OracleDbType.Varchar2, entity.Branches.ToList()[0].RegionMercadoBranch); // ◄ No se tiene ID, sino descripción                
                /* (06) P_COD_CLIENTE_MDM */
                AddParameter("P_COD_CLIENTE_MDM", OracleDbType.Int32, entity.ID);
                /* (07) P_COD_CLIENTE_CRM */
                AddParameter("P_COD_CLIENTE_CRM", OracleDbType.Varchar2, entity.IdSalesForce);
                /* (08) P_NOMBRE_CLIENTE */
                AddParameter("P_NOMBRE_CLIENTE", OracleDbType.Varchar2, entity.RazonSocial);
                /* (09) P_ALIAS_CLIENTE */
                if (entity.Alias != null)
                    AddParameter("P_ALIAS_CLIENTE", OracleDbType.Varchar2, entity.Alias);
                /* (10) P_NOMBRE_TIPO_CLIENTE */
                AddParameter("P_NOMBRE_TIPO_CLIENTE", OracleDbType.Varchar2, entity.TipoCuenta); // ◄ No se tiene ID, sino descripción
                /* (11) P_ESTADO_CLIENTE */
                AddParameter("P_ESTADO_CLIENTE", OracleDbType.Varchar2, entity.Estado);
                /* (12) P_NOMBRE_CONDICION_PAGO */
                AddParameter("P_NOMBRE_CONDICION_PAGO", OracleDbType.Varchar2, entity.CondicionPago.ID);
                /* (13) P_NOMBRE_TIPO_PERSONA */
                AddParameter("P_NOMBRE_TIPO_PERSONA", OracleDbType.Varchar2, entity.TipoPersona);
                /* (14) P_NOMBRE_PAIS */
                AddParameter("P_NOMBRE_PAIS", OracleDbType.Varchar2, entity.PaisProcedencia); // ¿Este campo o de la dirección?
                /* (15) P_NOMBRE_CIUDAD */
                AddParameter("P_NOMBRE_CIUDAD", OracleDbType.Varchar2, entity.Direcciones.ToList()[0].Ciudad);
                /* (16) P_NOMBRE_IDIOMA */
                AddParameter("P_NOMBRE_IDIOMA", OracleDbType.Varchar2, entity.IdiomasComunicCliente.ToList()[0].ID);
                if (entity.Correos != null)
                {
                /* (17) P_EMAIL1 */
                    if (entity.Correos.ToList().Count > 0)
                        AddParameter("P_EMAIL1", OracleDbType.Varchar2, entity.Correos.ToList()[0].Descripcion);
                /* (18) P_EMAIL2 */
                    if (entity.Correos.ToList().Count > 1)
                        AddParameter("P_EMAIL2", OracleDbType.Varchar2, entity.Correos.ToList()[1].Descripcion);
                }
                if (entity.Telefonos != null)
                {
                /* (19) P_TELEFONO_OFICINA */
                    if (entity.Telefonos.ToList().Count > 0)
                        AddParameter("P_TELEFONO_OFICINA", OracleDbType.Varchar2, entity.Telefonos.ToList()[0].Numero);
                /* (20) P_TELEFONO_CELULAR */
                    if (entity.Telefonos.ToList().Count > 1)
                        AddParameter("P_TELEFONO_CELULAR", OracleDbType.Varchar2, entity.Telefonos.ToList()[1].Numero);
                /* (21) P_TELEFONO_ADICIONAL */
                    if (entity.Telefonos.ToList().Count > 2)
                        AddParameter("P_TELEFONO_ADICIONAL", OracleDbType.Varchar2, entity.Telefonos.ToList()[2].Numero);
                /* (22) P_TELEFONO_EMERGENCIA */
                    if (entity.Telefonos.ToList().Count > 3)
                        AddParameter("P_TELEFONO_EMERGENCIA", OracleDbType.Varchar2, entity.Telefonos.ToList()[3].Numero);
                }
                /* (23) P_SITIO_WEB */
                if (entity.Sitios != null)
                    AddParameter("P_SITIO_WEB", OracleDbType.Varchar2, entity.Sitios.ToList()[0].Descripcion);
                /* (24) P_DIRECCION */
                if (entity.Direcciones != null)
                    AddParameter("P_DIRECCION", OracleDbType.Varchar2, entity.Direcciones.ToList()[0].Descripcion);
                /* (25) P_NOTAS */
                AddParameter("P_NOTAS", OracleDbType.Varchar2, entity.Comentarios);

                ExecuteSPWithoutResults("CONDOR.CRM_PKG.SP_CREAR_CLIENTE");

                operation[Operation.Result] = ResultType.Success;
            }
            catch (Exception ex)
            {
                operation[Operation.Result] = ResultType.Fail;
                operation[Operation.ErrorMessage] = ex.Message;
            }
            return operation;
        }

        public Operation Delete(CuentaB2B entity)
        {
            throw new NotImplementedException();
        }

        public Operation Read(CuentaB2B entity)
        {
            throw new NotImplementedException();
        }

        public Operation Update(CuentaB2B entity)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<CuentaB2B> DataTableToEnumerable(DataTable dt)
        {
            throw new NotImplementedException();
        }
    }
}