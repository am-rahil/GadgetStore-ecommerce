using Ecommerce.Common.CommonDto;
using Ecommerce.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.Repository
{
    public interface ISupplierRepository
    {
        Task<Result<List<Supplier>>> GetAllSuppliers();
        Task<Result<Supplier>> GetSupplierById(int id);
        Task AddSupplier(Supplier supplier);
        Task UpdateSupplier(Supplier supplier);
        Task DeleteSupplier(int id);
    }
}
