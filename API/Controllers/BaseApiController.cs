﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Application.Helper;
using Domain;

namespace API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BaseApiController : ControllerBase
	{
		protected IMediator Mediator;

		public BaseApiController(IMediator mediator)
		{
			Mediator = mediator;
		}

		protected ActionResult HandleResult<T>(Result<T> result)
		{
			var products = result.Value as Products;

			if (result == null) return NotFound();
			if (result.IsSuccess && products.Items != null)
				return Ok(result.Value);
			if (result.IsSuccess && products.Items == null)
				return NotFound();
			return BadRequest(result.Error);
		}
	}
}
