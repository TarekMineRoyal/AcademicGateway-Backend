using AcademicGateway.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcademicGateway.Application.Features.ProviderApplications.Queries.GetMyProviderApplication;

/// <summary>
/// Handles the execution of the <see cref="GetMyProviderApplicationQuery"/> request.
/// Retrieves the active or latest provider application for the currently authenticated provider user.
/// </summary>
public class GetMyProviderApplicationQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetMyProviderApplicationQuery, MyProviderApplicationDto?>
{
    public async Task<MyProviderApplicationDto?> Handle(
        GetMyProviderApplicationQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Enforce active authentication guard
        if (!currentUserService.IsAuthenticated || currentUserService.UserId == null)
        {
            throw new UnauthorizedAccessException("Access Denied: Authentication is mandatory to access your application record.");
        }

        var providerId = currentUserService.UserId.Value;

        // 2. Fetch the latest application record directly for the authenticated provider
        return await context.ProviderApplications
            .AsNoTracking()
            .Where(a => a.ProviderId == providerId)
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new MyProviderApplicationDto
            {
                Id = a.Id,
                ProviderId = a.ProviderId,
                CompanyDetails = a.CompanyDetails,
                VerificationDocumentsUrl = a.VerificationDocumentsUrl,
                Status = a.Status,
                ReviewerNotes = a.RejectionReason,
                CreatedAt = a.CreatedAt,
                LastUpdatedAt = a.ReviewedAt
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}