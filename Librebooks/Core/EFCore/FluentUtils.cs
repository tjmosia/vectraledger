using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VectraBooks.Core.EFCore;

public class FluentUtils<TModel, TKey> where TModel : class where TKey : notnull
{
	public static void UseSequentialId (PropertyBuilder<TKey> propertyBuilder)
	{
		propertyBuilder.HasDefaultValue("newsequentialid()");
	}
}

public class FluentUtils
{
	public const string NewSequentialId = "newsequentialid()";
}
