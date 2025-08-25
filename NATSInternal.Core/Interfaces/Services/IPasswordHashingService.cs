namespace NATSInternal.Core.Interfaces.Services;

public interface IPasswordHashingService
{
    #region Methods
    /// <summary>
    /// Generates a password hash from a given password.
    /// </summary>
    /// <param name="password">
    /// The password to hash.
    /// </param>
    /// <returns>
    /// The generated password hash.
    /// </returns>
    public string HashPassword(string password);

    /// <summary>
    /// Verify if a given password matches the given password hash.
    /// </summary>
    /// <param name="password">
    /// The password to verify.
    /// </param>
    /// <param name="passwordHash">
    /// The password hash to verify against.
    /// </param>
    /// <returns>
    /// A <see langword="bool"/> indicating whether the password matches the hash.
    /// </returns>
    public bool VerifyPassword(string password, string passwordHash);
    #endregion
}
