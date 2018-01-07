﻿using Moncore.Data.Context;
using Moncore.Domain.Entities;
using Moncore.Domain.Interfaces;

namespace Moncore.Data.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(ApplicationContext context) 
            : base(context)
        {
        }
    }
}