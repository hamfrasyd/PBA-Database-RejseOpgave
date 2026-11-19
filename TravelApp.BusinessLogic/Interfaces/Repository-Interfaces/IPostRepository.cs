using TravelApp.BusinessLogic.DTOs.Posts;
using TravelApp.BusinessLogic.Exceptions.SpecificCustomException;
using TravelApp.BusinessLogic.Models.Posts;

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
        public Task<Post> AddPostAsync(Post post);

        /// <summary>Updates an existing post. A null content leaves the stored content unchanged.</summary>
        /// <param name="post">The post fields to update.</param>
        /// <exception cref="TravelAppNotFoundException">Thrown when the post does not exist.</exception>
        public Task UpdatePostAsync(UpdatePostDTO post);

        /// <summary>Delete a specific post.</summary>
        public Task DeletePostAsync(int postId);
    }
}
