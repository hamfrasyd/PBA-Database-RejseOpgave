using Azure.Core;
using Microsoft.EntityFrameworkCore;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Handlers.Posts;
using TravelApp.BusinessLogic.Interfaces;
using TravelApp.BusinessLogic.Models;
using TravelApp.DataAccess.SQLRepositories.EntityFramework;

namespace TravelApp.DataAccess.SQLRepositories
{
    /// <summary>Provides SQL-based data access operations for posts.</summary>
    public class SqlPostRepository : IPostRepository
    {
        private readonly TravelAppContext _context;

        public SqlPostRepository(TravelAppContext context)
        {
            _context = context;
        }

        /// <summary>Gets a specific post by its ID.</summary>
        public async Task<Post?> GetPostByIdAsync(int postId)
        {
            Post? matchingPost = await _context.PostEntities.AsNoTracking().Include(p => p.User).FirstOrDefaultAsync(post => post.Id == postId);
            return matchingPost;
        }

        /// <summary>Gets all posts made by a specific user</summary>
        public async Task<List<Post>> GetPostsByUserIdAsync(int userId)
        {
            IQueryable<Post> posts = _context.PostEntities.AsNoTracking().Include(p => p.User).Where(post => post.User.Id == userId);
            List<Post> postList = await posts.ToListAsync();
            return postList;
        }

        /// <summary>Adds a post</summary>
        public async Task<Post> AddPostAsync(Post post)
        {
            var createdUser = await _context.PostEntities.AddAsync(post);
            return createdUser.Entity;
        }

        /// <inheritdoc/>
        public async Task UpdatePostAsync(Post post)
        {
            _context.PostEntities.Update(post);
        }

        /// <inheritdoc/>
        public async Task<bool> DeletePostAsync(int postId)
        {
            if (_context.Database.CurrentTransaction is null)
            {
                await _context.Database.BeginTransactionAsync();
            }

            int deletedRows = await _context.PostEntities.Where(post => post.Id == postId).ExecuteDeleteAsync();

            bool wasDeleted = deletedRows > 0;

            return wasDeleted;
        }
    }
}
