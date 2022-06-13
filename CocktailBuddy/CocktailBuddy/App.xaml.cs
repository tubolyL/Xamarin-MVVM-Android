using CocktailBuddy.Interfaces;
using CocktailBuddy.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CocktailBuddy
{
    public partial class App : Application
    {
        IAuthentication authentication;
        FireBase firebaseHelper;
        public App()
        {
            InitializeComponent();

            MainPage = new Views.Tabs();
        }

        protected override async void OnStart()
        {
            firebaseHelper = new FireBase();
            authentication = DependencyService.Get<IAuthentication>();
            if (authentication.IsSignedIn())
            {
                await UserInit();

            }


        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private async Task UserInit()
        {
            var user = authentication.GetCurrentUserID();
            Task t = Task.Run(async () => { await FireBaseInit.init(user); });
            t.Wait(100);
            //await FireBaseInit.init(user);
        }
    }
}
