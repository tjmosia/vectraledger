using System.ComponentModel.DataAnnotations;

namespace VectraBooks.Extensions.Models
{
    public abstract class VersionedEntityBase
    {
        [ConcurrencyCheck]
        public virtual string? RowVersion { get; set; }

        public void RefreshConcurrencyToken ()
            => RowVersion = Guid.NewGuid().ToString("N").ToUpper();

        protected VersionedEntityBase ()
            => RowVersion = Guid.NewGuid().ToString("N").ToUpper();
    }
}
