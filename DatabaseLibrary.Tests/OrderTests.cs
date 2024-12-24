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
	}
}