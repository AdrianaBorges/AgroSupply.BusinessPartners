namespace AgroSupply.BusinessPartners.Api.Contracts.Authentication;

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresInMinutes { get; set; }
}