using Languages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DatabaseLibrary.Tests;

public abstract class TestSetupCleanup {
	protected LiteDbService LiteDbService = null!;

	[TestInitialize]
	public void Setup() {
		ResourceManagerStringLocalizerFactory factory = new(
			Options.Create(new LocalizationOptions()),
			LoggerFactory.Create(_ => { })
		);

		LiteDbService = new LiteDbService(new StringLocalizer<OrderManagerAppLanguages>(factory), Path.GetTempFileName());
	}

	[TestCleanup]
	public void Cleanup() {
		string db = LiteDbService.DatabaseFile;
		LiteDbService.Dispose();
		File.Delete(db);
	}
}
