using eshop_orderapi.Business.ViewModels.Organization;
using eshop_orderapi.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eshop_orderapi.Interfaces.Services
{
    public interface IRoleModuleService : IBaseService<RoleModule>
    {
        Task<List<RoleModuleModel>> GetAllAsync();

        Task<bool> AddAsync(RoleModuleDetailsModel roleModule);

        Task<bool> UpdateAsync(RoleModuleDetailsModel roleModule);
    }
}