using DatabaseLibrary.Models;

namespace DatabaseLibrary.Tests;

using System.Collections;

internal static class DummyDb {
	internal static double[] GetItemPrices() {
		return [4.99, 8.99, 9.99, 14.99, 19.99, 24.99, 49.99];
	}

	internal static IEnumerable<Order> GetOrders() {
		double[] itemPrices = GetItemPrices();

		Order order1 = new() {
			OrderNumber = 10,
			DateCreated = DateTime.Now,
			DateStart = DateTime.Now,
			DateEnd = DateTime.Now.AddHours(1),
			IsCompleted = true,
			IsCanceled = false,
			OrderItems = [
				new OrderItem() {
					Id = 1,
					Quantity = 1,
					Item = new Item() {
						Id = 1,
						Name = "Test Item",
						Price = itemPrices[0],
					},
				}
			]
		};

		Order order2 = new() {
			OrderNumber = 11,
			DateCreated = DateTime.Now,
			DateStart = DateTime.Now,
			DateEnd = DateTime.Now.AddHours(1),
			IsCompleted = true,
			IsCanceled = false,
			OrderItems = [
				new OrderItem() {
					Id = 2,
					Quantity = 1,
					Item = new Item() {
						Id = 2,
						Name = "Test Item",
						Price = itemPrices[1],
					}
				}
			]
		};

		Order order3 = new() {
			OrderNumber = 12,
			DateCreated = DateTime.Now,
			DateStart = DateTime.Now,
			DateEnd = DateTime.Now.AddHours(1),
			IsCompleted = true,
			IsCanceled = false,
			OrderItems = [
				new OrderItem() {
					Id = 3,
					Quantity = 1,
					Item = new Item() {
						Id = 3,
						Name = "Test Item",
						Price = itemPrices[2]
					}
				}
			]
		};

		Order orderCanceled1 = new() {
			OrderNumber = 20,
			DateCreated = DateTime.Now,
			DateStart = DateTime.Now,
			DateEnd = DateTime.Now.AddHours(1),
			IsCompleted = false,
			IsCanceled = true,
			OrderItems = [
				new OrderItem() {
					Id = 4,
					Quantity = 1,
					Item = new Item() {
						Id = 4,
						Name = "Test Item",
						Price = itemPrices[3],
					}
				}
			]
		};

		Order orderCanceled2 = new() {
			OrderNumber = 21,
			DateCreated = DateTime.Now,
			DateStart = DateTime.Now,
			DateEnd = DateTime.Now.AddHours(1),
			IsCompleted = false,
			IsCanceled = true,
			OrderItems = [
				new OrderItem() {
					Id = 5,
					Quantity = 1,
					Item = new Item() {
						Id = 5,
						Name = "Test Item",
						Price = itemPrices[4],
					}
				}
			]
		};

		Order orderPending1 = new() {
			OrderNumber = 30,
			DateCreated = DateTime.Now,
			DateStart = DateTime.Now,
			DateEnd = DateTime.Now.AddHours(1),
			IsCompleted = false,
			IsCanceled = false,
			OrderItems = [
				new OrderItem() {
					Id = 6,
					Quantity = 1,
					Item = new Item() {
						Id = 6,
						Name = "Test Item",
						Price = itemPrices[5],
					}
				}
			]
		};

		Order orderPending2 = new() {
			OrderNumber = 31,
			DateCreated = DateTime.Now,
			DateStart = DateTime.Now,
			DateEnd = DateTime.Now.AddHours(1),
			IsCompleted = false,
			IsCanceled = false,
			OrderItems = [
				new OrderItem() {
					Id = 7,
					Quantity = 1,
					Item = new Item() {
						Id = 7,
						Name = "Test Item",
						Price = itemPrices[6],
					}
				}
			]
		};

		return [order1, order2, order3, orderCanceled1, orderCanceled2, orderPending1, orderPending2];
	}
}
