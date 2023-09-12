using ShopOnline.Api.Entities;

namespace ShopOnline.Api.Repositories.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetItems();   //returns IEnumerable collection of type Product.
                                                 //Done so that the method that implements this can run asynchronously

        Task<IEnumerable<ProductCategory>> GetCategories();

        Task<Product> GetItem(int id);

        Task<ProductCategory> GetCategory(int id);
    }
}
