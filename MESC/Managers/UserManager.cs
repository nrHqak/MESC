using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using MESC.Models;

namespace MESC.Managers
{
    public class UserManager
    {
        private readonly string _usersPath;
        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();
        public UserManager(string usersPath) { _usersPath = usersPath; }
        public List<User> GetAllUsers() => File.Exists(_usersPath)
            ? _serializer.Deserialize<List<User>>(File.ReadAllText(_usersPath)) ?? new List<User>()
            : new List<User>();
        public bool Register(User user)
        {
            var users = GetAllUsers();
            if (users.Any(u => u.Username == user.Username || u.Email == user.Email)) return false;
            users.Add(user);
            File.WriteAllText(_usersPath, _serializer.Serialize(users));
            return true;
        }
        public User Login(string username, string password) => GetAllUsers().FirstOrDefault(u => u.Username == username && u.Password == password);
    }
}
