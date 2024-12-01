// @sonhg: class: GooglePlayGames.Native.UnsupportedAppStateClient
using System;
using GooglePlayGames.BasicApi;
//using GooglePlayGames.OurUtils;

namespace GooglePlayGames.Native
{
	internal class UnsupportedAppStateClient : AppStateClient
	{
		internal UnsupportedAppStateClient(string message)
		{
			//this.mMessage = Misc.CheckNotNull<string>(message);
		}

		public void LoadState(int slot, OnStateLoadedListener listener)
		{
			throw new NotImplementedException(this.mMessage);
		}

		public void UpdateState(int slot, byte[] data, OnStateLoadedListener listener)
		{
			throw new NotImplementedException(this.mMessage);
		}

		private readonly string mMessage;
	}
}
