using ShopOnline.Api.Entities;
using ShopOnline.Models;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Extensions
{
    public static class DtoConversions
    {
        //Converting Products and its categories to Dto
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products)
        {
            return (from product in products                  
                    select new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        ImageURL = product.ImageURL,
                        Price = product.Price,
                        Qty = product.Qty,
                        CategoryId = product.ProductCategory.Id,
                        CategoryName = product.ProductCategory.Name
                    }).ToList();
        }


        //Converting Product and its category to Dto
        public static ProductDto ConvertToDto(this Product product)
                                                             
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageURL = product.ImageURL,
                Price = product.Price,
                Qty = product.Qty,
                CategoryId = product.ProductCategory.Id,
                CategoryName = product.ProductCategory.Name
            };
        }

        public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems,
                                                                   IEnumerable<Product> products)
        {
            return (from cartItem in cartItems
                    join product in products
                    on cartItem.ProductId equals product.Id
                    select new CartItemDto
                    {
                        Id = cartItem.Id,
                        ProductId= cartItem.ProductId,
                        ProductName = product.Name,
                        ProductDescription= product.Description,
                        ProductImageURL = product.ImageURL,
                        Price = product.Price,
                        CartId = cartItem.CartId,   
                        Qty= cartItem.Qty,
                        TotalPrice = product.Price * cartItem.Qty,
                    }).ToList();
        }

        public static CartItemDto ConvertToDto(this CartItem cartItem, Product product)
        {
            return new CartItemDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.ProductId,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductImageURL = product.ImageURL,
                Price = product.Price,
                CartId = cartItem.CartId,
                Qty = cartItem.Qty,
                TotalPrice = product.Price * cartItem.Qty,
            };
        }
    }
}
