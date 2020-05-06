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

        public Operation Update(RptaOportunidadSF RptaOportunidadNM)
        {
            var operation = new Operation();

            #region Parameters            
            AddParameter(OutParameter.CodigoError, OracleDbType.Varchar2, RptaOportunidadNM.CodigoError, ParameterDirection.Input, 2);            
            AddParameter(OutParameter.MensajeError, OracleDbType.Varchar2, RptaOportunidadNM.MensajeError, ParameterDirection.Input,1000);            
            AddParameter(OutParameter.SF_IDOPORTUNIDAD_NM, OracleDbType.Varchar2, RptaOportunidadNM.idOportunidad_SF);
            AddParameter(OutParameter.IdIdentificadorNM, OracleDbType.Int64, Convert.ToInt64(RptaOportunidadNM.Identificador_NM));
            AddParameter(OutParameter.IdActualizados, OracleDbType.Int32, DBNull.Value, ParameterDirection.Output);
            #endregion

            #region Invoke
            ExecuteStoredProcedure(StoredProcedureName.AW_Upd_OportunidadNM);
            operation[OutParameter.IdActualizados] = GetOutParameter(OutParameter.IdActualizados);            
            #endregion

            return operation;
        }
        #endregion

        #region Parse
        private IEnumerable<OportunidadNM> ToOportunidadNM(DataTable dt)
        {
            try
            {
                Int32 Cot_IdTempo = 0;                
                ReservasOportunidad_NM ObjReservasOportunidad_NM = null;
                PlanReservaSeguro_NM objPlanReservaSeguro_NM = null;
                EmergenciaReservaSeguro_NM objEmergenciaReservaSeguro_NM = null;

                var oportunidadNMList = new List<OportunidadNM>();

                foreach (DataRow row in dt.Rows)
                {
                    if(Cot_IdTempo != row.IntParse("IdCotSRV"))
                    {
                        oportunidadNMList.Add(new OportunidadNM()
                        {
                            idCuenta_SF = row.StringParse("idCuenta_SF"),
                            Identificador_NM = row.StringParse("Identificador_NM"),
                            fechaRegistro = row.StringParse("fechaRegistro"),
                            IdCanalVenta = row.StringParse("IdCanalVenta"),
                            metabuscador = row.StringParse("metabuscador"),
                            CajaVuelos = (row.IntParse("CajaVuelos") > 0 ? true : false),
                            CajaHotel = (row.IntParse("CajaHotel") > 0 ? true : false),
                            CajaPaquetes = (row.IntParse("CajaPaquetes") > 0 ? true : false),
                            CajaServicios = (row.IntParse("CajaServicios") > 0 ? true : false),
                            CajaSeguro = (row.IntParse("CajaSeguro") > 0 ? true : false),
                            modoIngreso = row.StringParse("modoIngreso"),
                            ordenAtencion = row.StringParse("ordenAtencion"),
                            evento = row.StringParse("evento"),
                            Estado = row.StringParse("Estado"),
                            IdCotSRV = row.IntParse("IdCotSRV"),                           
                            counterAsignado = row.StringParse("counterAsignado"),
                            EmpresaCliente = row.StringParse("EmpresaCliente"),
                            nombreCliente = row.StringParse("nombreCliente"),
                            apellidosCliente = row.StringParse("apeliidosCliente"),
                            IdLoginWeb = row.StringParse("idLoginWeb"),
                            telefonoCliente = row.StringParse("telefonoCliente"),
                            accion_SF = row.StringParse("accion_SF")
                        });

                        if (Convert.IsDBNull(row["IdUsuarioSrv"]) == false)
                        {
                            oportunidadNMList[oportunidadNMList.Count - 1].IdUsuarioSrv = row.IntParse("IdUsuarioSrv");
                        }

                        if (Convert.IsDBNull(row["IdReserva"]) == false)
                        {
                            oportunidadNMList[oportunidadNMList.Count - 1].ListReservas = new List<ReservasOportunidad_NM>();
                        }
                    }

                    if (Convert.IsDBNull(row["IdReserva"]) == false) 
                    {
                        ObjReservasOportunidad_NM = new ReservasOportunidad_NM() {
                            IdReserva = row.StringParse("IdReserva"),
                            codReserva = row.StringParse("codReserva"),
                            fechaCreación = row.StringParse("fechaCreacion"),
                            estadoVenta = row.StringParse("estadoVenta"),
                            codigoAerolinea = row.StringParse("codigoAerolinea"),
                            Tipo = row.StringParse("Tipo"),
                            PCCOfficeID = row.StringParse("PCC_OfficeID"),
                            IATA = row.StringParse("IATA"),
                            RUCEmpresa = row.StringParse("RUC_Empresa"),
                            descripPaquete = row.StringParse("descripPaquete"),
                            destinoPaquetes = row.StringParse("destinoPaquetes"),
                            fechasPaquetes = row.StringParse("fechasPaquetes"),
                            Proveedor = row.StringParse("Proveedor")
                        };
                        
                        if (Convert.IsDBNull(row["PlanSeguro"]) == false)
                        {
                            objPlanReservaSeguro_NM = new PlanReservaSeguro_NM() {
                                Plan = row.StringParse("PlanSeguro"),                               
                                Destino = row.StringParse("Destino"),
                                FechaSalida = row.StringParse("FechaSalida"),
                                FechaRetorno = row.StringParse("FechaRetorno"),
                                Edades = row.StringParse("Edades")
                            };

                            if (Convert.IsDBNull(row["CantPasajeros"]) == false)
                            {
                                objPlanReservaSeguro_NM.CantPasajeros = row.IntParse("CantPasajeros");
                            }

                            ObjReservasOportunidad_NM.PlanSeguro = objPlanReservaSeguro_NM;
                        }

                        if (Convert.IsDBNull(row["NombreEmergencia"]) == false)
                        {
                            objEmergenciaReservaSeguro_NM = new EmergenciaReservaSeguro_NM()
                            {
                                Nombre = row.StringParse("NombreEmergencia"),
                                Apellido = row.StringParse("ApellidoEmergencia"),
                                Telefono = row.StringParse("TelefonoEmergencia"),
                                Email = row.StringParse("EmailEmergencia")
                            };
                            ObjReservasOportunidad_NM.EmergenciaSeguro = objEmergenciaReservaSeguro_NM;
                        }

                        oportunidadNMList[oportunidadNMList.Count - 1].ListReservas.Add(ObjReservasOportunidad_NM);
                    }
                                    
                    Cot_IdTempo = row.IntParse("IdCotSRV");
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