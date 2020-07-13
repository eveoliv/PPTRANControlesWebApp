using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;
using PPTRANControlesWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Repositories
{
    public abstract class BaseRepository<T> where T : BaseModel
    {
        protected readonly IConfiguration configuration;
        protected readonly ApplicationContext context;
        protected readonly DbSet<T> dbSet;

        protected BaseRepository(IConfiguration configuration, ApplicationContext context)
        {
            this.configuration = configuration;
            this.context = context;
            dbSet = context.Set<T>();
        }
    }
}
