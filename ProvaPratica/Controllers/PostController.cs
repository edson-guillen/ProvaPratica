using Microsoft.AspNetCore.Mvc;
using ProvaPratica.Controllers;
using ProvaPratica.Domain.Models;
using ProvaPratica.Service.Interfaces;

namespace ProvaPratica.Controllers
{
    [ApiController]
    public class PostController : BaseController<PostModel>
    {
        public PostController(IPostService service, ILogger<PostController> loger) : base(service, loger)
        {
        }
    }
}