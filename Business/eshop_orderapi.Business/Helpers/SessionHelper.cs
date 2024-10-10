using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using eshop_orderapi.Business.ViewModels;

namespace eshop_orderapi.Business.Helpers
{
    public static class SessionHelper
    {
        public static void Set<T>(this HttpContext context, string key, T value)
        {
            context.Session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this HttpContext context, string key)
        {
            var value = context.Session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public static bool Remove(this HttpContext context)
        {
            context.Session.Clear();
            var user = Get<LoginResponseModel>(context, Constant.AppicationUserData);
            if (user != null)
            {
                return false;
            }
            return true;
        }

        public static bool IsUserLogin(this HttpContext context)
        {
            var value = context.Session.GetString(Constant.AppicationUserData);
            return value == null ? false : true;
        }

        public static LoginResponseModel GetLoginUserInfo(this HttpContext context)
        {
            return Get<LoginResponseModel>(context, Constant.AppicationUserData);
        }

        public static string GetUserToken(this HttpContext context)
        {
            var user = Get<LoginResponseModel>(context, Constant.AppicationUserData);
            if (user != null)
            {
                return user.Token;
            }
            return string.Empty;
        }
    }
}