using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseLibrary.Models;
using LiteDB;

namespace DatabaseLibrary;

public static class LiteDbQueries {
	// ---- Item ---- //

	public static int AddItem(this LiteDbService service, Item item) => service.Items.Insert(item);

	public static bool UpdateItem(this LiteDbService service, Item item) => service.Items.Update(item);

	public static bool DeleteItem(this LiteDbService service, int itemId) => service.Items.Delete(itemId);

	public static Item? GetItemById(this LiteDbService service, int itemId) => service.Items.FindById(itemId);

	public static List<Item> GetItems(this LiteDbService service) => service.Items.FindAll().ToList();

	// ---- Order ---- //

	public static int AddOrder(this LiteDbService service, Order order) {
		if (service.Orders.Exists(o => o.OrderNumber == order.OrderNumber))
			throw new InvalidOperationException(service.Localizer["OrderAlreadyExists", order.OrderNumber]);
		ILiteCollection<Order> collection = service.Orders;
		order.DateCreated = DateTime.Now;
		return collection.Insert(order);
	}

	public static bool UpdateOrder(this LiteDbService service, Order order) => service.Orders.Update(order);

	public static bool DeleteOrder(this LiteDbService service, int orderId) => service.Orders.Delete(orderId);

	public static Order? GetOrderById(this LiteDbService service, int orderId) => service.Orders.FindById(orderId);

	public static Order? GetOrderByOrderNumber(this LiteDbService service, int orderNumber) => service.Orders.FindOne(x => x.OrderNumber == orderNumber);

	public static List<Order> GetOrders(this LiteDbService service) => service.Orders.FindAll().ToList();

	// ---- OrderItem ---- //

	public static int AddOrderItem(this LiteDbService service, OrderItem orderItem) => service.OrderItems.Insert(orderItem);

	public static bool UpdateOrderItem(this LiteDbService service, OrderItem orderItem) => service.OrderItems.Update(orderItem);

	public static bool DeleteOrderItem(this LiteDbService service, int orderItemId) => service.OrderItems.Delete(orderItemId);

	public static OrderItem? GetOrderItemById(this LiteDbService service, int orderItemId) => service.OrderItems.FindById(orderItemId);

	// ---- Other ---- //

	public static int GetOrdersCount(this LiteDbService service) => service.Orders.Count();

	public static List<Order> GetCompletedOrders(this LiteDbService service) => service.Orders.FindAll().Where(x => x.IsCompleted).ToList();

	public static List<Order> GetIncompletedOrders(this LiteDbService service) => service.Orders.FindAll().Where(x => !x.IsCompleted).ToList();

	public static List<Order> GetCanceledOrders(this LiteDbService service) => service.Orders.FindAll().Where(x => x.IsCanceled).ToList();

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
		ILiteCollection<Order> collection = service.Orders;
		IEnumerable<Order>? activeOrders = collection.FindAll().Where(x => !x.IsCanceled);

		// Filter orders by the specified month if provided
		if (month is { } date)
			activeOrders = activeOrders.Where(order => order.DateCreated.Year == date.Year && order.DateCreated.Month == date.Month);

		return (double)activeOrders
			.SelectMany(order => order.OrderItems)
			.Where(orderItem => orderItem.Quantity > 0)
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
		ILiteCollection<Order> collection = service.Orders;
		IEnumerable<Order>? completedOrders = collection.FindAll().Where(x => x.IsCompleted);

		// Filter orders by the specified month if provided
		if (month is { } date)
			completedOrders = completedOrders.Where(order => order.DateCompleted.Year == date.Year && order.DateCompleted.Month == date.Month);

		return (double)completedOrders
			.SelectMany(order => order.OrderItems)
			.Where(orderItem => orderItem.Quantity > 0)
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
		ILiteCollection<Order> collection = service.Orders;
		IEnumerable<Order>? pendingOrders = collection.FindAll().Where(x => x.IsPending);

		// Filter orders by the specified month if provided
		if (month is { } date)
			pendingOrders = pendingOrders.Where(order => order.DateCreated.Year == date.Year && order.DateCreated.Month == date.Month);

		return (double)pendingOrders
			.SelectMany(order => order.OrderItems)
			.Where(orderItem => orderItem.Quantity > 0)
			.Sum(orderItem => orderItem.Quantity * orderItem.Item.Price)!;
	}
}
