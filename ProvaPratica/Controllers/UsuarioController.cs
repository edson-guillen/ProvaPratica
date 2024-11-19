using Microsoft.AspNetCore.Mvc;
using ProvaPratica.Controllers;
using ProvaPratica.Domain.Models;
using ProvaPratica.Service.Interfaces;

namespace ProvaPratica.Controllers
{
    [ApiController]
    public class UsuarioController : BaseController<UsuarioModel>
    {
        public UsuarioController(IUsuarioService service, ILogger<UsuarioController> loger) : base(service, loger)
        {
        }
    }
}