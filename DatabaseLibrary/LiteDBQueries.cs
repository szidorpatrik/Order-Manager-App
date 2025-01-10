using DatabaseLibrary.Models;

namespace DatabaseLibrary {
	using System;
	using System.Collections.Generic;
	using System.Linq;

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

		public static List<Item> GetItems(this LiteDbService service) {
			var collection = service.GetDatabase().GetCollection<Item>("Items");
			return collection.FindAll().ToList();
		}

		// CRUD for Order
		public static int AddOrder(this LiteDbService service, Order order) {
			if (service.Orders.Exists(o => o.OrderNumber == order.OrderNumber)) {
				throw new InvalidOperationException(service.Localizer["OrderAlreadyExists", order.OrderNumber]);
			}
			var collection = service.GetDatabase().GetCollection<Order>("Orders");
			order.DateCreated = DateTime.Now;
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

		public static List<Order> GetOrders(this LiteDbService service) {
			var collection = service.GetDatabase().GetCollection<Order>("Orders");
			return collection.FindAll().ToList();
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

		// Other
		public static int GetOrdersCount(this LiteDbService service) {
			var collection = service.GetDatabase().GetCollection<Order>("Orders");
			return collection.Count();
		}

		public static List<Order> GetCompletedOrders(this LiteDbService service) {
			var collection = service.GetDatabase().GetCollection<Order>("Orders");
			return collection.FindAll().Where(x => x.IsCompleted).ToList();
		}

		public static List<Order> GetIncompletedOrders(this LiteDbService service) {
			var collection = service.GetDatabase().GetCollection<Order>("Orders");
			return collection.FindAll().Where(x => !x.IsCompleted).ToList();
		}

		public static List<Order> GetCanceledOrders(this LiteDbService service) {
			var collection = service.GetDatabase().GetCollection<Order>("Orders");
			return collection.FindAll().Where(x => x.IsCanceled).ToList();
		}

		/// <summary>
		/// Calculates the total price of all active (non-canceled) orders in the database.
		/// Optionally filters the orders by the specified month.
		/// </summary>
		/// <param name="service">The instance of <see cref="LiteDbService"/> used to access the database.</param>
		/// <param name="month">
		/// An optional <see cref="DateTime"/> parameter to filter orders by a specific month.
		/// If not specified, calculates the total price of all active orders regardless of month.
		/// </param>
		/// <returns>
		/// A <see cref="double"/> representing the total price of all active orders for the specified month or all months.
		/// Returns 0 if there are no active orders or if the order items have invalid quantities or prices.
		/// </returns>
		public static double GetTotalPrice(this LiteDbService service, DateTime? month = null) {
			var collection = service.GetDatabase().GetCollection<Order>("Orders");
			var activeOrders = collection.FindAll().Where(x => !x.IsCanceled);

			// Filter orders by the specified month if provided
			if (month.HasValue) {
				activeOrders = activeOrders.Where(order =>
					                                  order.DateCreated.Month == month.Value.Month &&
					                                  order.DateCreated.Year == month.Value.Year);
			}

			return (double)activeOrders
				.SelectMany(order => order.OrderItems)
				.Where(orderItem => orderItem is { Item: not null, Quantity: > 0 })
				.Sum(orderItem => orderItem.Quantity * orderItem.Item.Price)!;
		}

		/// <summary>
		/// Calculates the total price of all completed orders in the database.
		/// Optionally filters the orders by the specified month.
		/// </summary>
		/// <param name="service">The instance of <see cref="LiteDbService"/> used to access the database.</param>
		/// <param name="month">
		/// An optional <see cref="DateTime"/> parameter to filter orders by a specific month.
		/// If not specified, calculates the total price of all completed orders regardless of month.
		/// </param>
		/// <returns>
		/// A <see cref="double"/> representing the total price of all completed orders for the specified month or all months.
		/// Returns 0 if there are no completed orders or if the order items have invalid quantities or prices.
		/// </returns>
		public static double GetTotalPriceOfCompleted(this LiteDbService service, DateTime? month = null) {
			var collection = service.GetDatabase().GetCollection<Order>("Orders");
			var completedOrders = collection.FindAll().Where(x => x.IsCompleted);

			// Filter orders by the specified month if provided
			if (month.HasValue) {
				completedOrders = completedOrders.Where(order =>
					                                        order.DateCompleted.Month == month.Value.Month &&
					                                        order.DateCompleted.Year == month.Value.Year);
			}

			return (double)completedOrders
				.SelectMany(order => order.OrderItems)
				.Where(orderItem => orderItem is { Item: not null, Quantity: > 0 })
				.Sum(orderItem => orderItem.Quantity * orderItem.Item.Price)!;
		}

		/// <summary>
		/// Calculates the total price of all pending orders in the database.
		/// Optionally filters the orders by the specified month.
		/// </summary>
		/// <param name="service">The instance of <see cref="LiteDbService"/> used to access the database.</param>
		/// <param name="month">
		/// An optional <see cref="DateTime"/> parameter to filter orders by a specific month.
		/// If not specified, calculates the total price of all pending orders regardless of month.
		/// </param>
		/// <returns>
		/// A <see cref="double"/> representing the total price of all pending orders for the specified month or all months.
		/// Returns 0 if there are no pending orders or if the order items have invalid quantities or prices.
		/// </returns>
		public static double GetTotalPriceOfPending(this LiteDbService service, DateTime? month = null) {
			var collection = service.GetDatabase().GetCollection<Order>("Orders");
			var pendingOrders = collection.FindAll().Where(x => x.IsPending);

			// Filter orders by the specified month if provided
			if (month.HasValue) {
				pendingOrders = pendingOrders.Where(order =>
					                                    order.DateCreated.Month == month.Value.Month &&
					                                    order.DateCreated.Year == month.Value.Year);
			}

			return (double)pendingOrders
				.SelectMany(order => order.OrderItems)
				.Where(orderItem => orderItem is { Item: not null, Quantity: > 0 })
				.Sum(orderItem => orderItem.Quantity * orderItem.Item.Price)!;
		}
	}
}
