namespace NATSInternal.Services.Entities;

internal class Customer : ICustomerEntity<Customer, User>
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(10)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(10)]
    public string NormalizedFirstName { get; set; }

    [StringLength(20)]
    public string MiddleName { get; set; }

    [StringLength(20)]
    public string NormalizedMiddleName { get; set; }

    [Required]
    [StringLength(10)]
    public string LastName { get; set; }

    [Required]
    [StringLength(10)]
    public string NormalizedLastName { get; set; }

    [Required]
    [StringLength(45)]
    public string FullName { get; set; }

    [Required]
    [StringLength(45)]
    public string NormalizedFullName { get; set; }

    [StringLength(35)]
    public string NickName { get; set; }

    [Required]
    public Gender Gender { get; set; }

    public DateOnly? Birthday { get; set; }

    [StringLength(15)]
    public string PhoneNumber { get; set; }

    [StringLength(15)]
    public string ZaloNumber { get; set; }

    [StringLength(1000)]
    public string FacebookUrl { get; set; }

    [StringLength(320)]
    public string Email { get; set; }

    [StringLength(255)]
    public string Address { get; set; }

    [Required]
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();

    public DateTime? UpdatedDateTime { get; set; }

    [StringLength(255)]
    public string Note { get; set; }

    [Required]
    public bool IsDeleted { get; set; }

    // Foreign keys
    public int? IntroducerId { get; set; }

    [Required]
    public int CreatedUserId { get; set; }

    // Concurrency operation tracking field
    [Timestamp]
    public byte[] RowVersion { get; set; }

    // Relationships
    public virtual User CreatedUser { get; set; }
    public virtual List<Customer> IntroducedCustomers { get; set; }
    public virtual Customer Introducer { get; set; }
    public virtual List<Order> Orders { get; set; }
    public virtual List<Treatment> Treatments { get; set; }
    public virtual List<Consultant> Consultants { get; set; }
    public virtual List<DebtIncurrence> DebtIncurrences { get; set; }
    public virtual List<DebtPayment> DebtPayments { get; set; }

    // Property for convinience.
    [NotMapped]
    public long DebtIncurredAmount => DebtIncurrences
        .Where(d => !d.IsDeleted)
        .Sum(d => d.Amount);

    [NotMapped]
    public long DebtPaidAmount => DebtPayments
        .Where(dp => !dp.IsDeleted)
        .Sum(dp => dp.Amount);

    [NotMapped]
    public long DebtAmount =>  DebtIncurredAmount - DebtPaidAmount;

    public static void ConfigureModel(EntityTypeBuilder<Customer> entityBuilder)
    {
        entityBuilder.HasKey(c => c.Id);
        entityBuilder.HasOne(c => c.Introducer)
            .WithMany(i => i.IntroducedCustomers)
            .HasForeignKey(c => c.IntroducerId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasOne(c => c.CreatedUser)
            .WithMany(u => u.CreatedCustomers)
            .HasForeignKey(c => c.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.Property(c => c.RowVersion)
            .IsRowVersion();
    }
}