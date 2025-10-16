using Ecommerce.Common.CommonDto;
using Ecommerce.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.Repository
{
    public interface ICategoryRepository
    {
        Task<Result<List<Category>>> GetAllCategories();
        Task<Result<Category>> GetCategoryById(int id);
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(int id);
    }
}
