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
    public class InformacionPagoNMRepository : OracleBase<object>, IInformacionPagoNMRepository
    {
        #region Constructor
        public InformacionPagoNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation GetInformacionPago(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs)
        {
            var operation = new Operation();

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_DETALLEPASAJEROSNM
            AddParameter(OutParameter.CursorInformacionPagoNM, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Get_InformacionPagoNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorInformacionPagoNM] = ToInformacionPagoNM(GetDtParameter(OutParameter.CursorInformacionPagoNM));
            #endregion

            return operation;
        }
        #endregion

        #region Parse
        private IEnumerable<InformacionPagoNM> ToInformacionPagoNM(DataTable dt)
        {
            try
            {
                var informacionPagoNMList = new List<InformacionPagoNM>();

                foreach (DataRow row in dt.Rows)
                {
                    informacionPagoNMList.Add(new InformacionPagoNM()
                    {
                        idOportunidad_SF = row.StringParse("idOportunidad_SF"),
                        Identificador_NM = row.StringParse("Identificador_NM"),
                        reservaID = row.StringParse("reservaID"),
                        tipoServicio = row.StringParse("tipoServicio"),
                        tipoPasajero = row.StringParse("tipoPasajero"),
                        totalBoleto = row.FloatParse("totalBoleto"),
                        tarifaNeto = row.FloatParse("tarifaNeto"),
                        impuestos = row.FloatParse("impuestos"),
                        cargos = row.FloatParse("cargos"),
                        nombreHotel = row.StringParse("nombreHotel"),
                        totalPagar = row.FloatParse("totalPagar"),
                        descripcion = row.StringParse("descripcion"),
                        feeAsumidoGeneral = row.StringParse("feeAsumidoGeneral"),
                        numHabitacionPaquete = row.FloatParse("numHabitacionPaquete"),
                        tipoPasajeroPaq = row.StringParse("tipoPasajeroPaq"),
                        cantidadPasajeroPaq = row.FloatParse("cantidadPasajeroPaq"),
                        monedaPaq = row.StringParse("monedaPaq"),
                        precioUnitarioPaq = row.FloatParse("precioUnitarioPaq"),
                        totalUnitarioPaq = row.FloatParse("totalUnitarioPaq"),
                        precioTotalPorHabitacionPaq = row.FloatParse("precioTotalPorHabitacionPaq"),
                        precioTotalHabitacionesPaq = row.FloatParse("precioTotalHabitacionesPaq"),
                        gastosAdministrativosPaq = row.FloatParse("gastosAdministrativosPaq"),
                        tarjetaDeTurismo = row.StringParse("tarjetaDeTurismo"),
                        tarjetaDeAsistencia = row.StringParse("tarjetaDeAsistencia"),
                        precioTotalPagarPaq = row.FloatParse("precioTotalPagarPaq"),
                        textoDescuentoPaq = row.StringParse("textoDescuentoPaq"),
                        montoDescuentoPaq = row.FloatParse("montoDescuentoPaq"),
                        totalFacturarPaq = row.FloatParse("totalFacturarPaq"),
                        cantDiasSeg = row.IntParse("cantDiasSeg"),
                        precioUnitarioSeg = row.FloatParse("precioUnitarioSeg"),
                        MontoReservaSeg = row.FloatParse("MontoReservaSeg"),
                        accion_SF = row.StringParse("accion_SF"),
                        IdInformacionPago_SF = row.StringParse("IdInformacionPago_SF"),
                        Id_Sucursal = row.StringParse("Id_Sucursal"),
                        Codigoweb = row.StringParse("Codigoweb"),
                        PaqueteId = row.StringParse("PaqueteId"),
                        SeguroId = row.StringParse("SeguroId"),
                        IdCotizacion = row.StringParse("IdCotizacion"),
                        OrdenServicio = row.StringParse("OrdenServicio"),
                        OrdenDatos = row.StringParse("OrdenDatos")
                    });
                }
                return informacionPagoNMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}