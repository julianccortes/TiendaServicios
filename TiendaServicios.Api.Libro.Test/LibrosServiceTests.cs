using System;
using System.Collections.Generic;
using System.Text;
using TiendaServicios.Api.Libro.Persistencia;
using Xunit;
using Moq;
using AutoMapper;
using TiendaServicios.Api.Libro.Aplicacion;
using GenFu;
using TiendaServicios.Api.Libro.Modelo;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TiendaServicios.Api.Libro.Test
{
    public class LibrosServiceTests
    {
        private IEnumerable<LibreriaMaterial> GetMockLibroData()
        {

            A.Configure<LibreriaMaterial>()
                .Fill(x => x.Titulo).AsArticleTitle()
                .Fill(x => x.LibreriaMaterialId, () => { return Guid.NewGuid(); });

            var lista = A.ListOf<LibreriaMaterial>(30);
            lista[0].LibreriaMaterialId = Guid.Empty;
            return lista;

        }

        private Mock<ContextoLibreria> CrearContexto()
        {

            var dataPrueba = GetMockLibroData().AsQueryable();
            var dbSet = new Mock<DbSet<LibreriaMaterial>>();
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(dataPrueba.Provider);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Expression).Returns(dataPrueba.Expression);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.ElementType).Returns(dataPrueba.ElementType);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator());

            dbSet.As<IAsyncEnumerable<LibreriaMaterial>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
                .Returns(new AsyncEnumerator<LibreriaMaterial>(dataPrueba.GetEnumerator()));

            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(new AsyncQueryProvider<LibreriaMaterial>(dataPrueba.Provider));

            var contexto = new Mock<ContextoLibreria>();
            contexto.Setup(x => x.LibreriaMaterial).Returns(dbSet.Object);

            return contexto;
        }

        [Fact]
        public async void GetLibrosById()
        {
            var mockContext = CrearContexto();
            var mapperConfig = new MapperConfiguration(cgf =>
            {
                cgf.AddProfile(new MappingTest());

            });
            var mockMapper = mapperConfig.CreateMapper();

            ConsultaFiltro.LibroUnico request = new ConsultaFiltro.LibroUnico() { LibroId = Guid.Empty };
            ConsultaFiltro.Manejador manejador = new ConsultaFiltro.Manejador(mockContext.Object, mockMapper);

            var libro = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.NotNull(libro);
            Assert.True(libro.LibreriaMaterialId == Guid.Empty);

        }

        [Fact]
        public async void GetLibros()
        {
            System.Diagnostics.Debugger.Launch();
            var mockContext = CrearContexto();
            var mapperConfig = new MapperConfiguration(cgf =>
            {
                cgf.AddProfile(new MappingTest());

            });
            var mockMapper = mapperConfig.CreateMapper();

            Consulta.Manejador manejador = new Consulta.Manejador(mockContext.Object, mockMapper);

            Consulta.Ejecuta request = new Consulta.Ejecuta();
            var lista = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(lista.Any());

        }

        [Fact]
        public async void GuardarLibro()
        {

            var options = new DbContextOptionsBuilder<ContextoLibreria>()
                .UseInMemoryDatabase(databaseName: "BaseDatosLibro")
                .Options;

            var contexto = new ContextoLibreria(options);
            Nuevo.Ejecuta request = new Nuevo.Ejecuta() { Titulo = "Libro prueba", FechaPublicacion = DateTime.Now, AutorLibro = Guid.Empty };

            Nuevo.Manejador manejador = new Nuevo.Manejador(contexto);
            var resultado = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(resultado != null);
        }

    }
}
