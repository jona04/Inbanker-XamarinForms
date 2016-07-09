using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Inbanker
{
	public class AcessoDadosAmigos : IDisposable
	{
		private SQLite.Net.SQLiteConnection _conexao;

		public AcessoDadosAmigos()
		{
			var config = DependencyService.Get<IConfig>();//utiliza dependency para verificar qual plataforma esta sendo utilizada
			_conexao = new SQLite.Net.SQLiteConnection(config.Plataforma, System.IO.Path.Combine(config.DiretorioDB, "bancodados.db3"));

			_conexao.CreateTable<AmigosSQLite>();
		}

		public void InsertAmigos(AmigosSQLite amigos)
		{
			_conexao.Insert(amigos);
		}

		//public void DeleteAmigos(string id_usu)
		//{
		//	//_conexao.Delete(usu);
		//	_conexao.Query<Amigos>("DELETE FROM [Usuario] WHERE [id_usuario] = " + id_usu);
		//}

		//public Amigos ObterAmigos()
		//{
		//	return _conexao.Table<Amigos>().FirstOrDefault();
		//}

		public void UpdateAmigos(AmigosSQLite amigos)
		{
			_conexao.Update(amigos);
		}

		public List<AmigosSQLite> Listar()
		{
			return _conexao.Table<AmigosSQLite>().OrderBy(c => c.name).ToList();
		}

		//usado para fechar conexao do sqlite
		public void Dispose()
		{
			_conexao.Dispose();
		}
	}
}

