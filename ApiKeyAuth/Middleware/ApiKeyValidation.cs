using ApiKeyAuth.Models;

namespace ApiKeyAuth.Middleware;

public class ApiKeyValidation(IConfiguration configuration) : IApiKeyValidation
{
    public bool IsValidApiKey(string userApiKey)
    {
       if(string.IsNullOrWhiteSpace(userApiKey))
           return false;
       
       var apiKey = configuration.GetValue<string>(Constants.ApiKeyName);
       
       return apiKey != null && apiKey == userApiKey;
    }
}