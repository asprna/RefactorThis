using Domain;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTest.Helper
{
	public static class SeedTestData
	{
		public static readonly List<Product> Products = new List<Product>
		{
			new Product
			{
				Id = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"),
				Name = "Samsung Galaxy S7",
				Description = "Newest mobile product from Samsung.",
				Price = 1024.99M,
				DeliveryPrice = 16.99M
			},
			new Product
			{
				Id = Guid.Parse("DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3"),
				Name = "Apple iPhone 6S",
				Description = "Newest mobile product from Apple.",
				Price = 1299.99M,
				DeliveryPrice = 15.99M
			}
		};

		public static readonly List<ProductOption> ProductOptions = new List<ProductOption>
		{
			new ProductOption
			{
				Id = Guid.Parse("0643CCF0-AB00-4862-B3C5-40E2731ABCC9"),
				ProductId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"),
				Name = "White",
				Description = "White Samsung Galaxy S7"
			},
			new ProductOption
			{
				Id = Guid.Parse("A21D5777-A655-4020-B431-624BB331E9A2"),
				ProductId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"),
				Name = "Black",
				Description = "Black Samsung Galaxy S7"
			},
			new ProductOption
			{
				Id = Guid.Parse("5C2996AB-54AD-4999-92D2-89245682D534"),
				ProductId = Guid.Parse("DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3"),
				Name = "Rose Gold",
				Description = "Gold Apple iPhone 6S"
			},
			new ProductOption
			{
				Id = Guid.Parse("9AE6F477-A010-4EC9-B6A8-92A85D6C5F03"),
				ProductId = Guid.Parse("DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3"),
				Name = "White",
				Description = "White Apple iPhone 6S"
			},
			new ProductOption
			{
				Id = Guid.Parse("4E2BC5F2-699A-4C42-802E-CE4B4D2AC0EF"),
				ProductId = Guid.Parse("DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3"),
				Name = "Black",
				Description = "Black Apple iPhone 6S"
			}
		};

		public static void InitializeDbForTests(DataContext context)
		{
			context.Products.AddRange(Products);
			context.ProductOptions.AddRange(ProductOptions);
			context.SaveChanges();
		}

		public static void ReinitializeDbForTests(DataContext context)
		{
			List<ProductOption> existingProductOptions = context.ProductOptions.ToList();
			List<Product> existingProducts = context.Products.ToList();

			context.ProductOptions.RemoveRange(existingProductOptions);
			context.Products.RemoveRange(existingProducts);

			InitializeDbForTests(context);
		}
	}
}
