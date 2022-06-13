using System.Threading.Tasks;

namespace CocktailBuddy.Services
{
    public class FireBaseInit
    {
        public static async Task init(string token)
        {
            FireBase firebaseHelper = new FireBase();
            var user = await firebaseHelper.GetCurrentUserFromFirebase(token);
        }
    }
}
