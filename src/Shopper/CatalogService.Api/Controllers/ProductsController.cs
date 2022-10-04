﻿using CatalogService.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;


namespace CatalogService.Api.Controllers
{
    

    // products+
   
    [ApiController]
    [Route("/api/products")]  // RoutePrefix
    public class ProductsController : ControllerBase
    {
        // GET http://localhost:5000/api/products

        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET api/customers/{id}/products
        [HttpGet("/api/customers/{customerId}/products")]
        public void GetByCustomer(int customerId)
        {
            throw new NotImplementedException();
        }


        // GET /api/products           
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            var products = _productRepository.Get();

            return products;
        }

        // GET /api/products/{id}
        [HttpGet("{id}.{format?}", Name = "GetProductById")]
        [FormatFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult<Product> Get([FromRoute] int id)
        {
            var product = _productRepository.Get(id);

            if (product is null)
            {
                // return new NotFoundResult();
                return NotFound();
                
            }

            // return new OkObjectResult(product);
            return Ok(product);
            
        }

        // GET /api/products?id={id}
        //[HttpGet("/api/products/{id}")]
        //public Product Get([FromQuery] int id)
        //{
        //    var product = _productRepository.Get(id);

        //    return product;
        //}

        // POST api/products

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public ActionResult<Product> Post(Product product, [FromServices] IMessageService messageService)
        {
            _productRepository.Add(product);

            messageService.Send(product);

            // zła praktyka
            // return Created($"http://localhost:5000/api/products/{product.Id}", product);

            // Utworzono pod adresem
            return CreatedAtRoute("GetProductById", new { id = product.Id }, product);
        }

        // PUT api/products/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public ActionResult Put(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            if (!_productRepository.IsExists(id))
            { 
                return NotFound();
            }

            _productRepository.Update(product);

            return NoContent();
        }


        // DELETE api/products/{id}

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Delete(int id)
        {
            if (!_productRepository.IsExists(id))
            {
                return NotFound();
            }

            _productRepository.Remove(id);

            return Ok();
        }

        // https://jsonpatch.com/
        // dotnet add package Microsoft.AspNetCore.JsonPatch
        // Content-Type: application/json-patch+json
        // dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson
        // builder.Services.AddControllers().AddNewtonsoftJson();
        // https://learn.microsoft.com/pl-pl/aspnet/core/web-api/jsonpatch?view=aspnetcore-6.0

        [HttpPatch("{id}")]
        public ActionResult Patch(int id, JsonPatchDocument<Product> patchProduct)
        {
            var product = _productRepository.Get(id);

            patchProduct.ApplyTo(product);


            return NoContent();
        }


        // JSON Merge Patch
        // https://www.rfc-editor.org/rfc/rfc7386
        // Content-Type: application/merge-patch+json
        // {
        //  "a":"z",
        //  "c": {
        //"f": null
        //  }
        //}


        // PATCH api/products/{id}/Accept                

        [HttpPatch("{id}/Accept")]
        public ActionResult Accept(int id)
        {
            var product = _productRepository.Get(id);

            if (product.Status == ProductStatus.Draft)
            {
                product.Status = ProductStatus.Published;
                return Ok();
            }
            else
            {
                ApplicationException ex = new ApplicationException();

                return BadRequest(ex.Message);
            }            
        }


        // GET api/products/{id}
        [HttpHead("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public ActionResult Head(int id)
        {
            if (_productRepository.IsExists(id))
                return Ok();
            else
                return NotFound();
        }

    }
}
