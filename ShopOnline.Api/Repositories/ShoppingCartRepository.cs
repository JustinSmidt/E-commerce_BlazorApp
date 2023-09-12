using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ShopOnlineDbContext shopOnlineDbContext;

        public ShoppingCartRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            this.shopOnlineDbContext = shopOnlineDbContext;
        }

        private async Task<bool> CartItemExits(int cartId, int productId)
        {
            return await this.shopOnlineDbContext.CartItems.AnyAsync(c => c.CartId == cartId && c.ProductId == productId);            
        }

        //Just adds the specific item selected by user to his cart (cartItem)
        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            if(await CartItemExits(cartItemToAddDto.CartId, cartItemToAddDto.ProductId) == false)
            {
                //LINQ to check if the product that user wants to add to cart exists in product table
                var item = await (from product in this.shopOnlineDbContext.Products
                                  where product.Id == cartItemToAddDto.ProductId
                                  select new CartItem
                                  {
                                      ProductId = product.Id,
                                      CartId = cartItemToAddDto.CartId,
                                      Qty = cartItemToAddDto.Qty,
                                  }).SingleOrDefaultAsync();

                //Add product item to CartItem database table If not null
                if (item != null)
                {
                    var result = await this.shopOnlineDbContext.CartItems.AddAsync(item);
                    await this.shopOnlineDbContext.SaveChangesAsync();
                    return result.Entity;     //returns entity that has successfully been added to CartItem database table
                }
            }        
            return null;           //if not successfully added
        }


        public async Task<CartItem> DeleteItem(int productId, int cartId)
        {
            var item = await this.shopOnlineDbContext.CartItems.FindAsync(productId, cartId);
            

            if(item != null)
            {
                this.shopOnlineDbContext.CartItems.Remove(item);
               
                await this.shopOnlineDbContext.SaveChangesAsync();            
            }

            return item;
        }


        //returns data pertaining to a specific product currently stored in users shopping cart
        public async Task<CartItem> GetItem(int id)
        {
            return await (from cart in this.shopOnlineDbContext.Carts
                         join cartItem in this.shopOnlineDbContext.CartItems
                         on cart.Id equals cartItem.CartId
                         where cart.Id == id
                         select new CartItem
                         {
                             Id = cartItem.Id,
                             ProductId = cartItem.ProductId,
                             Qty = cartItem.Qty,
                             CartId = cartItem.CartId,
                         }).SingleOrDefaultAsync();
        }


        //returns data pertaining to products currently stored in users shopping cart
        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await (from cart in this.shopOnlineDbContext.Carts
                          join cartItem in this.shopOnlineDbContext.CartItems
                          on cart.Id equals cartItem.CartId
                          where cart.UserId == userId
                          select new CartItem
                          {
                              Id = cartItem.Id,
                              ProductId= cartItem.ProductId,
                              Qty = cartItem.Qty,
                              CartId= cartItem.CartId,
                          }).ToListAsync();
        }



        public async Task<CartItem> UpdateQty(int productId, int cartId, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            var item = await this.shopOnlineDbContext.CartItems.FindAsync(productId, cartId);

            if(item != null)
            {
                //updating quantity with the value passed in by the client
                item.Qty = cartItemQtyUpdateDto.Qty;
                await this.shopOnlineDbContext.SaveChangesAsync();
                return item;

            }
            return null;
        }
    }
}
