public abstract class BaseRequestCreator
{
    public BaseRequestCreator()
    {
        makeRequestDelegate = MakeGetRequest;
    }

    protected delegate string GetBaseAddressDelegate();
    GetBaseAddressDelegate getBaseAddressDelegateMethod;

    private delegate string MakeRequestDelegate();
    MakeRequestDelegate makeRequestDelegate;

    public delegate void RequestStartedDelegate();
    RequestStartedDelegate requestStartedMethod;

    Func<int> requestCountFunc;

    private HttpMethod httpMethod;


    protected void SetRequestCount(int count)
    {
        requestCountFunc = () => count;
    }
    protected void SetBaseAddressMethod(GetBaseAddressDelegate paramDelegateMethod)
    {
        getBaseAddressDelegateMethod = paramDelegateMethod;
    }

    protected void SetRequestStartedMethod(RequestStartedDelegate requestStartedMethod)
    {
        this.requestStartedMethod = requestStartedMethod;
    }
    private string MakeGetRequest()
    {
        HttpClient client = new HttpClient();
        var baseAddress = getBaseAddressDelegateMethod.Invoke();

        var message = new HttpRequestMessage()
        {
            Method = GetHttpMethod(),
            RequestUri = new Uri(baseAddress + GetUrlPath())
        };

        var httpResponse = client.Send(message);

        httpResponse.EnsureSuccessStatusCode();

        var resultContent = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        return resultContent;
    }

    private string MakePostRequest()
    {
        HttpClient client = new HttpClient();
        var baseAddress = getBaseAddressDelegateMethod.Invoke();

        var message = new HttpRequestMessage()
        {
            Method = httpMethod,
            RequestUri = new Uri(baseAddress + GetUrlPath())
        };

        var bodyContent = GetBodyObject();
        if (bodyContent != null)
            message.Content = new StringContent(JsonSerializer.Serialize(bodyContent));

        var httpResponse = client.Send(message);

        httpResponse.EnsureSuccessStatusCode();

        var resultContent = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        return resultContent;
    }

    protected string MakeRequest()
    {
        var requestCount = Math.Max(requestCountFunc.Invoke(),1);

        while ((requestCount--) > 0)
            requestStartedMethod.Invoke();


        return makeRequestDelegate.Invoke();
    }

    protected virtual string GetUrlPath()
    {
        return "";
    }

    protected virtual HttpMethod SetHttpMethod(HttpMethod method)
    {
        if (method == HttpMethod.Post)
            makeRequestDelegate = MakePostRequest;

        return httpMethod = method;
    }

    protected virtual object GetBodyObject()
    {
        return default;
    }
}