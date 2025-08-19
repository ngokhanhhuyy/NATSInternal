namespace NATSInternal.Core.Interfaces.Services;

internal interface IPasswordHashingService
{
    #region Methods
    public string HashPassword(string password);
    public bool VerifyPassword(string password, string passwordHash);
    #endregion
}
