using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Expertia.Estructura.Repository.NuevoMundo
{
    public class OportunidadNMRepository : OracleBase<object>, IOportunidadNMRepository
    {
        #region Constructor
        public OportunidadNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation GetOportunidades()
        {
            var operation = new Operation();

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_OPORTUNIDADNM
            AddParameter(OutParameter.CursorOportunidadNM, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Get_OportunidadNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorOportunidadNM] = ToOportunidadNM(GetDtParameter(OutParameter.CursorOportunidadNM));
            #endregion

            return operation;
        }

        public Operation Update(OportunidadNM oportunidadNM)
        {
            var operation = new Operation();

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_IDCUENTA_SF
            AddParameter("P_IDOPORTUNIDAD_SF", OracleDbType.Varchar2, oportunidadNM.idOportunidad_SF);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Upd_CuentaNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            #endregion

            return operation;
        }
        #endregion

        #region Parse
        private IEnumerable<OportunidadNM> ToOportunidadNM(DataTable dt)
        {
            try
            {
                var oportunidadNMList = new List<OportunidadNM>();

                foreach (DataRow row in dt.Rows)
                {
                    oportunidadNMList.Add(new OportunidadNM()
                    {
                       idCuenta_SF = row.StringParse("IdCuenta_SF"),
                       fechaRegistro = row.StringParse("FechaRegistro"),
                       IdCanalVenta = row.StringParse("IdCanalVenta"),
                       metabuscador = row.StringParse("Metabuscador"),
                       CajaVuelos = row.BoolParse("CajaVuelos"),
                       CajaHotel = row.BoolParse("CajaHotel"),
                       CajaPaquetes = row.BoolParse("CajaPaquetes"),
                       CajaServicios = row.BoolParse("CajaServicios"),
                       modoIngreso = row.StringParse("ModoIngreso"),
                       ordenAtencion = row.StringParse("OrdenAtencion"),
                       evento = row.StringParse("Evento"),
                       Estado = row.StringParse("Estado"),
                       IdCotSRV = row.IntParse("IdCotSRV"),
                       IdUsuarioSrv = row.IntParse("IdUsuarioSrv"),
                       codReserva = row.StringParse("CodReserva"),
                       fechaCreación = row.StringParse("FechaCreación"),
                       estadoVenta = row.StringParse("EstadoVenta"),
                       codigoAerolinea = row.StringParse("CodigoAerolinea"),
                       Tipo = row.StringParse("Tipo"),
                       RUCEmpresa = row.IntParse("RUC"),
                       PCCOfficeID = row.StringParse("PCCOfficeID"),
                       counterAsignado = row.StringParse("CounterAsignado"),
                       IATA = row.StringParse("IATA"),
                       descripPaquete = row.StringParse("DescripPaquete"),
                       destinoPaquetes = row.StringParse("DestinoPaquetes"),
                       fechasPaquetes = row.StringParse("FechasPaquetes"),
                       EmpresaCliente = row.StringParse("EmpresaCliente"),
                       nombreCliente = row.StringParse("NombreCliente"),
                       apeliidosCliente = row.StringParse("ApeliidosCliente"),
                       IdLoginWeb = row.StringParse("IdLoginWeb"),
                       telefonoCliente = row.IntParse("TelefonoCliente"),
                       accion_SF = row.StringParse("Accion_SF")
                    });
                }

                return oportunidadNMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}