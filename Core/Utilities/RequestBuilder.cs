using log4net;
using RestSharp;

namespace PracticalTaskSelenium.Core.Utilities;

public class RequestBuilder
{
    private RestRequest _restRequest;
    private ILog _log;

    public RequestBuilder()
    {
        _log = LogManager.GetLogger(typeof(ApiHelper));
    }

    public RequestBuilder CreateRequest(string endpoint)
    {
        _log.Info("Creating new request");
        _restRequest = new RestRequest(endpoint);
        return this;
    }

    public RequestBuilder AddMethod(Method method)
    {
        _log.Info("Adding method to request");
        _restRequest.Method = method;
        return this;
    }

    public RequestBuilder AddJsonBody(object body)
    {
        _log.Info("Adding JSON body to a request");
        _restRequest.AddJsonBody(body);
        return this;
    }

    public RestRequest Build()
    {
        _log.Info("Building a request");
        return _restRequest;
    }
}