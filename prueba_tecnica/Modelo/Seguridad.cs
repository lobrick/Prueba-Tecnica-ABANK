namespace prueba_tecnica.Modelo
{
    public class Seguridad
    {
        public int? id { get; set; }
        public string? nombres { get; set; }
        public string? apellidos { get; set; }
        public DateOnly? fechanacimiento { get; set; }
        public string? direccion { get; set; }
        public string? password { get; set; }
        public string? telefono { get; set; }
        public string? email { get; set; }
        public DateOnly? fechacreacion { get; set; }
        public DateOnly? fechamodificacion { get; set; }
    }
}
