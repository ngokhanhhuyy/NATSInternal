namespace NATSInternal.Core.Dtos;

public class AccessTokenExchangeRequestDto : IRequestDto
{
    public string ExpiredAccessToken { get; set; }
    public string RefreshToken { get; set; }

    public void TransformValues()
    {
        ExpiredAccessToken = ExpiredAccessToken?.ToNullIfEmpty();
        RefreshToken = ExpiredAccessToken?.ToNullIfEmpty();
    }
}
