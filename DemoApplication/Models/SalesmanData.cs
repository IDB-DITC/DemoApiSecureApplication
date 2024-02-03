using DemoApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApplication.Api.Models
{
	public class SalesmanData:Salesman
	{
		public IFormFile? file { get; set; }
	}
}
