using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using TStore.Domain.Entities;

namespace TStore.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Spider> Spiders { get; set; }
    }
}
