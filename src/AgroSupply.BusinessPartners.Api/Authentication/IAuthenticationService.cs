namespace AgroSupply.BusinessPartners.Api.Authentication;

public interface IAuthenticationService
{
    string? Authenticate(
        string username,
        string password);
}