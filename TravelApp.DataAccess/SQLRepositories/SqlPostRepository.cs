using Microsoft.EntityFrameworkCore;
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
            Post? matchingPost = await _context.PostEntities.AsNoTracking().FirstOrDefaultAsync(post => post.Id == postId);
            return matchingPost;
        }

        /// <summary>Gets all posts made by a specific user</summary>
        public async Task<List<Post>> GetPostsByUserIdAsync(int userId)
        {
            IQueryable<Post> posts = _context.PostEntities.AsNoTracking().Where(post => post.User.Id == userId);
            List<Post> postList = await posts.ToListAsync();
            return postList;
        }
        
        /// <summary>Adds a post</summary>
        public async Task AddPostAsync(Post post)
        {
            await _context.PostEntities.AddAsync(post);
        }

        /// <summary>Updates a specific post</summary>
        public Task UpdatePostAsync(Post post)
        {
            
        }

        /// <summary>Delete a post by its id</summary>
        public Task DeletePostAsync(int postId)
        {
            throw new NotImplementedException();
        }
    }
}
