using System.Text.Json;

namespace SportHub.Infrastructure;

// Extension methods for ISession that add JSON serialization support.
// ISession natively stores only strings and byte arrays — these methods allow storing and retrieving any object type by serializing to/from JSON.
public static class SessionExtensionsCustomize
{
    // serializes object to JSON string and stores it in session under given key
    public static void SetJson(this ISession session, string key, object value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    // retrieves JSON string from session by key and deserializes it to type T.
    // returns default(T) if key doesn't exist in session
    public static T? GetJson<T>(this ISession session, string key)
    {
        var sessionData = session.GetString(key);
        return sessionData == null ? default(T) : JsonSerializer.Deserialize<T>(sessionData);
    }
}