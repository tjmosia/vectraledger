using VectraBooks.Core.Operations;
using VectraBooks.Models.Entity.CompanySpace;
using VectraBooks.Models.Entity.InventorySpace;

namespace VectraBooks.Areas.Inventory.Providers;

public interface IItemManager
{
	Task<TransactionResult<Item>> AddAdjustmentAsync (Company company, Item item, InventoryAdjustment adjustment);
	Task<TransactionResult<Item>> UpdateAdjustmentAsync (InventoryAdjustment adjustment, Item item);
	Task<TransactionResult> DeleteAdjustmentAsync (InventoryAdjustment adjustment);
}
