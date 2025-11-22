using Ecommerce.Entity.DTO;
using Ecommerce.Entity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Service.Repository
{
    public class SecurityRepository : ISecurityRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public SecurityRepository(
            UserManager<ApplicationUser> userManager,
            RoleManager<Role> roleManager,
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        // ---------------------- Generate Token ----------------------
        public async Task<string> GenerateToken(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                expires: DateTime.Now.AddHours(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ---------------------- Login ----------------------
        public async Task<UserResponse> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                throw new Exception("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);
            var token = await GenerateToken(user);

            return new UserResponse
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = roles.FirstOrDefault(),
                Token = token
            };
        }

        // ---------------------- Register ----------------------
        public async Task<UserResponse> Register(RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                throw new Exception("User already exists");

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            string defaultRole = "Customer";

            if (!await _roleManager.RoleExistsAsync(defaultRole))
            {
                var role = new Role { Name = defaultRole, DisplayName = defaultRole };
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(user, defaultRole);

            var token = await GenerateToken(user);

            return new UserResponse
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = defaultRole,
                Token = token
            };
        }

        // ---------------------- Get All Users ----------------------
        public async Task<IEnumerable<UserResponse>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync(); // ✅ Async-safe

            var list = new List<UserResponse>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                list.Add(new UserResponse
                {
                    UserId = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = roles.FirstOrDefault()
                });
            }

            return list;
        }

        // ---------------------- Delete User ----------------------
        public async Task DeleteUser(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new Exception("User not found");

            bool hasOrders = await _context.OrdersSet.AnyAsync(o => o.UserId == userId);

            if (hasOrders)
                throw new Exception("Cannot delete user because they have existing orders.");

            // Safe delete because no orders found
            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                throw new Exception("Failed to delete user");
        }
    }
}
