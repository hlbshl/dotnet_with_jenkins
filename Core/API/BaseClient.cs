using RestSharp;

namespace PracticalTaskSelenium.Core.API;

public class BaseClient
{
    private RestClient _client;

    public BaseClient(string url)
    {
        _client = new RestClient(url);
    }

    public RestClient Client => _client;
}