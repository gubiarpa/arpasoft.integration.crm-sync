using Expertia.Estructura.Models;
using Expertia.Estructura.Models.NuevoMundo;
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
        public Operation Update(RptaInformacionPagoSF oRptaInformacionPagoSF)
        {
            var operation = new Operation();

            #region Parameters  
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, oRptaInformacionPagoSF.CodigoError, ParameterDirection.Input, 2);
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, oRptaInformacionPagoSF.MensajeError, ParameterDirection.Input, 1000);

            AddParameter(OutParameter.SF_IDOPORTUNIDAD_NM, OracleDbType.Varchar2, oRptaInformacionPagoSF.idOportunidad_SF);
            AddParameter(OutParameter.SF_IdInformacionPagoNM, OracleDbType.Varchar2, oRptaInformacionPagoSF.IdInfoPago_SF);
            AddParameter(OutParameter.IdCodeIdentifyNM, OracleDbType.Varchar2, oRptaInformacionPagoSF.CodigoServicio_NM); //iderntificador nm
            AddParameter(OutParameter.IdIdentificadorNM, OracleDbType.Int64, Convert.ToInt64(oRptaInformacionPagoSF.Identificador_NM)); //P_IDENTIFY_NM_CAMB id _reserva
            AddParameter(OutParameter.IdActualizados, OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Upd_InformacionPagoNM);
            operation[OutParameter.IdActualizados] = GetOutParameter(OutParameter.IdActualizados);
            #endregion

            return operation;
        }
        //

        public Operation GetListPagosServicio( int pIntCodWeb_in, int pIntCodCot_in, int pIntCodSuc_in, string tpCharTipoPaquete)
        {
            var operation = new Operation();

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter("pIntCodWeb_in", OracleDbType.Int32, pIntCodWeb_in);
            AddParameter("pIntCodCot_in", OracleDbType.Int32, pIntCodCot_in);
            AddParameter("pIntCodSuc_in", OracleDbType.Int32, pIntCodSuc_in);
            AddParameter("tpCharTipoPaquete", OracleDbType.Varchar2, tpCharTipoPaquete);
            AddParameter(OutParameter.CursorInformacionPagoNM, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Get_ListInformacionPagoNM);
            operation[OutParameter.CursorInformacionPagoNM] = ToListPagosServicio(GetDtParameter(OutParameter.CursorInformacionPagoNM));
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
                        montodescuento = row.FloatParse("montodescuento"),
                        textodescuento = row.StringParse("textodescuento"),
                        promowebcode = row.StringParse("promowebcode"),
                        totalfacturar = row.FloatParse("totalfacturar"),
                        //nombreHotel = row.StringParse("nombreHotel"), //no
                        //totalPagar = row.FloatParse("totalPagar"),//no
                        descripcion = row.StringParse("descripcion"),
                        feeAsumidoGeneralBoletos = row.FloatParse("feeAsumidoGeneral"),
                        numHabitacionPaquete = row.FloatParse("numHabitacionPaquete"),
                        tipoPasajeroPaq = row.StringParse("tipoPasajeroPaq"),
                        cantidadPasajeroPaq = row.FloatParse("cantidadPasajeroPaq"),
                        monedaPaq = row.StringParse("monedaPaq"),
                        precioUnitarioPaq = row.FloatParse("precioUnitarioPaq"),
                        totalUnitarioPaq = row.FloatParse("totalUnitarioPaq"),
                        precioTotalPorHabitacionPaq = row.FloatParse("precioTotalPorHabitacionPaq"),
                        precioTotalHabitacionesPaq = row.FloatParse("precioTotalHabitacionesPaq"),
                        gastosAdministrativosPaq = row.FloatParse("gastosAdministrativosPaq"),
                        tarjetaDeTurismo = row.FloatParse("tarjetaDeTurismo"),
                        tarjetaDeAsistencia = row.FloatParse("tarjetaDeAsistencia"),
                        precioTotalPagarPaq = row.FloatParse("precioTotalPagarPaq"),
                        textoDescuentoPaq = row.StringParse("textoDescuentoPaq"),
                        montoDescuentoPaq = row.FloatParse("montoDescuentoPaq"),
                        totalFacturarPaq = row.FloatParse("totalFacturarPaq"),
                        cantDiasSeg = row.IntParse("cantDiasSeg"),
                        precioUnitarioSeg = row.FloatParse("precioUnitarioSeg"),
                        MontoReservaSeg = row.FloatParse("MontoReservaSeg"),
                        IdInformacionPago_SF = row.StringParse("idinformacionpago_sf"),
                        accion_SF = row.StringParse("accion_SF"),
                        Id_Sucursal = row.StringParse("Id_Sucursal"),
                        Codigoweb = row.StringParse("Codigoweb"),
                        PaqueteId = row.StringParse("PaqueteId"),
                        SeguroId = row.StringParse("SeguroId"),
                        IdCotizacion = row.StringParse("IdCotizacion"),
                        OrdenServicio = row.StringParse("OrdenServicio"),
                        OrdenDatos = row.StringParse("OrdenDatos"),
                        DescuentoSeg = row.FloatParse("DescuentoSeg"),
                        MontoSeg = row.FloatParse("MontoSeg"),
                        paq_reserva_tipo = row.StringParse("paq_reserva_tipo")
                    });
                }
                return informacionPagoNMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IEnumerable<PagosServicioPaquete> ToListPagosServicio(DataTable dt)
        {
            try
            {
                var LPagosServicioPaquete = new List<PagosServicioPaquete>();

                foreach (DataRow row in dt.Rows)
                {
                    LPagosServicioPaquete.Add(new PagosServicioPaquete()
                    {
                        descripcionServ = row.StringParse("descripcionServ"),
                        pasajerosServ = row.FloatParse("pasajerosServ"),
                        precioServ = row.FloatParse("precioServ"),

                    });
                }
                return LPagosServicioPaquete;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}