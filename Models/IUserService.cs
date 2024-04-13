namespace democode.Models
{
    public interface IUserService
    {
        User Register(string id, string name, string email, string password, UserRole role);
        void SaveUser(User user);
        User GetUser(string email, string password);
        bool IsEmailTaken(string email);
    }
}
