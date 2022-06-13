using CocktailBuddy.Models;
using CocktailBuddy.Services;
using CocktailBuddy.Views;
using System;
using System.Collections.ObjectModel;
using System.Dynamic;
using Xamarin.Forms;

namespace CocktailBuddy.ViewModels
{
    public class CocktailListViewModel : BaseViewModel
    {
        private ObservableCollection<Cocktail> _CocktailsCollection { get; set; }
        private Cocktail _selectedCocktail;
        private FireBase _firebase;
        public Command OpenDetailedViewCommand;
        private string _selectedLanguage { get; set; }
        private ImageSource _rating { get; set; }
        private ObservableCollection<string> _languages { get; set; }
        public ObservableCollection<string> Languages
        {
            get { return _languages; }
            set
            {
                _languages = value;
                NotifyPropertyChanged("Languages");
            }
        }
        public Cocktail SelectedCocktail
        {
            get { return _selectedCocktail; }
            set
            {
                _selectedCocktail = value;
                OpenDetailedView();
                NotifyPropertyChanged("SelectedCocktail");
            }
        }
        public string SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                FilterCocktails();
                NotifyPropertyChanged("SelectedLanguage");
            }
        }
        public ImageSource Rating
        {
            get { return _rating; }
            set
            {
                _rating = value;
                NotifyPropertyChanged("Rating");
            }
        }

        public ObservableCollection<Cocktail> CocktailsCollection
        {
            get { return _CocktailsCollection; }
            set
            {
                _CocktailsCollection = value;
                NotifyPropertyChanged("CocktailsCollection");
            }
        }
        public CocktailListViewModel()
        {
            _firebase = new FireBase();
            CocktailsCollection = new ObservableCollection<Cocktail>();
            Languages = new ObservableCollection<string>();
            OpenDetailedViewCommand = new Command(OpenDetailedView);
            GetCocktails();
            GetLanguages();
        }
        private void GetLanguages()
        {
            Languages.Add("Hungarian");
            Languages.Add("English");
        }

        private async void GetCocktails()
        {
            var allCocktails = await _firebase.GetAllCocktails();
            foreach (var item in allCocktails)
            {  
                if (item.VoterIDs != null)
                {

                    float rating = item.Rating / (item.VoterIDs.Count-1);
                    string ratingPath = "star" + Math.Round(rating, MidpointRounding.AwayFromZero) + ".png";
                    Rating = ImageSource.FromFile(ratingPath);
                    item.RatingSource = ratingPath;
                }
                else
                {
                    Rating = ImageSource.FromFile("star0.png");
                    item.RatingSource = "star0.png";
                }
                
                CocktailsCollection.Add(item);

         
            }
        }
        private async void FilterCocktails()
        {
            CocktailsCollection.Clear();
            var myCocktails = await _firebase.GetCocktailWithLanguage(_selectedLanguage);
            foreach (var item in myCocktails)
            {
                CocktailsCollection.Add(item);

            }
        }
        private async void OpenDetailedView()
        {
            if (SelectedCocktail != null)
            {
                ImageSource RatingImage = null;
                if (SelectedCocktail.VoterIDs != null)
                {

                    float rating = SelectedCocktail.Rating / (SelectedCocktail.VoterIDs.Count-1);
                    string path = "star" + Math.Round(rating, MidpointRounding.AwayFromZero) + ".png";
                    RatingImage = ImageSource.FromFile(path);
                }
                else
                {
                    RatingImage = ImageSource.FromFile("star0.png");
                }
                DetailedViewViewModel detailedView = new DetailedViewViewModel(SelectedCocktail, RatingImage);
                await Application.Current.MainPage.Navigation.PushModalAsync(new DetailedView(detailedView));
            }
        }
    }
}
