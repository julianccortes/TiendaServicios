using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.RemoteModel;

namespace TiendaServicios.Api.CarritoCompra.RemoteInterfaces
{
    public interface ILibrosService
    {
        public Task<(bool resultado, LibroRemote libro, string errorMessage)> GetLibro(Guid libroId);
    }
}
