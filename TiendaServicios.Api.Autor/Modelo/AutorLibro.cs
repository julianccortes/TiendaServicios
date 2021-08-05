namespace TiendaServicios.Api.Autor.Modelo
{
    using System;
    using System.Collections.Generic;
    public class AutorLibro
    {
        public int AutorLibroId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public ICollection<GradoAcademico> ListaGradoAcademico { get; set; }
        public string AutoLibroGuid { get; set; }

        internal class AutorDto
        {
        }
    }
}
