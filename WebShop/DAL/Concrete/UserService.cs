using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShop.DAL.Abstract;
using WebShop.Models;
using WebShop.Models.Entities;

namespace WebShop.DAL.Concrete
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationRoleManager _roleManager;
        //private readonly ApplicationUserManager _userManager;
        public UserService(ApplicationDbContext context, ApplicationRoleManager roleManager)
        {
            //_userManager = userManager;
            //int count = _userManager.Users.Count();// FindByEmail("novakvova@gmail.com")
            _context = context;
            _roleManager = roleManager;
        }

        public int AddRole(string name)
        {
            CustomRole role = new CustomRole
            {
                Name = name
            };
            var result=_roleManager.CreateAsync(role).Result;
            if (result.Succeeded)
                return role.Id;
            return 0;
        }

        public int AddUserProfiles(UserProfile userProfile)
        {
            _context.UserProfiles.Add(userProfile);
            _context.SaveChanges();
            return userProfile.Id;
        }

        public int GetCountUsers()
        {
            return _context.Users.Count();
        }
    }
}