namespace Domain.Posts;

public class PostImage
{
    public PostImageId Id { get; }
    
    public PostId PostId { get; }
    public Post? Post { get; }

    private PostImage(PostImageId id, PostId postId)
    {
        Id = id;
        PostId = postId;
    }

    public static PostImage New(PostImageId id, PostId postId) => new(id, postId);
    
    public string ImagePath => $"post_images/{PostId}/{Id}.png";
}