using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.RemoteInterfaces;
using TiendaServicios.Api.CarritoCompra.RemoteModel;

namespace TiendaServicios.Api.CarritoCompra.RemoteServices
{
    public class LibroService : ILibrosService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<LibroService> _logger;
        public LibroService(IHttpClientFactory httpClient, ILogger<LibroService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

        }

        public async Task<(bool resultado, LibroRemote libro, string errorMessage)> GetLibro(Guid libroId)
        {
            try
            {
                var clienteHttp = _httpClient.CreateClient("Libros");
                var response = await clienteHttp.GetAsync($"api/LibreriaMaterial/{libroId}");

                if (response.IsSuccessStatusCode)
                {
                    var contenidoLibro = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var resultado = JsonSerializer.Deserialize<LibroRemote>(contenidoLibro, options);
                    return (true, resultado, "");
                }
                return (false, null, response.ReasonPhrase.ToString());

            }
            catch (Exception ex)
            {
                _logger.LogError($"GetLibro Error :{ex}. Url = api/LibroMaterial/{libroId}");
                return (false, null, ex.StackTrace.ToString());
            }
        }
    }
}
