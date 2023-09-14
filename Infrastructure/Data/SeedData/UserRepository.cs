using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Models.Identities;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Acacia_Back_End.Infrastructure.Data.SeedData
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;

        public UserRepository(UserManager<AppUser> usermanager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, IConfiguration config)
        {
            _userManager = usermanager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _config = config;
        }

        public async Task<IReadOnlyList<UserVM>> GetUsersAsync(UserSpecParams userParams)
        {
            List<AppUser> users;

            if(userParams.UserRole == null)
            {
                users = await _userManager.Users.Where(x => string.IsNullOrEmpty(userParams.Search) || x.DisplayName.ToLower().Contains(userParams.Search.ToLower()) || x.Email.ToLower().Contains(userParams.Search.ToLower())).ToListAsync();
            }
            else
            {
                users = _userManager.GetUsersInRoleAsync(userParams.UserRole).Result.Where(x => string.IsNullOrEmpty(userParams.Search) || x.DisplayName.ToLower().Contains(userParams.Search.ToLower()) || x.Email.ToLower().Contains(userParams.Search.ToLower())).ToList();
            }
            

            switch (userParams.sort)
            {
                case "nameAsc":
                    users = users.OrderBy(p => p.DisplayName).ToList();
                    break;
                case "nameDesc":
                    users = users.OrderByDescending(p => p.DisplayName).ToList();
                    break;
                default:
                    users = users.OrderBy(n => n.DisplayName).ToList();
                    break;
            }

            return  users.Select(async user => new UserVM
            {
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user),
                DisplayName = user.DisplayName,
                ProfilePicture = _config["ApiUrl"] + "/" + user.ProfilePicture
            }).Select(t => t.Result).ToList();
        }

        // Implement your functions defiened in the IUserRepository interface here
    }
}
