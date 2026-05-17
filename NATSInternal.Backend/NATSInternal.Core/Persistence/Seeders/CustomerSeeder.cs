using System.Text;
using Bogus;
using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Features.Customers;
using NATSInternal.Core.Features.Users;
using NATSInternal.Core.Persistence.DbContext;
using BogusGender = Bogus.DataSets.Name.Gender;

namespace NATSInternal.Core.Persistence.Seeders;

internal class CustomerSeeder
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IClock _clock;
    private readonly Faker _faker;
    private readonly Random _random;
    #endregion

    #region Constructors
    public CustomerSeeder(AppDbContext context, IClock clock)
    {
        _context = context;
        _clock = clock;
        _faker = new("vi");
        _random = new();
    }
    #endregion

    #region Methods
    public async Task<Customer> PickOrSeedSingleCustomerAsync(
        List<User> users,
        List<Customer> customers,
        DateTime generatingDateTime)
    {
        Customer customer;
        int newCustomerRatio = _random.Next(0, 10);
        if (newCustomerRatio < 7)
        {
            Customer? pickedCustomer = customers
                .Where(c => c.DeletedDateTime == null)
                .OrderBy(_ => Guid.NewGuid())
                .FirstOrDefault();
            
            if (pickedCustomer is not null)
            {
                return pickedCustomer;
            }
        }
        
        List<int> userIds = users.Select(u => u.Id).ToList();
        int genderInt = _random.Next(2);
        BogusGender fakerGender = genderInt == 0 ? BogusGender.Male : BogusGender.Female;
        string fullName = _faker.Name.FullName(fakerGender);
        (string firstName, string? middleName, string lastName) = SplitFullName(fullName);
        string phoneNumber = _faker.Phone.PhoneNumber("##########");
        Customer? introducer = null;
        if (_random.Next(0, 11) > 8)
        {
            introducer = customers
                .Where(c => c.DeletedDateTime == null)
                .OrderBy(_ => Guid.NewGuid())
                .FirstOrDefault();
        }

        string nickName;
        do
        {
            nickName = GenerateNickName(lastName, _faker.Lorem.Word());
        }
        while (await _context.Customers.AnyAsync(c => c.NickName == nickName));

        customer = new()
        {
            FirstName = lastName,
            MiddleName = middleName,
            LastName = firstName,
            NickName = nickName,
            Gender = genderInt == 0 ? Gender.Male : Gender.Female,
            Birthday = DateOnly.FromDateTime(_faker.Date.Between(
                _clock.Now.AddYears(-20),
                _clock.Now.AddYears(-80))),
            PhoneNumber = phoneNumber.Replace(" ", string.Empty),
            ZaloNumber = new[] { phoneNumber, _faker.Phone.PhoneNumber("##########") }
                .Skip(_random.Next(3))
                .Take(1)
                .SingleOrDefault()
                ?.Replace(" ", string.Empty),
            FacebookUrl = "https://facebook.com/" + _faker.Internet.UserName().ToLower(),
            Email = _faker.Internet.Email(),
            Address = _faker.Address.StreetAddress(),
            Note = SliceNoteIfLong(_faker.Lorem.Paragraph()),
            CreatedDateTime = generatingDateTime,
            Introducer = introducer,
            CreatedUserId = userIds.Skip(_random.Next(userIds.Count)).Take(1).Single()
        };

        _context.Customers.Add(customer);
        customers.Add(customer);

        await _context.SaveChangesAsync();
        return customer;
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
