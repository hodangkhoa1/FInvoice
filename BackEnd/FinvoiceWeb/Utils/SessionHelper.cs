using Newtonsoft.Json;

namespace FinvoiceWeb.Utils
{
    public static class SessionHelper
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static IEnumerable<T> GetListFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(IEnumerable<T>) : JsonConvert.DeserializeObject<IEnumerable<T>>(value);
        }
    }
}
