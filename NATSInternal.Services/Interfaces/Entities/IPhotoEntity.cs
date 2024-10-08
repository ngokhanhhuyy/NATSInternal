﻿namespace NATSInternal.Services.Interfaces.Entities;

internal interface IPhotoEntity<T> : IIdentifiableEntity<T> where T : class, new()
{
    string Url { get; set; }
}
