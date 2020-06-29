using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace Expertia.Estructura.Repository.NuevoMundo
{
    public class SolicitudPagoNMRepository : OracleBase<object>, ISolicitudPagoNMRepository
    {
        #region Constructor
        public SolicitudPagoNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation GetSolicitudesPago()
        {
            var operation = new Operation();

            #region Parameters
            /// (1) P_CODIGO_ERROR
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (2) P_MENSAJE_ERROR
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (3) P_DETALLEPASAJEROSNM
            AddParameter(OutParameter.CursorSolicitudPagoNM, OracleDbType.RefCursor, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Get_SolicitudPagoNM);
            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            operation[OutParameter.CursorSolicitudPagoNM] = ToSolicitudPagoNM(GetDtParameter(OutParameter.CursorSolicitudPagoNM));
            #endregion

            return operation;
        }

        public Operation Update(RptaSolicitudPagoSF RptaSolicitudPagoNM)
        {
            var operation = new Operation();

            #region Parameters  
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, RptaSolicitudPagoNM.CodigoError, ParameterDirection.Input, 2);
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, RptaSolicitudPagoNM.MensajeError, ParameterDirection.Input, 1000);
            AddParameter(OutParameter.SF_IDOPORTUNIDAD_NM, OracleDbType.Varchar2, RptaSolicitudPagoNM.idOportunidad_SF);
            AddParameter(OutParameter.SF_IdInformacionPagoNM, OracleDbType.Varchar2, RptaSolicitudPagoNM.IdRegSolicitudPago_SF);
            AddParameter(OutParameter.IdIdentificadorNM, OracleDbType.Int64, Convert.ToInt64(RptaSolicitudPagoNM.Identificador_NM));
            AddParameter(OutParameter.IdActualizados, OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Upd_SolicitudPagoNM);
            operation[OutParameter.IdActualizados] = GetOutParameter(OutParameter.IdActualizados);
            #endregion

            return operation;
        }
        #endregion

        #region Parse
        private IEnumerable<SolicitudPagoNM> ToSolicitudPagoNM(DataTable dt)
        {
            try
            {
                var solicitudPagoNMList = new List<SolicitudPagoNM>();
                SolicitudPagoNM objSolicitudPagoNM = null;

                foreach (DataRow row in dt.Rows)
                {
                    objSolicitudPagoNM = new SolicitudPagoNM() {
                        idOportunidad_SF = row.StringParse("IdOportunidad_SF"),
                        Identificador_NM = row.StringParse("Identificador_NM"),
                        IdPedido = row.IntParse("NRO_PEDIDO"),
                        pasarela = row.StringParse("Pasarela"), //row.StringParse("Pasarela"),
                        fechaPedido = row.DateTimeParse("FechaPedido").ToString("yyyy-MM-dd'T'HH:mm:ss+00:00"),
                        estado1 = row.StringParse("Estado1"),
                        estado2 = row.StringParse("Estado2"),
                        resultado = row.StringParse("Resultado"),
                        montoPagar = row.FloatParse("MontoPagar"),
                        rcGenerado = row.StringParse("RcGenerado"),
                        lineaAereaValidadora = row.StringParse("LineaAereaValidadora"),
                        formaPago = row.StringParse("idFormpaPago"),//row.StringParse("FormaPago"),
                        entidadBancaria = row.StringParse("EntidadBancaria"),
                        nroTarjeta = row.StringParse("NroTarjeta"),
                        titularTarjeta = row.StringParse("TitularTarjeta"),
                        expiracion = row.StringParse("Expiracion"),
                        thReniec = row.StringParse("ThReniec"),
                        marcaTC = row.StringParse("MarcaTC"),
                        tipoTC = row.StringParse("TipoTC"),
                        nivelTC = row.StringParse("NivelTC"),
                        paisTC = row.StringParse("PaisTC"),
                        EsAutenticada = row.StringParse("EsAutenticada"),
                        Detalle = row.StringParse("Detalle"),
                        LinkPago = row.StringParse("LinkPago"),
                        CodAutorTarj = row.StringParse("CodAutorTarj"),
                        TipoImporte = row.StringParse("TipoImporte"),
                        MontoImporte = row.StringParse("MontoImporte"),
                        PlazoDePago =  (row.StringParse("PlazoDePago")=="") ? null : row.DateTimeParse("PlazoDePago").ToString("yyyy-MM-dd'T'HH:mm:ss+00:00"),
                        Error = row.StringParse("Error"),
                        CodCanje = row.StringParse("CodCanje"),
                        Puntos = row.StringParse("Puntos"),
                        ipCliente = row.StringParse("IpCliente"),
                        docTitular = row.StringParse("DocTitular"),
                        accion_SF = row.StringParse("Accion_SF"),
                        WebCid = row.IntParse("WEBS_CID"),
                        IdCotizacion = row.IntParse("COTSRV_ID"),
                        idFormpaPago = row.IntParse("idFormpaPago"),
                        igv = row.FloatParse("IGV"),
                        nroCuotas = row.StringParse("NroCuotas"),
                        IdRegSolicitudPago_SF = row.StringParse("IDREGSOLICITUDPAGO_SF"),
                        codigoPago = row.StringParse("CodigoPago"),
                        fechaExpiracion = (row.StringParse("FechaExpiracionPago") == "") ? null : row.DateTimeParse("FechaExpiracionPago").ToString("yyyy-MM-dd'T'HH:mm:ss+00:00"), //row.StringParse("FechaExpiracionPago") ,
                        email = row.StringParse("email")
                    };
                    if (string.IsNullOrWhiteSpace(objSolicitudPagoNM.nroCuotas))
                    {
                        objSolicitudPagoNM.nroCuotas = "0";
                    }
                    objSolicitudPagoNM.FEE =-2 ;
                    objSolicitudPagoNM.GEM = 1;
                    objSolicitudPagoNM.PEF = 1;
                    //if ("56789".Contains(objSolicitudPagoNM.idFormpaPago.ToString()) == true)
                    //{
                    //    /*Calculo del GEM Y PEF*/
                    //    double dblIGV = objSolicitudPagoNM.igv / (double)100;
                    //    double dblTopeMonto = System.Convert.ToDouble(ConfigurationManager.AppSettings[Constantes_FEE.DBL_PAGOEFECTIVO_TOPE_MONTO_COMISION]);
                    //    double dblMontoComision1 = Convert.ToDouble(ConfigurationManager.AppSettings[Constantes_FEE.DBL_PAGOEFECTIVO_MONTO_COMISION1]);
                    //    double dblMontoComision2 = Convert.ToDouble(ConfigurationManager.AppSettings[Constantes_FEE.DBL_PAGOEFECTIVO_MONTO_COMISION2]);
                    //    double dblPctajeComision = Convert.ToDouble(ConfigurationManager.AppSettings[Constantes_FEE.DBL_PAGOEFECTIVO_PCTAJE_COMISION]);
                    //    double dblComisionIGV = 0;
                    //    double dblMontoPagar = objSolicitudPagoNM.montoPagar;
                    //    double dblComisionIGVTope = ((dblMontoComision2 * dblIGV) + dblMontoComision2);

                    //    double dblPEF = 0;
                    //    double dblGEM = 0;
                    //    if (dblMontoPagar >= dblTopeMonto)
                    //    {
                    //        double dblComision = ((dblMontoPagar * dblPctajeComision) / 100);
                    //        dblComisionIGV = dblComision + (dblComision * dblIGV);

                    //        if (dblComisionIGV > dblComisionIGVTope)
                    //        {
                    //            dblPEF = dblComisionIGVTope; ;
                    //        }                                    
                    //        else
                    //        {
                    //            dblPEF = dblComisionIGV;// ((dblMontoPagar * dblPctajeComision) / 100) + dblComisionIGV
                    //        }                                    
                    //    }
                    //    else
                    //    {
                    //        dblComisionIGV = dblMontoComision1 + (dblMontoComision1 * dblIGV);
                    //        // dblPEF = dblMontoComision1 + dblComisionIGV
                    //        dblPEF = dblComisionIGV;
                    //    }


                    //    objSolicitudPagoNM.FEE = (Convert.IsDBNull(row["FEE"]) == false ? row.FloatParse("FEE") : 0);                            
                    //    if (objSolicitudPagoNM.FEE.HasValue)
                    //    {
                    //        dblGEM = (double)objSolicitudPagoNM.FEE - dblPEF;
                    //    }
                    //    else
                    //    {
                    //        dblGEM = -1;
                    //    }

                    //    objSolicitudPagoNM.PEF = (float)dblPEF;                            
                    //    if (dblGEM >= 0)
                    //    {
                    //        objSolicitudPagoNM.GEM = (float)dblGEM;                            
                    //    }
                    //    if (objSolicitudPagoNM.FEE==0 )
                    //    {
                    //        objSolicitudPagoNM.FEE = 1;
                    //        objSolicitudPagoNM.GEM = 1;
                    //        objSolicitudPagoNM.PEF = 1;
                    //    }
                    //}

                    if (string.IsNullOrEmpty(objSolicitudPagoNM.LinkPago) == false && objSolicitudPagoNM.LinkPago == "SI")
                    {
                        objSolicitudPagoNM.LinkPago = Obtiene_LinkPago(objSolicitudPagoNM.WebCid, objSolicitudPagoNM.IdPedido, objSolicitudPagoNM.IdCotizacion);
                    }

                    solicitudPagoNMList.Add(objSolicitudPagoNM);
                }
                return solicitudPagoNMList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string Obtiene_LinkPago(int pIntIdWeb, int pIntIdPedido, int pIntIdCotSRV)
        {
            EncriptaCadena objNMEncriptaCadena = new EncriptaCadena();
            try
            {
                string strURLPago = "";
                if (pIntIdWeb == Webs_Cid.ID_WEB_NM_PERUTRIP)
                    strURLPago = ConfigurationManager.AppSettings["URL_PAGO_SERVICIO_ONLINE_PERUTRIP"];
                else if (pIntIdWeb == Webs_Cid.DM_WEB_ID)
                    strURLPago = ConfigurationManager.AppSettings["URL_PAGO_SERVICIO_ONLINE_DM"];
                else
                    strURLPago = ConfigurationManager.AppSettings["URL_PAGO_SERVICIO_ONLINE"];

                string strIdEncrypt = objNMEncriptaCadena.DES_Encrypt(pIntIdPedido + ";" + pIntIdCotSRV, objNMEncriptaCadena.GetKEY(EncriptaCadena.TIPO_KEY.KEY_ENCRIPTA_NRO_PEDIDO_PAGO_ONLINE));

                return strURLPago + "?id=" + strIdEncrypt;
            }
            catch (Exception ex)
            {
                return "Error";
            }
            finally
            {
                objNMEncriptaCadena = null;
            }
        }
        #endregion
    }
}