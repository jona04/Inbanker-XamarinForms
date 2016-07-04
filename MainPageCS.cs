using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Inbanker
{
	public class MainPageCS : MasterDetailPage
	{
		MasterPage masterPage;

		public MainPageCS(Usuario eu, List<Amigos> list_amigos)
		{

			//if (Device.OS == TargetPlatform.Windows)
			//{
			//	Master.Icon = "icon.png";
			//}

			//faz desaparece o botao de voltar do toolbal
			//NavigationPage.SetHasNavigationBar(this, false);

			masterPage = new MasterPage(eu);
			Master = masterPage;
			Detail = new NavigationPage(new ListaAmigos(eu,list_amigos));

			masterPage.ListView.ItemSelected += (sender, e) =>
			{

				var item = e.SelectedItem as MasterPageItem;
				if (item != null)
				{

					//MessagingCenter.Subscribe<MasterPageItem>(this, "MenuCellMessage", (sender_arg) => NavigateTo(item.TargetType, eu));
					switch (item.ParamType)
					{
						case (1):
							Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType, eu, list_amigos));
							break;
						case (2):
							Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType, eu));
							break;
						case (3):
							// Kill the access_token so we don't look like we are logged in anymore.
							App.Current.Properties["access_token"] = "";
							// Make the main page the StartPage, which is where auth is launched from.
							App.Current.MainPage = new LoginPage();
							break;
					}

					masterPage.ListView.SelectedItem = null;
					IsPresented = false;
				}

			};

		}

	}
}


