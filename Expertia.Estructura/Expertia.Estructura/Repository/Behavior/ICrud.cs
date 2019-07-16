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
        OperationResult Create(T entity);
        OperationResult Read(T entity);
        OperationResult Update(T entity);
        OperationResult Delete(T entity);
    }
}
