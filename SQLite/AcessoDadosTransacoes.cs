using System;
namespace Inbanker
{
	public class AcessoDadosTransacoes : IDisposable
	{
		private SQLite.Net.SQLiteConnection _conexao;

		public AcessoDadosTransacoes()
		{
		}

		//fecha conexao SqlLite
		public void Dispose()
		{
			_conexao.Dispose();
		}
	}
}

