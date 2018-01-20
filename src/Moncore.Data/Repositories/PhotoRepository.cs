using Moncore.Data.Context;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces;
using Moncore.Domain.Interfaces.Repositories;

namespace Moncore.Data.Repositories
{
    public class PhotoRepository : Repository<Photo>, IPhotoRepository
    {
        public PhotoRepository(ApplicationContext context) 
            : base(context)
        {
        }
    }
}
