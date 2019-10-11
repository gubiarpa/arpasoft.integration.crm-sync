using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.DestinosMundiales
{
    public class Cotizacion_DM_Repository : OracleBase<Cotizacion>, ICrud<Cotizacion>, ISameSPName<Cotizacion>
    {
        #region Constructor
        public Cotizacion_DM_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.DestinosMundiales) : base(ConnectionKeys.DMConnKey, unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation Create(Cotizacion entity)
        {
            return ExecuteOperation(entity, StoredProcedureName.DM_Create_Cotizacion, entity.Auditoria.CreateUser.Descripcion);
        }

        public Operation Update(Cotizacion entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Auxiliar
        public Operation ExecuteOperation(Cotizacion entity, string SPName, string userName)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}