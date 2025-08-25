namespace NATSInternal.Core.Services;

/// <inheritdoc />
public class PasswordHashingService : IPasswordHashingService
{
    #region Methods
    /// <inheritdoc />
    public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    /// <inheritdoc />
    public bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
    #endregion
}
