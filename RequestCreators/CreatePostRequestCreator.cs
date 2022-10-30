public class CreatePostRequestCreator : BaseRequestCreator
{
    private PostModel postModel;
    public CreatePostRequestCreator()
    {
      SetBaseAddressMethod(() =>
      {
          return "https://jsonplaceholder.typicode.com/";
      });

      SetHttpMethod(HttpMethod.Post);
    }

    public PostModel CreatePost(PostModel post)
    {
        postModel = post;

        var responseContent = base.MakeRequest();
        return JsonSerializer.Deserialize<PostModel>(responseContent);
    }
    protected virtual object GetBodyObject()
    {
        return postModel;
    }
    protected string GetBaseAddress()
    {
        return "https://jsonplaceholder.typicode.com/";
    }

    protected override string GetUrlPath()
    {
        return "posts";
    }
}