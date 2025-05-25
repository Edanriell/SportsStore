using System.Text.Json;

namespace SportsStore.Infrastructure;

public static class SessionExtensions
{
	public static void SetJson ( this ISession session,
								 string key,
								 object value )
	{
		session.SetString (
				key : key,
				value : JsonSerializer.Serialize (
						value : value
					)
			);
	}

	public static T? GetJson <T> ( this ISession session, string key )
	{
		var sessionData = session.GetString (
				key : key
			);
		return sessionData == null
				   ? default(T)
				   : JsonSerializer.Deserialize<T> (
						   json : sessionData
					   );
	}
}
