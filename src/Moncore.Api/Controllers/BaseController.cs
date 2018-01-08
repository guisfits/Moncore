using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moncore.Domain.Entities.Base;
using Moncore.Domain.Interfaces;

namespace Moncore.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public abstract class BaseController<TEntity> : Controller where TEntity : BaseEntity
    {
        protected readonly IRepository<TEntity> _repository;

        protected BaseController(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public virtual async Task<IEnumerable<TEntity>> Get()
        {
            return await _repository.Get();
        }

        [HttpGet("{id}")]
        public virtual async Task<TEntity> Get(int id)
        {
            return await _repository.Get(id);
        }

        [HttpPost]
        public virtual async void Post([FromBody]TEntity entity)
        {
            await _repository.Add(entity);
        }

        [HttpPut]
        public virtual async void Put(int id, [FromBody]TEntity entity)
        {
            await _repository.Update(id, entity);
        }

        [HttpDelete]
        public virtual async void Delete(int id)
        {
            await _repository.Delete(id);
        }
    }
}