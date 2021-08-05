using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaServicios.Api.Libro.Aplicacion;

namespace TiendaServicios.Api.Libro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibreriaMaterialController : ControllerBase
    {
        private readonly IMediator _iMediator;

        public LibreriaMaterialController(IMediator iMediator)
        {
            _iMediator = iMediator;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta parametros)
        {
            return await _iMediator.Send(parametros);
        }

        [HttpGet]
        public async Task<ActionResult<List<LibroMaterialDto>>> GetAllLibros()
        {
            return await _iMediator.Send(new Consulta.Ejecuta());
        }

        [HttpGet("{guid}")]
        public async Task<ActionResult<LibroMaterialDto>> GetLibro(Guid guid)
        {
            return await _iMediator.Send(new ConsultaFiltro.LibroUnico() { LibroId = guid });
        }
    }
}
