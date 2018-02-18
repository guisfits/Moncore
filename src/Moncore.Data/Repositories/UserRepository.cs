using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context, IPropertyMappingService service) 
            : base(context, service)
        {
        }

        public PagedList<User> Pagination<T>(UserParameters parameters, Expression<Func<User, bool>> predicate = null)
        {
            IQueryable<User> result = document.AsQueryable();

            if(predicate != null)
                result = result.Where(predicate);

            FilterAndSearchQuery(parameters, ref result);

            if(parameters.OrderBy == "Id")
                result.OrderBy(c => c.Id);
            else
                result = result.ApplySort(parameters.OrderBy, _propertyMappingService.GetPropertyMappings<User, T>());

            return PagedList<User>.Create(result, parameters.Page, parameters.Size);
        }

        public void FilterAndSearchQuery(UserParameters parameters, ref IQueryable<User> result)
        {
            if(!parameters.Search.IsNullEmptyOrWhiteSpace()){
                result = result.Where(user => 
                    user.Email.Contains(parameters.Search) ||
                    user.Name.Contains(parameters.Search) || 
                    user.Phone.Contains(parameters.Search) || 
                    user.Email.Contains(parameters.Search) || 
                    user.Username.Contains(parameters.Search) || 
                    user.Website.Contains(parameters.Search));
            }

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
