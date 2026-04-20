namespace NATSInternal.Core.Common.Security;

internal class PasswordHasher : IPasswordHasher
{
    #region Methods
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
    #endregion
}