using ProvaPratica.Domain.Models;
using System;
using System.Collections.Generic;

namespace ProvaPratica.Domain.DTOs
{
    public class ResultBase<TModel> where TModel : BaseModel
    {
        public bool Success { get; set; } = true;

        public TModel Data { get; set; }

        public List<string> Errors { get; set; }

        public string Message { get; set; }
    }
}
