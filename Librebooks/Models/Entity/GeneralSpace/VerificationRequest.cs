using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VectraBooks.Core.Constants;
using VectraBooks.Extensions.Models;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Models.Entity.GeneralSpace;

[Table(nameof(VerificationRequest))]
public class VerificationRequest : VersionedEntityBase
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public virtual int Id { get; set; }

	[Required, MaxLength(75)]
	public virtual string? Email { get; set; }

	[Required, MaxLength(50)]
	public virtual string? Reason { get; set; }

	[Required, MaxLength(155)]
	public virtual string? HashString { get; set; }

	[Column(TypeName = ColumnTypes.DATETIME)]
	public virtual DateTime ValidUntil { get; set; }

	public virtual short Attempts { get; set; }
	public virtual short MaxAttemptsAllowed { get; set; }

	public VerificationRequest () : base()
	{
		Attempts = 0;
		MaxAttemptsAllowed = 3;
		ValidUntil = DateTime.Now.AddHours(3);
	}

	public VerificationRequest (string subject, string reason)
		: this()
	{
		Email = subject;
		Reason = reason;
	}

	public bool HasExpired ()
		=> DateTime.Now.CompareTo(ValidUntil) > 0 || Attempts > 2;

	public static void OnModelCreating (ModelBuilder builder)
	{
		builder.Entity<VerificationRequest>(options =>
		{

		});
	}
}
