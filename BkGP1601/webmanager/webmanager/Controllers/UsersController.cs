using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using webmanager.Models;
using webmanager.DAL;

namespace webmanager.Controllers
{
    public class UsersController : ApiController
    {
        public IEnumerable<UserModel> GetAllUsers()
        {
            UserDAL userDAL = new UserDAL();
            var userList = userDAL.getAllUsers();
            return userList;
        }

        public int register(string name, string password)
        {
            UserDAL userDal = new UserDAL();
            var result=userDal.insertUser(name, password);
            return result;
        }
        public int login(string name, string password)
        {
            var userDal = new UserDAL();
            var userList = userDal.getAllUsers();
            var isExist= userList.Exists(s => s.Name == name && s.password == password);
            if (isExist)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}
