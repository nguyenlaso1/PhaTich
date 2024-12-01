// @sonhg: class: Bomb.BombUserReadyRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

namespace Bomb
{
	public class BombUserReadyRequest : global::BaseRequest
	{
		private BombUserReadyRequest()
		{
			this.isShowWaiting = false;
		}

		protected override IRequest Request
		{
			get
			{
				SFSObject parameters = new SFSObject();
				return new ExtensionRequest("mmo-ready", parameters);
			}
		}

		public static void SendMessage()
		{
			new BombUserReadyRequest().Send();
		}
	}
}
