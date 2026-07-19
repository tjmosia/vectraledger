using VectraBooks.Core.EFCore;
using VectraBooks.Core.Operations;
using VectraBooks.Data;
using VectraBooks.Models.Entity.CompanySpace;
using VectraBooks.Models.Entity.CustomerSpace;
using VectraBooks.Models.Entity.GeneralSpace;
using Microsoft.EntityFrameworkCore;

namespace VectraBooks.Areas.Customers.Providers;

public class CustomerStore (AppDbContext context, ILogger<CustomerStore> logger) : DbStoreBase(context), ICustomerStore
{
	private readonly ILogger<CustomerStore> logger = logger;

	public async Task<TransactionResult<Customer>> AddAsync (Company company, Customer customer)
	{
		try
		{
			customer.CompanyId = company.Id;
			var add = await context.Customers!.AddAsync(customer);
			await context.SaveChangesAsync();
			return TransactionResult<Customer>.Success(add.Entity);
		}
		catch (Exception ex)
		{
			logger.LogError("{error}", ex.InnerException?.Message ?? ex.Message);
			return TransactionResult<Customer>.Failure();
		}
	}

	public async Task<TransactionResult> AddAccountsContact (Customer customer, Contact contact)
	{
		try
		{
			var add = await context.CustomerAccountsContacts!.AddAsync(new CustomerAccountsContact
			{
				CustomerId = customer.Id,
				CustomerContactId = contact.Id
			});

			await context.SaveChangesAsync();
			return TransactionResult.Success;
		}
		catch (Exception ex)
		{
			logger.LogError("{error}", ex.InnerException?.Message ?? ex.Message);
			return TransactionResult.Failure();
		}
	}

	public async Task<TransactionResult<CustomerCategory>> AddCategoryAsync (Company company, CustomerCategory category)
	{
		try
		{
			category.CompanyId = company.Id;
			var add = await context.CustomerCategories!.AddAsync(category);
			await context.SaveChangesAsync();
			return TransactionResult<CustomerCategory>.Success(add.Entity);
		}
		catch (Exception ex)
		{
			logger.LogError("{error}", ex.InnerException?.Message ?? ex.Message);
			return TransactionResult<CustomerCategory>.Failure();
		}
	}

	public async Task<TransactionResult<Contact>> AddContactAsync (Customer customer, Contact contact)
	{
		try
		{
			contact.CustomerContact = new CustomerContact
			{
				CustomerId = customer.Id
			};

			var add = await context.Contacts!.AddAsync(contact);
			await context.SaveChangesAsync();
			return TransactionResult<Contact>.Success(add.Entity);

		}
		catch (Exception ex)
		{
			logger.LogError("{error}", ex.InnerException?.Message ?? ex.Message);
			return TransactionResult<Contact>.Failure();
		}
	}

	public async Task<TransactionResult<Note>> AddNoteAsync (Customer customer, Note note)
	{
		try
		{
			note.CustomerNote = new CustomerNote
			{
				CustomerId = customer.Id,
			};
			var addNote = await context.Notes!.AddAsync(note);
			await context.SaveChangesAsync();
			return TransactionResult<Note>.Success(addNote.Entity);
		}
		catch (Exception ex)
		{
			logger.LogError("{error}", ex.InnerException?.Message ?? ex.Message);
			return TransactionResult<Note>.Failure();
		}
	}

	public async Task<TransactionResult> DeleteAsync (Customer customer)
	{
		try
		{
			context.RemoveRange(await GetNotesAsync(customer), await GetContactsAsync(customer), customer);
			await context.SaveChangesAsync();
			return TransactionResult.Success;
		}
		catch (Exception ex)
		{
			logger.LogError("{error}", ex.InnerException?.Message ?? ex.Message);
			if (IsForeignKeyViolation(ex) && ex.InnerException!.Message.Contains("CustomerId"))
				return TransactionResult.Failure(new TransactionError(nameof(AppErrorDescriber.ForeignKeyConstraint), "Delete failed: Customer has existing records."));
			return TransactionResult.Failure();
		}
	}

	public async Task<TransactionResult> DeleteCategoriesAsync (params CustomerCategory[] categories)
	{
		try
		{
			context.CustomerCategories!.RemoveRange(categories);
			await context.SaveChangesAsync();
			return TransactionResult.Success;
		}
		catch (Exception ex)
		{
			logger.LogError("{error}", ex.InnerException?.Message ?? ex.Message);
			if (IsForeignKeyViolation(ex))
				return TransactionResult.Failure(new TransactionError(nameof(AppErrorDescriber.ForeignKeyConstraint),
					$"Delete failed: {(categories.Length > 1 ? "One or more categories are currently being used." : "Category is currently in use.")}"));

			return TransactionResult.Failure();
		}
	}

	public async Task<TransactionResult> DeleteContactsAsync (params Contact[] contacts)
	{
		context.Contacts!.RemoveRange(contacts);
		await context.SaveChangesAsync();
		return TransactionResult.Success;
	}

	public async Task<TransactionResult> DeleteNotesAsync (params Note[] notes)
	{
		context.Notes!.RemoveRange(notes);
		await context.SaveChangesAsync();
		return TransactionResult.Success;
	}

	public async Task<Customer?> FindByIdAsync (Company company, int id, CancellationToken cancellationToken = default)
	{
		return await context.Customers!.Where(p => p.CompanyId == company.Id && p.Id == id)
			.FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<CustomerCategory?> FindCategoryByIdAsync (Company company, int categoryId, CancellationToken cancellationToken = default)
	{
		return await context.CustomerCategories!.Where(p => p.CompanyId == company.Id && p.Id == categoryId)
				.FirstOrDefaultAsync(cancellationToken);
	}

	public Task<Contact?> FindContactByIdAsync (Customer customer, int contactId, CancellationToken cancellationToken = default)
	{
		return context.CustomerContacts!.Where(p => p.CustomerId == customer.Id && p.ContactId == contactId)
			.Include(p => p.Contact).Select(p => p.Contact).FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<Note?> FindNoteByIdAsync (Customer customer, int noteId, CancellationToken cancellationToken = default)
	{
		return await context.CustomerNotes!
			.Where(p => p.CustomerId == customer.Id && p.NoteId == noteId)
			.Include(p => p.Note).Select(p => p.Note).FirstOrDefaultAsync(cancellationToken);
	}

	public async Task<IList<Customer>> GetAsync (Company company, CancellationToken cancellationToken = default)
	{
		return await context.Customers!
			.Where(p => p.CompanyId == company.Id)
			.ToListAsync(cancellationToken);
	}

	public async Task<IList<CustomerCategory>> GetCategoriesAsync (Company company, CancellationToken cancellationToken = default)
	{
		return await context.CustomerCategories!
			.Where(p => p.CompanyId == company.Id)
			.ToListAsync(cancellationToken);
	}

	public async Task<IList<Contact>> GetContactsAsync (Customer customer, CancellationToken cancellationToken = default)
	{
		return await context.CustomerContacts!.Where(p => p.CustomerId == customer.Id)
			.Include(p => p.Contact)
			.Select(p => p.Contact!)
			.ToListAsync(cancellationToken);
	}

	public async Task<IList<Note>> GetNotesAsync (Customer customer, CancellationToken cancellationToken = default)
	{
		return await context.CustomerNotes!.Where(p => p.CustomerId == customer.Id)
			.Include(p => p.Note)
			.Select(p => p.Note!)
			.ToListAsync(cancellationToken);
	}

	public async Task<TransactionResult<Customer>> UpdateAsync (Customer customer)
	{
		try
		{
			var update = context.Customers!.Update(customer);
			await context.SaveChangesAsync();
			return TransactionResult<Customer>.Success(update.Entity);
		}
		catch (Exception ex)
		{
			logger.LogError("{error}", ex.InnerException?.Message ?? ex.Message);
			return TransactionResult<Customer>.Failure();
		}
	}

	public async Task<TransactionResult<CustomerCategory>> UpdateCategoryAsync (CustomerCategory category)
	{
		try
		{
			var update = context.CustomerCategories!.Update(category);
			await context.SaveChangesAsync();
			return TransactionResult<CustomerCategory>.Success(update.Entity);
		}
		catch (Exception ex)
		{
			logger.LogError("{error}", ex.InnerException?.Message ?? ex.Message);
			return TransactionResult<CustomerCategory>.Failure();
		}
	}

	public async Task<TransactionResult<Contact>> UpdateContactAsync (Contact contact)
	{
		try
		{
			var update = context.Contacts!.Update(contact);
			await context.SaveChangesAsync();
			return TransactionResult<Contact>.Success(update.Entity);
		}
		catch (Exception ex)
		{
			logger.LogError("{error}", ex.InnerException?.Message ?? ex.Message);
			return TransactionResult<Contact>.Failure();
		}
	}

	public async Task<TransactionResult<Note>> UpdateNoteAsync (Note note)
	{
		try
		{
			var update = context.Notes!.Update(note);
			await context.SaveChangesAsync();
			return TransactionResult<Note>.Success(update.Entity);
		}
		catch (Exception ex)
		{
			logger.LogError("{error}", ex.InnerException?.Message ?? ex.Message);
			return TransactionResult<Note>.Failure();
		}
	}

	private async Task<TransactionResult<CustomerAdjustment>> AddAdjustmentAsync (Customer customer, CustomerAdjustment adjustment)
	{
		try
		{
			adjustment.CustomerId = customer.Id;
			adjustment.CompanyId = customer.CompanyId;
			var add = context.CustomerAdjustments!.Add(adjustment);
			await context.SaveChangesAsync();
			return TransactionResult<CustomerAdjustment>.Success(add.Entity);

		}
		catch (Exception ex)
		{
			logger.LogError("{error}", ex.InnerException?.Message ?? ex.Message);
			return TransactionResult<CustomerAdjustment>.Failure();
		}
	}

	private Task<TransactionResult<CustomerAdjustment>> UpdateAdjustmentAsync (CustomerAdjustment adjustment)
	{
		throw new NotImplementedException();
	}

	private Task<CustomerAdjustment?> FindAdjustmentByIdAsync (Customer customer, int id, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	private Task<IList<CustomerAdjustment>> GetAdjustmentsAsync (Customer customer, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	private Task<TransactionResult> DeleteAdjustmentAsync (CustomerAdjustment adjustment)
	{
		throw new NotImplementedException();
	}
}
