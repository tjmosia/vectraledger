namespace VectraBooks.Areas.Inventory.Providers
{
	public class ItemManager
		(ItemStore? store, ILogger<ItemManager> logger) : IItemManager
	{
		private readonly ItemStore? store = store;
		private readonly ILogger<ItemManager>? logger = logger;


	}
}
