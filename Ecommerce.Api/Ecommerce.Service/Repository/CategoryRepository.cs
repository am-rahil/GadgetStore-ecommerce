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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddCategory(Category category)
        {
            _context.CategoriesSet.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(int id)
        {
            var category = await _context.CategoriesSet.FindAsync(id);
            if (category != null)
            {
                _context.CategoriesSet.Remove(category);
                await _context.SaveChangesAsync();

            }
        }

        public async Task<Result<List<Category>>> GetAllCategories()
        {
            Result<List<Category>> result = new();
            var categories = await _context.CategoriesSet.ToListAsync();
            if (categories.Any())
                result.Response = categories;
            else
                result.Errors.Add(new Errors { ErrorCode = "404", ErrorMessage = "No categories found" });

            return result;
        }

        public async Task<Result<Category>> GetCategoryById(int id)
        {
            Result<Category> result = new();
            var category = await _context.CategoriesSet.FindAsync(id);

            if (category != null)
                result.Response = category;
            else
                result.Errors.Add(new Errors { ErrorCode = "404", ErrorMessage = "Category not found" });

            return result;
        }

        public async Task UpdateCategory(Category category)
        {
            _context.CategoriesSet.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
