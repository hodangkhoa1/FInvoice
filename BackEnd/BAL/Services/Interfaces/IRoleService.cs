using BAL.Models;
using DAL.Entities;

namespace BAL.Services.Interfaces
{
    public interface IRoleService
    {
        Task<RoleViewModel> GetRoleTask(Role role, string action);
        Task<Role> GetRole(Role role, string action);
        Task<List<RoleViewModel>> GetAllRoleTask();
        void Save();
        void SaveAsync();
    }
}
