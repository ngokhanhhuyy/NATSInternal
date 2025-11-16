using System.Text;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NATSInternal.Application.Time;
using NATSInternal.Domain.Features.Customers;
using NATSInternal.Domain.Features.Users;
using NATSInternal.Infrastructure.DbContext;

namespace NATSInternal.Infrastructure.Seeders;

internal class CustomerSeeder
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IClock _clock;
    private readonly ILogger<CustomerSeeder> _logger;
    private readonly Faker _faker;
    private readonly Random _random;
    #endregion

    #region Constructors
    public CustomerSeeder(
        AppDbContext context,
        IClock clock,
        ILogger<CustomerSeeder> logger)
    {
        _context = context;
        _clock = clock;
        _logger = logger;
        _faker = new("vi");
        _random = new();
    }
    #endregion

    #region Methods
    public async Task<CustomerSeededResult> SeedAsync(List<User> users, bool isDevelopment)
    {
        if (!isDevelopment)
        {
            return new();
        }

        return new()
        {
            Customers = await SeedCustomersAsync(users)
        };
    }
    #endregion

    #region PrivateMethods
    private async Task<List<Customer>> SeedCustomersAsync(List<User> users)
    {
        List<Customer> customers = await _context.Customers.ToListAsync();
        if (customers.Count > 0)
        {
            return customers;
        }

        _logger.LogInformation("Seeding customers.");

        List<Guid> userIds = users.Select(u => u.Id).ToList();
        for (int i = 0; i < 100; i++)
        {
            int genderInt = _random.Next(2);
            Bogus.DataSets.Name.Gender fakerGender = genderInt == 0
                ? Bogus.DataSets.Name.Gender.Male
                : Bogus.DataSets.Name.Gender.Female;
            string fullName = _faker.Name.FullName(fakerGender);
            (string firstName, string? middleName, string lastName) = SplitFullName(fullName);
            string phoneNumber = _faker.Phone.PhoneNumber("##########");
            Customer? introducer = null;
            if (customers.Count != 0 && _random.Next(0, 11) > 8)
            {
                introducer = customers.MinBy(_ => Guid.NewGuid());
            }

            Customer customer = new(
                firstName: lastName,
                middleName: middleName,
                lastName: firstName,
                nickName: GenerateNickName(lastName, _faker.Lorem.Word()),
                gender: genderInt == 0 ? Gender.Male : Gender.Female,
                birthday: DateOnly.FromDateTime(_faker.Date.Between(
                    _clock.Now.AddYears(-20),
                    _clock.Now.AddYears(-80))),
                phoneNumber: phoneNumber.Replace(" ", string.Empty),
                zaloNumber: new[] { phoneNumber, _faker.Phone.PhoneNumber("##########") }
                    .Skip(_random.Next(3))
                    .Take(1)
                    .SingleOrDefault()
                    ?.Replace(" ", string.Empty),
                facebookUrl: "https://facebook.com/" + _faker.Internet.UserName().ToLower(),
                email: _faker.Internet.Email(),
                address: _faker.Address.FullAddress(),
                createdDateTime: _clock.Now,
                note: SliceNoteIfLong(_faker.Lorem.Paragraph()),
                introducer: introducer,
                createdUserId: userIds.Skip(_random.Next(userIds.Count)).Take(1).Single()
            );

            _context.Customers.Add(customer);
            customers.Add(customer);
        }

        await _context.SaveChangesAsync();
        return customers;
    }
    #endregion

    #region StaticMethods
    private static (string FirstName, string? MiddleName, string LastName) SplitFullName(string fullName)
    {
        string[] nameElements = fullName.Split(" ");
        string firstName = nameElements[0];
        string lastName = nameElements[^1];
        string? middleName = null;

        if (nameElements.Length >= 3)
        {
            middleName = string.Join(
                " ",
                nameElements.Where((_, index) => index >= 1 && index < nameElements.Length - 1)
            );
        }

        return (firstName, middleName, lastName);
    }

    private static string GenerateNickName(string lastName, string callsign)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append(lastName + " ");
        if (callsign.Length > 0)
        {
            stringBuilder.Append(callsign[0].ToString().ToUpper());
            if (callsign.Length > 1)
            {
                stringBuilder.Append(callsign[1..]);
            }
        }

        return stringBuilder.ToString();
    }

    private static string SliceNoteIfLong(string note)
    {
        if (note.Length <= 255)
        {
            return note;
        }

        return note[..255];
    }
    #endregion
}

internal class CustomerSeededResult
{
    #region Properties
    public List<Customer> Customers { get; init; } = new(); 
    #endregion
}