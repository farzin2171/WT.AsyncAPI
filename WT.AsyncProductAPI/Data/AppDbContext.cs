using Microsoft.EntityFrameworkCore;
using WT.AsyncProductAPI.Models;

namespace WT.AsyncProductAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<ListingRequest> ListingRequests => Set<ListingRequest>();

    }
}
