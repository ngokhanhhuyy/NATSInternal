namespace NATSInternal.Application.Security;

public interface IPasswordHasher
{
    #region Methods
    string HashPassword(string password);

    bool VerifyPassword(string password, string passwordHash);
    #endregion
}