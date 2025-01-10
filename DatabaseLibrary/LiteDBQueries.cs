using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseLibrary.Models;
using LiteDB;

namespace DatabaseLibrary;

public static class LiteDbQueries {
	public static int AddItem(this LiteDbService service, Item item) {
		ILiteCollection<Item>? collection = service.Database.GetCollection<Item>("Items");
		return collection.Insert(item);
	}

	public static bool UpdateItem(this LiteDbService service, Item item) {
		ILiteCollection<Item>? collection = service.Database.GetCollection<Item>("Items");
		return collection.Update(item);
	}

	public static bool DeleteItem(this LiteDbService service, int itemId) {
		ILiteCollection<Item>? collection = service.Database.GetCollection<Item>("Items");
		return collection.Delete(itemId);
	}

	public static Item? GetItemById(this LiteDbService service, int itemId) {
		ILiteCollection<Item>? collection = service.Database.GetCollection<Item>("Items");
		return collection.FindById(itemId);
	}

	public static List<Item> GetItems(this LiteDbService service) {
		ILiteCollection<Item>? collection = service.Database.GetCollection<Item>("Items");
		return collection.FindAll().ToList();
	}

	// CRUD for Order
	public static int AddOrder(this LiteDbService service, Order order) {
		if (service.Orders.Exists(o => o.OrderNumber == order.OrderNumber)) throw new InvalidOperationException(service.Localizer["OrderAlreadyExists", order.OrderNumber]);
		ILiteCollection<Order>? collection = service.Database.GetCollection<Order>("Orders");
		order.DateCreated = DateTime.Now;
		return collection.Insert(order);
	}

	public static bool UpdateOrder(this LiteDbService service, Order order) {
		ILiteCollection<Order>? collection = service.Database.GetCollection<Order>("Orders");
		return collection.Update(order);
	}

	public static bool DeleteOrder(this LiteDbService service, int orderId) {
		ILiteCollection<Order>? collection = service.Database.GetCollection<Order>("Orders");
		return collection.Delete(orderId);
	}

	public static Order? GetOrderById(this LiteDbService service, int orderId) {
		ILiteCollection<Order>? collection = service.Database.GetCollection<Order>("Orders");
		return collection.FindById(orderId);
	}

	public static Order? GetOrderByOrderNumber(this LiteDbService service, int orderNumber) {
		ILiteCollection<Order>? collection = service.Database.GetCollection<Order>("Orders");
		return collection.FindOne(x => x.OrderNumber == orderNumber);
	}

	public static List<Order> GetOrders(this LiteDbService service) {
		ILiteCollection<Order>? collection = service.Database.GetCollection<Order>("Orders");
		return collection.FindAll().ToList();
	}

	// CRUD for OrderItem
	public static int AddOrderItem(this LiteDbService service, OrderItem orderItem) {
		ILiteCollection<OrderItem>? collection = service.Database.GetCollection<OrderItem>("OrderItems");
		return collection.Insert(orderItem);
	}

	public static bool UpdateOrderItem(this LiteDbService service, OrderItem orderItem) {
		ILiteCollection<OrderItem>? collection = service.Database.GetCollection<OrderItem>("OrderItems");
		return collection.Update(orderItem);
	}

	public static bool DeleteOrderItem(this LiteDbService service, int orderItemId) {
		ILiteCollection<OrderItem>? collection = service.Database.GetCollection<OrderItem>("OrderItems");
		return collection.Delete(orderItemId);
	}

	public static OrderItem? GetOrderItemById(this LiteDbService service, int orderItemId) {
		ILiteCollection<OrderItem>? collection = service.Database.GetCollection<OrderItem>("OrderItems");
		return collection.FindById(orderItemId);
	}
}
