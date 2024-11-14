using NATSInternal.Services.Localization;

namespace NATSInternal.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UtilityController : ControllerBase
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IAnnouncementService _announcementService;
    private readonly IBrandService _brandService;
    private readonly IConsultantService _consultantService;
    private readonly ICountryService _countryService;
    private readonly ICustomerService _customerService;
    private readonly IDebtIncurrenceService _debtIncurrenceService;
    private readonly IDebtPaymentService _debtPaymentService;
    private readonly IExpenseService _expenseService;
    private readonly IOrderService _orderService;
    private readonly IProductService _productService;
    private readonly IProductCategoryService _productCategoryService;
    private readonly IRoleService _roleService;
    private readonly ISupplyService _supplyService;
    private readonly ITreatmentService _treatmentService;
    private readonly IUserService _userService;

    public UtilityController(
            IAnnouncementService announcementService,
            IAuthorizationService authorizationService,
            IBrandService brandService,
            IConsultantService consultantService,
            ICountryService countryService,
            ICustomerService customerService,
            IDebtIncurrenceService debtIncurrenceService,
            IDebtPaymentService debtPaymentService,
            IExpenseService expenseService,
            IOrderService orderService,
            IProductService productService,
            IProductCategoryService productCategoryService,
            IRoleService roleService,
            ISupplyService supplyService,
            ITreatmentService treatmentService,
            IUserService userService)
    {
        _announcementService = announcementService;
        _authorizationService = authorizationService;
        _brandService = brandService;
        _consultantService = consultantService;
        _countryService = countryService;
        _customerService = customerService;
        _debtIncurrenceService = debtIncurrenceService;
        _debtPaymentService = debtPaymentService;
        _expenseService = expenseService;
        _orderService = orderService;
        _productService = productService;
        _productCategoryService = productCategoryService;
        _roleService = roleService;
        _supplyService = supplyService;
        _treatmentService = treatmentService;
        _userService = userService;
    }

    [HttpGet("InitialData")]
    public async Task<IActionResult> GetInitialData()
    {
        return Ok(new InitialDataResponseDto
        {
            DisplayNames = DisplayNames.GetAll(),
            Announcement = new AnnouncementInitialResponseDto
            {
                CreatingPermission = _announcementService.GetCreatingPermission(),
            },
            Brand = new BrandInitialResponseDto
            {
                AllAsOptions = await _brandService.GetAllAsync(),
                CreatingPermission = _brandService.GetCreatingPermission()
            },
            Consultant = new ConsultantInitialResponseDto
            {
                ListSortingOptions = _consultantService.GetListSortingOptions(),
                ListMonthYearOptions = await _consultantService.GetListMonthYearOptionsAsync(),
                CreatingAuthorization = _consultantService.GetCreatingAuthorization()
            },
            Country = new CountryInitialResponseDto
            {
                AllAsOptions = await _countryService.GetAllAsync()
            },
            Customer = new CustomerInitialResponseDto
            {
                ListSortingOptions = _customerService.GetListSortingOptions(),
                CreatingPermission = _customerService.GetCreatingPermission()
            },
            DebtIncurrence = new DebtIncurrenceInitialResponseDto
            {
                ListSortingOptions = _debtIncurrenceService.GetListSortingOptions(),
                ListMonthYearOptions = await _debtIncurrenceService
                    .GetListMonthYearOptionsAsync(),
                CreatingAuthorization = _debtIncurrenceService.GetCreatingAuthorization()
            },
            DebtPayment = new DebtPaymentInitialResponseDto
            {
                ListSortingOptions = _debtPaymentService.GetListSortingOptions(),
                ListMonthYearOptions = await _debtPaymentService
                    .GetListMonthYearOptionsAsync(),
                CreatingAuthorization = _debtPaymentService.GetCreatingAuthorization()
            },
            Expense = new ExpenseInitialResponseDto
            {
                ListSortingOptions = _expenseService.GetListSortingOptions(),
                ListMonthYearOptions = await _expenseService.GetListMonthYearOptionsAsync(),
                CreatingAuthorization = _expenseService.GetCreatingAuthorization()
            },
            Order = new OrderInitialResponseDto
            {
                ListSortingOptions = _orderService.GetListSortingOptions(),
                ListMonthYearOptions = await _orderService.GetListMonthYearOptionsAsync(),
                CreatingAuthorization = _orderService.GetCreatingAuthorization()
            },
            Product = new ProductInitialResponseDto
            {
                ListSortingOptions = _productService.GetListSortingOptions(),
                CreatingPermission = _userService.GetCreatingPermission()
            },
            ProductCategory = new ProductCategoryInitialResponseDto
            {
                AllAsOptions = await _productCategoryService.GetAllAsync(),
                CreatingPermission = _userService.GetCreatingPermission()
            },
            Role = new RoleInitialResponseDto
            {
                AllAsOptions = await _roleService.GetAllAsync(),
            },
            Supply = new SupplyInitialResponseDto
            {
                ListSortingOptions = _supplyService.GetListSortingOptions(),
                ListMonthYearOptions = await _supplyService.GetListMonthYearOptionsAsync(),
                CreatingAuthorization = _supplyService.GetCreatingAuthorization()
            },
            Treatment = new TreatmentInitialResponseDto
            {
                ListSortingOptions = _treatmentService.GetListSortingOptions(),
                ListMonthYearOptions = await _treatmentService.GetListMonthYearOptionsAsync(),
                CreatingAuthorization = _treatmentService.GetCreatingAuthorization()
            },
            User = new UserInitialResponseDto
            {
                Detail = await _userService.GetDetailAsync(_authorizationService.GetUserId()),
                ListSortingOptions = _userService.GetListSortingOptions(),
                CreatingPermission = _userService.GetCreatingPermission()
            },
        });
    }

    [HttpGet("DisplayNames")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetDisplayNames()
    {
        return Ok(DisplayNames.GetAll());
    }
}