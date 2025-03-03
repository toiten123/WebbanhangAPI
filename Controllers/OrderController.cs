using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data;
using QuanLyBanHang.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuanLyBanHang.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔹 GET: api/orders (Lấy danh sách đơn hàng)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orders>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // 🔹 GET: api/orders/{id} (Lấy chi tiết đơn hàng)
        [HttpGet("{id}")]
        public async Task<ActionResult<Orders>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            return order;
        }

        [HttpPost]
        public async Task<ActionResult<Orders>> CreateOrder([FromBody] OrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);  // Kiểm tra xem model có hợp lệ không

            // Tạo đơn hàng mới
            var order = new Orders
            {
                UserID = orderDTO.UserID,
                OrdersName = orderDTO.OrdersName,
                OrdersPhone = orderDTO.OrdersPhone,
                CityName = orderDTO.CityName,
                DistrictName = orderDTO.DistrictName,
                WardName = orderDTO.WardName,
                Address = orderDTO.Address,
                Description = orderDTO.Description,
                OrdersDate = DateTime.Now,
                ShippingFee = orderDTO.ShippingFee,
                PaymentTypeName = orderDTO.PaymentTypeName,
                TotalPrice = orderDTO.TotalPrice,
                CreatedDate = DateTime.Now,
                OrderStatus = "Pending",
                ShipName = "PCbest",
                ShippedDate = DateTime.Now.AddDays(3)  // 3 ngày sau
            };

            try
            {
                // Lưu đơn hàng vào cơ sở dữ liệu
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Trả về thông tin đơn hàng mới
                return Ok(new { order.OrdersID });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi tạo đơn hàng: {ex.Message}");
                return StatusCode(500, "Đã xảy ra lỗi trong quá trình tạo đơn hàng.");
            }
        }

        [HttpPost("{orderId}/details")]
        public async Task<ActionResult> AddOrderDetails(int orderId, [FromBody] List<OrderDetailDTO> orderDetails)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return NotFound("Đơn hàng không tồn tại");

            var addedDetails = new List<object>();

            foreach (var detail in orderDetails)
            {
                var orderDetail = new OrdersDetail
                {
                    OrdersID = orderId,
                    ProductID = detail.ProductID,
                    Quantity = detail.Quantity
                };
                _context.OrderDetails.Add(orderDetail);
                addedDetails.Add(new { detail.ProductID, detail.Quantity });

                // Cập nhật số lượng sản phẩm
                var product = await _context.Products.FindAsync(detail.ProductID);
                if (product != null)
                {
                    product.Quantity -= detail.Quantity;

                    // Nếu sản phẩm hết hàng
                    if (product.Quantity <= 0)
                    {
                        // Cập nhật trạng thái sản phẩm hết hàng
                        product.Description = "Hết hàng";
                    }

                    _context.Entry(product).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { OrderDetails = addedDetails });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm chi tiết đơn hàng: {ex.Message}");
                return StatusCode(500, "Đã xảy ra lỗi trong quá trình thêm chi tiết đơn hàng.");
            }
        }



        [HttpPost("{orderId}/status")]
        public async Task<ActionResult> AddOrderStatus(int orderId, [FromBody] OrderStatusHistoryDTO statusHistoryDTO)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // Tạo mới trạng thái từ DTO
            var newStatusHistory = new OrderStatusHistory
            {
                OrdersID = orderId,
                Status = statusHistoryDTO.Status ?? "Pending",  // Trạng thái mặc định là "Pending"
                Notes = statusHistoryDTO.Notes ?? "",            // Ghi chú trống nếu không có
                UpdatedDate = DateTime.Now
            };

            _context.OrderStatusHistories.Add(newStatusHistory);

            try
            {
                await _context.SaveChangesAsync();
                // Thay đổi tại đây để trả về Status và Notes
            return Ok(new { Status = newStatusHistory.Status});
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "An error occurred while adding the status.");
            }
        }


        // 🔹 PUT: api/orders/{id} (Cập nhật đơn hàng)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Orders order)
        {
            if (id != order.OrdersID) return BadRequest();

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Orders.Any(e => e.OrdersID == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // 🔹 DELETE: api/orders/{id} (Xóa đơn hàng)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
