using MediatR;

namespace AcademicGateway.Application.Features.ProviderApplications.Queries.GetMyProviderApplication;

/// <summary>
/// CQRS Query request to retrieve the active or latest provider application record for the current authenticated provider.
/// </summary>
public record GetMyProviderApplicationQuery : IRequest<MyProviderApplicationDto?>;