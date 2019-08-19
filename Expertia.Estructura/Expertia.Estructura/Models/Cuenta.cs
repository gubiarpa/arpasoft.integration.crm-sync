using Expertia.Estructura.Models.Behavior;
using System;
using System.Collections.Generic;

namespace Expertia.Estructura.Models
{
    public class Cuenta : UniqueBase, ISalesForce
    {
        #region Properties
        public string IdSalesForce { get; set; }
        public DateTime? FechaNacimOrAniv { get; set; }
        public string LogoFoto { get; set; }
        public bool RecibirInformacion { get; set; }
        public DateTime? FechaIniRelacionComercial { get; set; }
        public string Comentarios { get; set; }
        #endregion

        #region ForeignKey
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
        public IEnumerable<InteresProdActiv> InteresesProdActiv { get; set; }
        public IEnumerable<CanalInformacion> CanalesRecibirInfo { get; set; }
        public IEnumerable<Branch> Branches { get; set; }
        public IEnumerable<IdiomaComunicCliente> IdiomasComunicCliente { get; set; }
        #endregion
    }
}