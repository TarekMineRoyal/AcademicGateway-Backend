using AcademicGateway.Application.Features.ProviderApplications.Queries.GetMyProviderApplication;
using AcademicGateway.Domain.Common.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace AcademicGateway.Api.Features.ProviderApplications.Queries.GetMyProviderApplication;

/// <summary>
/// Exposes endpoints for authenticated providers to retrieve their active or latest onboarding application record.
/// </summary>
[ApiController]
[Tags("Provider Applications")]
[Authorize(Roles = Roles.Provider)]
[Route("api/provider-applications")]
public class GetMyProviderApplicationController(ISender mediator) : ControllerBase
{
    /// <summary>
    /// Fetches the active or latest onboarding application record and evaluation status for the authenticated provider.
    /// </summary>
    /// <param name="cancellationToken">Propagates notification that network operations should be aborted.</param>
    /// <returns>A 200 OK status containing the provider's application details, or 404 Not Found if no application exists.</returns>
    /// <response code="200">Returns the authenticated provider's application payload.</response>
    /// <response code="401">Returned if the request header lacks valid session authentication bearer tokens.</response>
    /// <response code="403">Returned if accessed by a user without the Provider role.</response>
    /// <response code="404">Returned if the provider has not submitted an onboarding application yet.</response>
    [HttpGet("me")]
    [ProducesResponseType(typeof(MyProviderApplicationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMyApplication(CancellationToken cancellationToken)
    {
        var query = new GetMyProviderApplicationQuery();
        var result = await mediator.Send(query, cancellationToken);

        if (result == null)
        {
            return NotFound("No onboarding application record was found for the authenticated provider.");
        }

        return Ok(result);
    }
}