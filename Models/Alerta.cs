namespace H_Emergencias.Models
{
    public class Alerta
    {
        public int Id { get; set; }
        public string Codigo { get; set; }

        public string CodigoPaciente { get; set; }
        public string Tipo { get; set; }
        // Emergencia, Ambulancia, Hospitalizacion, ServicioCritico

        public string Mensaje { get; set; }

        public DateTime Fecha { get; set; }

        public bool Estado { get; set; }
    }
}