using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Inbanker
{
	public partial class VerPedidosEnviados : ContentPage
	{
		
		double juros_mensal;

		public VerPedidosEnviados(Transacao trans)
		{
			InitializeComponent();

			Title = "Pedidos Enviados";

			nome_usuario.Text = trans.trans_nome_user2;

			DateTime dayNow = DateTime.Now;
			DateTime oDate = DateTime.Parse(trans.trans_data_pedido);
			var dias = (dayNow - oDate).Days;

			//fomula para calcular o valor total a ser pago, ao mesmo tempo que arredondamos o resultado final para 2 casas decimais depois da virgula
			double capital = double.Parse(trans.trans_valor);

			if (trans.trans_resposta_pedido == "0")
				juros_mensal = Math.Round(capital * (0.00066333 * trans.trans_dias), 2);
			else
				juros_mensal = Math.Round(capital * (0.00066333 * dias), 2);


			string juros_mensal_string = String.Format("{0:0.00}", juros_mensal);

			double total = juros_mensal + capital;
			string total_string = String.Format("{0:0.00}", total);

			string valor = String.Format("{0:0.00}", Double.Parse(trans.trans_valor));

			dias_pagamento.Text = trans.trans_vencimento;

			valor_solicitado.Text = "R$ "+valor;
			dias_corrido.Text = dias.ToString();
			valor_juros.Text = "R$ "+juros_mensal_string;
			valor_total_pago.Text = "R$ "+total_string;


			//manipulamos o xmal de acordo com a resposta dada ao pedido
			switch (int.Parse(trans.trans_resposta_pedido))
			{
				case (0): //usuario 2 ainda nao respondeu se aceita ou nao o pedido de emprestimo
					msg.Text = "Voce esta aguardando seu amigo(a) "+ trans.trans_nome_user2 +" aceitar ou recusar seu pedido.";
					break;
				//case (1): // o usuario 2 recusou o pedido de emprestimo
				//	msg.Text = "Seu amigo "+ trans.trans_nome_user2 +" recusou seu pedido de emprestimo.";
				//	break;
				case (2): // o usuario 2 aceitou o pedido de emprestimo

					stack_data_pagamento.IsVisible = false;
					stack_dias_corridos.IsVisible = true;

					if (trans.trans_recebimento_empre.Equals("0")) //usuario 1 ainda nao recebeu o valor do emprestimo
					{
						info_pedido.IsVisible = false;
						stack_confirm_receb.IsVisible = true;
						msg.Text = "Seu amigo "+trans.trans_nome_user2+" aceitou seu pedido de emprestimo.";
						msg_confirm_recb.Text = "Confirme o recebimento do valor solicitado.";
					}

					//aqui ainda tera outra oopcao informando que o usuario cancelou o emprestimo

					break;
			}

			btn_sim.Clicked += async (sender, e) =>
			{
				if (senha2.Text == "admin")
				{


					//fazemos cadastro do 
					ServiceWrapper serviceWrapper = new ServiceWrapper();
					var result = await serviceWrapper.ConfirmaRecebimentoEmprestimo(1, trans.trans_id);
					//string[]colunas = result.Split(',');
					lblNome.Text = "resultado " + result;

					if (result.Equals("ok"))
					{
						stack_confirm_receb.IsVisible = false;

						await DisplayAlert("InBanker","Parabéns, voce confirmou o recebimento do valor solicitado. Ao efetuar o pagamento de quitaçao, peça que seu amigo(a) " + trans.trans_nome_user2 + " confirme o recebimento do valor.", "Ok");

						App.Current.MainPage = new MainPageCS(new InicioPage());
					}

				}
				else {
					await DisplayAlert("Alerta", "Por favor informe sua senha correta", "Ok");
				}

			};

			btn_nao.Clicked += async (sender, e) =>
			{
				if (senha2.Text == "admin")
				{


					await DisplayAlert("InBanker", "Voce cancelou esse pedido de emprestimo com o seu amigo(a) " + trans.trans_nome_user2 + ".", "Ok");
					App.Current.MainPage = new MainPageCS(new InicioPage());
					/*
					//fazemos cadastro do 
					ServiceWrapper serviceWrapper = new ServiceWrapper();
					var result = await serviceWrapper.ConfirmaRecebimentoEmprestimo(1, trans.trans_id);
					//string[]colunas = result.Split(',');
					lblNome.Text = "resultado " + result;

					if (result.Equals("ok"))
					{

						//depois arrumar essa notificaçao
						//var result2 = await serviceWrapper.EnviarNotificacaoConfirmRecebimento(trans);


						await DisplayAlert("InBanker", "Voce cancelou esse pedido de emprestimo com o seu amigo(a) " + trans.trans_nome_user2 + ".", "Ok");
						App.Current.MainPage = new MainPageCS(new InicioPage());
					}
					*/

				}
				else {
					await DisplayAlert("Alerta", "Por favor informe sua senha correta", "Ok");
				}

			};

		}
	}
}

