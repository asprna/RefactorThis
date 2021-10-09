using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Helper;

namespace API.Controllers
{
	/// <summary>
	/// Base API Class
	/// </summary>
	[ApiController]
	[Route("api/[controller]")]
	public class BaseApiController : ControllerBase
	{
		protected IMediator Mediator;

		public BaseApiController(IMediator mediator)
		{
			Mediator = mediator;
		}

		/// <summary>
		/// Handle controller result.
		/// </summary>
		/// <typeparam name="T">Return type.</typeparam>
		/// <param name="result">Controller result.</param>
		/// <returns></returns>
		protected ActionResult HandleResult<T>(Result<T> result)
		{
			if (result == null) return NotFound();
			if (result.IsSuccess && result.Value != null)
				return Ok(result.Value);
			if (result.IsSuccess && result.Value == null)
				return NotFound();
			return BadRequest(result.Error);
		}
	}
}
