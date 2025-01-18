using DatabaseLibrary.Models;

namespace DatabaseLibrary.Tests {
	[TestClass]
	public class OrderTests : TestSetupCleanup {
		[TestMethod]
		public void AddOrder_ShouldInsertOrderIntoDatabase() {
			Order order = new() {
				OrderNumber = 123,
				DateCreated = DateTime.Now,
				DateStart = DateTime.Now,
				DateEnd = DateTime.Now.AddHours(1)
			};

			var id = LiteDbService.AddOrder(order);

			var retrievedOrder = LiteDbService.GetOrderById(id);
			Assert.IsNotNull(retrievedOrder);
			Assert.AreEqual(123, retrievedOrder.OrderNumber);
		}

		[TestMethod]
		public void UpdateOrder_ShouldModifyExistingOrder() {
			Order order = new() {
				OrderNumber = 456,
				DateCreated = DateTime.Now,
				DateStart = DateTime.Now,
				DateEnd = DateTime.Now.AddHours(1)
			};
			var id = LiteDbService.AddOrder(order);

			order.OrderNumber = 789;
			var isUpdated = LiteDbService.UpdateOrder(order);

			Assert.IsTrue(isUpdated);
			var updatedOrder = LiteDbService.GetOrderById(id);
			Assert.AreEqual(789, updatedOrder.OrderNumber);
		}

		[TestMethod]
		public void DeleteOrder_ShouldRemoveOrderFromDatabase() {
			Order order = new() {
				OrderNumber = 999,
				DateCreated = DateTime.Now,
				DateStart = DateTime.Now,
				DateEnd = DateTime.Now.AddHours(1)
			};

			var id = LiteDbService.AddOrder(order);
			var isDeleted = LiteDbService.DeleteOrder(id);

			Assert.IsTrue(isDeleted);
			var deletedOrder = LiteDbService.GetOrderById(id);
			Assert.IsNull(deletedOrder);
		}


		[TestMethod]
		public void AddDuplicateOrderNumber_ShouldThrowException() {
			Order order = new() {
				OrderNumber = 999,
				DateCreated = DateTime.Now,
				DateStart = DateTime.Now,
				DateEnd = DateTime.Now.AddHours(1)
			};

			Order order2 = new() {
				OrderNumber = 999,
				DateCreated = DateTime.Now,
				DateStart = DateTime.Now,
				DateEnd = DateTime.Now.AddHours(1)
			};

			LiteDbService.AddOrder(order);
			Assert.ThrowsException<InvalidOperationException>(() => { LiteDbService.AddOrder(order2); });
		}

		[TestMethod]
		public void UncompleteOrder_ShouldBePending() {
			Order order = new() {
				OrderNumber = 999,
				DateCreated = DateTime.Now,
				DateStart = DateTime.Now,
				DateEnd = DateTime.Now.AddHours(1),
				IsCompleted = false,
				IsCanceled = false
			};

			Assert.AreEqual(true, order.IsPending);
		}

		[TestMethod]
		public void GetCompletedOrders_ShouldReturnAllCompletedOrders() {
			var orders = DummyDb.GetOrders();

			foreach (Order order in orders) {
				LiteDbService.AddOrder(order);
			}

			var retrievedOrders = LiteDbService.GetCompletedOrders();
			Assert.IsNotNull(retrievedOrders);
			Assert.AreEqual(3, retrievedOrders.Count);
		}

		[TestMethod]
		public void GetPendingOrders_ShouldReturnAllPendingOrders() {
			var orders = DummyDb.GetOrders();

			foreach (Order order in orders) {
				LiteDbService.AddOrder(order);
			}

			var retrievedOrders = LiteDbService.GetPendingOrders();
			Assert.IsNotNull(retrievedOrders);
			Assert.AreEqual(2, retrievedOrders.Count);
		}

		[TestMethod]
		public void GetCanceledOrders_ShouldReturnAllCanceledOrders() {
			var orders = DummyDb.GetOrders();

			foreach (Order order in orders) {
				LiteDbService.AddOrder(order);
			}

			var retrievedOrders = LiteDbService.GetCanceledOrders();
			Assert.IsNotNull(retrievedOrders);
			Assert.AreEqual(2, retrievedOrders.Count);
		}

		[TestMethod]
		public void GetOrderByOrderNumber_ShouldReturnOrderNumber() {
			var orders = DummyDb.GetOrders();

			foreach (Order order in orders) {
				LiteDbService.AddOrder(order);
			}

			int[] numbers = [10, 12, 21, 30, 31];
			foreach (int number in numbers) {
				Order? order = LiteDbService.GetOrderByOrderNumber(number);
				Assert.IsNotNull(order);
				Assert.AreEqual(number, order.OrderNumber);
			}
		}

		[TestMethod]
		public void GetOrders_ShouldReturnAllOrders() {
			var orders = DummyDb.GetOrders();

			foreach (Order order in orders) {
				LiteDbService.AddOrder(order);
			}

			var retrievedOrders = LiteDbService.GetOrders();
			Assert.IsNotNull(retrievedOrders);
			Assert.AreEqual(orders.Count(), retrievedOrders.Count);
		}

		[TestMethod]
		public void GetOrdersCount_ShouldReturnAllOrdersCount() {
			var orders = DummyDb.GetOrders();
			foreach (Order order in orders) {
				LiteDbService.AddOrder(order);
			}

			Assert.AreEqual(orders.Count(), LiteDbService.GetOrdersCount());
		}

		[TestMethod]
		public void GetTotalPrice_ShouldReturnNotCanceledOrdersTotalPrice() {
			var orders = DummyDb.GetOrders();
			foreach (Order order in orders) {
				LiteDbService.AddOrder(order);
			}

			var totalPrice = (double)orders.Where(x => !x.IsCanceled).Sum(o => o.OrderItems.Select(oi => oi.Item.Price).Sum());
			Assert.AreEqual(totalPrice, LiteDbService.GetTotalPrice());
		}

		[TestMethod]
		public void GetTotalPriceOfCompleted_ShouldReturnCompletedOrdersTotalPrice() {
			var orders = DummyDb.GetOrders();
			foreach (Order order in orders) {
				LiteDbService.AddOrder(order);
			}

			var totalPrice = (double)orders.Where(x => x.IsCompleted).Sum(o => o.OrderItems.Select(oi => oi.Item.Price).Sum());
			Assert.AreEqual(totalPrice, LiteDbService.GetTotalPriceOfCompleted());
		}

		[TestMethod]
		public void GetTotalPriceOfPending_ShouldReturnPendingOrdersTotalPrice() {
			var orders = DummyDb.GetOrders();
			foreach (Order order in orders) {
				LiteDbService.AddOrder(order);
			}

			var totalPrice = (double)orders.Where(x => x.IsPending).Sum(o => o.OrderItems.Select(oi => oi.Item.Price).Sum());
			Assert.AreEqual(totalPrice, LiteDbService.GetTotalPriceOfPending());
		}
	}
}
