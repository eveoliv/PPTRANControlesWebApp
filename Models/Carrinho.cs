using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Carrinho : BaseModel
    {
        public long? ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [DisplayName("Data")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime Data { get; set; }

        [DisplayName("Exame")]
        public long? Produto1Id { get; set; }

        [DisplayName("Exame")]
        public long? Produto2Id { get; set; }

        [DisplayName("Exame")]
        public long? Produto3Id { get; set; }

        [DisplayName("Exame")]
        public long? Produto4Id { get; set; }

        [DisplayName("Usuário")]
        public string IdUser { get; set; }

    }
}
