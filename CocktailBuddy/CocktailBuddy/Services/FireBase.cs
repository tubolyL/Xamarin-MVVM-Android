using Firebase.Database;
using Firebase.Database.Query;
using CocktailBuddy.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailBuddy.Services
{
    public class FireBase
    {
        private static User User;
        FirebaseClient firebaseClient = new FirebaseClient("https://cocktailbuddy-5fc7b-default-rtdb.europe-west1.firebasedatabase.app/");
        public async Task<User> GetCurrentUserFromFirebase(string token)
        {
            User = new User();
            var query = await firebaseClient
                .Child("Users/" + token)
                .OnceSingleAsync<User>();

            User user = query;
            User = user;
            User.ID = token;

            return user;

        }
        public User GetCurrentUser()
        {
            return User;
        }
        public async Task AddUser(string email, string nickname, string token)
        {
            var user = new User() { Email = email, UserName = nickname, ID = token };

            await firebaseClient
                .Child("Users/" + token)
                .PutAsync(user);
        }

        public async Task AddCocktail(Cocktail cocktail)
        {
            await firebaseClient
                .Child("Cocktails")
                .PostAsync(cocktail);
        }
        public async Task EditCocktail(Cocktail cocktail)
        {
            await firebaseClient
                .Child("Cocktails")
                .Child(cocktail.CocktailID)
                .PutAsync(cocktail);
        }
        public async Task DeleteCocktail(Cocktail cocktail)
        {
            await firebaseClient
                .Child("Cocktails")
                .Child(cocktail.CocktailID)
                .DeleteAsync();
        }

        public async Task<List<Cocktail>> GetAllCocktails()
        {
            return( await firebaseClient
                .Child("Cocktails")
                .OnceAsync<Cocktail>()).Select(cocktail => new Cocktail
                {
                    CocktailID = cocktail.Key,
                    CocktailName = cocktail.Object.CocktailName,
                    UserId = cocktail.Object.UserId,
                    Description = cocktail.Object.Description,
                    Language = cocktail.Object.Language,
                    Picture = cocktail.Object.Picture,
                    Rating = cocktail.Object.Rating,
                    VoterIDs = cocktail.Object.VoterIDs,
                    RatingSource = cocktail.Object.RatingSource

                }).ToList();
        }

        public async Task<List<Cocktail>> GetMyCocktails()
        {
            return (await firebaseClient
                .Child("Cocktails")
                .OnceAsync<Cocktail>()).Where(x => x.Object.UserId == User.ID).Select(cocktail => new Cocktail
                {
                    CocktailID = cocktail.Key,
                    CocktailName = cocktail.Object.CocktailName,
                    UserId = cocktail.Object.UserId,
                    Description = cocktail.Object.Description,
                    Language = cocktail.Object.Language,
                    Picture = cocktail.Object.Picture,
                    Rating = cocktail.Object.Rating,
                    VoterIDs = cocktail.Object.VoterIDs,
                    RatingSource = cocktail.Object.RatingSource
                }).ToList();
        }
        public async Task<List<Cocktail>> GetCocktailWithLanguage(string language)
        {
            return (await firebaseClient
                .Child("Cocktails")
                .OnceAsync<Cocktail>()).Where(x => x.Object.Language == language).Select(cocktail => new Cocktail
                {
                    CocktailID = cocktail.Key,
                    CocktailName = cocktail.Object.CocktailName,
                    UserId = cocktail.Object.UserId,
                    Description = cocktail.Object.Description,
                    Language = cocktail.Object.Language,
                    Picture = cocktail.Object.Picture,
                    Rating = cocktail.Object.Rating,
                    VoterIDs = cocktail.Object.VoterIDs,
                    RatingSource = cocktail.Object.RatingSource
                }).ToList();
        }

        public void ClearUser()
        {
            User = null;
        }
    }
}
