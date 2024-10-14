global using System.Text.Json;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.SignalR;
global using NATSInternal.Extensions;
global using NATSInternal.Hubs;
global using NATSInternal.Hubs.Notifier;
global using NATSInternal.Hubs.ResourceAccess;
global using NATSInternal.Services;
global using NATSInternal.Services.Constants;
global using NATSInternal.Services.Dtos;
global using NATSInternal.Services.Enums;
global using NATSInternal.Services.Exceptions;
global using NATSInternal.Services.Interfaces;
global using NATSInternal.Validation;
global using NATSInternal.Validation.Validators;

global using IAuthorizationService = NATSInternal.Services.Interfaces.IAuthorizationService;
global using FluentValidation;
global using FluentValidation.Results;
global using ValidationResult = FluentValidation.Results.ValidationResult;
global using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;
