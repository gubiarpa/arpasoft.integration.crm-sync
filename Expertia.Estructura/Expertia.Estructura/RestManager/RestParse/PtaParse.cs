using Expertia.Estructura.Models;
using Expertia.Estructura.Models.NuevoMundo;
using Expertia.Estructura.Models.Retail;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;

namespace Expertia.Estructura.RestManager.RestParse
{
    public static class PtaParse
    {
        public static object ToSalesforceEntity(this CuentaPta cuentaPta)
        {
            try
            {
                var _unidadNegocio = cuentaPta.UnidadNegocio.ToUnidadNegocio();
                return new
                {
                    info = new
                    {
                        Accion = cuentaPta.Accion,
                        DkCuenta_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ? cuentaPta.DkCuenta.ToString() : null,
                        DkCuenta_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ? cuentaPta.DkCuenta.ToString() : null,
                        RazonSocial = cuentaPta.RazonSocial,
                        NombreComercial = cuentaPta.NombreComercial,
                        TipoCuenta = cuentaPta.TipoCuenta,
                        Propietario = cuentaPta.Propietario,
                        FechaAniversario = cuentaPta.FechaAniversario.ToString("dd/MM/yyyy"),
                        TipoDocumentoIdentidad = cuentaPta.TipoDocumentoIdentidad,
                        DocumentoIdentidad = cuentaPta.DocumentoIdentidad,
                        TipoDireccion = cuentaPta.TipoDireccion,
                        DireccionResidencia = cuentaPta.DireccionResidencia,
                        PaisResidencia = cuentaPta.PaisResidencia,
                        DepartamentoResidencia = cuentaPta.DepartamentoResidencia,
                        CiudadResidencia = cuentaPta.CiudadResidencia,
                        DistritoResidencia = cuentaPta.DistritoResidencia,
                        DireccionFiscal = cuentaPta.DireccionFiscal,
                        TipoTelefono1 = cuentaPta.TipoTelefono1,
                        Telefono1 = cuentaPta.Telefono1,
                        TipoTelefono2 = cuentaPta.TipoTelefono2,
                        Telefono2 = cuentaPta.Telefono2,
                        TipoTelefono3 = cuentaPta.TipoTelefono3,
                        Telefono3 = cuentaPta.Telefono3,
                        TelefonoEmergencia = cuentaPta.TelefonoEmergencia,
                        SitioWeb = cuentaPta.SitioWeb,
                        Twitter = cuentaPta.Twitter,
                        Facebook = cuentaPta.Facebook,
                        Linkedin = cuentaPta.LinkedIn,
                        Instagram = cuentaPta.Instagram,
                        TipoPresenciaDigital = cuentaPta.TipoPresenciaDigital,
                        UrlPresenciaDigital = cuentaPta.UrlPresenciaDigital,
                        TipoCorreo = cuentaPta.TipoCorreo,
                        Correo = cuentaPta.Correo,
                        Asesor_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ? cuentaPta.Asesor : null,
                        Asesor_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ? cuentaPta.Asesor : null,
                        PuntoContacto = cuentaPta.PuntoContacto,
                        CondicionPago_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ? cuentaPta.CondicionPago : null,
                        CondicionPago_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ? cuentaPta.CondicionPago : null,
                        LimiteCredito_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ? cuentaPta.LimiteCredito.ToString("0.00") : null,
                        LimiteCredito_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ? cuentaPta.LimiteCredito.ToString("0.00") : null,
                        Comentario = cuentaPta.Comentario,
                        CategValor = cuentaPta.CategoriaValor,
                        CategPerfilActitudTec = cuentaPta.CategoriaPerfilActitudTecnologica,
                        CategPerfilFidelidad = cuentaPta.CategoriaPerfilFidelidad,
                        Incentivo = cuentaPta.Incentivo,
                        EstadoActivacion = cuentaPta.EstadoActivacion,
                        Gds = cuentaPta.GDS,
                        Herramientas = cuentaPta.Herramientas,
                        FacturacionAnual_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ? cuentaPta.FacturacionAnual : null,
                        FacturacionAnual_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ? cuentaPta.FacturacionAnual : null,
                        ProyeccionFactAnual_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ? cuentaPta.ProyeccionFacturacionAnual : null,
                        ProyeccionFactAnual_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ? cuentaPta.ProyeccionFacturacionAnual : null,
                        InicioRelacionComercial_IA = _unidadNegocio.Equals(UnidadNegocioKeys.Interagencias) ? cuentaPta.InicioRelacionComercial.ToString("dd/MM/yyyy") : null,
                        InicioRelacionComercial_DM = _unidadNegocio.Equals(UnidadNegocioKeys.DestinosMundiales) ? cuentaPta.InicioRelacionComercial.ToString("dd/MM/yyyy") : null
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ToSalesforceEntity(this ContactoPta contactoPta)
        {
            try
            {
                return new
                {
                    datos = new
                    {
                        accion = contactoPta.Accion,
                        dkCuenta_DM = contactoPta.UnidadNegocio.ToUnidadNegocio().Equals(UnidadNegocioKeys.DestinosMundiales) ? contactoPta.DkAgencia : string.Empty,
                        dkCuenta_IA = contactoPta.UnidadNegocio.ToUnidadNegocio().Equals(UnidadNegocioKeys.Interagencias) ? contactoPta.DkAgencia : string.Empty,
                        primerNombre = contactoPta.PrimerNombre ?? string.Empty,
                        segundoNombre = contactoPta.SegundoNombre ?? string.Empty,
                        apellidoPaterno = contactoPta.ApellidoPaterno ?? string.Empty,
                        apellidoMaterno = contactoPta.ApellidoMaterno ?? string.Empty,
                        estadoCivil = contactoPta.EstadoCivil,
                        cargo = contactoPta.Cargo,
                        tipoContacto = contactoPta.TipoContacto,
                        genero = contactoPta.Genero,
                        fechaNacimiento = contactoPta.FechaNacimiento.ToString("dd/MM/yyyy"),
                        tieneHijos = contactoPta.TieneHijos,
                        tipoDocumentoIdentidad = contactoPta.TipoDocumentoIdentidad ?? string.Empty,
                        documentoIdentidad = contactoPta.DocumentoIdentidad ?? string.Empty,
                        direccion = contactoPta.Direccion,
                        twitter = contactoPta.Twitter,
                        facebook = contactoPta.Facebook,
                        linkedin = contactoPta.LinkedIn,
                        instagram = contactoPta.Instagram,
                        tipoPresenciaDigital = contactoPta.TipoPresenciaDigital,
                        urlPresenciaDigital = contactoPta.UrlPresenciaDigital,
                        tipoTelefono1 = contactoPta.TipoTelefono1,
                        telefono1 = contactoPta.Telefono1,
                        tipoTelefono2 = contactoPta.TipoTelefono2,
                        telefono2 = contactoPta.Telefono2,
                        telefonoEmergencia = contactoPta.TelefonoEmergencia,
                        tipoCorreo = contactoPta.TipoCorreo,
                        correo = contactoPta.Correo,
                        estadoContacto = contactoPta.EstadoContacto,
                        esContactoMarketing = contactoPta.EsContactoMarketing
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ToSalesforceEntity(this Subcodigo subcodigo)
        {
            try
            {
                return new
                {
                    info = new
                    {
                        Accion = subcodigo.Accion,
                        DkCuenta_DM = subcodigo.DkAgencia_DM,
                        DkCuenta_IA = subcodigo.DkAgencia_IA,
                        CorrelativoSubcodigo_DM = subcodigo.CorrelativoSubcodigo_DM,
                        CorrelativoSubcodigo_IA = subcodigo.CorrelativoSubcodigo_IA,
                        DireccionSucursal = subcodigo.DireccionSucursal,
                        EstadoSucursal = subcodigo.EstadoSucursal,
                        NombreSucursal = subcodigo.NombreSucursal,
                        Promotor_DM = subcodigo.Promotor_DM,
                        Promotor_IA = subcodigo.Promotor_IA,
                        CondicionPago_DM = subcodigo.CondicionPago_DM,
                        CondicionPago_IA = subcodigo.CondicionPago_IA
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ToSalesforceEntity(this Oportunidad oportunidad)
        {
            try
            {
                return new
                {
                    info = new
                    {
                        idOportunidad = oportunidad.IdOportunidad,
                        accion = oportunidad.Accion,
                        etapa = oportunidad.Etapa,
                        dkCuenta = oportunidad.DkCuenta.ToString(),
                        unidadNegocio = oportunidad.UnidadNegocio,
                        sucursal = oportunidad.NombreSubcodigo,
                        puntoVenta = oportunidad.PuntoVenta,
                        subcodigo = oportunidad.Subcodigo,
                        fechaOportunidad = oportunidad.FechaOportunidad.ToString("dd/MM/yyyy"),
                        nombreOportunidad = oportunidad.NombreOportunidad,
                        origenOportunidad = oportunidad.OrigenOportunidad,
                        medioOportunidad = oportunidad.MedioOportunidad,
                        gds = oportunidad.GDS,
                        tipoProducto = oportunidad.TipoProducto,
                        rutaViaje = oportunidad.RutaViaje,
                        ciudadOrigen = oportunidad.CiudadOrigen,
                        ciudadDestino = oportunidad.CiudadDestino,
                        tipoRuta = oportunidad.TipoRuta,
                        numPasajeros = oportunidad.NumPasajeros,
                        fechaInicioViaje1 = oportunidad.FechaInicioViaje1.ToString("dd/MM/yyyy"),
                        fechaFinViaje1 = oportunidad.FechaFinViaje1.ToString("dd/MM/yyyy"),
                        fechaInicioViaje2 = oportunidad.FechaInicioViaje2.ToString("dd/MM/yyyy"),
                        fechaFinViaje2 = oportunidad.FechaFinViaje2.ToString("dd/MM/yyyy"),
                        montoEstimado = oportunidad.MontoEstimado,
                        montoReal = oportunidad.MontoReal,
                        pnr1 = oportunidad.Pnr1,
                        pnr2 = oportunidad.Pnr2,
                        motivoPerdida = oportunidad.MotivoPerdida,
                        contacto = oportunidad.Contacto,
                        counterVentas = oportunidad.CounterVentas,
                        counterAdm = oportunidad.CounterAdmin
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ToSalesforceEntity(this Cotizacion_DM cotizacionDM)
        {
            try
            {
                return new
                {
                    ID_OPORTUNIDAD_SF = cotizacionDM.IdOportunidadSf,
                    ID_COTIZACION_SF = string.IsNullOrEmpty(cotizacionDM.IdCotizacionSf) ? null : cotizacionDM.IdCotizacionSf,
                    COTIZACION = cotizacionDM.IdCotizacion,
                    MONTO_COTIZACION = cotizacionDM.MontoCotizacion,
                    MONTO_COMISION = cotizacionDM.MontoComision,
                    ESTADO_COTIZACION = cotizacionDM.EstadoCotizacion,
                    NOMBRE_COTIZACION = cotizacionDM.NombreCotizacion,
                    NUM_PASAJEROS_ADL = cotizacionDM.NumPasajerosAdult,
                    NUM_PASAJEROS_CHD = cotizacionDM.NumPasajerosChild,
                    NUM_PASAJEROS_TOT = cotizacionDM.NumPasajerosTotal
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ToSalesforceEntity(this PedidosProcesados pedidosProcesados)
        {
            try
            {
                return new
                {
                    codigoTransaccion = pedidosProcesados.codigoTransaccion,
                    idSolicitudPago_SF = pedidosProcesados.idSolicitudPago_SF,
                    estadoPago = pedidosProcesados.estadoPago,
                    tipoSolicitud = true
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ToSalesforceEntity(this FilesAsociadosSRV filesAsociadosSRV)
        {
            try
            {
                return new
                {
                    info = new
                    {
                        idOportunidad = filesAsociadosSRV.id_oportunidad,
                        //idCotSrv_SF = filesAsociadosSRV.cot_id,
                        numeroFile = filesAsociadosSRV.file_id,
                        importe = filesAsociadosSRV.fpta_imp_fact,
                        sucursal = filesAsociadosSRV.suc_id,
                        fecha = filesAsociadosSRV.fpta_fecha.ToString("dd/MM/yyyy")
                    }
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ToSalesforceEntity(this Lead lead)
        {
            try
            {
                return new
                {
                    lead.IdLeads_SF,
                    lead.PrimerNombre,
                    lead.SegundoNombre,
                    lead.apellidoPaterno,
                    lead.apellidoMaterno,
                    lead.Titulo_profesional,
                    lead.Email,
                    lead.Telefono,
                    lead.Celular,
                    lead.Fax,
                    lead.Comentarios_Lead,
                    lead.Nombre_empresa,
                    lead.Cliente_de,
                    lead.Idioma_preferencia,
                    lead.Cliente_Trabajado,
                    Fuente = "Web",
                    lead.Address,
                    lead.Are_you,
                    lead.Best_Time,
                    lead.Country_Residence,
                    lead.Country_Visit,
                    lead.Departure_City,
                    Departure_Date = Convert.ToDateTime(lead.Departure_Date).ToString("dd/MM/yyyy"),
                    lead.Duration_Trip,
                    lead.Group_Private,
                    lead.Nivel_acomodacion,
                    lead.Ideal_Budget,
                    lead.Ubicacion_IP,
                    lead.Numero_Adultos,
                    lead.num_kids0_24m,
                    lead.num_kids2_5y,
                    lead.num_kids6_12,
                    lead.Referring_URL,
                    lead.Skype,
                    lead.Touchpoint,
                    lead.Tour_Interest,
                    lead.Propietario_oportunidad,
                    lead.Ejecutivo
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Nuevo Mundo
        public static object ToSalesforceEntity(this CuentaNM cuentaNM)
        {
            try
            {
                return new
                {
                    cuentaNM.idCuenta_SF,
                    cuentaNM.idCuenta_NM,
                    cuentaNM.nombreCliente,
                    ApePatCli = cuentaNM.apellidoCliente,
                    ApeMatCli = cuentaNM.apeMatCli,
                    cuentaNM.emailCliente,
                    cuentaNM.enviarPromociones,
                    cuentaNM.tipoTelefono1,
                    cuentaNM.pais1,
                    cuentaNM.codArea1,
                    cuentaNM.Anexo1,
                    cuentaNM.numero1,
                    cuentaNM.tipoTelefono2,
                    cuentaNM.pais2,
                    cuentaNM.codArea2,
                    cuentaNM.Anexo2,
                    cuentaNM.numero2,
                    cuentaNM.tipoTelefono3,
                    cuentaNM.pais3,
                    cuentaNM.codArea3,
                    cuentaNM.Anexo3,
                    cuentaNM.numero3,
                    Direccion = cuentaNM.direccion,
                    cuentaNM.idUsuarioSrv_SF,
                    cuentaNM.accion_SF,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ToSalesforceEntity(this OportunidadNM oportunidadNM)
        {
            try
            {
                return new
                {
                    oportunidadNM.idCuenta_SF,                    
                    oportunidadNM.Identificador_NM,
                    oportunidadNM.idOportunidad_SF,
                    oportunidadNM.fechaRegistro,
                    oportunidadNM.Servicio,
                    oportunidadNM.IdCanalVenta,
                    oportunidadNM.metabuscador,                    
                    oportunidadNM.modoIngreso,
                    oportunidadNM.ordenAtencion,
                    oportunidadNM.evento,
                    oportunidadNM.Estado,
                    oportunidadNM.IdCotSRV,
                    oportunidadNM.IdUsuarioSrv,
                    oportunidadNM.requiereFirmaCliente,                    
                    oportunidadNM.counterAsignado,
                    datosReservas = (oportunidadNM.ListReservas == null ? null :
                        oportunidadNM.ListReservas.ToSalesforceEntity()
                    ),
                    idLoginWeb = oportunidadNM.IdLoginWeb,
                    oportunidadNM.EmpresaCliente,
                    oportunidadNM.nombreCliente,
                    oportunidadNM.apellidosCliente,
                    oportunidadNM.emailUserLogin,                    
                    oportunidadNM.telefonoCliente,
                    oportunidadNM.IdMotivoNoCompro,
                    oportunidadNM.Emitido,
                    oportunidadNM.fechaPlazoEmision,
                    oportunidadNM.CiudadIata,
                    //oportunidadNM.ServiciosAdicionales,
                    //oportunidadNM.CantidadAdultos,
                    //oportunidadNM.CantidadNinos,
                    //oportunidadNM.FechaIngreso,
                    //oportunidadNM.Fecharegreso,
                    oportunidadNM.MontoEstimado,
                    oportunidadNM.ModalidadCompra,
                    oportunidadNM.tipoCotizacion,                    
                    oportunidadNM.accion_SF
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<object> ToSalesforceEntity(this List<ReservasOportunidad_NM> ListReservasOp_NM)
        {
            try
            {
                if (ListReservasOp_NM != null && ListReservasOp_NM.Count > 0)
                {
                    var objListReservasOp_NM = new List<object>();
                    foreach (ReservasOportunidad_NM ReservasOp_NM in ListReservasOp_NM)
                    {
                        objListReservasOp_NM.Add(
                            new
                            {
                                ReservasOp_NM.IdReserva,
                                ReservasOp_NM.codReserva,
                                fechaCreacion = ReservasOp_NM.fechaCreación,
                                ReservasOp_NM.estadoVenta,
                                ReservasOp_NM.codigoAerolinea,
                                ReservasOp_NM.Tipo,
                                ReservasOp_NM.PCCOfficeID,
                                ReservasOp_NM.IATA,
                                ReservasOp_NM.RUCEmpresa,
                                ReservasOp_NM.aceptarPoliticas,
                                ReservasOp_NM.razonSocial,
                                ReservasOp_NM.ruc,
                                ReservasOp_NM.descripPaquete,
                                ReservasOp_NM.destinoPaquetes,
                                ReservasOp_NM.fechasPaquetes,
                                ReservasOp_NM.Proveedor,
                                //PlanSeguro = "",
                                //Plan = (ReservasOp_NM.PlanSeguro == null ? null : ReservasOp_NM.PlanSeguro.Plan),
                                //CantPasajeros = (ReservasOp_NM.PlanSeguro == null ? null : ReservasOp_NM.PlanSeguro.CantPasajeros),
                                //Destino = (ReservasOp_NM.PlanSeguro == null ? null : ReservasOp_NM.PlanSeguro.Destino),
                                //FechaSalida = (ReservasOp_NM.PlanSeguro == null ? null : ReservasOp_NM.PlanSeguro.FechaSalida),
                                //FechaRetorno = (ReservasOp_NM.PlanSeguro == null ? null : ReservasOp_NM.PlanSeguro.FechaRetorno),
                                //Edades = (ReservasOp_NM.PlanSeguro == null ? null : ReservasOp_NM.PlanSeguro.Edades),
                                //EmergenciaSeguro = "",                                
                                //Nombre = (ReservasOp_NM.EmergenciaSeguro == null ? null : ReservasOp_NM.EmergenciaSeguro.Nombre),
                                //Apellido = (ReservasOp_NM.EmergenciaSeguro == null ? null : ReservasOp_NM.EmergenciaSeguro.Apellido),
                                //Telefono = (ReservasOp_NM.EmergenciaSeguro == null ? null : ReservasOp_NM.EmergenciaSeguro.Telefono),
                                //Email = (ReservasOp_NM.EmergenciaSeguro == null ? null : ReservasOp_NM.EmergenciaSeguro.Email)
                                PlanSeguro = (ReservasOp_NM.PlanSeguro == null ? null : new
                                {
                                    ReservasOp_NM.PlanSeguro.Plan,
                                    ReservasOp_NM.PlanSeguro.CantPasajeros,
                                    ReservasOp_NM.PlanSeguro.Destino,
                                    ReservasOp_NM.PlanSeguro.FechaSalida,
                                    ReservasOp_NM.PlanSeguro.FechaRetorno,
                                    ReservasOp_NM.PlanSeguro.Edades
                                }),
                                EmergenciaSeguro = (ReservasOp_NM.EmergenciaSeguro == null ? null : new
                                {
                                    ReservasOp_NM.EmergenciaSeguro.Nombre,
                                    ReservasOp_NM.EmergenciaSeguro.Apellido,
                                    ReservasOp_NM.EmergenciaSeguro.Telefono,
                                    ReservasOp_NM.EmergenciaSeguro.Email
                                })
                            }
                        );
                    }



                    return objListReservasOp_NM;



                }
                else
                {
                    return null;
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ToSalesforceEntity(this DetalleItinerarioNM detItinerarioNM)
        {
            try
            {
                return new
                {
                    detItinerarioNM.idOportunidad_SF,
                    detItinerarioNM.id_Itinerario_SF,
                    detItinerarioNM.Identificador_NM,
                    detItinerarioNM.id_reserva,
                    detItinerarioNM.id_itinerario,
                    detItinerarioNM.LAerea,
                    detItinerarioNM.Origen,
                    detItinerarioNM.Salida,
                    detItinerarioNM.Destino,
                    detItinerarioNM.llegada,
                    detItinerarioNM.numeroVuelo,
                    detItinerarioNM.Clase,
                    detItinerarioNM.fareBasis,
                    detItinerarioNM.OperadoPor,
                    detItinerarioNM.esRetornoItinerario,
                    detItinerarioNM.accion_SF
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ToSalesforceEntity(this DetallePasajerosNM detallePasajerosNM)
        {
            try
            {
                return new
                {
                    detallePasajerosNM.idOportunidad_SF,
                    detallePasajerosNM.idPasajero_SF,
                    detallePasajerosNM.Identificador_NM,
                    detallePasajerosNM.id_reserva,
                    detallePasajerosNM.IdPasajero,
                    detallePasajerosNM.tipo,
                    detallePasajerosNM.pais,
                    detallePasajerosNM.apellidos,
                    detallePasajerosNM.nombre,
                    detallePasajerosNM.tipoDocumento,
                    detallePasajerosNM.nroDocumento,
                    detallePasajerosNM.fechaNacimiento,
                    detallePasajerosNM.Genero,
                    detallePasajerosNM.FOID,
                    detallePasajerosNM.FEE,
                    detallePasajerosNM.NombreReniec,
                    detallePasajerosNM.numHabitacionPaquete,
                    detallePasajerosNM.accion_SF
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ToSalesforceEntity(this DetalleHotelNM detalleHotelNM)
        {
            try
            {
                return new
                {
                    detalleHotelNM.idOportunidad_SF,
                    detalleHotelNM.idDetalleHotel_SF,
                    detalleHotelNM.Identificador_NM,
                    detalleHotelNM.hotel,
                    detalleHotelNM.direccion,
                    detalleHotelNM.destino,
                    detalleHotelNM.categoria,
                    detalleHotelNM.fechaIngreso,
                    detalleHotelNM.fechaSalida,
                    detalleHotelNM.fechaCancelacion,
                    detalleHotelNM.codigoReservaNemo,
                    detalleHotelNM.Proveedor,
                    detalleHotelNM.accion_SF,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ToSalesforceEntity(this SolicitudPagoNM solicitudPagoNM)
        {
            try
            {
                return new
                {
                    solicitudPagoNM.idOportunidad_SF,
                    solicitudPagoNM.Identificador_NM,
                    solicitudPagoNM.IdPedido,
                    solicitudPagoNM.pasarela,
                    solicitudPagoNM.fechaPedido,
                    solicitudPagoNM.estado1,
                    solicitudPagoNM.estado2,
                    solicitudPagoNM.resultado,
                    solicitudPagoNM.montoPagar,
                    solicitudPagoNM.rcGenerado,
                    solicitudPagoNM.lineaAereaValidadora,
                    solicitudPagoNM.formaPago,
                    solicitudPagoNM.entidadBancaria,
                    solicitudPagoNM.nroTarjeta,
                    solicitudPagoNM.titularTarjeta,
                    solicitudPagoNM.expiracion,
                    solicitudPagoNM.thReniec,
                    solicitudPagoNM.marcaTC,
                    solicitudPagoNM.tipoTC,
                    solicitudPagoNM.nivelTC,
                    solicitudPagoNM.paisTC,
                    esautenticada = solicitudPagoNM.EsAutenticada,
                    detalle = solicitudPagoNM.Detalle,
                    linkpago = solicitudPagoNM.LinkPago,
                    solicitudPagoNM.CodAutorTarj,
                    solicitudPagoNM.TipoImporte,
                    solicitudPagoNM.MontoImporte,
                    solicitudPagoNM.PlazoDePago, //
                    solicitudPagoNM.Error,
                    solicitudPagoNM.CodCanje,
                    solicitudPagoNM.Puntos,
                    solicitudPagoNM.ipCliente,
                    solicitudPagoNM.docTitular,
                    solicitudPagoNM.FEE,
                    solicitudPagoNM.GEM,
                    solicitudPagoNM.PEF,
                    solicitudPagoNM.accion_SF
                    ,
                    solicitudPagoNM.IdRegSolicitudPago_SF//
                    ,
                    solicitudPagoNM.fechaExpiracion//
                    ,
                    solicitudPagoNM.codigoPago//
                    ,
                    solicitudPagoNM.nroCuotas,
                    solicitudPagoNM.email
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ToSalesforceEntity(this InfoPagoNM infoPagoNM)
        {
            try
            {

                return new
                {
                    infoPagoNM.idOportunidad_SF,
                    infoPagoNM.Identificador_NM,
                    infoPagoNM.IdInformacionPago_SF,
                    infoPagoNM.ListPago_Boleto_Servicios,
                    //Aqui Lista ListPago_Boleto_Servicios

                    infoPagoNM.totalPagar,
                    infoPagoNM.montoDescuento,
                    infoPagoNM.textoDescuento,
                    infoPagoNM.promoWebCode,
                    infoPagoNM.totalFacturar,
                    infoPagoNM.feeAsumidoGeneralBoletos,
                    infoPagoNM.ListPagosDesglose_Paquete,
                    //Aqui Lista ListPagosDesglose_Paquete

                    infoPagoNM.precioTotalHabitacionesPaq,
                    infoPagoNM.gastosAdministrativosPaq,
                    infoPagoNM.tarjetaDeTurismo,
                    infoPagoNM.tarjetaDeAsistencia,
                    //oInfoPagoNM.PaqueteId = infoPagoNM.PaqueteId,
                    infoPagoNM.ListPagosServicio_Paquete,
                    //Aqui Lista ListPagosServicio_Paquete

                    infoPagoNM.precioTotalActividadesPaq,
                    infoPagoNM.textoDescuentoPaq,
                    infoPagoNM.montoDescuentoPaq,
                    infoPagoNM.totalFacturarPaq,
                    infoPagoNM.precioTotalPagarPaq,
                    infoPagoNM.cantDiasSeg,
                    infoPagoNM.precioUnitarioSeg,
                    infoPagoNM.MontoSeg,
                    infoPagoNM.DescuentoSeg,
                    infoPagoNM.MontoReservaSeg,
                    infoPagoNM.accion_SF,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ToSalesforceEntity(this ChatterNM canalComunicacionNM)
        {
            try
            {
                return new
                {
                    canalComunicacionNM.idOportunidad_SF,
                    canalComunicacionNM.IdRegPostCotSrv_SF,
                    canalComunicacionNM.idPostCotSrv,
                    canalComunicacionNM.Identificador_NM,
                    canalComunicacionNM.cabecera,
                    canalComunicacionNM.texto,
                    canalComunicacionNM.fecha,
                    canalComunicacionNM.accion_SF
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ToSalesforceEntity(this FileOportunidadNM fileOportunidadNM)
        {
            try
            {
                return new
                {
                    fileOportunidadNM.idOportunidad_SF,
                    fileOportunidadNM.Identificador_NM,
                    fileOportunidadNM.idCotSrv_SF,
                    fileOportunidadNM.numeroFile,
                    fileOportunidadNM.importe,
                    fileOportunidadNM.sucursal,
                    fileOportunidadNM.fecha,
                    fileOportunidadNM.accion_SF
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}