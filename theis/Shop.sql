
create table [Users] (
	[user_id] int PRIMARY KEY IDENTITY(1,1),
	[full_name] nvarchar(100) not null,
	[user_name] nvarchar(100) not null,
	[pass_word] nvarchar(100) not null,
	[create_date] datetime default getdate(),
	[email] varchar(50),
	[user_phone_number] varchar(10) not null,
	[user_address] nvarchar(200),
	[user_image_url] varchar(100),
	[is_verify] bit default 0,
	[user_role] int default 1,
	[refresh_token] varchar(200)
);

create table [OTP] (
	[otp_id] int primary key identity(1,1),
	[email] varchar(50) not null,
	[otp_code] varchar(6) not null,
	[otp_expired] datetime,
);

CREATE TABLE [Brands] (
    [brand_id] INT PRIMARY KEY IDENTITY(1,1),
    [brand_name] NVARCHAR(100) NOT NULL,
	[brand_slug] varchar(150),
	[brand_image_url] varchar(100),
);

CREATE TABLE [Phones] (
    [phone_id] INT PRIMARY KEY IDENTITY(1,1),
    [phone_name] NVARCHAR(100) NOT NULL,
    [brand_id] INT NOT NULL,
	[phone_image_url] varchar(100),
    [phone_screen] NVARCHAR(100),        -- Kích thước và loại màn hình
    [phone_chip] NVARCHAR(100),          -- Tên chip xử lý
    [phone_battery] NVARCHAR(50),        -- Dung lượng pin
    [description] NVARCHAR(1000),   -- Mô tả chi tiết
	[phone_slug] varchar(150),
	[is_active] bit default 1,
    FOREIGN KEY ([brand_id]) REFERENCES [Brands]([brand_id])
);

CREATE TABLE [PhoneImages] (
	[img_id] int primary key identity(1,1),
	[phone_id] int,
	[img_image_url] varchar(100),
	FOREIGN KEY ([phone_id]) REFERENCES [Phones]([phone_id])
);

CREATE TABLE [Colors] (
    [color_id] INT PRIMARY KEY IDENTITY(1,1),
    [color_name] NVARCHAR(50) NOT NULL
);

CREATE TABLE [RAMs] (
    [ram_id] INT PRIMARY KEY IDENTITY(1,1),
    [ram_size] NVARCHAR(50) NOT NULL   -- VD: '4GB', '8GB'
);

CREATE TABLE [Storages] (
    [storage_id] INT PRIMARY KEY IDENTITY(1,1),
    [storage_size] NVARCHAR(50) NOT NULL   -- VD: '64GB', '128GB'
);

CREATE TABLE [PhoneVariants] (
    [var_id] INT PRIMARY KEY IDENTITY(1,1),
    [phone_id] INT NOT NULL,
    [color_id] INT NOT NULL,
    [ram_id] INT NOT NULL,
    [storage_id] INT NOT NULL,
    [var_price] int NOT NULL,
    [stock_quantity] INT DEFAULT 0,           -- Số lượng còn lại trong kho
    FOREIGN KEY ([phone_id]) REFERENCES [Phones]([phone_id]),
    FOREIGN KEY ([color_id]) REFERENCES [Colors]([color_id]),
    FOREIGN KEY ([ram_id]) REFERENCES [RAMs]([ram_id]),
    FOREIGN KEY ([storage_id]) REFERENCES [Storages]([storage_id])
);

CREATE TABLE [Carts] (
    [cart_id] INT PRIMARY KEY IDENTITY(1,1),
    [user_id] INT NOT NULL,
    [var_id] INT NOT NULL,
    [quantity] INT NOT NULL CHECK (quantity > 0),
    [added_date] DATETIME DEFAULT GETDATE(),
    FOREIGN KEY ([user_id]) REFERENCES [Users]([user_id]),
    FOREIGN KEY ([var_id]) REFERENCES [PhoneVariants]([var_id])
);

CREATE TABLE [PaymentMethods] (
    [method_id] INT PRIMARY KEY IDENTITY(1,1),
    [method_name] NVARCHAR(100) NOT NULL -- VD: Thanh toán khi nhận, Chuyển khoản, VNPay...
);

CREATE TABLE [Shippers] (
    [ship_id] INT PRIMARY KEY IDENTITY(1,1),
    [ship_name] NVARCHAR(100) NOT NULL,
	[ship_cost] int not null
);

CREATE TABLE [Orders] (
    [order_id] INT PRIMARY KEY IDENTITY(1,1),
    [user_id] INT NOT NULL,
    [order_date] DATETIME DEFAULT GETDATE(),
    [status] int DEFAULT 1, -- '1-Chờ duyệt', '2-Đang giao', '3-Đã nhận hàng', '0-Da huy'
    [method_id] INT NOT NULL,
    [shipper_id] INT NOT NULL,
    [shipping_address] NVARCHAR(200),
	[order_code] varchar(20),
    [is_prepaid] BIT DEFAULT 0, -- true nếu đã thanh toán trước
    FOREIGN KEY ([user_id]) REFERENCES [Users]([user_id]),
    FOREIGN KEY ([method_id]) REFERENCES [PaymentMethods]([method_id]),
    FOREIGN KEY ([shipper_id]) REFERENCES [Shippers]([ship_id])
);

ALTER TABLE [Orders]
ADD [total_price] INT DEFAULT 0;

CREATE TABLE [OrderDetails] (
    [order_detail_id] INT PRIMARY KEY IDENTITY(1,1),
    [order_id] INT NOT NULL,
    [var_id] INT NOT NULL,
    [quantity] INT NOT NULL,
    [price_at_order] INT NOT NULL,
    FOREIGN KEY ([order_id]) REFERENCES [Orders]([order_id]) ON DELETE CASCADE,
    FOREIGN KEY ([var_id]) REFERENCES [PhoneVariants]([var_id])
);

CREATE TABLE [Reviews] (
    [review_id] INT PRIMARY KEY IDENTITY(1,1),
    [user_id] INT NOT NULL,
    [phone_id] INT NOT NULL,
    [var_id] INT NOT NULL,
    [rating] INT CHECK (rating >= 1 AND rating <= 5),
    [review_text] NVARCHAR(1000),
    [review_date] DATETIME DEFAULT GETDATE(),
	[review_image_url] varchar(100),
    FOREIGN KEY ([user_id]) REFERENCES [Users]([user_id]),
    FOREIGN KEY ([phone_id]) REFERENCES [Phones]([phone_id]),
    FOREIGN KEY ([var_id]) REFERENCES [PhoneVariants]([var_id]),
); -- Chỉ 1 đánh giá mỗi sản phẩm đã mua

ALTER TABLE [OrderDetails]
ADD [is_review] BIT NOT NULL DEFAULT 0;

CREATE TABLE [Contacts] (
    [contact_id] INT IDENTITY(1,1) PRIMARY KEY,
    [reason] NVARCHAR(255) NOT NULL,         
    [order_id] INT NOT NULL,                 
    [user_id] INT NOT NULL                   
);

create table [UserProductInteractions] (
	[u_pro_inter_id] int identity(1,1) primary key,
	[user_id] int,
	[product_id] int,
	[label] bit not null default 0
);
GO

--ALTER TABLE [Phones]
--ADD [sold] int NOT NULL DEFAULT 0;

-- trigger
CREATE TRIGGER trg_AfterInsertUser_PopulateUserProductInteractions
ON [Users]
AFTER INSERT
AS
BEGIN
    -- Kiểm tra xem có dữ liệu nào được insert không
    IF EXISTS (SELECT 1 FROM inserted)
    BEGIN
        -- Lấy user_id của user mới được tạo
        DECLARE @new_user_id INT;
        SELECT @new_user_id = [user_id] FROM inserted;

        -- Insert nhiều dòng vào bảng UserProductInteractions
        -- Lấy user_id vừa tạo và tất cả phone_id từ bảng Phones
        INSERT INTO [UserProductInteractions] ([user_id], [product_id])
        SELECT @new_user_id, [phone_id]
        FROM [Phones];
    END
END;
GO

CREATE TRIGGER trg_AfterInsertPhone_PopulateUserProductInteractions
ON [Phones]
AFTER INSERT
AS
BEGIN
    -- Kiểm tra xem có dữ liệu nào được insert không
    IF EXISTS (SELECT 1 FROM inserted)
    BEGIN
        -- Lấy phone_id của điện thoại mới được tạo
        DECLARE @new_phone_id INT;
        SELECT @new_phone_id = [phone_id] FROM inserted;

        -- Insert nhiều dòng vào bảng UserProductInteractions
        -- Lấy phone_id vừa tạo và tất cả user_id từ bảng Users
        -- với điều kiện user_role = 1 (user thường) và is_verify = true
        INSERT INTO [UserProductInteractions] ([user_id], [product_id])
        SELECT [user_id], @new_phone_id
        FROM [Users]
        WHERE [user_role] = 1;
    END
END;
GO