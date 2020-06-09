using EventosTec.Library.model;
using EventosTec.Library.Service;
using ImTools;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventosTec.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private string password;
        private bool isrunning;
        private bool isenabled;
        private DelegateCommand logincommand;
        private readonly IApiServices apiservice;

        public LoginPageViewModel(INavigationService navigationService, IApiServices apiServices)
            : base(navigationService)
        {
            Title = "Login";
            IsEnabled = true;
            apiservice = apiServices;


        }

        public DelegateCommand LoginCommand => logincommand ?? (logincommand = new DelegateCommand(Login));

        private async void Login()
        {
            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Ingrese un Correo", "Accept");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Ingrese un Password", "Accept");
                return;
            }

            IsRunning = true;
            IsEnabled = false;
            var request = new TokenRequest()
            {
                Password = password,
                Username = Email,

            };

            var url = App.Current.Resources["UrlAPI"].ToString();

            var response = await apiservice.GetTokenAsync(url, "/Account", "/CreateToken", request);



            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Contraseña o Usuario Incorrectos", "Accept");
                Password = string.Empty;
                return;
            }
            var token = (TokenResponse)response.Result;

            await App.Current.MainPage.DisplayAlert("Ok", "Ya entre", "Accept");
        }

        public string Email { get; set; }
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        public bool IsRunning
        {
            get => isrunning;
            set => SetProperty(ref isrunning, value);
        }

        public bool IsEnabled
        {
            get => isenabled;
            set => SetProperty(ref isenabled, value);
        }

    }
}