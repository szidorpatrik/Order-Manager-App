using System;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace DatabaseLibrary;

public sealed class Context : DbContext {
	public const string FileName = "OrderManagerApp.db3";

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
		
	}
}
