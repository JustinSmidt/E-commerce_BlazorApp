using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;

namespace ShopOnline.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopOnlineDbContext shopOnlineDbContext;

        public ProductRepository(ShopOnlineDbContext shopOnlineDbContext)
        {
            this.shopOnlineDbContext = shopOnlineDbContext;
        }

        //Method for getting categories
        public async Task<IEnumerable<ProductCategory>> GetCategories()
        {
            var categories = await this.shopOnlineDbContext.ProductCategories.ToListAsync();

            return categories;
        }


        //Method for getting category
        public async Task<ProductCategory> GetCategory(int id)
        {
            var category = await shopOnlineDbContext.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);

            return category;
        }


        //Method for getting product
        public async Task<Product> GetItem(int id)
        {
            //var product = await this.shopOnlineDbContext.Products.FindAsync(id);
            var product = await this.shopOnlineDbContext.Products
                                    .Include(p => p.ProductCategory)
                                    .SingleOrDefaultAsync(p => p.Id == id);


            return product;
        }


        //Method for getting products
        public async Task<IEnumerable<Product>> GetItems()
        {
            //var products = await this.shopOnlineDbContext.Products.ToListAsync();

            var products = await this.shopOnlineDbContext.Products
                                     .Include(p => p.ProductCategory).ToArrayAsync();

                   
            return products;
        }
    }
}
