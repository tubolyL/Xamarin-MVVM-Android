using CocktailBuddy.Models;
using CocktailBuddy.Services;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace CocktailBuddy.ViewModels
{
    public class EditCocktailViewModel:BaseViewModel
    {
        private Cocktail _editCocktail;
        private FireBase _firebase;
        private ObservableCollection<string> _languages;
        public Command EditCommand { get; set; }
        
        public EditCocktailViewModel(Cocktail cocktail)
        {
            EditCocktail = cocktail;
            _languages = new ObservableCollection<string>();
            GetLanguages();
            _firebase = new FireBase();
            EditCommand = new Command(SaveEditedCocktail);
        }
        public ObservableCollection<string> Languages
        {
            get { return _languages; }
            set
            {
                _languages = value;
                NotifyPropertyChanged("Languages");
            }
        }

        public Cocktail EditCocktail
        {
            get { return _editCocktail; }
            set
            {
                if (_editCocktail != value)
                {
                    _editCocktail = value;
                    NotifyPropertyChanged("NewCocktail");
                }
            }
        }
        private async void SaveEditedCocktail()
        {
            var user = _firebase.GetCurrentUser();
            EditCocktail.UserId = user.ID;
            await _firebase.EditCocktail(EditCocktail);
            await Application.Current.MainPage.Navigation.PopModalAsync();
            Application.Current.MainPage = new Views.Tabs();
        }
        private void GetLanguages()
        {
            Languages.Add("Hungarian");
            Languages.Add("English");
        }
    }
}
