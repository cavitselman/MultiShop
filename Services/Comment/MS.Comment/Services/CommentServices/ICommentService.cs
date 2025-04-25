using MS.Comment.Entities;

namespace MS.Comment.Services.CommentServices
{
    public interface ICommentService
    {
        Task<List<UserComment>> GetCommentListByProductIdAsync(string productId);
        Task<Dictionary<string, int>> GetCommentCountsByProductIdsAsync(List<string> productIds);
    }
}
