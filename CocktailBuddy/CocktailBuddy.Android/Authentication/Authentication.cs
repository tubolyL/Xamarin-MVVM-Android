using CocktailBuddy.Interfaces;
using CocktailBuddy.Services;
using System.Threading.Tasks;
using Firebase.Auth;
using CocktailBuddy.Droid.Authentication;
using Xamarin.Forms;
using Java.Lang;

[assembly: Dependency(typeof(Authentication))]
namespace CocktailBuddy.Droid.Authentication
{
    class Authentication : IAuthentication
    {
        FireBase fireBase;
        public Authentication()
        {
            fireBase = new FireBase();
        }
        public async Task<string> Login(string email, string password)
        {
            try
            {
                var newUser = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                var token = newUser.User.Uid;
                return token;
            }
            catch (FirebaseAuthInvalidUserException ex)
            {
                ex.PrintStackTrace();
                return System.String.Empty;
            }
            catch (FirebaseAuthInvalidCredentialsException ex)
            {
                ex.PrintStackTrace();
                return System.String.Empty;
            }
            catch (IllegalArgumentException ex)
            {
                ex.PrintStackTrace();
                return System.String.Empty;
            }
        }

        public async Task<string> SignUp(string email, string password, string nickname)
        {
            try
            {
                var newUser = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
                var token = newUser.User.Uid;

                await fireBase.AddUser(email, nickname, token);

                return token;

            }
            catch (FirebaseAuthInvalidUserException ex)
            {
                ex.PrintStackTrace();
                return "Oops";
            }
            catch (FirebaseAuthInvalidCredentialsException ex)
            {
                ex.PrintStackTrace();
                return "Wrong email";
            }
            catch (FirebaseAuthUserCollisionException ex)
            {
                ex.PrintStackTrace();
                return "Email already registered";
            }
        }

        public bool SignOut()
        {
            try
            {
                FirebaseAuth.Instance.SignOut();

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public bool IsSignedIn()
        {
            var user = FirebaseAuth.Instance.CurrentUser;
            return user != null;
        }

        public string GetCurrentUserID()
        {
            return FirebaseAuth.Instance.CurrentUser.Uid;
        }

        public bool ResetPassword(string email)
        {
            try
            {
                FirebaseAuth.Instance.SendPasswordResetEmail(email);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}