using CocktailBuddy.Interfaces;
using CocktailBuddy.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CocktailBuddy.ViewModels
{
    class ProfileViewModel:BaseViewModel
    {
        private string _password;
        IAuthentication authentication;
        private string _email;
        private string _confirmPassword;
        private string _name;
        private bool _loginVisibility;
        private bool _signUpVisibility;
        private bool _profileVisibility;

        public string Email
        {
            get { return _email; }
            set
            {
                if (_email == value)
                {
                    return;
                }

                _email = value;
                NotifyPropertyChanged("Email");
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value)
                {
                    return;
                }

                _password = value;
                NotifyPropertyChanged("Password");
            }
        }
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                if (_confirmPassword == value)
                {
                    return;
                }

                _confirmPassword = value;
                NotifyPropertyChanged("ConfirmPassword");
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                {
                    return;
                }

                _name = value;
                NotifyPropertyChanged("Name");
            }
        }
        public bool LoginVisibility 
        {
            get { return _loginVisibility; }
            set
            {
                if (_loginVisibility == value)
                {
                    return;
                }
                _loginVisibility = value;
                NotifyPropertyChanged("LoginVisibility");
            }
        }
        public bool SignUpVisibility 
        {
            get { return _signUpVisibility; }
            set
            {
                if (_signUpVisibility == value)
                {
                    return;
                }
                _signUpVisibility = value;
                NotifyPropertyChanged("SignUpVisibility");
            }
        }
        public bool ProfileVisibility
        {
            get { return _profileVisibility; }
            set
            {
                if (_profileVisibility == value)
                {
                    return;
                }
                _profileVisibility = value;
                NotifyPropertyChanged("ProfileVisibility");
            }
        }

        public Command LoginCommand { get; set; }
        public Command SignUpCommand { get; set; }
        public Command SignOutCommand { get; set; }
        public Command SignUpClickedCommand { get; set; }
        public Command ForgottenPasswordCommand { get; set; }
        FireBase fireBase;

        public ProfileViewModel()
        {
            fireBase = new FireBase();
            authentication = DependencyService.Get<IAuthentication>();
            LoginCommand = new Command(LoginClicked);
            SignUpCommand = new Command(SignUp);
            SignUpClickedCommand = new Command(SignUpClicked);
            SignOutCommand = new Command(SignOutClicked);
            VisibilityCheck();

        }

        private async void LoginClicked(object obj)
        {
            string token = await authentication.Login(Email, Password);
            if (token != String.Empty && Password != string.Empty && Email != string.Empty)
            {
                Task t = Task.Run(async () => { await FireBaseInit.init(token); });
                t.Wait(100);
                Application.Current.MainPage = new Views.Tabs();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Login failed", token, "OK");
            }
            VisibilityCheck();
        }

        private async void SignUp(object obj)
        {
            if (Name == string.Empty || Email == string.Empty || Password == string.Empty || ConfirmPassword == string.Empty)
            {
                await Application.Current.MainPage.DisplayAlert("Failed", "Signup unsuccessful", "OK");
            }
            else if (Password != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Signup Failed", "Password fields must be the same", "OK");
            }
            else
            {
                string token = await authentication.SignUp(Email, Password, Name);
                await Application.Current.MainPage.DisplayAlert("Success", "Signup successful", "OK");
                VisibilityCheck();
                Application.Current.MainPage = new Views.Tabs();
            }
        }

        private void VisibilityCheck()
        {
            if (authentication.IsSignedIn())
            {
                LoginVisibility = false;
                SignUpVisibility = false;
                ProfileVisibility = true;
            }
            else
            {
                LoginVisibility = true;
                SignUpVisibility = false;
                ProfileVisibility = false;
            }
        }

        private void SignUpClicked()
        {
            ProfileVisibility = false;
            LoginVisibility = false;
            SignUpVisibility = true;
        }
        private void SignOutClicked()
        {
            authentication.SignOut();
            LoginVisibility = true;
            SignUpVisibility = false;
            ProfileVisibility = false;
            fireBase.ClearUser();
            Application.Current.MainPage = new Views.Tabs();
        }

    }
}
