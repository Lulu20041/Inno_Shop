using Application;
using Application.Commands;
using Application.Interfaces;
using Application.Queries;
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
        public async Task<ActionResult<Product>> Get(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var author = await mediator.Send(query, cancellationToken);
            return Ok(author);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
        {
            var productId = await mediator.Send(command, cancellationToken);
            return Created($"/api/products/{productId}",  productId);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<string>> Put(UpdateProductCommand command, CancellationToken cancellationToken)
        {

            var productId = await mediator.Send(command, cancellationToken);
            return Ok($"Product {productId} modified");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<string>> DeleteAsync(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await mediator.Send(query, cancellationToken);
            await productService.DeleteByIdAsync(product.Id, cancellationToken);
            return Ok($"Product {query.Id} was deleted");
        }
    }
}
