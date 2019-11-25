using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Repository.InterAgencias;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.AppWebs
{
    public class Oportunidad_AW_Repository : OracleBase<Oportunidad>, IOportunidadRepository
    {
        public Oportunidad_AW_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.AppWebs) : base(unidadNegocio.ToConnectionKey(), unidadNegocio)
        {
        }

        public Operation GetOportunidades()
        {
            return (new Oportunidad_IA_Repository(UnidadNegocioKeys.AppWebs).GetOportunidades());
        }

        public Operation Update(Oportunidad oportunidad)
        {
            throw new NotImplementedException();
        }
    }
}