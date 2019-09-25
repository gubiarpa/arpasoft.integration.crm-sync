using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Behavior;
using Expertia.Estructura.Models.Foreign;
using System;
using System.Collections.Generic;

namespace Expertia.Estructura.Models
{
    public class Cuenta : IUnique, ISalesForce, IAuditable, IUnidadNegocio
    {
        #region Properties
        public string ID { get; set; }
        public string IdSalesForce { get; set; }
        public DateTime? FechaNacimOrAniv { get; set; }
        public string LogoFoto { get; set; }
        public bool RecibirInformacion { get; set; }
        public DateTime? FechaIniRelacionComercial { get; set; }
        public string Comentarios { get; set; }
        #endregion

        #region ForeignKey
        public UnidadNegocio UnidadNegocio { get; set; }
        public TipoPersona TipoPersona { get; set; }
        public PuntoContacto PuntoContacto { get; set; }
        public NivelImportancia NivelImportancia { get; set; }
        public TipoCuenta TipoCuenta { get; set; }
        public Estado Estado { get; set; }
        public Pais PaisProcedencia { get; set; }
        #endregion

        #region MultipleKey
        public IEnumerable<Documento> Documentos { get; set; }
        public IEnumerable<Direccion> Direcciones { get; set; }
        public IEnumerable<Telefono> Telefonos { get; set; }
        public IEnumerable<Sitio> Sitios { get; set; }
        public IEnumerable<Correo> Correos { get; set; }
        public IEnumerable<Participante> Participantes { get; set; }
        public InteresProdActiv InteresesProdActiv { get; set; }
        public CanalInformacion CanalesRecibirInfo { get; set; }
        public IEnumerable<SimpleDesc> Branches { get; set; }
        public IEnumerable<SimpleDesc> IdiomasComunicCliente { get; set; }
        #endregion

        #region Auditoria
        public Auditoria Auditoria { get; set; }
        #endregion
    }
}