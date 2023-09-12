using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductsBase:ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        public IEnumerable<ProductDto> Products { get; set; }

      
        protected override async Task OnInitializedAsync()
        {
            Products = await ProductService.GetItems();

            //The user must be able to see how many items is in the cart when the application is first loaded,
            //thus implement logic in the OnInitializedAsync method
            var shoppingCartItems = await ShoppingCartService.GetItems(HardCoded.userId);
            var totalQty = shoppingCartItems.Sum(i => i.Qty);
            ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);
        }

        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
        {
            // LINQ query
            return from product in Products
                   group product by product.CategoryId into prodByCatGroup
                   orderby prodByCatGroup.Key
                   select prodByCatGroup;
        }


        protected string GetCategoryName(IGrouping<int, ProductDto> groupedProductDto)
        {
            return groupedProductDto.FirstOrDefault(pg => pg.CategoryId == groupedProductDto.Key).CategoryName;
        }


        protected IEnumerable<ProductDto> GetProductsByCategory(string name) 
        {
            return Products.Where(c => c.CategoryName == name).ToList();
                   
        }
        

    }

}

