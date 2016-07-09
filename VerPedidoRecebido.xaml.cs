using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Inbanker
{
	public partial class VerPedidoRecebido : ContentPage
	{

		Transacao trans;

		public VerPedidoRecebido(Transacao trans)
		{
			InitializeComponent();

			Title = "Pedidos Recebidos";

			//pegamos os dados do usuario logado que esta no msqlite
			AcessoDadosUsuario dados = new AcessoDadosUsuario();
			var eu = dados.ObterUsuario();

			AcessoDadosAmigos dadosAmigos = new AcessoDadosAmigos();
			var list_amigos = dadosAmigos.Listar();

			nome_usuario.Text = trans.trans_nome_user1;

			//fomula para calcular o valor total a ser pago, ao mesmo tempo que arredondamos o resultado final para 2 casas decimais depois da virgula
			double capital = double.Parse(trans.trans_valor);

			double juros_mensal = Math.Round(capital * (0.00066333 * trans.trans_dias), 2);
			//double taxa_fixa = Math.Round(capital * 0.0099, 2, MidpointRounding.ToEven);

			double total = juros_mensal + capital;

			string juros_mensal2 = String.Format("{0:0.00}", juros_mensal);
			//string taxa_fixa2 = String.Format("{0:0.00}", taxa_fixa);

			string valor = String.Format("{0:0.00}", Double.Parse(trans.trans_valor));

			valor_rendimento.Text = "R$ "+juros_mensal2;
			//valor_taxa_fixa.Text = "R$ " + taxa_fixa2;

			valor_solicitado.Text = "R$ "+valor;
			//data_vencimento.Text = trans.trans_vencimento;
			dias_pagamento.Text = trans.trans_vencimento;
			valor_total_pago.Text = "R$ "+total;


			//manipulamos o xmal de acordo com a resposta dada ao pedido
			switch (int.Parse(trans.trans_resposta_pedido)) {
				//case (0):
					//stack_btn_acc_pedido.IsVisible = false;
					//break;
				case (1):
					stack_btn_acc_pedido.IsVisible = false;
					stack_msg_pedido.IsVisible = true;
					msg_pedido.Text = "Voce recusou esse pedido de emprestimo.";

					break;
				case (2):
					stack_btn_acc_pedido.IsVisible = false;
					stack_msg_pedido.IsVisible = true;

					if (trans.trans_resposta_pagamento.Equals("0")) // se ainda nao houve solicitacao de pagamento
					{
						
						msg_pedido.Text =  "Voce aceitou o pedido. Voce sera informado quando seu amigo(a) " + trans.trans_nome_user1 + " solicitar a quitaçao da divida.";

					}
					if (trans.trans_resposta_pagamento.Equals("1")) // o usuario devedor esta solicitando confirmaçao de recebimento de pagamento
					{
						stack_btn_acc_pagamento.IsVisible = true;
						msg_acc_pagemento.Text = "Seu amigo(a) "+ trans.trans_nome_user1 +" esta solicitando que voce confirme a quitaçao do valor pedido em emprestimo.";

					}
					if (trans.trans_resposta_pagamento.Equals("2")) // o usuario que emprestou recusou o recebimento do pagamento
					{
						stack_btn_acc_pagamento.IsVisible = false;
						msg_pedido.Text = "O pedido foi aceito, e voce recusou uma solicitaçao de pagamento. Agora aguarde uma nova solicitaçao de pagamento.";

					}
					if (trans.trans_resposta_pagamento.Equals("3")) // o usuario esta solicitando confirmaçao de pagamento
					{
						stack_btn_acc_pagamento.IsVisible = false;
						msg_pedido.Text = "Voce confirmou o recebimento do valor para quitaçao do emprestimo solicitado por " + trans.trans_nome_user1 + ".Parabens, essa transacao foi finalizada.";

					}
					break;
			}
				
			

			btn_rejeitar_pedido.Clicked += async (sender, e) =>
			{
				if (senha.Text == "admin")
				{


					//fazemos cadastro do 
					ServiceWrapper serviceWrapper = new ServiceWrapper();
					var result = await serviceWrapper.RespostaPedidoUsuario(1, trans.trans_id);
					//string[]colunas = result.Split(',');
					lblNome.Text = "resultado " + result;

					if (result.Equals("ok"))
					{

						//stack_btn_acc_pedido.IsVisible = false;
						//stack_msg_pedido.IsVisible = true;
						//msg_pedido.Text = "Voce recusou esse pedido de emprestimo.";

						var result2 = await serviceWrapper.EnviarNotificacaoRespostaUsuario(trans.trans_id,trans.trans_id_user1, 1);
						//lblNome2.Text = "get call says: " + result2;

						await DisplayAlert("InBanker", "Voce recusou esse pedido de emprestimo de " + trans.trans_nome_user1, "Ok");

						App.Current.MainPage = new MainPageCS(new InicioPage());

					}


				}
				else {
					await DisplayAlert("Alerta", "Por favor informe sua senha correta", "Ok");
				}

			};

			btn_aceitar_pedido.Clicked += async (sender, e) =>
			{
				if (senha.Text == "admin")
				{


					//fazemos cadastro do 
					ServiceWrapper serviceWrapper = new ServiceWrapper();
					var result = await serviceWrapper.RespostaPedidoUsuario(2, trans.trans_id);
					//string[]colunas = result.Split(',');
					lblNome.Text = "resultado " + result;

					if (result.Equals("ok"))
					{

						//stack_btn_acc_pedido.IsVisible = false;
						//stack_msg_pedido.IsVisible = true;
						//msg_pedido.Text = "Voce aceitou o pedido. Voce sera informado quando seu amigo " + trans.trans_nome_user1 + " solicitar a quitaçao da divida.";

						var result2 = await serviceWrapper.EnviarNotificacaoRespostaUsuario(trans.trans_id, trans.trans_id_user1, 2);
						//lblNome2.Text = "get call says: " + result2;

						await DisplayAlert("InBanker", "Voce aceitou o pedido. Voce sera informado quando seu amigo(a) " + trans.trans_nome_user1 + " solicitar a quitaçao da divida.", "Ok");

						App.Current.MainPage = new MainPageCS(new InicioPage());

					}


				}
				else {
					await DisplayAlert("Alerta", "Por favor informe sua senha correta", "Ok");
				}

			};


			btn_confirmar_pagamento.Clicked += async (sender, e) =>
			{
				if (senha.Text == "admin")
				{


					//fazemos cadastro do 
					ServiceWrapper serviceWrapper = new ServiceWrapper();
					var result = await serviceWrapper.RespostaConfirmPagamento(trans.trans_id,3);
					//string[]colunas = result.Split(',');
					lblNome.Text = "resultado " + result;

					if (result.Equals("ok"))
					{

						//stack_btn_acc_pagamento.IsVisible = false;
						//msg_pedido.Text = "O pedido foi aceito. Voce fez o emprestimo. Voce ja confirmou o recebimento do pagamento.";

						//await DisplayAlert ("Inbanker", "Pedido foi enviado para "+nome,"Ok");

						var result2 = await serviceWrapper.EnviarNotificacaoRespostaConfirmPagamento(trans, 3);
						//lblNome2.Text = "get call says: " + result2;

						await DisplayAlert("InBanker", "Voce confirmou o recebimento do valor para quitaçao do emprestimo solicitado por " + trans.trans_nome_user1 + ". Parabens, essa transacao foi finalizada.", "Ok");

						App.Current.MainPage = new MainPageCS(new InicioPage());

					}


				}
				else {
					await DisplayAlert("Alerta", "Por favor informe sua senha correta", "Ok");
				}

			};

			btn_recusar_pagamento.Clicked += async (sender, e) =>
			{
				if (senha.Text == "admin")
				{


					//fazemos cadastro do 
					ServiceWrapper serviceWrapper = new ServiceWrapper();
					var result = await serviceWrapper.RespostaConfirmPagamento(trans.trans_id,2);
					//string[]colunas = result.Split(',');
					lblNome.Text = "resultado " + result;

					if (result.Equals("ok"))
					{

						//stack_btn_acc_pagamento.IsVisible = false;
						//msg_pedido.Text = "Voce recusou uma solicitaçao de pagamento feito pelo seu amigo(a) "+ trans.trans_nome_user1 +". Agora aguarde uma nova solicitaçao de pagamento.";

						//await DisplayAlert ("Inbanker", "Pedido foi enviado para "+nome,"Ok");

						var result2 = await serviceWrapper.EnviarNotificacaoRespostaConfirmPagamento(trans, 2);
						//lblNome2.Text = "get call says: " + result2;

						await DisplayAlert("InBanker", "Voce recusou uma solicitaçao de pagamento feito pelo seu amigo(a) " + trans.trans_nome_user1 + ". Agora aguarde uma nova solicitaçao de pagamento.", "Ok");

						App.Current.MainPage = new MainPageCS(new InicioPage());

					}


				}
				else {
					await DisplayAlert("Alerta", "Por favor informe sua senha correta", "Ok");
				}

			};
			
		}
	}
}

