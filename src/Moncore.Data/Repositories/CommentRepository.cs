using Moncore.Data.Context;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces;

namespace Moncore.Data.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(ApplicationContext context) 
            : base(context)
        {
        }
    }
}
