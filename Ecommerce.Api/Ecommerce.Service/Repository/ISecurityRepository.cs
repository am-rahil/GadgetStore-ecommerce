using Ecommerce.Entity.DTO;
using Ecommerce.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.Repository
{
    public interface ISecurityRepository
    {
        Task<UserResponse> Register(RegisterRequest request);
        Task<UserResponse> Login(LoginRequest request);
        Task<string> GenerateToken(ApplicationUser user);
    }
}
