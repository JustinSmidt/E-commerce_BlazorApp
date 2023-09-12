namespace ShopOnline.Api.Entities
{
    public class Product
    {                        //Has a many-to-many relationship with Cart, thus needing joining table CartItem
                                //(Product can be in many carts, and a cart can have many products)
                             //Has a one to many relationship with CartItem
          
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ImageURL { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Qty { get; set; }

        //Establishing connection to ProductCategory, ProductCategory has a one-to-many relationship with Product
        //Navigation property
        public ProductCategory ProductCategory { get; set; }           

        //FK
        public int CategoryId { get; set; }    //Represents FK from ProductCategory entity


        //Establishing relationship with CartItem,   CartItem is joining table between Cart and Product
        public ICollection<CartItem> CartItem { get; set; }
    }
}
