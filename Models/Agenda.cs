using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Agenda
    {
        public long? AgendaId { get; set; }
        public DateTime Data { get; set; }
        public long? ClienteId { get; set; }
        public long? PsicologoId { get; set; }
        public long? MedicoId { get; set; }

    }
}
