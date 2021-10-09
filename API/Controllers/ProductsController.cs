using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Domain;
using Application.Products;
using MediatR;
using Application.ProductOptions;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
		public ProductsController(IMediator mediator) : base(mediator)
		{
		}

        /// <summary>
        /// Gets all products or finds all products matching the specified name.
        /// URL: GET /products
        /// URL: GET /products?name={name}
        /// </summary>
        /// <param name="name">Product name</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get(string name)
        { 
            
            return HandleResult(await Mediator.Send(new List.Query { Name = name }));
        }

        /// <summary>
        /// Gets the product that matches the specified ID.
        /// URL: GET /products/{id}
        /// </summary>
        /// <param name="id">Product Id.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        }

        /// <summary>
        /// Creates a new product.\
        /// URL: POST /products
        /// <param name="product">Product details</param>
        /// <returns></returns>
		[HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Product = product }));
        }

        /// <summary>
        /// Updates a product.
        /// URL: PUT /products/{id}
        /// </summary>
        /// <param name="id">Product Id.</param>
        /// <param name="product">Product Details.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Product product)
        {
            return HandleResult(await Mediator.Send(new Edit.Command { Id = id, Product = product }));
        }

        /// <summary>
        /// Deletes a product and its options.
        /// URL: DELETE /products/{id}
        /// </summary>
        /// <param name="id">Product Id.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }

        /// <summary>
        /// Finds all options for a specified product.
        /// URL: GET /products/{id}/options
        /// </summary>
        /// <param name="productId">Product Id.</param>
        /// <returns></returns>
        [HttpGet("{productId}/options")]
        public async Task<IActionResult> GetOptions(Guid productId)
        {
            return HandleResult(await Mediator.Send(new ProductOptionList.Query { ProductID = productId }));
        }

        /// <summary>
        /// Finds the specified product option for the specified product.
        /// URL: GET /products/{id}/options/{optionId}
        /// </summary>
        /// <param name="productId">Product Id.</param>
        /// <param name="id">Product Option Id.</param>
        /// <returns></returns>
        [HttpGet("{productId}/options/{id}")]
        public async Task<IActionResult> GetOption(Guid productId, Guid id)
        {
            return HandleResult(await Mediator.Send(new ProductOptionDetails.Query { ProductId = productId, Id = id }));
        }

        /// <summary>
        /// Adds a new product option to the specified product.
        /// URL: POST /products/{id}/options
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <param name="option">Product Option Details</param>
        /// <returns></returns>
        [HttpPost("{productId}/options")]
        public async Task<IActionResult> CreateOption(Guid productId, ProductOption option)
        {
            return HandleResult(await Mediator.Send(new ProductOptionCreate.Command { ProductId = productId, ProductOption = option }));
        }

        /// <summary>
        /// Updates the specified product option.
        /// URL: PUT /products/{id}/options/{optionId}
        /// </summary>
        /// <param name="id">Product Id.</param>
        /// <param name="option">Product Option Details</param>
        /// <returns></returns>
        [HttpPut("{productId}/options/{id}")]
        public async Task<IActionResult> UpdateOption(Guid id, ProductOption option)
        {
            return HandleResult(await Mediator.Send(new ProductOptionEdit.Command { Id = id, ProductOption = option }));
        }

        /// <summary>
        /// Deletes the specified product option.
        /// URL: DELETE /products/{id}/options/{optionId}
        /// </summary>
        /// <param name="productId">Product Id.</param>
        /// <param name="id">Product Option Id.</param>
        /// <returns></returns>
        [HttpDelete("{productId}/options/{id}")]
        public async Task<IActionResult> DeleteOption(Guid productId, Guid id)
        {
            return HandleResult(await Mediator.Send(new ProductOptionDelete.Command { Id = id, ProductId = productId }));
        }
    }
}