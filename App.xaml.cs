using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Inbanker
{
	public partial class App : Application
	{

		//public static Action HideLoginView
		//{
		//	get
		//	{
		//		return new Action(() => Current.MainPage.Navigation.PopModalAsync());
		//	}
		//}

		//public async static Task NavigateToLista(Usuario eu,List<Amigos> list_amigos)
		//{
		//	await Current.MainPage.Navigation.PushAsync(new MainPageCS(eu,list_amigos));
		//}

		private object accessToken;
		private bool loggedIn;

		public App()
		{
			InitializeComponent();

			// This code is re-launched when an Android app is restarted from a sleep.  So we need to make sure that any calls in this area 
			// are idempotent, which is a word really only programmers and math geeks know.  This shit will run again - Make sure it doesn't trip over itself.

			loggedIn = false;

			if (App.Current.Properties.TryGetValue("access_token", out accessToken))
			{
				if (accessToken.ToString().Length > 0)
				{
					loggedIn = true;
				}
			}

			if (!loggedIn)
			{
				// If we aren't logged in, then this may be the first time we're starting the app, in which case we want to
				// jam some settings in for our auth that we can retrieve later.  
				// But MAYBE, we are re-launching an app that was not logged in, in which case jamming these values in would 
				// cause a crash.  So we wrap them up in an empty try-catch, which is not elegant but is effective.
				try
				{
					App.Current.Properties.Add("clientId", "1028244680574076");
					App.Current.Properties.Add("scope", "user_friends");
					App.Current.Properties.Add("authorizeUrl", "https://m.facebook.com/dialog/oauth/");
					App.Current.Properties.Add("redirectUrl", "https://www.facebook.com/connect/login_success.html");

					// These are not applicable for facebook login
					//App.Current.Properties.Add("clientSecret", "na");
					//App.Current.Properties.Add("accessTokenUrl", "na");

				}
				catch
				{
				}

				// The root page of your application before login.
				MainPage = new LoginPage();

			}
			else {
				// se estiver logado, ele ira capturar os dados do usuario atraves de uma redenrer page, e depois sera redirecionado para lista de amigos
				MainPage = new RedirectLogin();
			}

			//MainPage = new NavigationPage(new LoginPage());
		}

		//public App(string notification,Transacao transacao)
		//{
		//	InitializeComponent();

		//	if (notification.Equals("false"))
		//	{
		//		MainPage = new NavigationPage(new LoginPage());
		//	}
		//	else {
		//		if (notification.Equals("receber_pedido"))
		//		{
		//			// The root page of your application
		//			MainPage = new NavigationPage(new VerPedidoRecebido(transacao));
		//		}
		//		else 
		//		{
		//			// The root page of your application
		//			MainPage = new NavigationPage(new VerPedidosEnviados(transacao));
		//		}

		//	}
		//}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

