using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Inbanker
{
	public partial class VerPedidosHistorico : ContentPage
	{
		public VerPedidosHistorico(Transacao trans)
		{
			InitializeComponent();

			Title = "Pedido Historico";

			nome_usuario.Text = trans.trans_nome_user2;

			DateTime dayNow = DateTime.Now;
			DateTime oDate = DateTime.Parse(trans.trans_data_finalizada);
			var dias = (dayNow - oDate).Days;

			//fomula para calcular o valor total a ser pago, ao mesmo tempo que arredondamos o resultado final para 2 casas decimais depois da virgula
			double capital = double.Parse(trans.trans_valor);

			double juros_mensal = Math.Round(capital * (0.00066333 * dias), 2);
			string juros_mensal_string = String.Format("{0:0.00}", juros_mensal);

			double total = juros_mensal + capital;
			string total_string = String.Format("{0:0.00}", total);

			string valor = String.Format("{0:0.00}", Double.Parse(trans.trans_valor));

			valor_solicitado.Text = "R$ " + valor;
			data_finalizado.Text = trans.trans_data_finalizada;
			valor_juros.Text = "R$ " + juros_mensal_string;
			valor_total_pago.Text = "R$ " + total_string;


			//manipulamos o xmal de acordo com a resposta dada ao pedido
			switch (int.Parse(trans.trans_resposta_pedido))
			{
				case (1): // o usuario 2 recusou o pedido de emprestimo
					msg.Text = "Seu amigo " + trans.trans_nome_user2 + " recusou seu pedido de emprestimo.";
					break;
			}


		}
	}
}

