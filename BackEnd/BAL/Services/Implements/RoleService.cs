using AutoMapper;
using BAL.Models;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Implements;
using DAL.Repositories.Interfaces;

namespace BAL.Services.Implements
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRepository<Role> _roleRepository;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper, IRepository<Role> roleRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _roleRepository = roleRepository;
        }

        public async Task<List<RoleViewModel>> GetAllRoleTask()
        {
            return _mapper.Map<List<RoleViewModel>>(await _roleRepository.GetAll(null, ""));
        }

        public async Task<Role> GetRole(Role role, string action)
        {
            return await _roleRepository.Get(role, action);
        }

        public async Task<RoleViewModel> GetRoleTask(Role role, string action)
        {
            return _mapper.Map<RoleViewModel>(await _roleRepository.Get(role, action));
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void SaveAsync()
        {
            _unitOfWork.CommitAsync();
        }
    }
}
