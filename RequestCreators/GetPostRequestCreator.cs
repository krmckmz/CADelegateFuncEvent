using System.Net.Http;
using System;
using System.Collections.Generic;

public class GetPostRequestCreator : BaseRequestCreator
{
    public event EventHandler<int> OnRequestFinished;
    public GetPostRequestCreator()
    {
        SetBaseAddressMethod(() =>
        {
            return "https://jsonplaceholder.typicode.com/";
        });

        SetHttpMethod(HttpMethod.Get);
        SetRequestCount(2);
    }
    public List<PostModel> GetPosts()
    {
        var responseContent = MakeRequest();

        OnRequestFinished?.Invoke(this, responseContent.Length);

        return JsonSerializer.Deserialize<List<PostModel>>(responseContent);
    }

    protected override string GetBaseAddress()
    {
        return "https://jsonplaceholder.typicode.com/";
    }

    protected override string GetUrlPath()
    {
        return "posts";
    }
}