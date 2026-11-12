using BarberBoss.Application.UseCases.Delete;
using BarberBoss.Application.UseCases.GetAll;
using BarberBoss.Application.UseCases.GetById;
using BarberBoss.Application.UseCases.Register;
using BarberBoss.Application.UseCases.Update;
using BarberBoss.Communcations.Filters;
using BarberBoss.Communcations.Requests;
using BarberBoss.Communcations.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BarberBoss.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BillingsController : ControllerBase
{
    private const string ROUTE_ID = "{id:guid}";

    [HttpGet]
    [ProducesResponseType<PagedResponseJson<ResponseShortBillingJson>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetBillings(
        [FromServices] IGetAllUseCase<ResponseShortBillingJson> useCase,
        [FromQuery] PaginationFilter filters
    )
    {
        var response = await useCase.Execute(filters);
        if (response.TotalCount == 0)
        {
            return NoContent();
        }
        return Ok(response);
    }

    [HttpGet]
    [Route(ROUTE_ID)]
    [ProducesResponseType<ResponseBillingJson>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBillingById(
        [FromServices] IGetByIdUseCase<ResponseBillingJson> useCase,
        [FromRoute] Guid id
    )
    {
        var billing = await useCase.Execute(id);
        if (billing is null)
        {
            return NotFound();
        }
        return Ok(billing);
    }

    [HttpPost]
    [ProducesResponseType<ResponseBillingJson>(StatusCodes.Status201Created)]
    [ProducesResponseType<ResponseErrorsJson>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBilling(
        [FromServices] IRegisterUseCase<RequestRegisterBillingJson, ResponseBillingJson> useCase,
        [FromBody] RequestRegisterBillingJson request
        )
    {
        var billing = await useCase.Execute(request);
        return Created(string.Empty, billing);
    }

    [HttpPut]
    [Route(ROUTE_ID)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBilling(
        [FromServices] IUpdateUseCase<RequestRegisterBillingJson> useCase,
        [FromRoute] Guid id,
        [FromBody] RequestRegisterBillingJson request
        )
    {
        await useCase.Execute(id, request);
        return NoContent();
    }

    [HttpDelete]
    [Route(ROUTE_ID)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBilling(
        [FromServices] IDeleteUseCase useCase,
        [FromRoute] Guid id
    )
    {
        await useCase.Execute(id);
        return NoContent();
    }
}
