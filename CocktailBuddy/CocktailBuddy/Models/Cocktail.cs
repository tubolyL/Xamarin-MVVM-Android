
using System.Collections.Generic;

namespace CocktailBuddy.Models
{
    public class Cocktail
    {
        public string CocktailID { get; set; }
        public string UserId { get; set; }
        public string CocktailName { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string RatingSource { get; set; }
        public byte[] Picture { get; set; }
        public float Rating { get; set; }
        public List<string> VoterIDs { get; set; }
    }
}
