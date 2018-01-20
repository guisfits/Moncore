using Moncore.Data.Context;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces;
using Moncore.Domain.Interfaces.Repositories;

namespace Moncore.Data.Repositories
{
    public class AlbumRepository : Repository<Album>, IAlbumRepository 
    {
        public AlbumRepository(ApplicationContext context) 
            : base(context)
        {
        }
    }
}
