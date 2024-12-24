using DatabaseLibrary.Models;

namespace DatabaseLibrary.Tests {
	[TestClass]
	public class OrderItemTests : TestSetupCleanup {
		[TestMethod]
		public void AddOrderItem_ShouldInsertOrderItemIntoDatabase() {
			Item item = new() {
				Name = "OrderItem Test Item",
				Price = 15.99
			};
			LiteDbService.AddItem(item);

			OrderItem orderItem = new() {
				Item = item,
				Quantity = 2
			};

			var id = LiteDbService.AddOrderItem(orderItem);

			var retrievedOrderItem = LiteDbService.GetOrderItemById(id);
			Assert.IsNotNull(retrievedOrderItem);
			Assert.AreEqual("OrderItem Test Item", retrievedOrderItem.Item.Name);
			Assert.AreEqual(2, retrievedOrderItem.Quantity);
		}

		[TestMethod]
		public void UpdateOrderItem_ShouldModifyExistingOrderItem() {
			Item item = new() {
				Name = "OrderItem Test Item",
				Price = 15.99
			};
			LiteDbService.AddItem(item);

			OrderItem orderItem = new() {
				Item = item,
				Quantity = 2
			};
			var id = LiteDbService.AddOrderItem(orderItem);

			orderItem.Quantity = 5;
			var isUpdated = LiteDbService.UpdateOrderItem(orderItem);

			Assert.IsTrue(isUpdated);
			var updatedOrderItem = LiteDbService.GetOrderItemById(id);
			Assert.AreEqual(5, updatedOrderItem.Quantity);
		}

		[TestMethod]
		public void DeleteOrderItem_ShouldRemoveOrderItemFromDatabase() {
			Item item = new() {
				Name = "OrderItem to Delete",
				Price = 19.99
			};
			LiteDbService.AddItem(item);

			OrderItem orderItem = new() {
				Item = item,
				Quantity = 3
			};
			var id = LiteDbService.AddOrderItem(orderItem);

			var isDeleted = LiteDbService.DeleteOrderItem(id);

			Assert.IsTrue(isDeleted);
			var deletedOrderItem = LiteDbService.GetOrderItemById(id);
			Assert.IsNull(deletedOrderItem);
		}
	}
}
