using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class CheckoutBase:ComponentBase
    {

        protected IEnumerable<CartItemDto> ShoppingCartItems { get; set; }

        protected int TotalQty { get; set; }

        protected decimal PaymentAmount { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.userId);

                if(ShoppingCartItems != null)
                {                  
                    PaymentAmount = ShoppingCartItems.Sum(p => p.TotalPrice);

                    TotalQty = ShoppingCartItems.Sum(q => q.Qty);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
