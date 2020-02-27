using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Collections.Generic;

namespace Expertia.Estructura.Repository.Retail
{
    public class FileRetailRepository : OracleBase<Pedido>
    {
        #region Constructor
        public FileRetailRepository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region Metodos Publicos        
        #endregion

        #region Auxiliares       
        #endregion
    }
}