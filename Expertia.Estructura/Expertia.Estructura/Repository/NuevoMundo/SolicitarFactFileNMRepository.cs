using Expertia.Estructura.Models;
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
            /// (01) P_CODIGO_ERROR
            AddParameter(OutParameter.SF_Codigo, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (02) P_MENSAJE_ERROR
            AddParameter(OutParameter.SF_Mensaje, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            AddParameter("P_ID_DATOS_FACTURACION", OracleDbType.Varchar2, solicitarFacFile.iddatosfacturacion);
            AddParameter("P_ESTADO", OracleDbType.Varchar2, solicitarFacFile.estado);
            AddParameter("P_DK", OracleDbType.Varchar2, solicitarFacFile.dk);
            AddParameter("P_SUBCODIGO", OracleDbType.Varchar2, solicitarFacFile.subcodigo);
            AddParameter("P_COMISIONISTA", OracleDbType.Varchar2, solicitarFacFile.comisionista);
            AddParameter("P_COMPANIA", OracleDbType.Varchar2, solicitarFacFile.compania);
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
            AddParameter("P_ID_DETALLE_NRO_RECIBO", OracleDbType.Varchar2, solicitarFacFile.iddetallenrorecibo);
            AddParameter("P_ID_SUCURSAL", OracleDbType.Varchar2, solicitarFacFile.idsucursal);
            AddParameter("P_NOM_SUCURSAL", OracleDbType.Varchar2, solicitarFacFile.nomsucursal);
            AddParameter("P_NRO_RECIBO", OracleDbType.Varchar2, solicitarFacFile.nrorecibo);
            AddParameter("P_MONTO_RECIBO", OracleDbType.Varchar2, solicitarFacFile.montorecibo);
            AddParameter("P_PASAJERO_ADT", OracleDbType.Varchar2, solicitarFacFile.pasajeroadt);
            AddParameter("P_PASAJERO_CHD", OracleDbType.Varchar2, solicitarFacFile.pasajerochd);
            AddParameter("P_PASAJERO_INF", OracleDbType.Varchar2, solicitarFacFile.pasajeroinf);
            AddParameter("P_ID_GRUPO_SERVICIO", OracleDbType.Varchar2, solicitarFacFile.idgruposervicio);
            AddParameter("P_GRUPO_SERVICIO", OracleDbType.Varchar2, solicitarFacFile.gruposervicio);
            AddParameter("P_TARIFA_ADT", OracleDbType.Varchar2, solicitarFacFile.tarifaadt);
            AddParameter("P_TARIFA_CHD", OracleDbType.Varchar2, solicitarFacFile.tarifachd);
            AddParameter("P_TARIFA_INF", OracleDbType.Varchar2, solicitarFacFile.tarifainf);
            AddParameter("P_MONTO_ADT", OracleDbType.Varchar2, solicitarFacFile.montoadt);
            AddParameter("P_MONTO_CHD", OracleDbType.Varchar2, solicitarFacFile.montochd);
            AddParameter("P_MONTO_INF", OracleDbType.Varchar2, solicitarFacFile.montoinf);
            AddParameter("P_RUTA_ARCHIVO", OracleDbType.Varchar2, solicitarFacFile.rutaarchivo);
            AddParameter("P_NOM_ARCHIVO", OracleDbType.Varchar2, solicitarFacFile.nomarchivo);
            AddParameter("P_URL_ARCHIVO", OracleDbType.Varchar2, solicitarFacFile.urlarchivo);
            AddParameter("P_NRO_FILES", OracleDbType.Varchar2, solicitarFacFile.nrofiles);
            AddParameter("P_SUCURSAL", OracleDbType.Varchar2, solicitarFacFile.sucursal);
            AddParameter("P_FECHA_ASOCIACION", OracleDbType.Varchar2, solicitarFacFile.fechaasociacion);
            AddParameter("P_CLIENTE", OracleDbType.Varchar2, solicitarFacFile.cliente);
            AddParameter("P_IMPORTE", OracleDbType.Varchar2, solicitarFacFile.importe);
            AddParameter("P_ID_OPORTUNIDAD_SF", OracleDbType.Varchar2, solicitarFacFile.idoportunidadsf);
            AddParameter("P_ID_COT_SRV_SF", OracleDbType.Varchar2, solicitarFacFile.idcotsrvsf);
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
            catch (Exception)
            {

                operation[OutParameter.SF_Codigo] = "OK, Esto es codigo duro";
                operation[OutParameter.SF_Mensaje] = "Mensaje: Esto es codigo duro";
            }
            
            #endregion

            return operation;
        }
    }
}