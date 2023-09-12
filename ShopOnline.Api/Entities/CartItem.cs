namespace ShopOnline.Api.Entities
{
    public class CartItem                //Joining table for Product and Cart, can be seen as CartProduct
    {      
        public int Id { get; set; }                                  //Cart entity has a one to many relationship with cartItem 
         //reference navigation property                //Meaning many cartItems can be included within one particular shopping cart
        public Cart Cart { get; set; }      //Represents FK and is used to join the cart entity to cartItem entity
        //FK property
        public int CartId { get; set; }

        //reference navigation property
        public Product Product { get; set; }     //Product has a one to many relationship with CartItem
        //FK property
        public int ProductId { get; set; }        //Represents FK and is used to join the Product entity to cartItem entity
                                               //Meaning a product can be included many times  across many carts
        public int Qty { get; set; }
    }
}
