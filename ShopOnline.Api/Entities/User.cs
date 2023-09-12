namespace ShopOnline.Api.Entities
{
    public class User
    {                           //Has one-to-one relationship with Cart, because a user can have only one cart at a time

        public int Id { get; set; }

        public string UserName { get; set; } = string.Empty; 

        public Cart Cart { get; set; }
    }
}
