using DatabaseLibrary.Models;

namespace DatabaseLibrary.Tests {
	[TestClass]
	public class LiteDbTests : TestSetupCleanup {

		[TestMethod]
		public void AddItem_ShouldInsertItemIntoDatabase() {
			Item item = new() {
				Name = "Test Item",
				Price = 9.99
			};

			var id = LiteDbService.AddItem(item);

			var retrievedItem = LiteDbService.GetItemById(id);
			Assert.IsNotNull(retrievedItem);
			Assert.AreEqual("Test Item", retrievedItem.Name);
			Assert.AreEqual(9.99, retrievedItem.Price);
		}

		[TestMethod]
		public void UpdateItem_ShouldModifyExistingItem() {
			Item item = new() {
				Name = "Old Name",
				Price = 5.99
			};
			var id = LiteDbService.AddItem(item);

			item.Name = "New Name";
			item.Price = 10.99;
			var isUpdated = LiteDbService.UpdateItem(item);

			Assert.IsTrue(isUpdated);
			var updatedItem = LiteDbService.GetItemById(id);

			if (updatedItem is null) {
				Assert.Fail();
			}
			Assert.AreEqual("New Name", updatedItem.Name);
			Assert.AreEqual(10.99, updatedItem.Price);
		}

		[TestMethod]
		public void DeleteItem_ShouldRemoveItemFromDatabase() {
			Item item = new() {
				Name = "Item to Delete",
				Price = 3.99
			};

			var id = LiteDbService.AddItem(item);
			var isDeleted = LiteDbService.DeleteItem(id);

			Assert.IsTrue(isDeleted);
			var deletedItem = LiteDbService.GetItemById(id);
			Assert.IsNull(deletedItem);
		}

		[TestMethod]
		public void GetItemById_ShouldReturnCorrectItem() {
			Item item = new() {
				Name = "Specific Item",
				Price = 7.99
			};

			var id = LiteDbService.AddItem(item);
			var retrievedItem = LiteDbService.GetItemById(id);

			Assert.IsNotNull(retrievedItem);
			Assert.AreEqual("Specific Item", retrievedItem.Name);
			Assert.AreEqual(7.99, retrievedItem.Price);
		}

		[TestMethod]
		public void GetAllItems_ShouldReturnAllItems() {
			Item item1 = new() {
				Name = "Item1",
				Price = 4.99
			};
			Item item2 = new() {
				Name = "Item2",
				Price = 8.99
			};

			LiteDbService.AddItem(item1);
			LiteDbService.AddItem(item2);

			var items = LiteDbService.Items.FindAll().ToList();

			Assert.AreEqual(2, items.Count);
			Assert.IsTrue(items.Any(i => i.Name == "Item1"));
			Assert.IsTrue(items.Any(i => i.Name == "Item2"));
		}

		[TestMethod]
		public void AddDuplicateItem_ShouldAllowDuplicates() {
			Item item1 = new() {
				Name = "Duplicate Item",
				Price = 12.99
			};

			Item item2 = new() {
				Name = item1.Name,
				Price = item1.Price
			};

			LiteDbService.AddItem(item1);
			LiteDbService.AddItem(item2);

			var items = LiteDbService.Items.Find(i => i.Name == "Duplicate Item").ToList();

			Assert.AreEqual(2, items.Count);
		}
	}
}
