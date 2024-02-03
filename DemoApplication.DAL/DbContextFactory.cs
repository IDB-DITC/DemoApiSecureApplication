using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApplication.DAL
{
	public class DbContextFactory : IDesignTimeDbContextFactory<SalesDb>
	{
		public SalesDb CreateDbContext(string[] args)
		{
			var optionBuilder = new DbContextOptionsBuilder<SalesDb>();
			optionBuilder.UseSqlServer("server = (localdb)\\MSSqlLocaldb; Database= demoAppDatabase;  trusted_connection =true;");//AttachDbFileName = Database\\demoAppDatabase.mdf;

			return new SalesDb(optionBuilder.Options);

		}
	}
}
