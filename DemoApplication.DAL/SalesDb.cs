using DemoApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DemoApplication.DAL
{
	public class SalesDb : IdentityDbContext
	{
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Management> Managements { get; set; }
		public DbSet<Salesman> Salesmen { get; set; }
	

		public SalesDb(DbContextOptions<SalesDb> options) :base(options)
        {
            
        }
    }
}
