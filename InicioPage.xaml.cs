﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Inbanker
{
	public partial class InicioPage : ContentPage
	{
		public InicioPage(Usuario eu, List<Amigos> list_amigos)
		{
			InitializeComponent();

			img_inicio.Source = "icon.png";
			lbl_inicio.Text = "InBanker";

			Title = "Inicio";

			this.btn_list_amigos.Clicked += (sender, e) =>
			{
				//Navigation.PushModalAsync(new MainPageCS(eu, list_amigos,new ListaAmigos(eu, list_amigos)));
				App.Current.MainPage = new MainPageCS(eu, list_amigos, new ListaAmigos(eu, list_amigos)); //utilizamos esse formato para nao repetirmo a mesma page quando o usuario apertar no botaao voltar
			};
			this.btn_pedidos_enviados.Clicked += (sender, e) =>
			{
				//Navigation.PushModalAsync(new PedidosFeitos(eu));
				App.Current.MainPage = new MainPageCS(eu, list_amigos, new PedidosFeitos(eu));
			};
			this.btn_pedidos_recebidos.Clicked += (sender, e) =>
			{
				//Navigation.PushModalAsync(new PedidosRecebidos(eu));
				App.Current.MainPage = new MainPageCS(eu, list_amigos, new PedidosRecebidos(eu));
			};
			this.btn_config.Clicked += async (sender, e) =>
			{
				await DisplayAlert("Alerta", "Essa funcao ainda nao foi habilitada", "Ok");
			};
		}
	}
}
