using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;

namespace NATSInternal.Services;

internal class AuthorizationService : IAuthorizationInternalService
{
    private readonly DatabaseContext _context;
    private User _user;

    public AuthorizationService(
            DatabaseContext context,
            IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        ClaimsPrincipal user = httpContextAccessor.HttpContext?.User;
        
        string userIdAsString = user.FindFirstValue(ClaimTypes.NameIdentifier);
        bool parsable = int.TryParse(userIdAsString, out int userId);
        if (!parsable)
        {
            throw new AuthenticationException();
        }
        
        SetUserId(userId);
    }

    public void SetUserId(int id)
    {
        _user = _context.Users
            .Include(u => u.Roles).ThenInclude(r => r.Claims)
            .Where(u => u.Id == id)
            .Single();
    }
    
    public int GetUserId()
    {
        return _user.Id;
    }

    public UserDetailResponseDto GetUserDetail()
    {
        return new UserDetailResponseDto(_user);
    }

    // Authorization for users.
    public UserListAuthorizationResponseDto GetUserListAuthorization()
    {
        return new UserListAuthorizationResponseDto
        {
            CanCreate = _user.HasPermission(PermissionConstants.CreateUser)
        };
    }

    public UserBasicAuthorizationResponseDto GetUserBasicAuthorization(User targetUser)
    {
        return new UserBasicAuthorizationResponseDto
        {
            CanEdit = CanEditUserPersonalInformation(targetUser) ||
                CanEditUserUserInformation(targetUser),
            CanChangePassword = CanChangeUserPassword(targetUser),
            CanResetPassword = CanResetUserPassword(targetUser),
            CanDelete = CanDeleteUser(targetUser)
        };
    }

    public UserDetailAuthorizationResponseDto GetUserDetailAuthorization(User targetUser)
    {
        UserBasicAuthorizationResponseDto basicResponseDto;
        basicResponseDto = GetUserBasicAuthorization(targetUser);
        return new()
        {
            CanGetNote = CanGetNote(targetUser.PowerLevel),
            CanEdit = basicResponseDto.CanEdit,
            CanEditUserPersonalInformation = CanEditUserPersonalInformation(targetUser),
            CanEditUserUserInformation = CanEditUserUserInformation(targetUser),
            CanAssignRole = CanAssignRole(),
            CanChangePassword = CanChangeUserPassword(targetUser),
            CanResetPassword = CanResetUserPassword(targetUser),
            CanDelete = CanDeleteUser(targetUser)
        };
    }

    // Authorization for customers.
    public CustomerListAuthorizationResponseDto GetCustomerListAuthorization()
    {
        return new CustomerListAuthorizationResponseDto
        {
            CanCreate = _user.HasPermission(PermissionConstants.CreateCustomer)
        };
    }

    public CustomerAuthorizationResponseDto GetCustomerAuthorization(Customer customer)
    {
        return new CustomerAuthorizationResponseDto
        {
            CanEdit = _user.HasPermission(PermissionConstants.EditCustomer),
            CanDelete = _user.HasPermission(PermissionConstants.DeleteCustomer),
            CanCreateDebt = CanCreateDebtIncurrence(),
            CanCreateDebtPayment = CanCreateDebtPayment()
        };
    }

    // Authorization for brands.
    public BrandListAuthorizationResponseDto GetBrandListAuthorization()
    {
        return new BrandListAuthorizationResponseDto
        {
            CanCreate = _user.HasPermission(PermissionConstants.CreateBrand)
        };
    }

    public BrandAuthorizationResponseDto GetBrandAuthorization()
    {
        return new()
        {
            CanEdit = _user.HasPermission(PermissionConstants.EditBrand),
            CanDelete = _user.HasPermission(PermissionConstants.DeleteBrand)
        };
    }

    // Authorization for products.
    public ProductListAuthorizationResponseDto GetProductListAuthorization()
    {
        return new ProductListAuthorizationResponseDto
        {
            CanCreate = _user.HasPermission(PermissionConstants.CreateProduct)
        };
    }

    public ProductAuthorizationResponseDto GetProductAuthorization(Product product)
    {
        return new ProductAuthorizationResponseDto
        {
            CanEdit = _user.HasPermission(PermissionConstants.EditProduct),
            CanDelete = _user.HasPermission(PermissionConstants.DeleteProduct)
        };
    }

    // Authorization for product categories.
    public ProductCategoryAuthorizationResponseDto GetProductCategoryAuthorization()
    {
        return new()
        {
            CanEdit = _user.HasPermission(PermissionConstants.EditProductCategory),
            CanDelete = _user.HasPermission(PermissionConstants.DeleteProductCategory)
        };
    }

    // Authorization for supplies.
    public SupplyListAuthorizationResponseDto GetSupplyListAuthorization()
    {
        return new SupplyListAuthorizationResponseDto
        {
            CanCreate = CanCreateSupply()
        };
    }

    public SupplyAuthorizationResponseDto GetSupplyAuthorization(Supply supply)
    {
        return new()
        {
            CanEdit = CanEditSupply(supply),
            CanDelete = CanEditSupply(supply),
            CanSetStatsDateTime = CanSetSupplyStatsDateTime()
        };
    }
    
    // Authorization for expenses.
    public ExpenseListAuthorizationResponseDto GetExpenseListAuthorization()
    {
        return new ExpenseListAuthorizationResponseDto
        {
            CanCreate = CanCreateExpense()
        };
    }

    public ExpenseAuthorizationResponseDto GetExpenseAuthorization(Expense expense)
    {
        return new ExpenseAuthorizationResponseDto
        {
            CanEdit = CanEditExpense(expense),
            CanDelete = CanDeleteExpense(expense),
            CanSetStatsDateTime = CanSetExpenseStatsDateTime()
        };
    }

    // Authorization for orders.
    public OrderListAuthorizationResponseDto GetOrderListAuthorization()
    {
        return new OrderListAuthorizationResponseDto
        {
            CanCreate = CanCreateOrder()
        };
    }

    public OrderAuthorizationResponseDto GetOrderAuthorization(Order order)
    {
        return new OrderAuthorizationResponseDto
        {
            CanEdit = CanEditOrder(order),
            CanDelete = CanDeleteOrder(order),
            CanSetStatsDateTime = CanSetOrderStatsDateTime()
        };
    }
    
    // Authorization for treatments.
    public TreatmentListAuthorizationResponseDto GetTreatmentListAuthorization()
    {
        return new TreatmentListAuthorizationResponseDto
        {
            CanCreate = CanCreateTreatment()
        };
    }
    
    public TreatmentAuthorizationResponseDto GetTreatmentAuthorization(Treatment treatment)
    {
        return new TreatmentAuthorizationResponseDto
        {
            CanEdit = CanEditTreatment(treatment),
            CanDelete = CanDeleteTreatment(treatment),
            CanSetPaidDateTime = CanSetTreatmentStatsDateTime()
        };
    }

    // Authorization for debt incurrences.
    public DebtIncurrenceListAuthorizationResponseDto GetDebtIncurrenceListAuthorization()
    {
        return new DebtIncurrenceListAuthorizationResponseDto
        {
            CanCreate = CanCreateDebtIncurrence()
        };
    }

    public DebtIncurrenceAuthorizationResponseDto GetDebtIncurrenceAuthorization(
            DebtIncurrence debtIncurrence)
    {
        return new DebtIncurrenceAuthorizationResponseDto
        {
            CanEdit = CanEditDebtIncurrence(debtIncurrence),
            CanDelete = CanDeleteDebtIncurrence(debtIncurrence),
            CanSetIncurredDateTime = CanSetDebtIncurrenceStatsDateTime()
        };
    }

    // Authorization for debt payments.
    public DebtPaymentListAuthorizationResponseDto GetDebtPaymentListAuthorization()
    {
        return new DebtPaymentListAuthorizationResponseDto
        {
            CanCreate = CanCreateDebtPayment()
        };
    }

    public DebtPaymentAuthorizationResponseDto GetDebtPaymentAuthorization(
            DebtPayment debtPayment)
    {
        return new DebtPaymentAuthorizationResponseDto
        {
            CanEdit = CanEditDebtPayment(debtPayment),
            CanDelete = CanDeleteDebtPayment(debtPayment),
            CanSetStatsDateTime = CanSetDebtPaymentStatsDateTime()
        };
    }

    // Authorization for consultants.
    public ConsultantListAuthorizationResponseDto GetConsultantListAuthorization()
    {
        return new ConsultantListAuthorizationResponseDto
        {
            CanCreate = CanCreateConsultant()
        };
    }

    public ConsultantAuthorizationResponseDto GetConsultantAuthorization(Consultant consultant)
    {
        return new ConsultantAuthorizationResponseDto
        {
            CanEdit = CanEditConsultant(consultant),
            CanDelete = CanDeleteConsultant(consultant),
            CanSetStatsDateTime = CanSetConsultantStatsDateTime(),
            CanAccessUpdateHistories = CanAccessConsultantUpdateHistories()
        };
    }

    // Permissions to interact with users.
    public bool CanEditUserPersonalInformation(User targetUser)
    {
        // Check permission when the user is editing himself.
        if (_user.Id == targetUser.Id &&
            _user.HasPermission(PermissionConstants.EditSelfPersonalInformation))
        {
            return true;
        }

        // Check permission when the user is editing another user.
        else if (_user.HasPermission(PermissionConstants.EditOtherUserPersonalInformation) &&
                _user.PowerLevel > targetUser.PowerLevel)
        {
            return true;
        }

        return false;
    }

    public bool CanEditUserUserInformation(User targetUser)
    {
        // Check permission when the user is editing himself.
        if (_user.Id == targetUser.Id &&
            _user.HasPermission(PermissionConstants.EditSelfUserInformation))
        {
            return true;
        }

        // Check permission when the user is editing another user.
        else if (_user.HasPermission(PermissionConstants.EditOtherUserUserInformation) &&
                _user.PowerLevel > targetUser.PowerLevel)
        {
            return true;
        }

        return false;
    }

    public bool CanChangeUserPassword(User targetUser)
    {
        return _user.Id == targetUser.Id;
    }

    public bool CanResetUserPassword(User targetUser)
    {
        return _user.Id != targetUser.Id &&
            _user.HasPermission(PermissionConstants.ResetOtherUserPassword) &&
            _user.PowerLevel > targetUser.PowerLevel;
    }

    public bool CanDeleteUser(User targetUser)
    {
        return _user.Id != targetUser.Id &&
            _user.HasPermission(PermissionConstants.DeleteUser) &&
            !_user.IsDeleted &&
            _user.PowerLevel > targetUser.PowerLevel;
    }

    public bool CanRestoreUser(User targetUser)
    {
        return _user.Id != targetUser.Id &&
                _user.IsDeleted &&
                _user.HasPermission(PermissionConstants.RestoreUser);
    }

    public bool CanAssignToRole(Role role)
    {
        return _user.Role.Name == RoleConstants.Developer ||
            _user.Role.Name == RoleConstants.Manager ||
            _user.PowerLevel > role.PowerLevel;
    }

    public bool CanAssignRole()
    {
        return _user.HasPermission(PermissionConstants.AssignRole);
    }

    public bool CanGetNote(int powerLevel)
    {
        return _user.HasPermission(PermissionConstants.GetOtherUserNote) &&
            _user.PowerLevel > powerLevel;
    }

    // Permissions to interact with supplies.
    public bool CanCreateSupply()
    {
        return _user.HasPermission(PermissionConstants.CreateSupply);
    }

    public bool CanEditSupply(Supply supply)
    {
        if (!_user.HasPermission(PermissionConstants.EditSupply))
        {
            return false;
        }

        if (supply.IsLocked && !_user.HasPermission(PermissionConstants.EditLockedSupply))
        {
            return false;
        }

        return true;
    }

    public bool CanDeleteSupply(Supply supply)
    {
        return !supply.IsLocked && _user.HasPermission(PermissionConstants.DeleteSupply);
    }

    public bool CanSetSupplyStatsDateTime()
    {
        return _user.HasPermission(PermissionConstants.SetSupplyStatsDateTime);
    }
    
    public bool CanAccessSupplyUpdateHistories()
    {
        return _user.HasPermission(PermissionConstants.AccessSupplyUpdateHistories);
    }
    
    // Permissions to interact with expenses.
    public bool CanCreateExpense()
    {
        return _user.HasPermission(PermissionConstants.CreateExpense);
    }
    
    public bool CanEditExpense(Expense expense)
    {
        if (!_user.HasPermission(PermissionConstants.EditExpense))
        {
            return false;
        }
        
        if (expense.IsLocked && !_user.HasPermission(PermissionConstants.EditLockedExpense))
        {
            return false;
        }
        
        return true;
    }
    
    public bool CanDeleteExpense(Expense expense)
    {
        return !expense.IsLocked && _user.HasPermission(PermissionConstants.DeleteExpense);
    }

    public bool CanSetExpenseStatsDateTime()
    {
        return _user.HasPermission(PermissionConstants.SetExpenseStatsDateTime);
    }
    
    public bool CanAccessExpenseUpdateHistories()
    {
        return _user.HasPermission(PermissionConstants.AccessExpenseUpdateHistories);
    }
    
    // Permissions to interact with orders.
    public bool CanCreateOrder()
    {
        return _user.HasPermission(PermissionConstants.CreateOrder);
    }

    public bool CanEditOrder(Order order)
    {
        if (!_user.HasPermission(PermissionConstants.EditOrder))
        {
            return false;
        }
        
        if (order.IsLocked && !_user.HasPermission(PermissionConstants.EditLockedOrder))
        {
            return false;
        }
        
        return true;
    }
    
    public bool CanDeleteOrder(Order order)
    {
        return !order.IsLocked && _user.HasPermission(PermissionConstants.DeleteOrder);
    }

    public bool CanSetOrderStatsDateTime()
    {
        return _user.HasPermission(PermissionConstants.SetOrderStatsDateTime);
    }
    
    public bool CanAccessOrderUpdateHistories()
    {
        return _user.HasPermission(PermissionConstants.AccessOrderUpdateHistories);
    }
    
    // Permissions to interact with treatments.
    public bool CanCreateTreatment()
    {
        return _user.HasPermission(PermissionConstants.CreateTreatment);
    }
    
    public bool CanEditTreatment(Treatment treatment)
    {
        if (!_user.HasPermission(PermissionConstants.EditTreatment))
        {
            return false;
        }
        
        if (treatment.IsLocked && !_user.HasPermission(PermissionConstants.EditLockedTreatment))
        {
            return false;
        }
        
        return true;
    }
    
    public bool CanDeleteTreatment(Treatment treatment)
    {
        return !treatment.IsLocked && _user.HasPermission(PermissionConstants.DeleteTreatment);
    }

    public bool CanSetTreatmentStatsDateTime()
    {
        return _user.HasPermission(PermissionConstants.SetTreatmentStatsDateTime);
    }
    
    public bool CanAccessTreatmentUpdateHistories()
    {
        return _user.HasPermission(PermissionConstants.AccessTreatmentUpdateHistories);
    }
    
    // Permisisons to interact with debts.
    public bool CanCreateDebtIncurrence()
    {
        return _user.HasPermission(PermissionConstants.CreateDebtIncurrence);
    }

    public bool CanEditDebtIncurrence(DebtIncurrence debtIncurrence)
    {
        if (!_user.HasPermission(PermissionConstants.EditDebtIncurrence))
        {
            return false;
        }
        
        if (debtIncurrence.IsLocked && !_user.HasPermission(PermissionConstants.EditLockedDebtIncurrence))
        {
            return false;
        }
        
        return true;
    }
    
    public bool CanDeleteDebtIncurrence(DebtIncurrence debtIncurrence)
    {
        return !debtIncurrence.IsLocked &&
            _user.HasPermission(PermissionConstants.DeleteDebtIncurrence);
    }
    
    public bool CanSetDebtIncurrenceStatsDateTime()
    {
        return _user.HasPermission(PermissionConstants.SetDebtIncurrenceStatsDateTime);
    }
    
    public bool CanAccessDebtIncurrenceUpdateHistories()
    {
        return _user.HasPermission(PermissionConstants.AccessDebtIncurrenceUpdateHistories);
    }

    // Permissions to interact with debt payments.
    public bool CanCreateDebtPayment()
    {
        return _user.HasPermission(PermissionConstants.CreateDebtPayment);
    }

    public bool CanEditDebtPayment(DebtPayment debtPayment)
    {
        if (!_user.HasPermission(PermissionConstants.EditDebtIncurrence))
        {
            return false;
        }

        if (debtPayment.IsLocked && !_user.HasPermission(PermissionConstants.EditLockedDebtIncurrence))
        {
            return false;
        }

        return true;
    }

    public bool CanDeleteDebtPayment(DebtPayment debtPayment)
    {
        return !debtPayment.IsLocked &&
            _user.HasPermission(PermissionConstants.DeleteDebtPayment);
    }

    public bool CanSetDebtPaymentStatsDateTime()
    {
        return _user.HasPermission(PermissionConstants.SetDebtPaymentStatsDateTime);
    }
    
    public bool CanAccessDebtPaymentUpdateHistories()
    {
        return _user.HasPermission(PermissionConstants.AccessDebtPaymentUpdateHistories);
    }

    // Permissions to interact with consultant.
    public bool CanCreateConsultant()
    {
        return _user.HasPermission(PermissionConstants.CreateConsultant);
    }

    public bool CanEditConsultant(Consultant consultant)
    {
        if (!_user.HasPermission(PermissionConstants.EditConsultant))
        {
            return false;
        }

        if (consultant.IsLocked &&
            !_user.HasPermission(PermissionConstants.EditLockedConsultant))
        {
            return false;
        }

        return true;
    }

    public bool CanDeleteConsultant(Consultant consultant)
    {
        return !consultant.IsLocked &&
            _user.HasPermission(PermissionConstants.DeleteConsultant);
    }
    
    public bool CanSetConsultantStatsDateTime()
    {
        return _user.HasPermission(PermissionConstants.SetConsultantStatsDateTime);
    }
    
    public bool CanAccessConsultantUpdateHistories()
    {
        return _user.HasPermission(PermissionConstants.AccessConsultantUpdateHistories);
    }
}
