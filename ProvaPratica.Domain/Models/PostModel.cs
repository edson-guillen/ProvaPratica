using System;

namespace ProvaPratica.Domain.Models
{
    public class PostModel : BaseModel
    {
        public string Conteudo { get; set; }
        public int IdUsuario { get; set; }
        public virtual UsuarioModel Usuario { get; set; }
    }
}