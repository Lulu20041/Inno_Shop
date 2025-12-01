using Application;
using Application.Commands;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProductsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IMediator mediator;

        public ProductsController(IProductService productService, IMediator mediator)
        {
            this.productService = productService;
            this.mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Product>>> Get(CancellationToken cancellationToken)
        {
            var products =  await productService.GetAllAsync(cancellationToken);
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ActionResult<Product>> Get(int id, CancellationToken cancellationToken)
        {
            Product author = await productService.GetByIdAsync(id, cancellationToken);
            return Ok(author);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        {
            var productId = await mediator.Send(command, cancellationToken);
            return Created($"/api/products/{productId}",  productId);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<string>> Put(int id, [FromBody] Product product, CancellationToken cancellationToken)
        {

            await productService.UpdateAsync(product, cancellationToken);
            return Ok($"Product {product.Id} modified");

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<string>> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await productService.DeleteByIdAsync(id, cancellationToken);
            return Ok($"Product {id} was deleted");
        }
    }
}
