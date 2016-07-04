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
				Navigation.PushModalAsync(new Login());
			};

		}
	}
}

