using CatalogService.Data;
using CatalogService.Events;
using CatalogService.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly CatalogDbContext _db;
        private readonly IPublishEndpoint _publishEndpoint;

        public ProductsController(CatalogDbContext db, IPublishEndpoint publishEndpoint)
        {
            _db = db;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]

        public async Task<IActionResult> Get() => Ok(await _db.Products.ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            product.Id = Guid.NewGuid();
            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            await _publishEndpoint.Publish(new ProductCreatedEvent
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            });

            return Ok(product);
        }
    }
}
