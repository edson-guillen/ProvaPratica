using System;
using System.Collections.Generic;

namespace ProvaPratica.Domain.Models
{
    public class UsuarioModel : BaseModel
    {
        public string Nome { get; set; }
        public int Idade { get; set; }
        public virtual IList<PostModel> Posts { get; set; }
    }
}
