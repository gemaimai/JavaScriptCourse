using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
using webmanager.Models;
using System.IO;
using Newtonsoft.Json;

namespace webmanager.DAL
{
    public class UserDAL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MySQLconnStr"].ConnectionString;
        public List<UserModel> getAllUsers()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            List<UserModel> userList = new List<UserModel>();
            try
            {
                connection.Open();
                //string query = "SELECT * FROM user where name=" + name;
                string query = "SELECT * FROM user";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    UserModel uModel = new UserModel();
                    uModel.Id = int.Parse(dataReader["id"].ToString());
                    uModel.Name = dataReader["name"].ToString();
                    uModel.password = dataReader["password"].ToString();
                    userList.Add(uModel);
                }
                //close Data Reader
                dataReader.Close();

            }
            catch (MySqlException ex)
            {

            }
            return userList;
        }

        public List<UserModel> getUserByJson()
        {
            var userdata = new List<UserModel>();

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"datajson\user.json");
            if (File.Exists(path))
            {
                string str = File.ReadAllText(path);
                userdata = JsonConvert.DeserializeObject<List<UserModel>>(str);              

            }
            return userdata;
           
        }
        public int insertUser(string name,string password)
        {
            var userList = getUserByJson();
            var isExist = userList.Exists(s => s.Name == name);
            if (isExist)
            {
                return -1;
            }
            else
            {
                try
                {

                    UserModel userInfo = new UserModel();
                    userInfo.Id = 1;
                    userInfo.Name = name;
                    userInfo.password = password;
                    userList.Add(userInfo);
                    var userdata = JsonConvert.SerializeObject(userList);
                    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"datajson\user.json");
                    File.WriteAllText(path, userdata);
                    return 1;
                }
                catch (Exception ex)
                {
                    return  -1;
                }
            }            
        }

        public UserModel getUser(string name)
        {
            var userList = getUserByJson();
            var user = userList.FirstOrDefault(s => s.Name == name);
            return user;
        }

    }
   
}