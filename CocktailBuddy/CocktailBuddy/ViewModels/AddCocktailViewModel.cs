using CocktailBuddy.Models;
using CocktailBuddy.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CocktailBuddy.ViewModels
{
    public class AddCocktailViewModel : BaseViewModel
    {
        FireBase firebase;
        private ObservableCollection<string> _languages { get; set; }
        private Cocktail _newCocktail { get; set; }
        public Command SelectImageCommand { get; set; }
        public Command AddCommand { get; set; }
        private byte[] _Image;
        public Cocktail NewCocktail
        { 
            get { return _newCocktail; }
            set
            {
                if (_newCocktail != value)
                {
                    _newCocktail = value;
                    NotifyPropertyChanged("NewCocktail");
                }
            }
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

        public byte[] Image
        {
            get { return _Image; }
            set
            {
                _Image = value;
                NotifyPropertyChanged("Image");
            }
        }

        public AddCocktailViewModel()
        {
            SelectImageCommand = new Command(SelectImage);
            firebase = new FireBase();
            AddCommand = new Command(AddCocktail);
            NewCocktail = new Cocktail();
            Languages = new ObservableCollection<string>();
            GetLanguages();
        }

        private async void AddCocktail()
        {
            var user = firebase.GetCurrentUser();
            NewCocktail.UserId = user.ID;
            List<string> VoterIDs = new List<string>();
            VoterIDs.Add(user.ID);
            NewCocktail.VoterIDs = VoterIDs;
            await firebase.AddCocktail(NewCocktail);
            await Application.Current.MainPage.Navigation.PopModalAsync();
            Application.Current.MainPage = new Views.Tabs();
        }
        private void GetLanguages()
        {
            Languages.Add("Hungarian");
            Languages.Add("English");
        }
        private async void SelectImage()
        {
            byte[] imageArray = null;
            MemoryStream memory = new MemoryStream();
            try
            {
                var result = await FilePicker.PickAsync();
                if (result != null)
                {
                    if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                        result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                    {
                        var stream = await result.OpenReadAsync();
                        stream.CopyTo(memory);
                        imageArray = memory.ToArray();
                        NewCocktail.Picture = imageArray;
                        Image = imageArray;
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("No Cocktail Selected", ex.Message, "OK");
            }
        }
    }
}
