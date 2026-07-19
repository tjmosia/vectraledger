using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

using VectraBooks.Core.Constants;
using VectraBooks.Models.Entity.AccountingSpace;

namespace VectraBooks.Models.Entity.InventorySpace;

[Table(nameof(GoodsIssueLine))]
public class GoodsIssueLine
{
    public virtual int Id { get; set; }
    public virtual int IssueId { get; set; }
    public virtual decimal OldQuantityOnHand { get; set; }
    public virtual decimal Quantity { get; set;  }
    public virtual decimal OldAverageCost { get; set; }
    public virtual decimal AverageCost { get; set; }
    public virtual int WarehouseId { get; set; }
    public virtual int? SourceLineId { get; set; }
    public virtual int PostingRuleId { get; set; }

    public PostingRule? PostingRule { get; set; }
}
