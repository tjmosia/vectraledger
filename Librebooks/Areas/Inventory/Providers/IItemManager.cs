using Librebooks.Core.Operations;
using Librebooks.Models.Entity.CompanySpace;
using Librebooks.Models.Entity.InventorySpace;

namespace Librebooks.Areas.Inventory.Providers;

public interface IItemManager
{
	Task<TransactionResult<Item>> AddAdjustmentAsync (Company company, Item item, InventoryAdjustment adjustment);
	Task<TransactionResult<Item>> UpdateAdjustmentAsync (InventoryAdjustment adjustment, Item item);
	Task<TransactionResult> DeleteAdjustmentAsync (InventoryAdjustment adjustment);
}
