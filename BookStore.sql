
drop table if exists OrderDetails
drop table if exists Orders
drop table if exists OrderStatus
drop table if exists SaleItems
drop table if exists Sales
drop table if exists StoreBooks
drop table if exists Employees
drop table if exists Stores
drop table if exists BookAuthors
drop table if exists Books
drop table if exists Genres
drop table if exists Formats
drop table if exists Authors

drop view if exists EmployeeSalesAndOrders;
drop view if exists MostSalesGenres;
drop procedure if exists CreateSale;
drop type if exists SaleItemList;
drop view if exists AuthorStatistics;
drop procedure if exists MoveBook;

create table Authors
(
	Id Int identity primary key,
	FirstName nvarchar(100) not null,
	LastName nvarchar(100) not null,
	Birth date not null
)

insert into Authors values 
('Dan', 'Brown', '1964-06-22'),('Fredrik', 'Backman', '1981-06-02'),('Anders', 'Jacobsson', '1963-08-04'),
('Sören', 'Olsson', '1964-03-16'),('Andy', 'Weir', '1972-06-16'),('Lars', 'Kepler', '2009-01-01'),
('Charles', 'Bukowski', '1920-08-16'),('Fjodor', 'Dostojevskij', '1821-10-30'),('Douglas', 'Adams', '1952-03-11')

create table Genres
(
	GenreId int identity primary key,
	Name nvarchar(100) not null
)

insert into Genres values ('Deckare'), ('Fantasy,SciFi och Skräck'), ('Skönlitteratur'), ('Barn och Ungdom')

create table Formats
(
	FormatId int identity primary key,
	Name nvarchar(100) not null
)

insert into Formats values ('Pocket'), ('Inbunden'), ('Häftad'), ('Storpocket')

create table Books
(
	ISBN13 varchar(13) not null primary key,
	Title nvarchar(100) not null,
	Language nvarchar(100) not null,
	Price decimal(10,2) not null,
	ReleaseDate date not null,
	GenreId int not null,
	FormatId int not null,
	foreign key (GenreId) references Genres(GenreId),
	foreign key (FormatId) references Formats(FormatId)
)

insert into Books values 
('9789100810184', 'Da vinci koden', 'Svenska', 60.00, '2025-03-28', 1, 1),('9789100199906', 'Den yttersta hemligheten', 'Svenska', 190.00, '2025-09-09', 1, 2),
('9789189472549', 'Berts Dagbok 1', 'Svenska', 100.00, '2023-06-29', 4, 3),('9780593135228', 'Project Hail Mary', 'Engelska', 180.00, '2022-10-04', 2, 3), 
('9780804139021', 'The Martian', 'Engelska', 240.00, '2014-02-11', 2, 2),('9789100124045', 'Hypnotisören', 'Svenska', 200.00, '2009-07-24', 1, 2),
('9789100167110', 'Spindeln', 'Svenska', 190.00, '2022-10-19', 1, 2),('9789188753939', 'Postverket', 'Svenska', 200.00, '2025-04-07', 3, 3),
('9789176456095', 'Brott och Straff. 1', 'Svenska', 210.00, '2017-03-20', 3, 2),('9789177015772', 'Brott och Straff. 2', 'Svenska', 290.00, '2017-03-20', 3, 2),
('9789176433690', 'Liftarens guide till Galaxen, del 1-5', 'Svenska', 75.00, '1997-10-03', 2, 1)

create table BookAuthors
(
	ISBN13 varchar(13) not null,
	AuthorId int not null,
	primary key (ISBN13, AuthorId),
	foreign key (ISBN13) references Books(ISBN13),
	foreign key (AuthorId) references Authors(Id)
)

insert into BookAuthors values 
('9789100810184', 1),('9789100199906', 1),('9789189472549', 3), ('9789189472549', 4),('9780593135228', 5), ('9780804139021', 5)

create table Stores
(
	StoreId int identity primary key,
	StoreName nvarchar(100) not null,
	Address nvarchar(100) not null,
	ZipCode int not null,
	City nvarchar(100) not null,
	Country nvarchar(100) not null
)

insert into Stores values ('Akademibokhandeln', 'Norra Hamngatan 26', 41106, 'Göteborg', 'Sverige')
insert into Stores values ('Akademibokhandeln', 'Kungsgatan 44', 41115, 'Göteborg', 'Sverige')
insert into Stores values ('Akademibokhandeln', 'Mäster Samuelsgatan 28', 11157, 'Stockholm', 'Sverige')
insert into Stores values ('Akademibokhandeln', 'Norrtullsgatan 8', 11329, 'Stockholm', 'Sverige')
insert into Stores values ('Akademibokhandeln', 'Hyllie Boulevard 19', 21532, 'Malmö', 'Sverige')
insert into Stores values ('Akademibokhandeln', 'Rådmansgatan 12B', 21146, 'Malmö', 'Sverige')

create table Employees
(
	EmployeeId int identity primary key,
	FirstName nvarchar(100) not null,
	LastName nvarchar(100) not null,
	BirthDate date not null,
	HireDate date not null,
	StoreId int not null,
	foreign key (StoreId) references Stores(StoreId)
)

INSERT INTO Employees (FirstName, LastName, BirthDate, HireDate, StoreId)
VALUES 
('Anna', 'Svensson', '1985-03-12', '2010-05-01', 1),('Björn', 'Johansson', '1990-07-21', '2015-08-15', 1),
('Carina', 'Karlsson', '1982-11-03', '2008-02-10', 1),('David', 'Lind', '1995-01-17', '2020-06-01', 1),
('Elin', 'Andersson', '1988-09-29', '2012-03-12', 1),('Tom', 'Kurva', '1964-02-02', '2008-03-02', 1),
('Birgitta', 'Stolt', '1958-12-10', '2002-01-15', 1),('Fredrik', 'Nilsson', '1991-04-05', '2017-07-20', 2),
('Greta', 'Berg', '1987-12-18', '2013-09-01', 2),('Ellen', 'Borg', '1994-11-05', '2016-10-21', 2),
('Karin', 'Svensson', '1984-01-18', '2014-05-05', 2),('Helena', 'Olsson', '1979-02-10', '2005-05-15', 3),
('Karl', 'Hjelm', '1990-11-04', '2019-02-20', 3),('Björn', 'Torstensson', '1982-09-13', '2012-04-15', 3),
('Tor', 'Rutig', '1979-05-12', '1998-09-01', 3),('David', 'Johansson', '1995-07-29', '2018-02-01', 3),
('Elina', 'Svensson', '1993-03-10', '2020-05-09', 4),('Therese', 'Haraldsson', '1987-05-05', '2009-03-15', 4),
('Birger', 'Stolt', '1972-07-08', '1993-04-21', 4),('Frida', 'Arvidsson', '1992-10-05', '2015-10-01', 4),
('Ellen', 'Sundin', '1989-02-05', '2010-11-01', 4),('Fatima', 'Fatme', '1996-11-21', '2008-01-15', 5),
('Roger', 'Svensson', '1961-05-18', '1992-06-06', 5),('Bo', 'Ohlsson', '1979-08-06', '1999-01-14', 6),
('Gun', 'Johansson', '1958-08-12', '1984-07-02', 6);

create table StoreBooks
(
	StoreId int not null,
	ISBN13 varchar(13) not null,
	QuantityInStock int not null default 0,
	primary key (StoreId, ISBN13),
	foreign key (StoreId) references Stores(StoreId),
	foreign key (ISBN13) references Books(ISBN13)
)

insert into StoreBooks values 
(1, '9789100810184', 10),(2, '9789100810184', 10),(3, '9789100810184', 10),(4, '9789100810184', 10),
(5, '9789100810184', 10),(6, '9789100810184', 10),(1, '9789100199906', 10),(2, '9789100199906', 10),
(3, '9789100199906', 10),(4, '9789100199906', 10),(5, '9789100199906', 10),(6, '9789100199906', 10),
(1, '9789189472549', 10),(2, '9789189472549', 10),(3, '9789189472549', 10),(4, '9789189472549', 10),
(5, '9789189472549', 10),(6, '9789189472549', 11),(1, '9780593135228', 10),(2, '9780593135228', 10),
(3, '9780593135228', 10),(4, '9780593135228', 10),(5, '9780593135228', 10),(6, '9780593135228', 10),
(1, '9780804139021', 10),(2, '9780804139021', 10),(3, '9780804139021', 10),(4, '9780804139021', 10),
(5, '9780804139021', 10),(6, '9780804139021', 10),(1, '9789100124045', 10),(2, '9789100124045', 10),
(3, '9789100124045', 10),(4, '9789100124045', 10),(5, '9789100124045', 10),(6, '9789100124045', 10),
(1, '9789100167110', 10),(2, '9789100167110', 10),(3, '9789100167110', 10),(4, '9789100167110', 10),
(5, '9789100167110', 10),(6, '9789100167110', 10),(1, '9789188753939', 10),(2, '9789188753939', 10),
(3, '9789188753939', 10),(4, '9789188753939', 10),(5, '9789188753939', 10),(6, '9789188753939', 10),
(1, '9789176456095', 10),(2, '9789176456095', 10),(3, '9789176456095', 10),(4, '9789176456095', 10),
(5, '9789176456095', 10),(6, '9789176456095', 10),(1, '9789177015772', 10),(2, '9789177015772', 10),
(3, '9789177015772', 10),(4, '9789177015772', 10),(5, '9789177015772', 10),(6, '9789177015772', 10),
(1, '9789176433690', 10),(2, '9789176433690', 10),(3, '9789176433690', 10),(4, '9789176433690', 10),
(5, '9789176433690', 10),(6, '9789176433690', 10)

create table Sales
(
	SaleId int identity primary key,
	StoreId int not null,
	EmployeeId int not null,
	SaleDateTime datetime2 default getdate(),
	foreign key (StoreId) references Stores(StoreId),
	foreign key (EmployeeId) references Employees(EmployeeId)
)

insert into Sales (StoreId, EmployeeId) values 
(1, 1)

create table SaleItems
(
	SaleId int not null,
	ISBN13 varchar(13) not null,
	Quantity int default 1 not null,
	SalePrice decimal(10,2) not null,
	primary key (SaleId, ISBN13),
	foreign key (SaleId) references Sales(SaleId),
	foreign key (ISBN13) references Books(ISBN13)
)

insert into SaleItems(SaleId, ISBN13, Quantity, SalePrice) values 
(1, '9789100810184', 1, 80.00)

create table OrderStatus
(
	StatusId int identity primary key,
	StatusName nvarchar(100) not null
)

insert into OrderStatus values ('Inväntas'), ('Skickat'), ('Mottaget')

create table Orders
(
	OrderId int identity primary key,
	DestinationStoreId int not null,
	OrderingEmployeeId int default null,
	OrderDate datetime2 default null,
	SenderStoreId int default null,
	SenderEmployeeId int default null,
	ShippedDate datetime2 default null,
	ReceivedEmployeeId int default null,
	ReceivedDate datetime2 default null,
	StatusId int default 1,
	OrderType nvarchar(100) not null default 'FrånUtgivare',
	foreign key (DestinationStoreId) references Stores(StoreId),
	foreign key (SenderStoreId) references Stores(StoreId),
	foreign key (OrderingEmployeeId) references Employees(EmployeeId),
	foreign key (SenderEmployeeId) references Employees(EmployeeId),
	foreign key (ReceivedEmployeeId) references Employees(EmployeeId),
	foreign key (StatusId) references OrderStatus(StatusId)
)

insert into Orders (DestinationStoreId, OrderingEmployeeId, OrderDate) values (1, 1, getdate())

create table OrderDetails
(
	OrderId int not null,
	ISBN13 varchar(13) not null,
	UnitPrice decimal(10,2) not null,
	Quantity int not null,
	primary key (OrderId, ISBN13),
	foreign key (OrderId) references Orders(OrderId),
	foreign key (ISBN13) references Books(ISBN13)
)

insert into OrderDetails values (1, '9789100199906', 200.00, 40)

select * from Authors
select * from Genres
select * from Formats
select * from Books
select * from BookAuthors
select * from Stores
select * from StoreBooks
select * from Employees
select * from Sales
select * from SaleItems
select * from OrderStatus
select * from Orders
select * from OrderDetails


go
create view AuthorStatistics as
	select
		concat(a.FirstName, ' ', a.LastName) as [Name],
		concat(datediff(year, a.Birth, getdate()), ' år') as [Age],
		concat(count(distinct ba.ISBN13), ' st') as [Titles],
		concat(sum(sb.QuantityInStock * b.Price), ' kr') as [TotalInventoryValue]
	from
		Authors a
		join BookAuthors ba on ba.AuthorId = a.Id
		join Books b on b.ISBN13 = ba.ISBN13
		join StoreBooks sb on sb.ISBN13 = ba.ISBN13
	group by
		a.FirstName,
		a.LastName,
		a.Birth;
go

select * from AuthorStatistics

go
create procedure MoveBook 
	@FromStore int,
	@ToStore int,
	@BookISBN varchar(13),
	@Quantity int = 1,
	@SenderEmployee int
as
begin
	set nocount on
	begin try
		begin tran

			if not exists (select 1 from Employees where EmployeeId = @SenderEmployee)
			begin;
				throw 50001, 'Måste ange korrekt arbetares ID.', 1
			end

			if (@Quantity <= 0)
			begin;
				throw 50002, 'Antalet böcker måste vara större än 0.', 1
			end

			if (@FromStore = @ToStore)
			begin;
				throw 50003, 'Avsändare och mottagare kan inte vara samma butik.', 1
			end

			if not exists (select 1 from Stores where StoreId = @FromStore)
			begin;
				throw 50010, 'Avsändarens butiksID finns inte.', 1
			end

			if not exists (select 1 from Stores where StoreId = @ToStore)
			begin;
				throw 50011, 'Mottagarens butiksID finns inte.', 1
			end

			if not exists (select 1 from Books where ISBN13 = @BookISBN)
			begin;
				throw 50004, 'Bokens ISBN finns inte i databasen.', 1
			end

			if not exists (select 1 from StoreBooks where ISBN13 = @BookISBN and StoreId = @FromStore)
			begin;
				throw 50005, 'Vald bok finns inte avsändande butik.', 1
			end

			if (select QuantityInStock from StoreBooks where ISBN13 = @BookISBN and StoreId = @FromStore) - @Quantity < 0
			begin;
				throw 50006, 'Avsändande butik har inte tillräckligt med exemplar.', 1
			end

			insert into 
				Orders (DestinationStoreId, SenderStoreId, SenderEmployeeId, ShippedDate, ReceivedDate, StatusId, OrderType) 
			values (@ToStore, @FromStore, @SenderEmployee, getdate(), getdate(), 3, 'FlyttaBok')

			declare @OrderId int = scope_identity()

			insert into 
				OrderDetails (OrderId, ISBN13, UnitPrice, Quantity)
			select
				@OrderId, @BookISBN, Price, @Quantity
			from
				Books
			where
				Books.ISBN13 = @BookISBN

			update StoreBooks
			set QuantityInStock = QuantityInStock - @Quantity
			where StoreId = @FromStore and ISBN13 = @BookISBN

			update StoreBooks
			set QuantityInStock = QuantityInStock + @Quantity
			where StoreId = @ToStore and ISBN13 = @BookISBN

		commit
	end try
	begin catch;
		rollback;
		throw;
	end catch
end

exec MoveBook
	@FromStore = 1,
	@ToStore = 2,
	@BookISBN = '9789100199906',
	@Quantity = 10,
	@SenderEmployee = 4


go

create type SaleItemList as table
(
	ISBN13 varchar(13),
	Quantity int
)

go

create proc CreateSale
(
	@StoreId int,
	@EmployeeId int,
	@Items SaleItemList readonly
) as
begin;
	set nocount on

	begin try
		begin tran
			
			if exists (
				select 
					1 
				from 
					@Items i 
					left join Books b on b.ISBN13 = i.ISBN13 
				where 
					b.ISBN13 is null
			)
			begin;
				throw 51000, 'Någon eller några ISBN stämmer inte.', 1
			end

			if exists (
				select
					1
				from
					@Items i
					join StoreBooks sb on sb.ISBN13 = i.ISBN13 and sb.StoreId = @StoreId
				where
					sb.QuantityInStock < i.Quantity
			)
			begin;
				throw 51001, 'Butiken har inte tillräckligt med exemplar.', 1
			end

			insert into Sales (StoreId, EmployeeId) values 
			(@StoreId, @EmployeeId)

			declare @SaleId int = scope_identity()

			insert into SaleItems(SaleId, ISBN13, Quantity, SalePrice)
			select 
				@SaleId, i.ISBN13, i.Quantity, b.Price * 1.3
			from 
				@Items i
				join Books b on b.ISBN13 = i.ISBN13

			update sb
			set sb.QuantityInStock = sb.QuantityInStock - i.Quantity
			from
				StoreBooks sb
				join @Items i on i.ISBN13 = sb.ISBN13
			where
				sb.StoreId = @StoreId

		commit
	end try
	begin catch
		rollback;
		throw;
	end catch
end

go

declare @items SaleItemList

insert into @items values
('9789189472549', 2), ('9789188753939', 1)

exec CreateSale
	@StoreId = 6,
	@EmployeeId = 24,
	@Items = @Items

go

create view MostSalesGenres as
	select
		g.Name as [Genre],
		count(distinct b.ISBN13) as [# of books],
		sum(isnull(si.Quantity, 0)) as [# of sales]
	from
		Genres g
		left join Books b on b.GenreId = g.GenreId
		left join SaleItems si on si.ISBN13 = b.ISBN13
	group by
		g.Name

go

select * from MostSalesGenres order by [# of sales] desc

go

create view EmployeeSalesAndOrders as
	select
		e.EmployeeId as [EmployeeId],
		concat(e.FirstName, ' ', e.LastName) as [Name],
		s.City as [Store],
		count(distinct si.SaleId) as [# of sales],
		isnull(sum(si.Quantity), 0) as [# of sold articles],
		isnull(sum(si.SalePrice * si.Quantity), 0) as [Total sale price in swedish kr],
		count(distinct o.OrderId) as [# of orders],
		count(distinct o.SenderEmployeeId) as [# of orders send],
		count(distinct o.ReceivedEmployeeId) as [# of orders recieved]
	from
		Employees e
		join Stores s on s.StoreId = e.StoreId
		left join Sales sa on sa.EmployeeId = e.EmployeeId
		left join SaleItems si on si.SaleId = sa.SaleId
		left join Books b on b.ISBN13 = si.ISBN13
		left join Orders o on o.OrderingEmployeeId = e.EmployeeId
	group by
		e.EmployeeId,
		e.FirstName,
		e.LastName,
		s.City

go

select * from EmployeeSalesAndOrders order by [Total sale price in swedish kr] desc