﻿namespace NATSInternal.Services.Dtos;

public class ProductExistingAuthorizationResponseDto
        : IUpsertableExistingAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
}