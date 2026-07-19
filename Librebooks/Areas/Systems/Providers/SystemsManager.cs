namespace VectraBooks.Areas.Systems.Providers;

public class SystemsManager (SystemsStore systemStore)
	: ISystemsManager
{
	private readonly SystemsStore store = systemStore;

	private const string COMP_NUM_PREFIX = nameof(COMP_NUM_PREFIX);
	private const string COMP_NUM_FORMAT = nameof(COMP_NUM_FORMAT);

	/******************************************************************
    * SystemCompanyNumber Manager Actions
    ******************************************************************/
}
