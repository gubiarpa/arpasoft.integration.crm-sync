using Expertia.Estructura.Models;
using Expertia.Estructura.Models.Retail;
using Expertia.Estructura.Utils;
using System;

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

        public static object ToSalesforceEntity(this CuentaNM cuentaNM)
        {
            try
            {
                return new
                {
                    cuentaNM.nombreCli,
                    cuentaNM.apePatCli,
                    cuentaNM.apeMatCli,
                    cuentaNM.idCuenta_Sf,
                    cuentaNM.eMailCli,
                    cuentaNM.enviarPromociones,
                    cuentaNM.tipoTelefono1,
                    cuentaNM.codPais1,
                    cuentaNM.numero1,
                    cuentaNM.tipoTelefono2,
                    cuentaNM.codPais2,
                    cuentaNM.numero2,
                    cuentaNM.tipoTelefono3,
                    cuentaNM.codPais3,
                    cuentaNM.numero3,
                    cuentaNM.direccion,
                    cuentaNM.razonSocial,
                    cuentaNM.aceptarPoliticas,
                    cuentaNM.ruc,
                    cuentaNM.idUsuarioSrv_Sf,
                    cuentaNM.accion_Sf
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
                    oportunidadNM.fechaRegistro,
                    oportunidadNM.idCanalVenta,
                    oportunidadNM.metaBuscador,
                    oportunidadNM.cajaVuelos,
                    oportunidadNM.cajaHotel,
                    oportunidadNM.cajaPaquetes,
                    oportunidadNM.cajaServicios,
                    oportunidadNM.modoIngreso,
                    oportunidadNM.ordenAtencion,
                    oportunidadNM.evento,
                    oportunidadNM.estado,
                    oportunidadNM.idCotSRV,
                    oportunidadNM.idUsuarioSrv,
                    oportunidadNM.codReserva,
                    oportunidadNM.fechaCreación,
                    oportunidadNM.estadoVenta,
                    oportunidadNM.codigoAerolinea,
                    oportunidadNM.tipo,
                    oportunidadNM.ruc,
                    oportunidadNM.pcc_OfficeID,
                    oportunidadNM.counterAsignado,
                    oportunidadNM.iata,
                    oportunidadNM.descripPaquete,
                    oportunidadNM.destinoPaquetes,
                    oportunidadNM.fechasPaquetes,
                    oportunidadNM.empresaCliente,
                    oportunidadNM.nombreCliente,
                    oportunidadNM.apeliidosCliente,
                    oportunidadNM.idLoginWeb,
                    oportunidadNM.telefonoCliente,
                    oportunidadNM.accion_SF
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ToSalesforceEntity(this DetalleItinerarioNM detalleItinerarioNM)
        {
            try
            {
                return new
                {
                    detalleItinerarioNM.idOportunidad_SF,
                    detalleItinerarioNM.lAerea,
                    detalleItinerarioNM.origen,
                    detalleItinerarioNM.salida,
                    detalleItinerarioNM.destino,
                    detalleItinerarioNM.llegada,
                    detalleItinerarioNM.numeroVuelo,
                    detalleItinerarioNM.clase,
                    detalleItinerarioNM.fareBasis,
                    detalleItinerarioNM.operadoPor,
                    detalleItinerarioNM.accion_SF
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
                    detallePasajerosNM.tipo,
                    detallePasajerosNM.pais,
                    detallePasajerosNM.apellidos,
                    detallePasajerosNM.nombre,
                    detallePasajerosNM.tipoDocumento,
                    detallePasajerosNM.nroDocumento,
                    detallePasajerosNM.fechaNacimiento,
                    detallePasajerosNM.foid,
                    detallePasajerosNM.fee,
                    detallePasajerosNM.NombreReniec,
                    detallePasajerosNM.numHabitacionPaquete,
                    detallePasajerosNM.idPasajero_SF
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
                    detalleHotelNM.idDetalleHotel_SF
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
                    solicitudPagoNM.ipCliente,
                    solicitudPagoNM.docTitular,
                    solicitudPagoNM.mensaje,
                    solicitudPagoNM.mensajeError,
                    solicitudPagoNM.accion_SF
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object ToSalesforceEntity(this InformacionPagoNM informacionPagoNM)
        {
            try
            {
                return new
                {
                    informacionPagoNM.idOportunidad_SF,
                    informacionPagoNM.tipoServicio,
                    informacionPagoNM.tipoPasajero,
                    informacionPagoNM.totalBoleto,
                    informacionPagoNM.tarifaNeto,
                    informacionPagoNM.impuestos,
                    informacionPagoNM.cargos,
                    informacionPagoNM.nombreHotel,
                    informacionPagoNM.totalPagar,
                    informacionPagoNM.numHabitacionPaquete,
                    informacionPagoNM.cantidadPasajeroPaq,
                    informacionPagoNM.precioUnitarioPaq,
                    informacionPagoNM.totalUnitarioPaq,
                    informacionPagoNM.precioTotalPorHabitacionPaq,
                    informacionPagoNM.precioTotalHabitacionesPaq,
                    informacionPagoNM.gastosAdministrativosPaq,
                    informacionPagoNM.precioTotalPagarPaq,
                    informacionPagoNM.accion_SF
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
                    canalComunicacionNM.idCotSrv_SF,
                    canalComunicacionNM.texto,
                    canalComunicacionNM.accion_SF
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}