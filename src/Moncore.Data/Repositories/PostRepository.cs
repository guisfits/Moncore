using System;
using System.Linq;
using System.Linq.Expressions;
using Moncore.CrossCutting.Extensions;
using Moncore.CrossCutting.Helpers;
using Moncore.CrossCutting.Interfaces;
using Moncore.Data.Context;
using Moncore.Domain.Entities;
using Moncore.Domain.Helpers;
using Moncore.Domain.Interfaces.Repositories;
using MongoDB.Driver;

namespace Moncore.Data.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(ApplicationContext context, IPropertyMappingService service) 
            : base(context, service)
        {
        }

         public PagedList<Post> Pagination<T>(PostParameters parameters, Expression<Func<Post, bool>> predicate = null)
        {
            IQueryable<Post> result = document.AsQueryable();

            if(predicate != null)
                result = result.Where(predicate);

            FilterAndSearchQuery(parameters, ref result);

            if (parameters.OrderBy == "Id")
                result.OrderBy(c => c.Id);
            else
                result = result.ApplySort(parameters.OrderBy, _propertyMappingService.GetPropertyMappings<Post, T>());

            return PagedList<Post>.Create(result, parameters.Page, parameters.Size);
        }

        public void FilterAndSearchQuery(PostParameters parameters, ref IQueryable<Post> result)
        {
            if(!parameters.Search.IsNullEmptyOrWhiteSpace()){
                result = result.Where(user => 
                    user.Title.Contains(parameters.Search) ||
                    user.Body.Contains(parameters.Search));
            }

            if(!parameters.Title.IsNullEmptyOrWhiteSpace())
                result = result.Where(Post => Post.Title == parameters.Title).AsQueryable();

            if(!parameters.Body.IsNullEmptyOrWhiteSpace())
                result = result.Where(Post => Post.Body.Contains(parameters.Body)).AsQueryable();
        }
    }
}
