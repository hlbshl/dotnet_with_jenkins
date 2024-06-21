using log4net;
using RestSharp;

namespace PracticalTaskSelenium.Core.Utilities;

public static class ApiHelper
{
    private static ILog _log;

    static ApiHelper()
    {
        _log = LogManager.GetLogger(typeof(ApiHelper));
    }

    public static async Task<RestResponse<T>> ExecuteAsyncRequest<T>(RestClient client, RestRequest request) where T : new()
    {
        var response = await client.ExecuteAsync<T>(request);
        LogCreation(response);
        return response;
    }

    public static async Task<RestResponse> ExecuteAsyncRequest(RestClient client, RestRequest request)
    {
        var response = await client.ExecuteAsync(request);
        LogCreation(response);
        return response;
    }

    private static void LogCreation(RestResponse response)
    {
        _log.Info("Executing request");
        _log.Info("The status code is " + (int)response.StatusCode);
    }
}