using System.Linq;
using System;
using Moncore.CrossCutting.Helpers;
using Moncore.Data.Context;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces.Repositories;

namespace Moncore.Data.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(ApplicationContext context) 
            : base(context)
        {
        }

        protected override IQueryable<Post> SearchByParameters(IQueryable<Post> posts, PaginationParameters parameters)
        {
            if (!parameters.SearchQuery.IsNullEmptyOrWhiteSpace())
            {
                posts = posts.Where(c =>
                    c.Title.Contains(parameters.SearchQuery) ||
                    c.Body.Contains(parameters.SearchQuery));
            }

            return posts;
        }
    }
}
