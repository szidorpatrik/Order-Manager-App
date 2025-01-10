using System;
using System.IO;
using System.Reflection;
using DatabaseLibrary.Models;
using Languages;
using LiteDB;
using Microsoft.Extensions.Localization;

namespace DatabaseLibrary;

public sealed class LiteDbService : IDisposable {
	internal LiteDatabase Database { get; }

	public string DatabaseFile { get; }

	public IStringLocalizer Localizer { get; }

	public ILiteCollection<Item> Items => Database.GetCollection<Item>("Items");

	public ILiteCollection<Order> Orders => Database.GetCollection<Order>("Orders");

	public ILiteCollection<OrderItem> OrderItems => Database.GetCollection<OrderItem>("OrderItems");

	public LiteDbService(IStringLocalizer<OrderManagerAppLanguages> localizer, string dbFile) {
		Database = new LiteDatabase(dbFile);
		DatabaseFile = dbFile;
		Localizer = localizer;
	}

	public LiteDbService(IStringLocalizer<OrderManagerAppLanguages> localizer) {
		string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		string file = Assembly.GetExecutingAssembly().GetName().Name + ".db";
		DatabaseFile = Path.Combine(appData, file);
		Database = new LiteDatabase(DatabaseFile);
		Localizer = localizer;
	}

	public void Dispose() => Database.Dispose();
}
