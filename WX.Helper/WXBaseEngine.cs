using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WX.Model;

namespace WX.Helper
{
    public class WXBaseEngine
    {
        private string GET_ACCESS_TOKEN_URL = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
        private const string WXAccessTokenCacheKey = "WXAccessTokenCacheKey";
        private const int WXAccessTokenTimeOut = 7000;

        //默认使用的corpid、corpSecret
        public WXBaseEngine()
        {
            _corpid = "wx74a9df37cc009b3f";
            _corpsecret = "4fd83ecca6a6005ecd918154507bef16";
            _token = "NeweggWXToken";
            _encodingAESKey = "exhxcRVGB9vt6nCpA6uchb2aeexnFKSUjYtPngckujJ";

        }

        //可以自己填写corpid、corpSecret
        public WXBaseEngine(string corpid, string corpSecret)
        {
            _corpid = corpid;
            _corpsecret = corpSecret;
        }

        public string AccessToken
        {
            get
            {
                object accessToken = CacheHelper.Get<object>(WXAccessTokenCacheKey);
                if (accessToken == null)
                {
                    return NewAccessToken;
                }
                return accessToken.ToString();
            }
        }

        private string NewAccessToken
        {
            get
            {
                AccessToken token = WXReTryGet<AccessToken>(string.Format(GET_ACCESS_TOKEN_URL, CorpID, CorpSecret));
                CacheHelper.Update(WXAccessTokenCacheKey, token.access_token, WXAccessTokenTimeOut / 60);
                return token.access_token;
            }
        }

        //corpid是唯一的
        private string _corpid = "";
        public string CorpID
        {
            get
            {
                return _corpid;
            }
        }

        public string _corpsecret = "";
        public string CorpSecret
        {
            get
            {
                return _corpsecret;
            }
        }

        private string _token = "";
        public string Token
        {
            get { return _token; }
        }

        private string _encodingAESKey = "";
        public string EncodingAESKey
        {
            get { return _encodingAESKey; }
        }

        #region 辅助方法及属性

        public T WXReTryGet<T>(string url, string postData = "", HttpMethod method = HttpMethod.Get) where T : BaseModel
        {
            int count = 0;
            T result = null;
            do
            {
                string jsonResult = "";
                string newUrl = url;
                if (url.Contains("{AccessToken}"))
                    newUrl = url.Replace("{{AccessToken}}", AccessToken).Replace("{AccessToken}", AccessToken);
                try
                {
                    jsonResult = method == HttpMethod.Get ? HttpHelper.HttpGet(newUrl) : HttpHelper.HttpPost(newUrl, postData);
                    result = JsonConvert.DeserializeObject<T>(jsonResult);
                    if (result.errcode == 40001)
                    {
                        string token = NewAccessToken;
                    }
                }
                catch (Exception ex)
                {
                    ex.Data["From"] = "WXApiHttpRequestError";
                    ex.Data["WXApiUrl"] = newUrl;
                    ex.Data["postData"] = postData;
                    ex.Data["ReturnData"] = jsonResult;
                    LogHelper.Log(ex);
                }
                count++;
            } while ((result.errcode == -1 || result.errcode == 4001) && count < 3);
            return result;
        }

        #endregion

        public enum HttpMethod
        {
            Get,
            Post
        }


    }
}
