using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moncore.Data.Context;
using Moncore.Data.Helpers;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces;
using MongoDB.Driver;

namespace Moncore.Data.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly ApplicationContext _context;

        public AlbumRepository(IOptions<DbSettings> settings)
        {
            _context = new ApplicationContext(settings);
        }

        public async Task<ICollection<Album>> GetAll()
        {
            return await _context.Albums.Find(c => true).ToListAsync();
        }

        public Task<Album> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Album>> Find(Expression<Func<Album, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task Add(Album obj)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(int id, Album obj)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
