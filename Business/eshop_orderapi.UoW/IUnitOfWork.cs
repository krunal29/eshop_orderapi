using eshop_orderapi.Interfaces.Repositories;
using System;

namespace eshop_orderapi.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        //IPersonRepository PersonRepository { get; }
        //IRoleRepository RoleRepository { get; }
        //IModuleRepository ModuleRepository { get; }
        //IAccessModuleRepository AccessModuleRepository { get; }
        //IRoleModuleRepository RoleModuleRepository { get; }
        IOrderRepository OrderRepository { get; }

IOrderItemRepository OrderItemRepository { get; }


    }
}