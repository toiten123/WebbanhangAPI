using Microsoft.AspNetCore.Mvc;
using QuanLyBanHang.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.IO;

namespace QuanLyBanHang.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly Cloudinary _cloudinary;

        public ProductController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;

            // Cấu hình Cloudinary
            var cloudinarySettings = configuration.GetSection("CloudinarySettings");
            var account = new Account(
                cloudinarySettings["CloudName"],
                cloudinarySettings["ApiKey"],
                cloudinarySettings["ApiSecret"]
            );
            _cloudinary = new Cloudinary(account);
        }

        // Lấy danh sách sản phẩm
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
            var products = await _context.Products
                .Include(p => p.Attributes)
                .Include(p => p.Images)
                .ToListAsync();

            var productDTOs = products.Select(p => new ProductDTO
            {
                Product = p,
                Attributes = p.Attributes?.Where(a =>
                    // Lọc các thuộc tính quan trọng cho PC, Laptop, Mouse và Keyboard
                    a.ProductAttributeName == "CPU" || 
                    a.ProductAttributeName == "VGA" || 
                    a.ProductAttributeName == "Mainboard" || 
                    a.ProductAttributeName == "RAM" || 
                    a.ProductAttributeName == "SSD" ||
                    (p.Loai == "laptop" && (a.ProductAttributeName == "Màn hình" || a.ProductAttributeName == "Tần số quét")) ||
                    (p.Loai == "mouse" && (a.ProductAttributeName == "Dây" || a.ProductAttributeName == "LED RGB" || a.ProductAttributeName == "DPI")) ||
                    (p.Loai == "keyboard" && (a.ProductAttributeName == "Loại bàn phím" || a.ProductAttributeName == "Switch" || a.ProductAttributeName == "Keycaps")) ||
                    (p.Loai == "monitor" && (a.ProductAttributeName == "Kích thước" ||a.ProductAttributeName == "Độ phân giải" ||a.ProductAttributeName == "Tần số quét" ||a.ProductAttributeName == "Tấm nền" ))
                ).ToList() ?? new List<ProductAttribute>(),

                // Lấy ảnh thumbnail (ảnh đầu tiên nếu có từ cột ImageThumbnail)
                ImageThumbnail = p.Images?.Where(img => img.ImageThumbnail != null).Select(img => img.ImageThumbnail).FirstOrDefault(),

                // Lấy tất cả ảnh sản phẩm khác (không phải thumbnail)
                ImageData = p.Images?.Select(img => img.ImageData).Where(url => url != null).ToList() ?? new List<string>()

            }).ToList();

            return Ok(productDTOs);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Attributes)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ProductID == id);

            if (product == null)
            {
                return NotFound();
            }

            var productDTO = new ProductDTO
            {
                Product = product,
                Attributes = product.Attributes?.ToList() ?? new List<ProductAttribute>(),

                ImageThumbnail = product.Images?.Where(img => img.ImageThumbnail != null)
                                                .Select(img => img.ImageThumbnail)
                                                .FirstOrDefault(),

                ImageData = product.Images?.Select(img => img.ImageData)
                                        .Where(url => url != null)
                                        .ToList() ?? new List<string>()
            };

            // Trả về thông tin trạng thái sản phẩm
            productDTO.Product.Description = product.Quantity == 0 ? "Hết hàng" : product.Description;

            return Ok(productDTO);
        }



        // Thêm sản phẩm
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductDTO productDto)
        {
            if (productDto == null || productDto.Product == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            var product = productDto.Product;
            // Lưu sản phẩm vào cơ sở dữ liệu trước
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var attributes = productDto.Attributes ?? new List<ProductAttribute>();
            // Lưu thông số sản phảm vào cơ sở dữ liệu
            if (attributes.Count > 0)
            {
                foreach (var attribute in attributes)
                {
                    attribute.ProductID = product.ProductID;
                    _context.ProductAttributes.Add(attribute);
                }
                await _context.SaveChangesAsync();
            }


            var imageDataList = productDto.ImageData ?? new List<string>();
            var imageThumbnailUrl = productDto.ImageThumbnail; // Nhận ảnh Thumbnail từ request (URL ảnh)

            // Tạo danh sách ảnh
            var images = new List<ImageProduct>();
            if (imageDataList.Count > 0 || !string.IsNullOrEmpty(imageThumbnailUrl))
            {
                // Xử lý ảnh sản phẩm
                foreach (var imageUrl in imageDataList)
                {
                    if (string.IsNullOrEmpty(imageUrl))
                        continue;

                    // Lưu ảnh URL vào danh sách
                    images.Add(new ImageProduct
                    {
                        ProductID = product.ProductID,
                        ImageData = imageUrl // Lưu trực tiếp URL của ảnh
                    });
                }

                // Xử lý ảnh thumbnail nếu có
                if (!string.IsNullOrEmpty(imageThumbnailUrl))
                {
                    images.Add(new ImageProduct
                    {
                        ProductID = product.ProductID,
                        ImageThumbnail = imageThumbnailUrl, // Lưu trực tiếp URL thumbnail
                    });
                }
            }

            // Thêm ảnh vào cơ sở dữ liệu
            _context.ImageProducts.AddRange(images);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductID }, product);
        }

        [HttpGet("{productId}/check-stock")]
        public async Task<ActionResult> CheckProductStock(int productId, int quantity)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == productId);
            if (product == null)
            {
                return NotFound();  // Trả về lỗi 404 nếu không tìm thấy sản phẩm
            }

            // Kiểm tra nếu số lượng sản phẩm trong kho đủ để đáp ứng yêu cầu
            if (product.Quantity < quantity)
            {
                return Ok(new { isAvailable = false }); // Trả về JSON hợp lệ khi sản phẩm không đủ
            }

            return Ok(new { isAvailable = true }); // Trả về JSON hợp lệ khi sản phẩm đủ
        }


        // Cập nhật sản phẩm
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDTO productDto)
        {

            if (id != productDto.Product.ProductID)
            {
                return BadRequest("Product ID không khớp. ID trong URL: " + id + ", ID trong body: " + productDto.Product.ProductID);
            } 
            var existingProduct = await _context.Products
                                                .Include(p => p.Attributes)
                                                .Include(p => p.Images)
                                                .FirstOrDefaultAsync(p => p.ProductID == id);

            if (existingProduct == null)
            {
                return NotFound("Không tìm thấy sản phẩm.");
            }

            bool isProductUpdated = false;

            if (existingProduct.ProductName != productDto.Product.ProductName)
            {
                existingProduct.ProductName = productDto.Product.ProductName;
                isProductUpdated = true;
            }
            if (existingProduct.Manufacturer != productDto.Product.Manufacturer)
            {
                existingProduct.Manufacturer = productDto.Product.Manufacturer;
                isProductUpdated = true;
            }
            if (existingProduct.PriceOriginal != productDto.Product.PriceOriginal)
            {
                existingProduct.PriceOriginal = productDto.Product.PriceOriginal;
                isProductUpdated = true;
            }
            if (existingProduct.DiscountPercentage != productDto.Product.DiscountPercentage)
            {
                existingProduct.DiscountPercentage = productDto.Product.DiscountPercentage;
                isProductUpdated = true;
            }
            if (existingProduct.PriceAfterDiscount != productDto.Product.PriceAfterDiscount)
            {
                existingProduct.PriceAfterDiscount = productDto.Product.PriceAfterDiscount;
                isProductUpdated = true;
            }
            if (existingProduct.Quantity != productDto.Product.Quantity)
            {
                existingProduct.Quantity = productDto.Product.Quantity;
                isProductUpdated = true;
            }
            if (existingProduct.Description != productDto.Product.Description)
            {
                existingProduct.Description = productDto.Product.Description;
                isProductUpdated = true;
            }
            if (existingProduct.Loai != productDto.Product.Loai)
            {
                existingProduct.Loai = productDto.Product.Loai;
                isProductUpdated = true;
            }

            if (productDto.Attributes != null && productDto.Attributes.Count > 0)
            {
                var existingAttributes = await _context.ProductAttributes
                                                    .Where(a => a.ProductID == id)
                                                    .ToListAsync();
_context.ProductAttributes.RemoveRange(existingAttributes);
                foreach (var attribute in productDto.Attributes)
                {
                    attribute.ProductID = id;
                    _context.ProductAttributes.Add(attribute);
                }
                isProductUpdated = true;
            }

            bool isImageUpdated = false;
            if (productDto.ImageData != null && productDto.ImageData.Count > 0 || !string.IsNullOrEmpty(productDto.ImageThumbnail))
            {
                var existingImages = await _context.ImageProducts
                                                    .Where(img => img.ProductID == id)
                                                    .ToListAsync();
                var imagesToRemove = existingImages.Where(existing =>
                                    !productDto.ImageData?.Contains(existing.ImageData) ?? false).ToList();
                if (imagesToRemove.Any())
                {
                    _context.ImageProducts.RemoveRange(imagesToRemove);
                    isImageUpdated = true;
                }
                var imagesToAdd = productDto.ImageData?.Where(newImg => 
                                    !existingImages.Any(existing => existing.ImageData == newImg)).ToList() ?? new List<string>();
                if (imagesToAdd.Any())
                {
                    var newImages = imagesToAdd.Select(imageUrl => new ImageProduct
                    {
                        ProductID = id,
                        ImageData = imageUrl
                    }).ToList();

                    _context.ImageProducts.AddRange(newImages);
                    isImageUpdated = true;
                }

                if (!string.IsNullOrEmpty(productDto.ImageThumbnail) && 
                    !existingImages.Any(img => img.ImageThumbnail == productDto.ImageThumbnail))
                {
                    _context.ImageProducts.Add(new ImageProduct
                    {
                        ProductID       = id,
                        ImageThumbnail  = productDto.ImageThumbnail
                    });
                    isImageUpdated = true;
                }
            }

            if (isProductUpdated || isImageUpdated)
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(e => e.ProductID == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            else
            {
                return BadRequest("Không có thay đổi nào được thực hiện.");
            }
        }


        // Xóa sản phẩm
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
    }
}
