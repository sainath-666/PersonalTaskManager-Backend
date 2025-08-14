using Microsoft.AspNetCore.Mvc;
using PersonalTaskManager.Data;
using PersonalTaskManager.Models;

namespace PersonalTaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartRepository _cartRepo;

        public CartController(CartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }

        [HttpGet("{sessionId}")]
        public IActionResult GetCart(string sessionId)
        {
            var cartItems = _cartRepo.GetCart(sessionId);
            return Ok(cartItems);
        }

        [HttpPost]
        public IActionResult AddToCart([FromBody] CartItem cartItem)
        {
            _cartRepo.AddToCart(cartItem);
            return Ok(new { message = "Item added to cart successfully" });
        }

        [HttpPut("{cartItemId}")]
        public IActionResult UpdateQuantity(int cartItemId, [FromBody] int quantity)
        {
            _cartRepo.UpdateQuantity(cartItemId, quantity);
            return Ok(new { message = "Quantity updated successfully" });
        }

        [HttpDelete("{cartItemId}")]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            _cartRepo.RemoveFromCart(cartItemId);
            return Ok(new { message = "Item removed from cart successfully" });
        }
    }
}
