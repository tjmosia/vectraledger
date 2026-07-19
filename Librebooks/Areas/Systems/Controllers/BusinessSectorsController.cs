using VectraBooks.Areas.Systems.Data;
using VectraBooks.Areas.Systems.Models;
using VectraBooks.Areas.Systems.Providers;

using Microsoft.AspNetCore.Mvc;
namespace VectraBooks.Areas.Systems.Controllers
{
	[ApiController]
	[Route("sectors")]
	public class BusinessSectorsController (ISystemsManager systemManager)
		: SystemsControllerBase(systemManager)
	{
		[Route("create")]
		[HttpPost]
		public async Task<IActionResult> CreateAsync ([FromBody] BusinessSectorRequestModels.Create.Request model, CancellationToken cancellationToken)
		{


			return Ok(await Manager.AddBusinessSectorAsync(new(model.Name!), cancellationToken));

		}

		[HttpPatch("{id}")]
		public async Task<IActionResult> EditAsync (int id, [FromBody] BusinessSectorRequestModels.Create.Request model, CancellationToken cancellationToken)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var sector = await Manager.FindBusinessSectorByIdAsync(id, cancellationToken);

			if (sector is null)
				return NotFound();

			sector.Name = model.Name;
			return Ok(await Manager.UpdateBusinessSectorAsync(sector, cancellationToken));
		}

		[Route("")]
		[HttpGet]
		public async Task<IEnumerable<BusinessSectorData>> GetAsync ()
		{
			var sectors = await Manager.GetBusinessSectorsAsync();

			return sectors.Select(p => new BusinessSectorData(p));
		}

		[Route("{businessSectorId}")]
		[HttpGet]
		public async Task<IActionResult?> GetByIdAsync ([FromRoute] int businessSectorId, CancellationToken cancellationToken)
		{
			var sector = await Manager.FindBusinessSectorByIdAsync(businessSectorId, cancellationToken);
			if (sector is null)
				return NotFound();
			return Ok(new BusinessSectorData(sector));
		}

		[HttpDelete]
		public async Task<IActionResult> OnDeleteAsync ([FromBody] int[] businessSectorIds, CancellationToken cancellationToken)
		{
			var sectors = await Manager.FindBusinessSectorsByIdsAsync(businessSectorIds, cancellationToken);

			if (sectors.Count > 0)
				await Manager.DeleteBusinessSectorsAsync([.. sectors], cancellationToken);

			return Ok();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> OnDeleteAsync (int id, CancellationToken cancellationToken)
		{
			var sector = await Manager.FindBusinessSectorByIdAsync(id, cancellationToken);
			if (sector is null)
				return NotFound();

			return Ok(await Manager.DeleteBusinessSectorsAsync([sector], cancellationToken));
		}
	}
}
