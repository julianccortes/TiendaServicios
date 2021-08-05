using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Modelo;
using TiendaServicios.Api.CarritoCompra.Persistencia;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public DateTime FechaCreacionSesion { get; set; }
            public List<string> ProductoLista { get; set; }


        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CarritoContexto _contexto;
            private readonly IMapper _mapper;

            public Manejador(CarritoContexto contexto, IMapper mapper) {

                _contexto = contexto;
                _mapper = mapper;   

            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion = new CarritoSesion
                {
                    FechaCreacion= request.FechaCreacionSesion
                };

                _contexto.CarritoSesion.Add(carritoSesion);
                var value = await _contexto.SaveChangesAsync();
                if (value == 0) {
                    throw new Exception("Error en inserccion de carrito de compra");
                }

                int idSesion= carritoSesion.CarritoSesionId;

                foreach (string item in request.ProductoLista) {

                    var carritoSesionDetalle = new CarritoSesionDetalle {
                        CarritoSesionId = idSesion,
                        ProductoSeleccionado = item,
                        FechaCreacion = DateTime.Now
                    };

                     _contexto.CarritoSesionDetalle.Add(carritoSesionDetalle);                 

                }

                value = await _contexto.SaveChangesAsync();
                if (value > 0)                
                    return Unit.Value;

                throw new Exception("Error en inserccion detalle de carrito de compra");

            }
        }
    }
}
