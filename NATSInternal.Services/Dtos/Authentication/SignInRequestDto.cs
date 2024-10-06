namespace NATSInternal.Services.Dtos;

public class SignInRequestDto : IRequestDto {
    public string UserName { get; init; }
    public string Password { get; init; }
    
    public void TransformValues() {
    }
}