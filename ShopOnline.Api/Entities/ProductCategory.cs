namespace ShopOnline.Api.Entities
{
    public class ProductCategory
    {                                    
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        //ProductCategory can have many products , Has a one-to-many relationship with Product
        public ICollection<Product> Products { get; set; }
    }
}
