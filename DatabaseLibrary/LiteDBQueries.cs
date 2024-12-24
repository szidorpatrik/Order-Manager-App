using DatabaseLibrary.Models;

namespace DatabaseLibrary {
	using System;

	public static class LiteDbQueries {
		// CRUD for Item
		public static int AddItem(this LiteDbService service, Item item) {
			var collection = service.GetDatabase().GetCollection<Item>("Items");
			return collection.Insert(item);
		}

		public static bool UpdateItem(this LiteDbService service, Item item) {
			var collection = service.GetDatabase().GetCollection<Item>("Items");
			return collection.Update(item);
		}

		public static bool DeleteItem(this LiteDbService service, int itemId) {
			var collection = service.GetDatabase().GetCollection<Item>("Items");
			return collection.Delete(itemId);
		}

		public static Item? GetItemById(this LiteDbService service, int itemId) {
			var collection = service.GetDatabase().GetCollection<Item>("Items");
			return collection.FindById(itemId);
		}

		// CRUD for Order
		public static int AddOrder(this LiteDbService service, Order order) {
			if (service.Orders.Exists(o => o.OrderNumber == order.OrderNumber))
			{
				throw new InvalidOperationException($"{order} already exists.");
			}
			var collection = service.GetDatabase().GetCollection<Order>("Orders");
			return collection.Insert(order);
		}

		public static bool UpdateOrder(this LiteDbService service, Order order) {
			var collection = service.GetDatabase().GetCollection<Order>("Orders");
			return collection.Update(order);
		}

		public static bool DeleteOrder(this LiteDbService service, int orderId) {
			var collection = service.GetDatabase().GetCollection<Order>("Orders");
			return collection.Delete(orderId);
		}

		public static Order? GetOrderById(this LiteDbService service, int orderId) {
			var collection = service.GetDatabase().GetCollection<Order>("Orders");
			return collection.FindById(orderId);
		}

		public static Order? GetOrderByOrderNumber(this LiteDbService service, int orderNumber) {
			var collection = service.GetDatabase().GetCollection<Order>("Orders");
			return collection.FindOne(x => x.OrderNumber == orderNumber);
		}

		// CRUD for OrderItem
		public static int AddOrderItem(this LiteDbService service, OrderItem orderItem) {
			var collection = service.GetDatabase().GetCollection<OrderItem>("OrderItems");
			return collection.Insert(orderItem);
		}

		public static bool UpdateOrderItem(this LiteDbService service, OrderItem orderItem) {
			var collection = service.GetDatabase().GetCollection<OrderItem>("OrderItems");
			return collection.Update(orderItem);
		}

		public static bool DeleteOrderItem(this LiteDbService service, int orderItemId) {
			var collection = service.GetDatabase().GetCollection<OrderItem>("OrderItems");
			return collection.Delete(orderItemId);
		}

		public static OrderItem? GetOrderItemById(this LiteDbService service, int orderItemId) {
			var collection = service.GetDatabase().GetCollection<OrderItem>("OrderItems");
			return collection.FindById(orderItemId);
		}
	}
}
