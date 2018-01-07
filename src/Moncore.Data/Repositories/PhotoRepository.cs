using Moncore.Data.Context;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces;

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
