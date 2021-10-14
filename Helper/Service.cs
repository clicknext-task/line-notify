using LineNotifyService.API.Helper.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace LineNotifyService.API.Helper
{
    public interface IService
    {
        string GetOAuthLoginRedirectUrl(string redirect_uri, string state);
        Task<ResultAccessToken> GetToken(string redirect_uri, string code);
        Task<ResultNotify> SendNotify(string access_token, string message);
        Task<ResultStatus> CheckStatus(string access_token);
        Task<ResultRevoke> RevokeToken(string access_token);
    }

    public class Service : IService
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory clientFactory;
        public Service(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            this.configuration = configuration;
            this.clientFactory = clientFactory;
        }
        public string GetOAuthLoginRedirectUrl(string redirect_uri, string state)
        {
            UriBuilder uriBuilder = new UriBuilder("https://notify-bot.line.me/oauth/authorize");
            uriBuilder.Query = QueryString.Create(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("response_type", "code"),
                new KeyValuePair<string, string>("client_id", configuration["Line:ClientId"]),
                new KeyValuePair<string, string>("redirect_uri", redirect_uri),
                new KeyValuePair<string, string>("scope", "notify"),
                new KeyValuePair<string, string>("state", state),
            }).Value;
            return uriBuilder.Uri.AbsoluteUri;
        }
        public async Task<ResultAccessToken> GetToken(string redirect_uri, string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                    throw new ArgumentNullException(nameof(code));

                var uri = new Uri("https://notify-bot.line.me/oauth/token");
                var client = clientFactory.CreateClient();
                client.DefaultRequestHeaders.Accept.Clear();
                var body = new FormUrlEncodedContent(new[]
                {
                     new KeyValuePair<string, string>("grant_type", "authorization_code"),
                     new KeyValuePair<string, string>("code", code),
                     new KeyValuePair<string, string>("redirect_uri", redirect_uri),
                     new KeyValuePair<string, string>("client_id", configuration["Line:ClientId"]),
                     new KeyValuePair<string, string>("client_secret", configuration["Line:ClientSecret"]),
                });
                var response = await client.PostAsync(uri, body);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Get token failed with status code: {response.StatusCode}");
                }
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ResultAccessToken>(json);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResultNotify> SendNotify(string access_token, string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(access_token))
                    throw new ArgumentNullException(nameof(access_token));
                else if (string.IsNullOrWhiteSpace(message))
                    throw new ArgumentNullException(nameof(message));

                var uri = new Uri("https://notify-api.line.me/api/notify");
                var client = clientFactory.CreateClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {access_token}");
                var body = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("message", message) });
                var response = await client.PostAsync(uri, body);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Send notify failed with status code: {response.StatusCode}");
                }
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ResultNotify>(json);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResultStatus> CheckStatus(string access_token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(access_token))
                    throw new ArgumentNullException(nameof(access_token));

                var uri = new Uri("https://notify-api.line.me/api/status");
                var client = clientFactory.CreateClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {access_token}");
                var response = await client.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Check status failed with status code: {response.StatusCode}");
                }
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ResultStatus>(json);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ResultRevoke> RevokeToken(string access_token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(access_token))
                    throw new ArgumentNullException(nameof(access_token));

                var uri = new Uri("https://notify-api.line.me/api/revoke");
                var client = clientFactory.CreateClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {access_token}");
                HttpContent body = null;
                var response = await client.PostAsync(uri, body);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Revoke token failed with status code: {response.StatusCode}");
                }
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ResultRevoke>(json);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
