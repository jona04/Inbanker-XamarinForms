using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Inbanker
{
	public partial class VerPedidoRecebido : ContentPage
	{

		Transacao trans;

		double juros_mensal;

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

			DateTime dayNow = DateTime.Now;
			DateTime oDate = DateTime.Parse(trans.trans_data_pedido);
			var dias = (dayNow - oDate).Days;

			//fomula para calcular o valor total a ser pago, ao mesmo tempo que arredondamos o resultado final para 2 casas decimais depois da virgula
			double capital = double.Parse(trans.trans_valor);

			if (trans.trans_resposta_pedido == "0")
				juros_mensal = Math.Round(capital * (0.00066333 * trans.trans_dias), 2);
			else
				juros_mensal = Math.Round(capital * (0.00066333 * dias), 2);


			string juros_mensal2 = String.Format("{0:0.00}", juros_mensal);
			valor_rendimento.Text = "R$ " + juros_mensal2;

			double total = juros_mensal + capital;
			string valor_total = String.Format("{0:0.00}", total);

			string valor = String.Format("{0:0.00}", Double.Parse(trans.trans_valor));

			valor_solicitado.Text = "R$ "+valor;
			dias_pagamento.Text = trans.trans_vencimento;
			dias_corridos.Text = dias.ToString();

			valor_total_pago.Text = "R$ "+valor_total;


			//manipulamos o xmal de acordo com a resposta dada ao pedido
			switch (int.Parse(trans.trans_resposta_pedido)) {
				//case (0):
					//stack_btn_acc_pedido.IsVisible = false;
					//break;
				case (1): //recusou pedido de emprestimo
					stack_btn_acc_pedido.IsVisible = false;
					stack_msg_pedido.IsVisible = true;
					msg_pedido.Text = "Voce recusou esse pedido de emprestimo.";

					break;
				case (2): //aceitou pedido de emprestimo
					stack_btn_acc_pedido.IsVisible = false;
					stack_msg_pedido.IsVisible = true;


					//aqui tera a opcao para o caso de o osuario tiver cancelado o pedido de emprestimo


					if (trans.trans_resposta_pagamento.Equals("0")) // //usuario 1 ainda esta para solicitar quitacao de pagamento
					{

						stack_data_pagamento.IsVisible = false;
						stack_dias_corridos.IsVisible = true;

						msg_pedido.Text =  "Voce esta aguardando que seu amigo(a) " + trans.trans_nome_user1 + " faça a quitaçao do emprestimo.";

					}
					if (trans.trans_resposta_pagamento.Equals("1")) //usuario 1 solicitou quitacao de pagamento e esta no aguardo
					{

						stack_data_pagamento.IsVisible = false;
						stack_dias_corridos.IsVisible = true;

						stack_msg_acc_pagamento.IsVisible = true;
						stack_btn_acc_pagamento.IsVisible = true;
						msg_acc_pagemento.Text = "Seu amigo(a) "+ trans.trans_nome_user1 +" esta solicitando que voce confirme a quitaçao do valor que ele solicitou em emprestimo.";

					}
					if (trans.trans_resposta_pagamento.Equals("2")) //usuario 2 resposdeu negativamente a quitaçao do valor emprestado
					{

						stack_btn_acc_pagamento.IsVisible = false;
						msg_pedido.Text = "Voce recusou uma solicitaçao de pagamento. Agora aguarde uma nova solicitaçao de pagamento.";

					}
					if (trans.trans_resposta_pagamento.Equals("3")) //usuario 2 resposdeu positivamente a quitacao do valor emprestado
					{

						//aqui nao tera mais os dias corridos contando, pois essa transacao ja deve esta no historico

						stack_data_pagamento.IsVisible = false;
						stack_dias_corridos.IsVisible = true;

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

						//await DisplayAlert("InBanker", "Voce aceitou o pedido. Voce sera informado quando seu amigo(a) " + trans.trans_nome_user1 + " solicitar a quitaçao da divida.", "Ok");
						await DisplayAlert("InBanker", "Parabéns, voce aceitou o pedido. Ao efetuar o pagamento, peça que seu amigo(a) " + trans.trans_nome_user1 + " confirme o recebimento do valor.", "Ok");


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

