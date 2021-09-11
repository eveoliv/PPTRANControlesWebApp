namespace PPTRANControlesWebApp.Models.Administracao
{
    public class RepasseViewModel
    {
        public long? RepasseId { get; set; }
        public long? ClinicaId { get; set; }
        public string ClinicaAlias { get; set; }
        public string Profissional { get; set; }
        public decimal Valor { get; set; }
    }
}
