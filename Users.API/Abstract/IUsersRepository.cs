using InnoShop.Users.API.Models;

namespace InnoShop.Users.API.Abstract
{
    /// <summary>
    /// Interface for Users repository.
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// Retrieves a user by login info.
        /// </summary>
        /// <param name="loginInfo">Login info of the user.</param>
        /// <returns>User if found, otherwise null.</returns>
        Task<User?> GetUserByLoginInfoAsync(UserLoginInfo loginInfo);

        /// <summary>
        /// Retrieves a user by ID.
        /// </summary>
        /// <param name="id">ID of the user.</param>
        /// <returns>User if found, otherwise null.</returns>
        Task<User?> GetUserByIdAsync(int id);

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>List of all users.</returns>
        Task<List<User>> GetAllUsersAsync();

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="userDto">User data to add.</param>
        /// <returns>User data added.</returns>
        Task<User> AddUserAsync(UserDto userDto);

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="id">User Id to update.</param>
        /// <param name="userDto">User data to update.</param>
        /// <returns>User data if found, otherwise null.</returns>
        Task<User?> UpdateUserAsync(int id, UserDto userDto);

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">ID of the user to delete.</param>
        /// <returns>Number of affected records.</returns>
        Task<int> DeleteUserAsync(int id);

        /// <summary>
        /// Restores a user by ID.
        /// </summary>
        /// <param name="id">ID of the user to restore.</param>
        /// <returns>Number of affected records.</returns>
        Task<int> RestoreUserAsync(int id);

        /// <summary>
        /// Confirms user email by confirmation token.
        /// </summary>
        /// <param name="token">confirmation token.</param>
        /// <returns>Number of affected records.</returns>
        Task<int> ConfirmUserEmailAsync(string token);

        /// <summary>
        /// Requests password restore. If email is found a link to restore password will be sent.
        /// </summary>
        /// <param name="email">user email</param>
        /// <returns>Password restore token.</returns>
        Task<string> RequestPasswordRestoreAsync(string email);

        /// <summary>
        /// Restores password and sends it to email.
        /// </summary>
        /// <param name="email">user email</param>
        /// <returns>tuple (email, password)</returns>
        Task<(string, string)> RestorePasswordAsync(string email);
    }
}
