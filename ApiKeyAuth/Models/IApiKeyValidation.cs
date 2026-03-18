namespace ApiKeyAuth.Models;

public interface IApiKeyValidation
{
    bool IsValidApiKey(string userApiKey);
}