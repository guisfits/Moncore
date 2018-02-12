using System;
using System.Linq;
using System.Linq.Expressions;
using Moncore.CrossCutting.Extensions;
using Moncore.CrossCutting.Helpers;
using Moncore.Data.Context;
using Moncore.Domain.Entities;
using Moncore.Domain.Helpers;
using Moncore.Domain.Interfaces.Repositories;
using MongoDB.Driver;

namespace Moncore.Data.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(ApplicationContext context) 
            : base(context)
        {
        }

         public PagedList<Post> Pagination(PostParameters parameters, Expression<Func<Post, bool>> predicate = null)
        {
            IQueryable<Post> result = document.AsQueryable();

            if(predicate != null)
                result = result.Where(predicate);

            FilterQuery(parameters, ref result);

            result.OrderBy(c => c.Id);

            return PagedList<Post>.Create(result, parameters.Page, parameters.Size);
        }

        public void FilterQuery(PostParameters parameters, ref IQueryable<Post> result)
        {
            if(!parameters.Title.IsNullEmptyOrWhiteSpace())
                result = result.Where(Post => Post.Title == parameters.Title).AsQueryable();

            if(!parameters.Body.IsNullEmptyOrWhiteSpace())
                result = result.Where(Post => Post.Body.Contains(parameters.Body)).AsQueryable();
        }
    }
}
