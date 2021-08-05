using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public string Titulo { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public Guid? AutorLibro { get; set; }
        }
        public class EjecutaValidation : AbstractValidator<Ejecuta>
        {

            public EjecutaValidation()
            {
                RuleFor(x => x.Titulo).NotEmpty().WithMessage("Debe especificar el titulo del libro");
                RuleFor(x => x.FechaPublicacion).NotEmpty().WithMessage("Debe especificar la fecha de publicacion del libro");
                RuleFor(x => x.AutorLibro).NotEmpty().WithMessage("Debe especificar el id del autor del libro");

            }

        }
        public class Manejador : IRequestHandler<Ejecuta>
        {
            public readonly ContextoLibreria _contexto;

            public Manejador(ContextoLibreria contexto)
            {
                _contexto = contexto;
            }          
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var libro = new LibreriaMaterial()
                {
                    Titulo = request.Titulo,
                    FechaPublicacion = request.FechaPublicacion,
                    AutorLibro = request.AutorLibro,
                    LibreriaMaterialId = Guid.NewGuid()
                };
                _contexto.LibreriaMaterial.Add(libro);
                var valor = await _contexto.SaveChangesAsync();
                if (valor > 0)
                    return Unit.Value;

                throw new Exception("No se pudo insertar el libro");
            }
        }

    }
}
