using Microsoft.AspNetCore.Mvc;
using NATSInternal.Core.Features.Metadata;

namespace NATSInternal.Api.Controllers;

[ApiController]
[Route("api/metadata")]
public class MetadataController : ControllerBase
{
    #region Fields
    private readonly IMetadataService _service;
    #endregion
    
    #region Constructors
    public MetadataController(IMetadataService service)
    {
        _service = service;
    }
    #endregion
    
    #region Methods
    [HttpGet]
    [ProducesResponseType<MetadataResponseDto>(StatusCodes.Status200OK)]
    public IActionResult Index()
    {
        return Ok(_service.GetMetadata());
    }
    #endregion
}