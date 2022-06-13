using CocktailBuddy.Models;
using CocktailBuddy.Services;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace CocktailBuddy.ViewModels
{
    public class DetailedViewViewModel: BaseViewModel
    {
        private Cocktail _viewCocktail;
        private FireBase FireBase;
        private string _selectedrating;
        private User user;
        public string SelectedRating
        {
            get { return _selectedrating; }
            set
            {
                _selectedrating = value;
                NotifyPropertyChanged("RatingList");
                if (!_viewCocktail.VoterIDs.Contains(user.ID)) 
                {
                   _viewCocktail.Rating += int.Parse(_selectedrating);
                   _viewCocktail.VoterIDs.Add(user.ID);
                   FireBase.EditCocktail(_viewCocktail);
                }
                else if(_viewCocktail.VoterIDs[0] == user.ID)
                {
                    Application.Current.MainPage.DisplayAlert("Voting Failed", "You Can't Vote On Your Own Rescepie!", "OK");
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Voting Failed","You Already Voted On This Cocktail!", "OK");
                }

            }
        }
        private ObservableCollection<string> _ratingList { get; set; }
        public ObservableCollection<string> RatingList
        {
            get { return _ratingList; }
            set
            {
                _ratingList = value;
                NotifyPropertyChanged("RatingList");
            }
        }
        private ImageSource _rating;
        public ImageSource Rating 
        {
            get { return _rating; }
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                    NotifyPropertyChanged("Rating");
                }
            }
        }
        public Cocktail ViewCocktail
        {
            get { return _viewCocktail; }
            set
            {
                if (_viewCocktail != value)
                {
                    _viewCocktail = value;
                    NotifyPropertyChanged("ViewCocktail");
                }
            }
        }
        private void SetUser() 
        {
            user = new User();
            user = FireBase.GetCurrentUser();
        }
            
        public DetailedViewViewModel(Cocktail cocktail, ImageSource image)
        {
            FireBase = new FireBase();
            SetUser();
            RatingList = new ObservableCollection<string>();
            RatingList.Add("1");
            RatingList.Add("2");
            RatingList.Add("3");
            RatingList.Add("4");
            RatingList.Add("5");
            Rating = image;
            ViewCocktail = cocktail;
        }
    }
}
