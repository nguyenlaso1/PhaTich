// @sonhg: class: Bomb.PickItemRequest
using System;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

namespace Bomb
{
	public class PickItemRequest : global::BaseRequest
	{
		public PickItemRequest(int x, int y, int sfsId = -1)
		{
			this._x = x;
			this._y = y;
			this.sfsId = sfsId;
			this.isShowWaiting = false;
		}

		protected override IRequest Request
		{
			get
			{
				SFSObject sfsobject = new SFSObject();
				sfsobject.PutUtfString("task", "pickup");
				sfsobject.PutInt("x", this._x);
				sfsobject.PutInt("y", this._y);
				sfsobject.PutInt("sfsId", this.sfsId);
				return new ExtensionRequest("c-room-command", sfsobject);
			}
		}

		public static void SendMessage(int x, int y, int sfsId = -1)
		{
			new PickItemRequest(x, y, sfsId).Send();
		}

		private int _x;

		private int _y;

		private int sfsId;
	}
}
