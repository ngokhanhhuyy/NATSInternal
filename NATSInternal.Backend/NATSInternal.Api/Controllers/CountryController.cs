// using MediatR;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using NATSInternal.Application.UseCases.Products;

// namespace NATSInternal.Api.Controllers;

// [Route("api/products/brands/countries")]
// [ApiController]
// [Authorize]
// public class CountryController : ControllerBase
// {
//     #region Fields
//     private readonly IMediator _mediator;
//     #endregion

//     #region Constructors
//     public CountryController(IMediator mediator)
//     {
//         _mediator = mediator;
//     }
//     #endregion

//     #region Methods
//     public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
//     {
//         CountryGetAllRequestDto requestDto = new();
//         return Ok(await _mediator.Send(requestDto, cancellationToken));
//     }
//     #endregion
// }