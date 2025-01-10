using System;
using System.IO;
using System.Reflection;
using DatabaseLibrary.Models;
using Languages;
using LiteDB;
using Microsoft.Extensions.Localization;

namespace DatabaseLibrary;

public sealed class LiteDbService : IDisposable {
	private readonly LiteDatabase _database;

	public string DatabaseFile { get; }

	public IStringLocalizer Localizer { get; }

	public ILiteCollection<Item> Items => _database.GetCollection<Item>("Items");

	public ILiteCollection<Order> Orders => _database.GetCollection<Order>("Orders");

	public ILiteCollection<OrderItem> OrderItems => _database.GetCollection<OrderItem>("OrderItems");

	public LiteDbService(IStringLocalizer<OrderManagerAppLanguages> localizer, string dbFile) {
		_database = new LiteDatabase(dbFile);
		DatabaseFile = dbFile;
		Localizer = localizer;
	}

	public LiteDbService(IStringLocalizer<OrderManagerAppLanguages> localizer) {
		string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		string file = Assembly.GetExecutingAssembly().GetName().Name + ".db";
		DatabaseFile = Path.Combine(appData, file);
		_database = new LiteDatabase(DatabaseFile);
		Localizer = localizer;
	}

	public void Dispose() => _database.Dispose();
}
