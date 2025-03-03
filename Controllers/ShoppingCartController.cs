using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHang.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔹 GET: api/ShoppingCart/{userId} -> Lấy giỏ hàng theo UserID
        [HttpGet("{userId}")]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCart(int userId)
        {
            var shoppingCart = await _context.ShoppingCarts
                .Include(sc => sc.ShoppingCartDetails)
                .ThenInclude(scd => scd.Product)
                .FirstOrDefaultAsync(sc => sc.UserID == userId);

            if (shoppingCart == null)
            {
                return NotFound(new { message = "Giỏ hàng không tồn tại!" });
            }

            return Ok(shoppingCart);
        }

        // 🔹 POST: api/ShoppingCart -> Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public async Task<ActionResult<ShoppingCartDetails>> AddToCart([FromBody] ShoppingCartDetails cartDetail)
        {
            if (cartDetail == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ!" });
            }

            var shoppingCart = await _context.ShoppingCarts
                .FirstOrDefaultAsync(sc => sc.UserID == cartDetail.UserID);

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart
                {
                    UserID = cartDetail.UserID,
                    CreatedDate = DateTime.Now
                };

                _context.ShoppingCarts.Add(shoppingCart);
                await _context.SaveChangesAsync();
            }

            cartDetail.CreatedDate = DateTime.Now;

            _context.ShoppingCartDetails.Add(cartDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetShoppingCart), new { userId = shoppingCart.UserID }, cartDetail);
        }

        // 🔹 PUT: api/ShoppingCart/{id} -> Cập nhật sản phẩm trong giỏ hàng
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCartItem(int id, [FromBody] ShoppingCartDetails cartDetail)
        {
            if (id != cartDetail.CartDetailID)
            {
                return BadRequest(new { message = "ID không khớp với dữ liệu gửi lên!" });
            }

            _context.Entry(cartDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartDetailExists(id))
                {
                    return NotFound(new { message = "Không tìm thấy sản phẩm trong giỏ hàng!" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // 🔹 DELETE: api/ShoppingCart/{id} -> Xóa sản phẩm khỏi giỏ hàng
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cartDetail = await _context.ShoppingCartDetails.FindAsync(id);
            if (cartDetail == null)
            {
                return NotFound(new { message = "Sản phẩm không tồn tại trong giỏ hàng!" });
            }

            _context.ShoppingCartDetails.Remove(cartDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 🔹 Kiểm tra sản phẩm có tồn tại trong giỏ hàng không
        private bool CartDetailExists(int id)
        {
            return _context.ShoppingCartDetails.Any(e => e.CartDetailID == id);
        }
    }
}
