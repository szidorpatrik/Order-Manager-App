using System;
using System.IO;
using System.Linq.Expressions;
using DatabaseLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseLibrary;

public sealed class Context : DbContext {
	private const string FileName = "OrderManagerApp.db3";

	public DbSet<Item> Items { get; set; }

	public DbSet<OrderItem> OrderItems { get; set; }

	public DbSet<Order> Orders { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
		if (optionsBuilder.IsConfigured)
			return;

		string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), FileName);
		optionsBuilder.UseSqlite($"Filename={path}");

#if DEBUG
		optionsBuilder.EnableSensitiveDataLogging();
#endif
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		modelBuilder.Entity<Item>(items => {
			items.HasKey(x => x.Id);
			items.Property(x => x.Name).IsRequired().HasMaxLength(255);
			items.Property(x => x.Price).IsRequired();
			items.HasNotNegative(x => x.Price);
		});

		modelBuilder.Entity<OrderItem>(orderItems => {
			orderItems.HasKey(x => x.Id);
			orderItems.HasOne(x => x.Item);
			orderItems.Property(x => x.Quantity).IsRequired().HasPrecision(2);
			orderItems.HasNotNegative(x => x.Quantity);
		});

		modelBuilder.Entity<Order>(orders => {
			orders.HasKey(x => x.Id);
			orders.Property(x => x.DateCreated).IsRequired();
			orders.Property(x => x.DateStart).IsRequired();
			orders.Property(x => x.DateEnd).IsRequired();
			orders.Property(x => x.OrderNumber).IsRequired();
			orders.HasMany(x => x.OrderItems);
		});
	}
}

file static class ContextExtensions {
	public static EntityTypeBuilder<TEntity> HasNotNegative<TEntity, TProperty>(
		this EntityTypeBuilder<TEntity> entityTypeBuilder,
		Expression<Func<TEntity, TProperty>> propertyExpression
	) where TEntity : class {
		string name = propertyExpression.GetMemberAccess().Name;
		return entityTypeBuilder.ToTable(x => x.HasCheckConstraint($"CK_{nameof(TEntity)}_{name}", $"{name} >= 0"));
	}
}
