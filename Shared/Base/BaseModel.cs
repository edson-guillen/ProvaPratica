using System;

namespace ProvaPratica.Domain.Models
{
    public class BaseModel
    {
        public int Id { get; set; }

        public DateTime CriadoEm { get; set; }

        public DateTime AtualizadoEm { get; set; }
    }
}
