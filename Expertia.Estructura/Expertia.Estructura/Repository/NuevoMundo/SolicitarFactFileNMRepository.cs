﻿using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.NuevoMundo
{
    public class SolicitarFactFileNMRepository : OracleBase<GenCodigoPagoNM>
    {
        public SolicitarFactFileNMRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }

        public Operation SolicitarFactFile(SolicitarFactFileNM solicitarFacFile)
        {
            var operation = new Operation();

            #region Parameter
            AddParameter(OutParameter.SF_Codigo, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            AddParameter(OutParameter.SF_Mensaje, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            AddParameter("P_ID_DATOS_FACTURACION", OracleDbType.Varchar2, solicitarFacFile.iddatosfacturacion);
            AddParameter("P_ESTADO", OracleDbType.Varchar2, solicitarFacFile.estado);
            AddParameter("P_DK", OracleDbType.Varchar2, solicitarFacFile.dk);
            AddParameter("P_SUBCODIGO", OracleDbType.Varchar2, solicitarFacFile.subcodigo);
            AddParameter("P_COMISIONISTA", OracleDbType.Varchar2, solicitarFacFile.comisionista);
            AddParameter("P_COMPANIA", OracleDbType.Varchar2, solicitarFacFile.campania);
            AddParameter("P_NUM_FILENM", OracleDbType.Varchar2, solicitarFacFile.numfilenm);
            AddParameter("P_NUM_FILEDM", OracleDbType.Varchar2, solicitarFacFile.numfiledm);
            AddParameter("P_CCB", OracleDbType.Varchar2, solicitarFacFile.ccb);
            AddParameter("P_FACTURA_RUC", OracleDbType.Varchar2, solicitarFacFile.facturaruc);
            AddParameter("P_RAZON_SOCIAL", OracleDbType.Varchar2, solicitarFacFile.razonsocial);
            AddParameter("P_CORREO", OracleDbType.Varchar2, solicitarFacFile.correo);
            AddParameter("P_TIPO_DOC_VENTA", OracleDbType.Varchar2, solicitarFacFile.tipodocventa);
            AddParameter("P_TIPO_DOC_IDENTIDAD", OracleDbType.Varchar2, solicitarFacFile.tipodocidentidad);
            AddParameter("P_NUM_DOC_IDENTIDAD", OracleDbType.Varchar2, solicitarFacFile.numdocidentidad);
            AddParameter("P_NOMBRE", OracleDbType.Varchar2, solicitarFacFile.nombre);
            AddParameter("P_APEPATERNO", OracleDbType.Varchar2, solicitarFacFile.apepaterno);
            AddParameter("P_APEMATERNO", OracleDbType.Varchar2, solicitarFacFile.apemateno);
            AddParameter("P_OA_RIPLEY", OracleDbType.Varchar2, solicitarFacFile.oaripley);
            AddParameter("P_OA_MONTO", OracleDbType.Varchar2, solicitarFacFile.oamonto);
            AddParameter("P_ID_USUARIO", OracleDbType.Varchar2, solicitarFacFile.idusuario);
            AddParameter("P_COTID", OracleDbType.Varchar2, solicitarFacFile.cotid);
            AddParameter("P_BANCO", OracleDbType.Varchar2, solicitarFacFile.banco);
            AddParameter("P_CANTIDAD_MILLAS", OracleDbType.Varchar2, solicitarFacFile.cantidadmillas);
            AddParameter("P_MONTO_MILLAS", OracleDbType.Varchar2, solicitarFacFile.montomillas);
            #region ReciboDetalleList
            var reciboList = solicitarFacFile.ReciboDetalleList.ElementAt(0);
            AddParameter("P_ID_DETALLE_NRO_RECIBO", OracleDbType.Varchar2, reciboList.IdDetalleNoRecibo);
            AddParameter("P_ID_SUCURSAL", OracleDbType.Varchar2, reciboList.IdSucursal);
            AddParameter("P_NOM_SUCURSAL", OracleDbType.Varchar2, reciboList.Sucursal);
            AddParameter("P_NRO_RECIBO", OracleDbType.Varchar2, reciboList.NoRecibo);
            AddParameter("P_MONTO_RECIBO", OracleDbType.Varchar2, reciboList.MontoRecibo);
            #endregion
            #region TarifaDetalleList
            var tarifaDetalleList = solicitarFacFile.TarifaDetalleList.ElementAt(0);
            AddParameter("P_PASAJERO_ADT", OracleDbType.Varchar2, tarifaDetalleList.CantidadADT);
            AddParameter("P_PASAJERO_CHD", OracleDbType.Varchar2, tarifaDetalleList.CantidadCHD);
            AddParameter("P_PASAJERO_INF", OracleDbType.Varchar2, tarifaDetalleList.CantidadINF);
            AddParameter("P_ID_GRUPO_SERVICIO", OracleDbType.Varchar2, tarifaDetalleList.IdGrupoServicio);
            AddParameter("P_GRUPO_SERVICIO", OracleDbType.Varchar2, tarifaDetalleList.GrupoServicio);
            AddParameter("P_TARIFA_ADT", OracleDbType.Varchar2, tarifaDetalleList.TarifaPorADT);
            AddParameter("P_TARIFA_CHD", OracleDbType.Varchar2, tarifaDetalleList.TarifaPorCHD);
            AddParameter("P_TARIFA_INF", OracleDbType.Varchar2, tarifaDetalleList.TarifaINF);
            AddParameter("P_MONTO_ADT", OracleDbType.Varchar2, tarifaDetalleList.MontoPorADT);
            AddParameter("P_MONTO_CHD", OracleDbType.Varchar2, tarifaDetalleList.MontoPorCHD);
            AddParameter("P_MONTO_INF", OracleDbType.Varchar2, tarifaDetalleList.MontoPorINF);
            #endregion
            #region ArchivoList
            var archivoList = solicitarFacFile.ArchivoList.ElementAt(0);
            AddParameter("P_RUTA_ARCHIVO", OracleDbType.Varchar2, archivoList.RutaArchivo);
            AddParameter("P_NOM_ARCHIVO", OracleDbType.Varchar2, archivoList.NomArchivo);
            AddParameter("P_URL_ARCHIVO", OracleDbType.Varchar2, archivoList.UrlArchivo);
            AddParameter("P_NRO_FILES", OracleDbType.Varchar2, archivoList.NumeroFiles);
            AddParameter("P_SUCURSAL", OracleDbType.Varchar2, archivoList.Sucursal);
            AddParameter("P_FECHA_ASOCIACION", OracleDbType.Varchar2, archivoList.FechaAsociacion);
            AddParameter("P_CLIENTE", OracleDbType.Varchar2, archivoList.Cliente);
            AddParameter("P_IMPORTE", OracleDbType.Varchar2, archivoList.Importe);
            AddParameter("P_ID_OPORTUNIDAD_SF", OracleDbType.Varchar2, archivoList.IdOportunidad_SF);
            AddParameter("P_ID_COT_SRV_SF", OracleDbType.Varchar2, archivoList.IdCotSrv_SF);
            #endregion
            AddParameter("P_MONTO_COBRAR", OracleDbType.Varchar2, solicitarFacFile.montocobrar);
            AddParameter("P_OBSERVACIONES", OracleDbType.Varchar2, solicitarFacFile.observaciones);
            AddParameter("P_ACCION_SF", OracleDbType.Varchar2, solicitarFacFile.accionsf);
            AddParameter("P_ID_USUARIO_SRV", OracleDbType.Varchar2, solicitarFacFile.idusuariosrv);
            #endregion

            #region Invoke
            try
            {
                ExecuteStoredProcedure(StoredProcedureName.AW_Solicitar_Facturacion_FileNM);
                operation[OutParameter.SF_Codigo] = GetOutParameter(OutParameter.SF_Codigo);
                operation[OutParameter.SF_Mensaje] = GetOutParameter(OutParameter.SF_Mensaje);
            }
            catch (Exception ex)
            {
                operation[OutParameter.SF_Codigo] = ApiResponseCode.ErrorCode;
                operation[OutParameter.SF_Mensaje] = ex.Message;
            }
            #endregion

            return operation;
        }

        #region Desglose
        public Operation GuardarDesgloseCA(SolicitarFactFileNM solicitarFactFile)
        {
            var operation = new Operation();

            UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs;
            OracleTransaction objTx = null; OracleConnection objConn = null;

            ExecuteConexionBegin(unidadNegocio.ToConnectionKey(), ref objTx, ref objConn);

            GuardarDatosFacturacion(solicitarFactFile, objTx, objConn);

            if (solicitarFactFile.existeIdDatosFacturacion)
            {
                EliminarDetalleTarifa(solicitarFactFile, objTx, objConn);
                EliminarDetalleNoRecibos(solicitarFactFile, objTx, objConn);
            }

            //Using objCnx As New OracleConnection(Data.strCnx_WebsOracle)
            //    objCnx.Open()

            //    Using objTx As OracleTransaction = objCnx.BeginTransaction
            //        IdDatosFactura = GuardarDatosFacturacion(model.datosFacturacion, objTx) 'Guardar

            //        If model.datosFacturacion.IdDatosFacturacion <> 0 Then
            //            EliminarDetalleTarifa(model.datosFacturacion.IdDatosFacturacion, objTx)
            //            EliminarDetalleNoRecibos(model.datosFacturacion.IdDatosFacturacion, objTx)
            //        End If

            //        GuardarDetalleTarifa(model, IdDatosFactura, objTx) 'Guardar detalle de Factura
            //        GuardarDetalleNoRecibo(model, IdDatosFactura, objTx) 'Guardar detalle de N° Recibo
            //        objTx.Commit()
            //    End Using
            //    result = IdDatosFactura
            //End Using

            objTx.Commit();
            return operation;
        }

        private void GuardarDatosFacturacion(SolicitarFactFileNM solicitarFactFile, OracleTransaction objTx, OracleConnection objConn)
        {
            var spName = solicitarFactFile.existeIdDatosFacturacion ?
                    "APPWEBS.PKG_Desglose_CA.SP_ACTUALIZAR_DATOSFACTURACION" :
                    "APPWEBS.PKG_Desglose_CA.SP_INSERTAR_DATOSFACTURACION";

            if (solicitarFactFile.existeIdDatosFacturacion)
                AddParameter("pIdDatosFacturacion", OracleDbType.Int32, solicitarFactFile.iddatosfacturacion);

            AddParameter("pEstado", OracleDbType.Int32, solicitarFactFile.estado);
            AddParameter("pDK", OracleDbType.NVarchar2, solicitarFactFile.dk);
            AddParameter("pSubCodigo", OracleDbType.NVarchar2, solicitarFactFile.subcodigo);
            AddParameter("pEjecutiva", OracleDbType.NVarchar2, string.Empty, ParameterDirection.Input); // (!) Orig., model.Ejecutiva
            AddParameter("pNumfileNM", OracleDbType.NVarchar2, solicitarFactFile.numfilenm);
            AddParameter("pNumfileDM", OracleDbType.NVarchar2, solicitarFactFile.numfiledm);
            AddParameter("pCCB", OracleDbType.NVarchar2, solicitarFactFile.ccb);
            AddParameter("pRUC", OracleDbType.NVarchar2, string.Empty); // (!) Orig., model.RUC
            AddParameter("pRAZON", OracleDbType.NVarchar2, solicitarFactFile.razonsocial);
            AddParameter("pTipoDocumento", OracleDbType.NVarchar2, solicitarFactFile.tipodocidentidad);
            AddParameter("PDoc_Cid", OracleDbType.NVarchar2, solicitarFactFile.numdocidentidad);
            AddParameter("pDOCUMENTO", OracleDbType.NVarchar2, string.Empty); // (!) Orig., model.Documento
            AddParameter("pNombre", OracleDbType.NVarchar2, solicitarFactFile.nombre);
            AddParameter("pApellidoP", OracleDbType.NVarchar2, solicitarFactFile.apepaterno);
            AddParameter("pApellidoM", OracleDbType.NVarchar2, solicitarFactFile.apemateno);
            AddParameter("pOARippley", OracleDbType.NVarchar2, solicitarFactFile.oaripley);
            AddParameter("pMontoOA", OracleDbType.Decimal, solicitarFactFile.oamonto);
            AddParameter("pIdUsuario", OracleDbType.Int32, string.Empty); // (!) Orig., model.IdUsuario
            AddParameter("pCot_Id", OracleDbType.Int32, solicitarFactFile.cotid);
            AddParameter("pCampania", OracleDbType.NVarchar2, solicitarFactFile.campania);
            AddParameter("pCorreo", OracleDbType.NVarchar2, solicitarFactFile.correo);
            AddParameter("pBanco", OracleDbType.NVarchar2, solicitarFactFile.banco);
            AddParameter("pCantidadMillas", OracleDbType.NVarchar2, solicitarFactFile.cantidadmillas);
            AddParameter("pMontoMillas", OracleDbType.Decimal, solicitarFactFile.montomillas);
            AddParameter("pObservacion", OracleDbType.Clob, solicitarFactFile.observaciones);

            ExecuteStorePBeginCommit(spName, objTx, objConn);
        }

        private void EliminarDetalleTarifa(SolicitarFactFileNM solicitarFactFile, OracleTransaction objTx, OracleConnection objConn)
        {
            var spName = "APPWEBS.PKG_Desglose_CA.SP_ELIMINAR_DETALLETARIFA";

            AddParameter("pIdDatosFacturacion", OracleDbType.Int32, solicitarFactFile.iddatosfacturacion);

            ExecuteStorePBeginCommit(spName, objTx, objConn);
        }

        private void EliminarDetalleNoRecibos(SolicitarFactFileNM solicitarFactFile, OracleTransaction objTx, OracleConnection objConn)
        {
            var spName = "APPWEBS.PKG_Desglose_CA.SP_ELIMINAR_DETALLENORECIBOS";

            AddParameter("pIdDatosFacturacion", OracleDbType.Int32, solicitarFactFile.iddatosfacturacion);

            ExecuteStorePBeginCommit(spName, objTx, objConn);
        }
        #endregion
    }
}