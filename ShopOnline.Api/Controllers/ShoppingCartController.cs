using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IProductRepository productRepository;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.productRepository = productRepository;
        }

        [HttpGet]
        [Route("{userId}/GetItems")]
        //Method to join collection of Product and CartItem to retreive necessary data, and return a collection of type
        // CartItemDto. Returns all the data pertaining to products in a users cart. Thus we are getting the specific cart 
        //of a specific user, including all product details within his cart via CartItems, which is joining table between
        //Cart, that has id and UserId, and Product, which has data pertaining to products. It gets all cartItems of specific user
        //Responsible for GETTING ALL ITEMS IN CARTITEMS, so that we can display it in razor componenent, allowing user to
        //see all the products in their carts.
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetItems(int userId)
        {
            try
            {   //Getting collection of objects of type cartItems
                var cartItems = await this.shoppingCartRepository.GetItems(userId);

                if(cartItems == null)
                {
                    return NoContent();
                }

                var products = await this.productRepository.GetItems();

                if(products == null)
                {
                    throw new Exception("No products exist in the system");
                }

                var cartItemsDto = cartItems.ConvertToDto(products);

                return Ok(cartItemsDto);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpGet("{id:int}")]
        public async Task<ActionResult<CartItemDto>> GetItem(int id)
        {

            try
            {
                var cartItem = await this.shoppingCartRepository.GetItem(id);

                if (cartItem == null)
                {
                    return NotFound();
                }

                var product = await this.productRepository.GetItem(cartItem.ProductId);

                if (product == null)
                {
                    throw new Exception("No products exist in the system");
                }

                var cartItemDto = cartItem.ConvertToDto(product);
                return Ok(cartItemDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }




        //Adds a specific item selected by user(when clicking adding to cart button) to his cart(cartItem table).
        //So it posts item to cartItem. 
        [HttpPost]
        public async Task<ActionResult<CartItemDto>> PostItem([FromBody] CartItemToAddDto cartItemToAddDto)
        {
            
            try
            {               
                var newCartItem = await this.shoppingCartRepository.AddItem(cartItemToAddDto);

                if (cartItemToAddDto == null)
                {
                    return NoContent();
                }
                
                var product = await this.productRepository.GetItem(newCartItem.ProductId);

                if (product == null)
                {
                    throw new Exception($"Cannot retrieve product (productId:({cartItemToAddDto.ProductId}))");
                }

                

                var newCartItemDto = newCartItem.ConvertToDto(product);

                //standard practice for a POST action method to return the location of the resource where the newly
                //added item can be found. This location will be returned in the header of the HTTP response
                //returned from this method. 
                //Relevant resource can be found at the Uri pertaining to the GetItem action method and the id is
                //included of the newly added resource. For final argument, the newly added object which is now converted to
                //type cartItemDto
                return CreatedAtAction(nameof(GetItem), new {id=newCartItemDto.Id}, newCartItemDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpDelete("{productId}/{cartId}")]
        public async Task<ActionResult<CartItemDto>> DeleteItem(int productId, int cartId)  //Endpoints
        {
            try
            {
                var cartItem = await this.shoppingCartRepository.DeleteItem(productId, cartId);

                if(cartItem == null)
                {
                    return NotFound();
                }

                 //Before calling the convertDto method, the relevant product must first be retrieved from database
                var product = await this.productRepository.GetItem(cartItem.ProductId);

                if(product == null)
                {
                    return NotFound();
                }

                var cartItemDto = cartItem.ConvertToDto(product);


                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        //Only Qty field needs to be updated, making it a partial update function, thus PATCH is appropriate
        [HttpPatch("{productId}/{cartId}")]
        public async Task<ActionResult<CartItemDto>> UpdateQty(int productId, int cartId, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                var cartItem = await this.shoppingCartRepository.UpdateQty(productId, cartId, cartItemQtyUpdateDto);

                if(cartItem == null )
                {
                    return NotFound();
                }

                var product = await productRepository.GetItem(cartItem.ProductId);

                //convert cartItem object to type cartItemDto
                var cartItemDto = cartItem.ConvertToDto(product);

                return Ok(cartItemDto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
