using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Encodings.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

internal class Program
{
	private static readonly JsonSerializerOptions _options;
	private static readonly AppDbContext _context;

	static Program()
	{
		_options = new()
		{
			WriteIndented = true,
			Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
		};

		_context = new();
	}

	public static async Task Main()
	{
		await _context.Database.OpenConnectionAsync();
		await _context.Database.EnsureCreatedAsync();
		await GenerateData();
		await QueryData();
	}

	public static async Task GenerateData()
	{
		Random random = new();
		for (int outerIndex = 0; outerIndex < 10; outerIndex++)
		{
			Brand brand = new($"Brand {outerIndex + 1}");
			_context.Brands.Add(brand);

			Product product = new($"Product {outerIndex + 1}", brand);
			_context.Products.Add(product);

			Stock stock = new(random.Next(0, 10), product);
			_context.Stocks.Add(stock);

			bool isThumbnailPhotoAdded = false;
			for (int innerIndex = 0; innerIndex < 5; innerIndex++)
			{
				bool isThumbnail = false;
				if (!isThumbnailPhotoAdded)
				{
					isThumbnail = random.Next(0, 100) == 0;
				}

				_context.Photos.Add(new($"Photo {(innerIndex + 1) * outerIndex}", isThumbnail, product));
			}
		}

		await _context.SaveChangesAsync();
	}

	public static async Task QueryData()
	{
		Console.WriteLine("Starting querying");
		var data = await _context.Products
			.Include(p => p.Brand)
			.Select(product => new
			{
				Product = product,
				StockingQuantity = _context.Stocks.FirstOrDefault(stock => stock.ProductId == product.Id),
				ThumbnailUrl = _context.Photos
					.Where(photo => photo.ProductId == product.Id && photo.IsThumbnail)
					.Select(photo => photo.Url)
					.FirstOrDefault()
			})
			// .GroupJoin(
			// 	_context.Stocks,
			// 	product => product.Id,
			// 	stock => stock.ProductId,
			// 	(product, stocks) => new
			// 	{
			// 		Product = product,
			// 		StockingQuantity = stocks.Select(s => s.StockingQuantity).FirstOrDefault()
			// 	})
			// .GroupJoin(
			// 	_context.Photos,
			// 	productAndStockingQuantity => productAndStockingQuantity.Product.Id,
			// 	photo => photo.ProductId,
			// 	(productAndStockingQuantity, photos) => new
			// 	{
			// 		productAndStockingQuantity.Product,
			// 		productAndStockingQuantity.StockingQuantity,
			// 		ThumbnailUrl = photos.Where(p => p.IsThumbnail).Select(p => p.Url).FirstOrDefault()
			// 	}
			// )
			.ToListAsync();

		Console.WriteLine(JsonSerializer.Serialize(data, _options));
	}
}

internal class AppDbContext : DbContext
{
	public DbSet<Product> Products { get; set; }
	public DbSet<Brand> Brands { get; set; }
	public DbSet<Stock> Stocks { get; set; }
	public DbSet<Photo> Photos { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<Brand>(e =>
		{
			e.HasKey(b => b.Id);
			e.Property(b => b.Name).HasMaxLength(50).IsRequired();
		});

		builder.Entity<Product>(e =>
		{
			e.HasKey(p => p.Id);
			e.Property(p => p.Name).HasMaxLength(50).IsRequired();
			e.HasOne(p => p.Brand).WithMany().HasForeignKey(p => p.BrandId);
			e.Metadata.FindNavigation(nameof(Product.Brand))!
				.SetPropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);
		});

		builder.Entity<Stock>(e =>
		{
			e.HasKey(s => s.Id);
			e.HasOne<Product>().WithOne().HasForeignKey<Stock>(s => s.ProductId).IsRequired();
			e.HasIndex(s => s.ProductId).IsUnique();
		});

		builder.Entity<Photo>(e =>
		{
			e.HasKey(p => p.Id);
			e.HasOne<Product>().WithMany().HasForeignKey(p => p.ProductId).IsRequired();
		});
	}

	protected override void OnConfiguring(DbContextOptionsBuilder builder)
	{
		base.OnConfiguring(builder);
		builder
			.UseSqlite("Data Source=:memory:")
			.LogTo(Console.WriteLine, LogLevel.Information)
			.EnableSensitiveDataLogging();
    }
}

internal class Brand
{
#nullable disable
	private Brand() { }
#nullable enable

	public Brand(string name)
	{
		Name = name;
	}

	public Guid Id { get; private set; } = Guid.NewGuid();
	public string Name { get; private set; }
}

internal class Product
{
	private Brand? _brand;

#nullable disable
	private Product() { }
#nullable enable

	public Product(string name, Brand? brand)
	{
		Name = name;
		Brand = brand;
	}

	public Guid Id { get; private set; } = Guid.NewGuid();
	public string Name { get; private set; }
	public Guid? BrandId { get; private set; }

	public Brand? Brand
	{
		get => _brand;
		set
		{
			BrandId = value?.Id;
			_brand = value;
		}
	}
}

internal class Stock
{
#nullable disable
	private Stock() { }
#nullable enable

	public Stock(int stockingQuantity, Product product)
	{
		StockingQuantity = stockingQuantity;
		ProductId = product.Id;
	}

	public Guid Id { get; private set; } = Guid.NewGuid();
	public int StockingQuantity { get; private set; }
	public Guid ProductId { get; private set; }
}

internal class Photo
{
#nullable disable
	private Photo() { }
#nullable enable

	public Photo(string url, bool isThumbnail, Product product)
	{
		Url = url;
		IsThumbnail = isThumbnail;
		ProductId = product.Id;
	}

	public Guid Id { get; private set; } = Guid.NewGuid();
	public string Url { get; private set; }
	public bool IsThumbnail { get; private set; }
	public Guid ProductId { get; private set; }
}