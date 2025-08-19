namespace NATSInternal.Core.Services;

public class PasswordHashingService : IPasswordHashingService
{
    #region Methods
    public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
    #endregion
}
