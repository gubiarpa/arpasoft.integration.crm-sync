using Expertia.Estructura.Models;
using Expertia.Estructura.Repository.Base;
using Expertia.Estructura.Repository.Behavior;
using Expertia.Estructura.Utils;
using System;

namespace Expertia.Estructura.Repository.InterAgencias
{
    public class Contacto_IA_Repository : OracleBase<Contacto>, ICrud<Contacto>
    {
        public Contacto_IA_Repository() : base(ConnectionKeys.IAConnKey)
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