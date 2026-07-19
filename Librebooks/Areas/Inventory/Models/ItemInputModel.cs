using System.ComponentModel.DataAnnotations;

namespace VectraBooks.Areas.Inventory.Models
{
    public class ItemInputModel
    {
        [Required]
        public virtual string? Code { get; set; }
        [Required]
        public virtual string? Description { get; set; }
        public virtual string? Unit { get; set; }
        public virtual bool Physical { get; set; } = true;
        public virtual string? CategoryId { get; set; }
        public virtual string? VATId { get; set; }
        public virtual string? CompanyId { get; set; }
    }
}
