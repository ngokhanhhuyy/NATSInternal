using System.Text.RegularExpressions;
using Bogus;
using Bogus.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NATSInternal.Application.Time;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Infrastructure.DbContext;

namespace NATSInternal.Infrastructure.Seeders;

internal partial class ProductSeeder
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IClock _clock;
    private readonly ILogger<ProductSeeder> _logger;
    private readonly Faker _viFaker;
    private readonly Faker _enFaker;
    private readonly Random _random;
    #endregion
    
    #region Constrcutors
    public ProductSeeder(AppDbContext context, IClock clock, ILogger<ProductSeeder> logger)
    {
        _context = context;
        _clock = clock;
        _logger = logger;
        _viFaker = new("vi");
        _enFaker = new();
        _random = new();
    }
    #endregion
    
    #region Methods
    public async Task<ProductSeededResult> SeedAsync(bool isDevelopment)
    {
        List<Country> countries = await SeedCountriesAsync();

        if (isDevelopment)
        {
            List<ProductCategory> categories = await SeedProductCategoriesAsync();
            List<Brand> brands = await SeedBrandsAsync(countries);
            return new()
            {
                Products = await SeedProductsAsync(brands, categories)
            };
        }

        return new();
    }
    #endregion
    
    #region PrivateMethods
    private async Task<List<Country>> SeedCountriesAsync()
    {
        List<Country> countries = await _context.Countries.ToListAsync();
        if (countries.Count > 0)
        {
            return countries;
        }
        
        _logger.LogInformation("Seeding countries.");
        
        countries = new()
        {
            new("Aruba", "ABW"),
            new("Afghanistan", "AFG"),
            new("Angola", "AGO"),
            new("Anguilla", "AIA"),
            new("Quần đảo Åland", "ALA"),
            new("Albania", "ALB"),
            new("Andorra", "AND"),
            new("CTVQ Ả Rập Thống nhất", "ARE"),
            new("Argentina", "ARG"),
            new("Armenia", "ARM"),
            new("Samoa thuộc Mỹ", "ASM"),
            new("Nam Cực", "ATA"),
            new("Các vùng lãnh thổ phía Nam (Pháp)", "ATF"),
            new("Antigua và Barbuda", "ATG"),
            new("Úc", "AUS"),
            new("Áo", "AUT"),
            new("Azerbaijan", "AZE"),
            new("Burundi", "BDI"),
            new("Bỉ", "BEL"),
            new("Bénin", "BEN"),
            new("Bonaire, Sint Eustatius và Saba", "BES"),
            new("Burkina Faso", "BFA"),
            new("Bangladesh", "BGD"),
            new("Bulgaria", "BGR"),
            new("Bahrain", "BHR"),
            new("Bahamas", "BHS"),
            new("Bosna và Hercegovina", "BIH"),
            new("Saint-Barthélemy", "BLM"),
            new("Belarus", "BLR"),
            new("Belize", "BLZ"),
            new("Bermuda", "BMU"),
            new("Bolivia", "BOL"),
            new("Brasil", "BRA"),
            new("Barbados", "BRB"),
            new("Brunei", "BRN"),
            new("Bhutan", "BTN"),
            new("Đảo Bouvet", "BVT"),
            new("Botswana", "BWA"),
            new("Cộng hòa Trung Phi", "CAF"),
            new("Canada", "CAN"),
            new("Quần đảo Cocos (Keeling)", "CCK"),
            new("Thụy Sĩ", "CHE"),
            new("Chile", "CHL"),
            new("Trung Quốc", "CHN"),
            new("Bờ Biển Ngà", "CIV"),
            new("Cameroon", "CMR"),
            new("Cộng hòa Dân chủ Congo", "COD"),
            new("Cộng hòa Congo", "COG"),
            new("Quần đảo Cook", "COK"),
            new("Colombia", "COL"),
            new("Comoros", "COM"),
            new("Cabo Verde", "CPV"),
            new("Costa Rica", "CRI"),
            new("Cuba", "CUB"),
            new("Curaçao", "CUW"),
            new("Đảo Giáng Sinh", "CXR"),
            new("Quần đảo Cayman", "CYM"),
            new("Síp", "CYP"),
            new("Cộng hòa Séc", "CZE"),
            new("Đức", "DEU"),
            new("Djibouti", "DJI"),
            new("Dominica", "DMA"),
            new("Đan Mạch", "DNK"),
            new("Cộng hòa Dominica", "DOM"),
            new("Algérie", "DZA"),
            new("Ecuador", "ECU"),
            new("Ai Cập", "EGY"),
            new("Eritrea", "ERI"),
            new("Tây Sahara", "ESH"),
            new("Tây Ban Nha", "ESP"),
            new("Estonia", "EST"),
            new("Ethiopia", "ETH"),
            new("Phần Lan", "FIN"),
            new("Fiji", "FJI"),
            new("Quần đảo Falkland", "FLK"),
            new("Pháp", "FRA"),
            new("Quần đảo Faroe", "FRO"),
            new("Liên bang Micronesia", "FSM"),
            new("Gabon", "GAB"),
            new("Anh Quốc", "GBR"),
            new("Gruzia", "GEO"),
            new("Guernsey", "GGY"),
            new("Ghana", "GHA"),
            new("Gibraltar", "GIB"),
            new("Guinée", "GIN"),
            new("Guadeloupe", "GLP"),
            new("Gambia", "GMB"),
            new("Guiné-Bissau", "GNB"),
            new("Guinea Xích Đạo", "GNQ"),
            new("Hy Lạp", "GRC"),
            new("Grenada", "GRD"),
            new("Greenland", "GRL"),
            new("Guatemala", "GTM"),
            new("Guyane thuộc Pháp", "GUF"),
            new("Guam", "GUM"),
            new("Guyana", "GUY"),
            new("Hồng Kông", "HKG"),
            new("Đảo Heard và quần đảo McDonald", "HMD"),
            new("Honduras", "HND"),
            new("Croatia", "HRV"),
            new("Haiti", "HTI"),
            new("Hungary", "HUN"),
            new("Indonesia", "IDN"),
            new("Đảo Man", "IMN"),
            new("Ấn Độ", "IND"),
            new("Lãnh thổ Ấn Độ Dương thuộc Anh", "IOT"),
            new("Cộng hòa Ireland", "IRL"),
            new("Iran", "IRN"),
            new("Iraq", "IRQ"),
            new("Iceland", "ISL"),
            new("Israel", "ISR"),
            new("Ý", "ITA"),
            new("Jamaica", "JAM"),
            new("Jersey", "JEY"),
            new("Jordan", "JOR"),
            new("Nhật Bản", "JPN"),
            new("Kazakhstan", "KAZ"),
            new("Kenya", "KEN"),
            new("Kyrgyzstan", "KGZ"),
            new("Campuchia", "KHM"),
            new("Kiribati", "KIR"),
            new("Saint Kitts và Nevis", "KNA"),
            new("Hàn Quốc", "KOR"),
            new("Kuwait", "KWT"),
            new("Lào", "LAO"),
            new("Liban", "LBN"),
            new("Liberia", "LBR"),
            new("Libya", "LBY"),
            new("Saint Lucia", "LCA"),
            new("Liechtenstein", "LIE"),
            new("Sri Lanka", "LKA"),
            new("Lesotho", "LSO"),
            new("Litva", "LTU"),
            new("Luxembourg", "LUX"),
            new("Latvia", "LVA"),
            new("Ma Cao", "MAC"),
            new("Saint-Martin", "MAF"),
            new("Maroc", "MAR"),
            new("Monaco", "MCO"),
            new("Moldova", "MDA"),
            new("Madagascar", "MDG"),
            new("Maldives", "MDV"),
            new("México", "MEX"),
            new("Quần đảo Marshall", "MHL"),
            new("Bắc Macedonia", "MKD"),
            new("Mali", "MLI"),
            new("Malta", "MLT"),
            new("Myanmar", "MMR"),
            new("Montenegro", "MNE"),
            new("Mông Cổ", "MNG"),
            new("Quần đảo Bắc Mariana", "MNP"),
            new("Mozambique", "MOZ"),
            new("Mauritanie", "MRT"),
            new("Montserrat", "MSR"),
            new("Martinique", "MTQ"),
            new("Mauritius", "MUS"),
            new("Malawi", "MWI"),
            new("Malaysia", "MYS"),
            new("Mayotte", "MYT"),
            new("Namibia", "NAM"),
            new("Nouvelle-Calédonie", "NCL"),
            new("Niger", "NER"),
            new("Đảo Norfolk", "NFK"),
            new("Nigeria", "NGA"),
            new("Nicaragua", "NIC"),
            new("Niue", "NIU"),
            new("Hà Lan", "NLD"),
            new("Na Uy", "NOR"),
            new("Nepal", "NPL"),
            new("Nauru", "NRU"),
            new("New Zealand", "NZL"),
            new("Oman", "OMN"),
            new("Pakistan", "PAK"),
            new("Panama", "PAN"),
            new("Quần đảo Pitcairn", "PCN"),
            new("Peru", "PER"),
            new("Philippines", "PHL"),
            new("Palau", "PLW"),
            new("Papua New Guinea", "PNG"),
            new("Ba Lan", "POL"),
            new("Puerto Rico", "PRI"),
            new("Bắc Triều Tiên", "PRK"),
            new("Bồ Đào Nha", "PRT"),
            new("Paraguay", "PRY"),
            new("Palestine", "PSE"),
            new("Polynésie thuộc Pháp", "PYF"),
            new("Qatar", "QAT"),
            new("Réunion", "REU"),
            new("România", "ROU"),
            new("Nga", "RUS"),
            new("Rwanda", "RWA"),
            new("Ả Rập Xê Út", "SAU"),
            new("Sudan", "SDN"),
            new("Sénégal", "SEN"),
            new("Singapore", "SGP"),
            new("Nam Georgia và Q.đ. Nam Sandwich", "SGS"),
            new("Saint Helena, Ascension và T.d.C.", "SHN"),
            new("Svalbard và Jan Mayen", "SJM"),
            new("Quần đảo Solomon", "SLB"),
            new("Sierra Leone", "SLE"),
            new("El Salvador", "SLV"),
            new("San Marino", "SMR"),
            new("Somalia", "SOM"),
            new("Saint-Pierre và Miquelon", "SPM"),
            new("Serbia", "SRB"),
            new("Nam Sudan", "SSD"),
            new("São Tomé và Príncipe", "STP"),
            new("Suriname", "SUR"),
            new("Slovakia", "SVK"),
            new("Slovenia", "SVN"),
            new("Thụy Điển", "SWE"),
            new("Eswatini", "SWZ"),
            new("Sint Maarten", "SXM"),
            new("Seychelles", "SYC"),
            new("Syria", "SYR"),
            new("Quần đảo Turks và Caicos", "TCA"),
            new("Tchad", "TCD"),
            new("Togo", "TGO"),
            new("Thái Lan", "THA"),
            new("Tajikistan", "TJK"),
            new("Tokelau", "TKL"),
            new("Turkmenistan", "TKM"),
            new("Đông Timor", "TLS"),
            new("Tonga", "TON"),
            new("Trinidad và Tobago", "TTO"),
            new("Tunisia", "TUN"),
            new("Thổ Nhĩ Kỳ", "TUR"),
            new("Tuvalu", "TUV"),
            new("Đài Loan", "TWN"),
            new("Tanzania", "TZA"),
            new("Uganda", "UGA"),
            new("Ukraina", "UKR"),
            new("Các tiểu đảo xa của Hoa Kỳ", "UMI"),
            new("Uruguay", "URY"),
            new("Hoa Kỳ", "USA"),
            new("Uzbekistan", "UZB"),
            new("Thành Vatican", "VAT"),
            new("Saint Vincent và Grenadines", "VCT"),
            new("Venezuela", "VEN"),
            new("Quần đảo Virgin thuộc Anh", "VGB"),
            new("Quần đảo Virgin thuộc Mỹ", "VIR"),
            new("Việt Nam", "VNM"),
            new("Vanuatu", "VUT"),
            new("Wallis và Futuna", "WLF"),
            new("Samoa", "WSM"),
            new("Yemen", "YEM"),
            new("Cộng hòa Nam Phi", "ZAF"),
            new("Zambia", "ZMB"),
            new("Zimbabwe", "ZWE")
        };
        
        _context.Countries.AddRange(countries);
        await _context.SaveChangesAsync();

        return countries;
    }

    private async Task<List<ProductCategory>> SeedProductCategoriesAsync()
    {
        List<ProductCategory> categories = await _context.ProductCategories.ToListAsync();
        if (categories.Count > 0)
        {
            return categories;
        }
        
        _logger.LogInformation("Seeding product categories.");

        foreach (string name in _viFaker.Commerce.Categories(5))
        {
            ProductCategory category = new(name, _clock.Now);
            _context.ProductCategories.Add(category);
            categories.Add(category);
        }

        await _context.SaveChangesAsync();
        
        return categories;
    }

    private async Task<List<Brand>> SeedBrandsAsync(List<Country> countries)
    {
        List<Brand> brands = await _context.Brands.ToListAsync();
        if (brands.Count > 0)
        {
            return brands;
        }
        
        _logger.LogInformation("Seeding brands.");

        for (int _ = 0; _ < 10; _++)
        {
            string name = _enFaker.Company.CompanyName();
            string lowerNonDiacriticsName = name.RemoveDiacritics().ToLower();
            string domainName = GetBrandNameSpecialCharactersToBeRemovedRegex().Replace(lowerNonDiacriticsName, "");
            Brand brand = new(
                name: name,
                website: $"https://{domainName}.{_enFaker.Internet.DomainSuffix()}",
                socialMediaUrl: $"https://facebook.com/{domainName}",
                email: _enFaker.Internet.Email(),
                address: _enFaker.Address.StreetAddress(),
                phoneNumber: GetPhoneNumberCountryCodeRegex()
                    .Replace(_viFaker.Phone.PhoneNumber(), "0")
                    .Replace(" ", ""),
                createdDateTime: _clock.Now,
                countryId: countries[_random.Next(countries.Count)].Id
            );

            _context.Brands.Add(brand);
            brands.Add(brand);
        }

        await _context.SaveChangesAsync();

        return brands;
    }

    private async Task<List<Product>> SeedProductsAsync(List<Brand> brands, List<ProductCategory> categories)
    {
        List<Product> products = await _context.Products.ToListAsync();
        if (products.Count > 0)
        {
            return products;
        }
        
        _logger.LogInformation("Seeding products.");

        string[] units = new[] { "Chai", "Lọ", "Túi", "Hộp", "Vĩ" };

        for (int _ = 0; _ < 30; _++)
        {
            Product product = new(
                name: _enFaker.Commerce.ProductName(),
                description: _viFaker.Commerce.ProductDescription(),
                unit: units[_random.Next(units.Length)],
                defaultAmountBeforeVatPerUnit: _random.Next(200, 1000) * 1000L,
                defaultVatPercentage: 10,
                isForRetail: _random.Next(10) > 2,
                isDiscontinued: _random.Next(10) == 0,
                createdDateTime: _clock.Now,
                brand: _random.Next(2) == 0 ? brands[_random.Next(brands.Count)] : null,
                category: _random.Next(2) == 0 ? categories[_random.Next(categories.Count)] : null
            );

            _context.Products.Add(product);
            products.Add(product);
        }

        await _context.SaveChangesAsync();
        return products;
    }
    #endregion
    
    #region StaticMethods
    [GeneratedRegex(@"\.|&|-|_")]
    private static partial Regex GetBrandNameSpecialCharactersToBeRemovedRegex();

    [GeneratedRegex(@"^\+\d+")]
    private static partial Regex GetPhoneNumberCountryCodeRegex();
    #endregion
}

internal class ProductSeededResult
{
    #region Properties
    public List<Product> Products { get; init; } = new();
    #endregion
}