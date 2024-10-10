using AutoMapper;
using eshop_orderapi.Business.ViewModels.Organization;
using eshop_orderapi.Domain.Models;
using eshop_orderapi.Interfaces.Services;
using eshop_orderapi.UoW;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eshop_orderapi.Services
{
    public class RoleModuleService : ServiceBase, IRoleModuleService
    {
        public RoleModuleService(IUnitOfWork unitOfWork, IMapper _mapper) : base(unitOfWork, _mapper)
        {
        }

        public async Task<List<RoleModuleModel>> GetAllAsync()
        {
            var getRoleModule = await unitOfWork.RoleModuleRepository.GetAllAsync();
            List<RoleModuleModel> model = new List<RoleModuleModel>();
            model = getRoleModule.GroupBy(x => x.RoleId, (key, g) => new { RoleId = key, ModuleDetails = g.ToList() }).Select(x => new RoleModuleModel
            {
                RoleId = x.RoleId,
                RoleModuleDetails = mapper.Map<List<RoleModuleDetailsModel>>(x.ModuleDetails.ToList())
            }).ToList();
            return model;
        }

        public async Task<bool> AddAsync(RoleModuleDetailsModel roleModuleModel)
        {
            var roleModule = mapper.Map<RoleModule>(roleModuleModel);
            await unitOfWork.RoleModuleRepository.AddAsync(roleModule);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync(RoleModuleDetailsModel roleModuleModel)
        {
            var roleModule = await unitOfWork.RoleModuleRepository.GetAsync(roleModuleModel.Id);
            if (roleModule != null)
            {
                roleModule = mapper.Map(roleModuleModel, roleModule);
                roleModule.Id = roleModuleModel.Id;
                await unitOfWork.RoleModuleRepository.UpdateAsync(roleModule);
            }
            return await Task.FromResult(true);
        }
    }
}