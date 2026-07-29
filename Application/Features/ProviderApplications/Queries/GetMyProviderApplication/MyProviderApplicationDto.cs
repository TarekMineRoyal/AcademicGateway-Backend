using System;
using AcademicGateway.Domain.Providers.Enums;

namespace AcademicGateway.Application.Features.ProviderApplications.Queries.GetMyProviderApplication;

/// <summary>
/// Data Transfer Object representing the active or latest provider application record for the current authenticated provider.
/// </summary>
public record MyProviderApplicationDto
{
    /// <summary>
    /// Gets the unique identifier of the provider application.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the unique identifier of the applicant provider user profile.
    /// </summary>
    public Guid ProviderId { get; init; }

    /// <summary>
    /// Gets the full operational overview and academic/industry credentials submitted for review.
    /// </summary>
    public string CompanyDetails { get; init; } = string.Empty;

    /// <summary>
    /// Gets the remote storage link containing attached corporate verification paperwork.
    /// </summary>
    public string VerificationDocumentsUrl { get; init; } = string.Empty;

    /// <summary>
    /// Gets the current state within the evaluation lifecycle pipeline.
    /// </summary>
    public ProviderApplicationStatus Status { get; init; }

    /// <summary>
    /// Gets rejection reason, compliance feedback, or notes left by the reviewer.
    /// </summary>
    public string? ReviewerNotes { get; init; }

    /// <summary>
    /// Gets the timestamp when the application record was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Gets the timestamp when the application record was last updated, if applicable.
    /// </summary>
    public DateTime? LastUpdatedAt { get; init; }
}