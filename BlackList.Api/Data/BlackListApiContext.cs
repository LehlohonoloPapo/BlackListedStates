using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlackList.Api.Models;

namespace BlackList.Api.Data
{
    public class BlackListApiContext : DbContext
    {
        public BlackListApiContext (DbContextOptions<BlackListApiContext> options)
            : base(options)
        {
        }

        public DbSet<BlackList.Api.Models.BlackListedStates> BlackListedStates { get; set; } = default!;
    }
}
