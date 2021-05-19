using Microsoft.AspNetCore.Mvc;
using ShoppyAPI.Data;
using ShoppyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet(Name = "GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            //var result = new Product() { Id = 1, Name = "Samsung" };
            var productList = _db.Product.ToList();


            return Ok(productList);
        }

        [HttpPost]
        public IActionResult CreateNewProduct([FromBody] Product obj)
        {

            if (obj == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                _db.Product.Add(obj);
                _db.SaveChanges();
            }

            var product = _db.Product.Where(p => p.Name == obj.Name).FirstOrDefault();

            return CreatedAtRoute("GetAllProducts", new { productId = product.Id }, obj);

        }

        [HttpGet]
        [Route("{id:int}", Name = "GetProductById")]
        public IActionResult GetProductById([FromRoute] int id)
        {
            var product = _db.Product.Where(p => p.Id == id).FirstOrDefault();

            if (product == null)
            {
                return BadRequest();
            }
            return Ok(product);
        }


        [HttpPatch]
        [Route("{id:int}", Name = "UpdateProductById")]
        public IActionResult UpdateProductById([FromRoute] int id, [FromBody] Product newProduct)
        {
            if (newProduct == null || id != newProduct.Id)
            {
                return BadRequest(ModelState);
            }
            //var product = _db.Product.Where(p => p.Id == id).FirstOrDefault();

            //if (product == null)
            //{
            //    return BadRequest(ModelState);
            //}

            _db.Update(newProduct);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteProduct([FromRoute] int id)
        {
            var product = _db.Product.Where(p => p.Id == id).FirstOrDefault();

            if(product== null)
            {
                return NotFound();
            }
            _db.Product.Remove(product);
            _db.SaveChanges();
            return NoContent();
        }


    }
}
