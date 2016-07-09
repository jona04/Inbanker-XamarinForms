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

			_conexao.CreateTable<Amigos>();
		}

		public void InsertAmigos(Amigos amigos)
		{
			_conexao.Insert(amigos);
		}

		//usado sempre antes de adicoonar a lista no login, para que nao se tenha usuarios repitidos
		public void DeleteAmigos()
		{
			//_conexao.Delete(usu);
			_conexao.Query<Amigos>("DELETE FROM [Amigos]");
		}

		//public Amigos ObterAmigos()
		//{
		//	return _conexao.Table<Amigos>().FirstOrDefault();
		//}

		public void UpdateAmigos(Amigos amigos)
		{
			_conexao.Update(amigos);
		}

		public List<Amigos> Listar()
		{
			return _conexao.Table<Amigos>().OrderBy(c => c.name).ToList();
		}

		//usado para fechar conexao do sqlite
		public void Dispose()
		{
			_conexao.Dispose();
		}
	}
}

