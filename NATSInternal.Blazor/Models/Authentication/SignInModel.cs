﻿namespace NATSInternal.Blazor.Models;

public class SignInModel : BaseModel<SignInModel>, IValidatableModel<SignInRequestDto>
{
    [Display(Name = DisplayNames.UserName)]
    public string UserName { get; set; }

    [Display(Name = DisplayNames.Password)]
    public string Password { get; set; }

    public SignInRequestDto ToRequestDto()
    {
        SignInRequestDto requestDto = new SignInRequestDto
        {
            UserName = UserName,
            Password = Password
        };
        requestDto.TransformValues();
        return requestDto;
    }
}