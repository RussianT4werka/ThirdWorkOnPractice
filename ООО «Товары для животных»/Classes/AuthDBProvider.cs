using Microsoft.EntityFrameworkCore;
using System.Linq;
using ООО__Товары_для_животных_.Database;
using ООО__Товары_для_животных_.Interfaces;
using ООО__Товары_для_животных_.Models;

namespace ООО__Товары_для_животных_.Classes
{
    public class AuthDBProvider : IDBAuth
    {
        public User FindUserByLoginPassword(string login, string pass)
        {
            return DB.Instance.Users.Include("UserRoleNavigation").FirstOrDefault(s => s.UserPassword == pass & s.UserLogin == login)!;
        }
    }
}