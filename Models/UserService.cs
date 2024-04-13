using System.Collections.Generic;
using System.Linq;
using democode.Models;
using Newtonsoft.Json;

namespace democode.Models
{
    public class UserService : IUserService
    {
        public User GetUser(string email, string password)
        {
            var users = LoadUsersFromJson();
            return users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public bool IsEmailTaken(string email)
        {
            var users = LoadUsersFromJson();
            return users.Any(u => u.Email == email);
        }

        public void SaveUser(User user)
        {
            var users = LoadUsersFromJson();
            users.Add(user);
            SaveUsersToJson(users);
        }

        public User Register(string id, string name, string email, string password, UserRole role)
        {
            var users = LoadUsersFromJson();

            var user = new User
            {
                Id = id,
                Name = name,
                Email = email,
                Password = password,
                Role = role
            };

            users.Add(user);

            SaveUsersToJson(users);

            return user;
        }

        private List<User> LoadUsersFromJson()
        {
            var json = System.IO.File.ReadAllText("json.json");
            return JsonConvert.DeserializeObject<List<User>>(json);
        }

        private void SaveUsersToJson(List<User> users)
        {
            var json = JsonConvert.SerializeObject(users);
            System.IO.File.WriteAllText("json.json", json);
        }
    }
}
