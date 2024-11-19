using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ProvaPratica.Domain.DTOs;
using ProvaPratica.Domain.Models;
using ProvaPratica.Service.Interfaces;
using System.Net;

namespace ProvaPratica.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<TModel> : Controller where TModel : BaseModel
    {
        public Func<IActionResult> BeforeInsert;
        public Func<IActionResult> AfterInsert;

        protected readonly IBaseService<TModel> _service;
        protected readonly ILogger _logger;

        protected TModel _model;

        public BaseController(IBaseService<TModel> service, ILogger loger)
        {
            _service = service;
            _logger = loger;
        }

        [HttpGet]
        public virtual IActionResult GetAll()
        {
            return Ok(_service.Get());
        }

        [HttpGet("{id}")]
        public virtual IActionResult GetById(int id)
        {
            return Ok(_service.Get(id).FirstOrDefault());
        }

        [HttpGet("odata")]
        public virtual IActionResult Get(ODataQueryOptions<TModel> queryOptions)
        {
            var query = _service.Get();

            var queryfilter = queryOptions.Filter?.ApplyTo(query, new ODataQuerySettings()) ?? query;

            return Ok(new
            {
                _count = queryOptions.Count?.GetEntityCount(queryfilter),
                value = queryOptions.ApplyTo(query)
            });
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TModel model)
        {
            _model = model;

            var result = new ResultBase<TModel>
            {
                Data = model
            };

            if (!ModelState.IsValid)
            {
                result.Success = false;
                result.Message = "No foi possvel incluir o registro";
                result.Errors = GetModelStateErrors();

                return Ok(result);
            }

            var res = BeforeInsert?.Invoke();

            if (res != null)
                return res;


            if (await _service.InsertAsync(model) > 0)
            {
                AfterInsert?.Invoke();

                result.Message = "Registro criado com sucesso";

                return Ok(result);
            }

            result.Success = false;
            result.Message = "No foi possvel incluir o registro";

            AfterInsert?.Invoke();

            return Ok(result);
        }

        /*protected async Task<int> DoPost(TModel model)
        {
            _repository.Db.Add<TModel>(model);

            return await _repository.Db.SaveChangesAsync();
        }*/

        [HttpPut]
        public virtual async Task<IActionResult> Put(TModel model)
        {
            _model = model;

            var result = new ResultBase<TModel>
            {
                Data = model
            };

            if (!ModelState.IsValid)
            {
                result.Message = "No foi possvel alterar o registro.";
                result.Errors = GetModelStateErrors();

                return Ok(result);
            }


            if (await _service.UpdateAsync(model) > 0)
            {
                result.Message = "Registro alterado com sucesso";
                return Ok(result);
            }

            result.Message = "No foi possvel alterar o registro";
            return Ok(result);

        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var result = new ResultBase<TModel>();

            var resultado = await _service.DeleteAsync(id);

            if (resultado > 0)
            {
                result.Message = "Registro excludo com sucesso";
            }

            if (resultado == 0)
            {
                result.Message = "No foi possvel excluir o registro";
                result.Success = false;
            }

            if (resultado == -1)
            {
                result.Success = false;
                result.Message = "Registro no encontrado";
            }

            if (resultado == -2)
            {
                result.Success = false;
                result.Message = "No foi possvel excluir o registro no momento";
            }

            return Ok(result);
        }

        [NonAction]
        public IActionResult CreateResult(HttpStatusCode statusCode, string message,
            object data = null, object errors = null)
        {
            return StatusCode((int)statusCode, new { statusCode, message, data, errors });
        }

        [NonAction]
        public List<string> GetModelStateErrors()
        {
            //var errors = new Dictionary<string, string[]>(); //List<KeyValuePair<string, string>>();
            var errors = new List<string>();

            foreach (var key in ModelState.Keys)
            {
                errors.AddRange(ModelState[key].Errors.ToList().Select(t => t.ErrorMessage).ToArray());
                /*foreach (var m in ModelState[key].Errors.ToList().Select(t => t.ErrorMessage))
                {
                    errors.Add(string, string > (key, m));
                }*/

            }

            return errors;

        }

    }
}
