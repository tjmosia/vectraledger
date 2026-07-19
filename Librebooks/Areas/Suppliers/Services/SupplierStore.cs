using VectraBooks.Data;

namespace VectraBooks.Areas.Suppliers.Services;

public class SupplierStore (AppDbContext context, ILogger<SupplierStore> logger)
{
	private readonly AppDbContext context = context;
	private readonly ILogger<SupplierStore> logger = logger;
}
