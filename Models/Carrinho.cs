using System;
using System.ComponentModel;

namespace Models
{
    public class Carrinho : BaseModel
    {
        public long? ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public DateTime Data { get; set; }

        [DisplayName("Exame")]
        public long? Produto1Id { get; set; }

        [DisplayName("Exame")]
        public long? Produto2Id { get; set; }

        [DisplayName("UsuExameário")]
        public long? Produto3Id { get; set; }

        [DisplayName("Exame")]
        public long? Produto4Id { get; set; }

        [DisplayName("Usuário")]
        public string IdUser { get; set; }

    }
}
