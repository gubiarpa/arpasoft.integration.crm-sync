using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.InterAgencias
{
    public class Subcodigo_IA_Repository : OracleBase<Subcodigo>, ICrud<Subcodigo>
    {
        public Subcodigo_IA_Repository(UnidadNegocioKeys? unidadNegocio = UnidadNegocioKeys.InterAgencias) : base(ConnectionKeys.IAConnKey, unidadNegocio)
        {
        }

        public Operation Asociate(Subcodigo entity)
        {
            throw new NotImplementedException();
        }

        public Operation Create(Subcodigo entity)
        {
            throw new NotImplementedException();
        }

        public Operation Generate(Subcodigo entity)
        {
            throw new NotImplementedException();
        }

        public Operation Read(Subcodigo entity)
        {
            throw new NotImplementedException();
        }

        public Operation Update(Subcodigo entity)
        {
            throw new NotImplementedException();
        }
    }
}