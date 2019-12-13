using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.Utils;
using System;

namespace Expertia.Estructura.Repository.DestinosMundiales
{
    public class Subcodigo_DM_Repository : OracleBase<Subcodigo>, ISubcodigoRepository
    {
        #region Constructor
        public Subcodigo_DM_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.DestinosMundiales) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }
        #endregion

        #region PublicMethods
        public Operation Create(Subcodigo entity)
        {
            return new Subcodigo_IA_Repository(UnidadNegocioKeys.DestinosMundiales).Create(entity);
        }

        public Operation Read()
        {
            return new Subcodigo_IA_Repository(UnidadNegocioKeys.DestinosMundiales).Read();
        }

        public Operation Update(Subcodigo subcodigo)
        {
            return new Subcodigo_IA_Repository(UnidadNegocioKeys.DestinosMundiales).Update(subcodigo);
        }
        #endregion
    }
}