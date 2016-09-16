
using SQLite.Net.Interop;

namespace Inbanker
{

	public interface IConfig
	{
		string DiretorioDB { get; }
		ISQLitePlatform Plataforma { get; }
	}

}


