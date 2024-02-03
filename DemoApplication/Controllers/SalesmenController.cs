using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DemoApplication.DAL;
using DemoApplication.Models;
using Microsoft.AspNetCore.Authorization;
using DemoApplication.Api.Models;
using Microsoft.Extensions.Hosting;

namespace DemoApplication.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize()]
	public class SalesmenController : ControllerBase
	{
		private readonly SalesDb _context;
		private readonly IWebHostEnvironment _hostEnvironment;

		public SalesmenController(SalesDb context, IWebHostEnvironment hostEnvironment)
		{
			_context = context;
			_hostEnvironment = hostEnvironment;
		}

		// GET: api/Salesmen
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Salesman>>> GetSalesmen()
		{
			return await _context.Salesmen.ToListAsync();
		}

		// GET: api/Salesmen/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Salesman>> GetSalesman(int id)
		{
			var salesman = await _context.Salesmen.FindAsync(id);

			if (salesman == null)
			{
				return NotFound();
			}

			return salesman;
		}

		// PUT: api/Salesmen/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutSalesman(int id, SalesmanData salesman)
		{
			if (id != salesman.Id)
			{
				return BadRequest();
			}

			if (salesman.file != null)
			{

				try
				{
					salesman.ImagePath = await UploadImage(salesman);

				}
				catch (Exception ex)
				{

					return BadRequest(ex.Message);
				}

			}




			_context.Entry<Salesman>(salesman).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!SalesmanExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}
		[HttpPost("LiveUpload")]

		public async Task<ActionResult<string>> Upload(IFormFile file)
		{

			string filePath = "";

			try
			{
				string imagepath = "\\Upload\\" + file.FileName;

				filePath = _hostEnvironment.WebRootPath + imagepath;

				using (FileStream filestream = System.IO.File.Create(filePath))
				{
					await file.CopyToAsync(filestream);
					await filestream.FlushAsync();
					//  return "\\Upload\\" + objFile.files.FileName;
				}

			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}


			return filePath;
		}
		[HttpPost("Upload")]

		public async Task<ActionResult<string>> Upload()
		{

			if (HttpContext.Request.Form.Files.Count == 0)
				return BadRequest("No file uploaded");

			var file = HttpContext.Request.Form.Files[0];

			string filePath = "";

			try
			{
				string imagepath = "\\Upload\\" + file.FileName;


				filePath = _hostEnvironment.WebRootPath + imagepath;

				using (FileStream filestream = System.IO.File.Create(filePath))
				{
					await file.CopyToAsync(filestream);
					await filestream.FlushAsync();
					//  return "\\Upload\\" + objFile.files.FileName;
				}

			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}


			return filePath;
		}




		private async Task<string> UploadImage(SalesmanData salesman)
		{
			string imagepath = "\\Upload\\" + salesman.file.FileName;


			string filepath = _hostEnvironment.WebRootPath + imagepath;

			using (FileStream filestream = System.IO.File.Create(filepath))
			{
				await salesman.file.CopyToAsync(filestream);
				await filestream.FlushAsync();
				//  return "\\Upload\\" + objFile.files.FileName;
			}

			salesman.ImagePath = imagepath;
			return imagepath;
		}

		// POST: api/Salesmen
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Salesman>> PostSalesman(SalesmanData salesman)
		{


			if (salesman.file != null)
			{

				try
				{
					salesman.ImagePath = await UploadImage(salesman);

				}
				catch (Exception ex)
				{

					return BadRequest(ex.Message);
				}

			}



			_context.Salesmen.Add(salesman);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetSalesman", new { id = salesman.Id }, salesman);
		}

		// DELETE: api/Salesmen/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteSalesman(int id)
		{
			var salesman = await _context.Salesmen.FindAsync(id);
			if (salesman == null)
			{
				return NotFound();
			}

			_context.Salesmen.Remove(salesman);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool SalesmanExists(int id)
		{
			return _context.Salesmen.Any(e => e.Id == id);
		}

	}
}
