using RestSharp;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApiClient
{
    class Program
    {
        private const string WebApiGet = "https://localhost:44311/api/OrderItems";

        private const string IdentityServer = "https://localhost:5001/";

        public static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            string content = await GetTest();
            
            Console.WriteLine(content);
            Console.ReadLine();
        }

        private static async Task<string> GetTest()
        {
            var apiClient = new RestClient();
            var restRequest = new RestRequest(WebApiGet);

            string accessToken = await GetAccessToken();
            restRequest.AddHeader("authorization", $"Bearer {accessToken}");

            var result = await apiClient.GetAsync<string>(restRequest);

            return result;
        }

        private static async Task<string> GetAccessToken()
        {
            var tokenClient = new RestClient($"{IdentityServer}connect/token");

            var tokenRequest = new RestRequest(Method.POST);
            tokenRequest.AddHeader("content-type", "application/x-www-form-urlencoded");

            var grantType = "client_credentials";
            var clientId = "oauthClient";
            var scope = "api1.read";
            var secret = "secret";

            tokenRequest.AddParameter("client_id", clientId, ParameterType.GetOrPost);
            tokenRequest.AddParameter("grant_type", grantType, ParameterType.GetOrPost);
            tokenRequest.AddParameter("scope", scope, ParameterType.GetOrPost);
            tokenRequest.AddParameter("client_secret", secret, ParameterType.GetOrPost);

            IRestResponse response = await tokenClient.ExecuteAsync(tokenRequest);

            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(response.Content);

            return tokenResponse.AccessToken;
        }
    }

    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}
