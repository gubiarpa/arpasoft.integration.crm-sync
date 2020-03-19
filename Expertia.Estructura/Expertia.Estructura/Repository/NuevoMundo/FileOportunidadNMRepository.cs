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
    public class FileOportunidadRepository : OracleBase<FileOportunidadNM>
    {
        public FileOportunidadRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }

        public Operation AsociarFileOportunidad(FileOportunidadNM fileOportunidad)
        {
            var operation = new Operation();

            #region Parameter
            /// (01) P_CODIGO_ERROR
            AddParameter(OutParameter.SF_Codigo, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            /// (02) P_MENSAJE_ERROR
            AddParameter(OutParameter.SF_Mensaje, OracleDbType.Varchar2, DBNull.Value, ParameterDirection.Output, OutParameter.DefaultSize);
            ///// (03) P_ID_OPORTUNIDAD_SF
            //AddParameter("P_ID_OPORTUNIDAD_SF", OracleDbType.Varchar2, cotizacion.Id_Oportunidad_Sf);
            ///// (04) P_ID_COTIZACION_SF
            //AddParameter("P_ID_COTIZACION_SF", OracleDbType.Varchar2, cotizacion.Id_Cotizacion_Sf);
            ///// (05) P_ID_CUENTA_SF
            //AddParameter("P_ID_CUENTA_SF", OracleDbType.Varchar2, cotizacion.Id_Cuenta_Sf);
            ///// (06) P_USUARIO
            //AddParameter("P_USUARIO", OracleDbType.Varchar2, cotizacion.Usuario);
            ///// (07) P_ACCION
            //AddParameter("P_ACCION", OracleDbType.Varchar2, cotizacion.Accion);
            ///// (08) P_TIPO_DOCUMENTO
            //AddParameter("P_TIPO_DOCUMENTO", OracleDbType.Varchar2, cotizacion.Tipo_Documento);
            ///// (09) P_DOCUMENTO
            //AddParameter("P_DOCUMENTO", OracleDbType.Varchar2, cotizacion.Documento);
            ///// (10) P_CORREO
            //AddParameter("P_CORREO", OracleDbType.Varchar2, cotizacion.Correo);
            ///// (11) P_COTIZACION
            //AddParameter("P_COTIZACION", OracleDbType.Varchar2, cotizacion.Cotizacion);
            ///// (12) P_FILE
            //AddParameter("P_FILE", OracleDbType.Varchar2, cotizacion.File);
            ///// (13) P_NUMERO_PAXS
            //AddParameter("P_NUMERO_PAXS", OracleDbType.Varchar2, cotizacion.Numero_Paxs);
            
            #endregion



            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.CT_Obtiene_NovedadCotizacion);

            operation[OutParameter.CodigoError] = GetOutParameter(OutParameter.CodigoError);
            operation[OutParameter.MensajeError] = GetOutParameter(OutParameter.MensajeError);
            //operation[OutParameter.CursorCotizacionAsociada] = ToCotizacionJY(GetDtParameter(OutParameter.CursorCotizacionAsociada));
            #endregion

            return operation;
        }

        //private IEnumerable<CotizacionJYResponse> ToCotizacionJY(DataTable dt)
        //{
        //    try
        //    {
        //        var cotizaciones = new List<CotizacionJYResponse>();

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                cotizaciones.Add(new CotizacionJYResponse()
        //                {
        //                    Grupo = row.StringParse("Grupo"),
        //                    Estado = row.StringParse("Estado"),
        //                    VentaEstimada = row.FloatParse("Venta_Estimada"),
        //                    FileSubfile = row.StringParse("File_SubFile"),
        //                    VentaFile = row.FloatParse("Venta_File"),
        //                    MargenFile = row.FloatParse("Margen_File"),
        //                    PaxsFile = row.IntParse("Paxs_File"),
        //                    EstadoFile = row.StringParse("Estado_File"),
        //                    FechaInicioViaje = row.DateTimeParse("Fecha_Inicio_Viaje").ToString("dd/MM/yyyy")
        //                });
        //            }
        //        }
        //        return cotizaciones;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}