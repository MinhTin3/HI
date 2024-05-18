create database WebFPTShop
go
use WebFPTShop
go
create table Category(
	IDCate int identity(1,1) primary key,
	NameCate nvarchar(100) not null,
	CateIcon text
);
go
create table Manufacturer(
  IDManu int identity(1,1) primary key,
  NameManu nvarchar(100) not null 
);

create table Products(
ProductID int identity(1,1) primary key,
NamePro nvarchar(100) not null,
DescriptionPro ntext,
IDCate int foreign key references Category(IDCate) on delete cascade,
IDManu int foreign key references Manufacturer(IDManu) on delete cascade,
Price bigint,
SoLuongTon int,
SoLuongBan int default 0,
Screen nvarchar(255),
Camera nvarchar(255),
CameraSelfie nvarchar(255),
CPU nvarchar(255),
Store int,
ImagePro text
);
go
create table Customer(
IDCus int identity(1,1) primary key,
NameCus nvarchar(100),
VocativeCus nvarchar(5),
PhoneCus varchar(20),
EmailCus varchar(100),
AddressCus nvarchar(max),
PassCus nvarchar(255)
);
go
create table Rating(
IDRat int identity(1,1) primary key,
IDCus int foreign key references Customer(IdCus) on delete cascade,
IDPro int foreign key references Products(ProductID) on delete cascade,
Start int,
Content text
);
create table AdminUser(
ID int identity(1,1) primary key,
NameUser nvarchar(50) not null,
RoleUser bit default 0,
PasswordUser nvarchar(100) not null
);
go

create table OrderPro(
ID int identity(1,1) primary key,
DateOrder datetime default getdate(),
IDCus int foreign key references Customer(IdCus) on delete cascade,
AddressDeliverry nvarchar(200)
);
go
create table OrderDetails(
ID int identity(1,1) primary key,
IDProduct int foreign key references Products(ProductID) on delete cascade,
IDOrder int foreign key references OrderPro(ID) on delete cascade,
Quantity int not null,
UnitPrice bigint not null
);
go

CREATE TRIGGER UpdateProductQuantity
ON OrderDetails
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ProductID INT;
    DECLARE @Quantity INT;

    -- Assuming the Quantity 
    SELECT @ProductID = inserted.IDProduct, @Quantity = inserted.Quantity
    FROM inserted;

    -- Update product quantity 
    UPDATE Products
    SET SoLuongTon = SoLuongTon - @Quantity,
        SoLuongBan = SoLuongBan + @Quantity
    WHERE ProductID = @ProductID;
END;
GO

