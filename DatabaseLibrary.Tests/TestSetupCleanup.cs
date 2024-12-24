namespace DatabaseLibrary.Tests {
	public abstract class TestSetupCleanup {
		protected LiteDbService LiteDbService;
		private string        _tempDatabasePath;

		[TestInitialize]
		public void Setup()
		{
			var tempDatabaseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TestDb_OrderManagerApp");

			if (!Directory.Exists(tempDatabaseDirectory))
			{
				Directory.CreateDirectory(tempDatabaseDirectory);
			}

			_tempDatabasePath = Path.Combine(tempDatabaseDirectory, "TestDB_OrderManagerApp.db");

			LiteDbService = new LiteDbService(null, tempDatabaseDirectory);
		}

		[TestCleanup]
		public void Cleanup()
		{
			LiteDbService.GetDatabase().Dispose();

			var tempDatabaseDirectory = Path.GetDirectoryName(_tempDatabasePath);
			if (tempDatabaseDirectory != null && Directory.Exists(tempDatabaseDirectory))
			{
				Directory.Delete(tempDatabaseDirectory, recursive: true);
			}
		}
	}
}
