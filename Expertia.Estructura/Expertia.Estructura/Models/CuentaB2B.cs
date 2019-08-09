namespace Expertia.Estructura.Models
{
    /// <summary>
    /// Contacto B2B
    /// </summary>
    public class CuentaB2B : Cuenta
    {
        #region Properties
        /// <summary>
        /// Razón Social
        /// </summary>
        public string RazonSocial { get; set; } // Razón Social
        /// <summary>
        /// Alias
        /// </summary>
        public string Alias { get; set; } // Alias
        /// <summary>
        /// Monto de Línea de Crédito
        /// </summary>
        public decimal? MontoLineaCredito { get; set; } // Monto de Línea de Crédito        
        #endregion

        #region ForeignKey
        /// <summary>
        /// Condición de Pago
        /// </summary>
        public CondicionPago CondicionPago { get; set; } // Condición de Pago
        /// <summary>
        /// Tipo de Moneda
        /// </summary>
        public TipoMoneda TipoMonedaDeLineaCredito { get; set; } // Tipo de moneda de Linea de credito
        #endregion
    }
}