namespace SportHub.Infrastructure;

public static class UrlExtensions
{
    // PathAndQuery generates a URL. It helps us to grab query strings as well.
    public static string PathAndQuery(this HttpRequest request) => request.QueryString.HasValue
        ? $"{request.Path}{request.QueryString}" : request.Path.ToString();
}