    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaServicios.Api.Autor.Aplicacion;
using TiendaServicios.Api.Autor.Aplicacion.DTO;
using TiendaServicios.Api.Autor.Modelo;

namespace TiendaServicios.Api.Autor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly IMediator _iMediator;

        public AutorController(IMediator iMediator) {
            _iMediator = iMediator;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta parametros) {
            return await _iMediator.Send(parametros);
        }

        [HttpGet]
        public async Task<ActionResult<List<AutorDto>>> GetAutores()
        {
            return await _iMediator.Send(new Consulta.ListaAutor());
        }

        [HttpGet("{guid}")]
        public async Task<ActionResult<AutorDto>> GetAutor(string guid)
        {
            return await _iMediator.Send(new ConsultaFiltro.AutorUnico() { AutorGuid= guid});
        }

    }
}
