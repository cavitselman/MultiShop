using Microsoft.EntityFrameworkCore;
using MS.Comment.Context;
using MS.Comment.Entities;
using System.Net.Http;

namespace MS.Comment.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private readonly CommentContext _commentContext;

        public CommentService(CommentContext commentContext)
        {
            _commentContext = commentContext;
        }

        // Ürün ID'sine göre yorumları al
        public async Task<List<UserComment>> GetCommentListByProductIdAsync(string productId)
        {
            return await _commentContext.UserComments
                .Where(c => c.ProductId == productId)
                .ToListAsync();
        }

        // Birden fazla ürün ID'si için yorum sayısı al
        public async Task<Dictionary<string, int>> GetCommentCountsByProductIdsAsync(List<string> productIds)
        {
            if (productIds == null || !productIds.Any())
            {
                return new Dictionary<string, int>();
            }

            var commentCounts = await _commentContext.UserComments
                .Where(c => c.ProductId != null && productIds.Contains(c.ProductId))
                .GroupBy(c => c.ProductId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            return commentCounts;
        }
    }
}
