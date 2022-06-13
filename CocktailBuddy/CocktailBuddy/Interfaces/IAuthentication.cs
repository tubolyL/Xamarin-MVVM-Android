using System.Threading.Tasks;

namespace CocktailBuddy.Interfaces
{
    public interface IAuthentication
    {
        Task<string> Login(string email, string password);
        Task<string> SignUp(string email, string password, string nickname);
        bool SignOut();
        bool IsSignedIn();
        string GetCurrentUserID();
        bool ResetPassword(string email);
    }
}
