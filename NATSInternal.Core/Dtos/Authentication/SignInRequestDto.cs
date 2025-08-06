namespace NATSInternal.Core.Dtos;

public class SignInRequestDto : IRequestDto {
    public string UserName { get; set; }
    public string Password { get; set; }
    
    public void TransformValues() {
    }
}