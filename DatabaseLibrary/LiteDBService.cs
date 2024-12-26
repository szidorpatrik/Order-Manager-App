using DatabaseLibrary.Models;
using Languages;
using LiteDB;
using System;
using System.IO;

namespace DatabaseLibrary {
	using Microsoft.Extensions.Localization;

	public class LiteDbService {
		private readonly string fileName = "OrderManagerApp.db";
		private readonly string _databasePath;
		private readonly LiteDatabase _database;
		public readonly IStringLocalizer Localizer;

		public LiteDbService(IStringLocalizer<OrderManagerAppLanguages> localizer, string? databasePath = null) {
			Localizer = localizer;
			_databasePath = Path.Combine(databasePath ?? Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);
			_database = new LiteDatabase(_databasePath);
		}

		public ILiteCollection<Item> Items => _database.GetCollection<Item>("Items");
		public ILiteCollection<Order> Orders => _database.GetCollection<Order>("Orders");
		public ILiteCollection<OrderItem> OrderItems => _database.GetCollection<OrderItem>("OrderItems");

		public LiteDatabase GetDatabase() => _database;
	}
}
