using Languages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

			ResourceManagerStringLocalizerFactory factory = new(
				Options.Create(new LocalizationOptions()),
				LoggerFactory.Create(_ => { })
			);
			
			LiteDbService = new LiteDbService(new StringLocalizer<OrderManagerAppLanguages>(factory), tempDatabaseDirectory);
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
