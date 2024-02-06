﻿using DemoApplication.Api.Services;
using DemoApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly UserManager<IdentityUser> userManager;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly ITokenService tokenService;
		public AccountController(UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> roleManager,
			ITokenService tokenService)
		{
			this.userManager = userManager;
			this.roleManager = roleManager;
			this.tokenService = tokenService;

		}


		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult<string>> Login(LoginUser user)
		{
			try
			{
				return Ok(await this.tokenService.Get(user));
			}
			catch (Exception ex)
			{

				return BadRequest(ex.Message);
			}

			

		}
		[HttpPost]
		public async Task<ActionResult< string>> RegisterUser(LoginUser user)
		{
			var identityUser = new IdentityUser()
			{
				UserName = user.UserName,
				Email = user.UserName,
			};

			var result = await this.userManager.CreateAsync(identityUser, user.Password);


			if (result.Succeeded)
			{
				return Ok(await this.tokenService.Get(user));
			}
			else
			{
				return BadRequest(result.Errors);
			}

		}


		[HttpPost("RoleCreate")]
		public async Task<ActionResult> CreateRole(string role)
		{
			try
			{
				var identityRole = new IdentityRole(role);


				var result = await roleManager.CreateAsync(identityRole);

				if (result.Succeeded)
				{
					return Ok(identityRole);
				}

				else
				{
					return BadRequest(result.Errors);
				}

			}
			catch (Exception exc)
			{

				return BadRequest(exc);
			}

			
		}
	}
}
