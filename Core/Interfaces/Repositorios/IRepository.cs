using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositorios
{
    public interface IRepository<T> where T : class
    {
        T? GetById(int id);
        IEnumerable<T> ObtenerTodos();
        void Agregar(T entity);
        void Actualizar(T entity);
        void Eliminar(T entity);

    }
}
