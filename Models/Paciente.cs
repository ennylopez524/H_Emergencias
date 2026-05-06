namespace H_Emergencias.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public bool Estado { get; set; }
    }
}
