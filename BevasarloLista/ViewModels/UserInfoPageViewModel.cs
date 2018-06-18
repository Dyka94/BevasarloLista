using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace BevasarloLista.ViewModels
{
	public class UserInfoPageViewModel : ViewModelBase
	{
        public Command OnPressCommand { get; }

        private string nickName;
        public string NinckName
        {
            get { return nickName; }
            set { SetProperty(ref nickName, value); }
        }
        public UserInfoPageViewModel(INavigationService navigationService): base(navigationService)
        {
            OnPressCommand = new Command(onPress);
            Title = Resources.AppResource.userinfoTitle;
        }

        private async void onPress(object obj)
        {
            var np = new NavigationParameters { { NAVPARAM_NICKNAME, nickName } };
            await NavigationService.GoBackAsync(np);
        }
    }
}
