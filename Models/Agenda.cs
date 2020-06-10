using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Agenda
    {
        public long? Agenda_Id { get; set; }
        public DateTime Data { get; set; }
        public long? Cliente_id { get; set; }
        public long? Psicologo_Id { get; set; }
        public long? Medico_Id { get; set; }
    }
}
