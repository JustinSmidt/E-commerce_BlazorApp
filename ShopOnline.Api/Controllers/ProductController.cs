using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        //Method to get All products and the categories it belongs to
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()  //A benefit of returning ActionResult is that it returns the 
        {                                                                    //requested data from action method to the client  
                                                                             //AND an appropriate response status code
            try
            {
                var products = await this.productRepository.GetItems();
              

                if (products == null)
                {
                    return NotFound();
                }
                else
                {
                    var productDtos = products.ConvertToDto();

                    return Ok(productDtos);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }

        }


        //Method to get specific product and its category
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {

            try
            {
                var product = await this.productRepository.GetItem(id);

                if (product == null)
                {
                    return BadRequest();
                }
                else
                {                  
                    var productDto = product.ConvertToDto();

                    return Ok(productDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }

        }
    }
}
