using TravelApp.BusinessLogic.Handlers.Posts.GetPost;
using TravelApp.BusinessLogic.Handlers.Posts;
using TravelApp.BusinessLogic.Models;

namespace TravelApp.BusinessLogic.Interfaces
{
    /// <summary>Defines database operations for posts.</summary>
    public interface IPostRepository
    {
        /// <summary>Get a specific post.</summary>
        public Task<Post?> GetPostByIdAsync(int postId);

        /// <summary>Retrieves posts for a specific user.</summary>
        public Task<List<Post>> GetPostsByUserIdAsync(int userId);

        /// <summary>Adds a new post.</summary>
        public Task AddPostAsync(Post post);

        /// <summary>Updates an existing post.</summary>
        public Task UpdatePostAsync(Post post);

        /// <summary>Delete a specific post.</summary>
        public Task DeletePostAsync(int postId);
    }
}
