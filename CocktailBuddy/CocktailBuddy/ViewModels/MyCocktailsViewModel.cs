using CocktailBuddy.Interfaces;
using CocktailBuddy.Models;
using CocktailBuddy.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace CocktailBuddy.ViewModels
{
    public class MyCocktailsViewModel : BaseViewModel
    {
        private FireBase _firebase;
        private Cocktail _selectedCocktail;
        private ObservableCollection<Cocktail> _myCocktailsCollection { get; set; }
        IAuthentication authentication;
        public Command AddCommand { get; set; }
        public Command EditCommand { get; set; }
        public Command DeleteCommand {get; set;}
        private bool _visibility { get; set; }
        private bool _loginvisibility { get; set; }
        public bool Visibility 
        { 
            get { return _visibility; }
            set
            {
                _visibility = value;
                NotifyPropertyChanged("Visibility");
            }
        }
        public bool LoginVisibility
        {
            get { return _loginvisibility; }
            set
            {
                _loginvisibility = value;
                NotifyPropertyChanged("Visibility");
            }
        }
        public Cocktail SelectedCocktail
        {
            get { return _selectedCocktail; }
            set
            {
                _selectedCocktail = value;
                NotifyPropertyChanged("SelectedCocktail");
            }
        }

        public ObservableCollection<Cocktail> MyCocktailsCollection
        {
            get { return _myCocktailsCollection; }
            set
            {
                _myCocktailsCollection = value;
                NotifyPropertyChanged("MyCocktailsCollection");
            }
        }

        public MyCocktailsViewModel()
        {
            _firebase = new FireBase();
            AddCommand = new Command(AddClicked);
            EditCommand = new Command(EditClicked);
            DeleteCommand = new Command(DeleteSelectedCocktail);
            MyCocktailsCollection = new ObservableCollection<Cocktail>();
            authentication = DependencyService.Get<IAuthentication>();
            GetMyCocktails();
        }

        public async void AddClicked()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new Views.AddCocktailPage());
        }
        public async void EditClicked()
        {
            if (SelectedCocktail != null)
            {
                EditCocktailViewModel editPlantViewModel = new EditCocktailViewModel(SelectedCocktail);
                await Application.Current.MainPage.Navigation.PushModalAsync(new Views.EditCocktailPage(editPlantViewModel));
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("No Cocktail Selected", "Please select an entry to edit", "OK");
            }
        }
        async void GetMyCocktails()
        {
            if (authentication.IsSignedIn())
            {
                Visibility = true;
                var myCocktails = await _firebase.GetMyCocktails();

                foreach (var item in myCocktails)
                {
                    MyCocktailsCollection.Add(item);
                }
            }
            else
            {
                Visibility = false;
                LoginVisibility = true;
            }
        }

        async void DeleteSelectedCocktail()
        {
            try
            {
                await _firebase.DeleteCocktail(SelectedCocktail);
                MyCocktailsCollection.Remove(SelectedCocktail);
                Application.Current.MainPage = new Views.Tabs();
                await Application.Current.MainPage.DisplayAlert("Cocktail Deleted", "Cocktail Successfully Removed", "OK");
            }
            catch (Exception ex)
            {
                if (SelectedCocktail == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Something went wrong", "No Cocktail Selected", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Something went wrong", ex.Message, "OK");
                }
                
            }   
        }
    }
}
