using LineNotifyService.API.Helper;
using LineNotifyService.API.ViewModels;
using LineNotifyService.API.ViewModels.BaseViewModel;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LineNotifyService.API.Repositories
{
    public class LineNotifyServiceRepository
    {
        private readonly ILogger logger;
        private readonly IConfiguration configuration;
        private readonly IService lineNotifyService;
        private readonly IMemoryCache memoryCache;
        private string _access_token = "access_token";

        public LineNotifyServiceRepository(ILogger<LineNotifyServiceRepository> logger, IConfiguration configuration, IService lineNotifyService, IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.lineNotifyService = lineNotifyService;
            this.memoryCache = memoryCache;
        }

        public APIResult OAuthLineNotifyUrl(GetOAuthParam param)
        {
            try
            {
                var endpoint_url = lineNotifyService.GetOAuthLoginRedirectUrl(param.redirect_uri, param.user_id);
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("endpoint_url", endpoint_url);
                return this.APIResult(StatusCode.Success, values);
            }
            catch (Exception ex)
            {
                return this.APIResult(StatusCode.Fail, ex);
            }
        }

        public async Task<APIResult> OAuthExchangeCode(GetTokenParam param)
        {
            try
            {
                var access_token = await lineNotifyService.GetToken(param.redirect_uri, param.code);
                SetAccessToken(param.user_id, access_token.access_token);
                return this.APIResult(StatusCode.Success);
            }
            catch (Exception ex)
            {
                return this.APIResult(StatusCode.Fail, ex);
            }
        }

        public async Task<APIResult> SendNotify(NotifyParam param)
        {
            try
            {
                var access_token = GetAccessToken(param.user_id);
                var result = await lineNotifyService.SendNotify(access_token, param.message);
                return this.APIResult(StatusCode.Success, result);
            }
            catch (Exception ex)
            {
                return this.APIResult(StatusCode.Fail, ex);
            }
        }

        public async Task<APIResult> CheckStatus(CheckStatusParam param)
        {
            try
            {
                var access_token = GetAccessToken(param.user_id);
                var result = await lineNotifyService.CheckStatus(access_token);
                return this.APIResult(StatusCode.Success, result);
            }
            catch (Exception ex)
            {
                return this.APIResult(StatusCode.Fail, ex);
            }
        }

        public async Task<APIResult> RevokeToken(RevokeTokenParam param)
        {
            try
            {
                var access_token = GetAccessToken(param.user_id);
                var result = await lineNotifyService.RevokeToken(access_token);
                return this.APIResult(StatusCode.Success, result);
            }
            catch (Exception ex)
            {
                return this.APIResult(StatusCode.Fail, ex);
            }
        }


        string GetAccessToken(string user_id)
        {
            //TODO: ค่าจาก DataBase
            return (string)memoryCache.Get(_access_token);
        }
        void SetAccessToken(string user_id, string access_token)
        {
            //TODO: บันทึก access_token ลง DataBase
            memoryCache.Set(_access_token, access_token);
        }
    }

    public enum StatusCode
    {
        Success = 200,
        Fail = 500,
    }
    public static class Extensions
    {
        public static APIResult APIResult(this object value, Enum status)
        {
            return new APIResult
            {
                status = (int)(object)status,
            };
        }
        public static APIResult<T> APIResult<T>(this object value, Enum status, T data)
        {
            return new APIResult<T>
            {
                status = (int)(object)status,
                data = data,
            };
        }

    }
}

