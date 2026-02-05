using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace CraftDailyCorner.Extensions
{
    //暫時不使用此文件 2026/2/5
    //Session 伺服器幫每個使用者暫時記住資料的小抽屜
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T? GetObjectFromJson<T>(this ISession session)
        {
            var value = session.GetString(typeof(T).Name);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}

