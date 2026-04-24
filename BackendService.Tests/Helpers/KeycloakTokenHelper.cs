using System.Text.Json;
using System.Text.Json.Serialization;

namespace BackendService.Tests.Helpers
{
    public static class KeycloakTokenHelper
    {
        private const string KeycloakUrl = "http://host.docker.internal:8090";
        private const string Realm = "auth-dev";
        private const string ClientId = "backend-service";
        private const string ClientSecret = "hsHuglPJjFv20MJIkTkJTuw7GCosTlD3";

        public static async Task<string> GetToken(string username, string password)
        {
            using var client = new HttpClient();

            var requestUrl = $"{KeycloakUrl}/realms/{Realm}/protocol/openid-connect/token";

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            var response = await client.PostAsync(requestUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Keycloak token error: {response.StatusCode}, {error}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<KeycloakTokenResponse>(json);

            return tokenResponse.AccessToken;
        }
    }

    public class KeycloakTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
