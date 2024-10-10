using eshop_orderapi.Domain;
using eshop_orderapi.Interfaces.Repositories;
using eshop_orderapi.Repositories;
using System;

namespace eshop_orderapi.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly eshop_orderapiContext Context;

        public UnitOfWork(eshop_orderapiContext context)
        {
            this.Context = context;
            //PersonRepository = new PersonRepository(Context);
            //RoleRepository = new RoleRepository(Context);
            //ModuleRepository = new ModuleRepository(Context);
            //RoleModuleRepository = new RoleModuleRepository(Context);
            OrderRepository = new OrderRepository(Context); 

OrderItemRepository = new OrderItemRepository(Context); 


        }

        //public IPersonRepository PersonRepository { get; }
        //public IRoleRepository RoleRepository { get; }
        //public IModuleRepository ModuleRepository { get; }
        //public IAccessModuleRepository AccessModuleRepository { get; }
        //public IRoleModuleRepository RoleModuleRepository { get; }

        public IOrderRepository OrderRepository { get; }

public IOrderItemRepository OrderItemRepository { get; }



        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                Context.Dispose();
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}