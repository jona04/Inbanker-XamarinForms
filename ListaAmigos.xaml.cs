using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Inbanker
{
	public partial class ListaAmigos : ContentPage
	{

		private List<Amigos> list_usu;

		public ListaAmigos()
		{
			InitializeComponent();

			Title = "Lista Amigos";

			//pegamos os dados do usuario logado que esta no msqlite
			AcessoDadosUsuario dados = new AcessoDadosUsuario();
			var eu = dados.ObterUsuario();

			//para listar os amigos que estao armazenados no sqlite
			AcessoDadosAmigos dadosAmigos = new AcessoDadosAmigos();
			var list_amigos = dadosAmigos.Listar();

			if (list_amigos == null)
			{
				DisplayAlert("Alerta", "Lista Nula", "Ok");
			}
			else {

				list_usu = list_amigos;

				this.busca.TextChanged += Busca_TextChanged;
				this.lista.ItemsSource = list_usu;

				this.lista.ItemTapped += async (sender, args) =>
				{
					var item = args.Item as Amigos;
					if (item == null)
						return;

					await Navigation.PushAsync(new SimuladorPage(eu.id_usuario, eu.nome_usuario, item.id, item.name,item.url_picture));
				};

			}

		}

		public IEnumerable<Amigos> Listar(string filtro = "")
		{

			IEnumerable<Amigos> livrosFiltrados = this.list_usu;
			if (!string.IsNullOrEmpty(filtro))
				livrosFiltrados = this.list_usu.Where(l => (l.name.ToLower().Contains(filtro.ToLower())));

			return livrosFiltrados;
		}

		void Busca_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.lista.ItemsSource = this.Listar(this.busca.Text);
		}

	}
}

