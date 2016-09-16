using Xamarin.Forms;

namespace Inbanker
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();

			Title = "Inbanker";

			this.btnLogar.Clicked += (sender, e) =>
			{
				//teste de coneccao
				var networkConnection = DependencyService.Get<INetworkConnection>();
				networkConnection.CheckNetworkConnection();
				if (networkConnection.IsConnected)
				{
					Navigation.PushModalAsync(new Login());

				}
				else
				{
					App.Current.MainPage = new NavigationPage(new PageNoConnection());
				}
			};

		}
	}
}

