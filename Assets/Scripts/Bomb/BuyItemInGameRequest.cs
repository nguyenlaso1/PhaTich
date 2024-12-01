// @sonhg: class: Bomb.BuyItemInGameRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

namespace Bomb
{
	public class BuyItemInGameRequest : global::BaseRequest
	{
		private BuyItemInGameRequest(int id)
		{
			this.isShowWaiting = false;
			this.id = id;
		}

		protected override IRequest Request
		{
			get
			{
				SFSObject sfsobject = new SFSObject();
				sfsobject.PutUtfString("task", "buy-item-ingame");
				sfsobject.PutInt("id", this.id);
				return new ExtensionRequest("c-room-command", sfsobject);
			}
		}

		public static void SendMessage(int id)
		{
			new BuyItemInGameRequest(id).Send();
		}

		private int id;
	}
}
