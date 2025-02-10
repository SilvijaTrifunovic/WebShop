using Newtonsoft.Json;

namespace WebShopSK.Extensions
{
	public static class SessionExtension
	{
		public static void SetObjectAsJSON(this ISession session, string key, object value)
		{
			session.SetString(key, JsonConvert.SerializeObject(value) );
		}

		public static T GetObjectFromJSON<T>(this ISession session, string key) 
		{
			var value = session.GetString(key);
			return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
		}
	}
}
