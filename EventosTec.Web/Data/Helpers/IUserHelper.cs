using EventosTec.Web.Models;
using EventosTec.Web.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventosTec.Web.Data.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEMailAsync(string email);
        Task<IdentityResult> AddUserAssync(User user, string Password);
        Task CheckRoleAsync(string roleName);
        Task AddUserToRoleAsync(User user, string roleName);

        Task<bool> IsUserInRoleAsync(User user,string roleName);

        Task<SignInResult> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
    }
}
