namespace ShopOnline.Api.Entities
{
    public class Cart
    { 
                                    //Has a many-to-many relationship with Product, thus needing joining table CartItem
                                    //(Product can be in many carts, and a cart can have many products)
                                   //Has a one to many relationship with CartItem                                   
        public int Id { get; set; }


        //Establishing relationship with User. One to one relationship
        public User User { get; set; } 
        public int UserId { get; set; }


        //Establishing relationship with CartItem, as it is the joining table between Cart and Product
        public ICollection<CartItem> CartItem { get; set; }
    }
}
