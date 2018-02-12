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
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context) 
            : base(context)
        {
        }

        public PagedList<User> Pagination(UserParameters parameters, Expression<Func<User, bool>> predicate = null)
        {
            IQueryable<User> result = document.AsQueryable();

            if(predicate != null)
                result = result.Where(predicate);

            FilterQuery(parameters, ref result);

            result.OrderBy(c => c.Id);

            return PagedList<User>.Create(result, parameters.Page, parameters.Size);
        }

        public void FilterQuery(UserParameters parameters, ref IQueryable<User> result)
        {
            if(!parameters.Email.IsNullEmptyOrWhiteSpace())
                result = result.Where(user => user.Email == parameters.Email).AsQueryable();

            if(!parameters.Name.IsNullEmptyOrWhiteSpace())
                result = result.Where(user => user.Name == parameters.Name).AsQueryable();
                
            if(!parameters.Username.IsNullEmptyOrWhiteSpace())
                result = result.Where(user => user.Username == parameters.Username).AsQueryable();

            if(!parameters.Phone.IsNullEmptyOrWhiteSpace())
                result = result.Where(user => user.Phone == parameters.Phone).AsQueryable();

            if(!parameters.Website.IsNullEmptyOrWhiteSpace())
                result = result.Where(user => user.Website == parameters.Website).AsQueryable();
        }
    }
}
