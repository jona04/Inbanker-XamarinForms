using SQLite.Net.Attributes;

namespace Inbanker
{

	public class Amigos
	{
		public string id { get; set; }
		public string name { get; set; }
		public string url_picture { get; set;}

		[Ignore]
		public Picture picture { get; set; }

	}

	public class Picture
	{
		public Data data { get; set; }
	}

	public class Data
	{
		public string url { get; set; }
	}

}


