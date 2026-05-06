namespace H_Emergencias.Models
{
    public class MisionAmbulancia
    {
        public int Id { get; set; }
        public string Codigo { get; set; }

        public string CodigoAtencion { get; set; } // relación con Emergencias
        public string NivelGravedad { get; set; }

        public DateTime FechaSalida { get; set; }
        public DateTime? ETA { get; set; } // tiempo estimado llegada

        public bool Estado { get; set; }
    }
}