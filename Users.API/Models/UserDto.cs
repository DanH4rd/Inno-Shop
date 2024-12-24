namespace InnoShop.Users.API.Models
{
    /// <summary>
    /// User DTO.
    /// </summary>
    public class UserDto : UserLoginInfo
    {
        public required string Name { get; set; }

        public UserRoles Role { get; set; }
    }
}
