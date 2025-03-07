
-- Tạo cơ sở dữ liệu mới
CREATE DATABASE PCbest; -- Tạo cơ sở dữ liệu "testtttt"
USE PCbest; -- Chuyển sang sử dụng cơ sở dữ liệu mới
GO

-- Bảng: User
CREATE TABLE dbo.[User] (
    UserID INT PRIMARY KEY IDENTITY(1,1), -- ID tự tăng, là khóa chính
    Username VARCHAR(50) UNIQUE NOT NULL, -- Tên đăng nhập, duy nhất, không được để trống
    Password VARCHAR(255) NOT NULL, -- Mật khẩu được lưu dưới dạng mã hóa (hash)
    Role NVARCHAR(20) NOT NULL, -- Vai trò của người dùng (admin, user,...)
    LastName NVARCHAR(50), -- Họ của người dùng
    FirstName NVARCHAR(50), -- Tên của người dùng
    Gender NVARCHAR(10), -- Giới tính
    Email VARCHAR(100) UNIQUE, -- Email duy nhất
    Birthday DATE, -- Ngày sinh
    PhoneNumber VARCHAR(15), -- Số điện thoại
    Created DATETIME DEFAULT GETDATE() -- Ngày tạo tài khoản, mặc định là thời điểm hiện tại
);

-- Bảng: Address (Địa chỉ)
CREATE TABLE dbo.Address (
    AddressID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    CityName NVARCHAR(MAX) NOT NULL,
    DistrictName NVARCHAR(MAX) NOT NULL,
    WardName NVARCHAR(MAX) NOT NULL,
    Address NVARCHAR(255) NOT NULL,
    Description NVARCHAR(1000) NULL,
    FOREIGN KEY (UserID) REFERENCES dbo.[User](UserID),
);

-- Bảng: Product
CREATE TABLE dbo.[Product] (
    ProductID INT PRIMARY KEY IDENTITY(1,1), -- ID sản phẩm, tự tăng
    ProductName NVARCHAR(255) NOT NULL, -- Tên sản phẩm
    Loai VARCHAR(200) NOT NULL, --Loại PC hay laptop
    Manufacturer NVARCHAR(255), -- Nhà sản xuất
    PriceOriginal DECIMAL(18,2) NOT NULL, -- Giá sản phẩm, định dạng số thập phân
    DiscountPercentage DECIMAL(5,2) NULL,	-- % giảm giá
    PriceAfterDiscount DECIMAL(18,2) NOT NULL, -- Giá đã giảm 
    Quantity INT NOT NULL, -- Số lượng sản phẩm
    Description NVARCHAR(1000), -- Mô tả sản phẩm
    Created DATETIME DEFAULT GETDATE() -- Ngày tạo sản phẩm, mặc định là thời điểm hiện tại
);

-- Bảng: Product_attribute
CREATE TABLE dbo.[Product_attribute] (
    AttributeID INT PRIMARY KEY IDENTITY(1,1), -- ID thuộc tính sản phẩm, tự tăng
    ProductID INT NOT NULL, -- Liên kết với sản phẩm
    ProductAttributeName NVARCHAR(255) NOT NULL, --Tên của thông số kĩ thuật vd:Mainboard,CPU ....
    AttributeName NVARCHAR(255) NOT NULL, -- Tên thuộc tính sản phẩm
    AttributeSummary NVARCHAR(255) NULL, --Tên tóm tắt sản phẩm
    FOREIGN KEY (ProductID) REFERENCES dbo.[Product](ProductID) ON DELETE CASCADE -- Nếu sản phẩm bị xóa, các thuộc tính liên quan cũng bị xóa
);

-- Bảng: ImageProduct
CREATE TABLE dbo.[ImageProduct] (
    ImageProductID INT PRIMARY KEY IDENTITY(1,1), -- ID hình ảnh sản phẩm, tự tăng
    ProductID INT NOT NULL, -- Liên kết với sản phẩm
    ImageThumbnail NVARCHAR(MAX) NULL, --Ảnh chính sản phẩm 
    ImageData NVARCHAR(MAX) NULL, -- Lưu của hình ảnh
    FOREIGN KEY (ProductID) REFERENCES dbo.[Product](ProductID) ON DELETE CASCADE -- Nếu sản phẩm bị xóa, các hình ảnh liên quan cũng bị xóa
);

-- Bảng: Vouchers
CREATE TABLE dbo.[Vouchers] (
    VoucherID INT PRIMARY KEY IDENTITY(1,1), -- ID voucher, tự tăng
    VoucherCode VARCHAR(50) NOT NULL UNIQUE, -- Mã voucher, duy nhất
    VoucherName NVARCHAR(255), -- Tên voucher
    Uses INT CHECK (Uses >= 0), -- Số lần sử dụng, phải lớn hơn hoặc bằng 0
    TypeOfVoucher NVARCHAR(50), -- Loại voucher
    DiscountAmount DECIMAL(10,2), -- Giá trị giảm giá
    StartDate DATETIME, -- Ngày bắt đầu hiệu lực
    EndDate DATETIME, -- Ngày kết thúc hiệu lực
    CreatedDate DATETIME DEFAULT GETDATE(), -- Ngày tạo voucher, mặc định là thời điểm hiện tại
    DeletedDate DATETIME -- Ngày voucher bị xóa (nếu có)
);

-- Bảng: Orders (Đơn hàng)
CREATE TABLE dbo.Orders (
    OrdersID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    OrdersName NVARCHAR(MAX) NOT NULL,
    OrdersPhone NVARCHAR(MAX) NOT NULL,
    CityName NVARCHAR(MAX) NOT NULL,
    DistrictName NVARCHAR(MAX) NOT NULL,
    WardName NVARCHAR(MAX) NOT NULL,
    Address NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    OrdersDate DATETIME NOT NULL DEFAULT GETDATE(),
    ShippedDate DATE,
    ShipName NVARCHAR(100),
    ShippingFee DECIMAL(10,2) DEFAULT 0,
    PaymentTypeName NVARCHAR(MAX) NOT NULL,
    OrderStatus NVARCHAR(50) DEFAULT 'Pending',
    TotalPrice DECIMAL(19,4) NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES dbo.[User](UserID)
);

-- Bảng: OrdersDetail (Chi tiết đơn hàng)
CREATE TABLE dbo.OrdersDetail (
    OrderDetailID INT PRIMARY KEY IDENTITY(1,1),
    OrdersID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    FOREIGN KEY (OrdersID) REFERENCES dbo.Orders(OrdersID) ON DELETE CASCADE,
    FOREIGN KEY (ProductID) REFERENCES dbo.Product(ProductID)
);

-- Bảng: OrderStatusHistory (Lịch sử trạng thái đơn hàng)
CREATE TABLE dbo.OrderStatusHistory (
    StatusHistoryID INT PRIMARY KEY IDENTITY(1,1),
    OrdersID INT NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    Notes NVARCHAR(255) NULL,
    UpdatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (OrdersID) REFERENCES dbo.Orders(OrdersID) ON DELETE CASCADE
);


-- Bảng: CustomerVouchers
CREATE TABLE dbo.[CustomerVouchers] (
    CustomerVoucherID INT PRIMARY KEY IDENTITY(1,1), -- ID voucher của khách hàng, tự tăng
    UserID INT NOT NULL, -- Liên kết với người dùng
    VoucherID INT NOT NULL, -- Liên kết với voucher
    Description NVARCHAR(MAX), -- Mô tả (nếu có)
    StartDate DATE, -- Ngày bắt đầu hiệu lực
    EndDate DATE, -- Ngày kết thúc hiệu lực
    Created DATETIME DEFAULT GETDATE(), -- Ngày tạo bản ghi
    FOREIGN KEY (UserID) REFERENCES dbo.[User](UserID), -- Liên kết với người dùng
    FOREIGN KEY (VoucherID) REFERENCES dbo.[Vouchers](VoucherID) -- Liên kết với voucher
);

-- Bảng: ShoppingCart
CREATE TABLE ShoppingCart (
    CartID INT PRIMARY KEY IDENTITY(1,1), -- ID giỏ hàng, tự tăng
    UserID INT NOT NULL, -- Liên kết với người dùng
    CreatedDate DATETIME DEFAULT GETDATE(), -- Ngày tạo giỏ hàng
    FOREIGN KEY (UserID) REFERENCES dbo.[User](UserID) -- Liên kết với người dùng
);

-- 7️⃣ Bảng: ShoppingCartDetails (Giỏ hàng)
CREATE TABLE dbo.ShoppingCartDetails (
    CartDetailID INT PRIMARY KEY IDENTITY(1,1),  -- Mã chi tiết giỏ hàng
    UserID INT NOT NULL,                         -- Người dùng
    ProductID INT NOT NULL,                      -- Sản phẩm
    Quantity INT NOT NULL CHECK (Quantity > 0),  -- Số lượng sản phẩm
    CreatedDate DATETIME DEFAULT GETDATE(),      -- Ngày thêm vào giỏ hàng
    FOREIGN KEY (UserID) REFERENCES dbo.[User](UserID),  -- Khóa ngoại tới User
    FOREIGN KEY (ProductID) REFERENCES dbo.Product(ProductID) -- Khóa ngoại tới Product
);


-- 8️⃣ Bảng: Payment (Thanh toán)
CREATE TABLE dbo.Payment (
    PaymentID INT PRIMARY KEY IDENTITY(1,1), -- Mã thanh toán
    OrdersID INT NOT NULL,                    -- Liên kết đơn hàng
    PaymentType NVARCHAR(50) NOT NULL,        -- Loại thanh toán
    PaymentStatus NVARCHAR(50) DEFAULT 'Pending', -- Trạng thái thanh toán
    Amount DECIMAL(19,4) NOT NULL,            -- Số tiền thanh toán
    PaidDate DATETIME NULL,                   -- Ngày thanh toán
    CreatedDate DATETIME DEFAULT GETDATE(),   -- Ngày tạo giao dịch
    FOREIGN KEY (OrdersID) REFERENCES dbo.Orders(OrdersID) -- Khóa ngoại tới Orders
);


-- Bảng: CreditCard
CREATE TABLE CreditCard (
    CardID INT PRIMARY KEY IDENTITY(1,1), -- ID thẻ tín dụng, tự tăng
    CardNumber NVARCHAR(16) NOT NULL, -- Số thẻ tín dụng
    CardHolder NVARCHAR(255), -- Chủ thẻ
    ExpiryDate DATE, -- Ngày hết hạn thẻ
    CreatedDate DATETIME DEFAULT GETDATE(), -- Ngày tạo bản ghi
);

-- 🔟 Bảng: Invoice (Hóa đơn)
CREATE TABLE dbo.Invoice (
    InvoiceID INT PRIMARY KEY IDENTITY(1,1),  -- Mã hóa đơn
    OrdersID INT NOT NULL,                     -- Liên kết đơn hàng
    InvoiceDate DATETIME DEFAULT GETDATE(),    -- Ngày xuất hóa đơn
    TotalAmount DECIMAL(19,4) NOT NULL,        -- Tổng tiền trên hóa đơn
    FOREIGN KEY (OrdersID) REFERENCES dbo.Orders(OrdersID) -- Khóa ngoại tới Orders
);
CREATE TABLE [dbo].[SidebarMenu](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Icon] [nvarchar](50) NULL,
	[Controller] [nvarchar](50) NULL,
	[Action] [nvarchar](50) NULL,
	[Order] [int] NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SidebarMenu] ADD  DEFAULT ((1)) FOR [IsActive]
