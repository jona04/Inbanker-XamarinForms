using System;
namespace Inbanker
{
	public interface INetworkConnection
	{
		bool IsConnected { get; }
		void CheckNetworkConnection();
	}
}

