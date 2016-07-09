using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Inbanker
{
	public class AcessoDadosUsuario : IDisposable
	{
		private SQLite.Net.SQLiteConnection _conexao;

		public AcessoDadosUsuario()
		{
			var config = DependencyService.Get<IConfig>();//utiliza dependency para verificar qual plataforma esta sendo utilizada
			_conexao = new SQLite.Net.SQLiteConnection(config.Plataforma, System.IO.Path.Combine(config.DiretorioDB, "bancodados.db3"));

			_conexao.CreateTable<Usuario>();
		}

		public void InsertUsuario(Usuario usu)
		{
			_conexao.Insert(usu);
		}

		public void DeleteUsuario(string id_usu)
		{
			//_conexao.Delete(usu);
			_conexao.Query<Usuario>("DELETE FROM [Usuario] WHERE [id_usuario] = " + id_usu);
		}

		public Usuario ObterUsuario()
		{
			return _conexao.Table<Usuario>().FirstOrDefault();
		}

		public void UpdateUsuario(Usuario usu)
		{
			_conexao.Update(usu);
		}

		//public List<Usuario> Listar()
		//{
		//	return _conexao.Table<Usuario>().OrderBy(c => c.nome_usuario).ToList();
		//}

		//usado para fechar conexao do sqlite
		public void Dispose()
		{
			_conexao.Dispose();
		}
	}
}

