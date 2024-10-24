global using System.Reflection;
global using System.Text.Json;
global using System.Linq.Expressions;
global using System.ComponentModel.DataAnnotations;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.Components.Forms;
global using NATSInternal.Blazor.Extensions;
global using NATSInternal.Blazor.Helpers;
global using NATSInternal.Blazor.Models;
global using NATSInternal.Blazor.Models.Interfaces;
global using NATSInternal.Services;
global using NATSInternal.Services.Constants;
global using NATSInternal.Services.Dtos;
global using NATSInternal.Services.Enums;
global using NATSInternal.Services.Exceptions;
global using NATSInternal.Services.Extensions;
global using NATSInternal.Services.Interfaces;
global using NATSInternal.Services.Localization;
global using NATSInternal.Validation;
global using NATSInternal.Validation.Validators;

global using IAuthorizationService = NATSInternal.Services.Interfaces.IAuthorizationService;
global using FluentValidation;
global using FluentValidation.Results;
global using ValidationResult = FluentValidation.Results.ValidationResult;
global using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;
