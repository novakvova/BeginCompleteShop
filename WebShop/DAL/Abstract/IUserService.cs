using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models.Entities;

namespace WebShop.DAL.Abstract
{
    public interface IUserService
    {
        int AddUserProfiles(UserProfile userProfile);
        int GetCountUsers();
        int AddRole(string name);
    }
}
