using ShoesSalePage.Data;

namespace ShoesSalePage.Models
{
    /* Since we don't want to require users to sign up for an account just to add items to their shopping cart, 
       we will assign users a temporary unique identifier (using a GUID, or globally unique identifier) when
       they access the shopping cart. We'll store this ID using the ASP.NET Session class.*/

    /* Note: The ASP.NET Session is a convenient place to store user-specific information 
       which will expire after they leave the site. While misuse of session state can have performance implications
       on larger sites, our light use will work well for demonstration purposes.*/
    public class ShoppingCart
    {
        ShoesDbContext shoes = new ShoesDbContext();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";

    }
}