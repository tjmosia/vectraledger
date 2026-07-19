using VectraBooks.Core.Operations;
using VectraBooks.Models.Entity.CompanySpace;
using VectraBooks.Models.Entity.IdentitySpace;
using VectraBooks.Models.Entity.SystemSpace;
using Microsoft.EntityFrameworkCore.Storage;

namespace VectraBooks.Areas.Companies.Services;

public interface ICompanyManager
{
    Task<TransactionResult<Company>> CreateAsync(Company company, User user, Country country, Currency currency);
}
