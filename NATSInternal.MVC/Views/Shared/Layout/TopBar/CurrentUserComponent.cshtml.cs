namespace NATSInternal.Components;

public class CurrentUserComponent : ViewComponent
{
    private IAuthorizationService _authorizationService;

    public CurrentUserComponent(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public IViewComponentResult Invoke()
    {
        UserDetailResponseDto responseDto = _authorizationService.GetUserDetail();
        UserDetailModel model = new UserDetailModel(responseDto);
        return View("~/Views/Shared/Layout/TopBar/CurrentUserComponent.cshtml", model);
    }
}