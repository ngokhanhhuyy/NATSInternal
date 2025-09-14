using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Encodings.Web;
using Microsoft.EntityFrameworkCore;

internal class Program
{
	private static JsonSerializerOptions _options;
	private static AppDbContext _context;

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
		Brand brand = new("Apple Inc.");
		_context.Brands.Add(brand);

		Product product = new("iPhone 17 Pro Max", brand);
		_context.Products.Add(product);

		await _context.SaveChangesAsync();
	}

	public static async Task QueryData()
	{
		Product? product = await _context.Products
			.Include(p => p.Brand)
			.FirstOrDefaultAsync();

		Console.WriteLine(JsonSerializer.Serialize(product, _options));
	}

	internal class AppDbContext : DbContext
	{
		public DbSet<Product> Products { get; set; }
		public DbSet<Brand> Brands { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Brand>(e =>
			{
				e.HasKey(b => b.Id);
				e.Property(b => b.Name).IsRequired();
			});

			builder.Entity<Product>(e =>
			{
				e.HasKey(p => p.Id);
				e.Property(p => p.Name).IsRequired();
				e.HasOne<Brand>(p => p.Brand).WithMany().HasForeignKey(p => p.BrandId);
				e.Metadata.FindNavigation(nameof(Product.Brand))!
					.SetPropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);
			});
		}

		protected override void OnConfiguring(DbContextOptionsBuilder builder)
		{
			base.OnConfiguring(builder);
			builder.UseSqlite("Data Source=:memory:");
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
}