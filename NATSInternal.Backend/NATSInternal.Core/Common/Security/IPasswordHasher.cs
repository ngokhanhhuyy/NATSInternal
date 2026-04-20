namespace NATSInternal.Core.Common.Security;

internal interface IPasswordHasher
{
    #region Methods
    string HashPassword(string password);

    bool VerifyPassword(string password, string passwordHash);
    #endregion
}