namespace ProjectPRN222.Services
{
    public interface IUserRoleService
    {
        Task<bool> AssignRoleToUserAsync(string userId, string roleName);
        Task<bool> RemoveRoleFromUserAsync(string userId, string roleName);
        Task<bool> IsUserInRoleAsync(string userId, string roleName);
    }
}
