using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Inbanker
{
	public partial class PedidosFeitos : ContentPage
	{
		public PedidosFeitos()
		{
			InitializeComponent();

			Title = "Pedidos Enviados";

			//pegamos os dados do usuario logado que esta no msqlite
			AcessoDadosUsuario dados = new AcessoDadosUsuario();
			var eu = dados.ObterUsuario();

			VerificaListaTransacoes(eu);

		}

		public async void VerificaListaTransacoes(Usuario eu) { 
			ServiceWrapper service = new ServiceWrapper();
			var result = await service.ListaPedidosFeitos(eu.id_usuario);
			//carregando.Text = result;

			if (result.Equals("menor")){
				carregando.Text = "Voce ainda nao fez nenhuma pedido de emprestimo.";
			}else { 
				
				stack_load.IsVisible = false;
				stack_lista.IsVisible = true;

				var trans = JsonConvert.DeserializeObject<List<Transacao>>(result);
				//carregando.Text = result;
				ListarTransacoes(trans);
			}

		}

		public void ListarTransacoes(List<Transacao> lista_trans) { 
			
			//busca.TextChanged += Busca_TextChanged;

			lista.ItemsSource = lista_trans;

			lista.ItemTapped += async (sender, args) =>
			{
				var item = args.Item as Transacao;
				if (item == null)
					return;

				await Navigation.PushAsync(new VerPedidosEnviados(item));
			};

		}
	}
}

