﻿namespace NATSInternal.Services.Interfaces.Entities;

internal interface IIdentifiableEntity<T> : IEntity<T> where T : class, new()
{
    int Id { get; set; }
}
