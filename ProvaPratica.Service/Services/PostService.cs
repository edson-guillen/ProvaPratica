using Microsoft.Extensions.Configuration;
using ProvaPratica.Domain.Models;
using ProvaPratica.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ProvaPratica.Service.Interfaces;

namespace ProvaPratica.Service.Services
{
    public class PostService : BaseService<PostModel>, IPostService
    {
        public PostService(AppDbContext db) : base(db)
        {
        }
    }
}
