using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVSWithLibary
{
    public class UserManager
    {
        private readonly string _path;
        private List<User> _users;

        public UserManager(string path)
        {
            _path = path;
            _users = LoadUsers();
        }

        private List<User> LoadUsers()
        {
            if (!File.Exists(_path))
                return new List<User>();

            var lines = File.ReadAllLines(_path);
            var users = new List<User>();

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 3)
                {
                    users.Add(new User
                    {
                        Username = parts[0],
                        Password = parts[1],
                        IsActive = bool.Parse(parts[2])
                    });
                }
            }
            return users;
        }

        public bool ValidateUser(string username, string password)
        {
            var user = _users.FirstOrDefault(u => u.Username == username && u.IsActive);
            return user != null && user.Password == password;
        }

        public bool UserExists(string username)
        {
            return _users.Any(u => u.Username == username);
        }

        public bool AddUser(string username, string password)
        {
            if (UserExists(username))
                return false;

            var newUser = new User
            {
                Username = username,
                Password = password,
                IsActive = true
            };

            _users.Add(newUser);
            SaveUsers();

            return true;
        }

        private void SaveUsers()
        {
            var lines = _users.Select(u => $"{u.Username},{u.Password},{u.IsActive}");
            File.WriteAllLines(_path, lines);
        }
    }
}

