using Expertia.Estructura.Models.Auxiliar;
using Expertia.Estructura.Models.Foreign;
using System.Collections.Generic;

namespace Expertia.Estructura.Models
{
    public class CuentaB2B : Cuenta
    {
        #region Properties
        // DkAgencia
        public string DkAgencia_DM { get; set; }
        public string DkAgencia_IA { get; set; }
        // LimiteCredito
        public float? LimiteCredito_DM { get; set; }
        public float? LimiteCredito_IA { get; set; }
        // Asesor
        public string Asesor_DM { get; set; }
        public string Asesor_IA { get; set; }

        public string RazonSocial { get; set; } // Razón Social
        public string Alias { get; set; } // Alias
        public decimal? MontoLineaCredito { get; set; } // Monto de Línea de Crédito        
        #endregion

        #region ForeignKey
        public TipoMoneda TipoMonedaDeLineaCredito { get; set; } // Tipo de moneda de Linea de credito
        public CategoriaValor CategoriaValor { get; set; }
        public CategoriaPerfilActitudTecnologica CategoriaPerfilActitudTecnologica { get; set; }
        public CategoriaPerfilFidelidad CategoriaPerfilFidelidad { get; set; }
        public Incentivo Incentivo { get; set; }
        public MotivoEstado MotivoEstado { get; set; }
        #endregion

        #region MultipleKey
        public IEnumerable<CondicionPago> CondicionesPago { get; set; } // Condición de Pago
        public Herramientas Herramientas { get; set; }
        public GDS GDS { get; set; }
        public GrupoComunicacion GruposComunicacion { get; set; } // Grupos de Comunicación (ejm. Skype)
        #endregion
    }
}