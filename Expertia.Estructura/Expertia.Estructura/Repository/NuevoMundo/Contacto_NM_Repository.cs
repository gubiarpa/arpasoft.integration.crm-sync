using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Expertia.Estructura.Repository.NuevoMundo
{
    public class Contacto_NM_Repository : OracleBase<Contacto>, ICrud<Contacto>
    {
        public Contacto_NM_Repository() : base(ConnectionKeys.NMConnKey)
        {
        }

        public Operation Create(Contacto entity)
        {
            throw new NotImplementedException();
        }

        public Operation Delete(Contacto entity)
        {
            throw new NotImplementedException();
        }

        public Operation Read(Contacto entity)
        {
            throw new NotImplementedException();
        }

        public Operation Update(Contacto entity)
        {
            throw new NotImplementedException();
        }
    }
}