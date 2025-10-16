using Ecommerce.Common.CommonDto;
using Ecommerce.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.Repository
{
    public class SupplierRepository:ISupplierRepository
    {
        private readonly ApplicationDbContext _context;

        public SupplierRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<Supplier>>> GetAllSuppliers()
        {
            Result<List<Supplier>> result = new();
            var suppliers = await _context.SuppliersSet.ToListAsync();

            if (suppliers.Any())
                result.Response = suppliers;
            else
                result.Errors.Add(new Errors { ErrorCode = "404", ErrorMessage = "No suppliers found" });

            return result;
        }

        public async Task<Result<Supplier>> GetSupplierById(int id)
        {
            Result<Supplier> result = new();
            var supplier = await _context.SuppliersSet.FindAsync(id);

            if (supplier != null)
                result.Response = supplier;
            else
                result.Errors.Add(new Errors { ErrorCode = "404", ErrorMessage = "Supplier not found" });

            return result;


        }

        public async Task AddSupplier(Supplier supplier)
        {
            _context.SuppliersSet.Add(supplier);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateSupplier(Supplier supplier)
        {
            _context.SuppliersSet.Update(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSupplier(int id)
        {
            var supplier = await _context.SuppliersSet.FindAsync(id);
            if (supplier != null)
            {
                _context.SuppliersSet.Remove(supplier); 
                await _context.SaveChangesAsync();
            }
        }
    }
}
