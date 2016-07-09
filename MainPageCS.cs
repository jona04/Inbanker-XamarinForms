using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Inbanker
{
	public class MainPageCS : MasterDetailPage
	{
		MasterPage masterPage;

		public MainPageCS(Page page)
		{

			//if (Device.OS == TargetPlatform.Windows)
			//{
			//	Master.Icon = "icon.png";
			//}

			//faz desaparece o botao de voltar do toolbal
			//NavigationPage.SetHasNavigationBar(this, false);

			//pegamos os dados do usuario logado que esta no msqlite
			AcessoDadosUsuario dados = new AcessoDadosUsuario();
			var eu = dados.ObterUsuario();

			masterPage = new MasterPage();
			Master = masterPage;
			Detail = new NavigationPage(page);

			masterPage.ListView.ItemSelected += (sender, e) =>
			{

				var item = e.SelectedItem as MasterPageItem;
				if (item != null)
				{

					//MessagingCenter.Subscribe<MasterPageItem>(this, "MenuCellMessage", (sender_arg) => NavigateTo(item.TargetType, eu));
					switch (item.ParamType)
					{
						case (1):
							Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
							break;
						case (2):
							// Kill the access_token so we don't look like we are logged in anymore.
							//App.Current.Properties["access_token"] = "";
							dados.DeleteUsuario(eu.id_usuario);
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


