using System;
using Microsoft.AspNetCore.Mvc;
//using API.Models;
using System.Linq;
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

        [HttpGet]
        public async Task<IActionResult> Get(string name)
        { 
            
            return HandleResult(await Mediator.Send(new List.Query { Name = name }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        }

        
		[HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Product = product }));
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Product product)
        {
            return HandleResult(await Mediator.Send(new Edit.Command { Id = id, Product = product }));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }

        
        [HttpGet("{productId}/options")]
        public async Task<IActionResult> GetOptions(Guid productId)
        {
            return HandleResult(await Mediator.Send(new ProductOptionList.Query { ProductID = productId }));
        }

        
        [HttpGet("{productId}/options/{id}")]
        public async Task<IActionResult> GetOption(Guid productId, Guid id)
        {
            return HandleResult(await Mediator.Send(new ProductOptionDetails.Query { ProductId = productId, Id = id }));
        }

        /*
        [HttpPost("{productId}/options")]
        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;
            option.Save();
        }

        [HttpPut("{productId}/options/{id}")]
        public void UpdateOption(Guid id, ProductOption option)
        {
            var orig = new ProductOption(id)
            {
                Name = option.Name,
                Description = option.Description
            };

            if (!orig.IsNew)
                orig.Save();
        }

        [HttpDelete("{productId}/options/{id}")]
        public void DeleteOption(Guid id)
        {
            var opt = new ProductOption(id);
            opt.Delete();
        }
        */
    }
}