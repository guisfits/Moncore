using Moncore.Data.Context;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces;

namespace Moncore.Data.Repositories
{
    public class ToDoRepository : Repository<ToDo>, IToDoRepository
    {
        public ToDoRepository(ApplicationContext context) 
            : base(context)
        {
        }
    }
}
