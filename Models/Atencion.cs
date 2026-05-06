namespace H_Emergencias.Models
{
    public class Atencion
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public DateTime Fecha { get; set; }
        public string Medico { get; set; }
        public string CodigoPaciente { get; set; }
        public bool Estado { get; set; }
    }
}
