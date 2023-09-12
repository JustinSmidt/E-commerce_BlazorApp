using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Globalization;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase:ComponentBase
    {

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        //will be used in ShoppingCart component to display data to screen
        public List<CartItemDto> ShoppingCartItems { get; set; }

        public string ErrorMessage { get; set; }

        //properties used for cart item summary

        protected string TotalPrice { get; set; }

        protected int TotalQuantity { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await this.ShoppingCartService.GetItems(HardCoded.userId);

                CartChanged();
            }
            catch (Exception ex)
            {

                ErrorMessage = ex.Message;
            }
        }


        protected async Task DeleteCartItem_Click(int productId, int cartId)
        {
            var cartItemDto = await this.ShoppingCartService.DeleteItem(productId, cartId);
            

            RemoveCartItem(productId, cartId);

           CartChanged();
        }


        private CartItemDto GetCartItem(int productId, int cartId)
        {
            return ShoppingCartItems.FirstOrDefault(x => x.ProductId == productId && x.CartId == cartId);
          
        }


        private void RemoveCartItem(int productId, int cartId)
        {
            var cartItemDto = GetCartItem(productId, cartId);

            ShoppingCartItems.Remove(cartItemDto);
        }


        protected async Task UpdateQtyCartItem_Click(int productId, int cartId, int quantity)
        {
            try
            {
                if(quantity > 0)
                {
                    var updateItemDto = new CartItemQtyUpdateDto
                    {
                        ProductId = productId,
                        CartId = cartId,
                        Qty = quantity
                    };

                    var returnedUpdateItemDto = await this.ShoppingCartService.UpdateQty(updateItemDto);

                    UpdateItemTotalPrice(returnedUpdateItemDto);

                    CartChanged();
                }
                else
                {
                    var item = this.ShoppingCartItems.FirstOrDefault(i => i.ProductId == productId && i.CartId == cartId);

                    if(item != null)
                    {
                        item.Qty = 1;
                        item.TotalPrice = item.Price;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SetTotalPrice()
        {
            TotalPrice = this.ShoppingCartItems.Sum(i => i.TotalPrice).ToString("C", new CultureInfo("en-US"));
        }

        private void SetTotalQuantity()
        {
            TotalQuantity = this.ShoppingCartItems.Sum(i => i.Qty);
        }

        private void CalculateSummaryTotals()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }

        private void UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            //referencing the relevant cart item
            var item = GetCartItem(cartItemDto.ProductId, cartItemDto.CartId);

            if(item != null)
            {
                item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
            }
        }


        // The event needs to be raised each time the quantity of items stored within the shoppingcart changes. 
        private void CartChanged()
        {
            CalculateSummaryTotals();

            ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
        }

    }
}
