using Expertia.Estructura.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expertia.Estructura.Repository.Behavior
{
    public interface ICrud<T>
    {
        Operation Create(T entity);
        Operation Read(T entity);
        Operation Update(T entity);
        Operation Delete(T entity);
    }
}
