using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Inbanker
{
	public partial class VerPedidosParaPagar : ContentPage
	{
		public VerPedidosParaPagar(Transacao trans)
		{
			InitializeComponent();

			Title = "Pedido para Pagar";

			nome_usuario.Text = trans.trans_nome_user2;

			DateTime dayNow = DateTime.Now;
			DateTime oDate = DateTime.Parse(trans.trans_data_pedido);
			var dias = (dayNow - oDate).Days;

			//fomula para calcular o valor total a ser pago, ao mesmo tempo que arredondamos o resultado final para 2 casas decimais depois da virgula
			double capital = double.Parse(trans.trans_valor);

			double juros_mensal = Math.Round(capital * (0.00066333 * dias), 2);
			string juros_mensal_string = String.Format("{0:0.00}", juros_mensal);

			double total = juros_mensal + capital;
			string total_string = String.Format("{0:0.00}", total);

			string valor = String.Format("{0:0.00}", Double.Parse(trans.trans_valor));

			valor_solicitado.Text = "R$ " + valor;
			dias_corrido.Text = dias.ToString();
			valor_juros.Text = "R$ " + juros_mensal_string;
			valor_total_pago.Text = "R$ " + total_string;


			if (trans.trans_resposta_pagamento.Equals("0")) //usuario 1 ainda esta para solicitar quitacao de pagamento
			{
				//stack_confirm_receb.IsVisible = false;
				stack_solicitar_pag.IsVisible = true;
				msg.Text = "Solicite a quitaçao do valor pedido em emprestimo.";
			}
			if (trans.trans_resposta_pagamento.Equals("1")) //usuario 1 solicitou quitacao de pagamento e esta no aguardo
			{
				//stack_confirm_receb.IsVisible = false;
				stack_solicitar_pag.IsVisible = false;
				//msg.Text = "Voce realizou uma solicitaçao de quitaçao do valor pedido para emprestimo. Aguarde a resposta do " + trans.trans_nome_user2;
				msg.Text = "Parabéns, voce realizou a solicitação de quitação do emprestimo. Peça que seu amigo(a) " + trans.trans_nome_user2 + " confirme o recebimento do valor.";
			}
			if (trans.trans_resposta_pagamento.Equals("2")) //usuario 2 resposdeu negativamente a quitaçao do valor emprestado
			{
				//stack_confirm_receb.IsVisible = false;
				stack_solicitar_pag.IsVisible = true;
				msg.Text = "Seu pedido de quitaçao foi negado, solicite a quitaçao do valor pedido em emprestimo novamente.";
			}


			btn_solicitar_pags.Clicked += async (sender, e) =>
			{
				if (senha.Text == "admin")
				{

					//	//await DisplayAlert ("Inbanker", "Pedido foi enviado para "+nome,"Ok");
					ServiceWrapper serviceWrapper = new ServiceWrapper();
					var result2 = await serviceWrapper.SolicitarPagamentoEmprestimo(trans.trans_id, 1);
					lblNome.Text = "get call says: " + result2;

					if (result2.Equals("ok"))
					{
						//stack_confirm_receb.IsVisible = false;
						//stack_solicitar_pag.IsVisible = false;
						//msg.Text = "Voce realizou uma solicitaçao de quitaçao do valor pedido para emprestimo. Aguarde a resposta do "+ trans.trans_nome_user2;

						var result = await serviceWrapper.EnviarNotificacaoConfirmPagamento(trans);
						//lblNome2.Text = "get call says: " + result;

						await DisplayAlert("InBanker", "Parabéns, voce realizou a solicitação de quitação do emprestimo. Peça que seu amigo(a) " + trans.trans_nome_user2 + " confirme o recebimento do valor.", "Ok");

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

